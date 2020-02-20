//#load ".fake/build.fsx/intellisense.fsx"
#load "docsTool/CLI.fs"
#if !FAKE
#r "netstandard"
#endif
open System
open Fake.SystemHelper
open Fake.Core
open Fake.DotNet
open Fake.Tools
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.Api
open Fake.BuildServer

BuildServer.install [
    AppVeyor.Installer
    Travis.Installer
]

let environVarAsBoolOrDefault varName defaultValue =
    let truthyConsts = [
        "1"
        "Y"
        "YES"
        "T"
        "TRUE"
    ]
    try
        let envvar = (Environment.environVar varName).ToUpper()
        truthyConsts |> List.exists((=)envvar)
    with
    | _ ->  defaultValue

//-----------------------------------------------------------------------------
// Metadata and Configuration
//-----------------------------------------------------------------------------

let productName = "canopy"
let sln = "canopy.sln"


let srcCodeGlob =
    !! (__SOURCE_DIRECTORY__  @@ "src/**/*.fs")
    ++ (__SOURCE_DIRECTORY__  @@ "src/**/*.fsx")

let testsCodeGlob =
    !! (__SOURCE_DIRECTORY__  @@ "tests/**/*.fs")
    ++ (__SOURCE_DIRECTORY__  @@ "tests/**/*.fsx")

let srcGlob =__SOURCE_DIRECTORY__  @@ "src/**/*.??proj"
let testsGlob = __SOURCE_DIRECTORY__  @@ "tests/**/*.??proj"

let srcAndTest =
    !! srcGlob
    ++ testsGlob

let distDir = __SOURCE_DIRECTORY__  @@ "dist"
let distGlob = distDir @@ "*.nupkg"

let coverageThresholdPercent = 80
let coverageReportDir =  __SOURCE_DIRECTORY__  @@ "docs" @@ "coverage"


let docsDir = __SOURCE_DIRECTORY__  @@ "docs"
let docsSrcDir = __SOURCE_DIRECTORY__  @@ "docsSrc"
let docsToolDir = __SOURCE_DIRECTORY__ @@ "docsTool"

let gitOwner = "JeremyBellows"
let gitRepoName = "canopy"

let gitHubRepoUrl = sprintf "https://github.com/%s/%s" gitOwner gitRepoName

let releaseBranch = "master"

let tagFromVersionNumber versionNumber = sprintf "v%s" versionNumber

let changelogFilename = "CHANGELOG.md"
let changelog = Fake.Core.Changelog.load changelogFilename
let mutable latestEntry =
    if Seq.isEmpty changelog.Entries
    then Changelog.ChangelogEntry.New("0.0.1", "0.0.1-alpha.1", Some DateTime.Today, None, [], false)
    else changelog.LatestEntry
let mutable linkReferenceForLatestEntry = ""
let mutable changelogBackupFilename = ""

let publishUrl = "https://www.nuget.org"

let docsSiteBaseUrl = sprintf "https://%s.github.io/%s" gitOwner gitRepoName

let disableCodeCoverage = environVarAsBoolOrDefault "DISABLE_COVERAGE" false

//-----------------------------------------------------------------------------
// Helpers
//-----------------------------------------------------------------------------

let isRelease (targets : Target list) =
    targets
    |> Seq.map(fun t -> t.Name)
    |> Seq.exists ((=)"Release")

let invokeAsync f = async { f () }

let configuration (targets : Target list) =
    let defaultVal = if isRelease targets then "Release" else "Debug"
    match Environment.environVarOrDefault "CONFIGURATION" defaultVal with
    | "Debug" -> DotNet.BuildConfiguration.Debug
    | "Release" -> DotNet.BuildConfiguration.Release
    | config -> DotNet.BuildConfiguration.Custom config

let failOnBadExitAndPrint (p : ProcessResult) =
    if p.ExitCode <> 0 then
        p.Errors |> Seq.iter Trace.traceError
        failwithf "failed with exitcode %d" p.ExitCode

// CI Servers can have bizzare failures that have nothing to do with your code
let rec retryIfInCI times fn =
    match Environment.environVarOrNone "CI" with
    | Some _ ->
        if times > 1 then
            try
                fn()
            with
            | _ -> retryIfInCI (times - 1) fn
        else
            fn()
    | _ -> fn()

let isReleaseBranchCheck () =
    if Git.Information.getBranchName "" <> releaseBranch then failwithf "Not on %s.  If you want to release please switch to this branch." releaseBranch

