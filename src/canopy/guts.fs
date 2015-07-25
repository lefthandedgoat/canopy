module canopy.guts

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

let private __saveScreenshot directory filename pic =
    if not <| Directory.Exists(directory)
        then Directory.CreateDirectory(directory) |> ignore
    IO.File.WriteAllBytes(Path.Combine(directory,filename + ".png"), pic)

let private __takeScreenShotIfAlertUp () =
    let bitmap = new Bitmap(width= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, height= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, format=PixelFormat.Format32bppArgb);
    use graphics = Graphics.FromImage(bitmap)
    graphics.CopyFromScreen(System.Windows.Forms.Screen.PrimaryScreen.Bounds.X, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y, 0, 0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
    use stream = new MemoryStream()
    bitmap.Save(stream, ImageFormat.Png)
    stream.Close()
    stream.ToArray()

let private __takeScreenshot (browser : IWebDriver) directory filename =
    try
        let pic = (browser :?> ITakesScreenshot).GetScreenshot().AsByteArray
        __saveScreenshot directory filename pic
        pic
    with
        | :? UnhandledAlertException as ex->
            let pic = __takeScreenShotIfAlertUp()
            __saveScreenshot directory filename pic
            let alert = browser.SwitchTo().Alert()
            alert.Accept()
            pic

let __screenshot (browser : IWebDriver) directory filename =
    match box browser with
        | :? ITakesScreenshot -> __takeScreenshot browser directory filename
        | _ -> Array.empty<byte>

let __js (browser : IWebDriver) script = (browser :?> IJavaScriptExecutor).ExecuteScript(script)

let private __swallowedJs (browser : IWebDriver) script = try __js browser script |> ignore with | ex -> ()

//dead
//let sleep seconds =

let __puts (browser : IWebDriver) text =
    reporter.write text
    let escapedText = System.Web.HttpUtility.JavaScriptStringEncode(text)
    let info = "
        var infoDiv = document.getElementById('canopy_info_div');
        if(!infoDiv) { infoDiv = document.createElement('div'); }
        infoDiv.id = 'canopy_info_div';
        infoDiv.setAttribute('style','position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;');
        document.getElementsByTagName('body')[0].appendChild(infoDiv);
        infoDiv.innerHTML = 'locating: " + escapedText + "';"
    __swallowedJs browser info

let private __wait (browser : IWebDriver) timeout f =
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

let private __colorizeAndSleep browser cssSelector =
    __puts browser cssSelector
    __swallowedJs browser <| sprintf "document.querySelector('%s').style.border = 'thick solid #FFF467';" cssSelector
    __swallowedJs browser <| sprintf "document.querySelector('%s').style.border = 'thick solid #ACD372';" cssSelector

let __highlight browser cssSelector =
    __swallowedJs browser <| sprintf "document.querySelector('%s').style.border = 'thick solid #ACD372';" cssSelector

let __suggestOtherSelectors browser cssSelector =
    if not disableSuggestOtherSelectors then
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
        let classes = __js browser classesViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> "." + item.ToString()) |> Array.ofSeq
        let ids = __js browser idsViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> "#" + item.ToString()) |> Array.ofSeq
        let values = __js browser valuesViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> item.ToString()) |> Array.ofSeq
        let texts = __js browser textsViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> item.ToString()) |> Array.ofSeq

        let results =
            Array.append classes ids
            |> Array.append values
            |> Array.append texts
            |> Seq.distinct |> List.ofSeq
            |> remove "." |> remove "#" |> Array.ofList
            |> Array.Parallel.map (fun u -> levenshtein cssSelector u)
            |> Array.sortBy (fun r -> r.distance)

        if results.Length >= 5 then
            results
            |> Seq.take 5
            |> Seq.map (fun r -> r.selector) |> List.ofSeq
            |> (fun suggestions -> reporter.suggestSelectors cssSelector suggestions)
        else
            results
            |> Seq.map (fun r -> r.selector) |> List.ofSeq
            |> (fun suggestions -> reporter.suggestSelectors cssSelector suggestions)

let __describe browser text =
    __puts browser text

