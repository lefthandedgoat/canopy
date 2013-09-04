module main

open System
open canopy
open runner
open configuration
open reporters

start chrome
let mainBrowser = browser
elementTimeout <- 3.0
compareTimeout <- 3.0
runFailedContextsFirst <- true
reporter <- new LiveHtmlReporter() :> IReporter

failFast := true

context "context1"
once (fun _ -> Console.WriteLine "once")
before (fun _ -> Console.WriteLine "before")
after (fun _ -> Console.WriteLine "after")
lastly (fun _ -> Console.WriteLine "lastly")
 
let testpage = "http://lefthandedgoat.github.io/canopy/testpages/" 

"intentionally skipped shows blue in LiveHtmlReport" &&! skipped

"#welcome should have Welcome" &&& (fun _ ->    
    url testpage
    "#welcome" == "Welcome")

//ntest "description" (fun _ -> url "http://www.google.com")
//and
//"description" &&& (fun _ -> url "http://www.google.com")
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

"#firstName should have John (using == infix operator)" &&& (fun _ ->
    url testpage
    "#firstName" == "John")

"id('firstName') should have John (using == infix operator), basic xpath test" &&& (fun _ ->
    url testpage
    "id('firstName')" == "John")

"#lastName should have Doe" &&& (fun _ ->
    !^ testpage
    "#lastName" == "Doe")

"#lastName should have Doe via read cssSelector" &&& fun _ ->
    !^ testpage
    read "#lastName" |> is "Doe"

"#lastName should have Doe via read IWebElements" &&& fun _ ->
    !^ testpage
    element "#lastname" |> read |> is "Doe"

"clearing #firstName sets text to new empty string" &&& (fun _ ->
    !^ testpage
    clear "#firstName"
    "#firstName" == "")

"clearing #firstName sets text to new empty string via IWebElement" &&& (fun _ ->
    !^ testpage
    element "#firstName" |> clear
    "#firstName" == "")

"writing to #lastName sets text to Smith" &&& (fun _ ->
    !^ testpage
    clear "#lastName"
    "#lastName" << "Smith"
    "#lastName" == "Smith")

"writing to .lastName sets text to new Smith in both boxes" &&& (fun _ ->
    !^ testpage
    clear "#lastName"
    ".lastName" << "Smith"
    "#lastName" == "Smith"
    "#lastName2" == "Smith")

"writing to .lastName sets text to new Smith in both boxes, xpath test" &&& (fun _ ->
    !^ testpage
    clear "#lastName"
    "//input[@class='lastName']" << "Smith"
    "#lastName" == "Smith"
    "#lastName2" == "Smith")

"writing to #lastName sets text to new Smith (implicit clear in write)" &&& (fun _ ->
    !^ testpage
    "#lastName" << "Smith"
    "#lastName" == "Smith")

"#ajax label should have ajax loaded" &&& (fun _ ->
    !^ testpage
    "#ajax" == "ajax loaded")

"Value 1 listed in #value_list" &&& (fun _ ->
    !^ testpage
    "#value_list td" *= "Value 1")

"Value 2 listed in #value_list" &&& (fun _ ->
    !^ testpage
    "#value_list td" *= "Value 2")

"Value 3 listed in #value_list" &&& (fun _ ->
    !^ testpage
    "#value_list td" *= "Value 3")

"Value 4 listed in #value_list" &&& (fun _ ->
    !^ testpage
    "#value_list td" *= "Value 4")

"Item 1 listed in #item_list" &&& (fun _ ->
    !^ testpage
    "#item_list option" *= "Item 1")

"Item 2 listed in #item_list" &&& (fun _ ->
    !^ testpage
    "#item_list option" *= "Item 2")

"Item 3 listed in #item_list" &&& (fun _ ->
    !^ testpage
    "#item_list option" *= "Item 3")

"Item 4 listed in #item_list" &&& (fun _ ->
    !^ testpage
    "#item_list option" *= "Item 4")

"clicking #button sets #button_clicked to button clicked" &&& (fun _ ->
    !^ testpage
    "#button_clicked" == "button not clicked"
    click "#button"
    "#button_clicked" == "button clicked")

"clicking button with text 'Click Me!!' sets #button_clicked to button clicked" &&& (fun _ ->
    !^ testpage
    "#button_clicked" == "button not clicked"
    click "Click Me!!"
    "#button_clicked" == "button clicked")

"clicking (element #button) sets #button_clicked to button clicked" &&& (fun _ ->
    !^ testpage
    "#button_clicked" == "button not clicked"
    click (element "#button")
    "#button_clicked" == "button clicked")

"clicking hyperlink sets #link_clicked to link clicked" &&& (fun _ ->
    !^ testpage
    "#link_clicked" == "link not clicked"
    click "#hyperlink"
    "#link_clicked" == "link clicked")

"clicking hyperlink via text sets #link_clicked to link clicked" &&& (fun _ ->
    !^ testpage
    "#link_clicked" == "link not clicked"
    click "Click Me!"
    "#link_clicked" == "link clicked")

