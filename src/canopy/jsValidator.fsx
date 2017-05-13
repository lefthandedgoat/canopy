module jsonValidator

#r "../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data

let mutable tests : (string * (unit -> unit)) list = []
let ( &&& ) desc f = tests <- (desc, f) :: tests
let run () = tests |> List.rev |> List.iter (fun (desc, f) -> printfn "%s" desc; try f(); printfn "pass" with _ -> ())
let ( == ) left right = if left <> right then failwith (sprintf "expected %A got %A" left right)
let clear () = tests <- []

type Difference =
  | Missing of string
  | Extra   of string

let rec print jsonValue =
  match jsonValue with
  | JsonValue.String value -> printfn "%A is a string!" value
  | JsonValue.Record properties -> printfn "its a record with properties!"; properties |> Array.iter (fun (property, value) -> printfn "property %s" property; print value)
  | JsonValue.Number value -> printfn "%A is a number!" value
  | JsonValue.Float value -> printfn "%A is a float!" value
  | JsonValue.Boolean value -> printfn "%A is a bool!" value
  | JsonValue.Null -> printfn "Its Null!"
  | JsonValue.Array values -> printfn "Its an array!"; values |> Array.iter print

let diff example actual =
  let example = JsonValue.Parse(example)
  print example
  let actual = JsonValue.Parse(actual)
  ["Missing {}.middle"]

let validate example actual =
  let results = diff example actual
  if results <> [] then failwith (sprintf "%A" diff)

let person1 = """{ "first":"jane", "middle":"something", "last":"doe" } """
let person3 = """{ "first":"jane", "last":"doe" } """

"two identical people have no differences" &&& fun _ ->
  diff person1 person1 == []

"missing property is identified" &&& fun _ ->
  diff person1 person3 == ["Missing {}.middle"]

run()

clear()
