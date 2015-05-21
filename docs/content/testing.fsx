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
Testing
========================

context 
-------
Define the context of the tests. A default context is defined and used if one is not provided. 
You can have as many contexts as you like. Each context gets a new once/before/after/lastly function. 
*)
context "Login page tests"
//some tests

context "Search page tests"
//different tests

context "Reset password page tests"
//different tests

(**
once 
----
Function that is run once time at the beginning of a test suite. 
*)
once (fun _ -> 
    ()//do this one time at the beginning of the most recently defined context
)

(**
before 
------
Function that is run before each test in a context. 
*)
before (fun _ -> 
    ()//do this before every test of the most recently defined context
)

(**
after 
-----
Function that is run after each test in a context. 
*)
after (fun _ -> 
    ()//do this after every test of the most recently defined context
)

(**
lastly 
------
Function that is run once at the end of a context. 
*)
lastly (fun _ -> 
    ()//do this after the very last test of the most recently defined context
)

(**
test 
----
Standard test definition (name defined automatically by the test number eg: Test #1). 
*)
test (fun _ ->
    //go somewhere
    //interact with page
    //assert
    ()
)

(**
&&& (named test, aliased as ntest) 
----------------------------------
Standard test definition with a name. 
*)
"go somewhere, do some stuff, assert" &&& fun _ ->
    //go somewhere
    //interact with page
    //assert
    ()
//Or
ntest "go somewhere, do some stuff, assert" (fun _ ->
    //go somewhere
    //interact with page
    //assert
    ()
)

(**
&&&& (work in progress, aliased as wip) 
---------------------------------------
Used for debugging. Test runs slower and highlights the elements that it is interacting with to help debug. 
If one test is marked wip, only wip tests are ran. Other tests are skipped. 
*)
//this test is run slow and only with other tests marked as wip
"go somewhere, do some stuff, assert" &&&& fun _ ->
    //go somewhere
    //interact with page
    //assert
    ()
//Or
wip (fun _ ->
    //go somewhere
    //interact with page
    //assert
    ()
)

(**
&&! (skip, aliased as xtest) 
----------------------------
Do not run a test. 
*)
//skipped
"go somewhere, do some stuff, assert" &&! fun _ ->
    //go somewhere
    //interact with page
    //assert
    ()
//Or
xtest (fun _ ->
    //go somewhere
    //interact with page
    //assert
    ()
)

(**
&&&&& (always run, in both standard and wip modes) 
---------------------------------------
Test will always be run.  If some tests are marked work in progress, tests marked as always will also run.  
The test will run slow with wip tests, but run at normal speed when there are no wip tests.
*)
//this test is run slow with wip test and regular speed with standard tests, test will always run.
"go somewhere, do some stuff, assert" &&&&& fun _ ->
    //go somewhere
    //interact with page
    //assert
    ()

(**
many 
----
Run a single test X times. Helps with troubleshooting tests that sometimes fail. 
*)
many 20 (fun _ ->
    //go somewhere
    //interact with page
    //assert
    ()
)

(**
todo 
----
Mark a test as todo to fill in later. LiveHtmlReporter will mark todo tests in the output. 
*)
"go somewhere, do some stuff, assert" &&& todo
