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

let private __findByFunction (browser : IWebDriver) wipTest cssSelector timeout waitFunc searchContext reliable =
    if wipTest then __colorizeAndSleep browser cssSelector
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

let private __find browser wipTest cssSelector timeout searchContext reliable =
    (__findByFunction browser wipTest cssSelector timeout __findElements searchContext reliable).Head

let private __findMany browser wipTest cssSelector timeout searchContext reliable =
    __findByFunction browser wipTest cssSelector timeout __findElements searchContext reliable

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

let __elements browser wipTest cssSelector = __findMany browser wipTest cssSelector elementTimeout browser true

let __unreliableElements browser wipTest cssSelector = __findMany browser wipTest cssSelector elementTimeout browser false

let __unreliableElement browser wipTest cssSelector = cssSelector |> __unreliableElements browser wipTest |> __elementFromList cssSelector

let __element browser wipTest cssSelector = cssSelector |> __elements browser wipTest |> __elementFromList cssSelector

let __elementWithin browser wipTest cssSelector (elem:IWebElement) =  __find browser wipTest cssSelector elementTimeout elem true

let __parent browser wipTest elem = elem |> __elementWithin browser wipTest ".."

let __elementsWithin browser wipTest cssSelector elem = __findMany browser wipTest cssSelector elementTimeout elem true

let __unreliableElementsWithin browser wipTest cssSelector elem = __findMany browser wipTest cssSelector elementTimeout elem false

let __someElement browser wipTest cssSelector = cssSelector |> __unreliableElements browser wipTest |> __someElementFromList cssSelector

let __someElementWithin browser wipTest cssSelector elem = elem |> __unreliableElementsWithin browser wipTest cssSelector |> __someElementFromList cssSelector

let __someParent browser wipTest elem = elem |> __elementsWithin browser wipTest ".." |> __someElementFromList "provided element"

let __nth browser wipTest index cssSelector = List.nth (__elements browser wipTest cssSelector) index

let __first browser wipTest cssSelector = (__elements browser wipTest cssSelector).Head

let __last browser wipTest cssSelector = (List.rev (__elements browser wipTest cssSelector)).Head
