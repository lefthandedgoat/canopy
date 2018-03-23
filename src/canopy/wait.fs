module canopy.classic.wait

open OpenQA.Selenium

let mutable waitSleep = 0.5

let waitResults timeout (f : unit -> 'a) =
    let sw = System.Diagnostics.Stopwatch.StartNew()
    let mutable finalResult : 'a = Unchecked.defaultof<'a>
    let mutable keepGoing = true
    while keepGoing do    
        try
            if sw.Elapsed.TotalSeconds >= timeout then raise <| WebDriverTimeoutException("Timed out!")

            let result = f()
            match box result with
              | :? bool as b ->
                   if b then 
                      keepGoing <- false
                      finalResult <- result
                   else System.Threading.Thread.Sleep(int (waitSleep * 1000.0))
              | _ as o ->
                    if o <> null then 
                      keepGoing <- false
                      finalResult <- result
                    else System.Threading.Thread.Sleep(int (waitSleep * 1000.0))
        with
          | :? WebDriverTimeoutException as ex -> reraise()
          | :? canopy.types.CanopyException as ce -> raise(ce)
          | _ -> System.Threading.Thread.Sleep(int (waitSleep * 1000.0))

    finalResult
          
let wait timeout (f : unit -> bool) = waitResults timeout f |> ignore


