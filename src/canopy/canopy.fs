[<AutoOpen>]
module canopy.core

open OpenQA.Selenium.Firefox
open OpenQA.Selenium
open OpenQA.Selenium.Interactions
open Microsoft.FSharp.Core.Printf
open System.IO
open System
open configuration
open reporters
open types
open finders
open System.Drawing
open System.Drawing.Imaging
open EditDistance

let mutable (failureMessage : string) = null
let mutable wipTest = false
let mutable searchedFor : (string * string) list = []

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
let phantomJS = PhantomJS
(* documented/actions *)
let safari = Safari
(* DONT/documented/actions *)
let phantomJSProxyNone = PhantomJSProxyNone

let mutable browsers = []

//misc
(* documented/actions *)
let failsWith message = failureMessage <- message

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

let private regexMatch pattern input = System.Text.RegularExpressions.Regex.Match(input, pattern).Success

let private saveScreenshot directory filename pic =
    if not <| Directory.Exists(directory)
        then Directory.CreateDirectory(directory) |> ignore
    IO.File.WriteAllBytes(Path.Combine(directory,filename + ".jpg"), pic)

let private takeScreenShotIfAlertUp () =
    try
        let screenBounds = canopy.screen.getPrimaryScreenBounds ()
        let bitmap = new Bitmap(width= screenBounds.width, height= screenBounds.height, format=PixelFormat.Format32bppArgb);
        use graphics = Graphics.FromImage(bitmap)
        graphics.CopyFromScreen(screenBounds.x, screenBounds.y, 0, 0, screenBounds.size, CopyPixelOperation.SourceCopy);
        use stream = new MemoryStream()
        bitmap.Save(stream, ImageFormat.Png)
        stream.Close()
        stream.ToArray()
    with ex ->
        printfn "Sorry, unable to take a screenshot. An alert was up, and the backup plan failed!
        Exception: %s" ex.Message
        Array.empty<byte>

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

let private pngToJpg pngArray =
  let pngStream = new MemoryStream()
  let jpgStream = new MemoryStream()

  pngStream.Write(pngArray, 0, pngArray.Length)
  let img = Image.FromStream(pngStream)

  img.Save(jpgStream, ImageFormat.Jpeg)
  jpgStream.ToArray()

(* documented/actions *)
let screenshot directory filename =
    match box browser with
        | :? ITakesScreenshot -> takeScreenshot directory filename |> pngToJpg
        | _ -> Array.empty<byte>

(* documented/actions *)
let js script = (browser :?> IJavaScriptExecutor).ExecuteScript(script)

let private swallowedJs script = try js script |> ignore with | ex -> ()

(* documented/actions *)
let sleep seconds =
    let ms = match box seconds with
              | :? int as i -> i * 1000
              | :? float as i -> Convert.ToInt32(i * 1000.0)
              | _ -> 1000
    System.Threading.Thread.Sleep(ms)

(* documented/actions *)
let puts text =
    reporter.write text
    if (showInfoDiv) then
        let escapedText = System.Web.HttpUtility.JavaScriptStringEncode(text)
        let info = "
            var infoDiv = document.getElementById('canopy_info_div');
            if(!infoDiv) { infoDiv = document.createElement('div'); }
            infoDiv.id = 'canopy_info_div';
            infoDiv.setAttribute('style','position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;');
            document.getElementsByTagName('body')[0].appendChild(infoDiv);
            infoDiv.innerHTML = 'locating: " + escapedText + "';"
        swallowedJs info

let private colorizeAndSleep cssSelector =
    puts cssSelector
    swallowedJs <| sprintf "document.querySelector('%s').style.border = 'thick solid #FFF467';" cssSelector
    sleep wipSleep
    swallowedJs <| sprintf "document.querySelector('%s').style.border = 'thick solid #ACD372';" cssSelector

(* documented/actions *)
let highlight cssSelector =
    swallowedJs <| sprintf "document.querySelector('%s').style.border = 'thick solid #ACD372';" cssSelector

