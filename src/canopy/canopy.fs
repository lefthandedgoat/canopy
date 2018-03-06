[<AutoOpen>]
module Canopy.Core

open System.Collections.ObjectModel
open OpenQA.Selenium.Firefox
open OpenQA.Selenium
open OpenQA.Selenium.Interactions
open Microsoft.FSharp.Core.Printf
open System.IO
open System
open Configuration
open Reporters
open Types
open Finders
open System.Drawing
open System.Drawing.Imaging
open EditDistance

// TODO: remove global mutable
let mutable (failureMessage : string) = null

// TODO: remove global mutable
let mutable wipTest = false

(* documented/actions *)
let firefox = Firefox

(* documented/actions *)
let aurora = FirefoxWithPath(@"C:\Program Files (x86)\Aurora\firefox.exe")

(* documented/actions *)
let ie = IE

(* documented/actions *)
let edgeBETA = EdgeBETA

(* documented/actions *)
let chrome = Chrome

(* documented/actions *)
let chromium = Chromium

(* documented/actions *)
let safari = Safari

// TODO: remove global mutable
let mutable browsers = []

//misc
(* documented/actions *)
let failsWith message =
    // TODO: handle write to global via `Configuration`
    failureMessage <- message

let internal textOf (element: IWebElement) =
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

let internal regexMatch pattern input =
    System.Text.RegularExpressions.Regex.Match(input, pattern).Success

let internal saveScreenshot directory filename pic =
    if not <| Directory.Exists(directory)
        then Directory.CreateDirectory(directory) |> ignore
    IO.File.WriteAllBytes(Path.Combine(directory,filename + ".jpg"), pic)

let internal takeScreenShotIfAlertUp () =
    try
        let screenBounds = Screen.getPrimaryScreenBounds ()
        let bitmap = new Bitmap(width=screenBounds.width, height=screenBounds.height, format=PixelFormat.Format32bppArgb);
        use graphics = Graphics.FromImage(bitmap)
        graphics.CopyFromScreen(screenBounds.x, screenBounds.y, 0, 0, screenBounds.size, CopyPixelOperation.SourceCopy);
        use stream = new MemoryStream()
        bitmap.Save(stream, ImageFormat.Png)
        stream.ToArray()
    with ex ->
        printfn "Sorry, unable to take a screenshot. An alert was up, and the backup plan failed!
        Exception: %O" ex
        Array.empty<byte>

let internal takeScreenshotB (browser: IWebDriver) directory filename =
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

let internal takeScreenshot directory filename =
    takeScreenshotB browser directory filename

let internal pngToJpg pngArray =
  let pngStream = new MemoryStream()
  let jpgStream = new MemoryStream()

  pngStream.Write(pngArray, 0, pngArray.Length)
  let img = Image.FromStream(pngStream)

  img.Save(jpgStream, ImageFormat.Jpeg)
  jpgStream.ToArray()

let screenshotB (browser: IWebDriver) directory filename =
    match box browser with
    | :? ITakesScreenshot -> takeScreenshot directory filename |> pngToJpg
    | _ -> Array.empty<byte>

(* documented/actions *)
let screenshot directory filename =
    screenshotB browser directory filename

let jsB (browser: IWebDriver) script =
    (browser :?> IJavaScriptExecutor).ExecuteScript(script)

(* documented/actions *)
let js script =
    jsB browser script

let internal swallowedJs script =
    try js script |> ignore with | ex -> ()

(* documented/actions *)
let sleep seconds =
    let ms =
        match box seconds with
        | :? int as i ->
            i * 1000
        | :? float as i ->
            Convert.ToInt32(i * 1000.0)
        | :? TimeSpan as ts ->
           int (ts.TotalSeconds * 1000.0)
        | _ ->
            1000
    // TO CONSIDER: async?
    System.Threading.Thread.Sleep(ms)

(* documented/actions *)
let puts text =
    reporter.write text
    if showInfoDiv then
        let escapedText = System.Web.HttpUtility.JavaScriptStringEncode(text)
        let info = "
            var infoDiv = document.getElementById('canopy_info_div');
            if(!infoDiv) { infoDiv = document.createElement('div'); }
            infoDiv.id = 'canopy_info_div';
            infoDiv.setAttribute('style','position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;');
            document.getElementsByTagName('body')[0].appendChild(infoDiv);
            infoDiv.innerHTML = 'locating: " + escapedText + "';"
        swallowedJs info

let internal colorizeAndSleep cssSelector =
    puts cssSelector
    swallowedJs <| sprintf "document.querySelector('%s').style.border = 'thick solid #FFF467';" cssSelector
    sleep wipSleep
    swallowedJs <| sprintf "document.querySelector('%s').style.border = 'thick solid #ACD372';" cssSelector

