#r @"packages/build/FAKE/tools/FakeLib.dll"
open Fake 
open Fake.Git
open Fake.AssemblyInfoFile
open Fake.ReleaseNotesHelper
open System

let project = "canopy"
let projectIntegration = "canopy.integration"
let summary = "F# web testing framework"
let description = """A simple framework in F# on top of selenium for writing UI automation and tests."""
let descriptionIntegration = """A sister package to canopy for integration tests."""
let authors = [ "Chris Holt"; "Henrik Feldt" ]
let tags = "f# fsharp canopy selenium ui automation tests"

let solutionFile  = "canopy"
let testAssemblies = "tests/**/bin/Release/*basictests*.exe"

let gitHome = "https://github.com/lefthandedgoat"
let gitName = "canopy"

Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
let release = parseReleaseNotes (IO.File.ReadAllLines "RELEASE_NOTES.md")

// Generate assembly info files with the right version & up-to-date information
Target "AssemblyInfo" (fun _ ->
  let fileName = "src/" + project + "/AssemblyInfo.fs"
  CreateFSharpAssemblyInfo fileName
      [ Attribute.Title project
        Attribute.Product project
        Attribute.Description summary
        Attribute.Version release.AssemblyVersion
        Attribute.FileVersion release.AssemblyVersion ] 

  let fileName = "src/" + projectIntegration + "/AssemblyInfo.fs"
  CreateFSharpAssemblyInfo fileName
      [ Attribute.Title projectIntegration
        Attribute.Product projectIntegration
        Attribute.Description summary
        Attribute.Version release.AssemblyVersion
        Attribute.FileVersion release.AssemblyVersion ] 
)

Target "LoggingFile" (fun _ ->
    ReplaceInFiles [ "namespace Logary.Facade", "namespace Canopy.Logging" ]
                   [ "paket-files/logary/logary/src/Logary.Facade/Facade.fs" ]
)

// --------------------------------------------------------------------------------------
// Clean build results

Target "Clean" (fun _ ->
    CleanDirs ["bin"; "temp"]
)

Target "CleanDocs" (fun _ ->
    CleanDirs ["docs/output"]
)

// --------------------------------------------------------------------------------------
// Build library & test project

Target "Build" (fun _ ->
    !! (solutionFile + "*.sln")
    |> MSBuild "" "Rebuild" [ "Configuration", "Release"; "VisualStudioVersion", "14.0" ]
    |> ignore
)

// --------------------------------------------------------------------------------------
// Run the unit tests using test runner

Target "RunTests" (fun _ ->
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
// Build a NuGet package

Target "NuGet" (fun _ ->
    CleanDirs ["temp"]
    CreateDir "temp/lib"
    
    XCopy @"./bin" "temp/lib"
    !! @"temp/lib/*.*"
      -- @"temp/lib/canopy.???"
      |> Seq.iter (System.IO.File.Delete)

    NuGet (fun p -> 
        { p with   
            Authors = authors
            Project = project
            Summary = summary
            Description = description
            Version = release.NugetVersion
            ReleaseNotes = String.Join(Environment.NewLine, release.Notes)
            Tags = tags
            WorkingDir = "temp"
            OutputPath = "bin"
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey"
            Dependencies = [] })
        ("nuget/" + project + ".nuspec")
)

Target "NuGet.Integration" (fun _ ->
    CleanDirs ["temp"]
    CreateDir "temp/lib"
    
    XCopy @"./bin" "temp/lib"
    !! @"temp/lib/*.*"
      -- @"temp/lib/canopy.integration.???"
      |> Seq.iter (System.IO.File.Delete)

    NuGet (fun p -> 
        { p with   
            Authors = authors
            Project = projectIntegration
            Summary = summary
            Description = descriptionIntegration
            Version = release.NugetVersion
            ReleaseNotes = String.Join(Environment.NewLine, release.Notes)
            Tags = tags
            WorkingDir = "temp"
            OutputPath = "bin"
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey"
            Dependencies = [] })
        ("nuget/canopy.integration.nuspec")
)

// --------------------------------------------------------------------------------------
// Generate the documentation

Target "GenerateDocs" (fun _ ->
    executeFSIWithArgs "docs/tools" "generate.fsx" ["--define:RELEASE"] [] |> ignore
)

// --------------------------------------------------------------------------------------
// Release Scripts

Target "ReleaseDocs" (fun _ ->
    let tempDocsDir = "temp/gh-pages"
    CleanDir tempDocsDir
    Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir

    fullclean tempDocsDir
    CopyRecursive "docs/output" tempDocsDir true |> tracefn "%A"
    StageAll tempDocsDir
    Commit tempDocsDir (sprintf "Update generated documentation for version %s" release.NugetVersion)
    Branches.push tempDocsDir
)

Target "Release" DoNothing

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target "All" DoNothing

"Clean" 
  ==> "AssemblyInfo"
  ==> "LoggingFile"
  ==> "Build"
  ==> "RunTests"
  //==> "CleanDocs"
  //==> "GenerateDocs"
  //==> "ReleaseDocs"
  ==> "Release"
  ==> "All"

RunTargetOrDefault "All"
