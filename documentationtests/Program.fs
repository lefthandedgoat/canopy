open canopy
open runner
open OpenQA.Selenium
open configuration
open reporters
reporter <- new LiveHtmlReporter() :> IReporter

start chrome

let actions = @"file:///C:/actions.html"

context "test source links"

let sourceLinkCorrect index = 
    test(fun _ ->
        url actions
        let e = nth index ".source"
        let datafor = e.GetAttribute("data-for")
        let dataline = e.GetAttribute("data-line")
        let lineSelector = "#LC" + dataline + " span"
        if datafor <> null && dataline <> null then
            describe ("Test for " + datafor)
            click e
            on "https://github.com/lefthandedgoat/canopy/blob/master/canopy/canopy.fs"
            lineSelector *= datafor 
            lineSelector *= "let" 
    )

url actions
[0 .. (elements ".source").Length]
|> List.iter sourceLinkCorrect
|> ignore
()
    
run()

System.Console.ReadKey() |> ignore

quit()