let private suggestOtherSelectors cssSelector =
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
        let classes = js classesViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> "." + item.ToString()) |> Array.ofSeq
        let ids = js idsViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> "#" + item.ToString()) |> Array.ofSeq
        let values = js valuesViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> item.ToString()) |> Array.ofSeq
        let texts = js textsViaJs :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object> |> Seq.map (fun item -> item.ToString()) |> Array.ofSeq

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
                raise (CanopyWaitForException(sprintf "%s%swaitFor condition failed to become true in %.1f seconds" message System.Environment.NewLine compareTimeout))

(* documented/actions *)
let waitFor = waitFor2 "Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout"

//find related
let rec private findElements cssSelector (searchContext : ISearchContext) : IWebElement list =
    if optimizeByDisablingCoverageReport = false then searchedFor <- (cssSelector, browser.Url) :: searchedFor
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
                webElements := findElements cssSelector root
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

let private findByFunction cssSelector timeout waitFunc searchContext reliable =
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

(* documented/actions *)
let elements cssSelector = findMany cssSelector elementTimeout browser true

(* documented/actions *)
let element cssSelector = cssSelector |> elements |> elementFromList cssSelector

(* documented/actions *)
let unreliableElements cssSelector = findMany cssSelector elementTimeout browser false

(* documented/actions *)
let unreliableElement cssSelector = cssSelector |> unreliableElements |> elementFromList cssSelector

(* documented/actions *)
let elementWithin cssSelector (elem:IWebElement) =  find cssSelector elementTimeout elem true

(* documented/actions *)
let elementsWithText cssSelector regex =
    unreliableElements cssSelector
    |> List.filter (fun elem -> regexMatch regex (textOf elem))

(* documented/actions *)
let elementWithText cssSelector regex = (elementsWithText cssSelector regex).Head

(* documented/actions *)
let parent elem = elem |> elementWithin ".."

(* documented/actions *)
let elementsWithin cssSelector elem = findMany cssSelector elementTimeout elem true

(* documented/actions *)
let unreliableElementsWithin cssSelector elem = findMany cssSelector elementTimeout elem false

(* documented/actions *)
let someElement cssSelector = cssSelector |> unreliableElements |> someElementFromList cssSelector

(* documented/actions *)
let someElementWithin cssSelector elem = elem |> unreliableElementsWithin cssSelector |> someElementFromList cssSelector

(* documented/actions *)
let someParent elem = elem |> elementsWithin ".." |> someElementFromList "provided element"

(* documented/actions *)
let nth index cssSelector = List.nth (elements cssSelector) index

(* documented/actions *)
let first cssSelector = (elements cssSelector).Head

(* documented/actions *)
let last cssSelector = (List.rev (elements cssSelector)).Head

//read/write
let private writeToSelect (elem:IWebElement) (text:string) =
    let options =
        if writeToSelectWithOptionValue then
            unreliableElementsWithin (sprintf """option[text()="%s"] | option[@value="%s"] | optgroup/option[text()="%s"] | optgroup/option[@value="%s"]""" text text text text) elem
        else //to preserve previous behaviour
            unreliableElementsWithin (sprintf """option[text()="%s"] | optgroup/option[text()="%s"]""" text text) elem

    match options with
    | [] -> raise (CanopyOptionNotFoundException(sprintf "element %s does not contain value %s" (elem.ToString()) text))
    | head::_ -> head.Click()

let private writeToElement (e : IWebElement) (text:string) =
    if e.TagName = "select" then
        writeToSelect e text
    else
        let readonly = e.GetAttribute("readonly")
        if readonly = "true" then
            raise (CanopyReadOnlyException(sprintf "element %s is marked as read only, you can not write to read only elements" (e.ToString())))
        if not optimizeByDisablingClearBeforeWrite then try e.Clear() with ex -> ex |> ignore
        e.SendKeys(text)

(* documented/actions *)
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

let private safeRead item =
    let readvalue = ref ""
    try
        wait elementTimeout (fun _ ->
          readvalue :=
            match box item with
            | :? IWebElement as elem -> textOf elem
            | :? string as cssSelector -> element cssSelector |> textOf
            | _ -> raise (CanopyReadException("was unable to read item because it was not a string or IWebElement"))
          true)
        !readvalue
    with
        | :? WebDriverTimeoutException -> raise (CanopyReadException("was unable to read item for unknown reason"))

