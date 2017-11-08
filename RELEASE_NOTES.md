#### 0.7.6 - May 26 2013
* Updated to Selenium 2.33.0 ([change log](http://selenium.googlecode.com/git/dotnet/CHANGELOG))
* (Breaking) Changed >> (drag) to --> it was overriding the f# function. << (write) stays
* Added pin FullScreen
* Updated documentation with more forms of element/elements

#### 0.7.7 - June 8 2013
* Fixed bug where text area's value was not being read correctly. [Pull Request](https://github.com/lefthandedgoat/canopy/pull/80)
* (Breaking) Changed how start (browser) works.  If you had 'start "firefox"' before, you need to change it from the string "firefox" to just firefox.  You can also start browsers with profiles now. [Commit](https://github.com/lefthandedgoat/canopy/commit/f7bd2cbc8352a1cd252916d1953cd5914df13aa0)

#### 0.7.8 - July 26 2013
* (Breaking) Deprecated HtmlReporter.  It was incomplete and not being worked on.
* LiveHtmlReporter now pins its browser to the left
* 'coverage' now accepts a url or unit.  Providing unit will create a coverage report of the page you are currently on
* '<<' (write) performance on select/options improved significantly

#### 0.7.9 - August 8 2013
* Updated to Selenium 2.34.0. (http://selenium.googlecode.com/git/dotnet/CHANGELOG)
* Added color to stack traces to point you at the first line of your code to help debugging.
* Fixed problem looping over multiple elements when trying to write, and one or more of them failing to write.
* Fixed problem looping over multiple elements when trying to click, and one or more of them failing to click.
* Fixed problem with other contexts continuing to run when failfast was enabled.  All subsequent tests/contexts do not run.
* Fixed problem with dealing with alerts not being reliable.

#### 0.8.0 - August 29 2013
* Updated to Selenium 2.35.0. (http://selenium.googlecode.com/git/dotnet/CHANGELOG)
* Added phantomJS as start option [Issue #94](https://github.com/lefthandedgoat/canopy/issues/94)
* read, clear, selected, and deselected can now take a cssSelector/xpath/etc or an IWebElement [Issue #93](https://github.com/lefthandedgoat/canopy/issues/93)

#### 0.8.1 - Septemer 2 2013
* Added phantomJSProxyNone as start option to work around phantom performance problem [Issue #94](https://github.com/lefthandedgoat/canopy/issues/94)
* When using PhantomJS, no longer auto pin browser as it causes a slow down [Issue #96] (https://github.com/lefthandedgoat/canopy/issues/96)

#### 0.8.2 - Septemer 2 2013
* Added several methods to LiveHtmlReport class allowing access to the report browser, execution of javascript, accessing the html of the report and saving the html [Issue #97](https://github.com/lefthandedgoat/canopy/issues/97)

#### 0.8.3 - Septemer 3 2013
* notDisplayed now correctly passes when checking for an element that does not exist [Issue #99](https://github.com/lefthandedgoat/canopy/issues/99)
* jquery and sizzle css selectors are now supported (:not, :checked, :selected etc...) [Pull Request #98](https://github.com/lefthandedgoat/canopy/pull/98)

#### 0.8.4 - Septemer 4 2013
* (Breaking) renamed reportPath to reportTemplateUrl when adding a new feature.  New feature will automatically save a report to specified path on quit() when using LiveHtmlReporter [Pull Request #103](https://github.com/lefthandedgoat/canopy/pull/103)

#### 0.8.5 - Septemer 7 2013
* (Breaking) previously element/elementWithin would return the first satisfactory element.  someElementWithin would throw an exception if there was more than one element.  Now all three will return first element.  If you want it to throw an exception if more than one is found, set 'throwIfMoreThanOneElement' configuration setting to true [Issue #105](https://github.com/lefthandedgoat/canopy/issues/105)
* sleep now accepts floats instead of integers [Pull Request #106](https://github.com/lefthandedgoat/canopy/pull/106)

#### 0.8.6 - Septemer 11 2013
* displayed and notDisplayed can now take a cssSelector/xpath/etc or an IWebElement [Issue #93](https://github.com/lefthandedgoat/canopy/issues/93)

#### 0.8.7 - Septemer 20 2013
* (Semi-Breaking) on check for url is now smarter. The original intention for using contains instead of equality was to disregard query strings.  That is now done more intelligently [Pull Request #107](https://github.com/lefthandedgoat/canopy/pull/107)

#### 0.8.8 - October 7 2013
* Improvements to on [Pull Request #108](https://github.com/lefthandedgoat/canopy/pull/108)

#### 0.9.0 - December 15 2013
* Updated to Selenium 2.38.0
* Pulled in IPad/IPhone user agent support
* General updates/improvements

#### 0.9.2 - January 23 2014
* Made the browser used for the html reporter configureable [Issue #126](https://github.com/lefthandedgoat/canopy/issues/126)
* (Semi-Breaking) Moved all the types to a new module: canopy.types.  You may need to add an open statement if you are referencing any of those types.

#### 0.9.3 - January 27 2014
* Fixed bug in html reporter if your test name had single ticks in it [Issue #127](https://github.com/lefthandedgoat/canopy/issues/127)
* Fix for #127 took a dependency on System.Web so make sure your canopy projects are not Client Profiles!
* (Semi-Breaking) Fixed [Issue #85] (https://github.com/lefthandedgoat/canopy/issues/85) 'element' and 'elements' are now reliable, meaning that they will try over and over until an element exists.
* Added 'unreliableElements' which has the same behavior that 'elements' had if you relied on it
* Fixed [Issue #91](https://github.com/lefthandedgoat/canopy/issues/91) so that there are better error messages.  Suggestions for mistyped selectors is now back and improved!
* Fixed [Issue #128](https://github.com/lefthandedgoat/canopy/issues/128).  You can now add finders to canopy and it will search for things by you via your defined conventions.  Check the bottom of the basicTests/Program.fs for an example.

#### 0.9.5 - February 7 2014
* Fixed subtle bugs in some actions [Issue #130](https://github.com/lefthandedgoat/canopy/issues/130)

#### 0.9.8 - March 31 2014
* Fixed bug when a modal was open and would cause an error, would cause the runner to fail [Issue #135](https://github.com/lefthandedgoat/canopy/issues/135)
* Added `waitFor2` which takes a message and function and uses the message as the error message if waitFor fails [Issue #136](https://github.com/lefthandedgoat/canopy/issues/136)
* Fixed problem where canopy would not work with VS 2010 [Issue #137](https://github.com/lefthandedgoat/canopy/issues/137)
* Updated to Selenium 2.41 [Issue #138](https://github.com/lefthandedgoat/canopy/issues/138)
* Now built in Release mode [Issue #139](https://github.com/lefthandedgoat/canopy/issues/139)
* Added support for Firefox Aurora (nightly build) [Issue #141](https://github.com/lefthandedgoat/canopy/issues/141)

#### 0.9.10 - May 26 2014
* Accepted @rachelreese refactor.  Thanks Rachel! [Issue #143](https://github.com/lefthandedgoat/canopy/issues/143)
* Improved behaviour of << for writing to options to now also look by options value [Issue #144](https://github.com/lefthandedgoat/canopy/issues/144)
* Removed redundant Sizzle finder [Issue #145](https://github.com/lefthandedgoat/canopy/issues/145)
* Updated to the lastest version of SizSelCsZzz [Issue #146](https://github.com/lefthandedgoat/canopy/issues/146)
* Accepted reporter bug fix by soerennielsen.  Thanks! [Issue #150](https://github.com/lefthandedgoat/canopy/issues/150)
* Accepted improvement to << by soerennielsen. Thanks! [Issue #151](https://github.com/lefthandedgoat/canopy/issues/151)

#### 0.9.11 - May 28 2014
* Upgraded to Selenium 2.42 [Issue #152](https://github.com/lefthandedgoat/canopy/issues/152)
* Chrome is now launched with --test-type flag to hide warning message of deprecated flags [Issue #153](https://github.com/lefthandedgoat/canopy/issues/153)

#### 0.9.12 - July 2 2014
* Accepted TeamCity Report fix by mattsonlyattack and incorporated other fixes to TC Reporter.  Thanks! [Issue #154](https://github.com/lefthandedgoat/canopy/issues/154)

#### 0.9.13 - August 10 2014
* Fix issue with iframes [Issue #161](https://github.com/lefthandedgoat/canopy/issues/161)
* Added sugar for Remote web driver [Issue #159](https://github.com/lefthandedgoat/canopy/issues/159)

#### 0.9.14 - September 7 2014
* Added the ability to provide hints to a selector to say what finder to use.  Helps with performance in some scenarios.  Also added two configuration options to help with performance.  One to turn off implicit iFrame searching and one to turn off logging used for coverage report. [Issue #163](https://github.com/lefthandedgoat/canopy/issues/163)
* Fixed an issue with Team City reporter character escaping

#### 0.9.15 - September 17 2014
* Updated to latest Selenium for issue [Issue #166](https://github.com/lefthandedgoat/canopy/issues/166)

#### 0.9.16 - September 28 2014
* Updated to latest Selenium 2.43.1
* Made path for failed screen shots mutable.  Thanks soerennielsen! [Pull Request #168](https://github.com/lefthandedgoat/canopy/pull/168)

#### 0.9.17 - October 20 2014
* Added back and forward via pull request from @JonCanning, thanks! [Pull Request #169](https://github.com/lefthandedgoat/canopy/pull/169)
* Fixed issue with using select/options in iframes, thanks @lokki [Issue #170](https://github.com/lefthandedgoat/canopy/issues/170)

#### 0.9.18 - December 8 2014
* Added switchToTab and closeTab [Issue #165](https://github.com/lefthandedgoat/canopy/issues/165)
* Added hover [Issue #172](https://github.com/lefthandedgoat/canopy/issues/172)
* Updated to latest Selenium 2.44.0

#### 0.9.19 - January 22 2015
* Exceptions blowing up 'once' now fixed, thanks ludekcakl! [Issue #178](https://github.com/lefthandedgoat/canopy/issues/178)
* Improvements to team city reporting enabling parallel reporting, thanks again ludekcakl! [Pull Request #180] (https://github.com/lefthandedgoat/canopy/pull/180)

#### 0.9.20 - February 10 2015
* Improvements to pin screen for multi monitors
* Switched FSharp.Core nuget packages

#### 0.9.21 - February 10 2015
* Changed nuget dependency from = to >= for Fsharp.Core

#### 0.9.22 - March 4 2015
* Updated to Selenium 2.45, thanks @pottereric

#### 0.9.23 - May 18 2015
* Added &&&&& (always) tests which will run in both normal mode and wip mode [Issue #195](https://github.com/lefthandedgoat/canopy/issues/195)
* Added runFor which lets you run the same set of tests for multiple browsers [Issue #190](https://github.com/lefthandedgoat/canopy/issues/190)

#### 0.9.24 - May 19 2015
* Fixed bug related to new &&&&& (always) operator [Issue #195](https://github.com/lefthandedgoat/canopy/issues/195)

#### 0.9.26 - June 1 2015
* Fixed bug where after wouldn't run following a failed test [Issue #197](https://github.com/lefthandedgoat/canopy/issues/197)

#### 0.9.27 - June 15 2015
* Improved Html reporter to have stack trace, error message and url of failing page [Issue #202](https://github.com/lefthandedgoat/canopy/issues/202)
* Updated to Selenium 2.46

#### 0.9.28 - June 16 2015
* Fix broken link to html reporter introduced in 0.9.27

#### 0.9.29 - June 17 2015
* Fix issue 203, using canopy for unit tests [Issue #203](https://github.com/lefthandedgoat/canopy/issues/203)

#### 0.9.30 - July 30 2015
* Updated to Selenium 2.47
* Fix issue 205, making switchToTab more reliable [Issue #205](https://github.com/lefthandedgoat/canopy/issues/205)
* Improve LiveHtmlReporter so that puts and describe will add ordered messages to the output
* Improved behaviour for failing all tests if on failed.  Changed it to only fail all tests if the 'once' function fails

#### 0.9.31 - August 18 2015
* Fix issue 209, screenshot being taken too late in some cases [Issue #209](https://github.com/lefthandedgoat/canopy/issues/209)
* Fix issue 210, text based selector with apostrophes were not working [Issue #205](https://github.com/lefthandedgoat/canopy/issues/210)

#### 0.9.32 - August 25 2015
* Accept PR adding better support for OSX and Safari.  Thanks to Pedro Ramirez! [Issue #211](https://github.com/lefthandedgoat/canopy/pull/211)

#### 0.9.33 - September 9 2015
* Fix bug were failing tests were marked as passed [Issue #212](https://github.com/lefthandedgoat/canopy/issues/212)
* Make print info displayed in bottom right of page optional, thanks Jeremy Bellows! [Issue #213](https://github.com/lefthandedgoat/canopy/issues/213)

#### 0.9.34 - September 15 2015
* Improved the HTML reporter to have environment, total time, start date, time per context, and time per test

#### 0.9.35 - October 7 2015
* Added features to improve granularity for skipping tests after a failure and skipping individual tests [Issue #215](https://github.com/lefthandedgoat/canopy/issues/215)
* Updated to Selenium 2.48

#### 0.9.36 - October 13 2015
* Improve reporting skip for console and TeamCity reporter [Issue #215](https://github.com/lefthandedgoat/canopy/issues/215)

#### 0.9.37 - October 18 2015
* Great PR from @LimoWanKenobi that fixed a performance problem with add a lot of tests, thanks! [Issue #217](https://github.com/lefthandedgoat/canopy/issues/217)

#### 0.9.38 - October 22 2015
* Fix a bug in html reporter trying to describe json, it would fail to because the data was not encoded properly [Issue #218](https://github.com/lefthandedgoat/canopy/issues/218)

#### 0.9.39 - October 22 2015
* Fix a bug in the bug I fixed in 0.9.38 [Issue #218](https://github.com/lefthandedgoat/canopy/issues/218)

#### 0.9.40 - November 4 2015
* Added nmany to let you run many tests with a name [Issue #219](https://github.com/lefthandedgoat/canopy/issues/219)

#### 0.9.41 - November 12 2015
* Updated to Selenium 2.48.1 [Issue #221](https://github.com/lefthandedgoat/canopy/issues/221)
* Added AutoOpen attribute so you dont have open types, runner, or configuration explicitly [Issue #222](https://github.com/lefthandedgoat/canopy/issues/222)
* New Startup mode for Firefox taking a path and a TimeSpan [Issue #223](https://github.com/lefthandedgoat/canopy/issues/223)

#### 0.9.42 - November 18 2015
* Updated to Selenium 2.48.2
* Added support for configurable file names on failure screenshots [Issue #224](https://github.com/lefthandedgoat/canopy/issues/224)

#### 0.9.43 - December 2 2015
* Improved error handeling on screenshots [Issue #225](https://github.com/lefthandedgoat/canopy/issues/225)

#### 0.9.44 - January 18 2016
* Accepted PR to fix error handeling regression, thanks @RubenOen  [Pull Request #230](https://github.com/lefthandedgoat/canopy/pull/230)
* Fixed issue where read could some times suffer form stale element exceptions and the likes [Issue #229](https://github.com/lefthandedgoat/canopy/issues/229)

#### 0.9.45 - February 27 2016
* Added !=~ (regex not match) operator [Issue #237](https://github.com/lefthandedgoat/canopy/issues/237)
* Add a configurable optimization to disable clear before writing [Issue #236](https://github.com/lefthandedgoat/canopy/issues/236)
* Improve html reporter to make it friendlier to parallel runs [Issue #235](https://github.com/lefthandedgoat/canopy/issues/235)
* Update to selenium 2.52.0 [Issue #233](https://github.com/lefthandedgoat/canopy/issues/233)

#### 0.9.46 - March 12 2016
* Failures in Once or Before will now skip tests in context with skipRemainingTestsInContextOnFailure <- true [Issue #242](https://github.com/lefthandedgoat/canopy/issues/242)
* Fix #241 for using html reporter with parallel test runs [Issue #241](https://github.com/lefthandedgoat/canopy/issues/241)
* Fix #240 selection + optgroup erorr [Issue #240](https://github.com/lefthandedgoat/canopy/issues/240)
* Fix #238 add escape key [Issue #238](https://github.com/lefthandedgoat/canopy/issues/238)

#### 0.9.47 - March 13 2016
* Drop SizSelCsZzz dependency [Issue #244](https://github.com/lefthandedgoat/canopy/issues/244)
* Drop Selenium.Support dependency [Issue #245](https://github.com/lefthandedgoat/canopy/issues/245)
* BREAKING moved browser from canopy.core.browser to canopy.types.browser

#### 0.9.48 - March 24 2016
* Fix issue with jquery selectors that had single ticks in them [Issue #250](https://github.com/lefthandedgoat/canopy/issues/250)
* Update to latest Selenium 2.53.0 [Issue #251](https://github.com/lefthandedgoat/canopy/issues/251)
* Added ability to skip a test that fails with a specific error message [Issue #248](https://github.com/lefthandedgoat/canopy/issues/248)
* Fix issue with failing and using the after method [Issue #247](https://github.com/lefthandedgoat/canopy/issues/247)

#### 0.9.49 - March 24 2016
* Fix issue with encoding on the command line [Issue #252](https://github.com/lefthandedgoat/canopy/issues/252)

#### 0.9.50 - March 27 2016
* Fix issue with multiple dropdowns with same values [Issue #253](https://github.com/lefthandedgoat/canopy/issues/253)

#### 0.9.51 - April 5 2016
* Add onPass and OnFail hooks, thanks Tony! [Issue #254](https://github.com/lefthandedgoat/canopy/issues/254)

#### 0.9.52 - April 5 2016
* make safelyGetUrl safery, thanks Tony! [Issue #255](https://github.com/lefthandedgoat/canopy/issues/255)

#### 0.9.53 - April 19 2016
* Fixed a bug for jquery selectors where they wouldnt respect their context, thanks goncaloc1! [Issue #257](https://github.com/lefthandedgoat/canopy/issues/257)

#### 0.9.54 - May 18 2016
* Added notContains [Issue #263](https://github.com/lefthandedgoat/canopy/issues/263)
* Added BETA support for Edge browser [Issue #264](https://github.com/lefthandedgoat/canopy/issues/264)

#### 0.9.55 - July 7 2016
* Updated to Selenium 2.53.1 for Firefox 47.1 support [Issue #270](https://github.com/lefthandedgoat/canopy/issues/270)

#### 0.9.56 - July 8 2016
* Add right click for [Issue #273](https://github.com/lefthandedgoat/canopy/issues/273)

#### 0.9.57 - July 16 2016
* Added better support for chromium [Issue #276](https://github.com/lefthandedgoat/canopy/issues/276)

#### 0.9.58 - July 31 2016
* Added ability to skip in the middle of a test [Issue #280](https://github.com/lefthandedgoat/canopy/issues/280)

#### 0.9.59 - August 12 2016
* Added ability to position a browser in a specific location, thanks @navmed! [Issue #285](https://github.com/lefthandedgoat/canopy/issues/285)

#### 0.9.60 - August 13 2016
* Added chromium with options support [Issue #276](https://github.com/lefthandedgoat/canopy/issues/276)

#### 0.9.61 - September 16 2016
* Fix chrome develop options popup [Issue #289](https://github.com/lefthandedgoat/canopy/issues/289)

#### 0.9.62 - October 5 2016
* Fix runFor [Issue #292](https://github.com/lefthandedgoat/canopy/issues/292)

#### 1.0.0 - Nov 4 2016
* Update to Selenium 3.0 (Note that anything like rightClick or hover which uses the actions api does not work with FF (which now requires gechkodriver)) [Issue #300](https://github.com/lefthandedgoat/canopy/issues/300)

#### 1.0.1 - Nov 6 2016
* Basic JUnitReporter implementation by blair55 [Issue #302](https://github.com/lefthandedgoat/canopy/issues/302)

#### 1.0.2 - Nov 9 2016
* Added strict 'onn' to check that you are on a url (not fuzzy) by blair55 [Issue #304](https://github.com/lefthandedgoat/canopy/pull/304)

#### 1.0.3 - Nov 9 2016
* Improve JUnitReporter by add time meta data by blair55 [Issue #305](https://github.com/lefthandedgoat/canopy/pull/305)

#### 1.0.4 - Nov 21 2016
* Large images in caused html reporter to act funny, converted from png to jpg for better compression [Issue #310](https://github.com/lefthandedgoat/canopy/issues/310)

#### 1.0.5 - Dec 5 2016
* Adding a new FireFox browser cosntructor [Issue #315](https://github.com/lefthandedgoat/canopy/issues/315)

#### 1.0.6 - Dec 5 2016
* Add timestamp to log in describe() func thanks @knocte! [Issue #318](https://github.com/lefthandedgoat/canopy/pull/318)

#### 1.0.7 - Jan 6 2017
* Make embedded images optional in TeamCity reporter [Issue #324](https://github.com/lefthandedgoat/canopy/issues/324)
* Improve suggestions on failed selector finds by switching from Levenshtein to Jaro-Winkler, thanks forki! [PR #323](https://github.com/lefthandedgoat/canopy/pull/323)
* Improve exception messages in errors, thanks @knocte! [PR #321](https://github.com/lefthandedgoat/canopy/issues/321)

#### 1.1.0 - Jan 12 2017
* Added a C# friendly wrapper (See program.cs in the C# project for examples) [Issue #327](https://github.com/lefthandedgoat/canopy/issues/327)

#### 1.1.1 - Jan 18 2017
* Improve compatability with Mono 64 bit, thanks MorganPersson [PR #333](https://github.com/lefthandedgoat/canopy/pull/333)

#### 1.1.2 - Jan 24 2017
* Fix bugs in C# friendly wrapper, thanks @spindev! [Issue #327](https://github.com/lefthandedgoat/canopy/pull/327)

#### 1.1.3 - Feb 15 2017
* Fix bugs jUnit reporter, thanks @vermeeca! [PR #345](https://github.com/lefthandedgoat/canopy/pull/345)

#### 1.1.4 - Feb 20 2017
* Improve error on url with browser is null, thanks @forki [PR #348](https://github.com/lefthandedgoat/canopy/pull/348)

#### 1.2.0 - Mar 11 2017
Update to Selenium 3.3 [Issue #351](https://github.com/lefthandedgoat/canopy/issues/351)
* Fix problem with auto skipping and Team city [Issue #350](https://github.com/lefthandedgoat/canopy/issues/350)
* Allow for the HideConsoleWindow setting to be set, thanks Ma3yTa! [PR #353](https://github.com/lefthandedgoat/canopy/pull/353)

#### 1.2.1 - Apr 2 2017
* Attempt to fix js error in class name suggestions [Issue #358](https://github.com/lefthandedgoat/canopy/issues/358)
* Disable the 'Chrome is being run by automated process' banner [Issue #355](https://github.com/lefthandedgoat/canopy/issues/355)

#### 1.2.2 - Apr 11 2017
* Add timestamps to write messages, thanks milkeg! [PR #360](https://github.com/lefthandedgoat/canopy/pull/360)
* Add a case insensitive contains assertion, thanks milkeg! [PR #361](https://github.com/lefthandedgoat/canopy/pull/361)

#### 1.2.3 - Apr 16 2017
* Fix wrong chromium path [Issue #360](https://github.com/lefthandedgoat/canopy/issues/362)
* BREAKING Update *nix driver path from usr/bin to usr/local/bin [Issue #363](https://github.com/lefthandedgoat/canopy/issues/363)

#### 1.3.0 - May 3 2017
* Update to Selenium 3.4
* BREAKING?? Added find by text selector that works by first trimming whitespace.  Now a default finder.

#### 1.3.1 - May 20 2017
* XmlEncode failure message in JUnitReporter [PR #367](https://github.com/lefthandedgoat/canopy/pull/367)

#### 1.4.0 - May 23 2017
* Added a json validator [PR #368](https://github.com/lefthandedgoat/canopy/pull/368)

#### 1.4.3 - June 1 2017
* json validator - null arrays and properties are now acceptable

#### 1.4.5 - June 9 2017
* Added ability to shiftClick

#### 1.5.0 - Apr 16 2017
* Breaking: move json validator to its own project to remove dependency on FSharp.Data [Issue #371](https://github.com/lefthandedgoat/canopy/issues/371)

#### 1.5.1 - Sept 5 2017
* Added configuration for failing if any tests are WIP (useful for CI) thanks @blair55! [PR #381](https://github.com/lefthandedgoat/canopy/pull/381)
* Added start mode for chrome headless and firefox headless
* Updated to Selenium 3.5.2

#### 1.5.2 - Oct 11 2017
* Fix a problem with failedContexts being written in parallel when not using it [Issue #387](https://github.com/lefthandedgoat/canopy/issues/387)

#### 1.5.3 - Oct 17 2017
* Improve how wait works to clean up stack trace and not stack overflow [Issue #388](https://github.com/lefthandedgoat/canopy/issues/388)

#### 1.6.0 - Nov 7 2017
* Updated to .net v4.5.2 (Possibly breaking!!!)
* Added load testing to canopy.integration !!! [PR #389](https://github.com/lefthandedgoat/canopy/pull/389)

#### 1.6.1 - Nov 8 2017
* Updated to FSharp.Core 4.0.0.1 (Possibly breaking!!!)
* Added more information to the results of a Load Test [PR #393](https://github.com/lefthandedgoat/canopy/pull/393)