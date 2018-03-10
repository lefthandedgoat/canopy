/// A module the indirects the function calls that the code makes into Expecto,
/// in order to spawn the quit the browser instances.
[<AutoOpen>]
module Expecto.Core

open Canopy
open System

/// You can implement this interface on your `conf` value if you want the testing
/// framework to start the browser at a specific URL.
type HasStartURI =
  abstract startURI: Uri

let private testB testFn conf name fn =
  testFn name <| async {
    use browser = start firefox // TODO: this must be configurable
    match box conf with
    | :? HasStartURI as testConfig ->
      uriB browser testConfig.startURI
    | _ ->
      ()
    try
      let ctx = Context.create conf browser
      do! fn ctx
    finally
      quit browser
  }

/// Creates a new browser-based async test.
let testBrowserAsync conf name fn =
  testB testCaseAsync conf name fn

/// Creates a new browser-based test.
let testBrowser conf name fn =
  testB testCaseAsync conf name (fn >> async.Return)

/// Creates a new skipped async test for the browser.
let ptestBrowserAsync conf name fn =
  testB ptestCaseAsync conf name fn

/// Creates a new skipped test for the browser.
let ptestBrowser conf name fn =
  testB ptestCaseAsync conf name (fn >> async.Return)

/// Creates a new focused async test for the browser.
let ftestBrowserAsync conf name fn =
  testB ftestCaseAsync conf name fn

/// Creates a new focused test for the browser.
let ftestBrowser conf name fn =
  testB ftestCaseAsync conf name (fn >> async.Return)