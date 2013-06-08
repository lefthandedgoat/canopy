// Learn more about F# at http://fsharp.net

open canopy
open runner

start firefox

let page = "http://skalinets.github.io/canopy/demo/amir.html"

context "demo tests"
before (fun _ -> url page
                 on page)

test(fun _ ->
    describe "fill out a form"
    "#firstName" << "Minnie"
    "#lastName" << "Mouse"
    "#dob" << "01/02/1942"
    
    click "#genderFemale"
    
    "#address" << "The Big Screen"
    "#city" << "Holywood"
    "#state" << "CA"
    "#zip" << "91601"
    
    click ".optin input"
    click ".agree input"

    "#address" == "The Big Screen"
    "#city" == "Holywood"
    "#state" == "CA"
    "#zip" == "91601"
)

run ()

quit ()
