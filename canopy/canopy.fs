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

type CanopyException(message) = inherit Exception(message)
type CanopyReadOnlyException(message) = inherit CanopyException(message)
type CanopyOptionNotFoundException(message) = inherit CanopyException(message)
type CanopySelectionFailedExeception(message) = inherit CanopyException(message)
type CanopyDeselectionFailedException(message) = inherit CanopyException(message)
type CanopyWaitForException(message) = inherit CanopyException(message)
type CanopyElementNotFoundException(message) = inherit CanopyException(message)
type CanopyMoreThanOneElementFoundException(message) = inherit CanopyException(message)
type CanopyEqualityFailedException(message) = inherit CanopyException(message)
type CanopyNotEqualsFailedException(message) = inherit CanopyException(message)
type CanopyValueNotInListException(message) = inherit CanopyException(message)
type CanopyValueInListException(message) = inherit CanopyException(message)
type CanopyContainsFailedException(message) = inherit CanopyException(message)
type CanopyCountException(message) = inherit CanopyException(message)
type CanopyDisplayedFailedException(message) = inherit CanopyException(message)
type CanopyNotDisplayedFailedException(message) = inherit CanopyException(message)
type CanopyEnabledFailedException(message) = inherit CanopyException(message)
type CanopyDisabledFailedException(message) = inherit CanopyException(message)
type CanopyNotStringOrElementException(message) = inherit CanopyException(message)
type CanopyOnException(message) = inherit CanopyException(message)
type CanopyCheckFailedException(message) = inherit CanopyException(message)
type CanopyUncheckFailedException(message) = inherit CanopyException(message)

let mutable (browser : IWebDriver) = null
let mutable (failureMessage : string) = null
let mutable wipTest = false
let mutable searchedFor : (string * string) list = []

//directions
type direction =
    | Left
    | Right
    | FullScreen

//browser
type BrowserStartMode =
    | Firefox
    | FirefoxWithProfile of Firefox.FirefoxProfile
    | IE
    | IEWithOptions of IE.InternetExplorerOptions
    | IEWithOptionsAndTimeSpan of IE.InternetExplorerOptions * TimeSpan
    | Chrome
    | ChromeWithOptions of Chrome.ChromeOptions
    | ChromeWithOptionsAndTimeSpan of Chrome.ChromeOptions * TimeSpan
    | PhantomJS
    | PhantomJSProxyNone

let firefox = Firefox
let ie = IE
let chrome = Chrome
let phantomJS = PhantomJS
let phantomJSProxyNone = PhantomJSProxyNone
  
let mutable browsers = []

//misc
let failsWith message = failureMessage <- message

let screenshot directory filename =
    let pic = (browser :?> ITakesScreenshot).GetScreenshot().AsByteArray
    if not <| Directory.Exists(directory) 
        then Directory.CreateDirectory(directory) |> ignore
    IO.File.WriteAllBytes(Path.Combine(directory,filename + ".png"), pic)
    pic
    
let js script = (browser :?> IJavaScriptExecutor).ExecuteScript(script)

let private swallowedJs script = try js script |> ignore with | ex -> ()

let sleep seconds =
    let ms = match box seconds with
              | :? int as i -> i * 1000
              | :? float as i -> Convert.ToInt32(i * 1000.0)
              | _ -> 1000
    System.Threading.Thread.Sleep(ms)

let puts (text : string) = 
    reporter.write text
    let escapedText = text.Replace("'", @"\'")
    let info = "
        var infoDiv = document.getElementById('canopy_info_div'); 
        if(!infoDiv) { infoDiv = document.createElement('div'); } 
        infoDiv.id = 'canopy_info_div'; 
        infoDiv.setAttribute('style','position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;'); 
        document.getElementsByTagName('body')[0].appendChild(infoDiv); 
        infoDiv.innerHTML = 'locating: " + escapedText + "';"
    swallowedJs info

