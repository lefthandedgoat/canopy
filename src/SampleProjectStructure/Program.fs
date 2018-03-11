module Program

open canopy
open runner
//canopyExtensions is the file I add nice little helper functions to that are specific to my site 
open canopyExtensions

start firefox
//dont run these tests, they point to local host and wont do anything =)

//note the order of files, F# uses top down compilation so your dependencies have to be in order in your proj solution
//to add folders to your project file you have to manually modify the .proj file, minor annoyance.

//I run everything via console with a rake task.  You can use FAKE/Nant/MSBuild/batch file/whatever to run your code
//Since I host my tests in a console app I can pass in arguments to determine what tests to run but typically I just comment
//out the tests I do not want to run e.g:

home.all()
//login.all()
login.positive()

//for me breaking things up by page that you test works well, with selectors for the page, helper methods specific to actions
//you want to perform on that page, and then the tests at the bottom

//also I am pretty sure you can write canopy tests in any testing framework like nUnit, but cant use WIP/many/other features
//unique to the canopy test runner.  The runner is very simple and you can extend it any way you like.

run()