(* documented/actions *)
let read item =
    match box item with
    | :? IWebElement as elem -> safeRead elem
    | :? string as cssSelector -> safeRead cssSelector
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't read %O because it is not a string or element" item))

(* documented/actions *)
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
(* documented/assertions *)
let selected item =
    let selected cssSelector (elem : IWebElement) =
        if not <| elem.Selected then raise (CanopySelectionFailedExeception(sprintf "element selected failed, %s not selected." cssSelector))

    match box item with
    | :? IWebElement as elem -> selected elem.TagName elem
    | :? string as cssSelector -> element cssSelector |> selected cssSelector
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check selected on %O because it is not a string or element" item))

(* documented/assertions *)
let deselected item =
    let deselected cssSelector (elem : IWebElement) =
        if elem.Selected then raise (CanopyDeselectionFailedException(sprintf "element deselected failed, %s selected." cssSelector))

    match box item with
    | :? IWebElement as elem -> deselected elem.TagName elem
    | :? string as cssSelector -> element cssSelector |> deselected cssSelector
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check deselected on %O because it is not a string or element" item))

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
let press key =
    let elem = ((js "return document.activeElement;") :?> IWebElement)
    elem.SendKeys(key)

//alerts
(* documented/actions *)
let alert() =
    waitFor (fun _ ->
        browser.SwitchTo().Alert() |> ignore
        true)
    browser.SwitchTo().Alert()

(* documented/actions *)
let acceptAlert() =
    wait compareTimeout (fun _ ->
        browser.SwitchTo().Alert().Accept()
        true)

(* documented/actions *)
let dismissAlert() =
    wait compareTimeout (fun _ ->
        browser.SwitchTo().Alert().Dismiss()
        true)

(* documented/actions *)
let fastTextFromCSS selector =
  let script =
    //there is no map on NodeList which is the type returned by querySelectorAll =(
    sprintf """return [].map.call(document.querySelectorAll("%s"), function (item) { return item.text || item.innerText; });""" selector
  try
    js script :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object>
    |> Seq.map (fun item -> item.ToString())
    |> List.ofSeq
  with | _ -> [ "default" ]

//assertions
(* documented/assertions *)
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
            | :? CanopyElementNotFoundException as ex -> raise (CanopyEqualityFailedException(sprintf "%s%sequality check failed.  expected: %s, got: %s" ex.Message Environment.NewLine value !bestvalue))
            | :? WebDriverTimeoutException -> raise (CanopyEqualityFailedException(sprintf "equality check failed.  expected: %s, got: %s" value !bestvalue))

    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check equality on %O because it is not a string or alert" item))

(* documented/assertions *)
let ( != ) cssSelector value =
    try
        wait compareTimeout (fun _ -> (read cssSelector) <> value)
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyNotEqualsFailedException(sprintf "%s%snot equals check failed.  expected NOT: %s, got: " ex.Message Environment.NewLine value))
        | :? WebDriverTimeoutException -> raise (CanopyNotEqualsFailedException(sprintf "not equals check failed.  expected NOT: %s, got: %s" value (read cssSelector)))

(* documented/assertions *)
let ( *= ) cssSelector value =
    try
        wait compareTimeout (fun _ -> ( cssSelector |> elements |> Seq.exists(fun element -> (textOf element) = value)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyValueNotInListException(sprintf "%s%scan't find %s in list %s%sgot: " ex.Message Environment.NewLine value cssSelector Environment.NewLine))
        | :? WebDriverTimeoutException ->
            let sb = new System.Text.StringBuilder()
            cssSelector |> elements |> List.iter (fun e -> bprintf sb "%s%s" (textOf e) Environment.NewLine)
            raise (CanopyValueNotInListException(sprintf "can't find %s in list %s%sgot: %s" value cssSelector Environment.NewLine (sb.ToString())))

(* documented/assertions *)
let ( *!= ) cssSelector value =
    try
        wait compareTimeout (fun _ -> ( cssSelector |> elements |> Seq.exists(fun element -> (textOf element) = value) |> not))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyValueInListException(sprintf "%s%sfound check failed" ex.Message Environment.NewLine))
        | :? WebDriverTimeoutException -> raise (CanopyValueInListException(sprintf "found %s in list %s, expected not to" value cssSelector))

