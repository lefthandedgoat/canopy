module loadTestTests

open canopy.runner
open loadTest

let all () =
    context "tests for the load tester in integration"

    "can create a basic job" &&&& fun _ ->
        let job =
          {
            Timescale = Minutes
            Warmup = true
            Baseline = true
            AcceptableRatioPercent = 200
            Iterations = 2
            Tasks =
              [
                {
                  Description = "print hello world"
                  Action = fun _ -> printf "hello world"
                  Frequency = 1
                }
              ]
          }

        runLoadTest job
