module canopy.runner.classic

open System
open canopy.configuration
open canopy.classic
open canopy.reporters
open canopy.types
open OpenQA.Selenium

let private last = function
    | hd :: tl -> hd
    | [] -> failwith "Empty list."

let mutable suites = [new suite()]
let mutable todo = fun () -> ()
let mutable skipped = fun () -> ()
(* documented/configuration *)
let mutable skipNextTest = false

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
        let s = new suite()
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

let pass id (suite : suite) =
    passedCount <- passedCount + 1
    reporter.pass id
    suite.OnPass()

let skip id =
    skippedCount <- skippedCount + 1
    reporter.skip id

let fail (ex : Exception) (test : Test) (suite : suite) autoFail url =
    if failureMessagesThatShoulBeTreatedAsSkip |> List.exists (fun message -> ex.Message = message) then
        skip test.Id
    else
        if skipAllTestsOnFailure = true || skipRemainingTestsInContextOnFailure = true then skipNextTest <- true
        if autoFail then
            // same as a regular fail but w/o trying to get a screenshot
            if failFast = ref true then failed <- true
            failedCount <- failedCount + 1
            contextFailed <- true            
            reporter.fail ex test.Id Array.empty<byte> url
            suite.OnFail()
        else
            try
                if failFast = ref true then failed <- true
                failedCount <- failedCount + 1
                contextFailed <- true
                let f = canopy.configuration.failScreenshotFileName test suite
                if failureScreenshotsEnabled = true then
                  let ss = screenshot canopy.configuration.failScreenshotPath f
                  reporter.fail ex test.Id ss url
                else reporter.fail ex test.Id Array.empty<byte> url
                suite.OnFail()
            with
                | failExc ->
                    //Fail during error report (likely  OpenQA.Selenium.WebDriverException.WebDriverTimeoutException ).
                    // Don't fail the runner itself, but report it.
                    reporter.write (sprintf "Error during fail reporting: %s" (failExc.ToString()))
                    reporter.fail ex test.Id Array.empty url
                    suite.OnFail()

let safelyGetUrl () =
  if browser = null then "no browser = no url"
  else try browser.Url with _ -> "failed to get url"

let failSuite (ex: Exception) (suite : suite) =
    let reportFailedTest (ex: Exception) (test : Test) =
        reporter.testStart test.Id
        fail ex test suite true <| safelyGetUrl()
        reporter.testEnd test.Id

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
    | Todo -> reporter.todo test.Id
    | FailFast -> ()
    | Failed -> ()

    reporter.testEnd test.Id

let private runtest (suite : suite) (test : Test) =
    if failed = false then
        reporter.testStart test.Id
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

let private runSuitesForBrowser browserName useBrowser (suites:suite list) =
    let browserSuites =
        suites
        |> List.rev // suites list is in reverse order and have to be reversed first
        |> List.map (fun s ->
            match browserName with
            | Some name ->
                // clone with context specific to browser
                let clone = s.Clone()
                clone.Context <- sprintf "(%s) %s" name s.Context
                clone
            | None ->
                s)

    let prioritisedSuites =
        if runFailedContextsFirst
        then
            let fc, pc = browserSuites |> List.partition (fun s -> failedContexts |> List.contains s.Context)
            fc @ pc
        else
            browserSuites

    let openContext = Option.map (sprintf "Running tests with %s browser") browserName
    Option.iter reporter.contextStart openContext

    let browserSuccess =
        try
            useBrowser ()
            true
        with
        | ex ->
            List.iter (failSuite ex) prioritisedSuites //fail all tests in all suites for the browser
            false

    Option.iter reporter.contextEnd openContext


    let runSuite (s:suite) =
        if skipRemainingTestsInContextOnFailure = true && skipAllTestsOnFailure = false then skipNextTest <- false
        if failed = false then
            contextFailed <- false
            if s.Context <> null then reporter.contextStart s.Context
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
            if s.Context <> null then reporter.contextEnd s.Context

    if browserSuccess then List.iter runSuite prioritisedSuites
    ()

let private runForBrowsers (browsers: Browsers option) =
    let wipsExist = suites |> List.exists (fun s -> not s.Wips.IsEmpty)

    if wipsExist && failIfAnyWipTests then
       raise <| Exception "Wip tests found and failIfAnyWipTests is true"

    reporter.suiteBegin()
    let stopWatch = Diagnostics.Stopwatch()
    stopWatch.Start()

    //if there are wips, run only wip and always tests
    let filteredSuites =
        if wipsExist
        then suites |> List.filter (fun s -> not (s.Wips.IsEmpty && s.Always.IsEmpty))
        else suites
    let failedContexts = canopy.history.get()

    match browsers with
    | Some (BrowserStartModes browsers) ->
        for browser in browsers do
            runSuitesForBrowser (Some (toString browser)) (fun () -> start browser) filteredSuites
    | Some (WebDrivers browsers) ->
        for browser in browsers do
            runSuitesForBrowser (Some (browser.ToString())) (fun () -> switchTo browser) filteredSuites
    | None ->
        runSuitesForBrowser None id filteredSuites

    canopy.history.save failedContexts

    stopWatch.Stop()
    reporter.summary stopWatch.Elapsed.Minutes stopWatch.Elapsed.Seconds passedCount failedCount skippedCount
    reporter.suiteEnd()

(* documented/testing *)
let run () =
    runForBrowsers None

(* documented/testing *)
let runFor browsers =
    runForBrowsers (Some browsers)
