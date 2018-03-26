module instancedTests

open prunner
open canopy.parallell.instanced
open canopy.types

////////////////////////////////////
////////////////////////////////////
////////////////////////////////////
(*
These are examples of using the instance for parallel support
each function hangs off instance and uses its internal instance implicitly
If you do not want to do this, use the instance version, see tests1.fs
Use whatever test runner you want
Use whatever scoping strategy you want
I just use one I made so that I dont have to take external depencies for regression tests
*)
////////////////////////////////////
////////////////////////////////////
////////////////////////////////////

let add () = 
  let ( === ) left right = if left <> right then failwith "%A NOT EQUAL %B" left right
  let testpage = "http://lefthandedgoat.github.io/canopy/testpages/"
        
  "context1" &&& fun ctx -> 
    let x = new Instance()
    x.start Chrome
    
    //"#firstName should have John (using infix operator)"
    x.url testpage
    x.equals "#firstName" "John"

    //"id('firstName') should have John (using infix operator), basic xpath test"
    x.url testpage
    x.equals "id('firstName')" "John"

    //"#lastName should have Doe"
    x.url testpage
    x.equals "#lastName" "Doe"

    //"#lastName should have Doe via read cssSelector"
    x.url testpage
    x.read "#lastName" |> x.is "Doe"

    //"#lastName should have Doe via read IWebElements"
    x.url testpage
    x.element "#lastname" |> (fun x' -> x.read x') |> x.is "Doe"

  //"clearing #firstName sets text to new empty string"
    x.url testpage
    x.clear "#firstName"
    x.equals "#firstName" ""

    //"clearing #firstName sets text to new empty string via IWebElement"
    x.url testpage
    let el = x.element "#firstName"
    x.clear el
    x.equals "#firstName" "" 

    //"writing to #lastName sets text to Smith"
    x.url testpage
    x.clear "#lastName"
    x.write "#lastName" "Smith"
    x.equals "#lastName" "Smith"

    //"writing to #lastName (as element) sets text to John"
    x.url testpage
    let lastname = x.element "#lastname"
    x.clear lastname
    x.write lastname "John"
    x.equals "#lastname" "John"

    //"writing to .lastName sets text to new Smith in both boxes"
    x.url testpage
    x.clear "#lastName"
    x.write ".lastName" "Smith"
    x.equals "#lastName" "Smith"
    x.equals "#lastName2" "Smith"

    //"writing to .lastName sets text to new Smith in both boxes, xpath test"
    x.url testpage
    x.clear "#lastName"
    x.write "//input[@class='lastName']" "Smith"
    x.equals "#lastName" "Smith"
    x.equals "#lastName2" "Smith"

    //"writing to #lastName sets text to new Smith (implicit clear in write)"
    x.url testpage
    x.write "#lastName" "Smith"
    x.equals "#lastName" "Smith"

    //"#ajax label should have ajax loaded"
    x.url testpage
    x.equals "#ajax" "ajax loaded"

    //"Value 1 listed in #value_list"
    x.url testpage
    x.oneOrManyEquals "#value_list td" "Value 1"

    //"Value 2 listed in #value_list"
    x.url testpage
    x.oneOrManyEquals "#value_list td" "Value 2"

    //"Value 3 listed in #value_list"
    x.url testpage
    x.oneOrManyEquals "#value_list td" "Value 3"

    //"Value 4 listed in #value_list"
    x.url testpage
    x.oneOrManyEquals "#value_list td" "Value 4"

    //"Item 1 listed in #item_list"
    x.url testpage
    x.oneOrManyEquals "#item_list option" "Item 1"

    //"Item 2 listed in #item_list"
    x.url testpage
    x.oneOrManyEquals "#item_list option" "Item 2"

    //"Item 3 listed in #item_list"
    x.url testpage
    x.oneOrManyEquals "#item_list option" "Item 3"

    //"Item 4 listed in #item_list"
    x.url testpage
    x.oneOrManyEquals "#item_list option" "Item 4"

    //"clicking #button sets #button_clicked to button clicked"
    x.url testpage
    x.equals "#button_clicked" "button not clicked"
    x.click "#button"
    x.equals "#button_clicked" "button clicked"

    //"clicking button with text 'Click Me!!' sets #button_clicked to button clicked"
    x.url testpage
    x.equals "#button_clicked" "button not clicked"
    x.click "Click Me!!"
    x.equals "#button_clicked" "button clicked"

    //"clicking (element #button) sets #button_clicked to button clicked"
    x.url testpage
    x.equals "#button_clicked" "button not clicked"
    x.click (x.element "#button")
    x.equals "#button_clicked" "button clicked"

    //"clicking hyperlink sets #link_clicked to link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"

    //"clicking hyperlink via text sets #link_clicked to link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "Click Me!"
    x.equals "#link_clicked" "link clicked"

    //"clicking #radio1 selects it"
    x.url testpage
    x.click "#radio1"
    x.selected "#radio1"

    //"clicking #radio1 selects it via IWebElement"
    x.url testpage
    x.click "#radio1"
    let el = x.element "#radio1"
    x.selected el

    //"clicking #radio2 selects it"
    x.url testpage
    x.click "#radio2"
    x.selected "#radio2"
    
    //"clicking #checkbox selects it"
    x.url testpage
    x.check "#checkbox"
    x.selected "#checkbox"

    //"clicking #checkbox selects it via sizzle"
    x.url testpage
    x.check "#checkbox"
    x.count "input:checked" 1

    //"clicking selected #checkbox deselects it"
    x.url testpage
    x.check "#checkbox"
    x.uncheck "#checkbox"
    x.deselected "#checkbox"

    //"clicking selected #checkbox deselects it via IWebElement"
    x.url testpage
    x.check "#checkbox"
    x.uncheck "#checkbox"
    let el = x.element "#checkbox"
    x.deselected el

    //"rightClicking Works"
    x.url "https://api.jquery.com/contextmenu/"
    let iframe = x.element "iframe"
    x.browser.SwitchTo().Frame(iframe) |> ignore
    x.notDisplayed ".contextmenu"

    x.rightClick "div:first"
    x.displayed ".contextmenu"
    
    //"element within only searching within the element"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin"
    x.count ".item" 5
    "spanned item 4" === (x.element "span" |> x.elementWithin ".item").Text

    //"elements within only searching within element"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin"
    x.count ".item" 5
    2 === (x.element "span" |> x.elementsWithin ".item" |> List.length)

    //"someElementWithin only searching within element"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin"
    x.count ".item" 5
    true === (x.element "span" |> x.someElementWithin ".specialItem").IsSome

    //"element within works with jquery selectors"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin"
    "spanned item 6" === (x.element ".specialItem:visible" |> x.parent |> x.elementWithin ".specialItem:visible").Text

    //"element within works with jquery selectors and respects scope"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin"
    false === (x.element ".specialItem:visible" |> x.someElementWithin ".specialItem:visible").IsSome

    //"parent of firstItem and secondItem is list"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/parent"
    "list" === (x.element "#firstItem" |> x.parent).GetAttribute("id")

    //"some parent of firstItem and secondItem is list"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/parent"
    true === (x.element "#firstItem" |> x.someParent).IsSome
    "list" === (x.element "#firstItem" |> x.someParent).Value.GetAttribute("id")

    //"someElement returns Some when element found"
    x.url testpage
    true === (x.someElement "#testfield2").IsSome
    "test value 2" === (x.someElement "#testfield2").Value.GetAttribute("value")

    //"someElement returns None when element not found"
    x.url testpage
    None === (x.someElement "#thisIdDoesNotExist")
    
    //"Navigating to a url should be on url"
    x.url testpage
    x.on testpage

    //"Navigating to a url should be on exact url"
    x.url testpage
    x.onn testpage

    x.quit()

  "reddit tests" &&& fun ctx -> 
    let x = new Instance()
    x.start Chrome

    //"browsing to redit should be on reddit '"
    x.url "http://www.reddit.com/"
    x.on "http://www.reddit.com/"

    //"reloading redit should be on reddit"
    x.url "http://www.reddit.com/"
    x.on "http://www.reddit.com/"
    x.reload()
    x.on "http://www.reddit.com/"
    
    x.quit()

  "post reddit tests" &&& fun ctx -> 
    let x = new Instance()
    x.start Chrome

    //"textbox should not equals dontequalme"
    x.url testpage
    x.notEquals "#welcome" "dontequalme"

    //"list should not have item"
    x.url testpage
    x.noneOfManyNotEquals "#value_list td" "Value 5"

    //"ajax button should click"
    x.url testpage
    x.equals "#ajax_button_clicked" "ajax button not clicked"
    x.click "#ajax_button"
    x.equals "#ajax_button_clicked" "ajax button clicked"

    //"pressing keys should work (may need to verify visually)"
    x.url testpage
    x.click "#firstName"
    x.press x.tab
    x.press x.tab
    x.press x.down

    //"click polling"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/autocomplete"
    x.click "#search"
    x.click "table tr td"
    x.equals "#console" "worked"

    //"ajax button should click after sleep"
    x.url testpage
    x.equals "#ajax_button_clicked" "ajax button not clicked"
    x.sleep 2.5
    x.click "#ajax_button"
    x.equals "#ajax_button_clicked" "ajax button clicked"
            
    x.quit()
    
  "other tests" &&& fun ctx -> 
    let x = new Instance()
    x.start Chrome

    //"define a custom wait for using any function that takes in unit and returns bool"
    let pageLoaded () =
        (x.element "#wait_for").Text = "Done!"

    x.url "http://lefthandedgoat.github.io/canopy/testpages/waitFor"
    x.waitFor pageLoaded
    x.equals "#wait_for" "Done!"
        
    
    //"define a custom wait for using any function that takes in unit and returns bool, example using lists"
    let fiveNumbersShown () =
        (x.elements ".number").Length = 5

    x.url "http://lefthandedgoat.github.io/canopy/testpages/waitFor"
    x.waitFor fiveNumbersShown
    (x.elements ".number").Length === 5

    //"regex test"
    x.url testpage
    x.write "#lastName" "Gray"
    x.regexEquals "#lastName" "Gr[ae]y"
    x.write "#lastName" "Grey"
    x.regexEquals "#lastName""Gr[ae]y"

    //"regex not test"
    x.url testpage
    x.write "#lastName" "Gray"
    x.regexNotEquals "#lastName" "Gr[o]y"
    x.write "#lastName" "Grey"
    x.regexNotEquals "#lastName" "Gr[o]y"

    //"regex one of many test"
    x.url testpage
    x.oneOrManyRegexEquals "#colors li" "gr[ea]y"

    //"test for first function"
    x.url testpage
    (x.first "#value_list td").Text === "Value 1"

    //"test for last function"
    x.url testpage
    (x.last "#value_list td").Text === "Value 4"

    //"test for nth function"
    x.url testpage
    (x.nth 2 "#value_list td").Text === "Value 3"

    //"writting (selecting) to drop down test"
    x.url testpage
    x.write "#item_list" "Item 2"
    x.equals "#item_list" "Item 2"

    //"writting (selecting) to drop down test, many options"
    x.url testpage
    x.write "#states" "Kingman Reef"
    x.equals "#states" "Kingman Reef"

    //"writting (selecting) to drop down test, via option value, many options"
    x.url testpage
    x.write "#states" "95"
    x.equals "#states" "Palmyra Atoll"

    //"writting (selecting) to drop down test, two selects same value"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/selectOptions"
    x.write "#test-select" "g"
    x.write "#test-select2" "b"
    x.equals "#test-select" "Green"
    x.equals "#test-select2" "Blue"

    //"writting (selecting) to drop down test, value list, opt group"
    x.url testpage
    x.write "#test-select" "Audi"
    x.equals "#test-select" "Audi"

    //"double clicking"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/doubleClick"
    x.equals "#clicked" "Not Clicked"
    x.doubleClick "#double_click"
    x.equals "#clicked" "Clicked"

    //"ctrl clicking"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/ctrlClick"

    x.ctrlClick "One"
    x.ctrlClick "2"
    x.ctrlClick "Three"

    x.selected "1"
    x.selected "Two"
    x.selected "3"

    //"displayed test"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/displayed"
    x.displayed "#displayed"

    //"displayed test via element"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/displayed"
    x.element "#displayed" |> x.displayed

    //"displayed test2"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/displayed"
    x.waitFor (fun _ -> (x.element "#displayed").Displayed)

    //"displayed test3"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/waitFor"
    x.waitFor (fun _ -> (x.element "#wait_for_2").Displayed)

    //"notDisplayed test"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/notDisplayed"
    x.notDisplayed "#notDisplayed"

    //"notDisplayed test via element"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/notDisplayed"
    x.element "#notDisplayed"  |> x.notDisplayed

    //"notDisplayed test for element that is not on the screen"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/notDisplayed"
    x.notDisplayed "#nalsjdfalfalsdjfalsjfaljsflsjf"

    //"count test"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/count"
    x.count ".number" 5

    //"count test via sizzle"
    x.url testpage
    x.count "option:selected" 3

    //"#firstName should have John (using == infix operator), iframe1"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/iframe1"
    x.equals "#firstName" "John"

    //"elementWithin will find iFrame inside of outter element properly, iframe1"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/iframe1"
    x.first "body" 
    |> x.elementWithin "1"
    |> x.read
    |> x.is "Alabama"

    //"#firstName should have John (using == infix operator), iframe2"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/iframe2"
    x.equals "#firstName" "John"

    //"elementWithin will find iFrame inside of outter element properly, iframe2"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/iframe2"
    x.first "body"
    |> x.elementWithin "#states"
    |> x.elementWithin "1"
    |> x.read
    |> x.is "Alabama"

    //"selecting option in iframe works by text and value"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/iframe1"

    x.write "#item_list" "Item 2"
    x.equals "#item_list" "Item 2"
    x.write "#item_list" "3"
    x.equals "#item_list" "Item 3"
        
    x.quit()

  "the rest" &&& fun ctx -> 
    let x = new Instance()
    x.start Chrome

    //"css hint"
    x.url testpage
    let firstName = x.css "#firstName"
    x.equals firstName "John"
    x.equals (x.css "#lastName") "Doe"

    //"xpath hint"
    x.url testpage
    let firstName = x.xpath "id('firstName')"
    x.equals firstName "John"
    x.equals (x.xpath "id('lastName')") "Doe"

    //"jquery hint"
    x.url testpage
    let firstName = x.jquery "#firstName"
    x.equals firstName "John"
    x.equals (x.jquery "#lastName") "Doe"

    //"value hint"
    x.url testpage
    x.equals (x.value "Click Me!!") "Click Me!!"

    //"text hint"
    x.url testpage
    x.equals (x.text "ajax button not clicked") "ajax button not clicked"
      
    //"hover works"
    x.url testpage
    x.equals "#hover" "not hovered"
    x.hover "Milk"
    x.equals "#hover" "hovered"
  
    //"draging works" &&! fun _ ->
    x.url "http://scrumy.com/silenter39delayed"
    x.click ".plus-button a img"
    x.write "#task_title" "Demo"
    x.click "#task_editor_buttons .save_button"
    x.drag ".handle" ".inprogress"
    x.click "Blog"
        
    //"alert box should have 'Alert Test'"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/alert"
    x.click "#alert_test"
    x.equals (x.alert()) "Alert Test"
    x.acceptAlert ()

    //"alert box should have 'Alert Test'"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/alert"
    x.click "#alert_test"
    x.equals (x.alert()) "Alert Test"
    x.dismissAlert()

    //"confirmation box should have 'Confirmation Test'"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/alert"
    x.click "#confirmation_test"
    x.equals (x.alert()) "Confirmation Test"
    x.acceptAlert()

    //"confirmation box should have 'Confirmation Test'"
    x.url "http://lefthandedgoat.github.io/canopy/testpages/alert"
    x.click "#confirmation_test"
    x.equals (x.alert()) "Confirmation Test"
    x.dismissAlert()

    x.quit()