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
*)

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
=== (aliased as is)
-----------
Assert that the value on the left is equal to the value on right.
* Note: does not use retry-ability.  Equivalent to Assert.Equals.
*)
"Not a selector" === "Not a selector"

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
containsInsensitive
--------
Assert that one string contains (case insensitive) another.
*)
containsInsensitive "Log" (read "#logout")

(**
notContains
--------
Assert that one string does not contains another.
*)
notContains "Hello Bob!" (read "#name")

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
!=~ (regex match)
----------------
Assert that an element does not `regex` match a value.
*)
"#lastName" << "Gr0y"
"#lastName" !=~ "Gr[ae]y"
"#lastName" << "Gr1y"
"#lastName" !=~ "Gr[ae]y"

(**
*~ (one of many regex match)
----------------------------
Assert that one of many element `regex` matches a value.
*)
"#colors li" *~ "gr[ea]y"

(**
on
--
Assert that the browser is currently on a url. Falls back to using `String.Contains` after page timeout.
*)
url "https://duckduckgo.com/?q=canopy+f%23"
on "https://duckduckgo.com/?q"

(**
onn
--
Same as `on` but does not fall back to using `String.Contains`.
*)
url "https://duckduckgo.com/about"
onn "https://duckduckgo.com/about"

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
displayed 
---------
Assert that an element is displayed on the screen. (Note:  Will not walk up the dom.  If a parent container is hidden this may give the wrong results, try adding :visible to selector)
*)
displayed "#modal"

(**
notDisplayed
------------
Assert that an element is not displayed on the screen. (Note:  Will not walk up the dom.  If a parent container is hidden this may give the wrong results, try adding :visible to selector)
*)
notDisplayed "#modal"

(**
enabled
---------
Assert that an element is enabled.
*)
enabled "#button"

(**
disabled
---------
Assert that an element is not enabled.
*)
disabled "#button"

(**
fadedIn
---------
Returns true/false if element has finished fading in and is shown.
*)
let isShown = (fadedIn "#message")()
waitFor <| fadedIn "#message"