"clicking #radio1 selects it" &&& (fun _ ->
    !^ testpage
    click "#radio1"
    selected "#radio1")

"clicking #radio1 selects it via IWebElement" &&& (fun _ ->
    !^ testpage
    click "#radio1"
    element "#radio1" |> selected)

"clicking #radio2 selects it" &&& (fun _ ->
    !^ testpage
    click "#radio2"
    selected "#radio2")

"clicking #checkbox selects it" &&& (fun _ ->
    !^ testpage
    check "#checkbox"
    selected "#checkbox")

"clicking #checkbox selects it via sizzle" &&& (fun _ ->
    !^ testpage
    check "#checkbox"
    count "input:checked" 1)

"clicking selected #checkbox deselects it" &&& (fun _ ->
    !^ testpage
    check "#checkbox"
    uncheck "#checkbox"
    deselected "#checkbox")

"clicking selected #checkbox deselects it via IWebElement" &&& (fun _ ->
    !^ testpage
    check "#checkbox"
    uncheck "#checkbox"
    element "#checkbox" |> deselected)

"element within only searching within the element" &&& (fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin"
    count ".item" 5
    "spanned item 4" === (element "span" |> elementWithin ".item").Text)

"elements within only searching within element" &&& (fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin"
    count ".item" 5
    2 === (element "span" |> elementsWithin ".item" |> List.length))

"someElementWithin only searching within element" &&& (fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin"
    count ".item" 5
    true === (element "span" |> someElementWithin ".specialItem").IsSome)

"parent of firstItem and secondItem is list" &&& (fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/parent"
    "list" === (element "#firstItem" |> parent).GetAttribute("id"))

"some parent of firstItem and secondItem is list" &&& (fun _ ->
    url "http://lefthandedgoat.github.io/canopy/testpages/parent"
    true === (element "#firstItem" |> someParent).IsSome
    "list" === (element "#firstItem" |> someParent).Value.GetAttribute("id"))

"someElement returns Some when element found" &&& (fun _ ->
    url testpage
    true === (someElement "#testfield2").IsSome
    "test value 2" === (someElement "#testfield2").Value.GetAttribute("value"))

"someElement returns None when element not found" &&& (fun _ ->
    url testpage
    None === (someElement "#thisIdDoesNotExist"))

"someElement fails when more than one element found" &&& (fun _ ->
    url testpage    
    failsWith "More than one element was selected when only one was expected for selector: .lastName"
    someElement ".lastName" |> ignore)

context "reddit tests"
once (fun _ -> Console.WriteLine "once: reddit tests")
before (fun _ -> Console.WriteLine "before: reddit tests")
after (fun _ -> Console.WriteLine "after: reddit tests")
lastly (fun _ -> Console.WriteLine "lastly: reddit tests")

"browsing to redit should be on reddit" &&& (fun _ ->
    url "http://www.reddit.com/"
    on "http://www.reddit.com/")

"reloading redit should be on reddit" &&& (fun _ ->
    url "http://www.reddit.com/"
    on "http://www.reddit.com/"
    reload ()
    on "http://www.reddit.com/")

context "post reddit tests"
before (fun _ -> Console.WriteLine "only before set now")

"textbox should not equals dontequalme" &&& (fun _ ->
    !^ testpage
    "#welcome" != "dontequalme")

"list should not have item" &&& (fun _ ->
    !^ testpage
    "#value_list td" *!= "Value 5")

"ajax button should click" &&& (fun _ ->
    !^ testpage
    "#ajax_button_clicked" == "ajax button not clicked"
    click "#ajax_button"
    "#ajax_button_clicked" == "ajax button clicked")

"pressing keys should work (may need to verify visually)" &&& (fun _ ->
    !^ testpage
    click "#firstName"
    press tab
    press tab
    press down)

"click polling" &&& (fun _ -> 
    url "http://lefthandedgoat.github.io/canopy/testpages/autocomplete"
    click "#search"
    click "table tr td"
    "#console" == "worked")

"ajax button should click after sleep" &&& (fun _ ->
    !^ testpage
    "#ajax_button_clicked" == "ajax button not clicked"
    sleep 3
    click "#ajax_button"
    "#ajax_button_clicked" == "ajax button clicked")

"readonly should throw error on read only field with clear" &&& (fun _ ->
    failsWith "element #read_only is marked as read only, you can not clear read only elements"
    !^ "http://lefthandedgoat.github.io/canopy/testpages/readonly"    
    clear "#read_only")
        
"readonly should throw error on read only field with write" &&& (fun _ ->
    failsWith "element #read_only is marked as read only, you can not write to read only elements"
    !^ "http://lefthandedgoat.github.io/canopy/testpages/readonly"    
    "#read_only" << "new text")

"when value is wrong and changes to empty string prior to time out, it should show wrong value, not empty string" &&& (fun _ ->
    failsWith "equality check failed.  expected: John1, got: John"
    url testpage    
    "#firstName" == "John1")

context "other tests"

