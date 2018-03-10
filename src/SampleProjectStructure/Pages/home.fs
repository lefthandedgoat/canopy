module home

open canopy
open runner
open canopyExtensions
open config
open common

//page info
let address = root + "/Home"
let Landing = address + "/Landing"

//selctors
let welcomeText = "#welcome"
let email = "#viewEmail"
let bills = "#viewBills"
let news = "#viewNews"

//helpers
//none currently

//tests
let positive _ =
    context "positive home tests"

    "do something" &&& fun _ ->
        url address

let boundary _ =
    context "boundary home tests"

    "do something" &&& fun _ ->
        url address

let negative _ =
    context "negative home tests"

    "do something" &&& fun _ ->
        url address

let all _ =
    positive()
    boundary()
    negative()