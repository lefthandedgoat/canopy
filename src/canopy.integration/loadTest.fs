module loadTest

open System

let guid guid = System.Guid.Parse(guid)

type Timescale =
  | Seconds
  | Minutes

type Task =
  {
    Description : string
    Action : (unit -> unit)
    Frequency : int
  }

type Job =
  {
    Timescale : Timescale
    Warmup : bool
    Baseline : bool
    AcceptableRatioPercent : int
    Iterations : int
    Tasks : Task list
  }

type Result =
  {
    Description : string
    Min : float
    Max : float
    Average: float
    //Todo maybe add 95% and 99%
  }

//actor stuff
type actor<'t> = MailboxProcessor<'t>

type Worker =
  | Do
  | Retire

type Reporter =
  | WorkerDone of description:string * timing:float
  | Retire of AsyncReplyChannel<Result list>

type Manager =
  | Initialize of workerCount:int
  | WorkerDone
  | Retire of AsyncReplyChannel<bool>

let time f =
  //just time it and swallow any exceptions for now
  let stopWatch = System.Diagnostics.Stopwatch.StartNew()
  try f() |> ignore
  with _ -> ()
  stopWatch.Elapsed.TotalMilliseconds

let newWorker (manager : actor<Manager>) (reporter : actor<Reporter>) description action : actor<Worker> =
  actor.Start(fun self ->
    let rec loop () =
      async {
        let! msg = self.Receive ()
        match msg with
        | Worker.Retire ->
          return ()
        | Worker.Do ->
          let timing = time action
          reporter.Post(Reporter.WorkerDone(description, timing))
          manager.Post(Manager.WorkerDone)
          return! loop ()
      }
    loop ())

let newReporter () : actor<Reporter> =
  let mutable results : (string * float) list = []
  actor.Start(fun self ->
    let rec loop () =
      async {
        let! msg = self.Receive ()
        match msg with
        | Reporter.Retire replyChannel ->
          let finalResults =
            results
            |> Seq.groupBy (fun (description, timing) -> description)
            |> Seq.map (fun (description, pairs) -> description, pairs |> Seq.map (fun (_, timings) -> timings))
            |> Seq.map (fun (description, timings) ->
                 {
                   Description = description
                   Min = Seq.min timings
                   Max = Seq.max timings
                   Average = Seq.average timings
                 }
               )
             |> List.ofSeq
          replyChannel.Reply(finalResults)
          return ()
        | Reporter.WorkerDone (description, timing) ->
          results <- (description, timing) :: results
          return! loop ()
      }
    loop ())

let newManager () : actor<Manager> =
  let sw = System.Diagnostics.Stopwatch()
  actor.Start(fun self ->
    let rec loop workerCount =
      async {
        let! msg = self.Receive ()
        match msg with
        | Manager.Retire replyChannel ->
          if workerCount = 0 then
            replyChannel.Reply(true)
            return ()
          else
            self.Post(Manager.Retire replyChannel)
            return! loop workerCount
        | Manager.Initialize workerCount ->
          return! loop workerCount
        | Manager.WorkerDone ->
          return! loop (workerCount - 1)
      }
    loop 0)

let private failIfDuplicateTasks job =
  let duplicates =
    job.Tasks
    |> Seq.groupBy (fun task -> task.Description)
    |> Seq.filter (fun (description, tasks) -> Seq.length tasks > 1)
    |> Seq.map (fun (description, _) -> description)
    |> List.ofSeq

  if duplicates <> [] then failwith <| sprintf "You have tasks with duplicates decriptions: %A" duplicates

let private runTasksOnce job =
  let manager = newManager ()
  let reporter = newReporter ()

  manager.Post(Initialize(job.Tasks.Length))

  job.Tasks
  |> List.map (fun task -> newWorker manager reporter task.Description task.Action)
  |> List.iter (fun worker -> worker.Post(Do); worker.Post(Worker.Retire))

  manager.PostAndReply(fun replyChannel -> Manager.Retire replyChannel) |> ignore
  reporter.PostAndReply(fun replyChannel -> Reporter.Retire replyChannel)

let private baseline job =
  let results = runTasksOnce job
  results |> List.iter (printfn "%A")
  results

//warmup and baseline are the same but you ignore the results of warmup
let private warmup job = runTasksOnce job |> ignore

let runLoadTest job =
  let reporter = newReporter ()
  let mutable baselineResults : Result list = []

  //make sure that we dont have duplicate descriptions because it will mess up the numbers
  failIfDuplicateTasks job

  //create warmup workers if need be and run them 1 after the other
  if job.Warmup = true then warmup job

  if job.Baseline = true then baselineResults <- baseline job
  //create baseline workers and run them 1 after the other and record values

  //create all the workers and create the time they should execute
  //loop and look at head and see if its time has passed and if it has then
    //run it and pass tail
    //recurse
    //finish when manager says that there are no more workers

  //workers report to reporter that they are done and maybe the manager


  //if baselined validate times against baseline and fail if off
    //map diffs and print them
  //else
    //print averages

  ()
