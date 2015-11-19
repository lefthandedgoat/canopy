module main

open System
open OpenQA.Selenium
open canopy
open reporters

start chrome
let mainBrowser = browser
elementTimeout <- 3.0
compareTimeout <- 3.0
pageTimeout <- 3.0
runFailedContextsFirst <- true
reporter <- new LiveHtmlReporter(Chrome, configuration.chromeDir) :> IReporter 
reporter.setEnvironment "My Machine"

configuration.failScreenshotFileName <- 
  (fun test suite -> 
      let suiteContext = if suite.Context = null then "" else suite.Context
      let cleanName = test.Description.Replace(' ','_') //etc
      let stamp = DateTime.Now.ToString("MMM-d_HH-mm-ss")
      sprintf "%s_%s_%s" suiteContext cleanName stamp)

failFast := true

file1.all()
file2.all()

context "context1"
once (fun _ -> Console.WriteLine "once")
before (fun _ -> Console.WriteLine "before")
after (fun _ -> Console.WriteLine "after")
lastly (fun _ -> Console.WriteLine "lastly")
 
let testpage = "http://lefthandedgoat.github.io/canopy/testpages/" 

"intentionally skipped shows blue in LiveHtmlReport" &&! skipped

"Apostrophes don't break anything" &&& fun _ ->    
    url testpage
    count "I've got an apostrophe" 1

"#welcome should have Welcome" &&& fun _ ->    
    url testpage
    "#welcome" == "Welcome"

//ntest "description" (fun _ -> url "http://www.google.com")
//and
//"description" &&& fun _ -> url "http://www.google.com")
//do the same thing
ntest "find by label, following field" (fun _ ->
    url testpage
    "Test Field 1" == "test value 1")

//using regular test will not name your test and it will be assigned a number
//This test would be
//Test #3
//in the html report
test (fun _ ->
    describe "find by label, for attribute"
    url testpage
    "Test Field 2" == "test value 2")

"#firstName should have John (using == infix operator)" &&& fun _ ->
    url testpage
    "#firstName" == "John"

"id('firstName') should have John (using == infix operator), basic xpath test" &&& fun _ ->
    url testpage
    "id('firstName')" == "John"

"#lastName should have Doe" &&& fun _ ->
    !^ testpage
    "#lastName" == "Doe"

"#lastName should have Doe via read cssSelector" &&& fun _ ->
    !^ testpage
    read "#lastName" |> is "Doe"

"#lastName should have Doe via read IWebElements" &&& fun _ ->
    !^ testpage
    element "#lastname" |> read |> is "Doe"

"clearing #firstName sets text to new empty string" &&& fun _ ->
    !^ testpage
    clear "#firstName"
    "#firstName" == ""

"clearing #firstName sets text to new empty string via IWebElement" &&& fun _ ->
    !^ testpage
    element "#firstName" |> clear
    "#firstName" == ""

"writing to #lastName sets text to Smith" &&& fun _ ->
    !^ testpage
    clear "#lastName"
    "#lastName" << "Smith"
    "#lastName" == "Smith"

"writing to #lastName (as element) sets text to John" &&& fun _ ->
    !^ testpage
    let lastname = element "#lastname"
    clear lastname   
    lastname << "John"
    "#lastname" == "John"

"writing to .lastName sets text to new Smith in both boxes" &&& fun _ ->
    !^ testpage
    clear "#lastName"
    ".lastName" << "Smith"
    "#lastName" == "Smith"
    "#lastName2" == "Smith"

"writing to .lastName sets text to new Smith in both boxes, xpath test" &&& fun _ ->
    !^ testpage
    clear "#lastName"
    "//input[@class='lastName']" << "Smith"
    "#lastName" == "Smith"
    "#lastName2" == "Smith"

"writing to #lastName sets text to new Smith (implicit clear in write)" &&& fun _ ->
    !^ testpage
    "#lastName" << "Smith"
    "#lastName" == "Smith"

