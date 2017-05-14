module jsonValidator

#r "../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data

let mutable tests : (string * (unit -> unit)) list = []
let ( &&& ) desc f = tests <- (desc, f) :: tests
let ( == ) left right = if left <> right then failwith (sprintf "expected %A%sgot %A" right System.Environment.NewLine left)
let clear () = tests <- []
let run () = tests |> List.rev |> List.iter (fun (desc, f) -> printfn "%s" desc; try f(); printfn "pass" with ex -> printfn "failed: %s" ex.Message)

type Difference =
  | Missing of string
  | Extra   of string

type Type =
  | Array
  | Record
  | AnonymousRecord
  | Property
  | Root

type Meta =
  {
    Path : string
    ParentPath : string
    Type : Type
    ParentType : Type
    ImmediateOptional : bool
    HistoricalOptional : bool
  }

let root =
  {
    Path = "{root}"
    ParentPath = "{root}"
    Type = Type.Record
    ParentType = Type.Root
    ImmediateOptional = false
    HistoricalOptional = false
  }

let jsonValueToType jsonValue =
  match jsonValue with
  | JsonValue.Array  _ -> Array
  | JsonValue.Record _ -> AnonymousRecord
  | _                  -> Property

let AST jsonValue =
  let rec AST meta jsonValue =
    match jsonValue with
    | JsonValue.String  _
    | JsonValue.Number  _
    | JsonValue.Float   _
    | JsonValue.Boolean _
    | JsonValue.Null    -> [| { meta with ImmediateOptional = false } |]

    //real work done here
    | JsonValue.Array values ->
        values
        |> Array.map (AST { meta with ParentType = Array; ImmediateOptional = true; HistoricalOptional = true })
        |> Array.concat

    //real work done here
    | JsonValue.Record props ->
        props
        |> Array.map (fun (prop, value) ->
             let type' = jsonValueToType value
             let path =
               match meta.ParentType with
               | Root
               | AnonymousRecord ->
                 match type' with
                 | Array           -> sprintf "%s.[%s]" meta.Path prop
                 | Record          -> sprintf "%s.{%s}" meta.Path prop
                 | Property        -> sprintf "%s.%s"   meta.Path prop
                 | _        -> failwith (sprintf "should not happen %A" type')
               | Array
               | Record
               | Property ->
                 match type' with
                 | Array           -> sprintf "%s.[%s]" meta.Path prop
                 | Record          -> sprintf "%s.{%s}" meta.Path prop
                 | Property        -> sprintf "%s.%s"   meta.Path prop
                 | _        -> failwith (sprintf "should not happen 2 %A" type')

             AST { meta with Path = path; ParentPath = meta.Path; Type = type'; ParentType = meta.Type; } value
           )
        |> Array.concat

  AST root jsonValue

let diff example actual =
  let example = JsonValue.Parse(example) |> AST |> Set.ofArray
  printfn "example: %A" example
  let actual = JsonValue.Parse(actual) |> AST |> Set.ofArray
  //printfn "actual: %A" actual

  let missing =
    let allMissing =
      example - actual
      |> Seq.filter (fun meta -> meta.ImmediateOptional = false && meta.HistoricalOptional = false)
      |> Set.ofSeq
    //printfn "allMissing %A" allMissing

    let falsePositives =
      allMissing
      |> Seq.filter (fun meta ->
                       (meta.ImmediateOptional = true || meta.HistoricalOptional = true)
                       && actual |> Seq.exists (fun meta2 -> meta.ParentPath = meta2.ParentPath))
      |> Set.ofSeq

    //printfn "falsePositives %A" falsePositives

    allMissing - falsePositives
    |> Seq.map    (fun meta -> Missing meta.Path)
    |> List.ofSeq

  let missingButWithSiblingsThatArent =
    let optionalThatExistInActual = actual |> Seq.filter (fun meta -> (meta.ImmediateOptional = true || meta.HistoricalOptional = true))
    example - actual
    |> Seq.filter (fun meta -> optionalThatExistInActual |> Seq.exists (fun meta2 -> meta.ParentPath = meta2.ParentPath))
    |> Seq.map    (fun meta -> Missing meta.Path)
    |> List.ofSeq

  let extra =
    actual - example
    |> Seq.map    (fun meta -> Extra meta.Path)
    |> List.ofSeq

  missing @ missingButWithSiblingsThatArent @ extra

let person1 = """{ "first":"jane", "middle":"something", "last":"doe" } """
let person2 = """{ "first":"jane", "last":"doe" } """
let person3 = """{ "first":"jane", "middle":"something", "last":"doe", "phone":"800-555-5555" } """

let location1 = """{ "lat":4.0212, "long":12.102012, "people":[ 1, 2, 3 ] } """
let location2 = """{ "lat":4.0212, "long":12.102012, "people":[ ] } """

let location3 = """{ "lat":4.0212, "long":12.102012, "people":[ { "first":"jane", "middle":"something", "last":"doe" } ] } """
let location4 = """{ "lat":4.0212, "long":12.102012, "people":[ ] } """
let location5 = """{ "lat":4.0212, "long":12.102012, "people":[ { "first":"jane", "last":"doe" } ] } """
let location6 = """{ "lat":4.0212, "long":12.102012, "people":[ { "first":"jane", "middle":"something", "last":"doe", "phone":"800-555-5555" } ] } """

let location7 = """{ "lat":4.0212, "long":12.102012, "workers":[ { "first":"jane", "last":"doe" } ] } """
let location8 = """{ "lat":4.0212, "long":12.102012, "people":[ { "first":"jane", "middle":"something", "last":"doe", "phone":"800-555-5555" } ] } """

"two identical people have no differences" &&& fun _ ->
  diff person1 person1 == []

"missing property is identified" &&& fun _ ->
  diff person1 person2 == [ Missing "{root}.middle" ]

"extra property is identified" &&& fun _ ->
  diff person1 person3 == [ Extra "{root}.phone" ]

"empty array is acceptable array of ints" &&& fun _ ->
  diff location1 location2 == [ ]

"empty array is acceptable array of records" &&& fun _ ->
  diff location3 location4 == [ ]

"missing fields on records in arrays recognized correctly" &&& fun _ ->
  diff location3 location5 == [ Missing "{root}.[people].middle" ]

"extra fields on records in arrays recognized correctly" &&& fun _ ->
  diff location3 location6 == [ Extra "{root}.[people].phone" ]

"renamed field with extra property shows" &&& fun _ ->
  diff location7 location8 ==
    [
      Missing "{root}.[workers].first"
      Missing "{root}.[workers].last"

      Extra "{root}.[people].first"
      Extra "{root}.[people].middle"
      Extra "{root}.[people].last"
      Extra "{root}.[people].phone"
    ]

run()

clear()

(*

*)
