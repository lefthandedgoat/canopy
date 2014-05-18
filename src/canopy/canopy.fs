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

let private saveScreenshot directory filename pic =
    if not <| Directory.Exists(directory) 
        then Directory.CreateDirectory(directory) |> ignore
    IO.File.WriteAllBytes(Path.Combine(directory,filename + ".png"), pic)

let private takeScreenShotIfAlertUp () =
    let bitmap = new Bitmap(width= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, height= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, format=PixelFormat.Format32bppArgb); 
    use graphics = Graphics.FromImage(bitmap)
    graphics.CopyFromScreen(System.Windows.Forms.Screen.PrimaryScreen.Bounds.X, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y, 0, 0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);    
    use stream = new MemoryStream()
    bitmap.Save(stream, ImageFormat.Png)
    stream.Close()
    stream.ToArray()


let private takeScreenshot directory filename =    
    try
        let pic = (browser :?> ITakesScreenshot).GetScreenshot().AsByteArray
        saveScreenshot directory filename pic        
        pic
    with 
        | :? UnhandledAlertException as ex->                     
            let pic = takeScreenShotIfAlertUp()
            saveScreenshot directory filename pic        
            let alert = browser.SwitchTo().Alert()
            alert.Accept()
            pic

let screenshot directory filename =
    match box browser with 
        | :? ITakesScreenshot -> takeScreenshot directory filename
        | _ -> Array.empty<byte>

let js script = (browser :?> IJavaScriptExecutor).ExecuteScript(script)

let private swallowedJs script = try js script |> ignore with | ex -> ()

let sleep seconds =
    let ms = match box seconds with
              | :? int as i -> i * 1000
              | :? float as i -> Convert.ToInt32(i * 1000.0)
              | _ -> 1000
    System.Threading.Thread.Sleep(ms)

let puts text = 
    reporter.write text
    let escapedText = System.Web.HttpUtility.JavaScriptStringEncode(text)
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

let highlight cssSelector = 
    swallowedJs <| sprintf "document.querySelector('%s').style.border = 'thick solid #ACD372';" cssSelector

let suggestOtherSelectors cssSelector =     
    if not disableSuggestOtherSelectors then
        let allElements = browser.FindElements(By.CssSelector("html *")) |> Array.ofSeq
        let classesViaJs = """
            var classes = [];
            var all = document.getElementsByTagName('*');
            for (var i=0, max=all.length; i < max; i++) {
	            var ary = all[i].className.split(' ');
	            for(var j in ary){
		            if(ary[j] === ''){
			            ary.splice(j,1);
			            j--;
		            }
	            }
               classes = classes.concat(ary);
            }
            return classes;"""
        let idsViaJs = """
            var ids = [];
            var all = document.getElementsByTagName('*');
            for (var i=0, max=all.length; i < max; i++) {
	            if(all[i].id !== "") {
		            ids.push(all[i].id);
	            }   
            }
            return ids;"""
        let valuesViaJs = """
            var values = [];
            var all = document.getElementsByTagName('*');
            for (var i=0, max=all.length; i < max; i++) {
	            if(all[i].value && all[i].value !== "") {
		            values.push(all[i].value);
	            }   
            }
            return values;"""
        let textsViaJs = """
            var texts = [];
            var all = document.getElementsByTagName('*');
            for (var i=0, max=all.length; i < max; i++) {
	            if(all[i].text && all[i].tagName !== 'SCRIPT' && all[i].text !== "") {
		            texts.push(all[i].text);
	            }   
            }
            return texts;"""
        let classes = js classesViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> "." + item.ToString()) |> Array.ofSeq
        let ids = js idsViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> "#" + item.ToString()) |> Array.ofSeq
        let values = js valuesViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> item.ToString()) |> Array.ofSeq
        let texts = js textsViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> item.ToString()) |> Array.ofSeq
        Array.append classes ids
        |> Array.append values
        |> Array.append texts
        |> Seq.distinct |> List.ofSeq 
        |> remove "." |> remove "#" |> Array.ofList
        |> Array.Parallel.map (fun u -> levenshtein cssSelector u)
        |> Array.sortBy (fun r -> r.distance)
        |> Seq.take 5
        |> Seq.map (fun r -> r.selector) |> List.ofSeq
        |> (fun suggestions -> reporter.suggestSelectors cssSelector suggestions)    

let describe text =
    puts text

let waitFor2 message (f : unit -> bool) =
    try        
        wait compareTimeout f
    with
        | :? WebDriverTimeoutException ->
                raise (CanopyWaitForException(sprintf "%s%swaitFor condition failed to become true in %.1f seconds" message System.Environment.NewLine compareTimeout))

let waitFor = waitFor2 "Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout"
    
