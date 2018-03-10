module ParallelUsage.Program

open Argu
open Canopy
open Expecto
open Expecto.Flip
open Expecto.Logging
open Expecto.Logging.Message
open OpenQA.Selenium

// config for your own test suite
type CLIArguments =
  | Site of initialURL:string
  interface IArgParserTemplate with
    member s.Usage =
      match s with
      | Site _ -> "What URL to start the browser at."

type Config =
  { site: string }

module Config =
  let update config arg =
    match arg with
    | Site site ->
      { config with site = site }

// runtime state for parallel support
type Context<'config> =
  { conf: 'config
    browser: IWebDriver }

module Context =
  let wrap (browser: IWebDriver) =
    browser

// indirection
let testCase name fn =
    testCaseAsync name <| async {
      use browser = start firefox
      try fn (Context.wrap browser)
      finally quit browser
    }

// tests
let tests =
  testList "staging.qvitoo.com" [
    testCase "navigate to" <| fun x ->
      urlB x "https://staging.qvitoo.com"
  ]

// program
[<EntryPoint>]
let main argv =
  // TO CONSIDER: https://github.com/fsprojects/Argu/issues/107
  tests |> runTestsWithArgs defaultConfig argv