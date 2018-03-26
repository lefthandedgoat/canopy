module instancedTests

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
    


    x.quit()

