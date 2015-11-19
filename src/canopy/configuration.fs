[<AutoOpen>]
module canopy.configuration
open reporters
open System

//location of drivers depending on OS
let folderByOSType = 
    match System.Environment.OSVersion.Platform with
    | PlatformID.MacOSX 
    | PlatformID.Unix -> @"/usr/bin/"
    | _ -> @"c:\"

//runner related
let failFast = ref false
let mutable failScreenshotPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
let mutable failScreenshotFileName = fun (test : types.Test) (suite: types.suite) -> DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")

let mutable chromeDir = folderByOSType
let mutable ieDir = folderByOSType
let mutable phantomJSDir = folderByOSType
let mutable safariDir = folderByOSType
let mutable elementTimeout = 10.0
let mutable compareTimeout = 10.0
let mutable pageTimeout = 10.0
let mutable wipSleep = 1.0
let mutable runFailedContextsFirst = false
let mutable reporter : IReporter = new ConsoleReporter() :> IReporter
let mutable disableSuggestOtherSelectors = false
let mutable autoPinBrowserRightOnLaunch = true
let mutable throwIfMoreThanOneElement = false
let mutable configuredFinders = finders.defaultFinders
let mutable writeToSelectWithOptionValue = true
let mutable optimizeBySkippingIFrameCheck = false
let mutable optimizeByDisablingCoverageReport = false
let mutable showInfoDiv = true
let mutable failureScreenshotsEnabled = true
let mutable skipAllTestsOnFailure = false
let mutable skipRemainingTestsInContextOnFailure = false
let mutable skipNextTest = false