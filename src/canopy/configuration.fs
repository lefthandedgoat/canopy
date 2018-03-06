[<AutoOpen>]
module canopy.configuration

open reporters
open System

//runner related
// TODO: remove global variable
(* documented/configuration *)
let failFast = ref false
// TODO: remove global variable
(* documented/configuration *)
let mutable failScreenshotPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
// TODO: remove global variable
(* documented/configuration *)
let mutable failScreenshotFileName = fun (test : types.Test) (suite: types.suite) -> DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
// TODO: remove global variable
(* documented/configuration *)
let mutable failIfAnyWipTests = false
// TODO: remove global variable
(* documented/configuration *)
let mutable runFailedContextsFirst = false
// TODO: remove global variable
(* documented/configuration *)
let mutable failureScreenshotsEnabled = true
// TODO: remove global variable
(* documented/configuration *)
let mutable skipAllTestsOnFailure = false
// TODO: remove global variable
(* documented/configuration *)
let mutable skipRemainingTestsInContextOnFailure = false
// TODO: remove global variable
(* documented/configuration *)
let mutable skipNextTest = false
// TODO: remove global variable
(* documented/configuration *)
let mutable failureMessagesThatShoulBeTreatedAsSkip : string list = []

type CanopyRunnerConfig =
    {
        failFast: bool
        failScreenshotPath: string
        failScreenshotFileName: string
        failIfAnyWIPTests: bool
        runFailedContextsFirst: bool
        // remaining...
    }


//location of drivers depending on OS
let folderByOSType =
    match System.Environment.OSVersion.Platform with
    | PlatformID.MacOSX
    | PlatformID.Unix -> @"/usr/local/bin/"
    | _ -> @"c:\"

let folderByOSTypeChromium =
    match System.Environment.OSVersion.Platform with
    | PlatformID.MacOSX
    | PlatformID.Unix -> @"/usr/lib/chromium-browser"
    | _ -> @"c:\"

// TODO: remove global variable
(* documented/configuration *)
let mutable chromeDir = folderByOSType
// TODO: remove global variable
(* documented/configuration *)
let mutable chromiumDir = folderByOSTypeChromium
// TODO: remove global variable
(* documented/configuration *)
let mutable ieDir = folderByOSType
// TODO: remove global variable
(* documented/configuration *)
let mutable safariDir = folderByOSType
// TODO: remove global variable
(* documented/configuration *)
let mutable edgeDir = @"C:\Program Files (x86)\Microsoft Web Driver\"

type CanopyConfig =
    {
        hideCommandPromptWindow: bool
        elementTimeout: TimeSpan
        compareTimeout: TimeSpan
        pageTimeout: TimeSpan
        wipSleep: TimeSpan
        reporter: IReporter
        disableSuggestOtherSelectors: bool
        autoPinBrowserRightOnLaunch: bool
        throwIfMoreThanOneElement: bool
        configuredFinders: finders.Finders
        writeToSelectWithOptionValue: bool
        optimizeBySkippingIFrameCheck: bool
        optimizeByDisablingClearBeforeWrite: bool
        showInfoDiv: bool
    }

// TODO: remove global variable
(* documented/configuration *)
let mutable hideCommandPromptWindow = false

// TODO: remove global variable
(* documented/configuration *)
let mutable elementTimeout = 10.0
// TODO: remove global variable
(* documented/configuration *)
let mutable compareTimeout = 10.0
// TODO: remove global variable
(* documented/configuration *)
let mutable pageTimeout = 10.0
// TODO: remove global variable
(* documented/configuration *)
let mutable wipSleep = 1.0
// TODO: remove global variable
(* documented/configuration *)
let mutable reporter = new ConsoleReporter() :> IReporter
// TODO: remove global variable
(* documented/configuration *)
let mutable disableSuggestOtherSelectors = false
// TODO: remove global variable
(* documented/configuration *)
let mutable autoPinBrowserRightOnLaunch = true
// TODO: remove global variable
(* documented/configuration *)
let mutable throwIfMoreThanOneElement = false
// TODO: remove global variable
(* documented/configuration *)
let mutable configuredFinders = finders.defaultFinders
// TODO: remove global variable
(* documented/configuration *)
let mutable writeToSelectWithOptionValue = true
// TODO: remove global variable
(* documented/configuration *)
let mutable optimizeBySkippingIFrameCheck = false
// TODO: remove global variable
(* documented/configuration *)
let mutable optimizeByDisablingClearBeforeWrite = false
// TODO: remove global variable
(* documented/configuration *)
let mutable showInfoDiv = true