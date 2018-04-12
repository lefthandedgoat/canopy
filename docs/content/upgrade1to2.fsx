(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../src/canopy/bin/Release/netstandard2.0"
#r "canopy.dll"

open canopy.classic
open canopy.types
open canopy.runner.classic
open System

(**
Upgrade 1.x to 2.x
========================
*)

(**
Breaking changes
-----
* Removed coverage report support, made move to 2.0 more difficult and I don't think they were used
* Removed phantomjs support, the project is no longer updated because of chrome headless 
* Removed writeToSelectWithOptionValue, old backwards compat flag
* Removed optimizeByDisablingCoverageReport, old optimization around coverage reports
* Changed default driver path from c:\ to executing directory
*)

(**
Namespace changes
-----
```
canopy        -> canopy.classic
canopy.runner -> canopy.runner.classic //no longer auto opened

canopy.configuration //no longer auto opened
canopy.reporters
canopy.screen
canopy.types         //no longer auto opened
canopy.wait          //no longer auto opened
canopy.finders
canopy.history
canopy.userAgents

canopy.parallell.functions //new
canopy.parallell.instanced //new
```
*)

(**
Signature changes
-----
* The signature for custom finders now requires an instance of IWebDriver (the browser) also
* From  ```string -> (By -> ReadOnlyCollection<IWebElement>) -> IWebElement list```
* To ```string -> (By -> ReadOnlyCollection<IWebElement>) -> IWebDriver -> IWebElement list```
*)

(**
Screen management changes
-----
* Due to the upgrade to support .net Core canopy can no longer determine your resolution
* You will need to manually set the resolutions so things like `pin` work correctly

```
canopy.screen.screenWidth <- 3840  //default 1920
canopy.screen.screenHeight <- 2160 //default 1080
canopy.screen.monitorCount <- 2    //default 1
```
*)

(**
New features
-----
* .net Standard 2.0 support
* Added parallel support in two styles in two new namespaces
*)

(**
Parallel testing
-----
* Added parallel support in two styles in two new namespaces
```
canopy.parallell.functions
canopy.parallell.instanced
```
* See examples of function based tests: https://github.com/lefthandedgoat/canopy/blob/master/tests/paralleltests/functionsTests.fs
* See examples of instanced based tests: https://github.com/lefthandedgoat/canopy/blob/master/tests/paralleltests/instancedTests.fs

* `canopy.runner.classic` does NOT support parallel tests.
* You should choose a unit test library that you like and supports this and write canopy tests in it
* In the future I may release a parallel friendly test runner for canopy that includes some features from the classic runner
* Parallel test running is HARD.  Your database is a global mutable variable and your tests may interfere with each other 
* You will also need to understand how your test runner runs tests, as it may not run them in order
*)

(**
Upgrading from .net 4.5.2
-----
* In an real world code base I simply updated the project from 4.5.2 to 4.6.1, installed the .net core SDK, and it worked
* If you have problems please let me know in the github issues and I will see what I can do to repro and help
*)