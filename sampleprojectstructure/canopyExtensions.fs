module canopyExtensions

//overwrite and extend canopy functions here

open canopy
open runner
open configuration
open OpenQA.Selenium

let disabled cssSelector = 
    waitFor (fun _ -> (element cssSelector).GetAttribute("class").Contains("buttonDisabled"))
    let currentUrl = currentUrl()
    click cssSelector
    on currentUrl

let enabled cssSelector = waitFor (fun _ -> (element cssSelector).GetAttribute("class").Contains("buttonDisabled") = false)

let red cssSelector = waitFor (fun _ -> (element cssSelector).GetAttribute("style").Contains("border-color: red"))

let clickAll cssSelector = elements cssSelector |> List.iter (fun e -> click e)