module runner

open System
open configuration
open canopy

let mutable _once = fun () -> ()
let mutable _before = fun () -> ()
let mutable _after = fun () -> ()
let mutable _lastly = fun () -> ()

let once f = _once <- f
let before f = _before <- f
let after f = _after <- f
let lastly f = _lastly <- f

let mutable tests = []
let mutable wips = []
let mutable manys = []

let test f = tests <- tests @ [f]
let wip f = wips <- wips @ [f]
let many count f = [1 .. count] |> List.map (fun _ -> manys <- manys @ [f]) |> ignore
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

    let runtest f = 
            if failed = ref false then
                try
                    _before ()
                    f ()
                    _after ()
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
            ()

    if wips.IsEmpty = false then
        wipTest <- true
        wips 
        |> List.map (fun t -> 
                        runtest t
                        Console.WriteLine("This is a wip test, press enter to run next test")
                        Console.ReadLine() |> ignore
                    )
        |> ignore
        wipTest <- false
    else if manys.IsEmpty = false then
        manys 
        |> List.map runtest 
        |> ignore
    else
        tests 
        |> List.map runtest 
        |> ignore
    
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
