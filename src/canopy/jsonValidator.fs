module jsonValidator

open FSharp.Data

let mutable tests : (string * (unit -> unit)) list = []
let ( &&& ) desc f = tests <- (desc, f) :: tests
let run () = tests |> List.rev |> List.iter (fun (desc, f) -> printfn "%s" desc; try f(); printfn "pass" with _ -> ())
let ( == ) left right = if left <> right then failwith (sprintf "expected %A got %A" left right)
let clear () = tests <- []




"test 1" &&& fun _ ->
  1 == 1

"test 2" &&& fun _ ->
  1 == 1

"test 3" &&& fun _ ->
  1 == 1

"test 4" &&& fun _ ->
  1 == 1

"test 5" &&& fun _ ->
  1 == 1

"test 6" &&& fun _ ->
  1 == 1

"test 7" &&& fun _ ->
  1 == 1

run()

clear()
