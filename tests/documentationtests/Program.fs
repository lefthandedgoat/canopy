open canopy
open runner
open OpenQA.Selenium
open configuration
open reporters
reporter <- new LiveHtmlReporter() :> IReporter
elementTimeout <- 2.0
compareTimeout <- 2.0

start chrome

let actions = @"file:///C:/actions.html"

context "test source links"

let sourceLinkCorrect index datafor = 
    
    ntest ("Test for " + datafor) (fun _ ->        
        url actions
        let e = nth index ".source"
        let dataline = e.GetAttribute("data-line")
        let lineSelector = "#LC" + dataline + " span"
        if datafor <> null && dataline <> null then
            click e
            on "https://github.com/lefthandedgoat/canopy/blob/master/canopy/canopy.fs"
            lineSelector *= datafor 
            lineSelector *= "let" 
    )

url actions
[0 .. (elements ".source").Length - 1]
|> List.iter (fun index -> 
    let e = nth index ".source"
    let datafor = e.GetAttribute("data-for")
    sourceLinkCorrect index datafor)
|> ignore
()
    
run()

System.Console.ReadKey() |> ignore

quit()