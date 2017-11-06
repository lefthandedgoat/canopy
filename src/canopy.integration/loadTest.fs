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

type SendData =
  {
    JobId : System.Guid
    NumberOfRequests : int
    MaxWorkers : int
    Uri : string
  }

//actor stuff
type actor<'t> = MailboxProcessor<'t>

type Worker =
  | Do
  | Retire

type Reporter =
  | WorkerDone of description:string * timing:float
  | Retire

type Manager =
  | WorkerDone of float * actor<Worker>
  | Retire

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
          manager.Post(Manager.WorkerDone(timing, self))
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
        | Reporter.Retire ->
          //do some calculations and return them?
          return ()
        | Reporter.WorkerDone (description, timing) ->
          results <- (description, timing) :: results
          return! loop ()
      }
    loop ())

let rec haveWorkersWork (idleWorkers : actor<Worker> list) numberOfRequests =
    match idleWorkers with
    | [] -> numberOfRequests
    | worker :: remainingWorkers ->
      worker.Post(Worker.Do)
      haveWorkersWork remainingWorkers (numberOfRequests - 1)

let newManager () : actor<Manager> =
  let sw = System.Diagnostics.Stopwatch()
  actor.Start(fun self ->
    let rec loop numberOfRequests pendingRequests results =
      async {
        let! msg = self.Receive ()
        match msg with
        | Manager.Retire ->
          return ()
        | Manager.WorkerDone(ms, worker) ->
          let results = ms :: results
          if numberOfRequests > 0 then
            let numberOfRequests = haveWorkersWork [worker] numberOfRequests
            return! loop numberOfRequests pendingRequests results
          else if pendingRequests > 1 then //if only 1 pendingRequest, then this that pendingRequest so we are done
            let pendingRequests = pendingRequests - 1
            return! loop numberOfRequests pendingRequests results
          else
            sw.Stop()
            let avg = results |> List.average
            let min = results |> List.min
            let max = results |> List.max
            printfn "Total seconds: %A, Average ms: %A, Max ms: %A, Min ms: %A" sw.Elapsed.TotalSeconds avg max min
            return! loop numberOfRequests pendingRequests results
      }
    loop 0 0 [])

let private failIfDuplicateTasks job =
  let duplicates =
    job.Tasks
    |> Seq.groupBy (fun task -> task.Description)
    |> Seq.filter (fun (description, tasks) -> Seq.length tasks > 1)
    |> Seq.map (fun (description, _) -> description)
    |> List.ofSeq

  if duplicates <> [] then failwith <| sprintf "You have tasks with duplicates decriptions: %A" duplicates

let private warmup job =
  let manager = newManager ()
  let reporter = newReporter ()

  job.Tasks
  |> List.map (fun task -> newWorker manager reporter task.Description task.Action)
  |> List.iter (fun worker -> worker.Post(Do); worker.Post(Worker.Retire))

  reporter.Post(Reporter.Retire)
  manager.Post(Manager.Retire)


let runLoadTest job =
  let reporter = newReporter ()

  //make sure that we dont have duplicate descriptions because it will mess up the numbers
  failIfDuplicateTasks job

  //create warmup workers if need be and run them 1 after the other
  if job.Warmup = true then warmup job

  //if job.Baseline = true then warmup job
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
