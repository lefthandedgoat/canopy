module runner

let mutable tests = []

let test f = 
    let fAsList = [f]
    tests <- List.append tests fAsList

//do this for type inference of tests collection, not sure how to type annotate function signatures
test (fun _ -> System.Console.WriteLine(""))

let run _ =
    List.map (fun f -> (f ())) tests
