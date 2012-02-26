module main

open firefox
 
let testpage = @"C:\projects\canopy\basictests\BasicPage.html"

describe "#welcome should have Welcome"
url testpage
read "#welcome" |> equals "Welcome"

describe "#firstName should have John (using == infix operator)"
url testpage
"#firstName" == "John"

describe "#lastName should have Doe"
!^ testpage
"#lastName" == "Doe"

describe "clearing #firstName sets text to new empty string"
!^ testpage
clear "#firstName"
"#firstName" == ""

describe "writing to #lastName sets text to new Smith"
!^ testpage
clear "#lastName"
write "#lastName" "Smith"
"#lastName" == "Smith"

describe "#ajax label should have ajax loaded"
!^ testpage
"#ajax" == "ajax loaded"

describe "Value 1 listed in #value_list"
!^ testpage
listed "#value_list td" "Value 1"

describe "Value 2 listed in #value_list"
!^ testpage
listed "#value_list td" "Value 2"

describe "Value 3 listed in #value_list (using *= infix operator)"
!^ testpage
"#value_list td" *= "Value 3"

describe "Value 4 listed in #value_list (using *= infix operator)"
!^ testpage
"#value_list td" *= "Value 4"

describe "Item 1 listed in #item_list"
!^ testpage
listed "#item_list option" "Item 1"

describe "Item 2 listed in #item_list"
!^ testpage
listed "#item_list option" "Item 2"

describe "Item 3 listed in #item_list (using *= infix operator)"
!^ testpage
"#item_list option" *= "Item 3"

describe "Item 4 listed in #item_list (using *= infix operator)"
!^ testpage
"#item_list option" *= "Item 4"

describe "clicking #button sets #button_clicked to button clicked"
!^ testpage
"#button_clicked" == "button not clicked"
click "#button"
"#button_clicked" == "button clicked"

describe "clicking hyperlink sets #link_clicked to link clicked"
!^ testpage
"#link_clicked" == "link not clicked"
click "#hyperlink"
"#link_clicked" == "link clicked"

describe "clicking #radio1 selects it"
!^ testpage
click "#radio1"
selected "#radio1"

describe "clicking #radio2 selects it"
!^ testpage
click "#radio2"
selected "#radio2"

describe "clicking #checkbox selects it"
!^ testpage
click "#checkbox"
selected "#checkbox"

describe "clicking selected #checkbox deselects it"
!^ testpage
click "#checkbox"
click "#checkbox"
deselected "#checkbox"

quit ()

System.Console.ReadKey()