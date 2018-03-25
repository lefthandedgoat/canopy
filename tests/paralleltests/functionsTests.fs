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
    
    //"#firstName should have John (using == infix operator)"
    url testpage browser
    equals "#firstName" "John" browser

    //"id('firstName') should have John (using == infix operator), basic xpath test"
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