let isEmptyChange = function
    | Changelog.Change.Added s
    | Changelog.Change.Changed s
    | Changelog.Change.Deprecated s
    | Changelog.Change.Fixed s
    | Changelog.Change.Removed s
    | Changelog.Change.Security s
    | Changelog.Change.Custom (_, s) ->
        String.IsNullOrWhiteSpace s.CleanedText

let mkLinkReference (newVersion : SemVerInfo) (changelog : Changelog.Changelog) =
    if changelog.Entries |> List.isEmpty then
        // No actual changelog entries yet: link reference will just point to the Git tag
        sprintf "[%s]: %s/releases/tag/%s" newVersion.AsString gitHubRepoUrl (tagFromVersionNumber newVersion.AsString)
    else
        let versionTuple version = (version.Major, version.Minor, version.Patch)
        // Changelog entries come already sorted, most-recent first, by the Changelog module
        let prevEntry = changelog.Entries |> List.skipWhile (fun entry -> entry.SemVer.PreRelease.IsSome && versionTuple entry.SemVer = versionTuple newVersion) |> List.tryHead
        let linkTarget =
            match prevEntry with
            | Some entry -> sprintf "%s/compare/%s...%s" gitHubRepoUrl (tagFromVersionNumber entry.SemVer.AsString) (tagFromVersionNumber newVersion.AsString)
            | None -> sprintf "%s/releases/tag/%s" gitHubRepoUrl (tagFromVersionNumber newVersion.AsString)
        sprintf "[%s]: %s" newVersion.AsString linkTarget

let mkReleaseNotes (linkReference : string) (latestEntry : Changelog.ChangelogEntry) =
    if String.isNullOrEmpty linkReference then latestEntry.ToString()
    else
        // Add link reference target to description before building release notes, since in main changelog file it's at the bottom of the file
        let description =
            match latestEntry.Description with
            | None -> linkReference
            | Some desc when desc.Contains(linkReference) -> desc
            | Some desc -> sprintf "%s\n\n%s" (desc.Trim()) linkReference
        { latestEntry with Description = Some description }.ToString()

let getVersionNumber envVarName ctx =
    let args = ctx.Context.Arguments
    let verArg =
        args
        |> List.tryHead
        |> Option.defaultWith (fun () -> Environment.environVarOrDefault envVarName "")
    if SemVer.isValid verArg then verArg
    elif verArg.StartsWith("v") && SemVer.isValid verArg.[1..] then
        let target = ctx.Context.FinalTarget
        Trace.traceImportantfn "Please specify a version number without leading 'v' next time, e.g. \"./build.sh %s %s\" rather than \"./build.sh %s %s\"" target verArg.[1..] target verArg
        verArg.[1..]
    elif String.isNullOrEmpty verArg then
        let target = ctx.Context.FinalTarget
        Trace.traceErrorfn "Please specify a version number, either at the command line (\"./build.sh %s 1.0.0\") or in the %s environment variable" target envVarName
        failwith "No version number found"
    else
        Trace.traceErrorfn "Please specify a valid version number: %A could not be recognized as a version number" verArg
        failwith "Invalid version number"

module dotnet =
    let watch cmdParam program args =
        DotNet.exec cmdParam (sprintf "watch %s" program) args

    let run cmdParam args =
        DotNet.exec cmdParam "run" args

    let tool optionConfig command args =
        DotNet.exec optionConfig (sprintf "%s" command) args
        |> failOnBadExitAndPrint

    let reportgenerator optionConfig args =
        tool optionConfig "reportgenerator" args

    let sourcelink optionConfig args =
        tool optionConfig "sourcelink" args

    let fcswatch optionConfig args =
        tool optionConfig "fcswatch" args