"#ajax label should have ajax loaded" &&& fun _ ->
    !^ testpage
    "#ajax" == "ajax loaded"

"Value 1 listed in #value_list" &&& fun _ ->
    !^ testpage
    "#value_list td" *= "Value 1"

"Value 2 listed in #value_list" &&& fun _ ->
    !^ testpage
    "#value_list td" *= "Value 2"

"Value 3 listed in #value_list" &&& fun _ ->
    !^ testpage
    "#value_list td" *= "Value 3"

"Value 4 listed in #value_list" &&& fun _ ->
    !^ testpage
    "#value_list td" *= "Value 4"

"Item 1 listed in #item_list" &&& fun _ ->
    !^ testpage
    "#item_list option" *= "Item 1"

"Item 2 listed in #item_list" &&& fun _ ->
    !^ testpage
    "#item_list option" *= "Item 2"

"Item 3 listed in #item_list" &&& fun _ ->
    !^ testpage
    "#item_list option" *= "Item 3"

"Item 4 listed in #item_list" &&& fun _ ->
    !^ testpage
    "#item_list option" *= "Item 4"

"clicking #button sets #button_clicked to button clicked" &&& fun _ ->
    !^ testpage
    "#button_clicked" == "button not clicked"
    click "#button"
    "#button_clicked" == "button clicked"

"clicking button with text 'Click Me!!' sets #button_clicked to button clicked" &&& fun _ ->
    !^ testpage
    "#button_clicked" == "button not clicked"
    click "Click Me!!"
    "#button_clicked" == "button clicked"

"clicking (element #button) sets #button_clicked to button clicked" &&& fun _ ->
    !^ testpage
    "#button_clicked" == "button not clicked"
    click (element "#button")
    "#button_clicked" == "button clicked"

"clicking hyperlink sets #link_clicked to link clicked" &&& fun _ ->
    !^ testpage
    "#link_clicked" == "link not clicked"
    click "#hyperlink"
    "#link_clicked" == "link clicked"

"clicking hyperlink via text sets #link_clicked to link clicked" &&& fun _ ->
    !^ testpage
    "#link_clicked" == "link not clicked"
    click "Click Me!"
    "#link_clicked" == "link clicked"

"clicking #radio1 selects it" &&& fun _ ->
    !^ testpage
    click "#radio1"
    selected "#radio1"

"clicking #radio1 selects it via IWebElement" &&& fun _ ->
    !^ testpage
    click "#radio1"
    element "#radio1" |> selected

"clicking #radio2 selects it" &&& fun _ ->
    !^ testpage
    click "#radio2"
    selected "#radio2"


"clicking #checkbox selects it" &&& fun _ ->
    !^ testpage
    check "#checkbox"
    selected "#checkbox"

"clicking #checkbox selects it via sizzle" &&& fun _ ->
    !^ testpage
    check "#checkbox"
    count "input:checked" 1

"clicking selected #checkbox deselects it" &&& fun _ ->
    !^ testpage
    check "#checkbox"
    uncheck "#checkbox"
    deselected "#checkbox"

"clicking selected #checkbox deselects it via IWebElement" &&& fun _ ->
    !^ testpage
    check "#checkbox"
    uncheck "#checkbox"
    element "#checkbox" |> deselected

"element within only searching within the element" &&& fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin"
    count ".item" 5
    "spanned item 4" === (element "span" |> elementWithin ".item").Text

"elements within only searching within element" &&& fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin"
    count ".item" 5
    2 === (element "span" |> elementsWithin ".item" |> List.length)

"someElementWithin only searching within element" &&& fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin"
    count ".item" 5
    true === (element "span" |> someElementWithin ".specialItem").IsSome

"parent of firstItem and secondItem is list" &&& fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/parent"
    "list" === (element "#firstItem" |> parent).GetAttribute("id")

