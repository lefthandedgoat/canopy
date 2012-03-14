module runner

let mutable tests = []
let mutable before = fun () -> ()
let failfast = ref false

let test f = 
    let fAsList = [f]
    tests <- List.append tests fAsList

let run _ =
    let failed = ref false
    tests |> List.map (fun f -> (
                                if failed = ref false then
                                    try
                                        (before ())
                                        (f ())
                                        System.Console.WriteLine("Passed");
                                    with
                                        | ex -> (
                                                    if failfast = ref true then
                                                        failed := true
                                                        System.Console.WriteLine("failfast was set to true and an error occured; stopping testing");
                                                    System.Console.WriteLine("Error: {0}", ex.Message);
                                                )
                                        )
                                ) 
