module Canopy.ClassicMode

open Canopy

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