"some parent of firstItem and secondItem is list" &&& fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/parent"
    true === (element "#firstItem" |> someParent).IsSome
    "list" === (element "#firstItem" |> someParent).Value.GetAttribute("id")

"someElement returns Some when element found" &&& fun _ ->
    url testpage
    true === (someElement "#testfield2").IsSome
    "test value 2" === (someElement "#testfield2").Value.GetAttribute("value")

"someElement returns None when element not found" &&& fun _ ->
    url testpage
    None === (someElement "#thisIdDoesNotExist")

"someElement fails when more than one element found" &&& fun _ ->
    url testpage    
    failsWith "More than one element was selected when only one was expected for selector: .lastName"
    someElement ".lastName" |> ignore

"Navigating to a url should be on url" &&& fun _ ->
    url testpage
    on testpage

"Navigating to a url with query string should be on url with and without a query string" &&& fun _ ->
    let testpageWithQueryString = testpage + "?param1=weeeee"
    url testpageWithQueryString
    on testpageWithQueryString //with query string
    on testpage //without query string

"Should be on a non absolute url" &&& fun _ ->
    let testpageWithQueryString = testpage + "?param1=weeeee"
    url testpageWithQueryString

    let path = (new System.UriBuilder(testpage)).Path
    on path

    //ensure all permutations of missing leading and trailing slashes work
    on (path.TrimStart('/'))
    on (path.TrimEnd('/'))
    on (path.Trim('/'))

"Should not be on partial url" &&& fun _ ->
    url testpage
    let partialUrl = "http://lefthandedgoat.github.io/canopy"
    failsWith <| sprintf "on check failed, expected expression '%s' got %s" partialUrl testpage
    on partialUrl

"Should not be on child url" &&& fun _ ->
    url testpage
    let childUrl = testpage + "notatthspath/"
    failsWith <| sprintf "on check failed, expected expression '%s' got %s" childUrl testpage
    on childUrl

context "reddit tests '"
once (fun _ -> Console.WriteLine "once: reddit tests")
before (fun _ -> Console.WriteLine "before: reddit tests")
after (fun _ -> Console.WriteLine "after: reddit tests")
lastly (fun _ -> Console.WriteLine "lastly: reddit tests")

"browsing to redit should be on reddit '" &&& fun _ ->
    url "http://www.reddit.com/"
    on "http://www.reddit.com/"

"reloading redit should be on reddit" &&& fun _ ->
    url "http://www.reddit.com/"
    on "http://www.reddit.com/"
    reload ()
    on "http://www.reddit.com/"

context "post reddit tests"
before (fun _ -> Console.WriteLine "only before set now")

"textbox should not equals dontequalme" &&& fun _ ->
    !^ testpage
    "#welcome" != "dontequalme"

"list should not have item" &&& fun _ ->
    !^ testpage
    "#value_list td" *!= "Value 5"

"ajax button should click" &&& fun _ ->
    !^ testpage
    "#ajax_button_clicked" == "ajax button not clicked"
    click "#ajax_button"
    "#ajax_button_clicked" == "ajax button clicked"

"pressing keys should work (may need to verify visually)" &&& fun _ ->
    !^ testpage
    click "#firstName"
    press tab
    press tab
    press down

"click polling" &&& fun _ -> 
    url "http://lefthandedgoat.github.io/canopy/testpages/autocomplete"
    click "#search"
    click "table tr td"
    "#console" == "worked"

"ajax button should click after sleep" &&& fun _ ->
    !^ testpage
    "#ajax_button_clicked" == "ajax button not clicked"
    sleep 2.5
    click "#ajax_button"
    "#ajax_button_clicked" == "ajax button clicked"

"readonly should throw error on read only field with clear" &&& fun _ ->
    failsWith "element #read_only is marked as read only, you can not clear read only elements"
    !^ "http://lefthandedgoat.github.io/canopy/testpages/readonly"    
    clear "#read_only"
        
