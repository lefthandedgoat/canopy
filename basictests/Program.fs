module main

open System
open runner
open canopy
open configuration
open reporters

suggestions := true
start firefox
let mainBrowser = browser
elementTimeout <- 3.0
compareTimeout <- 3.0
runFailedContextsFirst <- true
reporter <- new HtmlReporter() :> IReporter

context "context1"
once (fun _ -> Console.WriteLine "once")
before (fun _ -> Console.WriteLine "before")
after (fun _ -> Console.WriteLine "after")
lastly (fun _ -> Console.WriteLine "lastly")
 
let testpage = "http://localhost:4567" 

test (fun _ ->
    describe "#welcome should have Welcome"
    url testpage
    "#welcome" == "Welcome")

test (fun _ ->
    describe "find by label, following field"
    url testpage
    "Test Field 1" == "test value 1")

test (fun _ ->
    describe "find by label, for attribute"
    url testpage
    "Test Field 2" == "test value 2")

test (fun _ ->
    describe "#firstName should have John (using == infix operator)"
    url testpage
    "#firstName" == "John")

test (fun _ ->
    describe "#lastName should have Doe"
    !^ testpage
    "#lastName" == "Doe")

test (fun _ ->
    describe "clearing #firstName sets text to new empty string"
    !^ testpage
    clear "#firstName"
    "#firstName" == "")

test (fun _ ->
    describe "writing to #lastName sets text to new Smith"
    !^ testpage
    clear "#lastName"
    "#lastName" << "Smith"
    "#lastName" == "Smith")

test (fun _ ->
    describe "writing to #lastName sets text to new Smith (implicit clear in write)"
    !^ testpage
    "#lastName" << "Smith"
    "#lastName" == "Smith")

test (fun _ ->
    describe "#ajax label should have ajax loaded"
    !^ testpage
    "#ajax" == "ajax loaded")

test (fun _ ->
    describe "Value 1 listed in #value_list"
    !^ testpage
    "#value_list td" *= "Value 1")

test (fun _ ->
    describe "Value 2 listed in #value_list"
    !^ testpage
    "#value_list td" *= "Value 2")

test (fun _ ->
    describe "Value 3 listed in #value_list"
    !^ testpage
    "#value_list td" *= "Value 3")

test (fun _ ->
    describe "Value 4 listed in #value_list"
    !^ testpage
    "#value_list td" *= "Value 4")

test (fun _ ->
    describe "Item 1 listed in #item_list"
    !^ testpage
    "#item_list option" *= "Item 1")

test (fun _ ->
    describe "Item 2 listed in #item_list"
    !^ testpage
    "#item_list option" *= "Item 2")

test (fun _ ->
    describe "Item 3 listed in #item_list"
    !^ testpage
    "#item_list option" *= "Item 3")

test (fun _ ->
    describe "Item 4 listed in #item_list"
    !^ testpage
    "#item_list option" *= "Item 4")

test (fun _ ->
    describe "clicking #button sets #button_clicked to button clicked"
    !^ testpage
    "#button_clicked" == "button not clicked"
    click "#button"
    "#button_clicked" == "button clicked")

test (fun _ ->
    describe "clicking (element #button) sets #button_clicked to button clicked"
    !^ testpage
    "#button_clicked" == "button not clicked"
    click (element "#button")
    "#button_clicked" == "button clicked")

test (fun _ ->
    describe "clicking hyperlink sets #link_clicked to link clicked"
    !^ testpage
    "#link_clicked" == "link not clicked"
    click "#hyperlink"
    "#link_clicked" == "link clicked")

test (fun _ ->
    describe "clicking #radio1 selects it"
    !^ testpage
    click "#radio1"
    selected "#radio1")

test (fun _ ->
    describe "clicking #radio2 selects it"
    !^ testpage
    click "#radio2"
    selected "#radio2")

test (fun _ ->
    describe "clicking #checkbox selects it"
    !^ testpage
    check "#checkbox"
    selected "#checkbox")

test (fun _ ->
    describe "clicking selected #checkbox deselects it"
    !^ testpage
    check "#checkbox"
    uncheck "#checkbox"
    deselected "#checkbox")

context "reddit tests"
once (fun _ -> Console.WriteLine "once: reddit tests")
before (fun _ -> Console.WriteLine "before: reddit tests")
after (fun _ -> Console.WriteLine "after: reddit tests")
lastly (fun _ -> Console.WriteLine "lastly: reddit tests")

test (fun _ ->
    describe "browsing to redit should be on reddit"
    url "http://www.reddit.com/"
    on "http://www.reddit.com/")

test (fun _ ->
    describe "reloading redit should be on reddit"
    url "http://www.reddit.com/"
    on "http://www.reddit.com/"
    reload ()
    on "http://www.reddit.com/")

context "post reddit tests"
before (fun _ -> Console.WriteLine "only before set now")

test (fun _ ->
    describe "textbox should not equals dontequalme"
    !^ testpage
    "#welcome" != "dontequalme")

test (fun _ ->
    describe "list should not have item"
    !^ testpage
    "#value_list td" *!= "Value 5")

test (fun _ ->
    describe "ajax button should click"
    !^ testpage
    "#ajax_button_clicked" == "ajax button not clicked"
    click "#ajax_button"
    "#ajax_button_clicked" == "ajax button clicked")

