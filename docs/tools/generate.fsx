﻿// --------------------------------------------------------------------------------------
// Builds the documentation from `.fsx` and `.md` files in the 'docs/content' directory
// (the generated documentation is stored in the 'docs/output' directory)
// --------------------------------------------------------------------------------------

// Binaries that have XML documentation (in a corresponding generated XML file)
let referenceBinaries = [ "canopy.dll" ]
    //[ "WebDriver.dll"; "WebDriver.Support.dll"; "Newtonsoft.Json.dll"; "SizSelCsZzz.dll"; "canopy.dll" ]
// Web site location for the generated documentation
let website = "/canopy"

let githubLink = "https://github.com/lefthandedgoat/canopy"

// Specify more information about your project
let info =
  [ "project-name", "canopy"
    "project-author", "Chris Holt"
    "project-summary", "A simple framework in f# on top of selenium for writing UI automation and tests."
    "project-github", githubLink
    "project-nuget", "https://www.nuget.org/packages/canopy/" ]

// --------------------------------------------------------------------------------------
// For typical project, no changes are needed below
// --------------------------------------------------------------------------------------

#load "../../packages/FSharp.Formatting/FSharp.Formatting.fsx"
#I "../../packages/FAKE/tools/"
#r "FakeLib.dll"
open Fake
open System.IO
open Fake.FileHelper
open FSharp.Literate
open FSharp.MetadataFormat

// When called from 'build.fsx', use the public project URL as <root>
// otherwise, use the current 'output' directory.
#if RELEASE
let root = website
#else
let root = "file://" + (__SOURCE_DIRECTORY__ @@ "../output")
#endif

// Paths with template/source/output locations
let bin        = __SOURCE_DIRECTORY__ @@ "../../bin"
let content    = __SOURCE_DIRECTORY__ @@ "../content"
let output     = __SOURCE_DIRECTORY__ @@ "../output"
let files      = __SOURCE_DIRECTORY__ @@ "../files"
let templates  = __SOURCE_DIRECTORY__ @@ "templates"
let formatting = __SOURCE_DIRECTORY__ @@ "../../packages/FSharp.Formatting/"
let docTemplate = formatting @@ "templates/docpage.cshtml"

// Where to look for *.csproj templates (in this order)
let layoutRoots =
  [ templates; formatting @@ "templates"
    formatting @@ "templates/reference" ]

// Copy static files and CSS + JS from F# Formatting
let copyFiles () =
  CopyRecursive files output true |> Log "Copying file: "
  ensureDirectory (output @@ "content")
  CopyRecursive (formatting @@ "styles") (output @@ "content") true 
    |> Log "Copying styles and scripts: "

// Build API reference from XML comments
let buildReference () =
  CleanDir (output @@ "reference")
  for lib in referenceBinaries do
    MetadataFormat.Generate
      ( bin @@ lib, output @@ "reference", layoutRoots, 
        parameters = ("root", root)::info,
        sourceRepo = githubLink @@ "tree/scaffold", // TODO: revert to "tree/master"
        sourceFolder = __SOURCE_DIRECTORY__ @@ ".." @@ "..",
        publicOnly = true,
        libDirs = [__SOURCE_DIRECTORY__ @@ ".." @@ ".." @@ "bin"])

// Build documentation from `fsx` and `md` files in `docs/content`
let buildDocumentation () =
  let subdirs = Directory.EnumerateDirectories(content, "*", SearchOption.AllDirectories)
  for dir in Seq.append [content] subdirs do
    let sub = if dir.Length > content.Length then dir.Substring(content.Length + 1) else "."
    Literate.ProcessDirectory
      ( dir, docTemplate, output @@ sub, replacements = ("root", root)::info,
        layoutRoots = layoutRoots )

// Generate
copyFiles()
buildDocumentation()
buildReference()
