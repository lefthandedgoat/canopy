module tests1

open prunner

let add () = 
  "tests 1 write to console 1" &&& fun ctx -> ctx.printfn "test 1"
  "tests 1 write to console 2" &&& fun ctx -> ctx.printfn "test 2"
  "tests 1 write to console 3" &&& fun ctx -> ctx.printfn "test 3"
  "tests 1 write to console 4" &&& fun ctx -> ctx.printfn "test 4"
  "tests 1 write to console 5" &&& fun ctx -> ctx.printfn "test 5"
  "tests 1 write to console 6" &&& fun ctx -> ctx.printfn "test 6"
  "tests 1 write to console 7" &&& fun ctx -> ctx.printfn "test 7"
  "tests 1 write to console 8" &&& fun ctx -> ctx.printfn "test 8"
  "tests 1 write to console 9" &&& fun ctx -> ctx.printfn "test 9"