"define a custom wait for using any function that takes in unit and returns bool" &&& (fun _ ->    
    let pageLoaded () = 
        (element "#wait_for").Text = "Done!"
    
    !^ "http://lefthandedgoat.github.io/canopy/testpages/waitFor"
    waitFor pageLoaded
    "#wait_for" == "Done!")

"another example of another wait for, waiting on opacity to be 100% before clicking" &&& (fun _ ->
    compareTimeout <- 10.0
    !^ "http://lefthandedgoat.github.io/canopy/testpages/noClickTilVisible"
    waitFor (fadedIn "#link")
    click "#link"
    on "http://lefthandedgoat.github.io/canopy/testpages/home")

"define a custom wait for using any function that takes in unit and returns bool, example using lists" &&& (fun _ ->
    let fiveNumbersShown () = 
        (elements ".number").Length = 5
    
    !^ "http://lefthandedgoat.github.io/canopy/testpages/waitFor"
    waitFor fiveNumbersShown
    (elements ".number").Length === 5)

"regex test" &&& (fun _ ->
    url testpage
    "#lastName" << "Gray"
    "#lastName" =~ "Gr[ae]y"
    "#lastName" << "Grey"
    "#lastName" =~ "Gr[ae]y")

"regex one of many test" &&& (fun _ ->
    url testpage
    "#colors li" *~ "gr[ea]y")    

"test for first function" &&& (fun _ ->
    !^ testpage
    (first "#value_list td").Text === "Value 1")

"test for last function" &&& (fun _ ->
    !^ testpage    
    (last "#value_list td").Text === "Value 4")

"test for nth function" &&& (fun _ ->
    !^ testpage
    (nth 2 "#value_list td").Text === "Value 3")

"writting (selecting) to drop down test" &&& (fun _ ->
    !^ testpage
    "#item_list" << "Item 2"
    "#item_list" == "Item 2")

"writting (selecting) to drop down test, many options" &&& (fun _ ->
    !^ testpage
    "#states" << "Kingman Reef"
    "#states" == "Kingman Reef")
    
"double clicking" &&& (fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/doubleClick"
    "#clicked" == "Not Clicked"
    doubleClick "#double_click"
    "#clicked" == "Clicked")

"displayed test" &&& (fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/displayed"
    displayed "#displayed")

"displayed test2" &&& (fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/displayed"
    waitFor (fun _ -> (element "#displayed").Displayed))

"displayed test3" &&& (fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/waitFor"
    waitFor (fun _ -> (element "#wait_for_2").Displayed))

"notDisplayed test" &&& (fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/notDisplayed"
    notDisplayed "#notDisplayed")

"notDisplayed test for element that is not on the screen" &&& (fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/notDisplayed"
    notDisplayed "#nalsjdfalfalsdjfalsjfaljsflsjf")

"count test" &&& (fun _ ->
    !^ "http://lefthandedgoat.github.io/canopy/testpages/count"
    count ".number" 5)

"count test via sizzle" &&& (fun _ ->
    !^ testpage
    count "option:selected" 2)

context "dragging"
"draging works" &&& (fun _ ->
    url "http://scrumy.com/silenter39delayed"
    click ".plus-button a img"
    "#task_title" << "Demo"
    click "#task_editor_buttons .save_button"
    ".handle" --> ".inprogress")

if not (browser :? OpenQA.Selenium.PhantomJS.PhantomJSDriver) then
    context "alert tests"

    before (fun _ -> !^ "http://lefthandedgoat.github.io/canopy/testpages/alert")

    "alert box should have 'Alert Test'" &&& (fun _ ->    
        click "#alert_test"
        alert() == "Alert Test"
        acceptAlert())

    "alert box should have 'Alert Test'" &&& (fun _ ->
        click "#alert_test"
        alert() == "Alert Test"
        dismissAlert())

    "alert box should fail correctly when expecting wrong message" &&& (fun _ -> 
        failsWith "equality check failed.  expected: Not the message, got: Alert Test"    
        click "#alert_test"
        alert() == "Not the message")

    "confirmation box should have 'Confirmation Test'" &&& (fun _ ->
        click "#confirmation_test"
        alert() == "Confirmation Test"
        acceptAlert())

    "confirmation box should have 'Confirmation Test'" &&& (fun _ ->
        click "#confirmation_test"
        alert() == "Confirmation Test"
        dismissAlert())

    "confirmation box should fail correctly when expecting wrong message" &&& (fun _ ->
        failsWith "equality check failed.  expected: Not the message, got: Confirmation Test"
        click "#confirmation_test"
        alert() == "Not the message")

context "tiling windows"
"start multiple browsers and tile them" &&& (fun _ ->
    start chrome
    let browser1 = browser
    start chrome
    let browser2 = browser

    tile [mainBrowser; browser1; browser2]
    sleep 1
    quit browser2
    quit browser1
    switchTo mainBrowser
)

context "todo tests"
"write a test that tests the whole internet!" &&& todo

run ()
        
switchTo mainBrowser
coverage testpage
coverage()
coverage "http://scrumy.com/silenter39delayed"

//quit mainBrowser