//find related    
let rec private findElements cssSelector (searchContext : ISearchContext) : IWebElement list =
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
        let results =
            configuredFinders cssSelector searchContext.FindElements        
            |> Seq.filter(fun list -> not(list.IsEmpty))
        if Seq.isEmpty results then
            findInIFrame()
        else
           results |> Seq.head
    with | ex -> []

let private findByFunction cssSelector timeout waitFunc searchContext reliable =
    if wipTest then colorizeAndSleep cssSelector
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(elementTimeout))
    try
        if reliable then
            let elements = ref []
            wait.Until(fun _ -> 
                elements := waitFunc cssSelector searchContext
                not <| List.isEmpty !elements) |> ignore
            !elements
        else
            wait.Until(fun _ -> waitFunc cssSelector searchContext)
    with
        | :? WebDriverTimeoutException ->   
            suggestOtherSelectors cssSelector
            raise (CanopyElementNotFoundException(sprintf "can't find element %s" cssSelector))

let private find cssSelector timeout searchContext reliable =
    (findByFunction cssSelector timeout findElements searchContext reliable).Head

let private findMany cssSelector timeout searchContext reliable =
    findByFunction cssSelector timeout findElements searchContext reliable

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

let elements cssSelector = findMany cssSelector elementTimeout browser true

let unreliableElements cssSelector = findMany cssSelector elementTimeout browser false
    
let unreliableElement cssSelector = cssSelector |> unreliableElements |> elementFromList cssSelector

let element cssSelector = cssSelector |> elements |> elementFromList cssSelector

let elementWithin cssSelector (elem:IWebElement) =  find cssSelector elementTimeout elem true

let parent elem = elem |> elementWithin ".."

let elementsWithin cssSelector elem = findMany cssSelector elementTimeout elem true

let unreliableElementsWithin cssSelector elem = findMany cssSelector elementTimeout elem false

let someElement cssSelector = cssSelector |> unreliableElements |> someElementFromList cssSelector

let someElementWithin cssSelector elem = elem |> unreliableElementsWithin cssSelector |> someElementFromList cssSelector

let someParent elem = elem |> elementsWithin ".." |> someElementFromList "provided element"

let nth index cssSelector = List.nth (elements cssSelector) index

let first cssSelector = (elements cssSelector).Head

let last cssSelector = (List.rev (elements cssSelector)).Head
   
//read/write
let private writeToSelect (elem:IWebElement) (text:string) =
    let options =
        let optionsByText = unreliableElementsWithin (sprintf "option[text()='%s']" text) elem
        if writeToSelectWithOptionValue then            
            let optionsByValue = unreliableElementsWithin (sprintf "option[value='%s']" text) elem
            optionsByText @ optionsByValue
        else //to preserve previous behaviour
            optionsByText
    
    match options with
    | [] -> raise (CanopyOptionNotFoundException(sprintf "element %s does not contain value %s" (elem.ToString()) text))
    | head::tail -> head.Click()

let private writeToElement (e : IWebElement) (text:string) =
    if e.TagName = "select" then
        writeToSelect e text
    else
        let readonly = e.GetAttribute("readonly")
        if readonly = "true" then
            raise (CanopyReadOnlyException(sprintf "element %s is marked as read only, you can not write to read only elements" (e.ToString())))
        try e.Clear() with ex -> ex |> ignore
        e.SendKeys(text)

let ( << ) item text = 
    match box item with
    | :? IWebElement as elem ->  writeToElement elem text
    | :? string as cssSelector -> 
        wait elementTimeout (fun _ ->        
            elements cssSelector
                |> List.map (fun elem -> 
                    try  
                        writeToElement elem text
                        true
                    with
                        //Note: Enrich exception with proper cssSelector description
                        | :? CanopyReadOnlyException -> raise (CanopyReadOnlyException(sprintf "element %s is marked as read only, you can not write to read only elements" cssSelector))
                        | _ -> false)
                |> List.exists (fun elem -> elem = true)
        )
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't read %O because it is not a string or element" item))    
   


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
            | :? CanopyElementNotFoundException as ex -> raise (CanopyEqualityFailedException(sprintf "%s\r\nequality check failed.  expected: %s, got: %s" ex.Message value !bestvalue))
            | :? WebDriverTimeoutException -> raise (CanopyEqualityFailedException(sprintf "equality check failed.  expected: %s, got: %s" value !bestvalue))

    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check equality on %O because it is not a string or alert" item))

let ( != ) cssSelector value =
    try
        wait compareTimeout (fun _ -> (read cssSelector) <> value)
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyNotEqualsFailedException(sprintf "%s\r\nnot equals check failed.  expected NOT: %s, got: " ex.Message value))
        | :? WebDriverTimeoutException -> raise (CanopyNotEqualsFailedException(sprintf "not equals check failed.  expected NOT: %s, got: %s" value (read cssSelector)))
        
