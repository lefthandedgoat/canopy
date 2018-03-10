module Canopy.ParallelUsage.Tests

open Canopy.ParallelUsage
open Expecto
open Expecto.Logging
open Expecto.Logging.Message

let tests (conf: Config) =
  testList "e2e" [
    testCase conf "browse to start page" <| fun x ->
      x.url conf.site
    testCase conf "signup" <| fun x ->
      ()
  ]
