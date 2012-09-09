module history

open System.IO
open System

let path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\failedContexts.txt"

let save (results : string list) =
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