namespace Canopy.Runner

open Canopy.Runner.Reporters

type CanopyRunnerConfig =
    {
        failFast: bool
        failScreenshotPath: string
        failScreenshotFileName: Test -> Suite -> string
        failIfAnyWIPTests: bool
        failureMessage: string option
        runFailedContextsFirst: bool
        failureScreenshotsEnabled: bool
        skipAllTestsOnFailure: bool
        skipRemainingTestsInContextOnFailure: bool
        skipNextTest: bool
        failureMessagesThatShoulBeTreatedAsSkip: string list
        wipTest: bool
    }

[<AutoOpen>]
module CanopyRunnerConfig =
    open System

    //runner related
    // TODO: remove global variable
    (* documented/configuration *)
    let failFast = ref false
    // TODO: remove global variable
    (* documented/configuration *)
    let mutable failScreenshotPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
    // TODO: remove global variable
    (* documented/configuration *)
    let mutable failScreenshotFileName =
        fun (test: Test) (suite: Suite) -> DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
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
    // TODO: remove global mutable
    let mutable failureMessage: string option = None
    // TODO: remove global mutable
    let mutable wipTest = false

    (* documented/configuration *)
    let setFailureMessage message (config: CanopyRunnerConfig) =
        { config with failureMessage = message }

    type CanopyRunnerConfig with
        /// Hack to get around moving all the above variables to a structured, parallel-safe variant.
        static member create () =
            {
                failFast = !failFast
                failScreenshotPath = failScreenshotPath
                failScreenshotFileName = failScreenshotFileName
                failIfAnyWIPTests = failIfAnyWipTests
                failureMessage = failureMessage
                runFailedContextsFirst = runFailedContextsFirst
                failureScreenshotsEnabled = failureScreenshotsEnabled
                skipAllTestsOnFailure = skipAllTestsOnFailure
                skipRemainingTestsInContextOnFailure = skipRemainingTestsInContextOnFailure
                skipNextTest = skipNextTest
                failureMessagesThatShoulBeTreatedAsSkip = failureMessagesThatShoulBeTreatedAsSkip
                wipTest = wipTest
            }

    // TODO: remove global variable
    let mutable suites = [new Suite()]
    // TODO: remove global variable
    let mutable todo = fun () -> ()
    // TODO: remove global variable
    let mutable skipped = fun () -> ()