module canopy.integration.loadTest

open System

//IF YOU ARE READING THIS, SKIP TO BOTTOM TO runLoadTest TO GET IDEAD OF MAIN FLOW

let guid guid = System.Guid.Parse(guid)

//A task is work to be done, like do a GET on the login page
//Frequency is how many times per minute to run this action
type Task =
  {
    Description : string
    Action : (unit -> unit)
    Frequency : int
  }

//Jobs are a group of tasks that you want to run
//Warmup = true will run each task 1 time
//Baseline = true will run each task 1 time, capturing its performance
////and using it to determine if the run was a pass or fail
//AcceptableRatioPercent is used with baseline data to see if the average run of tasks exceeded the baseline
//Load is the work factor, it is multiplied by frequency.  So 2 would provide double the load, 10 would be 10x load
type Job =
  {
    Warmup : bool
    Baseline : bool
    AcceptableRatioPercent : int
    Minutes : int
    Load : int
    Tasks : Task list
  }

type private Result =
  {
    Task : Task
    Min : float
    Max : float
    Average: float
    //Todo maybe add 95% and 99%
  }

//actor stuff
type private actor<'t> = MailboxProcessor<'t>

type private Worker =
  | Do
  | Retire

type private Reporter =
  | WorkerDone of Task * timing:float
  | Retire of AsyncReplyChannel<Result list>

type private Manager =
  | Initialize of workerCount:int
  | WorkerDone
  | Retire of AsyncReplyChannel<bool>

let private time f =
  //just time it and swallow any exceptions for now
  let stopWatch = System.Diagnostics.Stopwatch.StartNew()
  try f() |> ignore
  with _ -> ()
  stopWatch.Elapsed.TotalMilliseconds

//worker ctor
//workers just run the action and send the timing informatoin to the reporter
let private newWorker (manager : actor<Manager>) (reporter : actor<Reporter>) task : actor<Worker> =
  actor.Start(fun self ->
    let rec loop () =
      async {
        let! msg = self.Receive ()
        match msg with
        | Worker.Retire ->
          return ()
        | Worker.Do ->
          let timing = time task.Action
          reporter.Post(Reporter.WorkerDone(task, timing))
          manager.Post(Manager.WorkerDone)
          return! loop ()
      }
    loop ())

