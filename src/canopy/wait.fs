[<AutoOpen>]
module Canopy.Wait

open System.Threading
open System.Diagnostics
open OpenQA.Selenium

let mutable waitSleep = 0.5

/// TO CONSIDER: making this async?
let waitResults timeout (f: unit -> 'a) =
    let sw = Stopwatch.StartNew()

    let mutable finalResult: 'a = Unchecked.defaultof<'a>
    let mutable keepGoing = true

    while keepGoing do
        try
            if sw.Elapsed.TotalSeconds >= timeout then
                raise <| WebDriverTimeoutException("Timed out!")

            let result = f ()
            match box result with
            | :? bool as b when b ->
                keepGoing <- false
                finalResult <- result

            | :? bool ->
                Thread.Sleep(int (waitSleep * 1000.0))

            | _ as o when not (isNull o) ->
                keepGoing <- false
                finalResult <- result

            | _ as o ->
                Thread.Sleep(int (waitSleep * 1000.0))
        with
        | :? WebDriverTimeoutException ->
            reraise ()
        | :? CanopyException as ce ->
            raise ce
        | _ ->
            Thread.Sleep(int (waitSleep * 1000.0))

    finalResult

let wait timeout (f: unit -> bool) =
    waitResults timeout f
    |> ignore