let ( *= ) cssSelector value =
    try        
        wait compareTimeout (fun _ -> ( cssSelector |> elements |> Seq.exists(fun element -> (textOf element) = value)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyValueNotInListException(sprintf "%s\r\ncan't find %s in list %s\r\ngot: " ex.Message value cssSelector))
        | :? WebDriverTimeoutException -> 
            let sb = new System.Text.StringBuilder()
            cssSelector |> elements |> List.iter (fun e -> bprintf sb "%s\r\n" (textOf e))
            raise (CanopyValueNotInListException(sprintf "can't find %s in list %s\r\ngot: %s" value cssSelector (sb.ToString())))

let ( *!= ) cssSelector value =
    try
        wait compareTimeout (fun _ -> ( cssSelector |> elements |> Seq.exists(fun element -> (textOf element) = value) |> not))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyValueInListException(sprintf "%s\r\nfound check failed" ex.Message))
        | :? WebDriverTimeoutException -> raise (CanopyValueInListException(sprintf "found %s in list %s, expected not to" value cssSelector))
    
let contains (value1 : string) (value2 : string) =
    if (value2.Contains(value1) <> true) then
        raise (CanopyContainsFailedException(sprintf "contains check failed.  %s does not contain %s" value2 value1))

let count cssSelector count =
    try        
        wait compareTimeout (fun _ -> (unreliableElements cssSelector).Length = count)
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyCountException(sprintf "%s\r\ncount failed. expected: %i got: %i" ex.Message count 0))
        | :? WebDriverTimeoutException -> raise (CanopyCountException(sprintf "count failed. expected: %i got: %i" count (unreliableElements cssSelector).Length))

let private regexMatch pattern input = System.Text.RegularExpressions.Regex.Match(input, pattern).Success

let elementsWithText cssSelector regex =
    unreliableElements cssSelector
    |> List.filter (fun elem -> regexMatch regex (textOf elem))

let elementWithText cssSelector regex = (elementsWithText cssSelector regex).Head

let ( =~ ) cssSelector pattern =
    try
        wait compareTimeout (fun _ -> regexMatch pattern (read cssSelector))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyEqualityFailedException(sprintf "%s\r\nregex equality check failed.  expected: %s, got:" ex.Message pattern))
        | :? WebDriverTimeoutException -> raise (CanopyEqualityFailedException(sprintf "regex equality check failed.  expected: %s, got: %s" pattern (read cssSelector)))

let ( *~ ) cssSelector pattern =
    try        
        wait compareTimeout (fun _ -> ( cssSelector |> elements |> Seq.exists(fun element -> regexMatch pattern (textOf element))))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyValueNotInListException(sprintf "%s\r\ncan't regex find %s in list %s\r\ngot: " ex.Message pattern cssSelector))
        | :? WebDriverTimeoutException -> 
            let sb = new System.Text.StringBuilder()
            cssSelector |> elements |> List.iter (fun e -> bprintf sb "%s\r\n" (textOf e))
            raise (CanopyValueNotInListException(sprintf "can't regex find %s in list %s\r\ngot: %s" pattern cssSelector (sb.ToString())))

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
        | :? CanopyElementNotFoundException as ex -> raise (CanopyDisplayedFailedException(sprintf "%s\r\ndisplay check for %O failed." ex.Message item))
        | :? WebDriverTimeoutException -> raise (CanopyDisplayedFailedException(sprintf "display check for %O failed." item))

let notDisplayed item =
    try
        wait compareTimeout (fun _ -> 
            match box item with
            | :? IWebElement as element -> not(shown element)
            | :? string as cssSelector -> (unreliableElements cssSelector |> List.isEmpty) || not(unreliableElement cssSelector |> shown)
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyNotDisplayedFailedException(sprintf "%s\r\nnotDisplay check for %O failed." ex.Message item))
        | :? WebDriverTimeoutException -> raise (CanopyNotDisplayedFailedException(sprintf "notDisplay check for %O failed." item))

let enabled item = 
    try
        wait compareTimeout (fun _ -> 
            match box item with
            | :? IWebElement as element -> element.Enabled = true
            | :? string as cssSelector -> (element cssSelector).Enabled = true
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyEnabledFailedException(sprintf "%s\r\nenabled check for %O failed." ex.Message item))
        | :? WebDriverTimeoutException -> raise (CanopyEnabledFailedException(sprintf "enabled check for %O failed." item))

let disabled item = 
    try
        wait compareTimeout (fun _ -> 
            match box item with
            | :? IWebElement as element -> element.Enabled = false
            | :? string as cssSelector -> (element cssSelector).Enabled = false
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyDisabledFailedException(sprintf "%s\r\ndisabled check for %O failed." ex.Message item))
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
        | :? CanopyElementNotFoundException as ex -> raise (CanopyCheckFailedException(sprintf "%s\r\nfailed to check %O." ex.Message item))
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
        | :? CanopyElementNotFoundException as ex -> raise (CanopyUncheckFailedException(sprintf "%s\r\nfailed to uncheck %O." ex.Message item))
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

