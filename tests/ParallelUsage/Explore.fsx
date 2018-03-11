#!/usr/bin/env fsharpi

// IMPORT FROM HERE AND DOWN
#I "bin/Debug"
#r "Argu"
#r "WebDriver"
#r "Canopy"
#r "Expecto"
#r "Expecto.FsCheck"
#load "Config.fs"
#load "Expecto.fs"
#load "Pages.fs"
open Canopy
open Canopy.ParallelUsage
open Canopy.ParallelUsage.Pages
// TO HERE!

let init () =
    let b = start firefox
    Context.create Config.empty browser
let x =  init ()

x.url "https://staging.qvitoo.com"

// lastly:
quit ()