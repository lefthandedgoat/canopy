module canopy.history

open System.IO
open System

let p = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
let path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\failedContexts.txt"

let save (results : string list) =
    if Directory.Exists(p) = false then Directory.CreateDirectory(p) |> ignore
    if File.Exists(path) = false then File.Create(path).Close()
    using(new StreamWriter(path)) (fun sw ->    
        sw.Write (String.Join("|", (results |> Array.ofList)))
    )

let get _ =
    let emptyList : string list = []
    if File.Exists(path) = false then 
        emptyList
    else
        using(new StreamReader(path)) (fun sr ->
            let line = sr.ReadToEnd()
            line.Split('|') |> List.ofArray
        )