open DocsTool.CLIArgs
module DocsTool =
    open Argu
    let buildparser = ArgumentParser.Create<BuildArgs>(programName = "docstool")
    let buildCLI () =
        [
            BuildArgs.SiteBaseUrl docsSiteBaseUrl
            BuildArgs.ProjectGlob srcGlob
            BuildArgs.DocsOutputDirectory docsDir
            BuildArgs.DocsSourceDirectory docsSrcDir
            BuildArgs.GitHubRepoUrl gitHubRepoUrl
            BuildArgs.ProjectName gitRepoName
            BuildArgs.ReleaseVersion latestEntry.NuGetVersion
        ]
        |> buildparser.PrintCommandLineArgumentsFlat

    let build () =
        dotnet.run (fun args ->
            { args with WorkingDirectory = docsToolDir }
        ) (sprintf " -- build %s" (buildCLI()))
        |> failOnBadExitAndPrint

    let watchparser = ArgumentParser.Create<WatchArgs>(programName = "docstool")
    let watchCLI () =
        [
            WatchArgs.ProjectGlob srcGlob
            WatchArgs.DocsSourceDirectory docsSrcDir
            WatchArgs.GitHubRepoUrl gitHubRepoUrl
            WatchArgs.ProjectName gitRepoName
            WatchArgs.ReleaseVersion latestEntry.NuGetVersion
        ]
        |> watchparser.PrintCommandLineArgumentsFlat

    let watch projectpath =
        dotnet.watch (fun args ->
           { args with WorkingDirectory = docsToolDir }
        ) "run" (sprintf "-- watch %s" (watchCLI()))
        |> failOnBadExitAndPrint

//-----------------------------------------------------------------------------
// Target Implementations
//-----------------------------------------------------------------------------

let clean _ =
    ["bin"; "temp" ; distDir; coverageReportDir]
    |> Shell.cleanDirs

    !! srcGlob
    ++ testsGlob
    |> Seq.collect(fun p ->
        ["bin";"obj"]
        |> Seq.map(fun sp -> IO.Path.GetDirectoryName p @@ sp ))
    |> Shell.cleanDirs

    [
        "paket-files/paket.restore.cached"
    ]
    |> Seq.iter Shell.rm

let dotnetRestore _ =
    [sln]
    |> Seq.map(fun dir -> fun () ->
        let args =
            [
            ] |> String.concat " "
        DotNet.restore(fun c ->
            { c with
                Common =
                    c.Common
                    |> DotNet.Options.withCustomParams
                        (Some(args))
            }) dir)
    |> Seq.iter(retryIfInCI 10)

let updateChangelog ctx =
    let description, unreleasedChanges =
        match changelog.Unreleased with
        | None -> None, []
        | Some u -> u.Description, u.Changes
    let verStr = ctx |> getVersionNumber "RELEASE_VERSION"
    let newVersion = SemVer.parse verStr
    changelog.Entries
    |> List.tryFind (fun entry -> entry.SemVer = newVersion)
    |> Option.iter (fun entry ->
        Trace.traceErrorfn "Version %s already exists in %s, released on %s" verStr changelogFilename (if entry.Date.IsSome then entry.Date.Value.ToString("yyyy-MM-dd") else "(no date specified)")
        failwith "Can't release with a duplicate version number"
    )
    changelog.Entries
    |> List.tryFind (fun entry -> entry.SemVer > newVersion)
    |> Option.iter (fun entry ->
        Trace.traceErrorfn "You're trying to release version %s, but a later version %s already exists, released on %s" verStr entry.SemVer.AsString (if entry.Date.IsSome then entry.Date.Value.ToString("yyyy-MM-dd") else "(no date specified)")
        failwith "Can't release with a version number older than an existing release"
    )
    let versionTuple version = (version.Major, version.Minor, version.Patch)
    let prereleaseEntries = changelog.Entries |> List.filter (fun entry -> entry.SemVer.PreRelease.IsSome && versionTuple entry.SemVer = versionTuple newVersion)
    let prereleaseChanges = prereleaseEntries |> List.collect (fun entry -> entry.Changes |> List.filter (not << isEmptyChange))
    let assemblyVersion, nugetVersion = Changelog.parseVersions newVersion.AsString
    linkReferenceForLatestEntry <- mkLinkReference newVersion changelog
    let newEntry = Changelog.ChangelogEntry.New(assemblyVersion.Value, nugetVersion.Value, Some System.DateTime.Today, description, unreleasedChanges @ prereleaseChanges, false)
    let newChangelog = Changelog.Changelog.New(changelog.Header, changelog.Description, None, newEntry :: changelog.Entries)
    latestEntry <- newEntry

    // Save changelog to temporary file before making any edits
    changelogBackupFilename <- System.IO.Path.GetTempFileName()
    changelogFilename |> Shell.copyFile changelogBackupFilename
    Target.activateFinal "DeleteChangelogBackupFile"

    newChangelog
    |> Changelog.save changelogFilename

    // Now update the link references at the end of the file
    linkReferenceForLatestEntry <- mkLinkReference newVersion changelog
    let linkReferenceForUnreleased = sprintf "[Unreleased]: %s/compare/%s...%s" gitHubRepoUrl (tagFromVersionNumber newVersion.AsString) "HEAD"
    let tailLines = File.read changelogFilename |> List.ofSeq |> List.rev

    let isRef line = System.Text.RegularExpressions.Regex.IsMatch(line, @"^\[.+?\]:\s?[a-z]+://.*$")
    let linkReferenceTargets =
        tailLines
        |> List.skipWhile String.isNullOrWhiteSpace
        |> List.takeWhile isRef
        |> List.rev  // Now most recent entry is at the head of the list

    let newLinkReferenceTargets =
        match linkReferenceTargets with
        | [] ->
            [linkReferenceForUnreleased; linkReferenceForLatestEntry]
        | first :: rest when first |> String.startsWith "[Unreleased]:" ->
            linkReferenceForUnreleased :: linkReferenceForLatestEntry :: rest
        | first :: rest ->
            linkReferenceForUnreleased :: linkReferenceForLatestEntry :: first :: rest

    let blankLineCount = tailLines |> Seq.takeWhile String.isNullOrWhiteSpace |> Seq.length
    let linkRefCount = linkReferenceTargets |> List.length
    let skipCount = blankLineCount + linkRefCount
    let updatedLines = List.rev (tailLines |> List.skip skipCount) @ newLinkReferenceTargets
    File.write false changelogFilename updatedLines

    // If build fails after this point but before we push the release out, undo our modifications
    Target.activateBuildFailure "RevertChangelog"