let private chromeWithUserAgent userAgent =
    let options = Chrome.ChromeOptions()
    options.AddArgument("--user-agent=" + userAgent)
    new Chrome.ChromeDriver(chromeDir, options) :> IWebDriver

let private firefoxWithUserAgent (userAgent : string) = 
    let profile = FirefoxProfile()
    profile.SetPreference("general.useragent.override", userAgent)
    new FirefoxDriver(profile) :> IWebDriver

let start b =    
    //for chrome you need to download chromedriver.exe from http://code.google.com/p/chromedriver/wiki/GettingStarted
    //place chromedriver.exe in c:\ or you can place it in a customer location and change chromeDir value above
    //for ie you need to set Settings -> Advance -> Security Section -> Check-Allow active content to run files on My Computer*
    //also download IEDriverServer and place in c:\ or configure with ieDir
    //firefox just works
    //for phantomjs download it and put in c:\ or configure with phantomJSDir
       
    browser <-        
        match b with
        | IE -> new IE.InternetExplorerDriver(ieDir) :> IWebDriver
        | IEWithOptions options -> new IE.InternetExplorerDriver(ieDir, options) :> IWebDriver
        | IEWithOptionsAndTimeSpan(options, timeSpan) -> new IE.InternetExplorerDriver(ieDir, options, timeSpan) :> IWebDriver
        | Chrome -> new Chrome.ChromeDriver(chromeDir) :> IWebDriver
        | ChromeWithOptions options -> new Chrome.ChromeDriver(chromeDir, options) :> IWebDriver
        | ChromeWithUserAgent userAgent -> chromeWithUserAgent userAgent
        | ChromeWithOptionsAndTimeSpan(options, timeSpan) -> new Chrome.ChromeDriver(chromeDir, options, timeSpan) :> IWebDriver
        | Firefox -> new FirefoxDriver() :> IWebDriver
        | FirefoxWithProfile profile -> new FirefoxDriver(profile) :> IWebDriver
        | FirefoxWithPath path -> new FirefoxDriver(Firefox.FirefoxBinary(path), Firefox.FirefoxProfile()) :> IWebDriver
        | FirefoxWithUserAgent userAgent -> firefoxWithUserAgent userAgent
        | PhantomJS -> 
            autoPinBrowserRightOnLaunch <- false
            new PhantomJS.PhantomJSDriver(phantomJSDir) :> IWebDriver
        | PhantomJSProxyNone -> 
            autoPinBrowserRightOnLaunch <- false
            let service = PhantomJS.PhantomJSDriverService.CreateDefaultService(canopy.configuration.phantomJSDir)
            service.ProxyType <- "none"
            new PhantomJS.PhantomJSDriver(service) :> IWebDriver            

    if autoPinBrowserRightOnLaunch = true then pin Right
    browsers <- browsers @ [browser]

let switchTo b = browser <- b

let tile (browsers : IWebDriver list) =   
    let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height
    let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width
    let count = browsers.Length
    let maxWidth = w / count

    let rec setSize (browsers : IWebDriver list) c =
        match browsers with
        | [] -> ()
        | b :: tail -> 
            b.Manage().Window.Size <- new System.Drawing.Size(maxWidth,h)        
            b.Manage().Window.Position <- new System.Drawing.Point((maxWidth * c),0)
            setSize tail (c + 1)
    
    setSize browsers 0

let innerSize() =
    let jsBrowser = browser :?> IJavaScriptExecutor
    let innerWidth = System.Int32.Parse(jsBrowser.ExecuteScript("return window.innerWidth").ToString())
    let innerHeight = System.Int32.Parse(jsBrowser.ExecuteScript("return window.innerHeight").ToString())
    innerWidth, innerHeight

let resize size =
    let width,height = size
    let innerWidth, innerHeight = innerSize()
    let newWidth = browser.Manage().Window.Size.Width - innerWidth + width
    let newHeight = browser.Manage().Window.Size.Height - innerHeight + height
    browser.Manage().Window.Size <- System.Drawing.Size(newWidth, newHeight)   

let rotate() =
    let innerWidth, innerHeight = innerSize()
    resize(innerHeight, innerWidth)

let quit browser =
    reporter.quit()
    match box browser with
    | :? IWebDriver as b -> b.Quit()
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
    let p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"canopy\")
    let f = DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
    let ss = screenshot p f
    reporter.coverage nonMutableInnerUrl ss

let addFinder finder =
    let currentFinders = configuredFinders
    configuredFinders <- (fun cssSelector f ->
        currentFinders cssSelector f
        |> Seq.append (seq { yield finder cssSelector f }))