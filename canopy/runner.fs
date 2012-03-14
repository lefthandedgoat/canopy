module runner

let mutable tests = []
let mutable before = fun () -> ()

let test f = 
    let fAsList = [f]
    tests <- List.append tests fAsList

let run _ =
    tests |> List.map (fun f -> (
                                try
                                    (before ())
                                    (f ())
                                    System.Console.WriteLine("Passed");
                                with
                                    | ex -> System.Console.WriteLine("Error: {0}", ex.Message);
                                    )
                                ) 
