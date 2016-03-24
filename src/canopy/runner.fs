[<AutoOpen>]
module canopy.runner

open System
open configuration
open canopy
open reporters
open types
open OpenQA.Selenium

let private last = function
    | hd :: tl -> hd
    | [] -> failwith "Empty list."

let mutable suites = [new suite()]
let mutable todo = fun () -> ()
let mutable skipped = fun () -> ()

let once f = (last suites).Once <- f
let before f = (last suites).Before <- f
let after f = (last suites).After <- f
let lastly f = (last suites).Lastly <- f
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
let ( &&& ) description f = 
    let lastSuite = incrementLastTestSuite()
    lastSuite.Tests <- Test(description, f, lastSuite.TotalTestsCount)::lastSuite.Tests
let test f = null &&& f
let ntest description f = description &&& f
let ( &&&& ) description f = 
    let lastSuite = incrementLastTestSuite()
    lastSuite.Wips <- Test(description, f, lastSuite.TotalTestsCount)::lastSuite.Wips
let wip f = null &&&& f
let many count f = 
    let lastSuite = incrementLastTestSuite()
    [1 .. count] |> List.iter (fun _ -> lastSuite.Manys <- Test(null, f, lastSuite.TotalTestsCount)::lastSuite.Manys)
let nmany count description f = 
    let lastSuite = incrementLastTestSuite()
    [1 .. count] |> List.iter (fun _ -> lastSuite.Manys <- Test(description, f, lastSuite.TotalTestsCount)::lastSuite.Manys)
let ( &&! ) description f = 
    let lastSuite = incrementLastTestSuite()
    lastSuite.Tests <- Test(description, skipped, lastSuite.TotalTestsCount)::lastSuite.Tests
let xtest f = null &&! f
let ( &&&&& ) description f = 
    let lastSuite = incrementLastTestSuite()
    lastSuite.Always <- Test(description, f, lastSuite.TotalTestsCount)::lastSuite.Always
let mutable passedCount = 0
let mutable skippedCount = 0
let mutable failedCount = 0
let mutable contextFailed = false
let mutable failedContexts : string list = []
let mutable failed = false

let pass id =    
    passedCount <- passedCount + 1
    reporter.pass id

let skip id =    
    skippedCount <- skippedCount + 1
    reporter.skip id

let fail (ex : Exception) (test : Test) (suite : suite) autoFail url =
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
                let f = configuration.failScreenshotFileName test suite
                if failureScreenshotsEnabled = true then
                  let ss = screenshot configuration.failScreenshotPath f
                  reporter.fail ex test.Id ss url
                else reporter.fail ex test.Id Array.empty<byte> url
            with 
                | failExc -> 
                    //Fail during error report (likely  OpenQA.Selenium.WebDriverException.WebDriverTimeoutException ). 
                    // Don't fail the runner itself, but report it.
                    reporter.write (sprintf "Error during fail reporting: %s" (failExc.ToString()))
                    reporter.fail ex test.Id Array.empty url

let safelyGetUrl () = if browser = null then "no browser = no url" else browser.Url

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
        | ex when failureMessage <> null && failureMessage = ex.Message -> Pass
        | ex -> Fail ex

let private processRunResult suite (test : Test) result = 
    match result with
    | Pass -> pass test.Id
    | Fail ex -> fail ex test suite false <| safelyGetUrl()
    | Skip -> skip test.Id
    | Todo -> reporter.todo test.Id
    | FailFast -> ()
    | Failed -> ()

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
              
              let afterResult = tryTest test suite (suite.After)
              match testResult with
              | Failed -> testResult
              | _ -> afterResult

        reporter.testEnd test.Id 
        failureMessage <- null
        result
    else
        failureMessage <- null
        FailFast

let run () =
    reporter.suiteBegin()
    let stopWatch = new Diagnostics.Stopwatch()
    stopWatch.Start()      
        
    // suites list is in reverse order and have to be reversed before running the tests
    suites <- List.rev suites
        
    //run all the suites
    if runFailedContextsFirst = true then
        let failedContexts = history.get()
        //reorder so failed contexts are first
        let fc, pc = suites |> List.partition (fun s -> failedContexts |> List.exists (fun fc -> fc = s.Context))
        suites <- fc @ pc

    //run only wips if there are any
    if suites |> List.exists (fun s -> s.Wips.IsEmpty = false) then
        suites <- suites |> List.filter (fun s -> s.Wips.IsEmpty = false || s.Always.IsEmpty = false)

    suites
    |> List.iter (fun s ->
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
    )
    
    history.save failedContexts

    stopWatch.Stop()    
    reporter.summary stopWatch.Elapsed.Minutes stopWatch.Elapsed.Seconds passedCount failedCount skippedCount 
    reporter.suiteEnd()

let runFor browsers =
    // suites are in reverse order and have to be reversed before running the tests
    let currentSuites = suites |> List.rev
    suites <- [new suite()]
    match box browsers with
        | :? (types.BrowserStartMode list) as browsers -> 
            browsers
            |> List.iter (fun browser ->
                toString browser
                |> sprintf "Running tests with %s browser"
                |> context
                once (fun _ -> start browser)
                suites <- suites @ currentSuites)
        | :? (IWebDriver list) as browsers ->
            browsers
            |> List.iter (fun browser ->
                browser.ToString()
                |> sprintf "Running tests with %s browser"
                |> context
                once (fun _ -> switchTo browser)
                suites <- suites @ currentSuites)
        | _ -> raise <| Exception "I dont know what you have given me"
  