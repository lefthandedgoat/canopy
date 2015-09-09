(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
canopy - f#rictionless web testing
===================

<div class="row">
  <div class="span1"></div>
  <div class="span6">
    <div class="well well-small" id="nuget">
      The canopy library can be <a href="https://www.nuget.org/packages/canopy/">installed from NuGet</a>:
      <pre>PM> Install-Package canopy</pre>
    </div>
  </div>
  <div class="span1"></div>
</div>

canopy is a web testing framework with one goal in mind, make UI testing simple:

* Solid stabilization layer built on top of Selenium. Death to "brittle, quirky, UI tests". 

* Quick to learn. Even if you've never done UI Automation, and don't know F#. 

* Clean, concise API. 

* MIT License.
   
Getting Started
---------------
####1\. Create a new F# console application
<img src="img/newProject.png" alt="F# New Project" style="display: inherit;"/>

####2\. Set target framework to .NET Framework 4
<img src="img/profile.png" alt="Change target framework" style="display: inherit;"/>

####3\. Install canopy via Nuget
<img src="img/installCanopy.png" alt="Install canopy" style="display: inherit;"/>

####4\. Paste the following code into `Program.fs`

*)
//these are similar to C# using statements
open canopy
open runner
open System

//start an instance of the firefox browser
start firefox

//this is how you define a test
"taking canopy for a spin" &&& fun _ ->
    //this is an F# function body, it's whitespace enforced

    //go to url
    url "http://lefthandedgoat.github.io/canopy/testpages/"

    //assert that the element with an id of 'welcome' has
    //the text 'Welcome'
    "#welcome" == "Welcome"

    //assert that the element with an id of 'firstName' has the value 'John'
    "#firstName" == "John"

    //change the value of element with
    //an id of 'firstName' to 'Something Else'
    "#firstName" << "Something Else"

    //verify another element's value, click a button,
    //verify the element is updated
    "#button_clicked" == "button not clicked"
    click "#button"
    "#button_clicked" == "button clicked"

//run all tests
run()

printfn "press [enter] to exit"
System.Console.ReadLine() |> ignore

quit()
(**
####5\. Run
<img src="img/run.png" alt="Run" style="display: inherit;"/>

####6\. Explore the rest of canopy's API 

* [Actions](/canopy/actions.html): documentation of everything you can do on a page
* [Assertions](/canopy/assertions.html): all the ways you can verify what's on the page is correct
* [Configuration](/canopy/configuration.html): configure and fine tune canopy
* [Testing](/canopy/testing.html): different ways to orchestrate tests and troubleshoot issues with a page
* [Reporting](/canopy/reporting.html): different ways to output the results of your test suite

*)
