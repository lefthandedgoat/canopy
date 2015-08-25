module canopy.configuration
open reporters
open System

//location of drivers depending on OS
let FolderByOSType = 
    if System.Environment.OSVersion.Platform = PlatformID.MacOSX then
        @"/usr/bin/"
    elif  System.Environment.OSVersion.Platform = PlatformID.Unix then
        @"/usr/bin/"
    else
        @"c:\"

//runner related
let failFast = ref false
let mutable failScreenshotPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"

let mutable chromeDir = FolderByOSType
let mutable ieDir = FolderByOSType
let mutable phantomJSDir = FolderByOSType
let mutable safariDir = FolderByOSType
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