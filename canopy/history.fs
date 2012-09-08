module history

open System.IO
open System

let path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy.txt"

let save (results : string list) =
    using(new StreamWriter(path)) (fun sw ->    
        sw.Write (String.Join("|", (results |> Array.ofList)))
    )

let get _ =
    using(new StreamReader(path)) (fun sr ->
        let line = sr.ReadToEnd()
        line.Split('|') |> List.ofArray
    )