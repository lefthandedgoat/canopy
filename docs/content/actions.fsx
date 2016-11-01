(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"
#r "canopy.dll"
#r "WebDriver.dll"
open canopy
open canopy.types
open runner
open System
open OpenQA.Selenium

(**
Actions
========================
*)

(**
start
-----
Start an instance of a browser.
*)
start firefox
start chrome
start ie
start safari
start phantomJS
start aurora
start edgeBETA
start chromium

(**
!^ (aliased by url)
---------------------
Go to a url.
*)
!^ "http://www.google.com"
url "http://www.google.com"

(**
quit
----
Quit the current browser or the specified browser.
*)
//quit current
quit ()
//quit specific
quit browser1

(**
<< (write)
----------
Write text to element.
*)
"#firstName" << "Alex"
//if you dont like the << syntax you can alias anyway you like, eg:
let write text selector =
    selector << text

(**
read
----
Read the text (or value or selected option) of an element.
*)
let selectedState = read "#states"
let firstName = read "#firstName"
let linkText = read "#someLink"

(**
click
-----
Click an element via selector or text, can also click selenium `IWebElements`.
*)
click "#login"
click "Login"
click (element "#login")

(**
doubleClick
-----------
Simulates a double click via JavaScript.
*)
doubleClick "#login"

(**
ctrlClick
-----
Click an element via selector or text while holding down the control key, can also click selenium `IWebElements`.
*)
ctrlClick "#list > option"
ctrlClick "Oklahoma"
ctrlClick (element "#list > option")

(**
rightClick
-----
Right click an element.
*)
rightClick "#settings"

(**
check
-----
Checks a checkbox if it is not already checked.
*)
check "#yes"
//below code will not click the checkbox again, which would uncheck it
check "#yes"

(**
uncheck
-------
Unchecks a checkbox if it is not already unchecked.
*)
uncheck "#yes"
//below code will not click the checkbox again, which would check it
uncheck "#yes"

(**
--> (drag is an alias)
----------------------
Drag on item to another.
*)
".todo" --> ".inprogress"
drag ".todo" ".inprogress"

(**
hover
----------------------
Hover over an element.
*)
url "http://lefthandedgoat.github.io/canopy/testpages/"
"#hover" == "not hovered"
hover "Milk"
"#hover" == "hovered"

(**
element
-------
Get an element (Selenium `IWebElement`) with given css selector or text (built in waits, automatically searches through `iFrames`).
Most useful if you need to write some custom helpers to provide functionality that canopy does not currently have.
*)
let logoutHref = (element "#logout").GetAttribute("href")
describe ("logout href is: " + logoutHref)
//continue with your test

(**
unreliableElement
-------
Try to get an element without the built in reliability. Throws exception if element not found.
*)
let logout = unreliableElement "#logout"

(**
elementWithText
-------
Unreliably get the first element with specific text for a selector.
*)
let firstBob = elementWithText ".name" "Bob"

(**
elementWithin
-------------
Get an element by searching within another element (nested).
*)
let name = element "#header" |> elementWithin ".name"


(**
someElement
-----------
Like `element` function except it runs a `Some(IWebElement)` or `None`.
Read more about `Option` types [here](http://en.wikibooks.org/wiki/F_Sharp_Programming/Option_Types).
*)
//create your own exists helper function
let exists selector =
    let someEle = someElement selector
    match someEle with
    | Some(_) -> true
    | None -> false

(**
someElementWithin
-----------------
Like `elementWithin` function except it runs a `Some(IWebElement)` or `None`.
Read more about Option types [here](http://en.wikibooks.org/wiki/F_Sharp_Programming/Option_Types).
*)
//create your own exists helper function
let someName = element "#header" |> someElementWithin ".name"

(**
parent
------
Get the parent element of provided element.
*)
element "#firstName" |> parent

(**
someParent
----------
Get the `Some`/`None` parent element of provided element.
*)
element "#firstName" |> someParent

(**
elements
--------
The same as element except you get all elements that match the css selector or text.
*)
let clickAll selector =
  elements selector
  |> List.iter (fun element -> click element)

clickAll ".button"

(**
unreliableElements
--------
The same as elements except there is no reliability.  You get an empty list if there no elements on the first try.
*)
let names = unreliableElements ".name"

(**
unreliableElementsWithin
--------
Try without reliability to get elements within an existing element.
*)
//note that the bellow can be done (better) with selector '#people tr:first'
//the space is 'within' in css selectors
let people = element "#people"
let firstPerson = unreliableElementsWithin "tr:first" people

(**
elementsWithText
-------
Unreliably gets elements with specific text for a selector.
*)
let daves = elementsWithText ".name" "Dave"

(**
elementsWithin
--------------
Get elements by searching within another element (nested).
*)
let names = element "#header" |> elementsWithin ".name"

(**
nth
---
Get the nth element.
*)
click (nth 4 ".button")

(**
first
-----
Get the first element.
*)
click (first ".button")

(**
last
----
Get the last element.
*)
click (last ".button")

(**
fastTextFromCSS
----
Effeciently get the text values for all elements matching a css selector.
*)
let names = fastTextFromCSS ".name"

(**
switchTo
--------
Switch to an existing instance of a browser.
*)
start firefox
let mainBrowser = browser
start chrome
let secondBrowser = browser
//switch back to mainBrowser after opening secondBrowser
switchTo mainBrowser

(**
switchToTab
--------
Switch browser focus between tabs.
*)
switchToTab 2

(**
closeTab
--------
Close a specific tab.
*)
closeTab 2

(**
resize
--------
Resize the browser to a specific size.
*)
resize (1920, 1080)

(**
rotate
--------
Rotate the browser by switching the Height and Width.
*)
rotate()

(**
js
--
Run JavaScript in the current browser.
*)
//give the title a border
js "document.querySelector('#title').style.border = 'thick solid #FFF467';"

(**
screenshot
----------
Take a screenshot and save it to the specified path with specified filename. Returns image as byte array.
*)
let path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
let filename = DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
screenshot path filename

(**
sleep
-----
Sleep for `X` seconds.
*)
//sleep for 1 second
sleep ()
//sleep for 1 second
sleep 1
//sleep for 3 seconds
sleep 3

(**
highlight
---------
Place a border around an element to help you identify it visually, used in wip test mode.
*)
highlight ".btn"

(**
describe (aliased as puts)
--------
Describe something in your test, currently writes description to console.
*)
describe "on main page, testing logout"

(**
waitFor
-------
Wait until custom function is true (better alternative to sleeping `X` seconds).
*)
let fiveNumbersShown () =
    (elements ".number").Length = 5

url "http://somepage.com/countdown"
waitFor fiveNumbersShown
//continue with your test

(**
waitFor2
-------
Wait (with message) until custom function is true (better alternative to sleeping `X` seconds).
*)
let fiveNumbersShown () =
    (elements ".number").Length = 5

url "http://somepage.com/countdown"
waitFor2 "waiting for five numbers to be shown" fiveNumbersShown
//continue with your test

(**
waitForElement
--------------
Wait until an element with a given CSS selector appears in the DOM.
This is useful when you need to wait for data being loaded and displayed.
*)
url "http://somepage.com/countdown"
waitForElement ".number"
//continue with your test

(**
clear
-----
Clear the text of an element.
*)
clear "#firstName"

(**
press
-----
Simulate a key press. Other keys can be sent by first importing OpenQA.Selenium and using the keys defined there.
*)
press tab
press enter
press down
press up
press left
press right
press esc

open OpenQA.Selenium
press Keys.Backspace


(**
alert
-----
Gets the current alert.
*)
alert() == "Welcome to Test Page!"

(**
acceptAlert
-----------
Accepts the current alert.
*)
acceptAlert()

(**
dismissAlert
------------
Dismiss the current alert.
*)
dismissAlert()

(**
pin
---
Pin a browser to the left, right, or fullscreen (any browser you start is pinned right automatically).
*)
pin Left
pin Right
pin FullScreen

(**
pinToMonitor
---
Move the browser to another monitor.
*)
pinToMonitor 2

(**
tile
----
Tile listed browsers equally across your screen. 4 open browsers would each take 25% of the screen.
*)
start chrome
let browser1 = browser
start chrome
let browser2 = browser
start chrome
let browser3 = browser

tile [browser1; browser2; browser3]

(**
positionBrowser
----
Position current browser on the screen - position is in percentages: positionBrowser left top width height 
*)
positionBrowser 66 0 33 50


(**
currentUrl
----------
Gets the current url.
*)
let u = currentUrl ()

(**
title
-----
Gets the title of the current page.
*)
let theTitle = title ()

(**
reload
------
Reload the current page.
*)
reload ()

(**
navigate
------
Navigate forward or back.
*)
navigate back
navigate forward

(**
coverage
------
Create a coverage report for a url.
*)
coverage "http://www.google.com"

(**
addFinder
------
Add a finder to the list of current finders to make your selectors cleaner.
*)
let findByHref href f =
    try
        let cssSelector = sprintf "a[href*='%s']" href
        f(By.CssSelector(cssSelector)) |> List.ofSeq
    with | ex -> []

addFinder findByHref

(**
Fast selectors
------
Skip looking through the list of finders for a specific selector, use a specific function.
*)
css ".name"
xpath "//div/span"
jquery ".name:first"
label "First Name"
text "Last Name"
value "Submit"

(**
failsWith
-----
Expect a failure with a specific message and pass test if it occurs
*)
failsWith "An expected error message"
