module main

open runner
open canopy

before <- fun () -> System.Console.WriteLine("This happens before every test")
suggestions := true
start "firefox"
 
let testpage = "http://localhost:4567" 

test (fun _ ->
    describe "#welcome should have Welcome"
    url testpage
    read "#welcome" |> equals "Welcome")

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
    write "#lastName" "Smith"
    "#lastName" == "Smith")

test (fun _ ->
    describe "#ajax label should have ajax loaded"
    !^ testpage
    "#ajax" == "ajax loaded")

test (fun _ ->
    describe "Value 1 listed in #value_list"
    !^ testpage
    listed "#value_list td" "Value 1")

test (fun _ ->
    describe "Value 2 listed in #value_list"
    !^ testpage
    listed "#value_list td" "Value 2")

test (fun _ ->
    describe "Value 3 listed in #value_list (using *= infix operator)"
    !^ testpage
    "#value_list td" *= "Value 3")

test (fun _ ->
    describe "Value 4 listed in #value_list (using *= infix operator)"
    !^ testpage
    "#value_list td" *= "Value 4")

test (fun _ ->
    describe "Item 1 listed in #item_list"
    !^ testpage
    listed "#item_list option" "Item 1")

test (fun _ ->
    describe "Item 2 listed in #item_list"
    !^ testpage
    listed "#item_list option" "Item 2")

test (fun _ ->
    describe "Item 3 listed in #item_list (using *= infix operator)"
    !^ testpage
    "#item_list option" *= "Item 3")

test (fun _ ->
    describe "Item 4 listed in #item_list (using *= infix operator)"
    !^ testpage
    "#item_list option" *= "Item 4")

test (fun _ ->
    describe "clicking #button sets #button_clicked to button clicked"
    !^ testpage
    "#button_clicked" == "button not clicked"
    click "#button"
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
    click "#checkbox"
    selected "#checkbox")

test (fun _ ->
    describe "clicking selected #checkbox deselects it"
    !^ testpage
    click "#checkbox"
    click "#checkbox"
    deselected "#checkbox")

test (fun _ ->
    describe "browsing to redit should be on reddit"
    url "http://www.reddit.com/"
    on "http://www.reddit.com/")

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

run ()

System.Console.ReadKey()

quit ()
