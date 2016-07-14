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
(* TODO/documented/configuration *)
let failFast = ref false
(* TODO/documented/configuration *)
let mutable failScreenshotPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
(* TODO/documented/configuration *)
let mutable failScreenshotFileName = fun (test : types.Test) (suite: types.suite) -> DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")

(* documented/configuration *)
let mutable chromeDir = folderByOSType
(* documented/configuration *)
let mutable ieDir = folderByOSType
(* documented/configuration *)
let mutable phantomJSDir = folderByOSType
(* documented/configuration *)
let mutable safariDir = folderByOSType
(* TODO/documented/configuration *)
let mutable edgeDir = @"C:\Program Files (x86)\Microsoft Web Driver\"

(* documented/configuration *)
let mutable elementTimeout = 10.0
(* documented/configuration *)
let mutable compareTimeout = 10.0
(* documented/configuration *)
let mutable pageTimeout = 10.0
(* documented/configuration *)
let mutable wipSleep = 1.0
(* documented/configuration *)
let mutable runFailedContextsFirst = false
(* documented/configuration *)
let mutable reporter : IReporter = new ConsoleReporter() :> IReporter
(* documented/configuration *)
let mutable disableSuggestOtherSelectors = false
(* documented/configuration *)
let mutable autoPinBrowserRightOnLaunch = true
(* documented/configuration *)
let mutable throwIfMoreThanOneElement = false
(* documented/configuration *)
let mutable configuredFinders = finders.defaultFinders
(* documented/configuration *)
let mutable writeToSelectWithOptionValue = true
(* documented/configuration *)
let mutable optimizeBySkippingIFrameCheck = false
(* documented/configuration *)
let mutable optimizeByDisablingCoverageReport = false
(* documented/configuration *)
let mutable optimizeByDisablingClearBeforeWrite = false
(* documented/configuration *)
let mutable showInfoDiv = true
(* TODO/documented/configuration *)
let mutable failureScreenshotsEnabled = true
(* TODO/documented/configuration *)
let mutable skipAllTestsOnFailure = false
(* TODO/documented/configuration *)
let mutable skipRemainingTestsInContextOnFailure = false
(* TODO/documented/configuration *)
let mutable skipNextTest = false
(* TODO/documented/configuration *)
let mutable failureMessagesThatShoulBeTreatedAsSkip : string list = []
