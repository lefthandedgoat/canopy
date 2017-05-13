module jsonValidator

#r "../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data

let mutable tests : (string * (unit -> unit)) list = []
let ( &&& ) desc f = tests <- (desc, f) :: tests
let ( == ) left right = if left <> right then failwith (sprintf "expected %A got %A" right left)
let clear () = tests <- []
let run () = tests |> List.rev |> List.iter (fun (desc, f) -> printfn "%s" desc; try f(); printfn "pass" with ex -> printfn "failed: %s" ex.Message)

type Difference =
  | Missing of string
  | Extra   of string

type NodeType =
  | Array
  | Record
  | Other

type Meta =
  {
    Path : string
    ParentPath : string
    ParentType : NodeType
    ImmediateMissing : bool
    HistoricalMissing : bool
  }

let root =
  {
    Path = "{root}"
    ParentPath = ""
    ParentType = NodeType.Record
    ImmediateMissing = false
    HistoricalMissing = false
  }

let jsonValueToNodeType jsonValue =
  match jsonValue with
  | JsonValue.Array  _ -> NodeType.Array
  | JsonValue.Record _ -> NodeType.Record
  | _                  -> NodeType.Other

let AST jsonValue =
  let rec AST meta jsonValue =
    match jsonValue with
    | JsonValue.String  _
    | JsonValue.Number  _
    | JsonValue.Float   _
    | JsonValue.Boolean _
    | JsonValue.Null      -> [| meta |]

    //real work done here
    | JsonValue.Array values ->
        values
        |> Array.map (AST { meta with ParentType = Array; ImmediateMissing = true; HistoricalMissing = true })
        |> Array.concat

    //real work done here
    | JsonValue.Record props ->
        props
        |> Array.map (fun (prop, value) ->
             let nodeType = jsonValueToNodeType value
             let immediateMissing =  match nodeType with | Array -> true | _ -> false
             let historicalMissing = match nodeType with | Array -> true | _ -> meta.HistoricalMissing
             let path =
               match nodeType with
               | Array  -> sprintf "%s.[%s]" meta.Path prop
               | Record -> sprintf "%s.{%s}" meta.Path prop
               | Other  -> sprintf "%s.%s" meta.Path prop

             AST { Path = path; ParentPath = meta.Path; ParentType = Record; ImmediateMissing = immediateMissing; HistoricalMissing = historicalMissing } value
           )
        |> Array.concat

  AST root jsonValue

let diff example actual =
  let example = JsonValue.Parse(example) |> AST |> Set.ofArray
  //printfn "%A" example
  let actual = JsonValue.Parse(actual) |> AST |> Set.ofArray
  //printfn "%A" actual

  let missing =
    let allMissing =
      example - actual
      |> Seq.filter (fun meta -> meta.ImmediateMissing = false && meta.HistoricalMissing = false)
      |> Set.ofSeq
    printfn "allMissing %A" allMissing

    let falsePositives =
      allMissing
      |> Seq.filter (fun meta ->
                       (meta.ImmediateMissing = true || meta.HistoricalMissing = true)
                       && actual |> Seq.exists (fun meta2 -> meta.ParentPath = meta2.ParentPath))
      |> Set.ofSeq

    printfn "falsePositives %A" falsePositives

    allMissing - falsePositives
    |> Seq.map    (fun meta -> Missing meta.Path)
    |> List.ofSeq

  let missingsThatExistInActual = actual |> Seq.filter (fun meta -> (meta.ImmediateMissing = true || meta.HistoricalMissing = true))
  let allThatCanBeMissing = example |> Seq.filter (fun meta -> (meta.ImmediateMissing = true || meta.HistoricalMissing = true))
  //todo
  //let siblingsPropertiesToMissing =
  //    missingsThatExistInActual
  //    |> Seq.map (fun meta ->



  let extra =
    actual - example
    |> Seq.map    (fun meta -> Extra meta.Path)
    |> List.ofSeq

  missing @ extra

let person1 = """{ "first":"jane", "middle":"something", "last":"doe" } """
let person2 = """{ "first":"jane", "last":"doe" } """
let person3 = """{ "first":"jane", "middle":"something", "last":"doe", "phone":"800-555-5555" } """

let location1 = """{ "lat":4.0212, "long":12.102012, "people":[ 1, 2, 3 ] } """
let location2 = """{ "lat":4.0212, "long":12.102012, "people":[ ] } """

let location3 = """{ "lat":4.0212, "long":12.102012, "people":[ { "first":"jane", "middle":"something", "last":"doe" } ] } """
let location4 = """{ "lat":4.0212, "long":12.102012, "people":[ ] } """
let location5 = """{ "lat":4.0212, "long":12.102012, "people":[ { "first":"jane", "last":"doe" } ] } """
let location6 = """{ "lat":4.0212, "long":12.102012, "people":[ { "first":"jane", "middle":"something", "last":"doe", "phone":"800-555-5555" } ] } """




run()


clear()

(*
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


*)
