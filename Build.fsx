// --------------------------------------------------------------------------------------
// FAKE build script 
// --------------------------------------------------------------------------------------
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open System
open Fake.Core.TargetOperators

// --------------------------------------------------------------------------------------
// Project-specific details below
// --------------------------------------------------------------------------------------

// Information about the project are used
//  - for version and project name in generated AssemblyInfo file
//  - by the generated NuGet package 
//  - to run tests and to publish documentation on GitHub gh-pages
//  - for documentation, you also need to edit info in "docs/tools/generate.fsx"

// The name of the project 
// (used by attributes in AssemblyInfo, name of a NuGet package and directory in 'src')
let project = "canopy"
let projectIntegration = "canopy.integration"

// Short summary of the project
// (used as description in AssemblyInfo and as a short summary for NuGet package)
let summary = "F# web testing framework"

// Longer description of the project
// (used as a description for NuGet package; line breaks are automatically cleaned up)
let description = """A simple framework in F# on top of selenium for writing UI automation and tests."""
let descriptionIntegration = """A sister package to canopy for integration tests."""
// List of author names (for NuGet package)
let authors = [ "Chris Holt" ]
// Tags for your project (for NuGet package)
let tags = "f# fsharp canopy selenium ui automation tests"

// File system information 
// (<solutionFile>.sln is built during the building process)
let solutionFile  = "canopy"
// Pattern specifying assemblies to be tested using NUnit
let testAssemblies = "tests/**/bin/Release/*basictests*.exe"

// Git configuration (used for publishing documentation in gh-pages branch)
// The profile where the project is posted 
let gitHome = "https://github.com/lefthandedgoat"
// The name of the project on GitHub
let gitName = "canopy"

// --------------------------------------------------------------------------------------
// END TODO: The rest of the file includes standard build steps 
// --------------------------------------------------------------------------------------

// Read additional information from the release notes document
Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
let release = Fake.Core.ReleaseNotes.parse (IO.File.ReadAllLines "RELEASE_NOTES.md")

// Generate assembly info files with the right version & up-to-date information
Fake.Core.Target.create "AssemblyInfo" (fun _ ->
  let fileName = "src/" + project + "/AssemblyInfo.fs"
  Fake.DotNet.AssemblyInfoFile.createFSharp fileName
      [ Fake.DotNet.AssemblyInfo.Title project
        Fake.DotNet.AssemblyInfo.Product project
        Fake.DotNet.AssemblyInfo.Description summary
        Fake.DotNet.AssemblyInfo.Version release.AssemblyVersion
        Fake.DotNet.AssemblyInfo.FileVersion release.AssemblyVersion ] 

  let fileName = "src/" + projectIntegration + "/AssemblyInfo.fs"
  Fake.DotNet.AssemblyInfoFile.createFSharp fileName
      [ Fake.DotNet.AssemblyInfo.Title projectIntegration
        Fake.DotNet.AssemblyInfo.Product projectIntegration
        Fake.DotNet.AssemblyInfo.Description summary
        Fake.DotNet.AssemblyInfo.Version release.AssemblyVersion
        Fake.DotNet.AssemblyInfo.FileVersion release.AssemblyVersion ] 
)

// --------------------------------------------------------------------------------------
// Clean build results

Fake.Core.Target.create "Clean" (fun _ ->
    Fake.IO.Shell.cleanDirs ["bin"; "temp"]
)

Fake.Core.Target.create "CleanDocs" (fun _ ->
    Fake.IO.Shell.cleanDirs ["docs/output"]
)

// --------------------------------------------------------------------------------------
// Build library & test project

Fake.Core.Target.create "Build" (fun _ ->
    !! (solutionFile + "*.sln")
    |> MSBuild "" "Rebuild" [ "Configuration", "Release"; "VisualStudioVersion", "15.0" ]
    |> ignore
)

// --------------------------------------------------------------------------------------
// Run the unit tests using test runner

Fake.Core.Target.create "RunTests" (fun _ ->
//    ()
    !! testAssemblies 
    |> Seq.iter (fun testFile ->
        let result =
            ExecProcess 
                (fun info -> info.FileName <- testFile) 
                (System.TimeSpan.FromMinutes 5.)
        if result <> 0 then failwith "Failed result from basictests"
    )
)

// --------------------------------------------------------------------------------------
// Generate the documentation

let fakePath = "packages" @@ "FAKE" @@ "tools" @@ "FAKE.exe"
let fakeStartInfo script workingDirectory args fsiargs environmentVars =
    (fun (info: System.Diagnostics.ProcessStartInfo) ->
        info.FileName <- System.IO.Path.GetFullPath fakePath
        info.Arguments <- sprintf "%s --fsiargs -d:FAKE %s \"%s\"" args fsiargs script
        info.WorkingDirectory <- workingDirectory
        let setVar k v =
            info.EnvironmentVariables.[k] <- v
        for (k, v) in environmentVars do
            setVar k v
        setVar "MSBuild" msBuildExe
        setVar "GIT" Git.CommandHelper.gitPath
        setVar "FSI" fsiPath)

/// Run the given buildscript with FAKE.exe
let executeFAKEWithOutput workingDirectory script fsiargs envArgs =
    let exitCode =
        ExecProcessWithLambdas
            (fakeStartInfo script workingDirectory "" fsiargs envArgs)
            TimeSpan.MaxValue false ignore ignore
    System.Threading.Thread.Sleep 1000
    exitCode

// Documentation
let buildDocumentationTarget fsiargs target =
    Fake.Core.Trace.trace (sprintf "Building documentation (%s), this could take some time, please wait..." target)
    let exit = executeFAKEWithOutput "docs/tools" "generate.fsx" fsiargs ["target", target]
    if exit <> 0 then
        failwith "generating reference documentation failed"
    ()

Fake.Core.Target.create "GenerateDocs" (fun _ ->
    buildDocumentationTarget "-D:RELEASE -d:REFERENCE" "Default"
 )

// --------------------------------------------------------------------------------------
// Release Scripts

Fake.Core.Target.create "ReleaseDocs" (fun _ ->
    let tempDocsDir = "temp/gh-pages"
    Fake.IO.Shell.cleanDir tempDocsDir
    Fake.Tools.Git.Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir

    Fake.Tools.Git.Repository.fullclean tempDocsDir
    Fake.IO.Shell.copyRecursive "docs/output" tempDocsDir true |> Fake.Core.Trace.tracefn "%A"
    Fake.Tools.Git.Staging.stageAll tempDocsDir
    Fake.Tools.Git.Commit.exec tempDocsDir (sprintf "Update generated documentation for version %s" release.NugetVersion)
    Fake.Tools.Git.Branches.push tempDocsDir
)

Fake.Core.Target.create "Release" ignore

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Fake.Core.Target.create "All" ignore

"Clean"
  ==> "AssemblyInfo"
  ==> "Build"
  // ==> "RunTests"
  ==> "All"

"CleanDocs"
  ==> "GenerateDocs"
  ==> "ReleaseDocs"

"All" 
  ==> "CleanDocs"
  ==> "GenerateDocs"
  ==> "ReleaseDocs"
  ==> "Release"

Fake.Core.Target.runOrDefault "All"
