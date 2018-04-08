module prunner

open System
open System.Collections.Generic

type actor<'t> = MailboxProcessor<'t>
type color = System.ConsoleColor

type Reporter =
  | ContextStart of description:string
  | ContextEnd of description:string
  | TestStart of description:string * id:Guid
  | Print of message:string * id:Guid
  | Skip of id:Guid
  | Todo of id:Guid
  | Pass of id:Guid
  | Fail of id:Guid * ex:Exception
  | RunOver of minutes:int * seconds:int * AsyncReplyChannel<int>

type TestContext (testId:Guid, reporter : actor<Reporter>) = class
  member x.TestId = testId
  member x.printfn fmtStr = Printf.kprintf (fun msg -> reporter.Post(Print(msg, x.TestId))) fmtStr
end

type Test =
  {
    Description : string
    Func : TestContext -> unit
    Id : Guid
  }

type Suite () = class
  member val Context : string = "" with get, set
  member val Tests : Test list = [] with get, set
  member val Wips : Test list = [] with get, set
end

type Worker =
  | Run

type Manager =
  | Initialize of Suite list
  | Start of AsyncReplyChannel<int>
  | Run of count:int
  | WorkerDone of suite:string * worker:actor<Worker>

let colorWriteReset color message =
  Console.ForegroundColor <- color
  printfn "%s" message
  Console.ResetColor()

let printError (ex : Exception) =
  colorWriteReset color.Red "Error: "
  printfn "%s" ex.Message
  printfn "%s" "Stack: "
  ex.StackTrace.Split([| "\r\n"; "\n" |], StringSplitOptions.None)
  |> Array.iter (fun trace ->
    Console.ResetColor()
    if trace.Contains(".FSharp.") then
      printfn "%s" trace
    else
      if trace.Contains(":line") then
        let beginning = trace.Split([| ":line" |], StringSplitOptions.None).[0]
        let line = trace.Split([| ":line" |], StringSplitOptions.None).[1]
        printf "%s" beginning
        colorWriteReset color.DarkGreen (":line" + line)
      else
        colorWriteReset color.DarkGreen trace
    Console.ResetColor())

let private last = function
  | head :: _ -> head
  | [] -> failwith "Empty list."

let private ng() = Guid.NewGuid()

let mutable suites = [new Suite()]

let mutable todo = fun (_:TestContext) -> ()
let mutable skipped = fun (_:TestContext) -> ()

let context c =
  if (last suites).Context = "" then
    (last suites).Context <- c
  else
    let s = new Suite()
    s.Context <- c
    suites <- s::suites

let ( &&& ) description f =
    (last suites).Tests <- { Description = description; Func = f; Id = ng() }::(last suites).Tests
let ( &&&& ) description f =
    (last suites).Wips <- { Description = description; Func = f; Id = ng() }::(last suites).Wips
let ( &&! ) description _ =
    (last suites).Tests <- { Description = description; Func = skipped; Id = ng() }::(last suites).Tests
    
let newReporter () : actor<Reporter> =
  let messages = new Dictionary<Guid, (color * string) list>()
  let printMessages id = messages.[id] |> List.rev |> List.iter (fun (color, message) -> colorWriteReset color message)
  actor.Start(fun self ->
    let rec loop passed failed skipped todo =
      async {
        let! msg = self.Receive ()
        match msg with
        | ContextStart description ->
            let message = sprintf "context: %s" description
            colorWriteReset color.DarkYellow message
            return! loop passed failed skipped todo
        | ContextEnd description ->
            let message = sprintf "context end: %s" description
            colorWriteReset color.DarkYellow message
            return! loop passed failed skipped todo
        | Reporter.TestStart(description, id) ->
            let message = sprintf "Test: %s" description
            messages.Add(id, [color.DarkCyan, message])
            return! loop passed failed skipped todo
        | Reporter.Print(message, id) ->
            messages.[id] <- (color.Black, message)::messages.[id] //prepend new message
            return! loop passed failed skipped todo
        | Reporter.Pass id ->
            printMessages id
            colorWriteReset color.Green "Passed"
            return! loop (passed + 1) failed skipped todo
        | Reporter.Fail(id, ex) ->
            printMessages id
            printError ex
            return! loop passed (failed + 1) skipped todo
        | Reporter.Skip id ->
            printMessages id
            colorWriteReset color.Yellow "Skipped"
            return! loop passed failed (skipped + 1) todo
        | Reporter.Todo id ->
            printMessages id
            colorWriteReset color.Yellow "Todo"
            return! loop passed failed skipped (todo + 1)
        | Reporter.RunOver (minutes, seconds, replyChannel) ->
            printfn ""
            printfn "%i minutes %i seconds to execute" minutes seconds
            colorWriteReset color.Green (sprintf "%i passed" passed)
            colorWriteReset color.Yellow (sprintf "%i skipped" skipped)
            colorWriteReset color.Yellow (sprintf "%i todo" todo)
            colorWriteReset color.Red (sprintf "%i failed" failed)
            replyChannel.Reply failed
            return! loop passed failed skipped todo
      }
    loop 0 0 0 0)

