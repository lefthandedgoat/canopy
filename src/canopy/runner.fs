module canopy.runner

open System
open configuration
open canopy
open reporters
open types
open OpenQA.Selenium

let rec private last = function
    | hd :: [] -> hd
    | hd :: tl -> last tl
    | _ -> failwith "Empty list."

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
        suites <- suites @ [s]

let ( &&& ) description f = (last suites).Tests <- (last suites).Tests @ [Test(description, f, (last suites).Tests.Length + 1)]
let test f = null &&& f
let ntest description f = description &&& f
let ( &&&& ) description f = (last suites).Wips <- (last suites).Wips @ [Test(description, f, (last suites).Wips.Length + 1)]
let wip f = null &&&& f
let many count f = [1 .. count] |> List.iter (fun _ -> (last suites).Manys <- (last suites).Manys @ [Test(null, f, (last suites).Manys.Length + 1)])
let ( &&! ) description f = (last suites).Tests <- (last suites).Tests @ [Test(description, skipped, (last suites).Tests.Length + 1)]
let xtest f = null &&! f

let mutable passedCount = 0
let mutable failedCount = 0
let mutable contextFailed = false
let mutable failedContexts : string list = []
let mutable failed = false

let pass () =    
    passedCount <- passedCount + 1
    reporter.pass ()

let fail (ex : Exception) id =
    try
        if failFast = ref true then failed <- true        
        failedCount <- failedCount + 1
        contextFailed <- true
        let f = DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
        let ss = screenshot configuration.failScheenshotPath f
        reporter.fail ex id ss
    with 
        | :? WebDriverException as failExc -> 
            //Fail during error report (likely  OpenQA.Selenium.WebDriverException.WebDriverTimeoutException ). 
            // Don't fail the runner itself, but report it.
            reporter.write (sprintf "Error during fail reporting: %s" (failExc.ToString()))
            reporter.fail ex id null

let run () =
    reporter.suiteBegin()
    let stopWatch = new Diagnostics.Stopwatch()
    stopWatch.Start()      
    
    let runtest (suite : suite) (test : Test) =
        if failed = false then
            let desc = if test.Description = null then (String.Format("Test #{0}", test.Number)) else test.Description
            try 
                reporter.testStart desc  
                if System.Object.ReferenceEquals(test.Func, todo) then 
                    reporter.todo ()
                else if System.Object.ReferenceEquals(test.Func, skipped) then 
                    reporter.skip ()
                else
                    suite.Before ()
                    test.Func ()
                    suite.After ()
                    pass()
            with
                | ex when failureMessage <> null && failureMessage = ex.Message -> pass()
                | ex -> fail ex desc
            reporter.testEnd desc
        
        failureMessage <- null            

    //run all the suites
    if runFailedContextsFirst = true then
        let failedContexts = history.get()
        //reorder so failed contexts are first
        let fc, pc = suites |> List.partition (fun s -> failedContexts |> List.exists (fun fc -> fc = s.Context))
        suites <- fc @ pc

    //run only wips if there are any
    if suites |> List.exists (fun s -> s.Wips.IsEmpty = false) then
        suites <- suites |> List.filter (fun s -> s.Wips.IsEmpty = false)

    suites
    |> List.iter (fun s ->
        if failed = false then
            contextFailed <- false
            if s.Context <> null then reporter.contextStart s.Context
            s.Once ()
            if s.Wips.IsEmpty = false then
                wipTest <- true
                s.Wips |> List.iter (fun w -> runtest s w)
                wipTest <- false
            else if s.Manys.IsEmpty = false then
                s.Manys |> List.iter (fun m -> runtest s m)
            else
                s.Tests |> List.iter (fun t -> runtest s t)
            s.Lastly ()        
            if contextFailed = true then failedContexts <- failedContexts @ [s.Context]
            if s.Context <> null then reporter.contextEnd s.Context
    )
    
    history.save failedContexts

    stopWatch.Stop()    
    reporter.summary stopWatch.Elapsed.Minutes stopWatch.Elapsed.Seconds passedCount failedCount 
    reporter.suiteEnd()