let __waitFor2 browser message (f : unit -> bool) =
    try
        __wait browser compareTimeout f
    with
        | :? WebDriverTimeoutException ->
                raise (CanopyWaitForException(sprintf "%s%swaitFor condition failed to become true in %.1f seconds" message System.Environment.NewLine compareTimeout))

let __waitFor browser = __waitFor2 browser "Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout"

//find related
let rec private __findElements (browser : IWebDriver) cssSelector (searchContext : ISearchContext) : IWebElement list =
    let findInIFrame () =
        let iframes = findByCss "iframe" searchContext.FindElements
        if iframes.IsEmpty then
            browser.SwitchTo().DefaultContent() |> ignore
            []
        else
            let webElements = ref []
            iframes |> List.iter (fun frame ->
                browser.SwitchTo().Frame(frame) |> ignore
                let root = browser.FindElement(By.CssSelector("html"))
                webElements := __findElements browser cssSelector root
            )
            !webElements

    try
        let results =
            if (hints.ContainsKey cssSelector) then
                let finders = hints.[cssSelector]
                finders
                |> Seq.map (fun finder -> finder cssSelector searchContext.FindElements)
                |> Seq.filter(fun list -> not(list.IsEmpty))
            else
                configuredFinders cssSelector searchContext.FindElements
                |> Seq.filter(fun list -> not(list.IsEmpty))
        if Seq.isEmpty results then
            if optimizeBySkippingIFrameCheck then [] else findInIFrame()
        else
           results |> Seq.head
    with | ex -> []

let private __findByFunction (browser : IWebDriver) cssSelector timeout waitFunc searchContext reliable =
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(elementTimeout))
    try
        if reliable then
            let elements = ref []
            wait.Until(fun _ ->
                elements := waitFunc browser cssSelector searchContext
                not <| List.isEmpty !elements) |> ignore
            !elements
        else
            wait.Until(fun _ -> waitFunc browser cssSelector searchContext)
    with
        | :? WebDriverTimeoutException ->
            __suggestOtherSelectors browser cssSelector
            raise (CanopyElementNotFoundException(sprintf "can't find element %s" cssSelector))

let private __find browser cssSelector timeout searchContext reliable =
    (__findByFunction browser cssSelector timeout __findElements searchContext reliable).Head

let private __findMany browser cssSelector timeout searchContext reliable =
    __findByFunction browser cssSelector timeout __findElements searchContext reliable

//get elements

let private __elementFromList cssSelector elementsList =
    match elementsList with
    | [] -> null
    | x :: [] -> x
    | x :: y :: _ ->
        if throwIfMoreThanOneElement then raise (CanopyMoreThanOneElementFoundException(sprintf "More than one element was selected when only one was expected for selector: %s" cssSelector))
        else x

let private __someElementFromList cssSelector elementsList =
    match elementsList with
    | [] -> None
    | x :: [] -> Some(x)
    | x :: y :: _ ->
        if throwIfMoreThanOneElement then raise (CanopyMoreThanOneElementFoundException(sprintf "More than one element was selected when only one was expected for selector: %s" cssSelector))
        else Some(x)

let __elements browser cssSelector = __findMany browser cssSelector elementTimeout browser true

let __unreliableElements browser cssSelector = __findMany browser cssSelector elementTimeout browser false

let __unreliableElement browser cssSelector = cssSelector |> __unreliableElements browser |> __elementFromList cssSelector

let __element browser cssSelector = cssSelector |> __elements browser |> __elementFromList cssSelector

let __elementWithin browser cssSelector (elem:IWebElement) =  __find browser cssSelector elementTimeout elem true

let __parent browser elem = elem |> __elementWithin browser ".."

let __elementsWithin browser cssSelector elem = __findMany browser cssSelector elementTimeout elem true

let __unreliableElementsWithin browser cssSelector elem = __findMany browser cssSelector elementTimeout elem false

let __someElement browser cssSelector = cssSelector |> __unreliableElements browser |> __someElementFromList cssSelector

let __someElementWithin browser cssSelector elem = elem |> __unreliableElementsWithin browser cssSelector |> __someElementFromList cssSelector

let __someParent browser elem = elem |> __elementsWithin browser ".." |> __someElementFromList "provided element"

let __nth browser index cssSelector = List.nth (__elements browser cssSelector) index

