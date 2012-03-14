module canopy

open OpenQA.Selenium.Firefox
open OpenQA.Selenium
open OpenQA.Selenium.Support.UI
open System

type Action = { action : string; url : string }

let mutable (actions : Action list) = [];
let mutable (browser : IWebDriver) = null;
let mutable chromedir = @"c:\"

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

let currentUrl _ =
    browser.Url

let logAction a = 
    let action = { action = a; url = currentUrl () }
    actions <- List.append actions [action]
    ()

let on (u: string) = 
    logAction "on"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(10.0))
    try
        wait.Until(fun _ -> (browser.Url = u)) |> ignore
    with
        | :? Exception -> failwith (System.String.Format("on check failed, expected {0} got {1}", u, browser.Url));
    ()

let ( !^ ) (u : string) = 
    logAction "url"
    browser.Navigate().GoToUrl(u)

let url (u : string) = 
    !^ u

let write (cssSelector : string) (text : string) = 
    logAction "write"
    let element = browser.FindElement(By.CssSelector(cssSelector))
    element.SendKeys(text)

let ( << ) (cssSelector : string) (text : string) = 
    write cssSelector text

let read (cssSelector : string) =    
    try
        logAction "read"
        let element = browser.FindElement(By.CssSelector(cssSelector))
        if element.TagName = "input" then
            element.GetAttribute("value")
        else
            element.Text    
    with
        | :? Exception -> failwith "can't find element"

        
let clear (cssSelector : string) = 
    logAction "clear"
    let element = browser.FindElement(By.CssSelector(cssSelector))
    element.Clear()
    
let click (cssSelector : string) = 
    try
        logAction "click"
        let element = browser.FindElement(By.CssSelector(cssSelector))
        element.Click()
    with
        | :? Exception -> failwith (System.String.Format("cant find element {0}", cssSelector));

let selected (cssSelector : string) = 
    try
        logAction "selected"
        let element = browser.FindElement(By.CssSelector(cssSelector))
        if element.Selected = false then
            failwith (System.String.Format("element selected failed, {0} not selected.", cssSelector));    
    with
        | :? Exception -> failwith (System.String.Format("cant find element {0}", cssSelector));

let deselected (cssSelector : string) = 
    try
        logAction "deselected"
        let element = browser.FindElement(By.CssSelector(cssSelector))
        if element.Selected then
            failwith (System.String.Format("element deselected failed, {0} selected.", cssSelector));    
    with
        | :? Exception -> failwith (System.String.Format("cant find element {0}", cssSelector));

let title _ = browser.Title

let quit _ = 
    logAction "quit"
    browser.Quit()    
    browser = null

let equals value1 value2 =
    logAction "equals"
    if (value1 <> value2) then
        failwith (System.String.Format("equality check failed.  expected: {0}, got: {1}", value1, value2));
    ()

let ( == ) element value =
    logAction "equals"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(3.0))
    try
        wait.Until(fun _ -> (read element) = value) |> ignore
    with
        | :? Exception -> failwith (System.String.Format("cant find element {0}", element));

let notequals value1 value2 =
    logAction "notequals"
    if (value1 = value2) then
        failwith (System.String.Format("notequals check failed.", value1, value2));
    ()

let ( != ) element value =
    logAction "notequals"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(3.0))
    try
        wait.Until(fun _ -> (read element) <> value) |> ignore
    with
        | :? Exception -> failwith (System.String.Format("notequals check failed for {0} on element {1}", value, element));

let listed (cssSelector : string) value =
    logAction "list"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(3.0))
    try
        wait.Until(fun _ -> browser.FindElements(By.CssSelector(cssSelector)) |> Seq.exists(fun element -> element.Text = value)) |> ignore
    with
        | :? Exception -> failwith (System.String.Format("cant find {0} in list {1}", value, cssSelector));

let ( *= ) (cssSelector : string) value =
    listed cssSelector value

let notlisted (cssSelector : string) value =
    logAction "notlisted"
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(3.0))
    try
        wait.Until(fun _ -> browser.FindElements(By.CssSelector(cssSelector)) |> Seq.exists(fun element -> element.Text = value) = false) |> ignore
    with
        | :? Exception -> failwith (System.String.Format("found {0} in list {1}, expected not to", value, cssSelector));

let ( *!= ) (cssSelector : string) value =
    notlisted cssSelector value
    
let contains (value1 : string) (value2 : string) =
    logAction "contains"
    if (value2.Contains(value1) <> true) then
        failwith (System.String.Format("contains check failed.  {0} does not contain {1}", value2, value1));
    ()

let describe (text : string) =
    System.Console.WriteLine(text);
    ()