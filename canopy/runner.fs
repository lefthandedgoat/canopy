module runner

open canopy

let mutable tests = []
let mutable wips = []
let mutable before = fun () -> ()
let failfast = ref false
let suggestions = ref true
let mutable passedCount = 0
let mutable failedCount = 0
let stopWatch = new System.Diagnostics.Stopwatch()
stopWatch.Start()

let test f = 
    tests <- tests @ [f]

let wip f = 
    wips <- wips @ [f]

let xtest f = ()

let rec private makeSuggestions actions =
    match actions with
    | [] -> ()
    | _ :: action2 :: _ when action2.action = "url" -> makeSuggestions actions.Tail
    | action1 :: action2 :: _ when action1.action <> "on" && action2.action <> "on" && (action1.url <> action2.url) ->  //suggestion for doing an action one page that transitioned you to another page and performing an action without checking to see if that page loaded with 'on'
        (
            System.Console.WriteLine("Suggestion: as a best practice you should check that you are on a page before
accessing an element on it
you were on {0}
then on {1}", action1.url, action2.url);
            makeSuggestions actions.Tail
        )
    | _ -> makeSuggestions actions.Tail

let run _ =
    let failed = ref false

    let runtest f = 
            if failed = ref false then
                try
                    (before ())
                    (f ())
                    System.Console.WriteLine("Passed");
                    passedCount <- passedCount + 1
                with
                    | ex -> (
                                if failfast = ref true then
                                    failed := true
                                    System.Console.WriteLine("failfast was set to true and an error occured; stopping testing");
                                System.Console.WriteLine("Error: {0}", ex.Message);
                                failedCount <- failedCount + 1
                            )
            if suggestions = ref true then
                makeSuggestions actions

            actions <- []
            ()

    if wips.IsEmpty = false then
        wips 
        |> List.map (fun t -> 
                        runtest t
                        System.Console.WriteLine("This is a wip test, press enter to run next test")
                        System.Console.ReadLine() |> ignore
                    )
        |> ignore
    else
        tests 
        |> List.map runtest 
        |> ignore
    
    stopWatch.Stop()
    System.Console.WriteLine()
    System.Console.WriteLine("{0} seconds to execute", stopWatch.Elapsed.Seconds)
    System.Console.WriteLine("{0} passed", passedCount)
    System.Console.WriteLine("{0} failed", failedCount)    

    ()
