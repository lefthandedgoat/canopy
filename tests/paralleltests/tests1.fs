module tests1

open prunner
open canopy.parallell.functions

let add () = 
  let testpage = "http://lefthandedgoat.github.io/canopy/testpages/"

  "Set 1 #firstName should have John" &&& fun ctx -> 
    let browser = start canopy.types.Chrome
    url testpage browser
    equals "#firstName" "John" browser
    quit browser

  "Set 1 writing to #lastName sets text to Smith" &&& fun ctx -> 
    let browser = start canopy.types.Chrome
    url testpage browser
    clear "#lastName" browser
    write "#lastName" "Smith" browser
    equals "#lastName" "Smith" browser
    quit browser
      
  "Set 1 clicking hyperlink sets #link_clicked to link clicked" &&& fun ctx -> 
    let browser = start canopy.types.Chrome
    url testpage browser
    equals "#link_clicked" "link not clicked" browser
    click "#hyperlink" browser
    equals "#link_clicked" "link clicked" browser
    quit browser
