(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"
#r "canopy.dll"
#r "WebDriver.dll"
open canopy
open runner
open System

(**
Reporting
=========

Console Reporter
----------------
The default reporter. Prints results to console.
<img src="img/console.png" alt="Console Reporter" style="display: inherit;"/>

Live HTML Reporter
------------------
Prints results to an html page. Support images. Screenshots on error, supports coverage reports.
*)
open configuration
open reporters
reporter <- new LiveHtmlReporter() :> IReporter

(**
<img src="img/livehtmlreport.png" alt="Live HTML Reporter" style="display: inherit;"/>

Coverage
--------
Builds up selectors used for a given url, goes to that page and highlights all elements you have interacted with there. 
Good for visual reference to what has and has not been tested.
<img src="img/coverage.png" alt="Live HTML Reporter" style="display: inherit;"/>

TeamCity Reporter
-----------------
Prints special outputs that are compatible with team city.
*)

open configuration
open reporters
reporter <- new TeamCityReporter() :> IReporter

//screenshot is TODO
