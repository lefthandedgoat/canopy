[<AutoOpen>]
module canopy.wait

open OpenQA.Selenium

let mutable waitSleep = 0.5

let waitResults timeout (f : unit -> 'a) =
  let sw = System.Diagnostics.Stopwatch.StartNew()
  let rec innerwait timeout f =
    let sleepAndWait () = System.Threading.Thread.Sleep(int (waitSleep * 1000.0)); innerwait timeout f
    if sw.Elapsed.TotalSeconds >= timeout then raise <| WebDriverTimeoutException("Timed out!")

    try
      let result = f()
      match box result with
        | :? bool as b ->
              if b then result
              else sleepAndWait ()
        | _ as o ->
              if o <> null then result
              else sleepAndWait ()
    with
      | :? WebDriverTimeoutException as ex -> reraise()
      | :? CanopyException as ce -> raise(ce)
      | _ -> sleepAndWait ()

  innerwait timeout f

let wait timeout (f : unit -> bool) = waitResults timeout f |> ignore


