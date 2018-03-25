module functionsTests

open prunner
open canopy.parallell.functions

////////////////////////////////////
////////////////////////////////////
////////////////////////////////////
(*
These are examples of using the functions for parallel support
each function requires you to pass in the instance of the IWebDriver to use
If you do not want to do this, use the instanced version, see tests2.fs
Use whatever test runner you want, use whatever scoping strategy you want
I just use one I made so that I dont have to take external depencies for regression tests
*)
////////////////////////////////////
////////////////////////////////////
////////////////////////////////////

let add () = 
  let testpage = "http://lefthandedgoat.github.io/canopy/testpages/"

  "context1" &&& fun ctx -> 
    let browser = start canopy.types.Chrome
    
    //"#firstName should have John (using infix operator)"
    url testpage browser
    equals "#firstName" "John" browser

    //"id('firstName') should have John (using infix operator), basic xpath test"
    url testpage browser
    equals "id('firstName')" "John" browser

    //"#lastName should have Doe"
    url testpage browser
    equals "#lastName" "Doe" browser

    //"#lastName should have Doe via read cssSelector"
    url testpage browser
    read "#lastName" browser |> is "Doe"

    //"#lastName should have Doe via read IWebElements"
    url testpage browser
    element "#lastname" browser |> (fun x -> read x browser) |> is "Doe"

  //"clearing #firstName sets text to new empty string"
    url testpage browser
    clear "#firstName" browser
    equals "#firstName" "" browser

    //"clearing #firstName sets text to new empty string via IWebElement"
    url testpage browser
    let el = element "#firstName" browser
    clear el browser
    equals "#firstName" ""  browser

    //"writing to #lastName sets text to Smith"
    url testpage browser
    clear "#lastName" browser
    write "#lastName" "Smith" browser
    equals "#lastName" "Smith" browser

    //"writing to #lastName (as element) sets text to John"
    url testpage browser
    let lastname = element "#lastname" browser
    clear lastname browser
    write lastname "John" browser
    equals "#lastname" "John" browser

    //"writing to .lastName sets text to new Smith in both boxes"
    url testpage browser
    clear "#lastName" browser
    write ".lastName" "Smith" browser
    equals "#lastName" "Smith" browser
    equals "#lastName2" "Smith" browser

    //"writing to .lastName sets text to new Smith in both boxes, xpath test"
    url testpage browser
    clear "#lastName" browser
    write "//input[@class='lastName']" "Smith" browser
    equals "#lastName" "Smith" browser
    equals "#lastName2" "Smith" browser

    //"writing to #lastName sets text to new Smith (implicit clear in write)"
    url testpage browser
    write "#lastName" "Smith" browser
    equals "#lastName" "Smith" browser

    //"#ajax label should have ajax loaded"
    url testpage browser
    equals "#ajax" "ajax loaded" browser

    //"Value 1 listed in #value_list"
    url testpage browser
    oneOrManyEquals "#value_list td" "Value 1" browser

    //"Value 2 listed in #value_list"
    url testpage browser
    oneOrManyEquals "#value_list td" "Value 2" browser

    //"Value 3 listed in #value_list"
    url testpage browser
    oneOrManyEquals "#value_list td" "Value 3" browser

    //"Value 4 listed in #value_list"
    url testpage browser
    oneOrManyEquals "#value_list td" "Value 4" browser

    //"Item 1 listed in #item_list"
    url testpage browser
    oneOrManyEquals "#item_list option" "Item 1" browser

    //"Item 2 listed in #item_list"
    url testpage browser
    oneOrManyEquals "#item_list option" "Item 2" browser

    //"Item 3 listed in #item_list"
    url testpage browser
    oneOrManyEquals "#item_list option" "Item 3" browser

    //"Item 4 listed in #item_list"
    url testpage browser
    oneOrManyEquals "#item_list option" "Item 4" browser

    //"clicking #button sets #button_clicked to button clicked"
    url testpage browser
    equals "#button_clicked" "button not clicked" browser
    click "#button" browser
    equals "#button_clicked" "button clicked" browser

    //"clicking button with text 'Click Me!!' sets #button_clicked to button clicked"
    url testpage browser
    equals "#button_clicked" "button not clicked" browser
    click "Click Me!!" browser
    equals "#button_clicked" "button clicked" browser

    //"clicking (element #button) sets #button_clicked to button clicked"
    url testpage browser
    equals "#button_clicked" "button not clicked" browser
    click (element "#button" browser) browser
    equals "#button_clicked" "button clicked" browser

    //"clicking hyperlink sets #link_clicked to link clicked"
    url testpage browser
    equals "#link_clicked" "link not clicked" browser
    click "#hyperlink" browser
    equals "#link_clicked" "link clicked" browser

    //"clicking hyperlink via text sets #link_clicked to link clicked"
    url testpage browser
    equals "#link_clicked" "link not clicked" browser
    click "Click Me!" browser
    equals "#link_clicked" "link clicked" browser

    //"clicking #radio1 selects it"
    url testpage browser
    click "#radio1" browser
    selected "#radio1" browser

    //"clicking #radio1 selects it via IWebElement"
    url testpage browser
    click "#radio1" browser
    let el = element "#radio1" browser
    selected el browser

    //"clicking #radio2 selects it"
    url testpage browser
    click "#radio2" browser
    selected "#radio2" browser
    
    //"clicking #checkbox selects it"
    url testpage browser
    check "#checkbox" browser
    selected "#checkbox" browser

    //"clicking #checkbox selects it via sizzle"
    url testpage browser
    check "#checkbox" browser
    count "input:checked" 1 browser

    //"clicking selected #checkbox deselects it"
    url testpage browser
    check "#checkbox" browser
    uncheck "#checkbox" browser
    deselected "#checkbox" browser

    //"clicking selected #checkbox deselects it via IWebElement"
    url testpage browser
    check "#checkbox" browser
    uncheck "#checkbox" browser
    let el = element "#checkbox" browser
    deselected el browser

    //"rightClicking Works"
    url "https://api.jquery.com/contextmenu/" browser
    let iframe = element "iframe" browser
    browser.SwitchTo().Frame(iframe) |> ignore
    notDisplayed ".contextmenu" browser

    rightClick "div:first" browser
    displayed ".contextmenu" browser

    //"element within only searching within the element"
    url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin" browser
    count ".item" 5 browser
    "spanned item 4" === (element "span" browser |> (fun e -> elementWithin ".item" e browser)).Text

    //"elements within only searching within element"
    url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin" browser
    count ".item" 5 browser
    2 === (element "span" browser |> (fun e -> elementsWithin ".item" e browser) |> List.length)

    //"someElementWithin only searching within element"
    url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin" browser
    count ".item" 5 browser
    true === (element "span" browser |> (fun e -> someElementWithin ".specialItem" e browser)).IsSome

    //"element within works with jquery selectors"
    url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin" browser
    "spanned item 6" === (element ".specialItem:visible" browser |> (fun e -> parent e browser) |> (fun e -> elementWithin ".specialItem:visible" e browser)).Text

    //"element within works with jquery selectors and respects scope"
    url "http://lefthandedgoat.github.io/canopy/testpages/elementWithin" browser
    false === (element ".specialItem:visible" browser |> (fun e -> someElementWithin ".specialItem:visible" e browser)).IsSome

    //"parent of firstItem and secondItem is list"
    url "http://lefthandedgoat.github.io/canopy/testpages/parent" browser
    "list" === (element "#firstItem" browser |> (fun e -> parent e browser)).GetAttribute("id")

    //"some parent of firstItem and secondItem is list"
    url "http://lefthandedgoat.github.io/canopy/testpages/parent" browser
    true === (element "#firstItem"  browser |> (fun e -> someParent e browser)).IsSome
    "list" === (element "#firstItem"  browser |> (fun e -> someParent e browser)).Value.GetAttribute("id")

    //"someElement returns Some when element found"
    url testpage browser
    true === (someElement "#testfield2" browser).IsSome
    "test value 2" === (someElement "#testfield2" browser).Value.GetAttribute("value")

    //"someElement returns None when element not found"
    url testpage browser
    None === (someElement "#thisIdDoesNotExist" browser)
    
    //"Navigating to a url should be on url"
    url testpage browser
    on testpage browser

    //"Navigating to a url should be on exact url"
    url testpage browser
    onn testpage browser

    quit browser

  "reddit tests" &&& fun ctx -> 
    let browser = start canopy.types.Chrome

    //"browsing to redit should be on reddit '"
    url "http://www.reddit.com/" browser
    on "http://www.reddit.com/" browser

    //"reloading redit should be on reddit"
    url "http://www.reddit.com/" browser
    on "http://www.reddit.com/" browser
    reload browser
    on "http://www.reddit.com/" browser
    
    quit browser

  "post reddit tests" &&& fun ctx -> 
    let browser = start canopy.types.Chrome

    //"textbox should not equals dontequalme"
    url testpage browser
    notEquals "#welcome" "dontequalme" browser

    //"list should not have item"
    url testpage browser
    noneOfManyNotEquals "#value_list td" "Value 5" browser

    //"ajax button should click"
    url testpage browser
    equals "#ajax_button_clicked" "ajax button not clicked" browser
    click "#ajax_button" browser
    equals "#ajax_button_clicked" "ajax button clicked" browser

    //"pressing keys should work (may need to verify visually)"
    url testpage browser
    click "#firstName" browser
    press tab browser
    press tab browser
    press down browser

    //"click polling"
    url "http://lefthandedgoat.github.io/canopy/testpages/autocomplete" browser
    click "#search" browser
    click "table tr td" browser
    equals "#console" "worked" browser

    //"ajax button should click after sleep"
    url testpage browser
    equals "#ajax_button_clicked" "ajax button not clicked" browser
    sleep 2.5
    click "#ajax_button" browser
    equals "#ajax_button_clicked" "ajax button clicked" browser
            
    quit browser
    
  "other tests" &&& fun ctx -> 
    let browser = start canopy.types.Chrome

    //"define a custom wait for using any function that takes in unit and returns bool"
    let pageLoaded () =
        (element "#wait_for" browser).Text = "Done!"

    url "http://lefthandedgoat.github.io/canopy/testpages/waitFor" browser
    waitFor pageLoaded
    equals "#wait_for" "Done!" browser
        
    
    //"define a custom wait for using any function that takes in unit and returns bool, example using lists"
    let fiveNumbersShown () =
        (elements ".number" browser).Length = 5

    url "http://lefthandedgoat.github.io/canopy/testpages/waitFor" browser
    waitFor fiveNumbersShown
    (elements ".number" browser).Length === 5

    //"regex test"
    url testpage browser
    write "#lastName" "Gray" browser
    regexEquals "#lastName" "Gr[ae]y" browser
    write "#lastName" "Grey" browser
    regexEquals "#lastName""Gr[ae]y" browser

    //"regex not test"
    url testpage browser
    write "#lastName" "Gray" browser
    regexNotEquals "#lastName" "Gr[o]y" browser
    write "#lastName" "Grey" browser
    regexNotEquals "#lastName" "Gr[o]y" browser

    //"regex one of many test"
    url testpage browser
    oneOrManyRegexEquals "#colors li" "gr[ea]y" browser

    //"test for first function"
    url testpage browser
    (first "#value_list td" browser).Text === "Value 1"

    //"test for last function"
    url testpage browser
    (last "#value_list td" browser).Text === "Value 4"

    //"test for nth function"
    url testpage browser
    (nth 2 "#value_list td" browser).Text === "Value 3"

    //"writting (selecting) to drop down test"
    url testpage browser
    write "#item_list" "Item 2" browser
    equals "#item_list" "Item 2" browser

    //"writting (selecting) to drop down test, many options"
    url testpage browser
    write "#states" "Kingman Reef" browser
    equals "#states" "Kingman Reef" browser

    //"writting (selecting) to drop down test, via option value, many options"
    url testpage browser
    write "#states" "95" browser
    equals "#states" "Palmyra Atoll" browser

    //"writting (selecting) to drop down test, two selects same value"
    url "http://lefthandedgoat.github.io/canopy/testpages/selectOptions" browser
    write "#test-select" "g" browser
    write "#test-select2" "b" browser
    equals "#test-select" "Green" browser
    equals "#test-select2" "Blue" browser

    //"writting (selecting) to drop down test, value list, opt group"
    url testpage browser
    write "#test-select" "Audi" browser
    equals "#test-select" "Audi" browser

    //"double clicking"
    url "http://lefthandedgoat.github.io/canopy/testpages/doubleClick" browser
    equals "#clicked" "Not Clicked" browser
    doubleClick "#double_click" browser
    equals "#clicked" "Clicked" browser

    //"ctrl clicking"
    url "http://lefthandedgoat.github.io/canopy/testpages/ctrlClick" browser

    ctrlClick "One" browser
    ctrlClick "2" browser
    ctrlClick "Three" browser

    selected "1" browser
    selected "Two" browser
    selected "3" browser

    //"displayed test"
    url "http://lefthandedgoat.github.io/canopy/testpages/displayed" browser
    displayed "#displayed" browser

    //"displayed test via element"
    url "http://lefthandedgoat.github.io/canopy/testpages/displayed" browser
    element "#displayed" browser |> (fun e -> displayed e browser)

    //"displayed test2"
    url "http://lefthandedgoat.github.io/canopy/testpages/displayed" browser
    waitFor (fun _ -> (element "#displayed" browser).Displayed)

    //"displayed test3"
    url "http://lefthandedgoat.github.io/canopy/testpages/waitFor" browser
    waitFor (fun _ -> (element "#wait_for_2" browser).Displayed)

    //"notDisplayed test"
    url "http://lefthandedgoat.github.io/canopy/testpages/notDisplayed" browser
    notDisplayed "#notDisplayed" browser

    //"notDisplayed test via element"
    url "http://lefthandedgoat.github.io/canopy/testpages/notDisplayed" browser
    element "#notDisplayed"  browser |> (fun e -> notDisplayed e browser)

    //"notDisplayed test for element that is not on the screen"
    url "http://lefthandedgoat.github.io/canopy/testpages/notDisplayed" browser
    notDisplayed "#nalsjdfalfalsdjfalsjfaljsflsjf" browser

    //"count test"
    url "http://lefthandedgoat.github.io/canopy/testpages/count" browser
    count ".number" 5 browser

    //"count test via sizzle"
    url testpage browser
    count "option:selected" 3 browser

    //"#firstName should have John (using == infix operator), iframe1"
    url "http://lefthandedgoat.github.io/canopy/testpages/iframe1" browser
    equals "#firstName" "John" browser

    //"elementWithin will find iFrame inside of outter element properly, iframe1"
    url "http://lefthandedgoat.github.io/canopy/testpages/iframe1" browser
    first "body" browser 
    |> fun e -> elementWithin "#states" e browser
    |> fun e -> elementWithin "1" e browser
    |> fun e -> read e browser 
    |> is "Alabama"

    //"#firstName should have John (using == infix operator), iframe2"
    url "http://lefthandedgoat.github.io/canopy/testpages/iframe2" browser
    equals "#firstName" "John" browser

    //"elementWithin will find iFrame inside of outter element properly, iframe2"
    url "http://lefthandedgoat.github.io/canopy/testpages/iframe2" browser
    first "body" browser
    |> fun e -> elementWithin "#states" e browser
    |> fun e -> elementWithin "1" e browser
    |> fun e -> read e browser
    |> is "Alabama"

    //"selecting option in iframe works by text and value"
    url "http://lefthandedgoat.github.io/canopy/testpages/iframe1" browser

    write "#item_list" "Item 2" browser
    equals "#item_list" "Item 2" browser
    write "#item_list" "3" browser
    equals "#item_list" "Item 3" browser
        
    quit browser