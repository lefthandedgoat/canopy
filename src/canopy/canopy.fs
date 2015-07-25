[<AutoOpen>]
module canopy.core

open OpenQA.Selenium.Firefox
open OpenQA.Selenium
open OpenQA.Selenium.Support.UI
open OpenQA.Selenium.Interactions
open SizSelCsZzz
open Microsoft.FSharp.Core.Printf
open System.IO
open System
open configuration
open levenshtein
open reporters
open types
open finders
open System.Drawing
open System.Drawing.Imaging
open canopy.guts

let mutable (browser : IWebDriver) = null
let mutable (failureMessage : string) = null
let mutable wipTest = false
let mutable searchedFor : (string * string) list = []

let firefox = Firefox
let aurora = FirefoxWithPath(@"C:\Program Files (x86)\Aurora\firefox.exe")
let ie = IE
let chrome = Chrome
let phantomJS = PhantomJS
let phantomJSProxyNone = PhantomJSProxyNone

let mutable browsers = []

//misc
let failsWith message = failureMessage <- message

let screenshot = __screenshot browser

let js = __js browser

let sleep seconds =
    let ms = match box seconds with
              | :? int as i -> i * 1000
              | :? float as i -> Convert.ToInt32(i * 1000.0)
              | _ -> 1000
    System.Threading.Thread.Sleep(ms)

let puts = __puts browser

let highlight = __highlight browser

let suggestOtherSelectors = __suggestOtherSelectors browser

let describe = puts

let waitFor2 = __waitFor2 browser

let waitFor = __waitFor browser

//get elements
let elements = __elements browser

let unreliableElements = __unreliableElements browser

let unreliableElement  = __unreliableElement browser

let element = __element browser

let elementWithin selector elem = __elementWithin browser selector elem

let parent elem = __parent browser elem

let elementsWithin selector elem = __elementsWithin browser selector elem

let unreliableElementsWithin selector elem = __unreliableElementsWithin browser selector elem

let someElement = __someElement browser

let someElementWithin selector elem = __someElementWithin browser selector elem

let someParent elem = __someParent browser elem

let nth = __nth browser

let first = __first browser

let last  = __last browser

//read/write
let ( << ) item text = __write browser item text

let read item = __read browser item

let clear item = __clear browser item

//status
let selected item =  __selected browser item

let deselected item = __deselected browser item

//keyboard
let tab = Keys.Tab
let enter = Keys.Enter
let down = Keys.Down
let up = Keys.Up
let left = Keys.Left
let right = Keys.Right

let press = __press browser

//alerts
let alert () = __alert browser

let acceptAlert () = __acceptAlert browser

let dismissAlert () = __dismissAlert browser

//assertions
let ( == ) item value = __equals browser item value

let ( != ) cssSelector value = __notEqual browser cssSelector value

let ( *= ) cssSelector value = __oneOfManyEqual browser cssSelector value

let ( *!= ) cssSelector value = __oneOfManyNotEqual browser cssSelector value

let contains = __contains

let count = __count browser

let elementsWithText = __elementsWithText browser

let elementWithText = __elementWithText browser

let ( =~ ) cssSelector pattern = __regexEqual browser cssSelector pattern

let ( *~ ) cssSelector pattern = __regexOneOfManyEqual browser cssSelector pattern

let is = __is

let (===) expected actual = is expected actual

let displayed item = __displayed browser item

let notDisplayed item = __notDisplayed browser item

let enabled item = __enabled browser item

let disabled item = __disabled browser item

let fadedIn selector = __fadedIn browser selector

//clicking/checking
let click item = __click browser item

let doubleClick item = __doubleClick browser item

let check item = __check browser item

let uncheck item = __uncheck browser item

//hoverin
let hover = __hover browser

//draggin
let drag = __drag browser

let (-->) cssSelectorA cssSelectorB = drag cssSelectorA cssSelectorB

//browser related
let pin = __pin browser

let pinToMonitor = __pinToMonitor browser

let start b =
    browser <- __start b
    browsers <- browsers @ [browser]

let switchTo b = browser <- b

let switchToTab = __switchToTab browser

let closeTab = __closeTab browser

let tile = __tile

let innerSize () = __innerSize browser

let resize = __resize browser

let rotate () = __rotate browser

let quit browser' = __quit browser' browsers

let currentUrl () = __currentUrl browser

let on = __on browser

let ( !^ ) (u : string) = __url browser u

let url u = __url browser u

let title () = __title browser

let reload = currentUrl >> url

let navigate direction = __navigate browser direction

let coverage (url : 'a) =
    ()
    //let mutable innerUrl = ""
    //match box url with
    //| :? string as u -> innerUrl <- u
    //| _ -> innerUrl <- currentUrl()
    //let nonMutableInnerUrl = innerUrl

    //let selectors =
    //    searchedFor
    //    |> List.filter(fun (c, u) -> u = nonMutableInnerUrl)
    //    |> List.map(fun (cssSelector, u) -> cssSelector)
    //    |> Seq.distinct
    //    |> List.ofSeq

    //let script cssSelector =
    //    "var results = document.querySelectorAll('" + cssSelector + "'); \
    //    for (var i=0; i < results.length; i++){ \
    //        results[i].style.border = 'thick solid #ACD372'; \
    //    }"

    ////kinda silly but the app I am current working on will redirect you to login if you try to access a url directly, so dont try if one isnt passed in
    //match box url with
    //| :? string as u -> !^ nonMutableInnerUrl
    //|_ -> ()

    //on nonMutableInnerUrl
    //selectors |> List.iter(fun cssSelector -> swallowedJs (script cssSelector))
    //let p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"canopy\")
    //let f = DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
    //let ss = screenshot p f
    //reporter.coverage nonMutableInnerUrl ss

let addFinder finder =
    let currentFinders = configuredFinders
    configuredFinders <- (fun cssSelector f ->
        currentFinders cssSelector f
        |> Seq.append (seq { yield finder cssSelector f }))

//hints
let private addHintFinder hints finder = hints |> Seq.append (seq { yield finder })
let private addSelector finder hintType selector =
    //gaurd against adding same hintType multipe times and increase size of finder seq
    if not <| (hints.ContainsKey(selector) && addedHints.[selector] |> List.exists (fun hint -> hint = hintType)) then
        if hints.ContainsKey(selector) then
            hints.[selector] <- addHintFinder hints.[selector] finder
            addedHints.[selector] <- [hintType] @ addedHints.[selector]
        else
            hints.[selector] <- seq { yield finder }
            addedHints.[selector] <- [hintType]
    selector

let css = addSelector findByCss "css"
let xpath = addSelector findByXpath "xpath"
let jquery = addSelector findByJQuery "jquery"
let label = addSelector findByLabel "label"
let text = addSelector findByText "text"
let value = addSelector findByValue "value"
