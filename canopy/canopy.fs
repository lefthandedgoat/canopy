module canopy

open OpenQA.Selenium.Firefox
open OpenQA.Selenium
open OpenQA.Selenium.Support.UI
open OpenQA.Selenium.Interactions
open System
open configuration
open levenshtein
open reporters

type Action = { action : string; url : string }
type CanopyException(message) = inherit Exception(message)

let mutable actions = [];
let mutable (browser : IWebDriver) = null;
let mutable (failureMessage : string) = null
let mutable wipTest = false

//keys
let tab = Keys.Tab
let enter = Keys.Enter
let down = Keys.Down
let up = Keys.Up
let left = Keys.Left
let right = Keys.Right

//browser
let firefox = "firefox"
let ie = "ie"
let chrome = "chrome"
  
let mutable browsers = []

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
    browsers <- browsers @ [browser]

let screenshot _ = 
    let pic = (browser :?> ITakesScreenshot).GetScreenshot().AsByteArray
    IO.File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\" + DateTime.Now.ToString("MMM-d_HH-mm-ss-fff") + ".png", pic)

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
        var infoDiv = document.getElementById('canopy_info_div'); if(!infoDiv) { infoDiv = document.createElement('div'); } infoDiv.id = 'canopy_info_div'; infoDiv.setAttribute('style','position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;'); document.getElementsByTagName('body')[0].appendChild(infoDiv); infoDiv.innerHTML = '" + escapedText + "';";
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
    | :? TimeoutException as te -> raise (System.TimeoutException(te.Message))
    | ex -> failwith ex.Message
    ()

let private colorizeAndSleep (cssSelector : string) =
    puts cssSelector
    let script = System.String.Format("document.querySelector('{0}').style.backgroundColor = '#FFF467';", cssSelector)
    swallowedJs script
    sleep wipSleep
    let script = System.String.Format("document.querySelector('{0}').style.backgroundColor = '#ACD372';", cssSelector)
    swallowedJs script

let highlight (cssSelector : string) =
    let script = System.String.Format("document.querySelector('{0}').style.backgroundColor = '#ACD372';", cssSelector)
    swallowedJs script

let suggestOtherSelectors (cssSelector : string) =     
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

let private findByFunction cssSelector timeout f =
    if wipTest then colorizeAndSleep cssSelector    
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(elementTimeout))
    try
        wait.Until(fun _ -> (
                                try
                                    f(By.CssSelector(cssSelector)) |> ignore
                                    true
                                with
                                | _ -> false
                            )
                  ) |> ignore
    with
        | :? System.TimeoutException -> puts "Element not found in the allotted time. If you want to increase the time, put elementTimeout <- 10.0 anywhere before a test to increase the timeout"
                                        suggestOtherSelectors cssSelector
                                        failwith (String.Format("cant find element {0}", cssSelector))
        | ex -> failwith ex.Message

    f(By.CssSelector(cssSelector))    

let private find cssSelector timeout =
    findByFunction cssSelector timeout browser.FindElement

let private findMany cssSelector timeout =
    Seq.toList (findByFunction cssSelector timeout browser.FindElements)
    
let currentUrl _ =
    browser.Url

let logAction a = 
    let action = { action = a; url = (currentUrl ()) }
    actions <- List.append actions [action]
    ()

let on (u: string) = 
    logAction "on"    
    try
        wait pageTimeout (fun _ -> (browser.Url.Contains(u)))
    with
        | ex -> failwith (String.Format("on check failed, expected {0} got {1}", u, browser.Url));
    ()

let ( !^ ) (u : string) = 
    browser.Navigate().GoToUrl(u)
    logAction "url"
    ()

let url (u : string) = 
    !^ u

let private writeToSelect cssSelector text =
    let element = find cssSelector elementTimeout
    let options = Seq.toList (element.FindElements(By.TagName("option")))
    let option = options |> List.filter (fun e -> e.Text = text)
    if option = [] then
        failwith (String.Format("element {0} does not contain value {1}", cssSelector, text))        
    else
        option.Head.Click()

