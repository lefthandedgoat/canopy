module tests2

open prunner
open canopy.parallell.instanced
open canopy.types

////////////////////////////////////
////////////////////////////////////
////////////////////////////////////
(*
These are examples of using the instance for parallel support
each function hangs off instance and uses its internal browser instance implicitly
If you do not want to do this, use the browser instance version, see tests1.fs
Use whatever test runner you want
Use whatever scoping strategy you want
I just use one I made so that I dont have to take external depencies for regression tests
*)
////////////////////////////////////
////////////////////////////////////
////////////////////////////////////

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