let private wait timeout f =
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(timeout))
    wait.Until(fun _ -> (
                            try
                                (f ()) = true
                            with
                            | :? CanopyException as ce -> raise(ce)
                            | _ -> false
                        )
                ) |> ignore        
    ()

let private colorizeAndSleep cssSelector =
    puts cssSelector
    swallowedJs <| sprintf "document.querySelector('%s').style.border = 'thick solid #FFF467';" cssSelector
    sleep wipSleep    
    swallowedJs <| sprintf "document.querySelector('%s').style.border = 'thick solid #ACD372';" cssSelector

let highlight (cssSelector : string) = 
    swallowedJs <| sprintf "document.querySelector('%s').style.border = 'thick solid #ACD372';" cssSelector

let suggestOtherSelectors (cssSelector : string) =     
    if not disableSuggestOtherSelectors then
        let allElements = browser.FindElements(By.CssSelector("html *")) |> Array.ofSeq
        let classes = allElements |> Array.Parallel.map (fun e -> "." + e.GetAttribute("class"))
        let ids = allElements |> Array.Parallel.map (fun e -> "#" + e.GetAttribute("id"))
        Array.append classes ids 
            |> Seq.distinct |> List.ofSeq 
            |> remove "." |> remove "#" |> Array.ofList
            |> Array.Parallel.map (fun u -> levenshtein cssSelector u)
            |> Array.sortBy (fun r -> r.distance)
            |> Seq.take 5
            |> Seq.map (fun r -> r.selector) |> List.ofSeq
            |> (fun suggestions -> reporter.suggestSelectors cssSelector suggestions)    

let describe (text : string) =
    puts text

let waitFor (f : unit -> bool) =
    try        
        wait compareTimeout f
    with
        | :? WebDriverTimeoutException -> 
                puts "Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout"
                raise (CanopyWaitForException(sprintf "waitFor condition failed to become true in %.1f seconds" compareTimeout))

//find related
let private findByCss cssSelector f =
    try
        f(By.CssSelector(cssSelector)) |> List.ofSeq
    with | ex -> []

let private findBySizzle cssSelector f =
    try
        f(BySizzle.CssSelector(cssSelector)) |> List.ofSeq
    with | ex -> []

let private findByJQuery cssSelector f =
    try
        f(ByJQuery.CssSelector(cssSelector)) |> List.ofSeq
    with | ex -> []

let private findByXpath xpath f =
    try
        f(By.XPath(xpath)) |> List.ofSeq
    with | ex -> []

let private findByLabel locator f =
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
        let label : IWebElement = f(By.XPath(sprintf ".//label[text() = '%s']" locator))
        if (label = null) then
            []
        else
            match label.GetAttribute("for") with
            | null -> firstFollowingField label
            | id -> [f(By.Id(id))]
    with | _ -> []

let private findByText text f =
    try
        f(By.XPath(sprintf ".//*[text() = '%s']" text)) |> List.ofSeq
    with | _ -> []

let private findByValue value f =
    try
        findByCss (sprintf "*[value='%s']" value) f |> List.ofSeq        
    with | _ -> []
    
let rec private findElements (cssSelector : string) (searchContext : ISearchContext) =
    searchedFor <- (cssSelector, browser.Url) :: searchedFor
    let findInIFrame () =
        let iframes = findByCss "iframe" searchContext.FindElements
        if iframes.IsEmpty then 
            browser.SwitchTo().DefaultContent() |> ignore
            []
        else
            let webElements = ref []
            iframes |> List.iter (fun frame -> 
                browser.SwitchTo().Frame(frame) |> ignore
                webElements := findElements cssSelector searchContext
            )
            !webElements

    try
        seq {
            yield (findByCss    cssSelector searchContext.FindElements)                        
            yield (findByValue  cssSelector searchContext.FindElements)
            yield (findByXpath  cssSelector searchContext.FindElements)
            yield (findByLabel  cssSelector searchContext.FindElement)            
            yield (findByText   cssSelector searchContext.FindElements)
            yield (findBySizzle cssSelector searchContext.FindElements)
            yield (findByJQuery cssSelector searchContext.FindElements)
            yield (findInIFrame())
        }
        |> Seq.filter(fun list -> not(list.IsEmpty))
        |> Seq.head
    with | ex -> []