(* documented/assertions *)
let contains (value1 : string) (value2 : string) =
    if (value2.Contains(value1) <> true) then
        raise (CanopyContainsFailedException(sprintf "contains check failed.  %s does not contain %s" value2 value1))

(* documented/assertions *)
let containsInsensitive (value1 : string) (value2 : string) =
    let rules = StringComparison.InvariantCultureIgnoreCase
    let contains = value2.IndexOf(value1, rules)
    if contains < 0 then
        raise (CanopyContainsFailedException(sprintf "contains insensitive check failed.  %s does not contain %s" value2 value1))

(* documented/assertions *)
let notContains (value1 : string) (value2 : string) =
    if (value2.Contains(value1) = true) then
        raise (CanopyNotContainsFailedException(sprintf "notContains check failed.  %s does contain %s" value2 value1))

(* documented/assertions *)
let count cssSelector count =
    try
        wait compareTimeout (fun _ -> (unreliableElements cssSelector).Length = count)
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyCountException(sprintf "%s%scount failed. expected: %i got: %i" ex.Message Environment.NewLine count 0))
        | :? WebDriverTimeoutException -> raise (CanopyCountException(sprintf "count failed. expected: %i got: %i" count (unreliableElements cssSelector).Length))

(* documented/assertions *)
let ( =~ ) cssSelector pattern =
    try
        wait compareTimeout (fun _ -> regexMatch pattern (read cssSelector))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyEqualityFailedException(sprintf "%s%sregex equality check failed.  expected: %s, got:" ex.Message Environment.NewLine pattern))
        | :? WebDriverTimeoutException -> raise (CanopyEqualityFailedException(sprintf "regex equality check failed.  expected: %s, got: %s" pattern (read cssSelector)))

(* documented/assertions *)
let ( !=~ ) cssSelector pattern =
    try
        wait compareTimeout (fun _ -> regexMatch pattern (read cssSelector) |> not)
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyEqualityFailedException(sprintf "%s%sregex not equality check failed.  expected NOT: %s, got:" ex.Message Environment.NewLine pattern))
        | :? WebDriverTimeoutException -> raise (CanopyEqualityFailedException(sprintf "regex not equality check failed.  expected NOT: %s, got: %s" pattern (read cssSelector)))

(* documented/assertions *)
let ( *~ ) cssSelector pattern =
    try
        wait compareTimeout (fun _ -> ( cssSelector |> elements |> Seq.exists(fun element -> regexMatch pattern (textOf element))))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyValueNotInListException(sprintf "%s%scan't regex find %s in list %s%sgot: " ex.Message Environment.NewLine pattern cssSelector Environment.NewLine))
        | :? WebDriverTimeoutException ->
            let sb = new System.Text.StringBuilder()
            cssSelector |> elements |> List.iter (fun e -> bprintf sb "%s%s" (textOf e) Environment.NewLine)
            raise (CanopyValueNotInListException(sprintf "can't regex find %s in list %s%sgot: %s" pattern cssSelector Environment.NewLine (sb.ToString())))

(* documented/assertions *)
let is expected actual =
    if expected <> actual then
        raise (CanopyEqualityFailedException(sprintf "equality check failed.  expected: %O, got: %O" expected actual))

(* documented/assertions *)
let (===) expected actual = is expected actual

let private shown (elem : IWebElement) =
    let opacity = elem.GetCssValue("opacity")
    let display = elem.GetCssValue("display")
    display <> "none" && opacity = "1"

(* documented/assertions *)
let displayed item =
    try
        wait compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element ->  shown element
            | :? string as cssSelector -> element cssSelector |> shown
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check displayed on %O because it is not a string or webelement" item)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyDisplayedFailedException(sprintf "%s%sdisplay check for %O failed." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyDisplayedFailedException(sprintf "display check for %O failed." item))

