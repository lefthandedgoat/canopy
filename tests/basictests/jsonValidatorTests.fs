module jsonValidatorTests

open Canopy.Runner.Runner
open Canopy.Runner.Runner.Operators
open Canopy.JsonValidator

let ( == ) left right = if left <> right then failwith (sprintf "expected %A%sgot %A" right System.Environment.NewLine left)

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

let class1 = """{ "name":"bio 101",  "building":"science", "location": { "lat":4.0212, "long":12.102012, "people": [ { "first":"jane", "middle":"something", "last":"doe" } ] } }"""
let class2 = """{ "name":"chem 101", "building":"science", "location": { "lat":4.0212, "lng":12.102012,  "people": [ { "first":"jane", "last":"doe" } ] } }"""

let withArray = """{ "name":"bio 101", "people": [ { "first":"jane" } ] }"""
let nullArray = """{ "name":"chem 101", "people": null }"""

let withProperty = """{ "name":"bio 101", "age": 1 }"""
let nullProperty = """{ "name":"chem 101", "age": null }"""

let all () =
  context "json validator tests"

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
    diff location3 location5 == [ Missing "{root}.[people].{}.middle" ]

  "extra fields on records in arrays recognized correctly" &&& fun _ ->
    diff location3 location6 == [ Extra "{root}.[people].{}.phone" ]

  "renamed field with extra property shows" &&& fun _ ->
    diff location7 location8 ==
      [
        Missing "{root}.[workers]"

        Extra "{root}.[people]"
        Extra "{root}.[people].{}"
        Extra "{root}.[people].{}.first"
        Extra "{root}.[people].{}.last"
        Extra "{root}.[people].{}.middle"
        Extra "{root}.[people].{}.phone"
      ]

  "nested objects with arrays reocgnized correctly" &&& fun _ ->
    diff class1 class2 ==
      [
        Missing "{root}.{location}.long"
        Missing "{root}.{location}.[people].{}.middle"

        Extra "{root}.{location}.lng"
      ]

  "null array is acceptable" &&& fun _ ->
    diff withArray nullArray == [ ]

  "null property is acceptable" &&& fun _ ->
    diff withProperty nullProperty == [ ]