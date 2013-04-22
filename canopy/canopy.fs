module canopy

open OpenQA.Selenium.Firefox
open OpenQA.Selenium
open OpenQA.Selenium.Support.UI
open OpenQA.Selenium.Interactions
open System.IO
open System
open configuration
open levenshtein
open reporters

type CanopyException(message) = inherit Exception(message)

let mutable (browser : IWebDriver) = null;
let mutable (failureMessage : string) = null
let mutable wipTest = false
let mutable searchedFor : (string * string) list = []

//directions
type direction =
    | Left
    | Right

//browser
let firefox = "firefox"
let ie = "ie"
let chrome = "chrome"
  
let mutable browsers = []

//misc
let failsWith message = failureMessage <- message

let screenshot directory filename =
    let pic = (browser :?> ITakesScreenshot).GetScreenshot().AsByteArray
    if Directory.Exists(directory) = false then Directory.CreateDirectory(directory) |> ignore
    IO.File.WriteAllBytes(directory + filename + ".png", pic)
    pic
    
let js script = (browser :?> IJavaScriptExecutor).ExecuteScript(script)

let private swallowedJs script = try js script |> ignore with | ex -> ()

let sleep seconds =
    match box seconds with
    | :? int as i -> System.Threading.Thread.Sleep(i * 1000)
    | _ -> System.Threading.Thread.Sleep(1 * 1000)

let puts (text : string) = 
    reporter.write text
    let escapedText = text.Replace("'", @"\'");
    let info = "
        var infoDiv = document.getElementById('canopy_info_div'); if(!infoDiv) { infoDiv = document.createElement('div'); } infoDiv.id = 'canopy_info_div'; infoDiv.setAttribute('style','position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;'); document.getElementsByTagName('body')[0].appendChild(infoDiv); infoDiv.innerHTML = 'locating: " + escapedText + "';";
    swallowedJs info

let private wait timeout f =
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(timeout))
    try
        wait.Until(fun _ -> (
                                try
                                    (f ()) = true
                                with
                                | :? CanopyException as ce -> failwith ce.Message
                                | _ -> false
                            )
                  ) |> ignore        
    with
    | :? OpenQA.Selenium.WebDriverTimeoutException as te -> raise (System.TimeoutException(te.Message))
    | ex -> failwith ex.Message
    ()

let private colorizeAndSleep cssSelector =
    puts cssSelector
    swallowedJs (System.String.Format("document.querySelector('{0}').style.border = 'thick solid #FFF467';", cssSelector))
    sleep wipSleep    
    swallowedJs (System.String.Format("document.querySelector('{0}').style.border = 'thick solid #ACD372';", cssSelector))

let highlight (cssSelector : string) = swallowedJs (System.String.Format("document.querySelector('{0}').style.border = 'thick solid #ACD372';", cssSelector))

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
    ()

let waitFor (f : unit -> bool) =
    try        
        wait compareTimeout (fun _ -> (f ()))
    with
        | :? System.TimeoutException -> puts "Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout"
                                        failwith (String.Format("waitFor condition failed to become true in {0} seconds", compareTimeout))
        | ex -> failwith ex.Message

//find related
let private findByCss cssSelector f =
    try
        f(By.CssSelector(cssSelector)) |> List.ofSeq
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
            | head :: tail -> 
                if isField head then
                    [head]
                else
                    []
            | [] -> []
    try
        let label = browser.FindElement(By.XPath(sprintf ".//label[text() = '%s']" locator))
        if (label = null) then
            []
        else
            let id = label.GetAttribute("for")
            match id with
                | null -> firstFollowingField label
                | id -> [f(By.Id(id))]
    with | _ -> []

let private findByText text f =
    try
        let byValue = findByCss (sprintf "*[value='%s']" text) f |> List.ofSeq
        if byValue.Length > 0 then
            byValue
        else
            f(By.XPath(sprintf ".//*[text() = '%s']" text)) |> List.ofSeq
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
        let cssResult = findByCss cssSelector searchContext.FindElements
        if cssResult.IsEmpty = false then
            cssResult
        else            
            let labelResult = findByLabel cssSelector searchContext.FindElement
            if labelResult.IsEmpty = false then
                labelResult
            else
                let textResult = findByText cssSelector searchContext.FindElements
                if textResult.IsEmpty = false then
                    textResult
                else
                    let xpathResult = findByXpath cssSelector searchContext.FindElements
                    if xpathResult.IsEmpty = false then
                        xpathResult
                    else
                        findInIFrame()
    with | ex -> []

