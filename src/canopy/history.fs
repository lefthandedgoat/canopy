module canopy.history

open System.IO
open System

let p = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
let path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\failedContexts.txt"

let save (results : string list) =
    if configuration.runFailedContextsFirst = true then
        if Directory.Exists(p) = false then Directory.CreateDirectory(p) |> ignore
        use sw = new StreamWriter(path)
        sw.Write (String.concat "|" results)

let get _ =    
    if File.Exists(path) = false then
        []
    else
        use sr = new StreamReader(path)        
        let line = sr.ReadToEnd()
        line.Split('|') |> List.ofArray