let __first browser cssSelector = (__elements browser cssSelector).Head

let __last browser cssSelector = (List.rev (__elements browser cssSelector)).Head

//read/write
let private __writeToSelect browser (elem:IWebElement) (text:string) =
    let options =
        if writeToSelectWithOptionValue then
            __unreliableElementsWithin browser (sprintf """option[text()="%s"] | option[@value="%s"]""" text text) elem
        else //to preserve previous behaviour
            __unreliableElementsWithin browser (sprintf """option[text()="%s"]""" text) elem

    match options with
    | [] -> raise (CanopyOptionNotFoundException(sprintf "element %s does not contain value %s" (elem.ToString()) text))
    | head::_ -> head.Click()

let private __writeToElement browser (e : IWebElement) (text:string) =
    if e.TagName = "select" then
        __writeToSelect browser e text
    else
        let readonly = e.GetAttribute("readonly")
        if readonly = "true" then
            raise (CanopyReadOnlyException(sprintf "element %s is marked as read only, you can not write to read only elements" (e.ToString())))
        try e.Clear() with ex -> ex |> ignore
        e.SendKeys(text)

let __write browser item text =
    match box item with
    | :? IWebElement as elem -> __writeToElement browser elem text
    | :? string as cssSelector ->
        __wait browser elementTimeout (fun _ ->
            __elements browser cssSelector
                |> List.map (fun elem ->
                    try
                        __writeToElement browser elem text
                        true
                    with
                        //Note: Enrich exception with proper cssSelector description
                        | :? CanopyReadOnlyException -> raise (CanopyReadOnlyException(sprintf "element %s is marked as read only, you can not write to read only elements" cssSelector))
                        | _ -> false)
                |> List.exists (fun elem -> elem = true)
        )
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't read %O because it is not a string or element" item))

let private __textOf (element : IWebElement) =
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

let __read browser item =
    match box item with
    | :? IWebElement as elem -> __textOf elem
    | :? string as cssSelector -> __element browser cssSelector |> __textOf
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't read %O because it is not a string or element" item))

let __clear browser item =
    let clear cssSelector (elem : IWebElement) =
        let readonly = elem.GetAttribute("readonly")
        if readonly = "true" then raise (CanopyReadOnlyException(sprintf "element %s is marked as read only, you can not clear read only elements" cssSelector))
        elem.Clear()

    match box item with
    | :? IWebElement as elem -> clear elem.TagName elem
    | :? string as cssSelector -> __element browser cssSelector |> clear cssSelector
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't clear %O because it is not a string or element" item))

//status
let __selected browser item =
    let selected cssSelector (elem : IWebElement) =
        if not <| elem.Selected then raise (CanopySelectionFailedExeception(sprintf "element selected failed, %s not selected." cssSelector))

    match box item with
    | :? IWebElement as elem -> selected elem.TagName elem
    | :? string as cssSelector -> __element browser cssSelector |> selected cssSelector
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check selected on %O because it is not a string or element" item))

let __deselected browser item =
    let deselected cssSelector (elem : IWebElement) =
        if elem.Selected then raise (CanopyDeselectionFailedException(sprintf "element deselected failed, %s selected." cssSelector))

    match box item with
    | :? IWebElement as elem -> deselected elem.TagName elem
    | :? string as cssSelector -> __element browser cssSelector |> deselected cssSelector
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check deselected on %O because it is not a string or element" item))

let __press browser key =
    let elem = ((__js browser "return document.activeElement;") :?> IWebElement)
    elem.SendKeys(key)

//alerts
let __alert browser =
    __waitFor browser (fun _ ->
        browser.SwitchTo().Alert() |> ignore
        true)
    browser.SwitchTo().Alert()

let __acceptAlert browser =
    __wait browser compareTimeout (fun _ ->
        browser.SwitchTo().Alert().Accept()
        true)

let __dismissAlert browser =
    __wait browser compareTimeout (fun _ ->
        browser.SwitchTo().Alert().Dismiss()
        true)