let private findByFunction cssSelector (timeout : float) waitFunc (searchContext : ISearchContext) =
    if wipTest then colorizeAndSleep cssSelector
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(elementTimeout))
    try
        wait.Until(fun _ -> waitFunc cssSelector searchContext)        
    with
        | :? WebDriverTimeoutException ->   puts "Element not found in the allotted time. If you want to increase the time, put elementTimeout <- 10.0 anywhere before a test to increase the timeout"
                                            suggestOtherSelectors cssSelector
                                            failwith (String.Format("cant find element {0}", cssSelector))
        | ex -> failwith ex.Message

let private find (cssSelector : string) (timeout : float) (searchContext : ISearchContext) =
    (findByFunction cssSelector timeout findElements searchContext).Head

let private findMany cssSelector timeout (searchContext : ISearchContext) =
    findByFunction cssSelector timeout findElements searchContext

//get elements
let element cssSelector = find cssSelector elementTimeout browser

let elementWithin cssSelector (elem:IWebElement) =  find cssSelector elementTimeout elem

let parent (elem:IWebElement) = elem |> elementWithin ".."

let elements cssSelector = findMany cssSelector elementTimeout browser

let elementsWithin cssSelector (elem:IWebElement) = findMany cssSelector elementTimeout elem

let exists cssSelector = find cssSelector elementTimeout browser

let nth index cssSelector = List.nth (elements cssSelector) index

let first cssSelector = (elements cssSelector).Head

let last cssSelector = (List.rev (elements cssSelector)).Head
   
//read/write
let private writeToSelect cssSelector text =
    let elem = element cssSelector
    let options = Seq.toList (elem.FindElements(By.TagName("option")))
    let option = options |> List.filter (fun e -> e.Text = text)
    if option = [] then
        failwith (String.Format("element {0} does not contain value {1}", cssSelector, text))        
    else
        option.Head.Click()

let ( << ) cssSelector (text : string) = 
    wait (elementTimeout + 1.0) (fun _ ->
        let elems = elements cssSelector
        let writeToElement (e : IWebElement) =
            if e.TagName = "select" then
                writeToSelect cssSelector text
            else
                let readonly = e.GetAttribute("readonly")
                if readonly = "true" then
                    raise (CanopyException((String.Format("element {0} is marked as read only, you can not write to read only elements", cssSelector))))
                try e.Clear() with ex -> ex |> ignore
                e.SendKeys(text)

        elems |> List.iter writeToElement
        true)

let private textOf (element : IWebElement) =
    if element.TagName = "input" then
        element.GetAttribute("value")
    else if element.TagName = "select" then
        let value = element.GetAttribute("value")
        let options = Seq.toList (element.FindElements(By.TagName("option")))
        let option = options |> List.filter (fun e -> e.GetAttribute("value") = value)
        option.Head.Text
    else
        element.Text    

let read (cssSelector : string) =    
    try
        let elem = element cssSelector
        textOf elem
    with
        | ex -> failwith ex.Message

        
let clear (cssSelector : string) = 
    let elem = element cssSelector
    let readonly = elem.GetAttribute("readonly")
    if readonly = "true" then
        failwith (String.Format("element {0} is marked as read only, you can not clear read only elements", cssSelector))        
    elem.Clear()

//status
let selected (cssSelector : string) = 
    let elem = element cssSelector
    if elem.Selected = false then
        failwith (String.Format("element selected failed, {0} not selected.", cssSelector));    
    
let deselected (cssSelector : string) =     
        let elem = element cssSelector
        if elem.Selected then
            failwith (String.Format("element deselected failed, {0} selected.", cssSelector));    

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
let alert() = browser.SwitchTo().Alert()

let acceptAlert _ = browser.SwitchTo().Alert().Accept()

let dismissAlert _ = browser.SwitchTo().Alert().Dismiss()
    