(* documented/assertions *)
let notDisplayed item =
    try
        wait compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element -> not(shown element)
            | :? string as cssSelector -> (unreliableElements cssSelector |> List.isEmpty) || not(unreliableElement cssSelector |> shown)
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check notDisplayed on %O because it is not a string or webelement" item)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyNotDisplayedFailedException(sprintf "%s%snotDisplay check for %O failed." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyNotDisplayedFailedException(sprintf "notDisplay check for %O failed." item))

(* documented/assertions *)
let enabled item =
    try
        wait compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element -> element.Enabled = true
            | :? string as cssSelector -> (element cssSelector).Enabled = true
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check enabled on %O because it is not a string or webelement" item)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyEnabledFailedException(sprintf "%s%senabled check for %O failed." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyEnabledFailedException(sprintf "enabled check for %O failed." item))

(* documented/assertions *)
let disabled item =
    try
        wait compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element -> element.Enabled = false
            | :? string as cssSelector -> (element cssSelector).Enabled = false
            | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't check disabled on %O because it is not a string or webelement" item)))
    with
        | :? CanopyElementNotFoundException as ex -> raise (CanopyDisabledFailedException(sprintf "%s%sdisabled check for %O failed." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyDisabledFailedException(sprintf "disabled check for %O failed." item))

(* documented/assertions *)
let fadedIn cssSelector = fun _ -> element cssSelector |> shown

//clicking/checking
(* documented/actions *)
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

(* documented/actions *)
let doubleClick item =
    let js = "var evt = document.createEvent('MouseEvents'); evt.initMouseEvent('dblclick',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0,null); arguments[0].dispatchEvent(evt);"

    match box item with
    | :? IWebElement as elem -> (browser :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
    | :? string as cssSelector ->
        wait elementTimeout (fun _ -> ( let elem = element cssSelector
                                        (browser :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
                                        true))
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't doubleClick %O because it is not a string or webelement" item))

(* documented/actions *)
let ctrlClick item =
        let actions = Actions(browser)

        match box item with
        | :? IWebElement as elem -> actions.KeyDown(Keys.Control).Click(elem).KeyUp(Keys.Control).Perform() |> ignore
        | :? string as cssSelector ->
        wait elementTimeout (fun _ -> ( let elem = element cssSelector
                                        actions.KeyDown(Keys.Control).Click(elem).KeyUp(Keys.Control).Perform()
                                        true))
        | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't ctrlClick %O because it is not a string or webelement" item))

(* documented/actions *)
let shiftClick item =
        let actions = Actions(browser)

        match box item with
        | :? IWebElement as elem -> actions.KeyDown(Keys.Shift).Click(elem).KeyUp(Keys.Shift).Perform() |> ignore
        | :? string as cssSelector ->
        wait elementTimeout (fun _ -> ( let elem = element cssSelector
                                        actions.KeyDown(Keys.Shift).Click(elem).KeyUp(Keys.Shift).Perform()
                                        true))
        | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't shiftClick %O because it is not a string or webelement" item))

(* documented/actions *)
let rightClick item =
        let actions = Actions(browser)

        match box item with
        | :? IWebElement as elem -> actions.ContextClick(elem).Perform() |> ignore
        | :? string as cssSelector ->
        wait elementTimeout (fun _ -> ( let elem = element cssSelector
                                        actions.ContextClick(elem).Perform()
                                        true))
        | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't rightClick %O because it is not a string or webelement" item))

(* documented/actions *)
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
        | :? CanopyElementNotFoundException as ex -> raise (CanopyCheckFailedException(sprintf "%s%sfailed to check %O." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyCheckFailedException(sprintf "failed to check %O." item))

(* documented/actions *)
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
        | :? CanopyElementNotFoundException as ex -> raise (CanopyUncheckFailedException(sprintf "%s%sfailed to uncheck %O." ex.Message Environment.NewLine item))
        | :? WebDriverTimeoutException -> raise (CanopyUncheckFailedException(sprintf "failed to uncheck %O." item))

//hoverin
(* documented/actions *)
let hover selector =
    let actions = Actions(browser)
    let e = element selector
    actions.MoveToElement(e).Perform()

//draggin
(* documented/actions *)
let (-->) cssSelectorA cssSelectorB =
    wait elementTimeout (fun _ ->
        let a = element cssSelectorA
        let b = element cssSelectorB
        (new Actions(browser)).DragAndDrop(a, b).Perform()
        true)

(* documented/actions *)
let drag cssSelectorA cssSelectorB = cssSelectorA --> cssSelectorB

//browser related
(* documented/actions *)
let pin direction =
    let (w, h) = canopy.screen.getPrimaryScreenResolution ()
    let maxWidth = w / 2
    browser.Manage().Window.Size <- new System.Drawing.Size(maxWidth, h)
    match direction with
    | Left -> browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 0), 0)
    | Right -> browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 1), 0)
    | FullScreen -> browser.Manage().Window.Maximize()

