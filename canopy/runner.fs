module runner

open System
open configuration
open canopy

let rec last = function
    | hd :: [] -> hd
    | hd :: tl -> last tl
    | _ -> failwith "Empty list."

type suite () = class
    let mutable context : string = null
    let mutable once = fun () -> ()
    let mutable before = fun () -> ()
    let mutable after = fun () -> ()
    let mutable lastly = fun () -> () 
    let mutable tests : (unit -> unit) list = []
    let mutable wips : (unit -> unit) list = []
    let mutable manys : (unit -> unit) list = []

    member x.Context
        with get() = context
        and set(value) = context <- value
    member x.Once
        with get() = once
        and set(value) = once <- value
    member x.Before
        with get() = before
        and set(value) = before <- value
    member x.After
        with get() = after
        and set(value) = after <- value
    member x.Lastly
        with get() = lastly
        and set(value) = lastly <- value
    member x.Tests
        with get() = tests
        and set(value) = tests <- value
    member x.Wips
        with get() = wips
        and set(value) = wips <- value
    member x.Manys
        with get() = manys
        and set(value) = manys <- value
end

let mutable suites = [new suite()]

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

let test f = (last suites).Tests <- (last suites).Tests @ [f]
let wip f = (last suites).Wips <- (last suites).Wips @ [f]
let many count f = [1 .. count] |> List.iter (fun _ -> (last suites).Manys <- (last suites).Manys @ [f])
let xtest f = ()

let rec private makeSuggestions actions =
    match actions with
    | [] -> ()
    | _ :: action2 :: _ when action2.action = "url" -> makeSuggestions actions.Tail
    | action1 :: action2 :: _ when action1.action <> "on" && action2.action <> "on" && (action1.url <> action2.url) ->  //suggestion for doing an action one page that transitioned you to another page and performing an action without checking to see if that page loaded with 'on'
            Console.WriteLine("Suggestion: as a best practice you should check that you are on a page before\r\naccessing an element on it\r\nyou were on {0}\r\nthen on {1}", action1.url, action2.url);
            makeSuggestions actions.Tail
    | _ -> makeSuggestions actions.Tail

let mutable passedCount = 0
let mutable failedCount = 0

let run _ =
    let stopWatch = new Diagnostics.Stopwatch()
    stopWatch.Start()
    
    let failed = ref false

    let runtest (suite : suite) test = 
        if failed = ref false then
            try
                suite.Before ()
                test ()
                suite.After ()
                Console.ForegroundColor <- ConsoleColor.Green
                Console.WriteLine("Passed");
                Console.ResetColor()
                passedCount <- passedCount + 1
            with
                | ex when failureMessage <> null && failureMessage = ex.Message ->
                    Console.ForegroundColor <- ConsoleColor.Green
                    Console.WriteLine("Passed");
                    Console.ResetColor()
                    passedCount <- passedCount + 1                            
                | ex -> 
                    if failFast = ref true then
                        failed := true
                        Console.WriteLine("failFast was set to true and an error occured; testing stopped");
                    Console.ForegroundColor <- ConsoleColor.Red
                    Console.WriteLine("Error: ");
                    Console.ResetColor()
                    Console.WriteLine(ex.Message);
                    failedCount <- failedCount + 1
                            
        if suggestions = ref true then
            makeSuggestions actions

        actions <- []
        failureMessage <- null            

    //run all the suites
    suites
    |> List.iter (fun s ->
        if s.Context <> null then Console.WriteLine (String.Format("context: {0}", s.Context))
        s.Once ()
        if s.Wips.IsEmpty = false then
            wipTest <- true
            s.Wips 
            |> List.iter (fun w -> 
                            runtest s w
                            Console.WriteLine("This is a wip test, press enter to run next test")
                            Console.ReadLine() |> ignore
                         )
            wipTest <- false
        else if s.Manys.IsEmpty = false then
            s.Manys |> List.iter (fun m -> runtest s m)
        else
            s.Tests |> List.iter (fun t -> runtest s t)
        s.Lastly ()
    )
    
    stopWatch.Stop()
    Console.WriteLine()
    Console.WriteLine("{0} seconds to execute", stopWatch.Elapsed.Seconds)
    if failedCount = 0 then
        Console.ForegroundColor <- ConsoleColor.Green
    Console.WriteLine("{0} passed", passedCount)
    Console.ResetColor()
    if failedCount > 0 then
        Console.ForegroundColor <- ConsoleColor.Red        
    Console.WriteLine("{0} failed", failedCount)    
    Console.ResetColor()
    ()
