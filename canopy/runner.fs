module runner

let mutable tests = []

let test f = 
    let fAsList = [f]
    tests <- List.append tests fAsList

let run _ =
    tests |> List.map (fun f -> (
                                try
                                    (f ())
                                    System.Console.WriteLine("Passed");
                                with
                                    | ex -> System.Console.WriteLine("Error: {0}", ex.Message);
                                    )
                                ) 
