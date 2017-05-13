module jsonValidator

#r "../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data

let mutable tests : (string * (unit -> unit)) list = []
let ( &&& ) desc f = tests <- (desc, f) :: tests
let run () = tests |> List.rev |> List.iter (fun (desc, f) -> printfn "%s" desc; try f(); printfn "pass" with ex -> printfn "failed: %s" ex.Message)
let ( == ) left right = if left <> right then failwith (sprintf "expected %A got %A" left right)
let clear () = tests <- []

type Difference =
  | Missing of string
  | Extra   of string

let AST jsonValue =
  let rec AST parent jsonValue =
    match jsonValue with
    | JsonValue.String  _    -> [| parent |]
    | JsonValue.Number  _    -> [| parent |]
    | JsonValue.Float   _    -> [| parent |]
    | JsonValue.Boolean _    -> [| parent |]
    | JsonValue.Null         -> [| parent |]
    | JsonValue.Array values -> values |> Array.map (AST parent) |> Array.concat
    | JsonValue.Record props -> props  |> Array.map (fun (prop, value) -> AST (sprintf "%s.%s" parent prop) value) |> Array.concat

  AST "{root}" jsonValue

let diff example actual =
  let example = JsonValue.Parse(example) |> AST |> Set.ofArray
  //printfn "%A" example
  let actual = JsonValue.Parse(actual) |> AST |> Set.ofArray
  //printfn "%A" actual
  let missing = example - actual |> Seq.map (fun missing -> Difference.Missing missing) |> List.ofSeq
  missing

let validate example actual =
  let results = diff example actual
  if results <> [] then failwith (sprintf "%A" diff)

let person1 = """{ "first":"jane", "middle":"something", "last":"doe" } """
let person3 = """{ "first":"jane", "last":"doe" } """

"two identical people have no differences" &&& fun _ ->
  diff person1 person1 == []

"missing property is identified" &&& fun _ ->
  diff person1 person3 == [ Missing "{root}.middle" ]

run()

clear()