//assertions    
let ( == ) (item : 'a) value =
    match box item with
    | :? IAlert as alert -> let text = alert.Text
                            if text = value then   
                                ()           
                            else
                                alert.Dismiss()
                                failwith (String.Format("equality check failed.  expected: {0}, got: {1}", value, text));                                
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
            | :? TimeoutException -> failwith (String.Format("equality check failed.  expected: {0}, got: {1}", value, !bestvalue));
            | ex -> System.Console.WriteLine(ex.GetType());
                    failwith ex.Message
    | _ -> failwith (String.Format("Can't check equality on {0} because it is not a string or alert", item.ToString()))

let ( != ) cssSelector value =
    try
        wait compareTimeout (fun _ -> (read cssSelector) <> value)
    with
        | :? TimeoutException -> failwith (String.Format("not equals check failed.  expected NOT: {0}, got: {1}", value, (read cssSelector)));
        | ex -> failwith ex.Message
        
let ( *= ) (cssSelector : string) value =
    try        
        wait compareTimeout (fun _ -> ( let elems = elements cssSelector
                                        elems |> Seq.exists(fun element -> (textOf element) = value)))
    with
        | :? TimeoutException -> let sb = new System.Text.StringBuilder()
                                 let elems = elements cssSelector
                                 elems |> List.map (fun e -> sb.Append(String.Format("{0}\r\n", (textOf e)))) |> ignore
                                 failwith (String.Format("cant find {0} in list {1}\r\ngot: {2}", value, cssSelector, sb.ToString()));
        | ex -> failwith ex.Message

let ( *!= ) (cssSelector : string) value =
    try
        wait compareTimeout (fun _ -> ( let elems = elements cssSelector
                                        elems |> Seq.exists(fun element -> (textOf element) = value) = false))
    with
        | :? TimeoutException -> failwith (String.Format("found {0} in list {1}, expected not to", value, cssSelector));
        | ex -> failwith ex.Message
    
let contains (value1 : string) (value2 : string) =
    if (value2.Contains(value1) <> true) then
        failwith (String.Format("contains check failed.  {0} does not contain {1}", value2, value1));
    ()

let count cssSelector count =
    try        
        wait compareTimeout (fun _ -> ( let elems = elements cssSelector
                                        elems.Length = count))
    with
        | :? TimeoutException -> failwith (String.Format("count failed. expected: {0} got: {1} ", count, (elements cssSelector).Length));
        | ex -> failwith ex.Message

let private regexMatch pattern input = System.Text.RegularExpressions.Regex.Match(input, pattern).Success

let elementsWithText cssSelector regex =
    elements cssSelector
    |> List.filter (fun elem -> regexMatch regex (textOf elem))

let elementWithText cssSelector regex = (elementsWithText cssSelector regex).Head

let ( =~ ) cssSelector pattern =
    try
        wait compareTimeout (fun _ -> regexMatch pattern (read cssSelector))
    with
        | :? TimeoutException -> failwith (String.Format("regex equality check failed.  expected: {0}, got: {1}", pattern, (read cssSelector)));
        | ex -> failwith ex.Message

let ( *~ ) (cssSelector : string) pattern =
    try        
        wait compareTimeout (fun _ -> ( let elems = elements cssSelector
                                        elems |> Seq.exists(fun element -> regexMatch pattern (textOf element))))
    with
        | :? TimeoutException -> let sb = new System.Text.StringBuilder()
                                 let elems = elements cssSelector
                                 elems |> List.map (fun e -> sb.Append(String.Format("{0}\r\n", (textOf e)))) |> ignore
                                 failwith (String.Format("cant regex find {0} in list {1}\r\ngot: {2}", pattern, cssSelector, sb.ToString()));
        | ex -> failwith ex.Message

let is expected actual =
    if expected = actual then
        ()
    else
        failwith (String.Format("equality check failed.  expected: {0}, got: {1}", expected, actual));

let (===) expected actual = is expected actual

let private shown cssSelector =
    let elem = element cssSelector
    let opacity = elem.GetCssValue("opacity")
    let display = elem.GetCssValue("display")
    display <> "none" && opacity = "1"
       
let displayed cssSelector =
    try
        wait compareTimeout (fun _ ->  shown cssSelector = true)
    with
        | :? TimeoutException -> failwith (String.Format("display checked for {0} failed.", cssSelector));
        | ex -> failwith ex.Message

let notDisplayed cssSelector =
    try
        wait compareTimeout (fun _ -> shown cssSelector = false)
        ()
    with
        | :? TimeoutException -> failwith (String.Format("notDisplay checked for {0} failed.", cssSelector));
        | ex -> failwith ex.Message    

let fadedIn cssSelector = (fun _ -> shown cssSelector)

//clicking/checking
let click item = 
    try
        match box item with
        | :? IWebElement as element -> element.Click()
        | :? string as cssSelector ->         
            wait elementTimeout (fun _ -> let elem = element cssSelector
                                          elem.Click()
                                          true)
        | _ -> failwith (String.Format("Can't click {0} because it is not a string or webelement", item.ToString()))
    with
        | ex -> failwith ex.Message

let doubleClick item =
    try
        let js = "var evt = document.createEvent('MouseEvents'); evt.initMouseEvent('dblclick',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0,null); arguments[0].dispatchEvent(evt);"

        match box item with
        | :? IWebElement as elem -> (browser :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
        | :? string as cssSelector ->         
            wait elementTimeout (fun _ -> ( let elem = element cssSelector
                                            (browser :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
                                            true))
        | _ -> failwith (String.Format("Can't doubleClick {0} because it is not a string or webelement", item.ToString()))
    with
        | ex -> failwith ex.Message

let check cssSelector = if (element cssSelector).Selected = false then click cssSelector

let uncheck cssSelector = if (element cssSelector).Selected = true then click cssSelector

//draggin
let (>>) cssSelectorA cssSelectorB =
    wait elementTimeout (fun _ ->
        let a = element cssSelectorA
        let b = element cssSelectorB
        (new Actions(browser)).DragAndDrop(a, b).Perform()
        true)

let drag cssSelectorA cssSelectorB = cssSelectorA >> cssSelectorB
    
//browser related
let pin direction =   
    let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
    let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
    let maxWidth = w / 2    
    browser.Manage().Window.Size <- new System.Drawing.Size(maxWidth,h);        
    match direction with
    | Left -> browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 0),0);   
    | Right -> browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 1),0);   

