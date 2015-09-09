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
Assertions
========================

on 
--
Assert that the browser is currently on a url (uses `String.Contains`).
*)
url "https://duckduckgo.com/?q=canopy+f%23"
on "https://duckduckgo.com/?q"

(**
selected 
--------
Assert that a radio or checkbox is selected. 
*)
selected "#yes"

(**
deselected 
----------
Assert that a radio or checkbox is not selected. 
*)
deselected "#yes"

(**
== (equals) 
-----------
Assert that the element on the left is equal to the value on the right. 
*)
"#firstName" == "Alex"

(**
!= (does not equal) 
-------------------
Assert that the element on the left is not equal to the value on the right. 
*)
"#firstName" != "Tom"

(**
*= (one of many equals) 
-----------------------
Assert that at least one element in a list equals a value. 
*)
".todoItem" *= "Buy milk"

(**
*!= (none equals) 
-----------------
Assert that none of the items in a list equals a value. 
*)
".todoItem" *!= "Sell everything"

(**
contains
--------
Assert that one string contains another. 
*)
contains "Log" (read "#logout")

(**
count 
-----
Assert there are `X` items of given css selector. 
*)
count ".todoItem" 5

(**
=~ (regex match) 
----------------
Assert that an element `regex` matches a value. 
*)
"#lastName" << "Gray"
"#lastName" =~ "Gr[ae]y"
"#lastName" << "Grey"
"#lastName" =~ "Gr[ae]y"

(**
*~ (one of many regex match) 
----------------------------
Assert that one of many element `regex` matches a value. 
*)
"#colors li" *~ "gr[ea]y" 

(**
displayed 
---------
Assert that an element is displayed on the screen. 
*)
displayed "#modal"

(**
notDisplayed 
------------
Assert that an element is not displayed on the screen. 
*)
notDisplayed "#modal"