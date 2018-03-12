module Canopy.Runner.Runner

open Canopy
open Canopy.Runner.CanopyRunnerConfig
open Canopy.Runner.Reporters
open OpenQA.Selenium
open System

(* documented/assertions *)
let contains (value1: string) (value2: string) =
    if not (value2.Contains value1) then
        let message = sprintf "contains check failed.  %s does not contain %s" value2 value1
        raise (CanopyContainsFailedException message)

(* documented/assertions *)
let containsInsensitive (value1: string) (value2: string) =
    let rules = StringComparison.InvariantCultureIgnoreCase
    let contains = value2.IndexOf(value1, rules)
    if contains < 0 then
        let message = sprintf "contains insensitive check failed.  %s does not contain %s" value2 value1
        raise (CanopyContainsFailedException message)

(* documented/assertions *)
let notContains (value1: string) (value2: string) =
    if value2.Contains value1 then
        let message = sprintf "notContains check failed.  %s does contain %s" value2 value1
        raise (CanopyNotContainsFailedException message)

(* documented/assertions *)
let is expected actual =
    if expected <> actual then
        let message = sprintf "equality check failed. Expected: %O, got: %O" expected actual
        raise (CanopyEqualityFailedException message)


let private last = function
    | hd :: tl -> hd
    | [] -> failwith "Empty list."

(* documented/testing *)
let once f = (last suites).Once <- f
(* documented/testing *)
let before f = (last suites).Before <- f
(* documented/testing *)
let after f = (last suites).After <- f
(* documented/testing *)
let lastly f = (last suites).Lastly <- f
(* documented/testing *)
let onPass f = (last suites).OnPass <- f
(* documented/testing *)
let onFail f = (last suites).OnFail <- f
(* documented/testing *)
let context c =
    if (last suites).Context = null then
        (last suites).Context <- c
    else
        let s = new Suite()
        s.Context <- c
        suites <- s::suites

let private incrementLastTestSuite () =
    let lastSuite = last suites
    lastSuite.TotalTestsCount <- lastSuite.TotalTestsCount + 1
    lastSuite


module Operators =
    (* documented/assertions *)
    let (===) expected actual = is expected actual

    (* documented/testing *)
    let ( &&& ) description f =
        let lastSuite = incrementLastTestSuite()
        lastSuite.Tests <- Test(description, f, lastSuite.TotalTestsCount)::lastSuite.Tests

    (* documented/testing *)
    let ( &&! ) description f =
        let lastSuite = incrementLastTestSuite()
        lastSuite.Tests <- Test(description, skipped, lastSuite.TotalTestsCount)::lastSuite.Tests

    (* documented/testing *)
    let ( &&&& ) description f =
        let lastSuite = incrementLastTestSuite()
        lastSuite.Wips <- Test(description, f, lastSuite.TotalTestsCount)::lastSuite.Wips

    (* documented/testing *)
    let ( &&&&& ) description f =
        let lastSuite = incrementLastTestSuite()
        lastSuite.Always <- Test(description, f, lastSuite.TotalTestsCount)::lastSuite.Always

open Operators

(* documented/testing *)
let test f = null &&& f

(* documented/testing *)
let ntest description f = description &&& f

(* documented/testing *)
let wip f = null &&&& f

(* documented/testing *)
let many count f =
    let lastSuite = incrementLastTestSuite()
    [1 .. count] |> List.iter (fun _ -> lastSuite.Manys <- Test(null, f, lastSuite.TotalTestsCount)::lastSuite.Manys)

(* documented/testing *)
let nmany count description f =
    let lastSuite = incrementLastTestSuite()
    [1 .. count] |> List.iter (fun _ -> lastSuite.Manys <- Test(description, f, lastSuite.TotalTestsCount)::lastSuite.Manys)

(* documented/testing *)
let xtest f = null &&! f

let mutable passedCount = 0
let mutable skippedCount = 0
let mutable failedCount = 0
let mutable contextFailed = false
let mutable failedContexts : string list = []
let mutable failed = false

let pass id (suite: Suite) =
    passedCount <- passedCount + 1
    reporter.Pass id
    suite.OnPass()

let skip id =
    skippedCount <- skippedCount + 1
    reporter.Skip id

let fail (ex : Exception) (test : Test) (suite: Suite) autoFail url =
    if failureMessagesThatShoulBeTreatedAsSkip |> List.exists (fun message -> ex.Message = message) then
        skip test.Id
    else
        if skipAllTestsOnFailure = true || skipRemainingTestsInContextOnFailure = true then skipNextTest <- true
        if autoFail then
            skip test.Id //dont take the time to fail all the tests just skip them
        else
            try
                if failFast = ref true then failed <- true
                failedCount <- failedCount + 1
                contextFailed <- true
                let f = failScreenshotFileName test suite
                if failureScreenshotsEnabled = true then
                  let ss = screenshot failScreenshotPath f
                  reporter.Fail ex test.Id ss url
                else reporter.Fail ex test.Id Array.empty<byte> url
                suite.OnFail()
            with
                | failExc ->
                    //Fail during error report (likely  OpenQA.Selenium.WebDriverException.WebDriverTimeoutException ).
                    // Don't fail the runner itself, but report it.
                    reporter.Write (sprintf "Error during fail reporting: %s" (failExc.ToString()))
                    reporter.Fail ex test.Id Array.empty url
                    suite.OnFail()

let safelyGetUrl () =
    match !Context._globalContext with
    | None ->
        "No context -> no URL"
    | Some context ->
        match context.browser with
        | None ->
            "No browser -> no URL"
        | Some browser ->
            try currentUrlB browser
            with _ -> "Failed to get URL"

