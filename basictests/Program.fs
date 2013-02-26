module main

open System
open runner
open canopy
open configuration
open reporters

start firefox
let mainBrowser = browser
elementTimeout <- 3.0
compareTimeout <- 3.0
runFailedContextsFirst <- true
reporter <- new LiveHtmlReporter() :> IReporter

context "context1"
once (fun _ -> Console.WriteLine "once")
before (fun _ -> Console.WriteLine "before")
after (fun _ -> Console.WriteLine "after")
lastly (fun _ -> Console.WriteLine "lastly")
 
let testpage = "http://localhost:4567/" 

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

"#lastName should have Doe" &&& (fun _ ->
    !^ testpage
    "#lastName" == "Doe")

"clearing #firstName sets text to new empty string" &&& (fun _ ->
    !^ testpage
    clear "#firstName"
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

"clicking #radio2 selects it" &&& (fun _ ->
    !^ testpage
    click "#radio2"
    selected "#radio2")

"clicking #checkbox selects it" &&& (fun _ ->
    !^ testpage
    check "#checkbox"
    selected "#checkbox")

"clicking selected #checkbox deselects it" &&& (fun _ ->
    !^ testpage
    check "#checkbox"
    uncheck "#checkbox"
    deselected "#checkbox")

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
    url "http://localhost:4567/autocomplete"
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
    !^ "http://localhost:4567/readonly"    
    clear "#read_only")
        
"readonly should throw error on read only field with write" &&& (fun _ ->
    failsWith "element #read_only is marked as read only, you can not write to read only elements"
    !^ "http://localhost:4567/readonly"    
    "#read_only" << "new text")

"when value is wrong and changes to empty string prior to time out, it should show wrong value, not empty string" &&& (fun _ ->
    failsWith "equality check failed.  expected: John1, got: John"
    url testpage    
    "#firstName" == "John1")

context "alert tests"
before (fun _ -> !^ "http://localhost:4567/alert")

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

context "other tests"

"define a custom wait for using any function that takes in unit and returns bool" &&& (fun _ ->    
    let pageLoaded () = 
        (element "#wait_for").Text = "Done!"
    
    !^ "http://localhost:4567/waitFor"
    waitFor pageLoaded
    "#wait_for" == "Done!")

"another example of another wait for, waiting on opacity to be 100% before clicking" &&& (fun _ ->
    compareTimeout <- 10.0
    !^ "http://localhost:4567/noClickTilVisible"
    waitFor (fadedIn "#link")
    click "#link"
    on "http://localhost:4567/home")

"define a custom wait for using any function that takes in unit and returns bool, example using lists" &&& (fun _ ->
    let fiveNumbersShown () = 
        (elements ".number").Length = 5
    
    !^ "http://localhost:4567/waitFor"
    waitFor fiveNumbersShown
    (elements ".number").Length === 5)

"regex test" &&& (fun _ ->
    url testpage
    "#lastName" << "Gray"
    "#lastName" =~ "Gr[ae]y"
    "#lastName" << "Grey"
    "#lastName" =~ "Gr[ae]y")

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

"double clicking" &&& (fun _ ->
    !^ "http://localhost:4567/doubleClick"
    "#clicked" == "Not Clicked"
    doubleClick "#double_click"
    "#clicked" == "Clicked")

"displayed test" &&& (fun _ ->
    !^ "http://localhost:4567/displayed"
    displayed "#displayed")

"displayed test2" &&& (fun _ ->
    !^ "http://localhost:4567/displayed"
    waitFor (fun _ -> (element "#displayed").Displayed))

"displayed test3" &&& (fun _ ->
    !^ "http://localhost:4567/waitFor"
    waitFor (fun _ -> (element "#wait_for_2").Displayed))

"notDisplayed test" &&& (fun _ ->
    !^ "http://localhost:4567/notDisplayed"
    notDisplayed "#notDisplayed")

"count test" &&& (fun _ ->
    !^ "http://localhost:4567/count"
    count ".number" 5)

context "dragging"
"draging works" &&& (fun _ ->
    url "http://scrumy.com/silenter39delayed"
    click ".plus-button a img"
    "#task_title" << "Demo"
    click "#task_editor_buttons .save_button"
    ".handle" >> ".inprogress")

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
)
run ()
        
switchTo mainBrowser
coverage testpage

//quit mainBrowser