//assertions
let __equals browser item value =
    match box item with
    | :? IAlert as alert ->
        let text = alert.Text
        if text <> value then
            alert.Dismiss()
            raise (CanopyEqualityFailedException(sprintf "equality check failed.  expected: %s, got: %s" value text))
    | :? string as cssSelector ->
        let bestvalue = ref ""
        try
            __wait browser compareTimeout (fun _ -> ( let readvalue = (__read browser cssSelector)
                                                      if readvalue <> value && readvalue <> "" then
                                                          bestvalue := readvalue
                                                          false
                                                      else
                                                          readvalue = value))
        with
            | :? CanopyElementNotFoundException as ex -> raise (CanopyEqualityFailedException(sprintf "%s%sequality check failed.  expected: %s, got: %s" ex.Message Environment.NewLine value !bestvalue))
            | :? WebDriverTimeoutException -> raise (CanopyEqualityFailedException(sprintf "equality check failed.  expected: %s, got: %s" value !bestvalue))

    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check equality on %O because it is not a string or alert" item))

let __notEqual browser cssSelector value =
    try
        __wait browser compareTimeout (fun _ -> (__read browser cssSelector) <> value)
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyNotEqualsFailedException(sprintf "%s%snot equals check failed.  expected NOT: %s, got: " ex.Message Environment.NewLine value))
        | :? WebDriverTimeoutException -> raise (CanopyNotEqualsFailedException(sprintf "not equals check failed.  expected NOT: %s, got: %s" value (__read browser cssSelector)))

let __oneOfManyEqual browser cssSelector value =
    try
        __wait browser compareTimeout (fun _ -> ( cssSelector |> __elements browser |> Seq.exists(fun element -> (__textOf element) = value)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyValueNotInListException(sprintf "%s%scan't find %s in list %s%sgot: " ex.Message Environment.NewLine value cssSelector Environment.NewLine))
        | :? WebDriverTimeoutException ->
            let sb = new System.Text.StringBuilder()
            cssSelector |> __elements browser |> List.iter (fun e -> bprintf sb "%s%s" (__textOf e) Environment.NewLine)
            raise (CanopyValueNotInListException(sprintf "can't find %s in list %s%sgot: %s" value cssSelector Environment.NewLine (sb.ToString())))

let __oneOfManyNotEqual browser cssSelector value =
    try
        __wait browser compareTimeout (fun _ -> ( cssSelector |> __elements browser |> Seq.exists(fun element -> (__textOf element) = value) |> not))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyValueInListException(sprintf "%s%sfound check failed" ex.Message Environment.NewLine))
        | :? WebDriverTimeoutException -> raise (CanopyValueInListException(sprintf "found %s in list %s, expected not to" value cssSelector))

let __contains (value1 : string) (value2 : string) =
    if (value2.Contains(value1) <> true) then
        raise (CanopyContainsFailedException(sprintf "contains check failed.  %s does not contain %s" value2 value1))

let __count browser cssSelector count =
    try
        __wait browser compareTimeout (fun _ -> (__unreliableElements browser cssSelector).Length = count)
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyCountException(sprintf "%s%scount failed. expected: %i got: %i" ex.Message Environment.NewLine count 0))
        | :? WebDriverTimeoutException -> raise (CanopyCountException(sprintf "count failed. expected: %i got: %i" count (__unreliableElements browser cssSelector).Length))

let private __regexMatch pattern input = System.Text.RegularExpressions.Regex.Match(input, pattern).Success

let __elementsWithText browser cssSelector regex =
    __unreliableElements browser cssSelector
    |> List.filter (fun elem -> __regexMatch regex (__textOf elem))

let __elementWithText browser cssSelector regex = (__elementsWithText browser cssSelector regex).Head

let __regexEqual browser cssSelector pattern =
    try
        __wait browser compareTimeout (fun _ -> __regexMatch pattern (__read browser cssSelector))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyEqualityFailedException(sprintf "%s%sregex equality check failed.  expected: %s, got:" ex.Message Environment.NewLine pattern))
        | :? WebDriverTimeoutException -> raise (CanopyEqualityFailedException(sprintf "regex equality check failed.  expected: %s, got: %s" pattern (__read browser cssSelector)))

