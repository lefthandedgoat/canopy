namespace canopy.csharp.loadTest

type task(description:string, action:System.Action, frequency:int) =
    member this.Description = description
    member this.Action = action
    member this.Frequency = frequency

type job(warmup:bool, baseline:bool, acceptableRatioPercent:int, minutes:int, load: int, tasks:ResizeArray<task>) =
    member this.Warmup = warmup
    member this.Baseline = baseline
    member this.AcceptableRatioPercent = acceptableRatioPercent
    member this.Minutes = minutes
    member this.Load = load
    member this.Tasks = tasks

open canopy.integration.loadTest

type runner () =
    static member run (job:job) =
      let newJob =
        {
          Warmup = job.Warmup
          Baseline = job.Baseline
          AcceptableRatioPercent = job.AcceptableRatioPercent
          Minutes = job.Minutes
          Load = job.Load
          Tasks = job.Tasks
                  |> Seq.map(fun task -> { Description = task.Description; Frequency = task.Frequency; Action = fun () -> task.Action.Invoke(); })
                  |> List.ofSeq
        }

      runLoadTest newJob