let ( << ) (cssSelector : string) (text : string) = 
    logAction "write"
    wait (elementTimeout + 1.0) (fun _ ->
        let element = find cssSelector elementTimeout
        if element.TagName = "select" then
            writeToSelect cssSelector text
        else
            let readonly = element.GetAttribute("readonly")
            if readonly = "true" then
                raise (CanopyException((String.Format("element {0} is marked as read only, you can not write to read only elements", cssSelector))))
            try element.Clear() with ex -> ex |> ignore
            element.SendKeys(text)
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
        logAction "read"
        let element = find cssSelector elementTimeout
        textOf element
    with
        | ex -> failwith ex.Message

        
let clear (cssSelector : string) = 
    logAction "clear"
    let element = find cssSelector elementTimeout
    let readonly = element.GetAttribute("readonly")
    if readonly = "true" then
        failwith (String.Format("element {0} is marked as read only, you can not clear read only elements", cssSelector))        
    element.Clear()
    
let click item = 
    try
        logAction "click"
        match box item with
        | :? IWebElement as element -> element.Click()
        | :? string as cssSelector ->         
            wait elementTimeout (fun _ -> let element = find cssSelector elementTimeout
                                          element.Click()
                                          true)
        | _ -> failwith (String.Format("Can't click {0} because it is not a string or webelement", item.ToString()))
    with
        | ex -> failwith ex.Message

let doubleClick item =
    try
        let js = "var evt = document.createEvent('MouseEvents'); evt.initMouseEvent('dblclick',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0,null); arguments[0].dispatchEvent(evt);"

        logAction "doubleClick"
        match box item with
        | :? IWebElement as element -> (browser :?> IJavaScriptExecutor).ExecuteScript(js, element) |> ignore
        | :? string as cssSelector ->         
            wait elementTimeout (fun _ -> ( let element = find cssSelector elementTimeout
                                            (browser :?> IJavaScriptExecutor).ExecuteScript(js, element) |> ignore
                                            true))
        | _ -> failwith (String.Format("Can't doubleClick {0} because it is not a string or webelement", item.ToString()))
    with
        | ex -> failwith ex.Message

let selected (cssSelector : string) = 
    logAction "selected"
    let element = find cssSelector elementTimeout
    if element.Selected = false then
        failwith (String.Format("element selected failed, {0} not selected.", cssSelector));    
    

let deselected (cssSelector : string) =     
        logAction "deselected"
        let element = find cssSelector elementTimeout
        if element.Selected then
            failwith (String.Format("element deselected failed, {0} selected.", cssSelector));    

let title _ = browser.Title

let quit browser =
    match box browser with
    | :? OpenQA.Selenium.IWebDriver as b -> b.Quit()
    | _ -> browsers |> List.iter (fun b -> b.Quit())
    
let ( == ) (item : 'a) value =
    match box item with
    | :? IAlert as alert -> let text = alert.Text
                            if text = value then   
                                ()           
                            else
                                alert.Dismiss()
                                failwith (String.Format("equality check failed.  expected: {0}, got: {1}", value, text));                                
    | :? string as cssSelector -> 
        logAction "equals"        
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
            | ex -> failwith ex.Message
    | _ -> failwith (String.Format("Can't check equality on {0} because it is not a string or alert", item.ToString()))

let ( != ) cssSelector value =
    logAction "notequals"
    try
        wait compareTimeout (fun _ -> (read cssSelector) <> value)
    with
        | :? TimeoutException -> failwith (String.Format("not equals check failed.  expected NOT: {0}, got: {1}", value, (read cssSelector)));
        | ex -> failwith ex.Message
        
let ( *= ) (cssSelector : string) value =
    logAction "listed"
    try        
        wait compareTimeout (fun _ -> ( let elements = findMany cssSelector elementTimeout
                                        elements |> Seq.exists(fun element -> (textOf element) = value)))
    with
        | :? TimeoutException -> let sb = new System.Text.StringBuilder()
                                 let elements = findMany cssSelector elementTimeout
                                 elements |> List.map (fun e -> sb.Append(String.Format("{0}\r\n", (textOf e)))) |> ignore
                                 failwith (String.Format("cant find {0} in list {1}\r\ngot: {2}", value, cssSelector, sb.ToString()));
        | ex -> failwith ex.Message

let ( *!= ) (cssSelector : string) value =
    logAction "notlisted"
    try
        wait compareTimeout (fun _ -> ( let elements = findMany cssSelector elementTimeout
                                        elements |> Seq.exists(fun element -> (textOf element) = value) = false))
    with
        | :? TimeoutException -> failwith (String.Format("found {0} in list {1}, expected not to", value, cssSelector));
        | ex -> failwith ex.Message
    
let contains (value1 : string) (value2 : string) =
    logAction "contains"
    if (value2.Contains(value1) <> true) then
        failwith (String.Format("contains check failed.  {0} does not contain {1}", value2, value1));
    ()

let count cssSelector count =
    logAction "count"
    try        
        wait compareTimeout (fun _ -> ( let elements = findMany cssSelector elementTimeout
                                        elements.Length = count))
    with
        | :? TimeoutException -> failwith (String.Format("count failed. expected: {0} got: {1} ", count, (findMany cssSelector elementTimeout).Length));
        | ex -> failwith ex.Message

let describe (text : string) =
    puts text
    ()

let press key = 
    let element = ((js "return document.activeElement;") :?> IWebElement)
    element.SendKeys(key)

let reload _ =
    url (currentUrl ())

let failsWith message = 
    failureMessage <- message

let alert (action : 'a) =
    let alert = browser.SwitchTo().Alert()
    alert

let acceptAlert _ =
    browser.SwitchTo().Alert().Accept()

let dismissAlert _ =
    browser.SwitchTo().Alert().Dismiss()

let waitFor (f : unit -> bool) =
    try        
        wait compareTimeout (fun _ -> (f ()))
    with
        | :? System.TimeoutException -> puts "Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout"
                                        failwith (String.Format("waitFor condition failed to become true in {0} seconds", compareTimeout))
        | ex -> failwith ex.Message

let element cssSelector = 
    find cssSelector elementTimeout

let elements cssSelector =
    findMany cssSelector elementTimeout

let exists cssSelector =
    find cssSelector elementTimeout

let is expected actual =
    if expected = actual then
        ()
    else
        failwith (String.Format("equality check failed.  expected: {0}, got: {1}", expected, actual));

let (===) expected actual =
    is expected actual

let private regexMatch pattern input =
    System.Text.RegularExpressions.Regex.Match(input, pattern).Success

let elementsWithText cssSelector regex =
    findMany cssSelector elementTimeout
    |> List.filter (fun element -> regexMatch regex (textOf element))

let elementWithText cssSelector regex = 
    (elementsWithText cssSelector regex).Head

let ( =~ ) cssSelector pattern =
    logAction "regex equals"    
    try
        wait compareTimeout (fun _ -> regexMatch pattern (read cssSelector))
    with
        | :? TimeoutException -> failwith (String.Format("regex equality check failed.  expected: {0}, got: {1}", pattern, (read cssSelector)));
        | ex -> failwith ex.Message

let nth index cssSelector =
 List.nth (elements cssSelector) index

let first cssSelector =
 (elements cssSelector).Head

let last cssSelector =
 (List.rev (elements cssSelector)).Head
      
let private shown cssSelector =
    let element = element cssSelector
    let opacity = element.GetCssValue("opacity")
    let display = element.GetCssValue("display")
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

let fadedIn cssSelector =
    (fun _ -> shown cssSelector)

let check cssSelector =
    if (element cssSelector).Selected = false then click cssSelector

let uncheck cssSelector =
    if (element cssSelector).Selected = true then click cssSelector

let (>>) cssSelectorA cssSelectorB =
    wait elementTimeout (fun _ ->
        let a = element cssSelectorA
        let b = element cssSelectorB
        (new Actions(browser)).DragAndDrop(a, b).Perform()
        true)

let drag cssSelectorA cssSelectorB =
    cssSelectorA >> cssSelectorB
    
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

let switchTo b = browser <- b

//really need to refactor so there are results for every action
//function for the action, true message, false message, something like that
//and it all gets passed into some sort of assertor, will help with code reuse
//and be able to impliment not this or not that easier