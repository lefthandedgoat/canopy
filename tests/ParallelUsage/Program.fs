module ParallelUsage.Program

open Argu
open Canopy
open Expecto
open Expecto.Flip
open Expecto.Logging
open Expecto.Logging.Message
open OpenQA.Selenium
open System
open System.Threading

// config for your own test suite
type CLIArguments =
  | Site of initialURL:string
  interface IArgParserTemplate with
    member s.Usage =
      match s with
      | Site _ -> "What URL to start the browser at."

type Config =
  { site: string }
  static member empty =
    { site = "https://staging.qvitoo.com" }

module Config =
  let update config arg =
    match arg with
    | Site site ->
      { config with site = site }

// indirection
let testCase conf name fn =
    testCaseAsync name <| async {
      use browser = start firefox
      try fn (Context.create conf browser)
      finally quit browser
    }

// tests
let tests (conf: Config) =
  testList "e2e" [
    testCase conf "browse to start page" <| fun x ->
      x.url conf.site
    testCase conf "signup" <| fun x -> ()
  ]

// program
[<EntryPoint>]
let main argv =
  use cts = new CancellationTokenSource()
  // let expectoConfig = { Expecto.Tests.defaultConfig with token = cts.Token }
  Console.CancelKeyPress.Subscribe(fun _ -> cts.Cancel()) |> ignore
  // TO CONSIDER: https://github.com/fsprojects/Argu/issues/107
  let parser = ArgumentParser.Create<CLIArguments>(programName = "Canopy.ParallelUsage.exe")
  let results = parser.Parse(argv, ConfigurationReader.FromEnvironmentVariables (), ignoreUnrecognized = true, raiseOnUsage = false)
  if results.IsUsageRequested then
    let expectoParser = ArgumentParser.Create<Expecto.Tests.CLIArguments>(programName = "Canopy.ParallelUsage.exe")
    printfn "%s" (parser.PrintUsage())
    printfn "%s" (expectoParser.PrintUsage(hideSyntax = true))
    0
  else
    // TO CONSIDER: https://github.com/haf/expecto/issues/229
    let expectoArguments = results.UnrecognizedCliParams |> List.toArray
    let conf = results.GetAllResults() |> List.fold Config.update Config.empty
    tests conf
      |> runTestsWithArgs defaultConfig expectoArguments