open canopy
open runner

start firefox

"Go to google and search for hello world" &&& fun _ ->
    url "http://google.com"
    "input[type='text']" << "hello world"

run()

System.Console.ReadKey() |> ignore

quit()