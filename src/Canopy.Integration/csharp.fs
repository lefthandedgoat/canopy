namespace Canopy.CSharp

type Integration () =

    static member DiffJson example actual =
        let diff = Canopy.JsonValidator.diff example actual
        let diffString =
            diff
            |> List.map (function
            | Canopy.JsonValidator.Missing s ->
                sprintf "Missing %s" s
            | Canopy.JsonValidator.Extra s ->
                sprintf "Extra %s" s)
        ResizeArray<string>(diffString)

    static member ValidateJson example actual =
        Canopy.JsonValidator.validate example actual