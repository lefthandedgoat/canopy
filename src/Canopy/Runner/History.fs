module Canopy.Runner.History

open Canopy
open System.IO
open System

let p =
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"

let path =
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\failedContexts.txt"

let saveC runFailedContextsFirst results =
    if runFailedContextsFirst then
        if not (Directory.Exists p) then
            Directory.CreateDirectory(p) |> ignore

        use sw = new StreamWriter(path)
        sw.Write (String.concat "|" results)

let save (results: string list) =
    saveC CanopyRunnerConfig.runFailedContextsFirst results

let get _ =
    if not (File.Exists path) then
        []
    else
        use sr = new StreamReader(path)
        let line = sr.ReadToEnd()
        line.Split('|') |> List.ofArray