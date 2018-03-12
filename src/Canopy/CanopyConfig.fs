namespace Canopy

open System
open Canopy.Logging

//misc
(* documented/actions *)
/// For chrome you need to download chromedriver.exe from
/// http://code.google.com/p/chromedriver/wiki/GettingStarted
///
/// Place chromedriver.exe in c:\ or you can place it in a custom location and
/// set the chromeDir field.
///
/// For IE you need to set `Settings -> Advance -> Security Section ->
/// Check-Allow active content` to run files on My Computer* also download
/// IEDriverServer and place in c:\ or configure with ieDir.
///
/// Firefox just works.
///
/// Safari: download it and put in c:\ or configure with safariDir
///
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
        /// How long to wait until a function is retried against the DOM.
        wipSleep: TimeSpan
        logger: Logger
        suggestOtherSelectors: bool
        autoPinBrowserRightOnLaunch: bool
        throwIfMoreThanOneElement: bool
        finders: Finders.Finders
        optimizeBySkippingIFrameCheck: bool
        optimizeByDisablingClearBeforeWrite: bool
        showInfoDiv: bool
        /// Whether to throw an exception is a global static variable is queried
        /// by the DSL; enable this when you run with Expecto to support parallel
        /// tests.
        throwOnStatics: bool
        /// What level to log static accesses with.
        logLevelOnStatics: LogLevel
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
            logger = Log.create "Canopy"
            suggestOtherSelectors = true
            autoPinBrowserRightOnLaunch = true
            throwIfMoreThanOneElement = false
            finders = Finders.defaultFinders
            optimizeBySkippingIFrameCheck = false
            optimizeByDisablingClearBeforeWrite = false
            showInfoDiv = true
            throwOnStatics = false
            logLevelOnStatics = Info
        }

    (* documented/configuration *)
    let setLogLevel logLevel (config: CanopyConfig) =
        { config with logger = Targets.create logLevel [| "Canopy" |] }

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
    let setThrowOnStatics enabled (config: CanopyConfig) =
        { config with throwOnStatics = enabled }

    (* documented/configuration *)
    let setWarnLevelOnStatics logLevel (config: CanopyConfig) =
        { config with logLevelOnStatics = logLevel }

    (* documented/configuration *)
    let setFinders finders (config: CanopyConfig) =
        { config with finders = finders }

    (* documented/configuration *)
    let addFinder finder (config: CanopyConfig) =
        let composed cssSelector f =
            config.finders cssSelector f
            |> Seq.append (seq { yield finder cssSelector f })
        { config with finders = composed }

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

    type CanopyConfig with
        member internal x.configureOp name fn: WaitOp<'ok, 'error> =
            let xA =
                async {
                    return fn ()
                }
            WaitOp<_, _>.create xA name x.logger x.compareTimeout x.wipSleep
