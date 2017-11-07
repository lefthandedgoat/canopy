namespace canopy.csharp

type integration () =

    static member diffJson example actual =
        let diff = jsonValidator.diff example actual
        let diffString = diff |> List.map (fun d -> match d with | jsonValidator.Missing s -> sprintf "Missing %s" s | jsonValidator.Extra s -> sprintf "Extra %s" s)
        ResizeArray<string>(diffString)

    static member validateJson example actual = jsonValidator.validate example actual