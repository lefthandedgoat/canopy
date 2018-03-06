namespace Canopy.CSharp

type Integration () =

    static member DiffJson example actual =
        let diff = JsonValidator.diff example actual
        let diffString =
            diff
            |> List.map (function
            | JsonValidator.Missing s ->
                sprintf "Missing %s" s
            | JsonValidator.Extra s ->
                sprintf "Extra %s" s)
        ResizeArray<string>(diffString)

    static member ValidateJson example actual =
        JsonValidator.validate example actual