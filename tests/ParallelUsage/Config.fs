namespace Canopy.ParallelUsage

open Argu
open Expecto
open System

// config for your own test suite
type ParallelUsageArguments =
  | Site of initialURL:string
  interface IArgParserTemplate with
    member s.Usage =
      match s with
      | Site _ -> "What URL to start the browser at."

type Config =
  { site: Uri }

  interface HasStartURI with
    member x.startURI = x.site

  static member empty =
    { site = Uri "https://staging.qvitoo.com" }

module Config =
  let update config = function
    | Site site -> { config with site = Uri site }

