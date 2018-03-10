/// A module the indirects the function calls that the code makes into Expecto,
/// in order to spawn the quit the browser instances.
[<AutoOpen>]
module Expecto.Core

open Canopy

let testCase conf name fn =
    testCaseAsync name <| async {
      use browser = start firefox
      try fn (Context.create conf browser)
      finally quit browser
    }
