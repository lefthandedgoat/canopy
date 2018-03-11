module Canopy.ParallelUsage.Tests

open Canopy.ParallelUsage
open Expecto
open Expecto.Logging
open Expecto.Logging.Message

let tests (conf: TestConfig) =
  testList "e2e" [
    testBrowser conf "browse to start page" <| fun _ -> ()
    testBrowser conf "signup" <| fun x ->
      ()
  ]