let private findByFunction cssSelector (timeout : float) waitFunc (searchContext : ISearchContext) =
    if wipTest then colorizeAndSleep cssSelector
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(elementTimeout))
    try
        wait.Until(fun _ -> waitFunc cssSelector searchContext)        
    with
        | :? WebDriverTimeoutException ->   
            puts "Element not found in the allotted time. If you want to increase the time, put elementTimeout <- 10.0 anywhere before a test to increase the timeout"
            suggestOtherSelectors cssSelector
            raise (CanopyElementNotFoundException(sprintf "cant find element %s" cssSelector))

let private find (cssSelector : string) (timeout : float) (searchContext : ISearchContext) =
    (findByFunction cssSelector timeout findElements searchContext).Head

let private findMany cssSelector timeout (searchContext : ISearchContext) =
    findByFunction cssSelector timeout findElements searchContext

//get elements

let private elementFromList cssSelector elementsList =
    match elementsList with
    | [] -> null
    | x :: [] -> x    
    | x :: y :: _ -> 
        if throwIfMoreThanOneElement then raise (CanopyMoreThanOneElementFoundException(sprintf "More than one element was selected when only one was expected for selector: %s" cssSelector))
        else x

let private someElementFromList cssSelector elementsList =
    match elementsList with
    | [] -> None    
    | x :: [] -> Some(x)
    | x :: y :: _ -> 
        if throwIfMoreThanOneElement then raise (CanopyMoreThanOneElementFoundException(sprintf "More than one element was selected when only one was expected for selector: %s" cssSelector))
        else Some(x)

let elements cssSelector = findMany cssSelector elementTimeout browser
    
let element cssSelector = cssSelector |> elements |> elementFromList cssSelector

let elementWithin cssSelector (elem:IWebElement) =  find cssSelector elementTimeout elem

let parent elem = elem |> elementWithin ".."

let elementsWithin cssSelector elem = findMany cssSelector elementTimeout elem

let someElement cssSelector = cssSelector |> elements |> someElementFromList cssSelector

let someElementWithin cssSelector elem = elem |> elementsWithin cssSelector |> someElementFromList cssSelector

let someParent elem = elem |> elementsWithin ".." |> someElementFromList "provided element"

let nth index cssSelector = List.nth (elements cssSelector) index

let first cssSelector = (elements cssSelector).Head

let last cssSelector = (List.rev (elements cssSelector)).Head
   
//read/write
let private writeToSelect cssSelector text =
    let elem = element cssSelector
    let options = Seq.toList (elem.FindElements(By.XPath(sprintf "option[text()='%s']" text)))
    match options with
    | [] -> raise (CanopyOptionNotFoundException(sprintf "element %s does not contain value %s" cssSelector text))
    | head::tail -> head.Click()

let ( << ) cssSelector (text : string) = 
    wait (elementTimeout + 1.0) (fun _ ->
        
        let writeToElement (e : IWebElement) =
            if e.TagName = "select" then
                writeToSelect cssSelector text
            else
                let readonly = e.GetAttribute("readonly")
                if readonly = "true" then
                    raise (CanopyReadOnlyException(sprintf "element %s is marked as read only, you can not write to read only elements" cssSelector))
                try e.Clear() with ex -> ex |> ignore
                e.SendKeys(text)

        let atleastOneItemWritten = ref false
        elements cssSelector
        |> List.iter (fun elem -> 
            try  
                writeToElement elem
                atleastOneItemWritten := true
            with
                | :? CanopyReadOnlyException as ex -> raise ex
                | _ -> ())
        !atleastOneItemWritten)

