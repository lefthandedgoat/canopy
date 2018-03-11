/// A module the indirects the function calls that the code makes into Expecto,
/// in order to spawn the quit the browser instances.
[<AutoOpen>]
module Expecto.Canopy

open Canopy
open System
open Expecto
open Argu

/// You can implement this interface on your `conf` value if you want the testing
/// framework to start the browser at a specific URL.
type HasStartURI =
  abstract startURI: Uri

// config for your own test suite
type ParallelUsageArguments =
  | Site of initialURL:string

  interface IArgParserTemplate with
    member s.Usage =
      match s with
      | Site _ -> "What URL to start the browser at."

type TestConfig =
  { site: Uri
    mode: BrowserStartMode
    canopyConfig: CanopyConfig
  }

  interface HasStartURI with
    member x.startURI = x.site

  static member empty =
    { site = Uri "https://staging.qvitoo.com"
      mode = Firefox
      canopyConfig = Canopy.CanopyConfig.defaultConfig
    }

module TestConfig =
  let update config = function
    | Site site -> { config with site = Uri site }

let private testB testFn (conf: TestConfig) name fn =
  testFn name <| async {
    use browser = startWithConfigPure conf.canopyConfig conf.mode
    match box conf with
    | :? HasStartURI as testConfig ->
      uriB browser testConfig.startURI
    | _ ->
      ()
    try
      let ctx = Context.createT (Some browser) conf.canopyConfig conf
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