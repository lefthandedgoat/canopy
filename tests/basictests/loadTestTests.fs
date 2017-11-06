module loadTestTests

open canopy.runner

let all () =
    context "tests for the load tester in integration"

    "blah" &&&& fun _ ->
        printfn "cry"
        ()