let private textOf (element : IWebElement) =
    match element.TagName  with
    | "input" ->
        element.GetAttribute("value")
    | "textarea" ->
        element.GetAttribute("value")
    | "select" ->
        let value = element.GetAttribute("value")
        let options = Seq.toList (element.FindElements(By.TagName("option")))
        let option = options |> List.filter (fun e -> e.GetAttribute("value") = value)
        option.Head.Text
    | _ ->
        element.Text    

let read item =
    match box item with
    | :? IWebElement as elem -> textOf elem
    | :? string as cssSelector -> element cssSelector |> textOf
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't read %O because it is not a string or element" item))
        
let clear item =
    let clear cssSelector (elem : IWebElement) =
        let readonly = elem.GetAttribute("readonly")
        if readonly = "true" then raise (CanopyReadOnlyException(sprintf "element %s is marked as read only, you can not clear read only elements" cssSelector))
        elem.Clear()
    
    match box item with
    | :? IWebElement as elem -> clear elem.TagName elem
    | :? string as cssSelector -> element cssSelector |> clear cssSelector 
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't clear %O because it is not a string or element" item))

//status
let selected item = 
    let selected cssSelector (elem : IWebElement) =
        if not <| elem.Selected then raise (CanopySelectionFailedExeception(sprintf "element selected failed, %s not selected." cssSelector))

    match box item with
    | :? IWebElement as elem -> selected elem.TagName elem
    | :? string as cssSelector -> element cssSelector |> selected cssSelector
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check selected on %O because it is not a string or element" item))
    
let deselected item =     
    let deselected cssSelector (elem : IWebElement) =
        if elem.Selected then raise (CanopyDeselectionFailedException(sprintf "element deselected failed, %s selected." cssSelector))

    match box item with
    | :? IWebElement as elem -> deselected elem.TagName elem
    | :? string as cssSelector -> element cssSelector |> deselected cssSelector
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check deselected on %O because it is not a string or element" item))

//keyboard
let tab = Keys.Tab
let enter = Keys.Enter
let down = Keys.Down
let up = Keys.Up
let left = Keys.Left
let right = Keys.Right

let press key = 
    let elem = ((js "return document.activeElement;") :?> IWebElement)
    elem.SendKeys(key)

//alerts
let alert() = 
    waitFor (fun _ ->
        browser.SwitchTo().Alert() |> ignore
        true)
    browser.SwitchTo().Alert()

let acceptAlert() = 
    wait compareTimeout (fun _ ->
        browser.SwitchTo().Alert().Accept()
        true)

let dismissAlert() = 
    wait compareTimeout (fun _ ->
        browser.SwitchTo().Alert().Dismiss()
        true)
    
//assertions    
let ( == ) item value =
    match box item with
    | :? IAlert as alert -> 
        let text = alert.Text
        if text <> value then   
            alert.Dismiss()
            raise (CanopyEqualityFailedException(sprintf "equality check failed.  expected: %s, got: %s" value text))
    | :? string as cssSelector -> 
        let bestvalue = ref ""
        try
            wait compareTimeout (fun _ -> ( let readvalue = (read cssSelector)
                                            if readvalue <> value && readvalue <> "" then
                                                bestvalue := readvalue
                                                false
                                            else
                                                readvalue = value))
        with
            | :? WebDriverTimeoutException -> raise (CanopyEqualityFailedException(sprintf "equality check failed.  expected: %s, got: %s" value !bestvalue))

    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check equality on %O because it is not a string or alert" item))

let ( != ) cssSelector value =
    try
        wait compareTimeout (fun _ -> (read cssSelector) <> value)
    with
        | :? WebDriverTimeoutException -> raise (CanopyNotEqualsFailedException(sprintf "not equals check failed.  expected NOT: %s, got: %s" value (read cssSelector)))
        
let ( *= ) (cssSelector : string) value =
    try        
        wait compareTimeout (fun _ -> ( cssSelector |> elements |> Seq.exists(fun element -> (textOf element) = value)))
    with
        | :? WebDriverTimeoutException -> 
                let sb = new System.Text.StringBuilder()
                cssSelector |> elements |> List.iter (fun e -> bprintf sb "%s\r\n" (textOf e))
                raise (CanopyValueNotInListException(sprintf "cant find %s in list %s\r\ngot: %s" value cssSelector (sb.ToString())))

