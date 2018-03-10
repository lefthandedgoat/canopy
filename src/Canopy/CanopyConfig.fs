namespace Canopy

open System

//// TODO: remove global mutable
//let mutable (failureMessage : string) = null
//
//// TODO: remove global mutable
//let mutable wipTest = false

//misc
(* documented/actions *)
//let failsWith message =
//    // TODO: handle write to global via `Configuration`
//    failureMessage <- message

type CanopyPaths =
    {
        chromeDir: string
        chromiumDir: string
        ieDir: string
        safariDir: string
        edgeDir: string
    }

type CanopyConfig =
    {
        paths: CanopyPaths
        hideCommandPromptWindow: bool
        elementTimeout: TimeSpan
        compareTimeout: TimeSpan
        pageTimeout: TimeSpan
        wipSleep: TimeSpan
        // TODO: consider introducing the facade
        //reporter: IReporter
        suggestOtherSelectors: bool
        autoPinBrowserRightOnLaunch: bool
        throwIfMoreThanOneElement: bool
        finders: Finders.Finders
        optimizeBySkippingIFrameCheck: bool
        optimizeByDisablingClearBeforeWrite: bool
        showInfoDiv: bool
        failureMessage: string option
    }

[<AutoOpen>]
module CanopyConfig =

    //location of drivers depending on OS
    let internal folderByOSType =
        match System.Environment.OSVersion.Platform with
        | PlatformID.MacOSX
        | PlatformID.Unix ->
            @"/usr/local/bin/"
        | _ ->
            @"c:\"

    let internal folderByOSTypeChromium =
        match System.Environment.OSVersion.Platform with
        | PlatformID.MacOSX
        | PlatformID.Unix ->
            @"/usr/lib/chromium-browser"
        | _ ->
            @"c:\"

    let defaultConfig: CanopyConfig =
        {
            paths =
                {
                    chromeDir = folderByOSType
                    chromiumDir = folderByOSTypeChromium
                    ieDir = folderByOSType
                    safariDir = folderByOSType
                    edgeDir = @"C:\Program Files (x86)\Microsoft Web Driver\"
                }
            hideCommandPromptWindow = false
            elementTimeout = TimeSpan.FromSeconds 10.
            compareTimeout = TimeSpan.FromSeconds 10.
            pageTimeout = TimeSpan.FromSeconds 10.
            wipSleep = TimeSpan.FromSeconds 1.
            suggestOtherSelectors = true
            autoPinBrowserRightOnLaunch = true
            throwIfMoreThanOneElement = false
            finders = Finders.defaultFinders
            optimizeBySkippingIFrameCheck = false
            optimizeByDisablingClearBeforeWrite = false
            showInfoDiv = true
            failureMessage = None
        }

    (* documented/configuration *)
    let setHideCommandPromptWindow shouldHide (config: CanopyConfig) =
        { config with hideCommandPromptWindow = shouldHide }

    (* documented/configuration *)
    let setElementTimeout duration (config: CanopyConfig) =
        { config with elementTimeout = duration }

    (* documented/configuration *)
    let setCompareTimeout duration (config: CanopyConfig) =
        { config with compareTimeout = duration }

    (* documented/configuration *)
    let setPageTimeout duration (config: CanopyConfig) =
        { config with pageTimeout = duration }

    (* documented/configuration *)
    let setWIPSleep duration (config: CanopyConfig) =
        { config with wipSleep = duration }

    (* documented/configuration *)
    let setSuggestOtherSelectors enabled (config: CanopyConfig) =
        { config with suggestOtherSelectors = enabled }

    (* documented/configuration *)
    let setAutoPinBrowserRightOnLaunch enabled (config: CanopyConfig) =
        { config with autoPinBrowserRightOnLaunch = enabled }

    (* documented/configuration *)
    let setThrowIfMoreThanOneElement enabled (config: CanopyConfig) =
        { config with throwIfMoreThanOneElement = enabled }

    (* documented/configuration *)
    let setFinders finders (config: CanopyConfig) =
        { config with finders = finders }

    (* documented/configuration *)
    let setOptimizeBySkippingIFrameCheck enabled (config: CanopyConfig) =
        { config with optimizeBySkippingIFrameCheck = enabled }

    (* documented/configuration *)
    let setOptimizeByDisablingClearBeforeWrite enabled (config: CanopyConfig) =
        { config with optimizeByDisablingClearBeforeWrite = enabled }

    (* documented/configuration *)
    let setShowInfoDiv enabled (config: CanopyConfig) =
        { config with showInfoDiv = enabled }

    // PATHS:

    (* documented/configuration *)
    let setChromeDir path (config: CanopyConfig) =
        { config with paths = { config.paths with chromeDir = path } }

    (* documented/configuration *)
    let setChromiumDir path (config: CanopyConfig) =
        { config with paths = { config.paths with chromiumDir = path } }

    (* documented/configuration *)
    let setIEDir path (config: CanopyConfig) =
        { config with paths = { config.paths with ieDir = path } }

    (* documented/configuration *)
    let setSafariDir path (config: CanopyConfig) =
        { config with paths = { config.paths with safariDir = path } }

    (* documented/configuration *)
    let setEdgeDir path (config: CanopyConfig) =
        { config with paths = { config.paths with edgeDir = path } }