let reporter = newReporter()

let private runtest (test : Test) =
  reporter.Post(Reporter.TestStart(test.Description, test.Id))
  if System.Object.ReferenceEquals(test.Func, todo) then
    reporter.Post(Reporter.Todo test.Id)
  else if System.Object.ReferenceEquals(test.Func, skipped) then
    reporter.Post(Reporter.Skip test.Id)
  else
    try
      test.Func (TestContext(test.Id, reporter))
      reporter.Post(Reporter.Pass test.Id)
    with ex -> reporter.Post(Reporter.Fail(test.Id, ex))

let newWorker (manager : actor<Manager>) (suite:Suite) test : actor<Worker> =
  actor.Start(fun self ->
    let rec loop () =
      async {
        let! msg = self.Receive ()
        match msg with
        | Worker.Run ->
            runtest test
            manager.Post(Manager.WorkerDone(suite.Context, self))
            return ()
      }
    loop ())

let newManager maxDOP : actor<Manager> =
  let sw = System.Diagnostics.Stopwatch.StartNew()
  let contexts = new HashSet<string>()
  actor.Start(fun self ->
    let rec loop workers pendingWorkers replyChannel =
      async {
        let! msg = self.Receive ()
        match msg with
        | Manager.Initialize (suites) ->
            let wipWorkers = suites |> List.map (fun suite -> suite.Wips |> List.map (fun test -> suite, newWorker self suite test)) |> List.concat |> List.rev
            let workers = suites |> List.map (fun suite -> suite.Tests |> List.map (fun test -> suite, newWorker self suite test)) |> List.concat |> List.rev
            if wipWorkers.IsEmpty then
              return! loop workers pendingWorkers replyChannel
            else
              return! loop wipWorkers pendingWorkers replyChannel
        | Manager.Start(replyChannel) ->
            self.Post(Manager.Run maxDOP)
            return! loop workers pendingWorkers (Some replyChannel)
        | Manager.Run(count) ->
            if count = 0 then
              return! loop workers pendingWorkers replyChannel
            else
              match workers with
              | [] -> return! loop workers pendingWorkers replyChannel
              | (suite, head) :: tail ->
                if not <| contexts.Contains(suite.Context) then
                  contexts.Add(suite.Context) |> ignore
                  reporter.Post(Reporter.ContextStart suite.Context)
                let pendingWorkers = (suite,head)::pendingWorkers
                head.Post(Worker.Run)
                self.Post(Manager.Run(count - 1))
                return! loop tail pendingWorkers replyChannel
        | Manager.WorkerDone(suiteContext, doneWorker) ->
            self.Post(Manager.Run(1))
            let pendingWorkers = pendingWorkers |> List.filter (fun (_, pendingWorker) -> pendingWorker <> doneWorker)
            let workersForSuite = workers |> List.filter (fun (suite,_) -> suiteContext = suite.Context)
            let pendingWorkersForSuite = pendingWorkers |> List.filter (fun (suite,_) -> suiteContext = suite.Context)
            if workersForSuite.IsEmpty && pendingWorkersForSuite.IsEmpty then
              reporter.Post(Reporter.ContextEnd suiteContext)
            if pendingWorkers.IsEmpty && workers.IsEmpty then
              let failed = reporter.PostAndReply(fun replyChannel -> Reporter.RunOver(int sw.Elapsed.TotalMinutes, int sw.Elapsed.Seconds, replyChannel))
              replyChannel.Value.Reply failed
              return! loop workers pendingWorkers replyChannel
            else
              return! loop workers pendingWorkers replyChannel
      }
    loop [] [] None)

let run maxDOP =
  let manager = newManager maxDOP
  manager.Post(Manager.Initialize(suites))
  manager.PostAndReply(fun replyChannel -> Manager.Start replyChannel)