"readonly should throw error on read only field with write" &&& fun _ ->
    failsWith "element #read_only is marked as read only, you can not write to read only elements"
    !^ "http://lefthandedgoat.github.io/canopy/testpages/readonly"    
    "#read_only" << "new text"

"when value is wrong and changes to empty string prior to time out, it should show wrong value, not empty string" &&& fun _ ->
    failsWith "equality check failed.  expected: John1, got: John"
    url testpage    
    "#firstName" == "John1"

context "other tests"

"define a custom wait for using any function that takes in unit and returns bool" &&& fun _ ->    
    let pageLoaded () = 
        (element "#wait_for").Text = "Done!"
    
    !^ "http://lefthandedgoat.github.io/canopy/testpages/waitFor"
    waitFor pageLoaded
    "#wait_for" == "Done!"

"define a custom wait for using any function that takes in unit and returns bool" &&& fun _ ->    
    failsWith <| sprintf "waiting for page to load%swaitFor condition failed to become true in 3.0 seconds" System.Environment.NewLine
    let pageLoaded () = 
        (element "#wait_for").Text = "Done!!!"
    
    !^ "http://lefthandedgoat.github.io/canopy/testpages/waitFor"
    waitFor2 "waiting for page to load" pageLoaded

"another example of another wait for, waiting on opacity to be 100% before clicking" &&& fun _ ->
    compareTimeout <- 10.0
    !^ "http://lefthandedgoat.github.io/canopy/testpages/noClickTilVisible"
    waitFor (fadedIn "#link")
    click "#link"
    on "http://lefthandedgoat.github.io/canopy/testpages/home"

"define a custom wait for using any function that takes in unit and returns bool, example using lists" &&& fun _ ->
    let fiveNumbersShown () = 
        (elements ".number").Length = 5
    
    !^ "http://lefthandedgoat.github.io/canopy/testpages/waitFor"
    waitFor fiveNumbersShown
    (elements ".number").Length === 5

"regex test" &&& fun _ ->
    url testpage
    "#lastName" << "Gray"
    "#lastName" =~ "Gr[ae]y"
    "#lastName" << "Grey"
    "#lastName" =~ "Gr[ae]y"

"regex one of many test" &&& fun _ ->
    url testpage
    "#colors li" *~ "gr[ea]y"

"test for first function" &&& fun _ ->
    !^ testpage
    (first "#value_list td").Text === "Value 1"

"test for last function" &&& fun _ ->
    !^ testpage    
    (last "#value_list td").Text === "Value 4"

"test for nth function" &&& fun _ ->
    !^ testpage
    (nth 2 "#value_list td").Text === "Value 3"

"writting (selecting) to drop down test" &&& fun _ ->
    !^ testpage
    "#item_list" << "Item 2"
    "#item_list" == "Item 2"

"writting (selecting) to drop down test, many options" &&& fun _ ->
    !^ testpage
    "#states" << "Kingman Reef"
    "#states" == "Kingman Reef"

"writting (selecting) to drop down test, via option value, many options" &&& fun _ ->
    //note that this can be turned off if its causing you problems via config.writeToSelectWithOptionValue
    !^ testpage
    "#states" << "95"
    "#states" == "Palmyra Atoll"
    
"double clicking" &&& fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/doubleClick"
    "#clicked" == "Not Clicked"
    doubleClick "#double_click"
    "#clicked" == "Clicked"

"displayed test" &&& fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/displayed"
    displayed "#displayed"

"displayed test via element" &&& fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/displayed"
    element "#displayed" |> displayed

"displayed test2" &&& fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/displayed"
    waitFor (fun _ -> (element "#displayed").Displayed)

"displayed test3" &&& fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/waitFor"
    waitFor (fun _ -> (element "#wait_for_2").Displayed)

"notDisplayed test" &&& fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/notDisplayed"
    notDisplayed "#notDisplayed"

