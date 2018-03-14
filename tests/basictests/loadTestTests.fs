module loadTestTests

open Canopy.Runner.Runner
open Canopy.Runner.Runner.Operators
open Canopy.Integration.LoadTest
open System.Net

let all () =
    context "tests for the load tester in integration"

    "can create a basic job" &&! fun _ ->
        let job =
          {
            Warmup = true
            Baseline = true
            AcceptableRatioPercent = 200
            Minutes = 1
            Load = 1
            Tasks =
              [
                {
                  Description = "task1"
                  Action = (fun _ ->
                    use client = new WebClient()
                    client.DownloadString("http://www.turtletest.com/chris") |> ignore
                    printfn "task1")
                  Frequency = 6
                }
                {
                  Description = "task2"
                  Action = (fun _ ->
                    use client = new WebClient()
                    client.DownloadString("http://www.turtletest.com/chris") |> ignore
                    printfn "task2")
                  Frequency = 6
                }
              ]
          }

        runLoadTest job

    "timelines look reasonable" &&! fun _ ->
        let job =
          {
            Warmup = true
            Baseline = true
            AcceptableRatioPercent = 200
            Minutes = 1
            Load = 1
            Tasks =
              [
                {
                  Description = "print hello world"
                  Action = fun _ -> printfn "hello world"
                  Frequency = 4
                }
              ]
          }

        createTimeline job
        |> List.iter (fun (timing, task) -> printfn "%A %A" timing task.Description)
