module canopy

open OpenQA.Selenium.Firefox
open OpenQA.Selenium
open OpenQA.Selenium.Support.UI
open System

type Action = { action : string; url : string }

let mutable (actions : Action list) = [];
let mutable (browser : IWebDriver) = null;
let mutable chromedir = @"c:\"
let mutable elementTimeout = 3.0
let mutable compareTimeout = 3.0
let mutable pageTimeout = 10.0

//keys
let tab = Keys.Tab
let enter = Keys.Enter
let down = Keys.Down
let up = Keys.Up
let left = Keys.Left
let right = Keys.Right

let start (b : string) =    
    //for chrome you need to download chromedriver.exe from http://code.google.com/p/chromedriver/wiki/GettingStarted
    //place chromedriver.exe in c:\ or you can place it in a customer location and change chromedir value above
    //for ie you need to set Settings -> Advance -> Security Section -> Check-Allow active content to run files on My Computer*
    //firefox just works
    match b with
    | "ie" -> browser <- new OpenQA.Selenium.IE.InternetExplorerDriver() :> IWebDriver
    | "chrome" -> browser <- new OpenQA.Selenium.Chrome.ChromeDriver(chromedir) :> IWebDriver
    | _ -> browser <- new OpenQA.Selenium.Firefox.FirefoxDriver() :> IWebDriver
    ()

let private findByFunction cssSelector timeout f =
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
        | :? System.TimeoutException -> failwith (String.Format("cant find element {0}", cssSelector))
        | ex -> failwith ex.Message

    f(By.CssSelector(cssSelector))    

let private find cssSelector timeout =
    findByFunction cssSelector timeout browser.FindElement

let private findMany cssSelector timeout =
    findByFunction cssSelector timeout browser.FindElements

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
    let action = { action = a; url = currentUrl () }
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

let write (cssSelector : string) (text : string) = 
    logAction "write"
    let element = find cssSelector elementTimeout
    element.SendKeys(text)

let ( << ) (cssSelector : string) (text : string) = 
    write cssSelector text

let read (cssSelector : string) =    
    try
        logAction "read"
        let element = find cssSelector elementTimeout
        if element.TagName = "input" then
            element.GetAttribute("value")
        else
            element.Text    
    with
        | ex -> failwith ex.Message

        
let clear (cssSelector : string) = 
    logAction "clear"
    let element = find cssSelector elementTimeout
    element.Clear()
    
let click (cssSelector : string) = 
    try
        logAction "click"
        let element = find cssSelector elementTimeout
        keepTrying element.Click
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
    logAction "quit"
    browser.Quit()    
    browser <- null
    ()

let equals value1 value2 =
    logAction "equals"
    if (value1 <> value2) then
        failwith (String.Format("equality check failed.  expected: {0}, got: {1}", value1, value2));
    ()

let ( == ) cssSelector value =
    logAction "equals"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(compareTimeout))
    try
        wait.Until(fun _ -> (read cssSelector) = value) |> ignore
    with
        | :? TimeoutException -> failwith (String.Format("equality check failed.  expected: {0}, got: {1}", value, (read cssSelector)));
        | ex -> failwith ex.Message

let notequals value1 value2 =
    logAction "notequals"
    if (value1 = value2) then
        failwith (String.Format("notequals check failed.", value1, value2));
    ()

let ( != ) cssSelector value =
    logAction "notequals"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(compareTimeout))
    try
        wait.Until(fun _ -> (read cssSelector) <> value) |> ignore
    with
        | :? TimeoutException -> failwith (String.Format("not equals check failed.  expected NOT: {0}, got: {1}", value, (read cssSelector)));
        | ex -> failwith ex.Message

let listed (cssSelector : string) value =
    logAction "list"
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

let ( *= ) (cssSelector : string) value =
    listed cssSelector value

let notlisted (cssSelector : string) value =
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

let ( *!= ) (cssSelector : string) value =
    notlisted cssSelector value
    
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
    