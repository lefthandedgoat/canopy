module canopy.classic.configuration

open canopy.reporters
open canopy.types
open System

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

let firefoxByOSType =
    match System.Environment.OSVersion.Platform with
    | PlatformID.MacOSX
    | PlatformID.Unix ->
        if System.IO.File.Exists(@"/Applications/Firefox.app/Contents/MacOS/firefox-bin")
        then @"/Applications/Firefox.app/Contents/MacOS/firefox-bin" //osx
        else @"/usr/lib/firefox-2.0" //linux, unsure of correct path
    | _ -> @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe"

//runner related
(* documented/configuration *)
let failFast = ref false
(* documented/configuration *)
let mutable failScreenshotPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
(* documented/configuration *)
let mutable failScreenshotFileName = fun (test : canopy.types.Test) (suite: canopy.types.suite) -> DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")

(* documented/configuration *)
let mutable chromeDir = folderByOSType
(* documented/configuration *)
let mutable chromiumDir = folderByOSTypeChromium
(* documented/configuration *)
let mutable ieDir = folderByOSType
(* documented/configuration *)
let mutable safariDir = folderByOSType
(* documented/configuration *)
let mutable edgeDir = @"C:\Program Files (x86)\Microsoft Web Driver\"
(* documented/configuration *)
let mutable hideCommandPromptWindow = false
(* TODO documented/configuration *)
let mutable firefoxDriverDir = folderByOSType
(* TODO documented/configuration *)
let mutable firefoxDir = firefoxByOSType

(* documented/configuration *)
let mutable elementTimeout = 10.0
(* documented/configuration *)
let mutable compareTimeout = 10.0
(* documented/configuration *)
let mutable pageTimeout = 10.0
(* documented/configuration *)
let mutable wipSleep = 1.0
(* documented/configuration *)
let mutable failIfAnyWipTests = false
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
let mutable configuredFinders = canopy.finders.defaultFinders
(* documented/configuration *)
let mutable optimizeBySkippingIFrameCheck = false
(* documented/configuration *)
let mutable optimizeByDisablingClearBeforeWrite = false
(* documented/configuration *)
let mutable showInfoDiv = true
(* documented/configuration *)
let mutable failureScreenshotsEnabled = true
(* documented/configuration *)
let mutable skipAllTestsOnFailure = false
(* documented/configuration *)
let mutable skipRemainingTestsInContextOnFailure = false
(* documented/configuration *)
let mutable failureMessagesThatShoulBeTreatedAsSkip : string list = []

//do not touch
let mutable wipTest = false
