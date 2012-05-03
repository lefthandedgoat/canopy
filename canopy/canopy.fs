module canopy

open OpenQA.Selenium.Firefox
open OpenQA.Selenium
open OpenQA.Selenium.Support.UI
open System
open configuration

type Action = { action : string; url : string }

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

let start (b : string) =    
    //for chrome you need to download chromedriver.exe from http://code.google.com/p/chromedriver/wiki/GettingStarted
    //place chromedriver.exe in c:\ or you can place it in a customer location and change chromeDir value above
    //for ie you need to set Settings -> Advance -> Security Section -> Check-Allow active content to run files on My Computer*
    //firefox just works
    match b with
    | "ie" -> browser <- new OpenQA.Selenium.IE.InternetExplorerDriver() :> IWebDriver
    | "chrome" -> browser <- new OpenQA.Selenium.Chrome.ChromeDriver(chromeDir) :> IWebDriver
    | _ -> browser <- new OpenQA.Selenium.Firefox.FirefoxDriver() :> IWebDriver
    ()

let sleep seconds =
    match box seconds with
    | :? int as i -> System.Threading.Thread.Sleep(i * 1000)
    | _ -> System.Threading.Thread.Sleep(1 * 1000)    
    
let private colorizeAndSleep (cssSelector : string) =
    let js = System.String.Format("document.querySelector('{0}').style.backgroundColor = '#FFF467';", cssSelector)
    try (browser :?> IJavaScriptExecutor).ExecuteScript(js) |> ignore with | ex -> ()
    sleep wipSleep
    let js = System.String.Format("document.querySelector('{0}').style.backgroundColor = '#ACD372';", cssSelector)
    try (browser :?> IJavaScriptExecutor).ExecuteScript(js) |> ignore with | ex -> ()

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
        | :? System.TimeoutException -> Console.WriteLine("Element not found in the allotted time. If you want to increase the time, put elementTimeout <- 10.0 anywhere before a test to increase the timeout")
                                        failwith (String.Format("cant find element {0}", cssSelector))
        | ex -> failwith ex.Message

    f(By.CssSelector(cssSelector))    

let private find cssSelector timeout =
    findByFunction cssSelector timeout browser.FindElement

let private findMany cssSelector timeout =
    Seq.toList (findByFunction cssSelector timeout browser.FindElements)

let private keepTrying (f : 'a -> 'b) =
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(elementTimeout))
    try
        wait.Until(fun _ -> (
                                try
                                    f () |> ignore
                                    true
                                with
                                | _ -> false
                            )
                  ) |> ignore        
    with
    | ex -> failwith ex.Message
    ()
    
let currentUrl _ =
    browser.Url

let logAction a = 
    let action = { action = a; url = (currentUrl ()) }
    actions <- List.append actions [action]
    ()

let on (u: string) = 
    logAction "on"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(pageTimeout))
    try
        wait.Until(fun _ -> (browser.Url = u)) |> ignore
    with
        | :? Exception -> failwith (String.Format("on check failed, expected {0} got {1}", u, browser.Url));
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
    let element = find cssSelector elementTimeout
    if element.TagName = "select" then
        writeToSelect cssSelector text
    else
        let readonly = element.GetAttribute("readonly")
        if readonly = "true" then
            failwith (String.Format("element {0} is marked as read only, you can not write to read only elements", cssSelector))        
        element.Clear()
        element.SendKeys(text)

let read (cssSelector : string) =    
    try
        logAction "read"
        let element = find cssSelector elementTimeout
        if element.TagName = "input" then
            element.GetAttribute("value")
        else if element.TagName = "select" then
            let value = element.GetAttribute("value")
            let options = Seq.toList (element.FindElements(By.TagName("option")))
            let option = options |> List.filter (fun e -> e.GetAttribute("value") = value)
            option.Head.Text
        else
            element.Text    
    with
        | ex -> failwith ex.Message

        
let clear (cssSelector : string) = 
    logAction "clear"
    let element = find cssSelector elementTimeout
    let readonly = element.GetAttribute("readonly")
    if readonly = "true" then
        failwith (String.Format("element {0} is marked as read only, you can not clear read only elements", cssSelector))        
    element.Clear()
    
