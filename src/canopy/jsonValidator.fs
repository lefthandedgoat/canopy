module jsonValidator

open FSharp.Data

type Difference =
  | Missing of string
  | Extra   of string

type Type =
  | Array
  | Record
  | AnonymousRecord
  | Property
  | Null
  | Root
  | None

type Meta =
  {
    Path : string
    Name : string
    ParentPath : string
    Type : Type
    ParentType : Type
    ImmediateOptional : bool
    HistoricalOptional : bool
  }

let root =
  {
    Path = "{root}"
    Name = "root"
    ParentPath = ""
    Type = Type.Root
    ParentType = Type.None
    ImmediateOptional = false
    HistoricalOptional = false
  }

let jsonValueToType jsonValue =
  match jsonValue with
  | JsonValue.Array  _ -> Array
  | JsonValue.Record _ -> Record
  | _                  -> Property

let AST jsonValue =
  let rec AST meta jsonValue =
    match jsonValue with
    | JsonValue.String  _
    | JsonValue.Number  _
    | JsonValue.Float   _
    | JsonValue.Boolean _ -> [| { meta with ImmediateOptional = false } |]
    
    | JsonValue.Null -> [| { meta with Type = Null; ImmediateOptional = false } |]

    //real work done here
    | JsonValue.Array values ->
        values
        |> Array.map (fun value -> AST { meta with ParentPath = meta.Path; ParentType = Array; Type = jsonValueToType value; ImmediateOptional = true; HistoricalOptional = true } value)
        |> Array.concat
        |> Array.append [| meta |]

    //real work done here
    | JsonValue.Record props ->
        let selfType = if meta.ParentType = Array then AnonymousRecord else Record
        let selfImmediateOptional = if meta.ParentType = Array then true else false
        let selfPath = if meta.ParentType = Array then sprintf "%s.{}" meta.Path else meta.Path
        let self = if meta.Type = Root then meta else { meta with Type = selfType; ParentType = meta.ParentType; Path = selfPath; ParentPath = meta.ParentPath; ImmediateOptional = selfImmediateOptional }
        props
        |> Array.map (fun (prop, value) ->
             let type' = jsonValueToType value
             let path =
               match type' with
               | Array           -> sprintf "%s.[%s]" self.Path prop
               | Record          -> sprintf "%s.{%s}" self.Path prop
               | Property        -> sprintf "%s.%s"   self.Path prop
               | _        -> failwith (sprintf "should not happen %A" type')

             AST { self with Path = path; Name = prop; ParentPath = self.Path; Type = type'; ParentType = self.Type; ImmediateOptional = false } value
           )
        |> Array.concat
        |> Array.append [| self |]

  AST root jsonValue

let diff example actual =
  let example = JsonValue.Parse(example) |> AST |> Set.ofArray
  //printfn "example: %A" example
  let actual = JsonValue.Parse(actual) |> AST |> Set.ofArray
  //printfn "actual: %A" actual

  //if there is a null in actual and it has a matching array value in example, replace with the array definition because null arrays are legit
  let actual =
    actual
    |> Seq.map (fun meta -> 
        if meta.Type = Type.Null then
          let asArray = { meta with Type = Array; Path = meta.Path.Replace(meta.Name, sprintf "[%s]" meta.Name) }
          let exists = example.Contains(asArray)
          if exists then asArray else meta
        else meta)
    |> Set.ofSeq

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

let validate example actual =
  let diff = diff example actual
  if diff <> [] then failwith (sprintf "Json Validation failed with errors:%s%A" System.Environment.NewLine diff)