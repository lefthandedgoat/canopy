// Learn more about F# at http://fsharp.net

open canopy
open runner

start firefox

let page = "http://lefthandedgoat.github.com/canopy/demo/amir.html"

context "demo tests"
before (fun _ -> url page
                 on page)

test(fun _ ->
    describe "fill out a form"
    "#firstName" << "Mickey"
    "#lastName" << "Mouse"
    "#dob" << "01/02/1942"
    click "#genderFemale"
    "#address" << "The Big Screen"
    "#city" << "Holywood"
    "#state" << "CA"
    "#zip" << "91601"
    click ".checkbox"
)

run ()