let revertChangelog _ =
    if String.isNotNullOrEmpty changelogBackupFilename then
        changelogBackupFilename |> Shell.copyFile changelogFilename

let deleteChangelogBackupFile _ =
    if String.isNotNullOrEmpty changelogBackupFilename then
        Shell.rm changelogBackupFilename

let dotnetBuild ctx =
    let args =
        [
            sprintf "/p:PackageVersion=%s" latestEntry.NuGetVersion
            "--no-restore"
        ]
    DotNet.build(fun c ->
        { c with
            Configuration = configuration (ctx.Context.AllExecutingTargets)
            Common =
                c.Common
                |> DotNet.Options.withAdditionalArgs args

        }) sln

let dotnetTest ctx =
    let excludeCoverage =
        !! testsGlob
        |> Seq.map IO.Path.GetFileNameWithoutExtension
        |> String.concat "|"
    let args =
        [
            "--no-build"
            sprintf "/p:AltCover=%b" (not disableCodeCoverage)
            sprintf "/p:AltCoverThreshold=%d" coverageThresholdPercent
            sprintf "/p:AltCoverAssemblyExcludeFilter=%s" excludeCoverage
        ]
    DotNet.test(fun c ->

        { c with
            Configuration = configuration (ctx.Context.AllExecutingTargets)
            Common =
                c.Common
                |> DotNet.Options.withAdditionalArgs args
            }) sln

let generateCoverageReport _ =
    let coverageReports =
        !!"tests/**/coverage.*.xml"
        |> String.concat ";"
    let sourceDirs =
        !! srcGlob
        |> Seq.map Path.getDirectory
        |> String.concat ";"
    let independentArgs =
            [
                sprintf "-reports:%s"  coverageReports
                sprintf "-targetdir:%s" coverageReportDir
                // Add source dir
                sprintf "-sourcedirs:%s" sourceDirs
                // Ignore Tests and if AltCover.Recorder.g sneaks in
                sprintf "-assemblyfilters:\"%s\"" "-*.Tests;-AltCover.Recorder.g"
                sprintf "-Reporttypes:%s" "Html"
            ]
    let args =
        independentArgs
        |> String.concat " "
    dotnet.reportgenerator id args

let watchTests _ =
    !! testsGlob
    |> Seq.map(fun proj -> fun () ->
        dotnet.watch
            (fun opt ->
                opt |> DotNet.Options.withWorkingDirectory (IO.Path.GetDirectoryName proj))
            "test"
            ""
        |> ignore
    )
    |> Seq.iter (invokeAsync >> Async.Catch >> Async.Ignore >> Async.Start)

    printfn "Press Ctrl+C (or Ctrl+Break) to stop..."
    let cancelEvent = Console.CancelKeyPress |> Async.AwaitEvent |> Async.RunSynchronously
    cancelEvent.Cancel <- true

