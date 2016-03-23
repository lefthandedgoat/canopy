module canopy.finders

open OpenQA.Selenium
open System.Collections.ObjectModel
open System.Collections.Generic

//have to use the ReadonlyCollection<IWebElement> because thats what selenium uses and it wont cast to seq<IWebElement> or something, and type inference isnt playing nice
//basically a hack because I dont know a better way
let findByCss (cssSelector : string) (f : (By -> ReadOnlyCollection<IWebElement>)) =
    try
        f(By.CssSelector(cssSelector)) |> List.ofSeq
    with | ex -> []

let findByXpath xpath f =
    try
        f(By.XPath(xpath)) |> List.ofSeq
    with | ex -> []

let findByLabel locator f =
    let isInputField (element : IWebElement) =
        element.TagName = "input" && element.GetAttribute("type") <> "hidden"
    
    let isField (element : IWebElement) =
        element.TagName = "select" || element.TagName = "textarea" || isInputField element

    let firstFollowingField (label : IWebElement) =
        let followingElements = label.FindElements(By.XPath("./following-sibling::*[1]")) |> Seq.toList
        match followingElements with
            | head :: tail when isField head-> [head]
            | _ -> []
    try
        let labels = f(By.XPath(sprintf """.//label[text() = "%s"]""" locator))
        if (Seq.isEmpty labels) then
            []
        else
            let (label : IWebElement) = (labels |> List.ofSeq).Head
            match label.GetAttribute("for") with
            | null -> firstFollowingField (labels |> List.ofSeq).Head
            | id -> f(By.Id(id)) |> List.ofSeq
    with | _ -> []

let findByText text f =
    try
        f(By.XPath(sprintf """.//*[text() = "%s"]""" text)) |> List.ofSeq
    with | _ -> []

let findByValue value f =
    try
        findByCss (sprintf """*[value="%s"]""" value) f |> List.ofSeq        
    with | _ -> []

//Inspired by https://github.com/RaYell/selenium-webdriver-extensions
let private loadJQuery () = 
    let jsBrowser = browser :?> IJavaScriptExecutor
    let jqueryExistsScript = """return (typeof window.jQuery) === 'function';"""
    let exists = jsBrowser.ExecuteScript(jqueryExistsScript) :?> bool
    if not exists then
        let load = """
            var jq = document.createElement('script');
            jq.src = '//code.jquery.com/jquery-2.2.1.min.js';
            document.getElementsByTagName('head')[0].appendChild(jq);
         """
        jsBrowser.ExecuteScript(load) |> ignore
        wait 2.0 (fun _ -> jsBrowser.ExecuteScript(jqueryExistsScript) :?> bool)

//total cheese but instead of making a By type, we will just do 'FindElements' style because
//thats the only way its currently used. Cry
let findByJQuery cssSelector _ =
    try
        loadJQuery()
        let script = sprintf """return jQuery("%s").get();""" cssSelector
        (browser :?> IJavaScriptExecutor).ExecuteScript(script) :?> ReadOnlyCollection<IWebElement> |> List.ofSeq
    with | ex -> []

//you can use this as an example to how to extend canopy by creating your own set of finders, tweaking the current collection, or adding/removing
let mutable defaultFinders = 
    (fun cssSelector f ->
        seq {
            yield findByCss     cssSelector f
            yield findByValue   cssSelector f
            yield findByXpath   cssSelector f
            yield findByLabel   cssSelector f
            yield findByText    cssSelector f
            yield findByJQuery  cssSelector f
        }
    )

let addedHints = Dictionary<string, string list>()
let hints = new Dictionary<string, seq<(string -> (By -> ReadOnlyCollection<IWebElement>) -> IWebElement list)>>()