module Canopy.ClassicMode

open Canopy
open Canopy.Runner

/// :-s
let mutable browser = null

do Context.contextChanged.Add <| fun context ->
    match context.browser with
    | Some b ->
        browser <- b
    | None ->
        browser <- null

let failsWith message =
    failureMessage <- Some message

let (<<) item text = write text item
let (-->) source target = drag source target
let ( !^ ) (u: string) = url u

type suite = Suite
type direction = Direction

module userAgents =
    let iPhone = UserAgents.iPhone
    let iPad = UserAgents.iPad
    let GalaxyNexus = UserAgents.GalaxyNexus

module screenSizes =
    let iPhone4 = Screen.Sizes.iPhone4
    let iPhone5 = Screen.Sizes.iPhone5
    let iPad = Screen.Sizes.iPad
    let Nexus4 = Screen.Sizes.Nexus4
    let Nexus7 = Screen.Sizes.Nexus7

module Assert =
    open Canopy.Expect

    (* documented/assertions *)
    let equal item value =
        elementHasText (context ()) value item

    (* documented/assertions *)
    let notEqual cssSelector value =
        noElementHasText (context ()) value cssSelector