let generateAssemblyInfo _ =

    let (|Fsproj|Csproj|Vbproj|) (projFileName:string) =
        match projFileName with
        | f when f.EndsWith("fsproj") -> Fsproj
        | f when f.EndsWith("csproj") -> Csproj
        | f when f.EndsWith("vbproj") -> Vbproj
        | _                           -> failwith (sprintf "Project file %s not supported. Unknown project type." projFileName)

    let releaseChannel =
        match latestEntry.SemVer.PreRelease with
        | Some pr -> pr.Name
        | _ -> "release"
    let getAssemblyInfoAttributes projectName =
        [
            AssemblyInfo.Title (projectName)
            AssemblyInfo.Product productName
            AssemblyInfo.Version latestEntry.AssemblyVersion
            AssemblyInfo.Metadata("ReleaseDate", latestEntry.Date.Value.ToString("o"))
            AssemblyInfo.FileVersion latestEntry.AssemblyVersion
            AssemblyInfo.InformationalVersion latestEntry.AssemblyVersion
            AssemblyInfo.Metadata("ReleaseChannel", releaseChannel)
            AssemblyInfo.Metadata("GitHash", Git.Information.getCurrentSHA1(null))
        ]

    let getProjectDetails projectPath =
        let projectName = IO.Path.GetFileNameWithoutExtension(projectPath)
        (
            projectPath,
            projectName,
            IO.Path.GetDirectoryName(projectPath),
            (getAssemblyInfoAttributes projectName)
        )

    srcAndTest
    |> Seq.map getProjectDetails
    |> Seq.iter (fun (projFileName, _, folderName, attributes) ->
        match projFileName with
        | Fsproj -> AssemblyInfoFile.createFSharp (folderName @@ "AssemblyInfo.fs") attributes
        | Csproj -> AssemblyInfoFile.createCSharp ((folderName @@ "Properties") @@ "AssemblyInfo.cs") attributes
        | Vbproj -> AssemblyInfoFile.createVisualBasic ((folderName @@ "My Project") @@ "AssemblyInfo.vb") attributes
        )

let dotnetPack ctx =
    // Get release notes with properly-linked version number
    let releaseNotes = latestEntry |> mkReleaseNotes linkReferenceForLatestEntry
    let args =
        [
            sprintf "/p:PackageVersion=%s" latestEntry.NuGetVersion
            sprintf "/p:PackageReleaseNotes=\"%s\"" releaseNotes
        ]
    DotNet.pack (fun c ->
        { c with
            Configuration = configuration (ctx.Context.AllExecutingTargets)
            OutputPath = Some distDir
            Common =
                c.Common
                |> DotNet.Options.withAdditionalArgs args
        }) sln

let sourceLinkTest _ =
    !! distGlob
    |> Seq.iter (fun nupkg ->
        dotnet.sourcelink id (sprintf "test %s" nupkg)
    )

let publishToNuget _ =
    isReleaseBranchCheck ()
    Paket.push(fun c ->
        { c with
            ToolType = ToolType.CreateLocalTool()
            PublishUrl = publishUrl
            WorkingDir = "dist"
        }
    )
    // If build fails after this point, we've pushed a release out with this version of CHANGELOG.md so we should keep it around
    Target.deactivateBuildFailure "RevertChangelog"

let gitRelease _ =
    isReleaseBranchCheck ()

    let releaseNotesGitCommitFormat = latestEntry.ToString()

    Git.Staging.stageAll ""
    Git.Commit.exec "" (sprintf "Bump version to %s\n\n%s" latestEntry.NuGetVersion releaseNotesGitCommitFormat)
    Git.Branches.push ""

    let tag = tagFromVersionNumber latestEntry.NuGetVersion

    Git.Branches.tag "" tag
    Git.Branches.pushTag "" "origin" tag

let githubRelease _ =
    let token =
        match Environment.environVarOrDefault "GITHUB_TOKEN" "" with
        | s when not (String.IsNullOrWhiteSpace s) -> s
        | _ -> failwith "please set the github_token environment variable to a github personal access token with repo access."

    let files = !! distGlob
    // Get release notes with properly-linked version number
    let releaseNotes = latestEntry |> mkReleaseNotes linkReferenceForLatestEntry

    GitHub.createClientWithToken token
    |> GitHub.draftNewRelease gitOwner gitRepoName (tagFromVersionNumber latestEntry.NuGetVersion) (latestEntry.SemVer.PreRelease <> None) (releaseNotes |> Seq.singleton)
    |> GitHub.uploadFiles files
    |> GitHub.publishDraft
    |> Async.RunSynchronously