//reporter ctor
//reporter recieves timing information about tasks from workers and aggregates it
let private newReporter () : actor<Reporter> =
  let mutable results : (Task * float) list = []
  actor.Start(fun self ->
    let rec loop () =
      async {
        let! msg = self.Receive ()
        match msg with
        | Reporter.Retire replyChannel ->
          let finalResults =
            results
            |> Seq.groupBy (fun (task, timing) -> task.Description)
            |> Seq.sortBy (fun (description, _) -> description)
            |> Seq.map (fun (description, pairs) -> description, pairs |> Seq.head |> fst, pairs |> Seq.map (fun (_, timings) -> timings))
            |> Seq.map (fun (description, task, timings) ->
                 {
                   Task = task
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

//manager ctor
//managers keep track of active workers and know when everything is done
let private newManager () : actor<Manager> =
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
            System.Threading.Thread.Sleep(10)
            self.Post(Manager.Retire replyChannel)
            return! loop workerCount
        | Manager.Initialize workerCount ->
          return! loop workerCount
        | Manager.WorkerDone ->
          return! loop (workerCount - 1)
      }
    loop 0)

//Validation for duplicate tasks
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
  |> List.map (fun task -> newWorker manager reporter task)
  |> List.iter (fun worker -> worker.Post(Do); worker.Post(Worker.Retire))

  manager.PostAndReply(fun replyChannel -> Manager.Retire replyChannel) |> ignore
  reporter.PostAndReply(fun replyChannel -> Reporter.Retire replyChannel)

let private baseline job = runTasksOnce job

//warmup and baseline are the same but you ignore the results of warmup
let private warmup job = runTasksOnce job |> ignore

//not private currently, for testing purposes
let createTimeline job =
  let random = System.Random(1) //always seed to 1 so we get the same pattern

  [0 .. job.Minutes - 1]
  |> List.map (fun i ->
       job.Tasks
         |> List.map (fun task ->
         //find a random time to wait before running the first iteration
         //for a Frequency of 1 its random between 0 and 60
         //for a Frequencey of 12 its random between 0 and 5
         //multiply by load to increase frequency
         let maxRandom = 60000 / (task.Frequency * job.Load) //ms
         let startPoint = random.Next(0, maxRandom)
         [0 .. (task.Frequency * job.Load) - 1] |> List.map (fun j -> startPoint + (maxRandom * j) + (60000 * i), task))
       |> List.concat)
     |> List.concat
     |> List.sortBy (fun (timing, _) -> timing)

let rec private iterateWorkers timingsAndWorkers (sw : System.Diagnostics.Stopwatch) =
  match timingsAndWorkers with
  | [] -> ()
  | (timing, worker : actor<Worker>) :: tail ->
    if int sw.Elapsed.TotalMilliseconds > timing then
      worker.Post(Worker.Do)
      iterateWorkers tail sw
    else
      System.Threading.Thread.Sleep(1)
      iterateWorkers timingsAndWorkers sw

let private printJob job = printfn "Job: (load %A) (minutes %A) (acceptableRatioPercent %A) (warmup %A) (baseline %A)" job.Load job.Minutes job.AcceptableRatioPercent job.Warmup job.Baseline

let private printBaseline baselineResults =
  printfn ""
  if baselineResults |> List.length = 0 then printfn "No Baseline"
  else
    printfn "Baseline                                                                      ms"
    printfn "--------------------------------------------------------------------------------"
    baselineResults
    |> List.iter (fun result ->
         let description = result.Task.Description.PadRight(70, ' ')
         let value = (sprintf "%.1f" result.Average).PadLeft(10, ' ')
         printfn "%s%s" description value)

let private printResults results load =
  printfn ""
  printfn "Task                                                  MIN ms    MAX ms    AVG ms"
  printfn "--------------------------------------------------------------------------------"
  results
  |> List.iter (fun result ->
       let temp = (sprintf "%s x%i" result.Task.Description (result.Task.Frequency * load))
       let description = temp.Substring(0, System.Math.Min(temp.Length, 50)).PadRight(50, ' ')
       let min = (sprintf "%.1f" result.Min).PadLeft(10, ' ')
       let max = (sprintf "%.1f" result.Max).PadLeft(10, ' ')
       let avg = (sprintf "%.1f" result.Average).PadLeft(10, ' ')
       printfn "%s%s%s%s" description min max avg)

let private runBaseline job baselineResults results =
  if job.Baseline = true then
    results
    |> List.map (fun result ->
         let baselineResult = baselineResults |> List.find(fun baselineResult -> result.Task.Description = baselineResult.Task.Description)
         let threshold = baselineResult.Average * (float job.AcceptableRatioPercent / 100.0)
         if result.Average > threshold then
           Some (sprintf "FAILED: Average of %.1f exceeded threshold of %.1f for %s" result.Average threshold result.Task.Description)
         else None)
    |> List.choose id
  else []

let private failIfFailure results = if results <> [] then failwith (System.String.Concat(results, "\r\n"))

//Meat and potatoes function
let runLoadTest job =
  let manager = newManager ()
  let reporter = newReporter ()
  let mutable baselineResults : Result list = []

  //make sure that we dont have duplicate descriptions because it will mess up the numbers
  failIfDuplicateTasks job

  //create warmup workers if need be and run them 1 after the other
  if job.Warmup = true then warmup job

  //create baseline workers and run them 1 after the other and record values
  if job.Baseline = true then baselineResults <- baseline job

  //create all the workers and create the time they should execute
  let timingsAndWorkers = createTimeline job |> List.map (fun (timing, task) -> timing, newWorker manager reporter task)
  manager.Post(Initialize(timingsAndWorkers.Length))

  //loop and look at head and see if its time has passed and if it has then
  iterateWorkers timingsAndWorkers (System.Diagnostics.Stopwatch.StartNew())

  manager.PostAndReply(fun replyChannel -> Manager.Retire replyChannel) |> ignore
  let results = reporter.PostAndReply(fun replyChannel -> Reporter.Retire replyChannel)

  printJob job
  printBaseline baselineResults
  printResults results job.Load

  runBaseline job baselineResults results |> failIfFailure
