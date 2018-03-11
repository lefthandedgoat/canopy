#!/usr/bin/env fsharpi

// IMPORT FROM HERE AND DOWN
#I "bin/Debug"
#r "Argu"
#r "WebDriver"
open OpenQA.Selenium
open OpenQA.Selenium.Firefox
#r "Canopy"
#r "Expecto"
#r "Expecto.FsCheck"
#load "Expecto.fs"
open Expecto
#load "Pages.fs"
open Canopy
open Canopy.ParallelUsage
open Canopy.ParallelUsage.Pages
open System
// TO HERE!

let init () =
    let firefoxWithOpts =
        let o = FirefoxOptions(AcceptInsecureCertificates = Nullable true)
        FirefoxWithOptions o
    let b = startWithConfigPure defaultConfig firefoxWithOpts
    Context.createT (Some b) defaultConfig ()

let x = init ()

x.url "https://qvitoo.dev:8080"
x.click (x.element ".button.signup")
x.write "#name" "Test User 1"
x.write "#email" "test@example.com"

// lastly:
quit ()