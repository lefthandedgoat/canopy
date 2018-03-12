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
open Canopy.Logging
open Canopy.ParallelUsage
open Canopy.ParallelUsage.Pages
open System
// TO HERE!

let init () =
    let configure =
        CanopyConfig.setOptimizeByDisablingClearBeforeWrite false
        >> CanopyConfig.setOptimizeBySkippingIFrameCheck true
        >> CanopyConfig.setSuggestOtherSelectors true
        >> CanopyConfig.setThrowOnStatics true
        >> CanopyConfig.setElementTimeout (TimeSpan.FromSeconds 2.)
        >> CanopyConfig.setLogLevel Debug
    let config = configure defaultConfig
    let firefoxWithOpts =
        let o = FirefoxOptions(AcceptInsecureCertificates = Nullable true)
        FirefoxWithOptions o
    let b = startWithConfigPure config firefoxWithOpts
    Context.createT (Some b) config ()

let x = init ()

x.url "https://qvitoo.dev:8080"

let signup () =
    x.click (x.element ".button.signup")
    x.write "Test User 1" "#name"
    x.write "test5@example.com" "#email"
    x.press tab
    x.press Keys.Space
    x.press tab
    x.press Keys.Enter
    x.element ".view-heading"
        |> Expect.elementHasText x "inbox"
        // why is this not crashing??
    

// lastly:
quit x.browser