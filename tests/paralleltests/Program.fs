open prunner

let executingDir () = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
canopy.configuration.chromeDir <- executingDir()

functionsTests.add()
//tests2.add()
//tests3.add()

let failed = prunner.run 5

System.Console.ReadKey() |> ignore