let ( *!= ) (cssSelector : string) value =
    try
        wait compareTimeout (fun _ -> ( cssSelector |> elements |> Seq.exists(fun element -> (textOf element) = value) |> not))
    with
        | :? WebDriverTimeoutException -> raise (CanopyValueInListException(sprintf "found %s in list %s, expected not to" value cssSelector))
    
let contains (value1 : string) (value2 : string) =
    if (value2.Contains(value1) <> true) then
        raise (CanopyContainsFailedException(sprintf "contains check failed.  %s does not contain %s" value2 value1))

let count cssSelector count =
    try        
        wait compareTimeout (fun _ -> ( let elems = elements cssSelector
                                        elems.Length = count))
    with
        | :? WebDriverTimeoutException -> raise (CanopyCountException(sprintf "count failed. expected: %i got: %i" count (elements cssSelector).Length))

let private regexMatch pattern input = System.Text.RegularExpressions.Regex.Match(input, pattern).Success

let elementsWithText cssSelector regex =
    elements cssSelector
    |> List.filter (fun elem -> regexMatch regex (textOf elem))

let elementWithText cssSelector regex = (elementsWithText cssSelector regex).Head

let ( =~ ) cssSelector pattern =
    try
        wait compareTimeout (fun _ -> regexMatch pattern (read cssSelector))
    with
        | :? WebDriverTimeoutException -> raise (CanopyEqualityFailedException(sprintf "regex equality check failed.  expected: %s, got: %s" pattern (read cssSelector)))

let ( *~ ) (cssSelector : string) pattern =
    try        
        wait compareTimeout (fun _ -> ( cssSelector |> elements |> Seq.exists(fun element -> regexMatch pattern (textOf element))))
    with
        | :? WebDriverTimeoutException -> 
                let sb = new System.Text.StringBuilder()
                cssSelector |> elements |> List.iter (fun e -> bprintf sb "%s\r\n" (textOf e))
                raise (CanopyValueNotInListException(sprintf "cant regex find %s in list %s\r\ngot: %s" pattern cssSelector (sb.ToString())))

let is expected actual =
    if expected <> actual then
        raise (CanopyEqualityFailedException(sprintf "equality check failed.  expected: %O, got: %O" expected actual))

let (===) expected actual = is expected actual

let private shown (elem : IWebElement) =    
    let opacity = elem.GetCssValue("opacity")
    let display = elem.GetCssValue("display")
    display <> "none" && opacity = "1"
       
let displayed item =
    try
        wait compareTimeout (fun _ -> 
            match box item with
            | :? IWebElement as element ->  shown element
            | :? string as cssSelector -> element cssSelector |> shown
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item)))
    with
        | :? WebDriverTimeoutException -> raise (CanopyDisplayedFailedException(sprintf "display check for %O failed." item))

let notDisplayed item =
    try
        wait compareTimeout (fun _ -> 
            match box item with
            | :? IWebElement as element -> not(shown element)
            | :? string as cssSelector -> (elements cssSelector |> List.isEmpty) || not(element cssSelector |> shown)
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item)))
    with
        | :? WebDriverTimeoutException -> raise (CanopyNotDisplayedFailedException(sprintf "notDisplay check for %O failed." item))

let enabled item = 
    try
        wait compareTimeout (fun _ -> 
            match box item with
            | :? IWebElement as element -> element.Enabled = true
            | :? string as cssSelector -> (element cssSelector).Enabled = true
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item)))
    with
        | :? WebDriverTimeoutException -> raise (CanopyEnabledFailedException(sprintf "enabled check for %O failed." item))

let disabled item = 
    try
        wait compareTimeout (fun _ -> 
            match box item with
            | :? IWebElement as element -> element.Enabled = false
            | :? string as cssSelector -> (element cssSelector).Enabled = false
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item)))
    with
        | :? WebDriverTimeoutException -> raise (CanopyDisabledFailedException(sprintf "disabled check for %O failed." item))

