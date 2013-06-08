// include Fake lib
#r @"tools\FAKE\tools\FakeLib.dll"
open Fake 
open Fake.AssemblyInfoFile

// Assembly / NuGet package properties
let projectName = "canopy"
let version = "0.7.7"
let projectDescription = "A simple framework in f# on top of selenium for writing UI automation and tests. https://github.com/lefthandedgoat/canopy/wiki/Change-Log"
let authors = ["Chris Holt"]

// Folders
let buildDir = @".\build\"
let testDir = @".\tests\"
let nugetDir = @".\nuget\"

// Targets

// Update assembly info
Target "UpdateAssemblyInfo" (fun _ ->
    CreateFSharpAssemblyInfo ".\AssemblyInfo.fs"
        [ Attribute.Product projectName
          Attribute.Title projectName
          Attribute.Description projectDescription
          Attribute.Version version ]
)

// Clean build directory
Target "Clean" (fun _ ->
    CleanDir buildDir
)

// Build Canopy
Target "BuildCanopy" (fun _ ->
    !! @"canopy\canopy.fsproj"
      |> MSBuildDebug buildDir "Build"
      |> Log "AppBuild-Output: "
)

// Clean tests directory
Target "CleanTests" (fun _ ->
    CleanDir testDir
)

// Test Canopy
Target "TestCanopy" (fun _ ->
    !! @"basictests\basictests.fsproj"
      |> MSBuildDebug testDir "Build"
      |> Log "AppBuild-Output: "
      
    let result =
        ExecProcess (fun info -> 
            info.FileName <- (testDir @@ "basictests.exe")
        ) (System.TimeSpan.FromMinutes 3.)
    if result <> 0 then failwith "Failed result from basictests"
)

// Clean NuGet directory
Target "CleanNuGet" (fun _ ->
    CleanDir nugetDir
)

// Create NuGet package
Target "CreateNuGet" (fun _ ->     
    XCopy @".\build\" (nugetDir @@ "lib")
    !+ @"nuget/lib/*.*"
        -- @"nuget/lib/canopy*.*"
        |> ScanImmediately
        |> Seq.iter (System.IO.File.Delete)

    "canopy.nuspec"
      |> NuGet (fun p -> 
            {p with
                Project = projectName
                Authors = authors
                Version = version
                Description = projectDescription
                NoPackageAnalysis = true
                ToolPath = @".\tools\Nuget\Nuget.exe" 
                OutputPath = nugetDir })
)

// Default target
Target "Default" (fun _ ->
    trace "Building canopy"
)

// Dependencies
"UpdateAssemblyInfo"
  ==> "CleanTests"
  ==> "TestCanopy"
  ==> "Clean"
  ==> "BuildCanopy"
  ==> "CleanNuGet"
  ==> "CreateNuGet"
  ==> "Default"

// start build
Run "Default"