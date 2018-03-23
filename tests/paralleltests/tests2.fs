module tests2

open prunner
open canopy.parallell.instanced
open canopy.types

let add () = 
  let testpage = "http://lefthandedgoat.github.io/canopy/testpages/"

  "Set 2 #firstName should have John" &&& fun ctx -> 
    let x = new Instance()
    x.start Chrome
    x.url testpage
    x.equals "#firstName" "John"
    x.url testpage
    x.equals "#firstName" "John"
    x.url testpage
    x.equals "#firstName" "John"
    x.url testpage
    x.equals "#firstName" "John"
    x.url testpage
    x.equals "#firstName" "John"
    x.url testpage
    x.equals "#firstName" "John"
    x.url testpage
    x.equals "#firstName" "John"
    x.url testpage
    x.equals "#firstName" "John"
    x.url testpage
    x.equals "#firstName" "John"
    x.url testpage
    x.equals "#firstName" "John"
    x.url testpage
    x.equals "#firstName" "John"
    x.quit()

  "Set 2 writing to #lastName sets text to Smith" &&& fun ctx -> 
    let x = new Instance()
    x.start Chrome
    x.url testpage
    x.clear "#lastName"
    x.write "#lastName" "Smith"
    x.equals "#lastName" "Smith"
    x.clear "#lastName"
    x.write "#lastName" "Smith"
    x.equals "#lastName" "Smith"
    x.clear "#lastName"
    x.write "#lastName" "Smith"
    x.equals "#lastName" "Smith"
    x.clear "#lastName"
    x.write "#lastName" "Smith"
    x.equals "#lastName" "Smith"
    x.clear "#lastName"
    x.write "#lastName" "Smith"
    x.equals "#lastName" "Smith"
    x.clear "#lastName"
    x.write "#lastName" "Smith"
    x.equals "#lastName" "Smith"
    x.clear "#lastName"
    x.write "#lastName" "Smith"
    x.equals "#lastName" "Smith"
    x.clear "#lastName"
    x.write "#lastName" "Smith"
    x.equals "#lastName" "Smith"
    x.clear "#lastName"
    x.write "#lastName" "Smith"
    x.equals "#lastName" "Smith"
    x.quit()

  "Set 2 clicking hyperlink sets #link_clicked to link clicked" &&& fun ctx -> 
    let x = new Instance()
    x.start Chrome
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"
    x.url testpage
    x.equals "#link_clicked" "link not clicked"
    x.click "#hyperlink"
    x.equals "#link_clicked" "link clicked"
    x.quit()