let failSuite (ex: Exception) (suite: Suite) =
    let reportFailedTest (ex: Exception) (test : Test) =
        reporter.TestStart test.Id
        fail ex test suite true <| safelyGetUrl()
        reporter.TestEnd test.Id

    // tests are in reverse order and have to be reversed first
    do suite.Tests
    |> List.rev
    |> List.iter (fun test -> reportFailedTest ex test)

let tryTest test suite func =
    try
        func ()
        Pass
    with
    | :? CanopySkipTestException ->
        Skip
    | ex when CanopyRunnerConfig.failureMessage = Some ex.Message ->
        Pass
    | ex ->
        Fail ex

let skipTest message =
    describe (sprintf "Skipped: %s" message)
    raise (CanopySkipTestException())

let private processRunResult suite (test : Test) result =
    match result with
    | Pass -> pass test.Id suite
    | Fail ex -> fail ex test suite false <| safelyGetUrl()
    | Skip -> skip test.Id
    | Todo -> reporter.Todo test.Id
    | FailFast -> ()
    | Failed -> ()

    reporter.TestEnd test.Id

let private runtest (suite: Suite) (test : Test) =
    let ctx = Canopy.Core.context ()
    if failed = false then
        reporter.TestStart test.Id
        let result =
          if System.Object.ReferenceEquals(test.Func, todo) then Todo
          else if System.Object.ReferenceEquals(test.Func, skipped) then Skip
          else if skipNextTest = true then Skip
          else
              let testResult =
                let testResult = tryTest test suite (suite.Before >> test.Func)
                match testResult with
                | Fail(_) -> processRunResult suite test testResult; Failed
                | _ -> testResult

              match testResult with
              | Skip -> Skip
              | _ ->
                let afterResult = tryTest test suite (suite.After)
                match testResult with
                | Failed -> testResult
                | _ -> afterResult

        CanopyRunnerConfig.failureMessage <- None
        result
    else
        CanopyRunnerConfig.failureMessage <- None
        FailFast

(* documented/testing *)
let run () =

    let wipsExist = suites |> List.exists (fun s -> s.Wips.IsEmpty = false)

    if wipsExist && failIfAnyWipTests then
       raise <| Exception "Wip tests found and failIfAnyWipTests is true"

    reporter.SuiteBegin()
    let stopWatch = new Diagnostics.Stopwatch()
    stopWatch.Start()

    // suites list is in reverse order and have to be reversed before running the tests
    suites <- List.rev suites

    //run all the suites
    if runFailedContextsFirst = true then
        let failedContexts = History.get()
        //reorder so failed contexts are first
        let fc, pc = suites |> List.partition (fun s -> failedContexts |> List.exists (fun fc -> fc = s.Context))
        suites <- fc @ pc

    //run only wips if there are any
    if wipsExist then
        suites <- suites |> List.filter (fun s -> s.Wips.IsEmpty = false || s.Always.IsEmpty = false)

    suites
    |> List.iter (fun s ->
        if skipRemainingTestsInContextOnFailure = true && skipAllTestsOnFailure = false then skipNextTest <- false
        if failed = false then
            contextFailed <- false
            if s.Context <> null then reporter.ContextStart s.Context
            try
                s.Once ()
            with
                | ex -> failSuite ex s
            if failed = false then
                if s.Wips.IsEmpty = false then
                    CanopyRunnerConfig.wipTest <- true
                    let tests = s.Wips @ s.Always |> List.sortBy (fun t -> t.Number)
                    tests |> List.iter (fun w -> runtest s w |> processRunResult s w)
                    wipTest <- false
                else if s.Manys.IsEmpty = false then
                    s.Manys |> List.rev |> List.iter (fun m -> runtest s m |> processRunResult s m)
                else
                    let tests = s.Tests @ s.Always |> List.sortBy (fun t -> t.Number)
                    tests |> List.iter (fun t -> runtest s t |> processRunResult s t)
            s.Lastly ()

            if contextFailed = true then failedContexts <- s.Context::failedContexts
            if s.Context <> null then reporter.ContextEnd s.Context
    )

    History.save failedContexts

    stopWatch.Stop()
    reporter.Summary stopWatch.Elapsed.Minutes stopWatch.Elapsed.Seconds passedCount failedCount skippedCount
    reporter.SuiteEnd()

(* documented/testing *)
let runFor browsers =
    // suites are in reverse order and have to be reversed before running the tests
    let currentSuites = suites
    let config = defaultConfig

    match box browsers with
    | :? (BrowserStartMode list) as browsers ->
        let newSuites =
          browsers
          |> List.rev
          |> List.map (fun browser ->
              let suite = new Suite()
              suite.Context <- sprintf "Running tests with %s browser" (toString browser)
              suite.Once <- fun _ -> startWithConfig config browser |> ignore
              let currentSuites2 = currentSuites |> List.map(fun suite -> suite.Clone())
              currentSuites2 |> List.iter (fun (suite: Suite) ->
                  suite.Context <- sprintf "(%s) %s" (toString browser) suite.Context)
              currentSuites2 @ [suite])
          |> List.concat
        suites <- newSuites
    | :? (IWebDriver list) as browsers ->
        let newSuites =
          browsers
          |> List.rev
          |> List.map (fun browser ->
              let suite = new Suite()
              suite.Context <- sprintf "Running tests with %s browser" (browser.ToString())
              suite.Once <- fun _ -> switchTo browser |> ignore
              let currentSuites2 = currentSuites |> List.map(fun suite -> suite.Clone())
              currentSuites2 |> List.iter (fun (suite: Suite) ->
                  suite.Context <- sprintf "(%s) %s" (browser.ToString()) suite.Context)
              currentSuites2 @ [suite])
          |> List.concat
        suites <- newSuites
    | _ ->
        raise <| Exception "I dont know what you have given me"