(* documented/actions *)
let highlight cssSelector =
    swallowedJs <| sprintf "document.querySelector('%s').style.border = 'thick solid #ACD372';" cssSelector

let internal suggestOtherSelectors cssSelector =
    if not disableSuggestOtherSelectors then
        let classesViaJs = """
            var classes = [];
            var all = document.getElementsByTagName('*');
            for (var i=0, max=all.length; i < max; i++) {
	            var ary = all[i].className.toString().split(' ');
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
        let safeSeq orig = if orig = null then Seq.empty else orig
        let safeToString orig = if orig = null then "" else orig.ToString()
        let classes = js classesViaJs :?> ReadOnlyCollection<System.Object> |> safeSeq |> Seq.map (fun item -> "." + safeToString item) |> Array.ofSeq
        let ids = js idsViaJs :?> ReadOnlyCollection<System.Object> |> safeSeq |> Seq.map (fun item -> "#" + safeToString item) |> Array.ofSeq
        let values = js valuesViaJs :?> ReadOnlyCollection<System.Object> |> safeSeq |> Seq.map (fun item -> safeToString item) |> Array.ofSeq
        let texts = js textsViaJs :?> ReadOnlyCollection<System.Object> |> safeSeq |> Seq.map (fun item -> safeToString item) |> Array.ofSeq

        let results =
            Array.append classes ids
            |> Array.append values
            |> Array.append texts
            |> Seq.distinct |> List.ofSeq
            |> remove "." |> remove "#" |> Array.ofList
            |> Array.Parallel.map (fun u -> editdistance cssSelector u)
            |> Array.sortBy (fun r -> - r.similarity)

        results
        |> fun xs -> if xs.Length >= 5 then Seq.take 5 xs else Array.toSeq xs
        |> Seq.map (fun r -> r.selector) |> List.ofSeq
        |> (fun suggestions -> reporter.suggestSelectors cssSelector suggestions)

(* documented/actions *)
let describe text =
    puts text

(* documented/actions *)
let waitFor2 message f =
    try
        wait compareTimeout f
    with
    | :? WebDriverTimeoutException ->
        let message = sprintf "%s%swaitFor condition failed to become true in %.1f seconds" message System.Environment.NewLine compareTimeout
        raise (CanopyWaitForException message)

(* documented/actions *)
let waitFor = waitFor2 "Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout"

/// Find related
let rec internal findElementsB (browser: IWebDriver) cssSelector (searchContext: ISearchContext): IWebElement list =
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
                webElements := findElementsB browser cssSelector root
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


/// Find related
let rec internal findElements cssSelector searchContext: IWebElement list =
    findElementsB browser cssSelector searchContext

let internal findByFunctionB (browser: IWebDriver) cssSelector timeout waitFunc searchContext reliable =
    if browser = null then raise (CanopyNoBrowserException("Can't perform the action because the browser instance is null.  `start chrome` to start a new browser."))
    if wipTest then colorizeAndSleep cssSelector

    try
        if reliable then
            let elements = ref []
            wait elementTimeout (fun _ ->
                elements := waitFunc cssSelector searchContext
                not <| List.isEmpty !elements)
            !elements
        else
            waitResults elementTimeout (fun _ -> waitFunc cssSelector searchContext)
    with
        | :? WebDriverTimeoutException ->
            suggestOtherSelectors cssSelector
            raise (CanopyElementNotFoundException(sprintf "can't find element %s" cssSelector))

let internal findByFunction cssSelector timeout waitFunc searchContext reliable =
    findByFunctionB browser cssSelector timeout waitFunc searchContext reliable

let internal findB browser cssSelector timeout searchContext reliable =
    let finder = findElementsB browser
    findByFunctionB browser cssSelector timeout finder searchContext reliable
    |> List.head

let internal find cssSelector timeout searchContext reliable =
    findB browser cssSelector timeout searchContext reliable

let internal findManyB browser cssSelector timeout searchContext reliable =
    let finder = findElementsB browser
    findByFunctionB browser cssSelector timeout finder searchContext reliable

let internal findMany cssSelector timeout searchContext reliable =
    findManyB browser cssSelector timeout searchContext reliable

//get elements

let internal elementFromList cssSelector elementsList =
    match elementsList with
    | [] -> null
    | x :: [] -> x
    | x :: y :: _ ->
        if throwIfMoreThanOneElement then raise (CanopyMoreThanOneElementFoundException(sprintf "More than one element was selected when only one was expected for selector: %s" cssSelector))
        else x

let internal someElementFromList cssSelector elementsList =
    match elementsList with
    | [] -> None
    | x :: [] -> Some(x)
    | x :: y :: _ ->
        if throwIfMoreThanOneElement then raise (CanopyMoreThanOneElementFoundException(sprintf "More than one element was selected when only one was expected for selector: %s" cssSelector))
        else Some(x)

(* documented/actions *)
let elementsB browser cssSelector =
    findManyB browser cssSelector elementTimeout browser true

(* documented/actions *)
let elements cssSelector =
    elementsB browser cssSelector

(* documented/actions *)
let elementB browser cssSelector =
    cssSelector
    |> elementsB browser
    |> elementFromList cssSelector

(* documented/actions *)
let element cssSelector =
    elementB browser cssSelector

(* documented/actions *)
let unreliableElementsB browser cssSelector =
    findManyB browser cssSelector elementTimeout browser false

(* documented/actions *)
let unreliableElements cssSelector =
    unreliableElementsB browser cssSelector

(* documented/actions *)
let unreliableElementB browser cssSelector =
    cssSelector
    |> unreliableElementsB browser
    |> elementFromList cssSelector

(* documented/actions *)
let unreliableElement cssSelector =
    unreliableElementB browser cssSelector

(* documented/actions *)
let elementWithinB browser cssSelector elem =
    findB browser cssSelector elementTimeout elem true

(* documented/actions *)
let elementWithin cssSelector elem =
    elementWithinB browser cssSelector elem

(* documented/actions *)
let elementsWithTextB browser cssSelector regex =
    unreliableElementsB browser cssSelector
    |> List.filter (fun elem -> regexMatch regex (textOf elem))

(* documented/actions *)
let elementsWithText cssSelector regex =
    elementsWithTextB browser cssSelector regex

(* documented/actions *)
let elementWithTextB browser cssSelector regex =
    (elementsWithTextB browser cssSelector regex).Head

(* documented/actions *)
let elementWithText cssSelector regex =
    elementWithTextB browser cssSelector regex

(* documented/actions *)
let parentB browser elem =
    elem |> elementWithinB browser ".."

(* documented/actions *)
let parent elem =
    parentB browser elem

(* documented/actions *)
let elementsWithinB browser cssSelector elem =
    findManyB browser cssSelector elementTimeout elem true

(* documented/actions *)
let elementsWithin cssSelector elem =
    elementsWithinB browser cssSelector elem

(* documented/actions *)
let unreliableElementsWithinB (browser: IWebDriver) cssSelector elem =
    findManyB browser cssSelector elementTimeout elem false

(* documented/actions *)
let unreliableElementsWithin cssSelector elem =
    unreliableElementsWithinB browser cssSelector elem

(* documented/actions *)
let someElementB browser cssSelector =
    cssSelector
    |> unreliableElementsB browser
    |> someElementFromList cssSelector

(* documented/actions *)
let someElement cssSelector =
    someElementB browser cssSelector

(* documented/actions *)
let someElementWithinB browser cssSelector elem =
    elem
    |> unreliableElementsWithinB browser cssSelector
    |> someElementFromList cssSelector

(* documented/actions *)
let someElementWithin cssSelector elem =
    someElementWithinB browser cssSelector elem

(* documented/actions *)
let someParentB browser elem =
    elem
    |> elementsWithinB browser ".."
    |> someElementFromList "provided element"

(* documented/actions *)
let someParent elem =
    someParentB browser elem

(* documented/actions *)
let nthB browser index cssSelector =
    List.item index (elementsB browser cssSelector)

(* documented/actions *)
let nth index cssSelector =
    nthB browser index cssSelector

(* documented/actions *)
let itemB browser index cssSelector =
    nthB index cssSelector

(* documented/actions *)
let item index cssSelector =
    itemB browser index cssSelector

(* documented/actions *)
let firstB browser cssSelector =
    elementsB browser cssSelector
    |> List.head

(* documented/actions *)
let first cssSelector =
    firstB browser cssSelector

(* documented/actions *)
let lastB browser cssSelector =
    elementsB browser cssSelector
    |> List.rev
    |> List.head

(* documented/actions *)
let last cssSelector =
    lastB browser cssSelector

//read/write
let internal writeToSelectB (browser: IWebDriver) (elem: IWebElement) (text:string) =
    let options =
        if writeToSelectWithOptionValue then
            unreliableElementsWithinB browser (sprintf """option[text()="%s"] | option[@value="%s"] | optgroup/option[text()="%s"] | optgroup/option[@value="%s"]""" text text text text) elem
        else //to preserve previous behaviour
            unreliableElementsWithinB browser (sprintf """option[text()="%s"] | optgroup/option[text()="%s"]""" text text) elem

    match options with
    | [] ->
        let message = sprintf "Element %s does not contain value %s" (elem.ToString()) text
        raise (CanopyOptionNotFoundException message)
    | head :: _ ->
        head.Click()

let internal writeToSelect (elem: IWebElement) (text:string) =
    writeToSelectB browser elem text

let internal writeToElementB browser (e : IWebElement) (text:string) =
    if e.TagName = "select" then
        writeToSelectB browser e text
    else
        let readonly = e.GetAttribute("readonly")
        if readonly = "true" then
            let message =sprintf "element %s is marked as read only, you can not write to read only elements" (e.ToString())
            raise (CanopyReadOnlyException message)
        if not optimizeByDisablingClearBeforeWrite then try e.Clear() with ex -> ex |> ignore
        e.SendKeys(text)

let internal writeToElement e text =
    writeToElementB browser e text

(* documented/actions *)
let writeB browser text item =
    match box item with
    | :? IWebElement as elem ->
        writeToElementB browser elem text
    | :? string as cssSelector ->
        wait elementTimeout (fun _ ->
            elements cssSelector
            |> List.map (fun elem ->
                try
                    writeToElementB browser elem text
                    true
                with
                    //Note: Enrich exception with proper cssSelector description
                    | :? CanopyReadOnlyException ->
                        let message = sprintf "element %s is marked as read only, you can not write to read only elements" cssSelector
                        raise (CanopyReadOnlyException message)
                    | _ ->
                        false)
            |> List.exists (fun elem -> elem = true)
        )
    | _ ->
        let message = sprintf "Can't read %O because it is not a string or element" item
        raise (CanopyNotStringOrElementException message)

(* documented/actions *)
let write text item =
    writeB browser text item

let internal safeReadB browser item =
    let readvalue = ref ""
    try
        wait elementTimeout (fun _ ->
          readvalue :=
            match box item with
            | :? IWebElement as elem ->
                textOf elem
            | :? string as cssSelector ->
                elementB browser cssSelector |> textOf
            | _ ->
                let message =
                    sprintf "Was unable to read item because it was not a string or IWebElement, but a %s"
                            (item.GetType().FullName)
                raise (CanopyReadException message)
          true)
        !readvalue
    with
        | :? WebDriverTimeoutException -> raise (CanopyReadException("was unable to read item for unknown reason"))

let internal safeRead item =
    safeReadB browser item

(* documented/actions *)
let readB browser item =
    match box item with
    | :? IWebElement as elem ->
        safeReadB browser elem
    | :? string as cssSelector ->
        safeReadB browser cssSelector
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't read %O because it is not a string or element" item))

(* documented/actions *)
let read item =
    readB browser item

(* documented/actions *)
let clearB browser item =
    let clear cssSelector (elem: IWebElement) =
        let readonly = elem.GetAttribute("readonly")
        if readonly = "true" then
            let message =
                sprintf "Element %s is marked as read only, you can not clear read only elements"
                        cssSelector
            raise (CanopyReadOnlyException message)
        elem.Clear()

    match box item with
    | :? IWebElement as elem ->
        clear elem.TagName elem
    | :? string as cssSelector ->
        elementB browser cssSelector
        |> clear cssSelector
    | _ ->
        let message = sprintf "Can't clear %O because it is not a string or element" item
        raise (CanopyNotStringOrElementException message)

(* documented/actions *)
let clear item =
    clearB browser item

//keyboard
(* documented/actions *)
let tab = Keys.Tab
(* documented/actions *)
let enter = Keys.Enter
(* documented/actions *)
let down = Keys.Down
(* documented/actions *)
let up = Keys.Up
(* documented/actions *)
let left = Keys.Left
(* documented/actions *)
let right = Keys.Right
(* documented/actions *)
let esc = Keys.Escape

(* documented/actions *)
let pressB browser key =
    let active =
        jsB browser "return document.activeElement;"
        :?> IWebElement
    active.SendKeys(key)

(* documented/actions *)
let press key =
    pressB browser key

//alerts
(* documented/actions *)
let alertB (browser: IWebDriver) =
    waitFor (fun _ ->
        browser.SwitchTo().Alert() |> ignore
        true)
    browser.SwitchTo().Alert()

(* documented/actions *)
let alert () =
    alertB browser

(* documented/actions *)
let acceptAlertB (browser: IWebDriver) =
    wait compareTimeout (fun _ ->
        browser.SwitchTo().Alert().Accept()
        true)

(* documented/actions *)
let acceptAlert () =
    acceptAlertB browser


(* documented/actions *)
let dismissAlertB (browser: IWebDriver) =
    wait compareTimeout (fun _ ->
        browser.SwitchTo().Alert().Dismiss()
        true)

(* documented/actions *)
let dismissAlert () =
    dismissAlertB browser

(* documented/actions *)
let fastTextFromCSSB browser selector =
    let script =
        //there is no map on NodeList which is the type returned by querySelectorAll =(
        sprintf """return [].map.call(document.querySelectorAll("%s"), function (item) { return item.text || item.innerText; });""" selector
    try
        js script :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object>
        |> Seq.map (fun item -> item.ToString())
        |> List.ofSeq
    with _ -> [ "default" ]

(* documented/actions *)
let fastTextFromCSS selector =
    fastTextFromCSSB browser selector

//clicking/checking
(* documented/actions *)
let clickB browser item =
    match box item with
    | :? IWebElement as element ->
        element.Click()
    | :? string as cssSelector ->
        wait elementTimeout (fun _ ->
            let atleastOneItemClicked = ref false
            elementsB browser cssSelector
            |> List.iter (fun elem ->
                try
                    elem.Click()
                    atleastOneItemClicked := true
                with ex -> ())
            !atleastOneItemClicked)
    | _ ->
        let message = sprintf "Can't click %O because it is not a string or webelement" item
        raise (CanopyNotStringOrElementException message)

(* documented/actions *)
let click item =
    clickB browser item

(* documented/actions *)
let doubleClickB (browser: IWebDriver) item =
    let js = "var evt = document.createEvent('MouseEvents'); evt.initMouseEvent('dblclick',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0,null); arguments[0].dispatchEvent(evt);"

    match box item with
    | :? IWebElement as elem ->
        (browser :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
    | :? string as cssSelector ->
        wait elementTimeout (fun _ ->
            let elem = elementB browser cssSelector
            (browser :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
            true)
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't doubleClick %O because it is not a string or webelement" item))

(* documented/actions *)
let doubleClick item =
    doubleClickB browser item

(* documented/actions *)
let modifierClickB browser modifier item =
    let actions = Actions browser

    match box item with
    | :? IWebElement as elem ->
        actions.KeyDown(modifier).Click(elem).KeyUp(modifier).Perform() |> ignore
    | :? string as cssSelector ->
        wait elementTimeout (fun _ ->
            let elem = elementB browser cssSelector
            actions.KeyDown(modifier).Click(elem).KeyUp(modifier).Perform()
            true)
    | _ ->
        let message = sprintf "Can't %O-click %O because it is not a string or webelement" modifier item
        raise (CanopyNotStringOrElementException message)

(* documented/actions *)
let ctrlClickB browser item =
    modifierClickB browser Keys.Control item

(* documented/actions *)
let ctrlClick item =
    ctrlClickB browser item

(* documented/actions *)
let shiftClickB browser item =
    modifierClickB browser Keys.Shift item

(* documented/actions *)
let shiftClick item =
    shiftClickB browser item

(* documented/actions *)
let rightClickB browser item =
    let actions = Actions(browser)

    match box item with
    | :? IWebElement as elem ->
        actions.ContextClick(elem).Perform() |> ignore
    | :? string as cssSelector ->
        wait elementTimeout (fun _ ->
            let elem = elementB browser cssSelector
            actions.ContextClick(elem).Perform()
            true)
    | _ ->
        let message = sprintf "Can't rightClick %O because it is not a string or webelement" item
        raise (CanopyNotStringOrElementException message)

(* documented/actions *)
let rightClick item =
    rightClickB browser item

(* documented/actions *)
let checkB browser item =
    try
        match box item with
        | :? IWebElement as elem ->
            if not elem.Selected then clickB browser elem
        | :? string as cssSelector ->
            waitFor (fun _ ->
                let selected = (elementB browser cssSelector).Selected
                if not selected then
                    clickB browser cssSelector
                (elementB browser cssSelector).Selected)
        | _ ->
            let message = sprintf "Can't read %O because it is not a string or element" item
            raise (CanopyNotStringOrElementException message)
    with
    | :? CanopyElementNotFoundException as ex -> raise (CanopyCheckFailedException(sprintf "%s%sfailed to check %O." ex.Message Environment.NewLine item))
    | :? WebDriverTimeoutException -> raise (CanopyCheckFailedException(sprintf "failed to check %O." item))

(* documented/actions *)
let check item =
    checkB browser item

(* documented/actions *)
let uncheckB browser item =
    try
        match box item with
        | :? IWebElement as elem ->
            if elem.Selected then clickB browser elem
        | :? string as cssSelector ->
            waitFor (fun _ ->
                if (elementB browser cssSelector).Selected then
                    clickB browser cssSelector
                not (elementB browser cssSelector).Selected)
        | _ ->
            let message = sprintf "Can't read %O because it is not a string or element" item
            raise (CanopyNotStringOrElementException message)
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%sfailed to uncheck %O." ex.Message Environment.NewLine item
        raise (CanopyUncheckFailedException message)
    | :? WebDriverTimeoutException ->
        let message = sprintf "Failed to uncheck %O." item
        raise (CanopyUncheckFailedException message)


(* documented/actions *)
let uncheck item =
    uncheckB browser item

//hoverin
(* documented/actions *)
let hoverB browser selector =
    let actions = Actions(browser)
    let e = elementB browser selector
    actions.MoveToElement(e).Perform()

(* documented/actions *)
let hover selector =
    hoverB browser selector

//draggin
(* documented/actions *)
let dragB browser cssSelectorA cssSelectorB =
    wait elementTimeout (fun _ ->
        let a = elementB browser cssSelectorA
        let b = elementB browser cssSelectorB
        (new Actions(browser)).DragAndDrop(a, b).Perform()
        true)

(* documented/actions *)
let drag cssSelectorA cssSelectorB =
    dragB browser cssSelectorA cssSelectorB

//browser related
(* documented/actions *)
let pinB (browser: IWebDriver) direction =
    let (w, h) = Screen.getPrimaryScreenResolution ()
    let maxWidth = w / 2
    browser.Manage().Window.Size <- new System.Drawing.Size(maxWidth, h)
    match direction with
    | Left ->
        browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 0), 0)
    | Right ->
        browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 1), 0)
    | FullScreen ->
        browser.Manage().Window.Maximize()

(* documented/actions *)
let pin direction =
    pinB browser direction

(* documented/actions *)
let pinToMonitorB (browser: IWebDriver) n =
    let n' = if n < 1 then 1 else n
    if Screen.monitorCount >= n' then
        let workingArea =  Screen.allScreensWorkingArea.[n'-1]
        browser.Manage().Window.Position <- new System.Drawing.Point(workingArea.X, 0)
        browser.Manage().Window.Maximize()
    else
        raise(CanopyException(sprintf "Monitor %d is not detected" n))

(* documented/actions *)
let pinToMonitor n =
    pinToMonitorB browser n

let internal firefoxDriverService _ =
    let service = Firefox.FirefoxDriverService.CreateDefaultService()
    service.HideCommandPromptWindow <- hideCommandPromptWindow
    service

let internal firefoxWithUserAgent (userAgent : string) =
    let profile = FirefoxProfile()
    profile.SetPreference("general.useragent.override", userAgent)
    let options = Firefox.FirefoxOptions();
    options.Profile <- profile
    new FirefoxDriver(firefoxDriverService (), options, TimeSpan.FromSeconds(elementTimeout))
    :> IWebDriver

let internal chromeDriverService dir =
    let service = Chrome.ChromeDriverService.CreateDefaultService(dir);
    service.HideCommandPromptWindow <- hideCommandPromptWindow;
    service

let internal chromeWithUserAgent dir userAgent =
    let options = Chrome.ChromeOptions()
    options.AddArgument("--user-agent=" + userAgent)
    new Chrome.ChromeDriver(chromeDriverService dir, options)
    :> IWebDriver

let internal ieDriverService _ =
    let service = IE.InternetExplorerDriverService.CreateDefaultService(ieDir)
    service.HideCommandPromptWindow <- hideCommandPromptWindow
    service

let internal edgeDriverService _ =
    let service = Edge.EdgeDriverService.CreateDefaultService(edgeDir)
    service.HideCommandPromptWindow <- hideCommandPromptWindow
    service

let internal safariDriverService _ =
    let service = Safari.SafariDriverService.CreateDefaultService(safariDir)
    service.HideCommandPromptWindow <- hideCommandPromptWindow
    service

(* documented/actions *)
let start b =
    //for chrome you need to download chromedriver.exe from http://code.google.com/p/chromedriver/wiki/GettingStarted
    //place chromedriver.exe in c:\ or you can place it in a customer location and change chromeDir value above
    //for ie you need to set Settings -> Advance -> Security Section -> Check-Allow active content to run files on My Computer*
    //also download IEDriverServer and place in c:\ or configure with ieDir
    //firefox just works
    //for Safari download it and put in c:\ or configure with safariDir

    browser <-
        match b with
        | IE -> new IE.InternetExplorerDriver(ieDriverService ()) :> IWebDriver
        | IEWithOptions options -> new IE.InternetExplorerDriver(ieDriverService (), options) :> IWebDriver
        | IEWithOptionsAndTimeSpan(options, timeSpan) -> new IE.InternetExplorerDriver(ieDriverService (), options, timeSpan) :> IWebDriver
        | EdgeBETA -> new Edge.EdgeDriver(edgeDriverService ()) :> IWebDriver
        | Chrome ->
            let options = Chrome.ChromeOptions()
            options.AddArgument("--disable-extensions")
            options.AddArgument("disable-infobars")
            options.AddArgument("test-type") //https://code.google.com/p/chromedriver/issues/detail?id=799
            new Chrome.ChromeDriver(chromeDriverService chromeDir, options) :> IWebDriver
        | ChromeWithOptions options ->
            new Chrome.ChromeDriver(chromeDriverService chromeDir, options) :> IWebDriver
        | ChromeWithUserAgent userAgent ->
            chromeWithUserAgent chromeDir userAgent
        | ChromeWithOptionsAndTimeSpan(options, timeSpan) ->
            new Chrome.ChromeDriver(chromeDriverService chromeDir, options, timeSpan) :> IWebDriver
        | ChromeHeadless ->
            let options = Chrome.ChromeOptions()
            options.AddArgument("--disable-extensions")
            options.AddArgument("disable-infobars")
            options.AddArgument("test-type") //https://code.google.com/p/chromedriver/issues/detail?id=799
            options.AddArgument("--headless")
            new Chrome.ChromeDriver(chromeDriverService chromeDir, options) :> IWebDriver
        | Chromium ->
            let options = Chrome.ChromeOptions()
            options.AddArgument("--disable-extensions")
            options.AddArgument("disable-infobars")
            options.AddArgument("test-type") //https://code.google.com/p/chromedriver/issues/detail?id=799
            new Chrome.ChromeDriver(chromeDriverService chromiumDir, options) :> IWebDriver
        | ChromiumWithOptions options ->
            new Chrome.ChromeDriver(chromeDriverService chromiumDir, options) :> IWebDriver
        | Firefox ->new FirefoxDriver(firefoxDriverService ()) :> IWebDriver
        | FirefoxWithProfile profile ->
            let options = new Firefox.FirefoxOptions();
            options.Profile <- profile
            new FirefoxDriver(firefoxDriverService (), options, TimeSpan.FromSeconds(elementTimeout)) :> IWebDriver
        | FirefoxWithPath path ->
          let options = new Firefox.FirefoxOptions()
          options.BrowserExecutableLocation <- path
          new FirefoxDriver(firefoxDriverService (), options, TimeSpan.FromSeconds(elementTimeout)) :> IWebDriver
        | FirefoxWithUserAgent userAgent -> firefoxWithUserAgent userAgent
        | FirefoxWithOptions options ->
            new FirefoxDriver(firefoxDriverService (), options, TimeSpan.FromSeconds(elementTimeout)) :> IWebDriver
        | FirefoxWithPathAndTimeSpan(path, timespan) ->
          let options = new Firefox.FirefoxOptions()
          options.BrowserExecutableLocation <- path
          new FirefoxDriver(firefoxDriverService (), options, timespan) :> IWebDriver
        | FirefoxWithProfileAndTimeSpan(profile, timespan) ->
          let options = new Firefox.FirefoxOptions()
          options.Profile <- profile
          new FirefoxDriver(firefoxDriverService (), options, timespan) :> IWebDriver
        | FirefoxHeadless ->
            let options = new Firefox.FirefoxOptions();
            options.AddArgument("--headless")
            new FirefoxDriver(firefoxDriverService (), options, TimeSpan.FromSeconds(elementTimeout)) :> IWebDriver
        | Safari ->
            new Safari.SafariDriver(safariDriverService ()) :> IWebDriver
        | Remote(url, capabilities) -> new Remote.RemoteWebDriver(new Uri(url), capabilities) :> IWebDriver

    if autoPinBrowserRightOnLaunch then
        pinB browser Right

    browsers <- browsers @ [browser]

(* documented/actions *)
/// Acts on global
let switchTo b =
    browser <- b

(* documented/actions *)
let switchToTabB (browser: IWebDriver) number =
    wait pageTimeout (fun _ ->
        let number = number - 1
        let tabs = browser.WindowHandles;
        if tabs |> Seq.length >= number then
            browser.SwitchTo().Window(tabs.[number]) |> ignore
            true
        else
            false)

(* documented/actions *)
let switchToTab number =
    switchToTabB browser number

(* documented/actions *)
let closeTabB (browser: IWebDriver) number =
    switchToTabB browser number
    browser.Close()

(* documented/actions *)
let closeTab number =
    closeTabB browser number


(* documented/actions *)
let tile (browsers: IWebDriver list) =
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

(* documented/actions *)
let positionBrowserB (browser: IWebDriver) left top width height =
    let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height
    let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width

    let x = left * w / 100
    let y = top * h / 100
    let bw = width * w /100
    let bh = height * h / 100

    browser.Manage().Window.Size <- new System.Drawing.Size(bw, bh)
    browser.Manage().Window.Position <- new System.Drawing.Point(x, y)

(* documented/actions *)
let positionBrowser left top width height =
    positionBrowserB browser left top width height

let internal innerSize (browser: IWebDriver) =
    let jsBrowser = browser :?> IJavaScriptExecutor
    let innerWidth = System.Int32.Parse(jsBrowser.ExecuteScript("return window.innerWidth").ToString())
    let innerHeight = System.Int32.Parse(jsBrowser.ExecuteScript("return window.innerHeight").ToString())
    innerWidth, innerHeight

(* documented/actions *)
let resizeB (browser: IWebDriver) size =
    let width,height = size
    let innerWidth, innerHeight = innerSize browser
    let newWidth = browser.Manage().Window.Size.Width - innerWidth + width
    let newHeight = browser.Manage().Window.Size.Height - innerHeight + height
    browser.Manage().Window.Size <- System.Drawing.Size(newWidth, newHeight)

(* documented/actions *)
let resize size =
    resizeB browser size

(* documented/actions *)
let rotateB browser =
    let innerWidth, innerHeight = innerSize browser
    resizeB browser (innerHeight, innerWidth)

(* documented/actions *)
let rotate () =
    rotateB browser

(* documented/actions *)
let quit browser =
    reporter.quit()
    match box browser with
    | :? IWebDriver as b ->
        b.Quit()
    | _ ->
        browsers |> List.iter (fun b -> b.Quit())

(* documented/actions *)
let currentUrlB (browser: IWebDriver) =
    browser.Url

(* documented/actions *)
let currentUrl () =
    currentUrlB browser

(* documented/assertions *)
let onnB (browser: IWebDriver) (u: string) =
    let urlPath (u: string) =
        let url = match u with
                  | x when x.StartsWith("http") -> u  //leave absolute urls alone
                  | _ -> "http://host/" + u.Trim('/') //ensure valid uri
        let uriBuilder = new System.UriBuilder(url)
        uriBuilder.Path.TrimEnd('/') //get the path part removing trailing slashes
    wait pageTimeout (fun _ ->
        browser.Url = u
        || urlPath (browser.Url) = urlPath(u))

(* documented/assertions *)
let onn (u: string) =
    onnB browser u

(* documented/assertions *)
let onB (browser: IWebDriver) (u: string) =
    try
        onnB browser u
    with
    | ex ->
        if not <| browser.Url.Contains(u) then
            let message = sprintf "on check failed, expected expression '%s' got %s" u browser.Url
            raise (CanopyOnException message)

(* documented/assertions *)
let on (u: string) =
    onB browser u

(* documented/actions *)
let urlB (browser: IWebDriver) (u: string) =
    if browser = null then
        raise (CanopyOnException "Can't navigate to the given url since the browser is not initialized.")
    browser.Navigate().GoToUrl(u)

(* documented/actions *)
let url u =
    urlB browser u

(* documented/actions *)
let titleB (browser: IWebDriver) =
    browser.Title

(* documented/actions *)
let title () =
    titleB browser

(* documented/actions *)
let reloadB browser =
    currentUrlB browser
    |> urlB browser

(* documented/actions *)
let reload () =
    reloadB browser

type Navigate =
    | Back
    | Forward

(* documented/actions *)
let back = Back
(* documented/actions *)
let forward = Forward

(* documented/actions *)
let navigateB (browser: IWebDriver) = function
    | Back ->
        browser.Navigate().Back()
    | Forward ->
        browser.Navigate().Forward()

(* documented/actions *)
let navigate direction =
    navigateB browser direction

(* documented/actions *)
let addFinder finder =
    // TODO: global variable
    let currentFinders = configuredFinders
    configuredFinders <- (fun cssSelector f ->
        currentFinders cssSelector f
        |> Seq.append (seq { yield finder cssSelector f }))

//hints
let internal addHintFinder hints finder =
    hints |> Seq.append (Seq.singleton finder)

(* DONT/documented/actions *)
let addSelector finder hintType selector =
    //gaurd against adding same hintType multipe times and increase size of finder seq
    if not <| (hints.ContainsKey(selector) && addedHints.[selector] |> List.exists (fun hint -> hint = hintType)) then
        if hints.ContainsKey(selector) then
            hints.[selector] <- addHintFinder hints.[selector] finder
            addedHints.[selector] <- [hintType] @ addedHints.[selector]
        else
            hints.[selector] <- Seq.singleton finder
            addedHints.[selector] <- [hintType]
    selector

(* documented/actions *)
let css = addSelector findByCss "css"
(* documented/actions *)
let xpath = addSelector findByXpath "xpath"
(* documented/actions *)
let jquery = addSelector findByJQuery "jquery"
(* documented/actions *)
let label = addSelector findByLabel "label"
(* documented/actions *)
let text = addSelector findByText "text"
(* documented/actions *)
let value = addSelector findByValue "value"

let skip message =
  describe <| sprintf "Skipped: %s" message
  raise <| CanopySkipTestException()

(* documented/actions *)
let waitForElementB browser cssSelector =
    waitFor (fun _ ->
        someElementB browser cssSelector |> Option.isSome)

(* documented/actions *)
let waitForElement cssSelector =
    waitForElementB browser cssSelector