let __regexOneOfManyEqual browser cssSelector pattern =
    try
        __wait browser compareTimeout (fun _ -> ( cssSelector |> __elements browser |> Seq.exists(fun element -> __regexMatch pattern (__textOf element))))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyValueNotInListException(sprintf "%s%scan't regex find %s in list %s%sgot: " ex.Message Environment.NewLine pattern cssSelector Environment.NewLine))
        | :? WebDriverTimeoutException ->
            let sb = new System.Text.StringBuilder()
            cssSelector |> __elements browser |> List.iter (fun e -> bprintf sb "%s%s" (__textOf e) Environment.NewLine)
            raise (CanopyValueNotInListException(sprintf "can't regex find %s in list %s%sgot: %s" pattern cssSelector Environment.NewLine (sb.ToString())))

let __is expected actual =
    if expected <> actual then
        raise (CanopyEqualityFailedException(sprintf "equality check failed.  expected: %O, got: %O" expected actual))

let (===) expected actual = __is expected actual

let private __shown (elem : IWebElement) =
    let opacity = elem.GetCssValue("opacity")
    let display = elem.GetCssValue("display")
    display <> "none" && opacity = "1"

let __displayed browser item =
    try
        __wait browser compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element ->  __shown element
            | :? string as cssSelector -> __element browser cssSelector |> __shown
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyDisplayedFailedException(sprintf "%s%sdisplay check for %O failed." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyDisplayedFailedException(sprintf "display check for %O failed." item))

let __notDisplayed browser item =
    try
        __wait browser compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element -> not <| __shown element
            | :? string as cssSelector -> (__unreliableElements browser cssSelector |> List.isEmpty) || not(__unreliableElement browser cssSelector |> __shown)
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyNotDisplayedFailedException(sprintf "%s%snotDisplay check for %O failed." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyNotDisplayedFailedException(sprintf "notDisplay check for %O failed." item))

let __enabled browser item =
    try
        __wait browser compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element -> element.Enabled = true
            | :? string as cssSelector -> (__element browser cssSelector).Enabled = true
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyEnabledFailedException(sprintf "%s%senabled check for %O failed." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyEnabledFailedException(sprintf "enabled check for %O failed." item))

let disabled browser item =
    try
        __wait browser compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element -> element.Enabled = false
            | :? string as cssSelector -> (__element browser cssSelector).Enabled = false
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyDisabledFailedException(sprintf "%s%sdisabled check for %O failed." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyDisabledFailedException(sprintf "disabled check for %O failed." item))

let __fadedIn browser cssSelector = fun _ -> __element browser cssSelector |> __shown

//clicking/checking
let __click browser item =
    match box item with
    | :? IWebElement as element -> element.Click()
    | :? string as cssSelector ->
        __wait browser elementTimeout (fun _ ->
            let atleastOneItemClicked = ref false
            __elements browser cssSelector
            |> List.iter (fun elem ->
                try
                    elem.Click()
                    atleastOneItemClicked := true
                with | ex -> ())
            !atleastOneItemClicked)
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't click %O because it is not a string or webelement" item))

let __doubleClick (browser : IWebDriver) item =
    let js = "var evt = document.createEvent('MouseEvents'); evt.initMouseEvent('dblclick',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0,null); arguments[0].dispatchEvent(evt);"

    match box item with
    | :? IWebElement as elem -> (browser :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
    | :? string as cssSelector ->
        __wait browser elementTimeout (fun _ -> ( let elem = __element browser cssSelector
                                                  (browser :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
                                                  true))
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't doubleClick %O because it is not a string or webelement" item))

let __check browser item =
    try
        match box item with
        | :? IWebElement as elem -> if not <| elem.Selected then __click browser elem
        | :? string as cssSelector ->
            __waitFor browser (fun _ ->
                                        if not <| (__element browser cssSelector).Selected then __click browser cssSelector
                                        (__element browser cssSelector).Selected)
        | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't read %O because it is not a string or element" item))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyCheckFailedException(sprintf "%s%sfailed to check %O." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyCheckFailedException(sprintf "failed to check %O." item))

let __uncheck browser item =
    try
        match box item with
        | :? IWebElement as elem -> if elem.Selected then __click browser elem
        | :? string as cssSelector ->
            __waitFor browser (fun _ ->
                if (__element browser cssSelector).Selected then __click browser cssSelector
                (__element browser cssSelector).Selected = false)
        | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't read %O because it is not a string or element" item))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyUncheckFailedException(sprintf "%s%sfailed to uncheck %O." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyUncheckFailedException(sprintf "failed to uncheck %O." item))

//hoverin
let __hover browser selector =
    let actions = Actions(browser)
    let e = __element browser selector
    actions.MoveToElement(e).Perform()

//draggin
let __drag browser cssSelectorA cssSelectorB =
    __wait browser elementTimeout (fun _ ->
        let a = __element browser cssSelectorA
        let b = __element browser cssSelectorB
        (new Actions(browser)).DragAndDrop(a, b).Perform()
        true)

//browser related
let __pin (browser : IWebDriver) direction =
    let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height
    let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width
    let maxWidth = w / 2
    browser.Manage().Window.Size <- new System.Drawing.Size(maxWidth,h)
    match direction with
    | Left -> browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 0),0)
    | Right -> browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 1),0)
    | FullScreen -> browser.Manage().Window.Maximize()