let formatCode _ =
    [
        srcCodeGlob
        testsCodeGlob
    ]
    |> Seq.collect id
    // Ignore AssemblyInfo
    |> Seq.filter(fun f -> f.EndsWith("AssemblyInfo.fs") |> not)
    //|> formatFilesAsync FormatConfig.FormatConfig.Default
    //|> Async.RunSynchronously
    //|> Seq.iter(fun result ->
     //   match result with
      //  | Formatted(original, tempfile) ->
       //     tempfile |> Shell.copyFile original
        //    Trace.logfn "Formatted %s" original
       // | _ -> ()
   // )


let buildDocs _ =
    DocsTool.build ()

let watchDocs _ =
    let watchBuild () =
        !! srcGlob
        |> Seq.map(fun proj -> fun () ->
            dotnet.watch
                (fun opt ->
                    opt |> DotNet.Options.withWorkingDirectory (IO.Path.GetDirectoryName proj))
                "build"
                ""
            |> ignore
        )
        |> Seq.iter (invokeAsync >> Async.Catch >> Async.Ignore >> Async.Start)
    watchBuild ()
    DocsTool.watch ()

let releaseDocs ctx =
    isReleaseBranchCheck ()

    Git.Staging.stageAll docsDir
    Git.Commit.exec "" (sprintf "Documentation release of version %s" latestEntry.NuGetVersion)
    if isRelease (ctx.Context.AllExecutingTargets) |> not then
        // We only want to push if we're only calling "ReleaseDocs" target
        // If we're calling "Release" target, we'll let the "GitRelease" target do the git push
        Git.Branches.push ""


//-----------------------------------------------------------------------------
// Target Declaration
//-----------------------------------------------------------------------------

Target.create "Clean" clean
Target.create "DotnetRestore" dotnetRestore
Target.create "UpdateChangelog" updateChangelog
Target.createBuildFailure "RevertChangelog" revertChangelog  // Do NOT put this in the dependency chain
Target.createFinal "DeleteChangelogBackupFile" deleteChangelogBackupFile  // Do NOT put this in the dependency chain
Target.create "DotnetBuild" dotnetBuild
Target.create "DotnetTest" dotnetTest
Target.create "GenerateCoverageReport" generateCoverageReport
Target.create "WatchTests" watchTests
Target.create "GenerateAssemblyInfo" generateAssemblyInfo
Target.create "DotnetPack" dotnetPack
Target.create "SourceLinkTest" sourceLinkTest
Target.create "PublishToNuGet" publishToNuget
Target.create "GitRelease" gitRelease
Target.create "GitHubRelease" githubRelease
//Target.create "FormatCode" formatCode
Target.create "Release" ignore
Target.create "BuildDocs" buildDocs
Target.create "WatchDocs" watchDocs
Target.create "ReleaseDocs" releaseDocs

//-----------------------------------------------------------------------------
// Target Dependencies
//-----------------------------------------------------------------------------


// Only call Clean if DotnetPack was in the call chain
// Ensure Clean is called before DotnetRestore
"Clean" ?=> "DotnetRestore"
"Clean" ==> "DotnetPack"

// Only call GenerateAssemblyInfo if Publish was in the call chain
// Ensure GenerateAssemblyInfo is called after DotnetRestore and before DotnetBuild
"DotnetRestore" ?=> "GenerateAssemblyInfo"
"GenerateAssemblyInfo" ?=> "DotnetBuild"
"GenerateAssemblyInfo" ==> "PublishToNuGet"

// Only call UpdateChangelog if Publish was in the call chain
// Ensure UpdateChangelog is called after DotnetRestore and before GenerateAssemblyInfo
"DotnetRestore" ?=> "UpdateChangelog"
"UpdateChangelog" ?=> "GenerateAssemblyInfo"
"UpdateChangelog" ==> "PublishToNuGet"

"DotnetBuild" ==> "BuildDocs"
"BuildDocs" ==> "ReleaseDocs"
"BuildDocs" ?=> "PublishToNuget"
"DotnetPack" ?=> "BuildDocs"
"GenerateCoverageReport" ?=> "ReleaseDocs"

"DotnetBuild" ==> "WatchDocs"

"DotnetRestore"
    ==> "DotnetBuild"
    =?> ("GenerateCoverageReport", not disableCodeCoverage)
    ==> "DotnetPack"
    ==> "SourceLinkTest"
    ==> "PublishToNuGet"
    ==> "GitRelease"
    ==> "GitHubRelease"
    ==> "Release"

"DotnetRestore"
    ==> "WatchTests"

//-----------------------------------------------------------------------------
// Target Start
//-----------------------------------------------------------------------------

Target.runOrDefaultWithArguments "DotnetPack"
