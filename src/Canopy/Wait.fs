[<AutoOpen>]
module Canopy.Wait

open Canopy.Logging
open Canopy.Logging.Message
open OpenQA.Selenium
open System
open System.Diagnostics
open System.Threading

/// A recursive type that collects all failures.
type WaitResult<'ok, 'error> =
    | Ok of value:'ok
    | Error of error:'error
    | Exn of e:exn
    | Failure of results:WaitResult<'ok, 'error> list

module WaitResult =
    let ofResult expectedOnError = function
        | Result.Ok value ->
            Ok value
        | Result.Error actual ->
            Error (expectedOnError, actual)

type CanopyWaitForException(message, result: obj) =
    inherit CanopyException(message)
    member x.result = result

type internal WaitOp<'ok, 'error> =
    {
        /// Unwrapped/possibly throwing async
        xA: Async<WaitResult<'ok, 'error>>
        /// Name/location for this operation
        name: string
        /// Logger to log with
        logger: Logger
        /// Budget to wait for the xA value to return Ok.
        budget: TimeSpan
        /// How long to wait in between checking the value of the operation.
        sleep: TimeSpan
    }
    static member create xA n l bdg sl =
        {
            xA = xA
            name = n
            logger = l
            budget = bdg
            sleep = sl
        }

let internal waitFor (op: WaitOp<'ok, 'error>): Async<WaitResult<'ok, 'error>> =
    // wrapped:
    let wA =
        async {
            try
                return! op.xA
            with e ->
                return Exn e
        }

    let rec exec (sw: Stopwatch) acc =
        async {
            if sw.Elapsed >= op.budget then
                return Failure acc
            else
                let! current = wA
                match current with
                | Ok value ->
                    return Ok value
                | Failure results ->
                    return! chill sw (List.concat [ results; acc ])
                | Exn _ as ee ->
                    return! chill sw (ee :: acc)
                | Error _ as e ->
                    return! chill sw (e :: acc)
        }

    and chill sw acc =
        async {
            do! Async.Sleep (op.sleep.TotalMilliseconds |> int)
            return! exec sw acc
        }

    async {
        let sw = Stopwatch.StartNew()
        return! exec sw []
    }

type internal AssertResult<'ok> =
    | WaitResult of wr:WaitResult<'ok, string>
    | Mismatch of expected:obj * actual:obj

type internal Asserter<'input, 'ok> = Asserter of fn:('input -> AssertResult<'ok>)

module internal Asserter =

    let ofPredicate predicate createError =
        Asserter (fun args ->
            if predicate args then
                WaitResult (Ok ())
            else
                createError ())

module internal String =
    let eqCII a b =
        String.Equals(a, b, StringComparison.InvariantCultureIgnoreCase)
    let neqCII a b =
        not (eqCII a b)
    let contains (subString: string) (fullString: string) =
        fullString
            .ToLowerInvariant()
            .Contains(subString.ToLowerInvariant())

let mutable waitSleep = 0.5

/// TO CONSIDER: making this async?
let waitResults timeout (f: unit -> 'a) =
    let sw = Stopwatch.StartNew()

    let mutable finalResult: 'a = Unchecked.defaultof<'a>
    let mutable keepGoing = true

    while keepGoing do
        try
            if sw.Elapsed >= timeout then
                raise <| WebDriverTimeoutException("Timed out!")

            let result = f ()
            match box result with
            | :? bool as b when b ->
                keepGoing <- false
                finalResult <- result

            | :? bool ->
                // TO CONSIDER: don't sleep the thread
                Thread.Sleep(int (waitSleep * 1000.0))

            | _ as o when not (isNull o) ->
                keepGoing <- false
                finalResult <- result

            | _ as o ->
                // TO CONSIDER: don't sleep the thread
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

let waitSeconds timeout f =
    wait (TimeSpan.FromSeconds timeout) f