let fadedIn cssSelector = fun _ -> element cssSelector |> shown

//clicking/checking
let click item =     
    match box item with
    | :? IWebElement as element -> element.Click()
    | :? string as cssSelector ->
        wait elementTimeout (fun _ -> 
            let atleastOneItemClicked = ref false
            elements cssSelector
            |> List.iter (fun elem ->                                 
                try
                    elem.Click()
                    atleastOneItemClicked := true
                with | ex -> ())
            !atleastOneItemClicked)
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item))
    
let doubleClick item =
    let js = "var evt = document.createEvent('MouseEvents'); evt.initMouseEvent('dblclick',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0,null); arguments[0].dispatchEvent(evt);"

    match box item with
    | :? IWebElement as elem -> (browser :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
    | :? string as cssSelector ->         
        wait elementTimeout (fun _ -> ( let elem = element cssSelector
                                        (browser :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
                                        true))
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't doubleClick %O because it is not a string or webelement" item))


let check item = 
    try
        match box item with
        | :? IWebElement as elem -> if not <| elem.Selected then click elem
        | :? string as cssSelector -> 
            waitFor (fun _ -> 
                if not <| (element cssSelector).Selected then click cssSelector
                (element cssSelector).Selected)        
        | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't read %O because it is not a string or element" item))
    with
        | :? WebDriverTimeoutException -> raise (CanopyCheckFailedException(sprintf "failed to check %O." item))

let uncheck item = 
    try
        match box item with
        | :? IWebElement as elem -> if elem.Selected then click elem
        | :? string as cssSelector -> 
            waitFor (fun _ -> 
                if (element cssSelector).Selected then click cssSelector
                (element cssSelector).Selected = false)        
        | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't read %O because it is not a string or element" item))
    with
        | :? WebDriverTimeoutException -> raise (CanopyUncheckFailedException(sprintf "failed to uncheck %O." item))

//draggin
let (-->) cssSelectorA cssSelectorB =
    wait elementTimeout (fun _ ->
        let a = element cssSelectorA
        let b = element cssSelectorB
        (new Actions(browser)).DragAndDrop(a, b).Perform()
        true)

let drag cssSelectorA cssSelectorB = cssSelectorA --> cssSelectorB
    
//browser related
let pin direction =   
    let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height
    let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width
    let maxWidth = w / 2    
    browser.Manage().Window.Size <- new System.Drawing.Size(maxWidth,h)        
    match direction with
    | Left -> browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 0),0)
    | Right -> browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 1),0)
    | FullScreen -> browser.Manage().Window.Maximize()

let start b =    
    //for chrome you need to download chromedriver.exe from http://code.google.com/p/chromedriver/wiki/GettingStarted
    //place chromedriver.exe in c:\ or you can place it in a customer location and change chromeDir value above
    //for ie you need to set Settings -> Advance -> Security Section -> Check-Allow active content to run files on My Computer*
    //also download IEDriverServer and place in c:\ or configure with ieDir
    //firefox just works
    //for phantomjs download it and put in c:\ or configure with phantomJSDir
    browser <-
        match b with
        | IE -> 
            new OpenQA.Selenium.IE.InternetExplorerDriver(ieDir) :> IWebDriver
        | IEWithOptions options ->
            new OpenQA.Selenium.IE.InternetExplorerDriver(ieDir, options) :> IWebDriver
        | IEWithOptionsAndTimeSpan(options, timeSpan) ->
            new OpenQA.Selenium.IE.InternetExplorerDriver(ieDir, options, timeSpan) :> IWebDriver
        | Chrome -> 
            new OpenQA.Selenium.Chrome.ChromeDriver(chromeDir) :> IWebDriver
        | ChromeWithOptions options ->
            new OpenQA.Selenium.Chrome.ChromeDriver(chromeDir, options) :> IWebDriver
        | ChromeWithOptionsAndTimeSpan(options, timeSpan) ->
            new OpenQA.Selenium.Chrome.ChromeDriver(chromeDir, options, timeSpan) :> IWebDriver
        | Firefox -> 
            new OpenQA.Selenium.Firefox.FirefoxDriver() :> IWebDriver
        | FirefoxWithProfile profile -> 
            new OpenQA.Selenium.Firefox.FirefoxDriver(profile) :> IWebDriver
        | PhantomJS -> 
            autoPinBrowserRightOnLaunch <- false
            new OpenQA.Selenium.PhantomJS.PhantomJSDriver(phantomJSDir) :> IWebDriver
        | PhantomJSProxyNone -> 
            autoPinBrowserRightOnLaunch <- false
            let service = OpenQA.Selenium.PhantomJS.PhantomJSDriverService.CreateDefaultService(canopy.configuration.phantomJSDir)
            service.ProxyType <- "none"
            new OpenQA.Selenium.PhantomJS.PhantomJSDriver(service) :> IWebDriver            

    if autoPinBrowserRightOnLaunch = true then pin Right
    browsers <- browsers @ [browser]

