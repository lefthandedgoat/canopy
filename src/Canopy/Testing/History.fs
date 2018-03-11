module Canopy.History

open System.IO
open System
open Canopy.Runner

let p = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
let path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\failedContexts.txt"

let saveC (config: CanopyRunnerConfig) results =
    if config.runFailedContextsFirst = true then
        if not (Directory.Exists p) then
            Directory.CreateDirectory(p) |> ignore

        use sw = new StreamWriter(path)
        sw.Write (String.concat "|" results)


let save (results: string list) =
    saveC (CanopyRunnerConfig.create ())

let get _ =
    if not (File.Exists path) then
        []
    else
        use sr = new StreamReader(path)
        let line = sr.ReadToEnd()
        line.Split('|') |> List.ofArray