test (fun _ ->
    describe "pressing keys should work (may need to verify visually)"
    !^ testpage
    click "#firstName"
    press tab
    press tab
    press down)

test (fun _ -> 
    describe "click polling"
    url "http://localhost:4567/autocomplete"
    click "#search"
    click "table tr td"
    "#console" == "worked")

test (fun _ ->
    describe "ajax button should click after sleep"
    !^ testpage
    "#ajax_button_clicked" == "ajax button not clicked"
    sleep 3
    click "#ajax_button"
    "#ajax_button_clicked" == "ajax button clicked")

test (fun _ ->
    describe "readonly should throw error on read only field with clear"
    failsWith "element #read_only is marked as read only, you can not clear read only elements"
    !^ "http://localhost:4567/readonly"    
    clear "#read_only")
        
test (fun _ ->
    describe "readonly should throw error on read only field with write"
    failsWith "element #read_only is marked as read only, you can not write to read only elements"
    !^ "http://localhost:4567/readonly"    
    "#read_only" << "new text")

test (fun _ ->
    describe "when value is wrong and changes to empty string prior to time out, it should show wrong value, not empty string"
    failsWith "equality check failed.  expected: John1, got: John"
    url testpage    
    "#firstName" == "John1")

context "alert tests"
before (fun _ -> !^ "http://localhost:4567/alert")

test (fun _ ->    
    describe "alert box should have 'Alert Test'"    
    click "#alert_test"
    alert() == "Alert Test"
    acceptAlert())

test (fun _ ->
    describe "alert box should have 'Alert Test'"    
    click "#alert_test"
    alert() == "Alert Test"
    dismissAlert())

test (fun _ ->
    describe "alert box should fail correctly when expecting wrong message"
    failsWith "equality check failed.  expected: Not the message, got: Alert Test"    
    click "#alert_test"
    alert() == "Not the message")

test (fun _ ->
    describe "confirmation box should have 'Confirmation Test'"    
    click "#confirmation_test"
    alert() == "Confirmation Test"
    acceptAlert())

test (fun _ ->
    describe "confirmation box should have 'Confirmation Test'"    
    click "#confirmation_test"
    alert() == "Confirmation Test"
    dismissAlert())

test (fun _ ->
    describe "confirmation box should fail correctly when expecting wrong message"
    failsWith "equality check failed.  expected: Not the message, got: Confirmation Test"
    click "#confirmation_test"
    alert() == "Not the message")

context "other tests"

test (fun _ ->
    describe "define a custom wait for using any function that takes in unit and returns bool"
    let pageLoaded () = 
        (element "#wait_for").Text = "Done!"
    
    !^ "http://localhost:4567/waitFor"
    waitFor pageLoaded
    "#wait_for" == "Done!")

test (fun _ ->
    compareTimeout <- 10.0
    describe "another example of another wait for, waiting on opacity to be 100% before clicking"
    !^ "http://localhost:4567/noClickTilVisible"
    waitFor (fadedIn "#link")
    click "#link"
    on "http://localhost:4567/home")

test (fun _ ->
    describe "define a custom wait for using any function that takes in unit and returns bool, example using lists"
    let fiveNumbersShown () = 
        (elements ".number").Length = 5
    
    !^ "http://localhost:4567/waitFor"
    waitFor fiveNumbersShown
    (elements ".number").Length === 5)

test (fun _ ->
    describe "regex test"
    url testpage
    "#lastName" << "Gray"
    "#lastName" =~ "Gr[ae]y"
    "#lastName" << "Grey"
    "#lastName" =~ "Gr[ae]y")

test (fun _ ->
    describe "test for first function"
    !^ testpage
    (first "#value_list td").Text === "Value 1")

test (fun _ ->
    describe "test for last function"
    !^ testpage
    (last "#value_list td").Text === "Value 4")

test (fun _ ->
    describe "test for nth function"
    !^ testpage
    (nth 2 "#value_list td").Text === "Value 3")

test (fun _ ->
    describe "writting (selecting) to drop down test"
    !^ testpage
    "#item_list" << "Item 2"
    "#item_list" == "Item 2")

test (fun _ ->
    describe "double clicking"
    !^ "http://localhost:4567/doubleClick"
    "#clicked" == "Not Clicked"
    doubleClick "#double_click"
    "#clicked" == "Clicked")

test (fun _ ->
    describe "displayed test"
    !^ "http://localhost:4567/displayed"
    displayed "#displayed")

test (fun _ ->
    describe "notDisplayed test"
    !^ "http://localhost:4567/notDisplayed"
    notDisplayed "#notDisplayed")

test (fun _ ->
    describe "count test"
    !^ "http://localhost:4567/count"
    count ".number" 5)

context "dragging"
test (fun _ ->
    describe "draging works"
    url "http://scrumy.com/silenter39delayed"
    click ".plus-button a img"
    "#task_title" << "Demo"
    click "#task_editor_buttons .save_button"
    ".handle" >> ".inprogress")

context "tiling windows"
test (fun _ ->
    describe "start multiple browsers and tile them"
    start firefox
    let browser1 = browser
    start firefox
    let browser2 = browser

    tile [mainBrowser; browser1; browser2]
    sleep 1
    quit browser2
    quit browser1
)
run ()

quit mainBrowser