let __pinToMonitor (browser : IWebDriver) n =
    let n' = if n < 1 then 1 else n
    if System.Windows.Forms.SystemInformation.MonitorCount >= n' then
        let workingArea = System.Windows.Forms.Screen.AllScreens.[n'-1].WorkingArea
        browser.Manage().Window.Position <- new System.Drawing.Point(workingArea.X, 0)
        browser.Manage().Window.Maximize()
    else
        raise(CanopyException(sprintf "Monitor %d is not detected" n))

let private __chromeWithUserAgent userAgent =
    let options = Chrome.ChromeOptions()
    options.AddArgument("--user-agent=" + userAgent)
    new Chrome.ChromeDriver(chromeDir, options) :> IWebDriver

let private __firefoxWithUserAgent (userAgent : string) =
    let profile = FirefoxProfile()
    profile.SetPreference("general.useragent.override", userAgent)
    new FirefoxDriver(profile) :> IWebDriver

let __start b =
    //for chrome you need to download chromedriver.exe from http://code.google.com/p/chromedriver/wiki/GettingStarted
    //place chromedriver.exe in c:\ or you can place it in a customer location and change chromeDir value above
    //for ie you need to set Settings -> Advance -> Security Section -> Check-Allow active content to run files on My Computer*
    //also download IEDriverServer and place in c:\ or configure with ieDir
    //firefox just works
    //for phantomjs download it and put in c:\ or configure with phantomJSDir

    let browser =
        match b with
        | IE -> new IE.InternetExplorerDriver(ieDir) :> IWebDriver
        | IEWithOptions options -> new IE.InternetExplorerDriver(ieDir, options) :> IWebDriver
        | IEWithOptionsAndTimeSpan(options, timeSpan) -> new IE.InternetExplorerDriver(ieDir, options, timeSpan) :> IWebDriver
        | Chrome ->
                let options = OpenQA.Selenium.Chrome.ChromeOptions()
                options.AddArgument("test-type") //https://code.google.com/p/chromedriver/issues/detail?id=799
                new Chrome.ChromeDriver(chromeDir, options) :> IWebDriver
        | ChromeWithOptions options -> new Chrome.ChromeDriver(chromeDir, options) :> IWebDriver
        | ChromeWithUserAgent userAgent -> __chromeWithUserAgent userAgent
        | ChromeWithOptionsAndTimeSpan(options, timeSpan) -> new Chrome.ChromeDriver(chromeDir, options, timeSpan) :> IWebDriver
        | Firefox -> new FirefoxDriver() :> IWebDriver
        | FirefoxWithProfile profile -> new FirefoxDriver(profile) :> IWebDriver
        | FirefoxWithPath path -> new FirefoxDriver(Firefox.FirefoxBinary(path), Firefox.FirefoxProfile()) :> IWebDriver
        | FirefoxWithUserAgent userAgent -> __firefoxWithUserAgent userAgent
        | PhantomJS ->
            autoPinBrowserRightOnLaunch <- false
            new PhantomJS.PhantomJSDriver(phantomJSDir) :> IWebDriver
        | PhantomJSProxyNone ->
            autoPinBrowserRightOnLaunch <- false
            let service = PhantomJS.PhantomJSDriverService.CreateDefaultService(canopy.configuration.phantomJSDir)
            service.ProxyType <- "none"
            new PhantomJS.PhantomJSDriver(service) :> IWebDriver
        | Remote(url, capabilities) -> new OpenQA.Selenium.Remote.RemoteWebDriver(new Uri(url), capabilities) :> IWebDriver

    if autoPinBrowserRightOnLaunch = true then __pin browser Right
    browser

let __switchToTab (browser: IWebDriver) number =
    let tabs = browser.WindowHandles;
    browser.SwitchTo().Window(tabs.[(number - 1)]) |> ignore

let __closeTab (browser: IWebDriver) number =
    __switchToTab browser number
    browser.Close()

let __tile (browsers : IWebDriver list) =
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

let __innerSize (browser: IWebDriver) =
    let jsBrowser = browser :?> IJavaScriptExecutor
    let innerWidth = System.Int32.Parse(jsBrowser.ExecuteScript("return window.innerWidth").ToString())
    let innerHeight = System.Int32.Parse(jsBrowser.ExecuteScript("return window.innerHeight").ToString())
    innerWidth, innerHeight

let __resize browser size =
    let width,height = size
    let innerWidth, innerHeight = __innerSize browser
    let newWidth = browser.Manage().Window.Size.Width - innerWidth + width
    let newHeight = browser.Manage().Window.Size.Height - innerHeight + height
    browser.Manage().Window.Size <- System.Drawing.Size(newWidth, newHeight)

let __rotate browser =
    let innerWidth, innerHeight = __innerSize browser
    __resize browser (innerHeight, innerWidth)

let __quit (browser : IWebDriver) (browsers : IWebDriver list) =
    reporter.quit()
    match box browser with
    | :? IWebDriver as b -> b.Quit()
    | _ -> browsers |> List.iter (fun b -> b.Quit())

let currentUrl (browser : IWebDriver) = browser.Url

let on browser (u: string) =
    let urlPath (u : string) =
        let url = match u with
                  | x when x.StartsWith("http") -> u  //leave absolute urls alone
                  | _ -> "http://host/" + u.Trim('/') //ensure valid uri
        let uriBuilder = new System.UriBuilder(url)
        uriBuilder.Path.TrimEnd('/') //get the path part removing trailing slashes
    try
        __wait browser pageTimeout (fun _ -> if browser.Url = u then true else urlPath(browser.Url) = urlPath(u))
    with
        | ex -> if browser.Url.Contains(u) = false then raise (CanopyOnException(sprintf "on check failed, expected expression '%s' got %s" u browser.Url))

let url (browser : IWebDriver) (u : string) = browser.Navigate().GoToUrl(u)

let title (browser : IWebDriver) = browser.Title

let reload (browser : IWebDriver) = currentUrl browser |> url browser

type Navigate =
  | Back
  | Forward

let back = Back
let forward = Forward

let navigate (browser : IWebDriver) = function
  | Back -> browser.Navigate().Back()
  | Forward -> browser.Navigate().Forward()

//let coverage (url : 'a) =
//    let mutable innerUrl = ""
//    match box url with
//    | :? string as u -> innerUrl <- u
//    | _ -> innerUrl <- currentUrl()
//    let nonMutableInnerUrl = innerUrl
//
//    let selectors =
//        searchedFor
//        |> List.filter(fun (c, u) -> u = nonMutableInnerUrl)
//        |> List.map(fun (cssSelector, u) -> cssSelector)
//        |> Seq.distinct
//        |> List.ofSeq
//
//    let script cssSelector =
//        "var results = document.querySelectorAll('" + cssSelector + "'); \
//        for (var i=0; i < results.length; i++){ \
//            results[i].style.border = 'thick solid #ACD372'; \
//        }"
//
//    //kinda silly but the app I am current working on will redirect you to login if you try to access a url directly, so dont try if one isnt passed in
//    match box url with
//    | :? string as u -> !^ nonMutableInnerUrl
//    |_ -> ()
//
//    on nonMutableInnerUrl
//    selectors |> List.iter(fun cssSelector -> swallowedJs (script cssSelector))
//    let p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"canopy\")
//    let f = DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
//    let ss = screenshot p f
//    reporter.coverage nonMutableInnerUrl ss

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
