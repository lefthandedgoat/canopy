(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../src/canopy/bin/Debug/netstandard2.0"
#I "../packages/Selenium.WebDriver/lib/netstandard2.0"
#r "canopy.dll"
#r "WebDriver.dll"
#if !FAKE
#r "netstandard"
#r "System.Runtime.Extensions"
#endif


(**
canopy - f#rictionless web testing
===================

<div class="row">
  <div class="col-2"></div>
  <div class="col-8">
    <div class="card bg-light" id="nuget">
      <div class="card-body">
        <p class="card-title">
      The canopy library can be <a href="https://www.nuget.org/packages/canopy/">installed from NuGet</a>:
      </p>
      <p class="card-text">
        <pre>PM> Install-Package canopy</pre>
      </p>
      </div>
    </div>
  </div>
</div>

canopy is a web testing framework with one goal in mind, make UI testing simple:

* Solid stabilization layer built on top of Selenium. Death to "brittle, quirky, UI tests".

* Quick to learn. Even if you've never done UI Automation, and don't know F#.

* Clean, concise API.

* .net Standard 2.0.

* MIT License.

Getting Started
---------------
####1\. Create a new F# console application (4.6.1+ or .net core)
<img src="img/newProject.png" alt="F# New Project" style="display: inherit;"/>

####2\. Install canopy via Nuget
<img src="img/installCanopy.png" alt="Install canopy" style="display: inherit;"/>

####3\. Install chromedriver via Nuget
<img src="img/installChromeDriver.png" alt="Install chromedriver" style="display: inherit;"/>

####4\. Paste the following code into `Program.fs`

*)
//these are similar to C# using statements
open canopy.runner.classic
open canopy.configuration
open canopy.classic

canopy.configuration.chromeDir <- System.AppContext.BaseDirectory

//start an instance of chrome
start chrome

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

* [Actions](/content/actions.html): documentation of everything you can do on a page
* [Assertions](/content/assertions.html): all the ways you can verify what's on the page is correct
* [Configuration](/content/configuration.html): configure and fine tune canopy
* [Testing](/canopy/content.html): different ways to orchestrate tests and troubleshoot issues with a page
* [Reporting](/content/reporting.html): different ways to output the results of your test suite

####7\. Watch some intro videos
4 minute canopy starter kit
<iframe width="960" height="540" src="https://www.youtube.com/embed/kLNPl3EcsCI" frameborder="0" allowfullscreen></iframe>

5 minutes with Amir Rajan
<iframe src="https://channel9.msdn.com/Events/NET-Fringe/NET-Fringe-2015/Web-UI-Testing-with-F-and-Canopy/player"
    width="960"
    height="540"
    allowFullScreen
    frameBorder="0">
</iframe>

30 minutes with Chris Holt at fsharpConf
<iframe src="https://channel9.msdn.com/Events/FSharp-Events/fsharpConf-2016/Web-UI-Automation-with-F-and-canopy/player"
    width="960"
    height="540"
    allowFullScreen
    frameBorder="0">
</iframe>
*)