let start (b : string) =    
    //for chrome you need to download chromedriver.exe from http://code.google.com/p/chromedriver/wiki/GettingStarted
    //place chromedriver.exe in c:\ or you can place it in a customer location and change chromeDir value above
    //for ie you need to set Settings -> Advance -> Security Section -> Check-Allow active content to run files on My Computer*
    //also download IEDriverServer and place in c:\ or configure with ieDir
    //firefox just works
    match b with
    | "ie" -> browser <- new OpenQA.Selenium.IE.InternetExplorerDriver(ieDir) :> IWebDriver
    | "chrome" -> browser <- new OpenQA.Selenium.Chrome.ChromeDriver(chromeDir) :> IWebDriver
    | _ -> browser <- new OpenQA.Selenium.Firefox.FirefoxDriver() :> IWebDriver
    if autoPinBrowserRightOnLaunch = true then pin Right
    browsers <- browsers @ [browser]

let switchTo b = browser <- b

let tile (browsers : OpenQA.Selenium.IWebDriver list) =   
    let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
    let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
    let count = browsers.Length
    let maxWidth = w / count

    let rec setSize (browsers : OpenQA.Selenium.IWebDriver list) c =
        match browsers with
        | [] -> ()
        | b :: tail -> 
            b.Manage().Window.Size <- new System.Drawing.Size(maxWidth,h);        
            b.Manage().Window.Position <- new System.Drawing.Point((maxWidth * c),0);
            setSize tail (c + 1)
    
    setSize browsers 0

let quit browser =
    reporter.quit()
    match box browser with
    | :? OpenQA.Selenium.IWebDriver as b -> b.Quit()
    | _ -> browsers |> List.iter (fun b -> b.Quit())

let currentUrl _ = browser.Url

let on (u: string) = 
    try
        wait pageTimeout (fun _ -> (browser.Url.Contains(u)))
    with
        | ex -> failwith (String.Format("on check failed, expected {0} got {1}", u, browser.Url));
    ()

let ( !^ ) (u : string) = browser.Navigate().GoToUrl(u)

let url (u : string) = !^ u

let title _ = browser.Title

let reload _ = url (currentUrl ())

let coverage url =
    let selectors = 
        searchedFor 
        |> List.filter(fun (c, u) -> u = url) 
        |> List.map(fun (cssSelector, u) -> cssSelector) 
        |> Seq.distinct 
        |> List.ofSeq
    
    let script cssSelector = 
        "var results = document.querySelectorAll('" + cssSelector + "'); \
        for (var i=0; i < results.length; i++){ \
            results[i].style.border = 'thick solid #ACD372'; \
        }"
    
    !^ url
    on url
    selectors |> List.iter(fun cssSelector -> swallowedJs (script cssSelector))
    let p = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\"
    let f = DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
    let ss = screenshot p f
    reporter.coverage url ss