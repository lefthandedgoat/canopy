module Canopy.ParallelUsage.Program

open Argu
open Expecto
open System
open System.Threading

// program
[<EntryPoint>]
let main argv =
  use cts = new CancellationTokenSource()
  // let expectoConfig = { Expecto.Tests.defaultConfig with token = cts.Token }
  Console.CancelKeyPress.Subscribe(fun _ -> cts.Cancel()) |> ignore
  // TO CONSIDER: https://github.com/fsprojects/Argu/issues/107
  let parser = ArgumentParser.Create<ParallelUsageArguments>(programName = "Canopy.ParallelUsage.exe")
  let results = parser.Parse(argv, ConfigurationReader.FromEnvironmentVariables (), ignoreUnrecognized = true, raiseOnUsage = false)
  if results.IsUsageRequested then
    let expectoParser = ArgumentParser.Create<Expecto.Tests.CLIArguments>(programName = "Canopy.ParallelUsage.exe")
    printfn "%s" (parser.PrintUsage())
    printfn "%s" (expectoParser.PrintUsage(hideSyntax = true))
    0
  else
    // TO CONSIDER: https://github.com/haf/expecto/issues/229
    let expectoArguments = results.UnrecognizedCliParams |> List.toArray
    let conf = results.GetAllResults() |> List.fold TestConfig.update TestConfig.empty
    Tests.tests conf
      |> runTestsWithArgs defaultConfig expectoArguments