"notDisplayed test via element" &&& fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/notDisplayed"
    element "#notDisplayed" |> notDisplayed

"notDisplayed test for element that is not on the screen" &&& fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/notDisplayed"
    notDisplayed "#nalsjdfalfalsdjfalsjfaljsflsjf"

"count test" &&& fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/count"
    count ".number" 5

"count test via sizzle" &&& fun _ ->
    !^ testpage
    count "option:selected" 2

"#firstName should have John (using == infix operator), iframe1" &&& fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/iframe1"
    "#firstName" == "John"

"elementWithin will find iFrame inside of outter element properly, iframe1" &&& fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/iframe1"
    first "body" |> elementWithin "#states" |> elementWithin "1" |> read |> is "Alabama" 

"#firstName should have John (using == infix operator), iframe2" &&& fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/iframe2"
    "#firstName" == "John"

"elementWithin will find iFrame inside of outter element properly, iframe2" &&& fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/iframe2"
    first "body" |> elementWithin "#states" |> elementWithin "1" |> read |> is "Alabama" 

"selecting option in iframe works by text and value" &&& fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/iframe1"    
    
    "#item_list" << "Item 2"
    "#item_list" == "Item 2"
    "#item_list" << "3"    
    "#item_list" == "Item 3"

context "hints tests"
"css hint" &&& fun _ ->
    url testpage
    let firstName = css "#firstName"
    firstName == "John"
    css "#lastName" == "Doe"

"xpath hint" &&& fun _ ->
    url testpage
    let firstName = xpath "id('firstName')"
    firstName == "John"
    xpath "id('lastName')" == "Doe"

"jquery hint" &&& fun _ ->
    url testpage
    let firstName = jquery "#firstName"
    firstName == "John"
    jquery "#lastName" == "Doe"

"value hint" &&& fun _ ->
    url testpage
    value "Click Me!!" == "Click Me!!"

"text hint" &&& fun _ ->
    url testpage
    text "ajax button not clicked" == "ajax button not clicked"

context "hovering"
"hover works" &&& fun _ ->
    url testpage
    "#hover" == "not hovered"
    hover "Milk"
    "#hover" == "hovered"
    
context "dragging"
"draging works" &&& fun _ ->
    url "http://scrumy.com/silenter39delayed"
    click ".plus-button a img"
    "#task_title" << "Demo"
    click "#task_editor_buttons .save_button"
    ".handle" --> ".inprogress"
    click "Blog"

if not (browser :? OpenQA.Selenium.PhantomJS.PhantomJSDriver) then
    context "alert tests"

    before (fun _ -> !^ "http://lefthandedgoat.github.io/canopy/testpages/alert")

    "alert box should have 'Alert Test'" &&& fun _ ->    
        click "#alert_test"
        alert() == "Alert Test"
        acceptAlert()

    "alert box should have 'Alert Test'" &&& fun _ ->
        click "#alert_test"
        alert() == "Alert Test"
        dismissAlert()

    "alert box should fail correctly when expecting wrong message" &&& fun _ -> 
        failsWith "equality check failed.  expected: Not the message, got: Alert Test"    
        click "#alert_test"
        alert() == "Not the message"

    "confirmation box should have 'Confirmation Test'" &&& fun _ ->
        click "#confirmation_test"
        alert() == "Confirmation Test"
        acceptAlert()

    "confirmation box should have 'Confirmation Test'" &&& fun _ ->
        click "#confirmation_test"
        alert() == "Confirmation Test"
        dismissAlert()

    "confirmation box should fail correctly when expecting wrong message" &&& fun _ ->
        failsWith "equality check failed.  expected: Not the message, got: Confirmation Test"
        click "#confirmation_test"
        alert() == "Not the message"

context "multiple elements test"

before (fun _ -> !^ "http://lefthandedgoat.github.io/canopy/testpages/")

"no error with multiple elements" &&& fun _ ->
    read (element "input") === "test value 1"

