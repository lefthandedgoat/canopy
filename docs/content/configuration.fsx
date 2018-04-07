(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"
#r "canopy.dll"
#r "WebDriver.dll"
open canopy
open canopy.configuration


(**
Configuration
========================
*)

(**
driverHostName
--
* .net core has a delay when resolving localhost that makes using canopy very slow
* canopy uses 127.0.0.1 instead but allows you to adjust it based on your ipv/ipv6 needs
* Defaults to "127.0.0.1"
*)
driverHostName <- "localhost"
driverHostName <- "::1"

(**
chromeDir
--
* Directory for the chromedriver
* Defaults to pre-set OS paths
* OSX: /usr/local/bin/
* Windows: C:\
*)
chromeDir <- "C:\\"

(**
chromiumDir
--
* Directory for the chromedriver for use with chromium
* Defaults to pre-set OS paths
* nix: /usr/lib/chromium-browser
* Windows: C:\
*)
chromiumDir <- "C:\\"

(**
firefoxDir
--
* Install path for firefox
* Defaults to pre-set OS paths
* OSX: /Applications/Firefox.app/Contents/MacOS/firefox-bin
* nix: /usr/lib/firefox-2.0 (probably wrong)
* Windows: C:\Program Files (x86)\Mozilla Firefox\firefox.exe
*)
firefoxDir <- @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe"


(**
firefoxDriverDir
--
* Directory for the firefox gecko driver
* Defaults to pre-set OS paths
* OSX: /usr/local/bin/
* Windows: C:\
*)
firefoxDriverDir <- "C:\\"

(**
ieDir
--------
* Directory for Internet Explorer
* Defaults to pre-set OS paths
* OSX: /usr/bin/
* Windows: C:\
*)
ieDir <- "C:\\"

(**
safariDir
-----------
* Directory for Safari
* Defaults to pre-set OS paths
* OSX: /usr/bin/
* Windows: C:\
*)
safariDir <- "C:\\"

(**
edgeDir
-----------
* Directory for edge driver
* Defaults to C:\Program Files (x86)\Microsoft Web Driver\
*)
edgeDir <- "C:\\"

(**
hideCommandPromptWindow
-----------
* Hide drivers command prompt window
* Defaults to false
*)
hideCommandPromptWindow <- true

(**
elementTimeout
-------------------
* Amount of time for the test runner to search for an element.
* Default is 10.0 seconds
*)
elementTimeout <- 10.0

(**
compareTimeout
-----------------------
* Amount of time for the test runner to spend comparing elements.
* Default is 10.0 seconds
*)
compareTimeout <- 10.0

(**
pageTimeout
-----------------
* Amount of time for the test runner to wait for the page to load.
* Default is 10.0 seconds
*)
pageTimeout <- 10.0

(**
wipSleep
--------
* Amount of time to spend between WIP tests (Tests marked with &&&&)
* Default is 1.0 seconds
*)
wipSleep <- 1.0

(**
failIfAnyWipTests
--------
* Prevents accidentally allowing wip tests into the build pipeline.
* Set to false locally so tests under development are not affected.
* Set to true in your CI environment to catch wip tests that have been mistakenly commited to trunk/master.
* Default is false
*)
failIfAnyWipTests <- true

(**
runFailedContextsFirst
-----
* Runs failed contexts first if the test suite has already executed.
* Defaults is false
*)
runFailedContextsFirst <- false

(**
failFast
-----
* Stop running test suite after one test fails.
* Defaults is false
*)
failFast := true

(**
failScreenshotPath
-----
* The path to save screenshots that are taken on failure.
* Defaults is AppData\canopy
*)
failScreenshotPath <- "C:\path\to\screenshot\folder"

(**
failScreenshotFileName
-----
* Function that is run to create the filename of screenshot.
* Takes a test and a suite to be optionally used in name creation.
* Defaults is Timestamp with format MMM-d_HH-mm-ss-fff
*)
failScreenshotFileName <- fun test suite -> System.DateTime.Now.ToString("MMM-d_HH-mm")

(**
reporter
----------------
* Reporter object that will handle how logs should be stored.
* Must inherit IReporter
* Default is ConsoleReporter
*)
reporter <- new reporters.ConsoleReporter() :> reporters.IReporter

(**
disableSuggestOtherSelectors
----------------------------
* Option that will disable selector suggestion if a selector fails to execute
* Defaults is false
*)
disableSuggestOtherSelectors <- false

(**
autoPinBrowserRightOnLaunch
---------
* Automatically pins the browser to the right of the screen on launch
* Default is true
*)
autoPinBrowserRightOnLaunch <- true

(**
throwIfMoreThanOneElement
------------
* Throws a CanopyMoreThanOneElementFoundException if more than one element is found using a selector
* Default is false
*)
throwIfMoreThanOneElement <- false

(**
configuredFinders
------------
* Defines functions for finding elements based on selectors
* Default is the following sequence
* findByCss
* findByValue
* findByXpath
* findByLabel
* findByText
* findByJQuery
*)
configuredFinders <- finders.defaultFinders

(**
optimizeBySkippingIFrameCheck
------------
* If you need your tests to be faster and don't have any iframes you can turn this to true
* Default is false
*)
optimizeBySkippingIFrameCheck <- false

(**
showInfoDiv
------------
* Allows information to be displayed on the browser when the puts function is called
* Default is true
*)
showInfoDiv <- true

(**
failureScreenshotsEnabled
------------
* Enables/Disables automatic taking of screenshot on failures.
* Default is true
*)
failureScreenshotsEnabled <- false

(**
skipAllTestsOnFailure
------------
* Like fail fast, but instead of not running tests, it skips them.
* Default is false
*)
skipAllTestsOnFailure <- true

(**
skipRemainingTestsInContextOnFailure
------------
* Like skipAllTestsOnFailure, but only skip those in the current context.
* Default is false
*)
skipRemainingTestsInContextOnFailure <- true

(**
skipNextTest
------------
* Skip the next test
* Default is false
*)
skipNextTest <- true

(**
failureMessagesThatShoulBeTreatedAsSkip
------------
* Mark a failed test as skipped if it failed for any of the listed known reasons
* Default is empty list
*)
failureMessagesThatShoulBeTreatedAsSkip <- ["message 1"; "message 2"]
