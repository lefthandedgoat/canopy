/// Module for waiting for things to become true.
[<AutoOpen>]
module internal Canopy.Wait

open OpenQA.Selenium
open System
open System.Diagnostics
open System.Threading
open Canopy.Logging
open Canopy.Logging.Message

/// A recursive type that collects all failures and also allows monadic value
/// passing between the invocations.
type WaitResult<'state, 'res> =
    | Ok of result:'res
    | Intermediate of state:'state
    | Exn of e:exn
    | Failure of results:WaitResult<'state, 'res> list

module WaitResult =
    let map fn = function
        | Ok results ->
            Ok (fn results)
        | other ->
            other

    let map2 fOk fF = function
        | Ok result ->
            Ok (fOk result)
        | Failure results ->
            Failure (fF results)
        | other ->
            other

    let map4 fOk fI fE fF = function
        | Ok results ->
            Ok (fOk results)
        | Intermediate state ->
            Intermediate (fI state)
        | Exn e ->
            Exn (fE e)
        | Failure results ->
            Failure (fF results)

    let bind fn = function
        | Ok results ->
            fn results
        | other ->
            other

    let bind2 fOk fF = function
        | Ok result ->
            fOk result
        | Failure results ->
            fF results
        | other ->
            other

    let bind4 fOk fI fE fF = function
        | Ok result ->
            fOk result
        | Intermediate state ->
            fI state
        | Exn e ->
            fE e
        | Failure results ->
            fF results

    let iter fn = function
        | Ok result ->
            ignore (fn result)
        | _ ->
            ()

    let iterFailure fn = function
        | Failure results ->
            ignore (fn results)
        | _ ->
            ()

type CanopyWaitForException(message, result: obj) =
    inherit CanopyException(message)
    member x.result = result

/// Nice logging extensions for metrics.
type Logger with
    /// Wraps the async value in an async value that times the execution of the
    /// `xA` async parameter.
    member x.time (xA: Async<'a>) =
        async {
            let sw = Stopwatch.StartNew()
            let! res = xA
            sw.Stop()
            do! x.log LogLevel.Debug (fun level -> { Message.gauge sw.ElapsedTicks "ticks" with level = level })
            return res
        }

/// A wait operation is a contextual operation (name, logger) that has a time
/// budget available to return WaitResult.Ok; it will be invoked every `sleep`
/// duration until it is successful.
type WaitOp<'state, 'res> =
    {
        /// Unwrapped/possibly throwing async
        fA: 'state -> Async<WaitResult<'state, 'res>>
        /// Name/location for this operation
        name: string
        /// Logger to log with
        logger: Logger
        /// Budget to wait for the xA value to return Ok.
        budget: TimeSpan
        /// How long to wait in between checking the value of the operation.
        sleep: TimeSpan
    }

    /// Create a new WaitOp.
    static member create fA n l bdg sl =
        {
            fA = fA
            name = n
            logger = l
            budget = bdg
            sleep = sl
        }

/// This waitFor function is basically a fold operation where each reduce step is
/// interspersed with an async wait.
///
/// Callers build the op that acts on the state and get back an async workflow
/// that implements the wait operation.
///
/// The returned value can be cached to save allocations, and the returned async
/// value (codomain of the returned function) is cold, so it can be re-executed
/// over and over again.
///
/// In the case of Failure being returned, this function will have acted as a
/// Scan operation, with all intermediate failures or intermediate values mappended
/// to a list in Failure.
///
/// Invariants:
///
///  - pre: op.fA: must not block forever
///  - pre: op.fA: liveness can only be garuanteed when neither the function nor
///    the async value blocks the thread-pool thead.
///  - post: either returns `Failure` or `Ok`.
///  - post: never throws exceptions.
///
let waitFor (op: WaitOp<'state, 'res>): 'state -> Async<WaitResult<'state, 'res>> =
    // wrapped, catching exceptions:
    let wfA state =
        async {
            try
                return! op.fA state
            with e ->
                return Exn e
        }

    let rec exec (sw: Stopwatch) state acc =
        async {
            if sw.Elapsed >= op.budget then
                return Failure acc
            else
                let! current = op.logger.time (wfA state)
                match current with
                | Ok value ->
                    return Ok value
                | Intermediate nextState as e ->
                    return! chill sw nextState (e :: acc)
                | Failure results ->
                    return! chill sw state (List.concat [ results; acc ])
                | Exn _ as ee ->
                    return! chill sw state (ee :: acc)
        }

    and chill sw state acc =
        async {
            let sleep = min op.sleep (op.budget - sw.Elapsed)

            if sleep <= TimeSpan.Zero then
                return Failure acc
            else
                do! Async.Sleep (sleep.TotalMilliseconds |> int)
                return! exec sw state acc
        }

    fun state ->
        async {
            let sw = Stopwatch.StartNew()
            return! exec sw state []
        }

module String =
    let eqCII a b =
        String.Equals(a, b, StringComparison.InvariantCultureIgnoreCase)
    let neqCII a b =
        not (eqCII a b)
    let contains (subString: string) (fullString: string) =
        fullString
            .ToLowerInvariant()
            .Contains(subString.ToLowerInvariant())