let switchTo b = browser <- b

let tile (browsers : OpenQA.Selenium.IWebDriver list) =   
    let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height
    let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width
    let count = browsers.Length
    let maxWidth = w / count

    let rec setSize (browsers : OpenQA.Selenium.IWebDriver list) c =
        match browsers with
        | [] -> ()
        | b :: tail -> 
            b.Manage().Window.Size <- new System.Drawing.Size(maxWidth,h)        
            b.Manage().Window.Position <- new System.Drawing.Point((maxWidth * c),0)
            setSize tail (c + 1)
    
    setSize browsers 0

let quit browser =
    reporter.quit()
    match box browser with
    | :? OpenQA.Selenium.IWebDriver as b -> b.Quit()
    | _ -> browsers |> List.iter (fun b -> b.Quit())

let currentUrl() = browser.Url

let on (u: string) =
    let urlPath (u : string) =
        let url = match u with
                  | x when x.StartsWith("http") -> u  //leave absolute urls alone 
                  | _ -> "http://host/" + u.Trim('/') //ensure valid uri
        let uriBuilder = new System.UriBuilder(url)
        uriBuilder.Path.TrimEnd('/') //get the path part removing trailing slashes
    try
        wait pageTimeout (fun _ -> if browser.Url = u then true else urlPath(browser.Url) = urlPath(u))
    with
        | ex -> if browser.Url.Contains(u) = false then raise (CanopyOnException(sprintf "on check failed, expected expression '%s' got %s" u browser.Url))

let ( !^ ) (u : string) = browser.Navigate().GoToUrl(u)

let url u = !^ u

let title() = browser.Title

let reload = currentUrl >> url

let coverage (url : 'a) =    
    let mutable innerUrl = ""
    match box url with    
    | :? string as u -> innerUrl <- u
    | _ -> innerUrl <- currentUrl()
    let nonMutableInnerUrl = innerUrl

    let selectors = 
        searchedFor 
        |> List.filter(fun (c, u) -> u = nonMutableInnerUrl) 
        |> List.map(fun (cssSelector, u) -> cssSelector) 
        |> Seq.distinct 
        |> List.ofSeq
    
    let script cssSelector = 
        "var results = document.querySelectorAll('" + cssSelector + "'); \
        for (var i=0; i < results.length; i++){ \
            results[i].style.border = 'thick solid #ACD372'; \
        }"
    
    //kinda silly but the app I am current working on will redirect you to login if you try to access a url directly, so dont try if one isnt passed in
    match box url with    
    | :? string as u -> !^ nonMutableInnerUrl
    |_ -> ()

    on nonMutableInnerUrl
    selectors |> List.iter(fun cssSelector -> swallowedJs (script cssSelector))
    let p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"\canopy\")
    let f = DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
    let ss = screenshot p f
    reporter.coverage nonMutableInnerUrl ss