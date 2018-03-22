open prunner

tests1.add()
tests2.add()
tests3.add()

let failed = prunner.run 3

System.Console.ReadKey() |> ignore