let click (cssSelector : string) = 
    try
        logAction "click"
        let element = find cssSelector elementTimeout
        keepTrying (fun _ ->
                        let element = find cssSelector elementTimeout
                        element.Click()
                   )
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

let quit _ = 
    browser.Quit()    
    browser <- null
    ()
    
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
        let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(compareTimeout))
        let bestvalue = ref ""
        try
            wait.Until(fun _ -> (
                                    let readvalue = (read cssSelector)
                                    if readvalue <> value && readvalue <> "" then
                                        bestvalue := readvalue
                                        false
                                    else
                                        readvalue = value
                                )) |> ignore
            ()
        with
            | :? TimeoutException -> failwith (String.Format("equality check failed.  expected: {0}, got: {1}", value, !bestvalue));
            | ex -> failwith ex.Message

let ( != ) cssSelector value =
    logAction "notequals"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(compareTimeout))
    try
        wait.Until(fun _ -> (read cssSelector) <> value) |> ignore
    with
        | :? TimeoutException -> failwith (String.Format("not equals check failed.  expected NOT: {0}, got: {1}", value, (read cssSelector)));
        | ex -> failwith ex.Message


let ( *= ) (cssSelector : string) value =
    logAction "listed"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(compareTimeout))
    try        
        wait.Until(fun _ -> (
                                let elements = findMany cssSelector elementTimeout
                                elements |> Seq.exists(fun element -> element.Text = value)
                            )
                  ) |> ignore
    with
        | :? TimeoutException -> failwith (String.Format("cant find {0} in list {1}", value, cssSelector));
        | ex -> failwith ex.Message

let ( *!= ) (cssSelector : string) value =
    logAction "notlisted"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(compareTimeout))
    try
        wait.Until(fun _ -> (
                                let elements = findMany cssSelector elementTimeout
                                elements |> Seq.exists(fun element -> element.Text = value) = false
                            )
                  ) |> ignore
    with
        | :? TimeoutException -> failwith (String.Format("found {0} in list {1}, expected not to", value, cssSelector));
        | ex -> failwith ex.Message
    
let contains (value1 : string) (value2 : string) =
    logAction "contains"
    if (value2.Contains(value1) <> true) then
        failwith (String.Format("contains check failed.  {0} does not contain {1}", value2, value1));
    ()

let describe (text : string) =
    Console.WriteLine(text);
    ()

let press key = 
    let element = ((browser :?> IJavaScriptExecutor).ExecuteScript("return document.activeElement;") :?> IWebElement)
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
        let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(compareTimeout))
        wait.Until(fun _ -> (f ())) |> ignore
    with
        | :? System.TimeoutException -> Console.WriteLine("Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout")
                                        failwith (String.Format("waitFor condition failed to become true in {0} seconds", compareTimeout))
        | ex -> failwith ex.Message

let element cssSelector = 
    find cssSelector elementTimeout

let elements cssSelector =
    findMany cssSelector elementTimeout

let fadedIn cssSelector =
    let shown cssSelector () =
        let opacity = (element cssSelector).GetCssValue("opacity")
        let display = (element cssSelector).GetCssValue("display")
        display <> "none" && opacity = "1"
    shown cssSelector

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
    |> List.filter (fun element -> regexMatch regex element.Text)

let elementWithText cssSelector regex = 
    (elementsWithText cssSelector regex).Head

let ( =~ ) cssSelector pattern =
    logAction "regex equals"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(compareTimeout))
    try
        wait.Until(fun _ -> (
                                regexMatch pattern (read cssSelector)
                            )) |> ignore
        ()
    with
        | :? TimeoutException -> failwith (String.Format("regex equality check failed.  expected: {0}, got: {1}", pattern, (read cssSelector)));
        | ex -> failwith ex.Message

let nth index cssSelector =
 List.nth (elements cssSelector) index

let first cssSelector =
 (elements cssSelector).Head

let last cssSelector =
 (List.rev (elements cssSelector)).Head
       

//really need to refactor so there are results for every action
//function for the action, true message, false message, something like that
//and it all gets passed into some sort of assertor, will help with code reuse
//and be able to impliment not this or not that easier