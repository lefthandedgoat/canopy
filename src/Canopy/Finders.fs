module Canopy.Finders

open Canopy.Logging
open OpenQA.Selenium
open System.Collections.ObjectModel
open System.Collections.Generic

//have to use the ReadonlyCollection<IWebElement> because thats what selenium uses and it wont cast to seq<IWebElement> or something, and type inference isnt playing nice
//basically a hack because I dont know a better way
let findByCss (cssSelector : string) (f : (By -> ReadOnlyCollection<IWebElement>)) =
    try
        f (By.CssSelector(cssSelector))
        |> List.ofSeq
    with ex ->
        []

let findByXpath xpath f =
    try
        f (By.XPath(xpath))
        |> List.ofSeq
    with ex ->
        []

let findByLabel locator f =
    let isInputField (element : IWebElement) =
        element.TagName = "input" && element.GetAttribute("type") <> "hidden"

    let isField (element: IWebElement) =
        element.TagName = "select" || element.TagName = "textarea" || isInputField element

    let firstFollowingField (label: IWebElement) =
        let followingElements = label.FindElements(By.XPath("./following-sibling::*[1]")) |> Seq.toList
        match followingElements with
        | head :: tail when isField head ->
            List.singleton head
        | _ ->
            []

    try
        let labels = f(By.XPath(sprintf """.//label[text() = "%s"]""" locator))
        if Seq.isEmpty labels then
            []
        else
            let (label: IWebElement) = (labels |> List.ofSeq).Head
            match label.GetAttribute("for") with
            | null ->
                firstFollowingField (labels |> List.ofSeq).Head
            | id ->
                f(By.Id(id)) |> List.ofSeq
    with _ ->
        []

let findByText text f =
    try
        f(By.XPath(sprintf """.//*[text() = "%s"]""" text)) |> List.ofSeq
    with _ ->
        []

let findByNormalizeSpaceText text f =
    try
        f(By.XPath(sprintf """.//*[normalize-space(text()) = "%s"]""" text)) |> List.ofSeq
    with _ ->
        []

let findByValue value f =
    try
        findByCss (sprintf """*[value="%s"]""" value) f |> List.ofSeq
    with _ ->
        []

/// Inspired by https://github.com/RaYell/selenium-webdriver-extensions
let private loadJQuery (browser: IWebDriver) =
    let jsBrowser = browser :?> IJavaScriptExecutor
    let jqueryExistsScript = """return (typeof window.jQuery) === 'function';"""
    let exists = jsBrowser.ExecuteScript(jqueryExistsScript) :?> bool
    if not exists then
        // https://code.jquery.com/
        let load = """
            var jq = document.createElement('script');
            jq.integrity = 'sha256-3edrmyuQ0w65f8gfBsqowzjJe2iM6n0nKciPUp8y+7E=';
            jq,crossorigin = "anonymous";
            jq.src = 'https://code.jquery.com/jquery-3.3.1.slim.min.js';
            document.getElementsByTagName('head')[0].appendChild(jq);
         """
        jsBrowser.ExecuteScript(load) |> ignore
        waitSeconds (*(Log.create "ByJQuery")*) 2.0 (fun _ -> jsBrowser.ExecuteScript(jqueryExistsScript) :?> bool)

type ByJQuery (selector) =
    inherit OpenQA.Selenium.By()

    do
        let findElements (context: ISearchContext) =
            match context with
            | :? IWebDriver as browser ->
                loadJQuery browser
                let script = sprintf """return jQuery("%s").get();""" selector
                (context :?> IJavaScriptExecutor).ExecuteScript(script) :?> ReadOnlyCollection<IWebElement>
            | :? OpenQA.Selenium.Internal.IWrapsDriver as wrapper ->
                let script = sprintf """return jQuery("%s", arguments[0]).get();""" selector
                loadJQuery wrapper.WrappedDriver
                (wrapper.WrappedDriver :?> IJavaScriptExecutor).ExecuteScript(script, wrapper) :?> ReadOnlyCollection<IWebElement>
            | other ->
                failwithf "Unexpected context '%s'" (other.GetType().FullName)

        base.FindElementsMethod <-
            fun context -> findElements context

        base.FindElementMethod <-
            fun context -> findElements context |> Seq.head

let findByJQuery jquerySelector f =
    try
        f (ByJQuery(jquerySelector) :> By)
        |> List.ofSeq
    with _ ->
        []

type Finders =
    string -> (By -> ReadOnlyCollection<IWebElement>) -> seq<IWebElement list>

// You can use this as an example to how to extend canopy by creating your own set of finders, tweaking the current collection, or adding/removing
let defaultFinders: Finders =
    (fun cssSelector f ->
        seq {
            yield findByCss                cssSelector f
            yield findByValue              cssSelector f
            yield findByXpath              cssSelector f
            yield findByLabel              cssSelector f
            yield findByText               cssSelector f
            yield findByJQuery             cssSelector f
            yield findByNormalizeSpaceText cssSelector f
        }
    )

/// TODO: remove global variable
let addedHints = Dictionary<string, string list>()
/// TODO: remove global variable
let hints = new Dictionary<string, seq<(string -> (By -> ReadOnlyCollection<IWebElement>) -> IWebElement list)>>()