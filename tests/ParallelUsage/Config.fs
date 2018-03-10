namespace Canopy.ParallelUsage

open Argu

// config for your own test suite
type ParallelUsageArguments =
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

