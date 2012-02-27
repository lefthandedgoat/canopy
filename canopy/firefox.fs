module firefox

open OpenQA.Selenium.Firefox
open OpenQA.Selenium
open OpenQA.Selenium.Support.UI
open System

let browser = new FirefoxDriver()

let ( !^ ) (u : string) = browser.Navigate().GoToUrl(u)

let url (u : string) = !^ u

let write (cssSelector : string) (text : string) = 
    let element = browser.FindElement(By.CssSelector(cssSelector))
    element.SendKeys(text)

let ( << ) (cssSelector : string) (text : string) = 
    write cssSelector text

let read (cssSelector : string) =    
    try
        let element = browser.FindElement(By.CssSelector(cssSelector))
        if element.TagName = "input" then
            element.GetAttribute("value")
        else
            element.Text    
    with
        | :? Exception -> "can't find element"
        
let clear (cssSelector : string) = 
    let element = browser.FindElement(By.CssSelector(cssSelector))
    element.Clear()
    
let click (cssSelector : string) = 
    try
        let element = browser.FindElement(By.CssSelector(cssSelector))
        element.Click()
    with
        | :? Exception -> System.Console.WriteLine("cant find element {0}", cssSelector);

let selected (cssSelector : string) = 
    try
        let element = browser.FindElement(By.CssSelector(cssSelector))
        if element.Selected then
            System.Console.WriteLine("element selected passed.");    
        else
            System.Console.WriteLine("element selected failed, {0} not selected.", cssSelector);    
    with
        | :? Exception -> System.Console.WriteLine("cant find element {0}", cssSelector);

let deselected (cssSelector : string) = 
    try
        let element = browser.FindElement(By.CssSelector(cssSelector))
        if element.Selected = false then
            System.Console.WriteLine("element deselected passed.");    
        else
            System.Console.WriteLine("element deselected failed, {0} selected.", cssSelector);    
    with
        | :? Exception -> System.Console.WriteLine("cant find element {0}", cssSelector);

let title _ = browser.Title

let quit _ = browser.Quit()

let equals value1 value2 =
    if (value1 = value2) then
        System.Console.WriteLine("equality check passed.");
    else
        System.Console.WriteLine("equality check failed.  expected: {0}, got: {1}", value1, value2);
    ()

let ( == ) element value =
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(3.0))
    try
        wait.Until(fun _ -> (read element) = value) |> ignore
        equals value (read element)
    with
        | :? Exception -> equals value "timeout trying to find element"

let listed (cssSelector : string) value =
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(3.0))
    try
        wait.Until(fun _ -> browser.FindElements(By.CssSelector(cssSelector)) |> Seq.exists(fun element -> element.Text = value)) |> ignore
        equals value value
    with
        | :? Exception -> equals value "timeout trying to find matching element in list"    

let ( *= ) (cssSelector : string) value =
    listed cssSelector value
    
let contains (value1 : string) (value2 : string) =
    if (value2.Contains(value1) = true) then
        System.Console.WriteLine("contains check passed.");
    else
        System.Console.WriteLine("contains check failed.  {0} does not contain {1}", value2, value1);        
    ()

let describe (text : string) =
    System.Console.WriteLine(text);
    ()

let currentUrl _ =
    browser.Url

let on (u: string) = 
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(10.0))
    try
        System.Console.WriteLine(browser.Url);
        System.Console.WriteLine(u);
        wait.Until(fun _ -> (browser.Url = u)) |> ignore
        System.Console.WriteLine("on check passed.");
    with
        | :? Exception -> System.Console.WriteLine("on check failed.");
    ()