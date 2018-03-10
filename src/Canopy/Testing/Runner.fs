﻿module Canopy.Runner

open Canopy
open Canopy.Configuration
open Canopy.Reporters
open OpenQA.Selenium
open System

type CanopyRunnerConfig =
    {
        failFast: bool
        failScreenshotPath: string
        failScreenshotFileName: string
        failIfAnyWIPTests: bool
        runFailedContextsFirst: bool
        failureScreenshotsEnabled: bool
        skipAllTestsOnFailure: bool
        skipRemainingTestsInContextOnFailure: bool
        skipNextTest: bool
        failureMessagesThatShoulBeTreatedAsSkip: string list
    }

//runner related
// TODO: remove global variable
(* documented/configuration *)
let failFast = ref false
// TODO: remove global variable
(* documented/configuration *)
let mutable failScreenshotPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
// TODO: remove global variable
(* documented/configuration *)
let mutable failScreenshotFileName = fun (test: Test) (suite: Suite) -> DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
// TODO: remove global variable
(* documented/configuration *)
let mutable failIfAnyWipTests = false
// TODO: remove global variable
(* documented/configuration *)
let mutable runFailedContextsFirst = false
// TODO: remove global variable
(* documented/configuration *)
let mutable failureScreenshotsEnabled = true
// TODO: remove global variable
(* documented/configuration *)
let mutable skipAllTestsOnFailure = false
// TODO: remove global variable
(* documented/configuration *)
let mutable skipRemainingTestsInContextOnFailure = false
// TODO: remove global variable
(* documented/configuration *)
let mutable skipNextTest = false
// TODO: remove global variable
(* documented/configuration *)
let mutable failureMessagesThatShoulBeTreatedAsSkip : string list = []
// TODO: remove global variable
(* documented/configuration *)
let mutable reporter = new ConsoleReporter() :> IReporter

let private last = function
    | hd :: tl -> hd
    | [] -> failwith "Empty list."

// TODO: remove global variable
let mutable suites = [new Suite()]
// TODO: remove global variable
let mutable todo = fun () -> ()
// TODO: remove global variable
let mutable skipped = fun () -> ()

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
(* documented/testing *)
let ( &&& ) description f =
    let lastSuite = incrementLastTestSuite()
    lastSuite.Tests <- Test(description, f, lastSuite.TotalTestsCount)::lastSuite.Tests
(* documented/testing *)
let test f = null &&& f
(* documented/testing *)
let ntest description f = description &&& f
(* documented/testing *)
let ( &&&& ) description f =
    let lastSuite = incrementLastTestSuite()
    lastSuite.Wips <- Test(description, f, lastSuite.TotalTestsCount)::lastSuite.Wips
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
let ( &&! ) description f =
    let lastSuite = incrementLastTestSuite()
    lastSuite.Tests <- Test(description, skipped, lastSuite.TotalTestsCount)::lastSuite.Tests
(* documented/testing *)
let xtest f = null &&! f
(* documented/testing *)
let ( &&&&& ) description f =
    let lastSuite = incrementLastTestSuite()
    lastSuite.Always <- Test(description, f, lastSuite.TotalTestsCount)::lastSuite.Always
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
  if browser = null then "no browser = no url"
  else try browser.Url with _ -> "failed to get url"

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
        | :? CanopySkipTestException -> Skip
        | ex when failureMessage <> null && failureMessage = ex.Message -> Pass
        | ex -> Fail ex

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

        failureMessage <- null
        result
    else
        failureMessage <- null
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
                    wipTest <- true
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

    match box browsers with
    | :? (BrowserStartMode list) as browsers ->
        let newSuites =
          browsers
          |> List.rev
          |> List.map (fun browser ->
              let suite = new Suite()
              suite.Context <- sprintf "Running tests with %s browser" (toString browser)
              suite.Once <- fun _ -> start browser |> ignore; ()
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
              suite.Once <- fun _ -> switchTo browser
              let currentSuites2 = currentSuites |> List.map(fun suite -> suite.Clone())
              currentSuites2 |> List.iter (fun (suite: Suite) ->
                  suite.Context <- sprintf "(%s) %s" (browser.ToString()) suite.Context)
              currentSuites2 @ [suite])
          |> List.concat
        suites <- newSuites
    | _ ->
        raise <| Exception "I dont know what you have given me"