"error with multiple elements" &&& fun _ ->
    throwIfMoreThanOneElement <- true
    failsWith "More than one element was selected when only one was expected for selector: input"
    read (element "input") === "test value 1"
        

context "tiling windows"
"start multiple browsers and tile them" &&& fun _ ->
    start chrome
    let browser1 = browser
    start chrome
    let browser2 = browser

    tile [mainBrowser; browser1; browser2]
    sleep 1
    quit browser2
    quit browser1
    switchTo mainBrowser

context "User Agents tests"

"ChromeWithUserAgent userAgents.iPad should show as iPad" &&& fun _ ->
    start <| ChromeWithUserAgent userAgents.iPad
    url "http://whatsmyuseragent.com/"
    ".info" *~ "iPad"
    quit browser
    switchTo mainBrowser

"FirefoxDeviceWithUserAgent userAgents.iPhone should show as iPhone" &&& fun _ ->
    start <| FirefoxWithUserAgent userAgents.iPhone
    url "http://whatsmyuseragent.com/"
    ".info" *~ "iPhone"
    quit browser
    switchTo mainBrowser

"FirefoxDeviceWithUserAgent myagent should show as myagent" &&& fun _ ->
    start <| FirefoxWithUserAgent "myagent"
    url "http://whatsmyuseragent.com/"
    ".info" *~ "myagent"
    quit browser
    switchTo mainBrowser

context "Resize tests"

"Firefox should be resized to 400,400" &&& fun _ ->
    start firefox
    url "http://resizemybrowser.com/"
    resize (400,400)
    "#cWidth" == "400"
    "#cHeight" == "400"
    quit browser
    switchTo mainBrowser

"Chrome should be resized to iPhone4" &&& fun _ ->
    start chrome
    url "http://resizemybrowser.com/"
    resize screenSizes.iPhone4
    "#cWidth" == "320"
    "#cHeight" == "480"
    quit browser
    switchTo mainBrowser

"Firefox should be resized to 400,500 then rotated to 500,400" &&& fun _ ->
    start firefox
    url "http://resizemybrowser.com/"
    resize (400,500)
    rotate()
    "#cHeight" == "400"
    "#cWidth" == "500"
    quit browser
    switchTo mainBrowser

"Chrome should be resized and rotated to iPhone4" &&& fun _ ->
    start chrome
    url "http://resizemybrowser.com/"
    resize screenSizes.iPhone4
    rotate()
    "#cHeight" == "320"
    "#cWidth" == "480"
    quit browser
    switchTo mainBrowser

context "pluggable finders tests"

//example of a finder you could write so you didnt have to write boring selectors, just add this
//and let canopy do the work of trying to find something by convention
let findByHref href f =
    try
        let cssSelector = sprintf "a[href*='%s']" href
        f(By.CssSelector(cssSelector)) |> List.ofSeq
    with | ex -> []

addFinder findByHref

"test new findByHref by clicking an href" &&& fun _ ->
    url "http://lefthandedgoat.github.io/canopy/index.html"
    click "actions.html"
    on "http://lefthandedgoat.github.io/canopy/actions.html"

context "Navigate tests"

"Browser should navigate back and forward" &&& fun _ ->
  url "http://lefthandedgoat.github.io/canopy/index.html"
  click "actions.html"
  navigate back
  on "http://lefthandedgoat.github.io/canopy/index.html"
  navigate forward
  on "http://lefthandedgoat.github.io/canopy/actions.html"

context "todo tests"

"write a test that tests the whole internet!" &&& todo

let createTest k =
    let testName = sprintf "Testing addition performance %i" k
    testName &&& todo

let createTestSuite contextName n =
    context contextName

    [1..n] |> Seq.iter createTest

start chrome

createTestSuite "Add test performance" 1000

run ()
        
switchTo mainBrowser
coverage testpage
coverage()
coverage "http://scrumy.com/silenter39delayed"

quit()