(* documented/actions *)
let pinToMonitor n =
    let n' = if n < 1 then 1 else n
    if canopy.screen.monitorCount >= n' then
        let workingArea =  canopy.screen.allScreensWorkingArea.[n'-1]
        browser.Manage().Window.Position <- new System.Drawing.Point(workingArea.X, 0)
        browser.Manage().Window.Maximize()
    else
        raise(CanopyException(sprintf "Monitor %d is not detected" n))

let private firefoxDriverService _ =
    let service = Firefox.FirefoxDriverService.CreateDefaultService()
    service.HideCommandPromptWindow <- hideCommandPromptWindow
    service

let private firefoxWithUserAgent (userAgent : string) =
    let profile = FirefoxProfile()
    profile.SetPreference("general.useragent.override", userAgent)
    let options = Firefox.FirefoxOptions();
    options.Profile <- profile
    new FirefoxDriver(firefoxDriverService (), options, TimeSpan.FromSeconds(elementTimeout)) :> IWebDriver

let private chromeDriverService dir = 
    let service = Chrome.ChromeDriverService.CreateDefaultService(dir);
    service.HideCommandPromptWindow <- hideCommandPromptWindow;
    service

let private chromeWithUserAgent dir userAgent =
    let options = Chrome.ChromeOptions()
    options.AddArgument("--user-agent=" + userAgent)
    new Chrome.ChromeDriver(chromeDriverService dir, options) :> IWebDriver

let private phantomJsDriverService _ = 
    let service = PhantomJS.PhantomJSDriverService.CreateDefaultService(phantomJSDir)
    service.HideCommandPromptWindow <- hideCommandPromptWindow
    service

let private ieDriverService _ = 
    let service = IE.InternetExplorerDriverService.CreateDefaultService(ieDir)
    service.HideCommandPromptWindow <- hideCommandPromptWindow
    service

let private edgeDriverService _ = 
    let service = Edge.EdgeDriverService.CreateDefaultService(edgeDir)
    service.HideCommandPromptWindow <- hideCommandPromptWindow
    service

let private safariDriverService _ = 
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
    //for phantomjs download it and put in c:\ or configure with phantomJSDir
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
        | PhantomJS ->
            autoPinBrowserRightOnLaunch <- false
            new PhantomJS.PhantomJSDriver(phantomJsDriverService ()) :> IWebDriver
        | PhantomJSProxyNone ->
            autoPinBrowserRightOnLaunch <- false
            let service = phantomJsDriverService ()
            service.ProxyType <- "none"
            new PhantomJS.PhantomJSDriver(service) :> IWebDriver
        | Remote(url, capabilities) -> new Remote.RemoteWebDriver(new Uri(url), capabilities) :> IWebDriver

    if autoPinBrowserRightOnLaunch = true then pin Right
    browsers <- browsers @ [browser]

(* documented/actions *)
let switchTo b = browser <- b

(* documented/actions *)
let switchToTab number =
    wait pageTimeout (fun _ ->
        let number = number - 1
        let tabs = browser.WindowHandles;
        if tabs |> Seq.length >= number then
            browser.SwitchTo().Window(tabs.[number]) |> ignore
            true
        else
            false)

(* documented/actions *)
let closeTab number =
    switchToTab number
    browser.Close()

(* documented/actions *)
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

(* documented/actions *)
let positionBrowser left top width height =
    let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height
    let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width

    let x = left * w / 100
    let y = top * h / 100
    let bw = width * w /100
    let bh = height * h / 100

    browser.Manage().Window.Size <- new System.Drawing.Size(bw, bh)
    browser.Manage().Window.Position <- new System.Drawing.Point(x, y)


let private innerSize() =
    let jsBrowser = browser :?> IJavaScriptExecutor
    let innerWidth = System.Int32.Parse(jsBrowser.ExecuteScript("return window.innerWidth").ToString())
    let innerHeight = System.Int32.Parse(jsBrowser.ExecuteScript("return window.innerHeight").ToString())
    innerWidth, innerHeight

(* documented/actions *)
let resize size =
    let width,height = size
    let innerWidth, innerHeight = innerSize()
    let newWidth = browser.Manage().Window.Size.Width - innerWidth + width
    let newHeight = browser.Manage().Window.Size.Height - innerHeight + height
    browser.Manage().Window.Size <- System.Drawing.Size(newWidth, newHeight)

(* documented/actions *)
let rotate() =
    let innerWidth, innerHeight = innerSize()
    resize(innerHeight, innerWidth)

(* documented/actions *)
let quit browser =
    reporter.quit()
    match box browser with
    | :? IWebDriver as b -> b.Quit()
    | _ -> browsers |> List.iter (fun b -> b.Quit())

(* documented/actions *)
let currentUrl() = browser.Url

(* documented/assertions *)
let onn (u: string) =
    let urlPath (u : string) =
        let url = match u with
                  | x when x.StartsWith("http") -> u  //leave absolute urls alone
                  | _ -> "http://host/" + u.Trim('/') //ensure valid uri
        let uriBuilder = new System.UriBuilder(url)
        uriBuilder.Path.TrimEnd('/') //get the path part removing trailing slashes
    wait pageTimeout (fun _ -> if browser.Url = u then true else urlPath(browser.Url) = urlPath(u))

(* documented/assertions *)
let on (u: string) =
    try
        onn u
    with
        | ex -> if browser.Url.Contains(u) = false then raise (CanopyOnException(sprintf "on check failed, expected expression '%s' got %s" u browser.Url))

(* documented/actions *)
let ( !^ ) (u : string) =
    if browser = null then
        raise (CanopyOnException "Can't navigate to the given url since the browser is not initialized.")
    browser.Navigate().GoToUrl(u)

(* documented/actions *)
let url u = !^ u

(* documented/actions *)
let title() = browser.Title

(* documented/actions *)
let reload = currentUrl >> url

type Navigate =
  | Back
  | Forward

(* documented/actions *)
let back = Back
(* documented/actions *)
let forward = Forward

(* documented/actions *)
let navigate = function
  | Back -> browser.Navigate().Back()
  | Forward -> browser.Navigate().Forward()

(* documented/actions *)
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
    let f = sprintf "Coverage_%s" (DateTime.Now.ToString("MMM-d_HH-mm-ss-fff"))
    let ss = screenshot p f
    reporter.coverage nonMutableInnerUrl ss nonMutableInnerUrl

(* documented/actions *)
let addFinder finder =
    let currentFinders = configuredFinders
    configuredFinders <- (fun cssSelector f ->
        currentFinders cssSelector f
        |> Seq.append (seq { yield finder cssSelector f }))

//hints
let private addHintFinder hints finder = hints |> Seq.append (seq { yield finder })
(* DONT/documented/actions *)
let addSelector finder hintType selector =
    //gaurd against adding same hintType multipe times and increase size of finder seq
    if not <| (hints.ContainsKey(selector) && addedHints.[selector] |> List.exists (fun hint -> hint = hintType)) then
        if hints.ContainsKey(selector) then
            hints.[selector] <- addHintFinder hints.[selector] finder
            addedHints.[selector] <- [hintType] @ addedHints.[selector]
        else
            hints.[selector] <- seq { yield finder }
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
let waitForElement cssSelector =
    waitFor (fun _ -> someElement cssSelector |> Option.isSome)
