[<AutoOpen>]
module Canopy.Core

open Canopy
open Canopy.Logging
open Canopy.Logging.Message
open Canopy.Finders
open Canopy.JaroWinkler
open FSharp.Core.Printf
open OpenQA.Selenium
open OpenQA.Selenium.Firefox
open OpenQA.Selenium.Interactions
open System
open System.Collections.ObjectModel
open System.Drawing
open System.Drawing.Imaging
open System.IO
open System.Runtime.CompilerServices

type Context =
    abstract config: CanopyConfig
    abstract browser: IWebDriver option
    abstract browsers: IWebDriver list

/// Runtime state for parallel support; you can access `.userState` for a value
/// of your choice.
type Context<'userState> =
    {
        userState: 'userState
        config: CanopyConfig
        browser: IWebDriver option
        browsers: IWebDriver list
    }
    interface Context with
        member x.config = x.config
        member x.browser = x.browser
        member x.browsers = x.browsers

[<AutoOpen>]
module ContextEx =
    let private failMissingBrowserInstance () =
        invalidOp "The browser was null; initialise your browser before calling `uriB`"

    type Context with
        member x.getBrowser () =
            match x.browser with
            | None ->
                failMissingBrowserInstance ()
            | Some x when isNull x ->
                failMissingBrowserInstance ()
            | Some b ->
                b

/// Module for manipulating the `Context` value.
module Context =
    open System.Threading

    let private map2 fnA b = function
        | None ->
            b
        | Some x ->
            fnA x

    /// Creates a new Context object, with a `conf` value of your choice and a
    /// browser.
    let create (browser: IWebDriver option) (config: CanopyConfig): Context =
        {
            config = config
            browser = browser
            browsers = browser |> map2 (fun browser -> browser :: []) []
            userState = obj ()
        }
        :> Context

    let createT browser config userState =
        {
            config = config
            browser = browser
            browsers = browser |> map2 (fun browser -> browser :: []) []
            userState = userState
        }

    /// Sets the passed browser as the current browsers and updates the list of
    /// browser instances for this context.
    let addCurrentBrowser browser context =
        { context with
            browser = Some browser
            browsers = browser :: context.browsers }

    /// Sets the currently active browser that Canopy acts on.
    let setCurrent browser context =
        { context with
            browser = Some browser
            browsers =
                if List.contains browser context.browsers then context.browsers
                else browser :: context.browsers }

    let internal _globalContext: Context option ref = ref None
    let private _contextChanged: Event<Context> = new Event<Context>()
    let private sem = obj ()
    let internal contextChanged = _contextChanged.Publish

    /// Configure the global configuration atomically (no lost writes).
    let internal configure (userState: 'us) (callback: CanopyConfig -> CanopyConfig) =
        lock sem <| fun () ->
            let context =
                match !_globalContext with
                | None ->
                    let newConfig = callback defaultConfig
                    createT None newConfig userState
                    :> Context
                | Some existing ->
                    let newConfig = callback existing.config
                    {
                        config = newConfig
                        userState = box userState
                        browser = existing.browser
                        browsers = existing.browsers
                    }
                    :> Context
            _globalContext := Some context
            _contextChanged.Trigger context
            context

    let internal getContext () =
        match !_globalContext with
        | None ->
            invalidOp "No global context configured yet"

        | Some context ->
            if context.config.throwOnStatics then
                invalidOp "Usages of functions that access the global statics is forbidden. You should be using the instance-based functions rather than the 'global' ones. See e.g. canopy/tests/ParallelUsage for examples of this"

            context.config.logger.writeLevel context.config.logLevelOnStatics (
                eventX "Global `context ()` was called.")

            context

    let private mutateNoLock (callback: Context<'us> -> Context<'us>) =
        let context = getContext ()
        let value = context :?> Context<'us>
        let result = callback value
        _globalContext := Some (result :> Context)
        _contextChanged.Trigger (result :> Context)
        result

    /// Changes the global context atomically.
    let internal mutate<'us> (callback: Context<'us> -> Context<'us>) =
        lock sem (fun () -> mutateNoLock callback :> Context)

    /// Lets the user pass a callback that configures the current context for
    /// just-so for as long as the IDisposable is undisposed. If the disposable
    /// is NOT disposed, no further progress can be made. Do NOT reconfigure
    /// canopy in one of these scopes, or you'll deadlock.
    let configureScope (configurator: CanopyConfig -> CanopyConfig) =
        Monitor.Enter sem
        try
            let current = mutateNoLock id
            let next = configure (obj ()) configurator
            { new IDisposable with
                member x.Dispose () =
                    ignore (mutateNoLock (fun _ -> current))
                    Monitor.Exit sem
            }
        with _ ->
            Monitor.Exit sem
            reraise ()

let internal context () =
    Context.getContext ()

let internal browser () =
    context().getBrowser()

let internal config () =
    context().config

// DEVELOPER? REMEMBER TO ADD ANY NEW FUNCTIONS TO `Context.fs`, as well as here!

(* documented/actions *)
let firefox = Firefox

(* documented/actions *)
let aurora = FirefoxWithPath(@"C:\Program Files (x86)\Aurora\firefox.exe")

(* documented/actions *)
let ie = IE

(* documented/actions *)
[<Obsolete "Use `edge` instead">]
let edgeBETA = Edge

(* documented/actions *)
let edge = Edge

(* documented/actions *)
let chrome = Chrome

(* documented/actions *)
let chromium = Chromium

(* documented/actions *)
let safari = Safari

let internal textOf (element: IWebElement) =
    match element.TagName with
    | "input" ->
        element.GetAttribute("value")
    | "textarea" ->
        element.GetAttribute("value")
    | "select" ->
        let value = element.GetAttribute("value")
        let options = Seq.toList (element.FindElements(By.TagName("option")))
        let option = options |> List.find (fun e -> e.GetAttribute("value") = value)
        option.Text
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
        let bitmap = new Bitmap(width=screenBounds.width, height=screenBounds.height, format=PixelFormat.Format32bppArgb)
        use graphics = Graphics.FromImage(bitmap)
        graphics.CopyFromScreen(screenBounds.x, screenBounds.y, 0, 0, screenBounds.size, CopyPixelOperation.SourceCopy)
        use stream = new MemoryStream()
        bitmap.Save(stream, ImageFormat.Png)
        stream.ToArray()
    with ex ->
        printfn "Sorry, unable to take a screenshot. An alert was up, and the backup plan failed!
        Exception: %O" ex
        Array.empty<byte>

// TODO: expose and don't force write to disk
let internal takeScreenshotB (browser: IWebDriver) directory filename =
    try
        let pic = (browser :?> ITakesScreenshot).GetScreenshot().AsByteArray
        saveScreenshot directory filename pic
        pic
    with :? UnhandledAlertException as ex ->
        let pic = takeScreenShotIfAlertUp()
        saveScreenshot directory filename pic
        let alert = browser.SwitchTo().Alert()
        alert.Accept()
        pic

let internal takeScreenshot directory filename =
    takeScreenshotB (browser ()) directory filename

let internal pngToJpg pngArray =
  let pngStream = new MemoryStream()
  let jpgStream = new MemoryStream()

  pngStream.Write(pngArray, 0, pngArray.Length)
  let img = Image.FromStream(pngStream)

  img.Save(jpgStream, ImageFormat.Jpeg)
  jpgStream.ToArray()

(* documented/actions *)
let screenshotC (context : Context) directory filename =
    match box (context.getBrowser()) with
    | :? ITakesScreenshot ->
        takeScreenshot directory filename |> pngToJpg
    | _ ->
        Array.empty<byte>

(* documented/actions *)
let screenshot directory filename =
    screenshotC (context ()) directory filename

(* documented/actions *)
let jsC (context : Context) script =
    (context.getBrowser() :?> IJavaScriptExecutor).ExecuteScript(script)

(* documented/actions *)
let js script =
    jsC (context ()) script

let internal swallowedJsC context script =
    try
        jsC context script
        |> ignore
    with ex ->
        ()

let internal swallowedJs script =
    swallowedJsC (context ()) script

(* documented/actions *)
/// This has nothing to do with the browser; consider just sleeping in your
/// test framework instead.
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
    System.Threading.Thread.Sleep(ms)

(* documented/actions *)
let putsC (context: Context) text =
    context.config.logger.write (eventX text)
    if context.config.showInfoDiv then
        let escapedText = System.Web.HttpUtility.JavaScriptStringEncode(text)
        let info = "
            var infoDiv = document.getElementById('canopy_info_div');
            if(!infoDiv) { infoDiv = document.createElement('div'); }
            infoDiv.id = 'canopy_info_div';
            infoDiv.setAttribute('style','position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;');
            document.getElementsByTagName('body')[0].appendChild(infoDiv);
            infoDiv.innerHTML = 'locating: " + escapedText + "';"
        swallowedJsC context info

(* documented/actions *)
let puts text =
    putsC (context ()) text

let internal colorizeAndSleepC (context: Context) cssSelector =
    let browser = context.getBrowser ()
    putsC context cssSelector
    swallowedJsC context <| sprintf "document.querySelector('%s').style.border = 'thick solid #FFF467';" cssSelector
    // TO CONSIDER: async instead of sleeping the thread
    sleep context.config.wipSleep
    swallowedJsC context <| sprintf "document.querySelector('%s').style.border = 'thick solid #ACD372';" cssSelector

let internal colorizeAndSleep cssSelector =
    colorizeAndSleepC (context ()) cssSelector

(* documented/actions *)
let highlightC context cssSelector =
    swallowedJsC context <| sprintf "document.querySelector('%s').style.border = 'thick solid #ACD372';" cssSelector

(* documented/actions *)
let highlight cssSelector =
    highlightC (context ()) cssSelector

let internal suggestOtherSelectors (config: CanopyConfig) cssSelector =
    if config.suggestOtherSelectors then
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
        |> (fun suggestions ->
            config.logger.write (
                eventX "Couldn't find any elements with selector '{selector}', did you mean:\n{alternatives}"
                >> setField "selector" cssSelector
                >> setField "alternatives" suggestions))

(* documented/actions *)
let describeC context text =
    putsC context text

(* documented/actions *)
let describe text =
    describeC (context ()) text

(* documented/actions *)
let waitForC (config: CanopyConfig) message f =
    let wO = config.configureOp message f
    waitFor wO

let waitForMessageC config message f =
    try
        wait config.compareTimeout f
    with
    | :? WebDriverTimeoutException ->
        raise (CanopyWaitForException (message, ()))

let waitForMessage message f =
    waitForMessageC (config ()) message f

//let waitForMessage message f =
//    let config = config ()
//    match waitForC config message f |> Async.RunSynchronously with
//    | WaitResult.Ok result ->
//        result
//    | WaitResult.Failure _
//    | WaitResult.Exn _
//    | WaitResult.Error _ as result ->
//        let message =
//            sprintf "%s%swaitFor condition failed to become true in %.1f seconds"
//                    message System.Environment.NewLine (config.compareTimeout.TotalSeconds)
//        raise (CanopyWaitForException (message, result))

(* documented/actions *)
[<Obsolete "Use waitForMessage">]
let waitFor2 message f =
    waitForMessage message f

(* documented/actions *)
let waitFor =
    waitForMessage "Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout"

/// Find related
let rec internal findElementsC (context: Context) cssSelector (searchContext: ISearchContext): IWebElement list =
    let findInIFrame () =
        let iframes = findByCss "iframe" searchContext.FindElements
        if iframes.IsEmpty then
            context.getBrowser().SwitchTo().DefaultContent() |> ignore
            []
        else
            let webElements = ref []
            iframes |> List.iter (fun frame ->
                context.getBrowser().SwitchTo().Frame(frame) |> ignore
                let root = context.getBrowser().FindElement(By.CssSelector("html"))
                webElements := findElementsC context cssSelector root
            )
            !webElements

    try
        let results =
            if hints.ContainsKey cssSelector then
                let finders = hints.[cssSelector]
                finders
                |> Seq.map (fun finder -> finder cssSelector searchContext.FindElements)
                |> Seq.filter(fun list -> not(list.IsEmpty))
            else
                context.config.finders cssSelector searchContext.FindElements
                |> Seq.filter(fun list -> not(list.IsEmpty))
        if Seq.isEmpty results then
            if context.config.optimizeBySkippingIFrameCheck then
                []
            else
                findInIFrame ()
        else
           results |> Seq.head
    with ex ->
        []

let internal findElements cssSelector searchContext: IWebElement list =
    findElementsC (context ()) cssSelector searchContext

let internal findByFunctionConf (config: CanopyConfig) cssSelector timeout waitFunc searchContext reliable =
//    TODO: how to handle this one? It's a runner config, it would seem?
//    if context.config.wipTest then
//        colorizeAndSleep cssSelector
    try
        if reliable then
            let elements = ref []
            wait (*config.logger*) config.elementTimeout (fun _ ->
                elements := waitFunc cssSelector searchContext
                not <| List.isEmpty !elements)
            !elements
        else
            waitResults (*config.logger*) config.elementTimeout (fun _ -> waitFunc cssSelector searchContext)
    with
    | :? WebDriverTimeoutException ->
        suggestOtherSelectors config cssSelector
        let message = sprintf "Canopy was unable to find element '%s'" cssSelector
        raise (CanopyElementNotFoundException message)

let internal findByFunction cssSelector timeout waitFunc searchContext reliable =
    findByFunctionConf (config ()) cssSelector timeout waitFunc searchContext reliable

let internal findC context cssSelector timeout searchContext reliable =
    let finder = findElementsC context
    findByFunctionConf context.config cssSelector timeout finder searchContext reliable
    |> List.head

let internal find cssSelector timeout searchContext reliable =
    findC (context ()) cssSelector timeout searchContext reliable

let internal findManyC context cssSelector timeout searchContext reliable =
    let finder = findElementsC context
    findByFunctionConf context.config cssSelector timeout finder searchContext reliable

let internal findMany cssSelector timeout searchContext reliable =
    findManyC (context ()) cssSelector timeout searchContext reliable

// Get elements

let internal elementFromListConf (config: CanopyConfig) cssSelector elementsList =
    match elementsList with
    | [] -> null
    | x :: [] -> x
    | x :: y :: _ ->
        if config.throwIfMoreThanOneElement then
            let message =
                sprintf "More than one element was selected when only one was expected for selector: %s"
                        cssSelector
            raise (CanopyMoreThanOneElementFoundException message)
        else x

let internal someElementFromListConf (config: CanopyConfig) cssSelector elementsList =
    match elementsList with
    | [] ->
        None
    | x :: [] ->
        Some x
    | x :: y :: _ ->
        if config.throwIfMoreThanOneElement then
            let message =
                sprintf "More than one element was selected when only one was expected for selector: %s"
                        cssSelector
            raise (CanopyMoreThanOneElementFoundException message)
        else
            Some x

(* documented/actions *)
let elementsC context cssSelector =
    findManyC context cssSelector context.config.elementTimeout
              (context.getBrowser ()) true

(* documented/actions *)
let elements cssSelector =
    elementsC (context ()) cssSelector

(* documented/actions *)
let elementC context cssSelector =
    cssSelector
    |> elementsC context
    |> elementFromListConf context.config cssSelector

(* documented/actions *)
let element cssSelector =
    elementC (context ()) cssSelector

(* documented/actions *)
let elementByIdC context (elId: string) =
    // TO CONSIDER: provide optimised implementation when finding by id
    elementC context (sprintf "#%s" elId)

(* documented/actions *)
let elementById elId =
    elementByIdC (context ()) elId

(* documented/actions *)
let unreliableElementsC context cssSelector =
    findManyC context cssSelector context.config.elementTimeout
              (context.getBrowser ()) false

(* documented/actions *)
let unreliableElements cssSelector =
    unreliableElementsC (context ()) cssSelector

(* documented/actions *)
let unreliableElementC context cssSelector =
    cssSelector
    |> unreliableElementsC context
    |> elementFromListConf context.config cssSelector

(* documented/actions *)
let unreliableElement cssSelector =
    unreliableElementC (context ()) cssSelector

(* documented/actions *)
let elementWithinC context cssSelector elem =
    findC context cssSelector context.config.elementTimeout elem true

(* documented/actions *)
let elementWithin cssSelector elem =
    elementWithinC (context ()) cssSelector elem

(* documented/actions *)
let elementsWithTextC context cssSelector regex =
    unreliableElementsC context cssSelector
    |> List.filter (fun elem -> regexMatch regex (textOf elem))

(* documented/actions *)
let elementsWithText cssSelector regex =
    elementsWithTextC (context ()) cssSelector regex

(* documented/actions *)
let elementWithTextC context cssSelector regex =
    (elementsWithTextC context cssSelector regex).Head

(* documented/actions *)
let elementWithText cssSelector regex =
    elementWithTextC (context ()) cssSelector regex

(* documented/actions *)
let parentC context elem =
    elem |> elementWithinC context ".."

(* documented/actions *)
let parent elem =
    parentC (context ()) elem

(* documented/actions *)
let elementsWithinC context cssSelector elem =
    findManyC context cssSelector context.config.elementTimeout elem true

(* documented/actions *)
let elementsWithin cssSelector elem =
    elementsWithinC (context ()) cssSelector elem

(* documented/actions *)
let unreliableElementsWithinC context cssSelector elem =
    findManyC context cssSelector context.config.elementTimeout elem false

(* documented/actions *)
let unreliableElementsWithin cssSelector elem =
    unreliableElementsWithinC (context ()) cssSelector elem

(* documented/actions *)
let someElementC context cssSelector =
    cssSelector
    |> unreliableElementsC context
    |> someElementFromListConf context.config cssSelector

(* documented/actions *)
let someElement cssSelector =
    someElementC (context ()) cssSelector

(* documented/actions *)
let someElementWithinC context cssSelector elem =
    elem
    |> unreliableElementsWithinC context cssSelector
    |> someElementFromListConf context.config cssSelector

(* documented/actions *)
let someElementWithin cssSelector elem =
    someElementWithinC (context ()) cssSelector elem

(* documented/actions *)
let someParentC context elem =
    elem
    |> elementsWithinC context ".."
    |> someElementFromListConf context.config "provided element"

(* documented/actions *)
let someParent elem =
    someParentC (context ()) elem

(* documented/actions *)
let nthC context index cssSelector =
    List.item index (elementsC context cssSelector)

(* documented/actions *)
let nth index cssSelector =
    nthC (context ()) index cssSelector

(* documented/actions *)
let itemC context index cssSelector =
    nthC context index cssSelector

(* documented/actions *)
let item index cssSelector =
    itemC (context ()) index cssSelector

(* documented/actions *)
let firstC context cssSelector =
    elementsC context cssSelector
    |> List.head

(* documented/actions *)
let first cssSelector =
    firstC (context ()) cssSelector

(* documented/actions *)
let lastC context cssSelector =
    elementsC context cssSelector
    |> List.rev
    |> List.head

(* documented/actions *)
let last cssSelector =
    lastC (context ()) cssSelector

//read/write
let internal writeToSelectC context (elem: IWebElement) (text:string) =
    let options = unreliableElementsWithinC context (sprintf """option[text()="%s"] | option[@value="%s"] | optgroup/option[text()="%s"] | optgroup/option[@value="%s"]""" text text text text) elem

    match options with
    | [] ->
        let message = sprintf "Element %s does not contain value %s" (elem.ToString()) text
        raise (CanopyOptionNotFoundException message)
    | head :: _ ->
        head.Click()

let internal writeToSelect (elem: IWebElement) (text:string) =
    writeToSelectC (context ()) elem text

let internal writeToElementC context (e : IWebElement) (text:string) =
    if e.TagName = "select" then
        writeToSelectC context e text
    else
        let readonly = e.GetAttribute("readonly")
        if readonly = "true" then
            let message =sprintf "element %s is marked as read only, you can not write to read only elements" (e.ToString())
            raise (CanopyReadOnlyException message)
        if not context.config.optimizeByDisablingClearBeforeWrite then try e.Clear() with ex -> ex |> ignore
        e.SendKeys(text)

let internal writeToElement e text =
    writeToElementC (context ()) e text

(* documented/actions *)
let writeC context text item =
    match box item with
    | :? IWebElement as elem ->
        writeToElementC context elem text
    | :? string as cssSelector ->
        wait (*context.config.logger*) context.config.elementTimeout (fun _ ->
            elementsC context cssSelector
            |> List.map (fun elem ->
                try
                    writeToElementC context elem text
                    true
                with
                // Note: Enrich exception with proper cssSelector description
                | :? CanopyReadOnlyException ->
                    let message =
                        sprintf "Element '%s' is marked as read only, you can not write to read only elements"
                                cssSelector
                    raise (CanopyReadOnlyException message)
                | _ ->
                    false)
            |> List.exists id)
    | _ ->
        let message = sprintf "Can't read %O because it is not a string or element" item
        raise (CanopyNotStringOrElementException message)

(* documented/actions *)
let write text item =
    writeC (context ()) text item

let internal safeReadC (context: Context) item =
    let readvalue = ref ""
    try
        wait (*context.config.logger*) context.config.elementTimeout (fun _ ->
          readvalue :=
            match box item with
            | :? IWebElement as elem ->
                textOf elem
            | :? string as cssSelector ->
                elementC context cssSelector |> textOf
            | _ ->
                let message =
                    sprintf "Was unable to read item because it was not a string or IWebElement, but a %s"
                            (item.GetType().FullName)
                raise (CanopyReadException message)
          true)
        !readvalue
    with
    | :? WebDriverTimeoutException ->
        raise (CanopyReadException("was unable to read item for unknown reason"))

let internal safeRead item =
    safeReadC (context ()) item

(* documented/actions *)
let readC context item =
    match box item with
    | :? IWebElement as elem ->
        safeReadC context elem
    | :? string as cssSelector ->
        safeReadC context cssSelector
    | _ ->
        raise (CanopyNotStringOrElementException(sprintf "Can't read %O because it is not a string or element" item))

(* documented/actions *)
let read item =
    readC (context ()) item

(* documented/actions *)
let clearC context item =
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
        elementC context cssSelector
        |> clear cssSelector
    | _ ->
        let message = sprintf "Can't clear %O because it is not a string or element" item
        raise (CanopyNotStringOrElementException message)

(* documented/actions *)
let clear item =
    clearC (context ()) item

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
let space = Keys.Space
(* documented/actions *)
let backspace = Keys.Backspace

(* documented/actions *)
let pressC context key =
    let active =
        jsC context "return document.activeElement;"
        :?> IWebElement
    active.SendKeys(key)

(* documented/actions *)
let press key =
    pressC (context ()) key

//alerts
(* documented/actions *)
let alertC (context : Context) =
    waitFor (fun _ ->
        context.getBrowser().SwitchTo().Alert() |> ignore
        true)
    context.getBrowser().SwitchTo().Alert()

(* documented/actions *)
let alert () =
    alertC (context ())

(* documented/actions *)
let acceptAlertC (context: Context) =
    wait (*context.config.logger*) context.config.compareTimeout (fun _ ->
        context.getBrowser().SwitchTo().Alert().Accept()
        true)

(* documented/actions *)
let acceptAlert () =
    acceptAlertC (context ())

(* documented/actions *)
let dismissAlertC (context: Context) =
    wait (*context.config.logger*) context.config.compareTimeout (fun _ ->
        context.getBrowser().SwitchTo().Alert().Dismiss()
        true)

(* documented/actions *)
let dismissAlert () =
    dismissAlertC (context ())

(* documented/actions *)
let fastTextFromCSSC context selector =
    let script =
        //there is no map on NodeList which is the type returned by querySelectorAll =(
        sprintf """return [].map.call(document.querySelectorAll("%s"), function (item) { return item.text || item.innerText; });""" selector
    try
        jsC context script :?> System.Collections.ObjectModel.ReadOnlyCollection<System.Object>
        |> Seq.map (fun item -> item.ToString())
        |> List.ofSeq
    with _ ->
        [ "default" ]

(* documented/actions *)
let fastTextFromCSS selector =
    fastTextFromCSSC (context ()) selector

//clicking/checking
(* documented/actions *)
let clickC (context: Context) item =
    match box item with
    | :? IWebElement as element ->
        element.Click()
    | :? string as cssSelector ->
        wait (*context.config.logger*) context.config.elementTimeout (fun _ ->
            let atleastOneItemClicked = ref false
            elementsC context cssSelector
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
    clickC (context ()) item

(* documented/actions *)
let doubleClickC (context : Context) item =
    let js = "var evt = document.createEvent('MouseEvents'); evt.initMouseEvent('dblclick',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0,null); arguments[0].dispatchEvent(evt);"

    match box item with
    | :? IWebElement as elem ->
        (context.getBrowser() :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
    | :? string as cssSelector ->
        wait (*context.config.logger*) context.config.elementTimeout (fun _ ->
            let elem = elementC context cssSelector
            (context.getBrowser() :?> IJavaScriptExecutor).ExecuteScript(js, elem) |> ignore
            true)
    | _ -> raise (CanopyNotStringOrElementException(sprintf "Can't doubleClick %O because it is not a string or webelement" item))

(* documented/actions *)
let doubleClick item =
    doubleClickC (context ()) item

(* documented/actions *)
let modifierClickC (context : Context) modifier item =
    let actions = Actions (context.getBrowser())

    match box item with
    | :? IWebElement as elem ->
        actions.KeyDown(modifier).Click(elem).KeyUp(modifier).Perform() |> ignore
    | :? string as cssSelector ->
        wait (*context.config.logger*) context.config.elementTimeout (fun _ ->
            let elem = elementC context cssSelector
            actions.KeyDown(modifier).Click(elem).KeyUp(modifier).Perform()
            true)
    | _ ->
        let message = sprintf "Can't %O-click %O because it is not a string or webelement" modifier item
        raise (CanopyNotStringOrElementException message)

(* documented/actions *)
let ctrlClickC context item =
    modifierClickC context Keys.Control item

(* documented/actions *)
let ctrlClick item =
    ctrlClickC (context ()) item

(* documented/actions *)
let shiftClickC context item =
    modifierClickC context Keys.Shift item

(* documented/actions *)
let shiftClick item =
    shiftClickC (context ()) item

(* documented/actions *)
let rightClickC (context : Context) item =
    let actions = Actions (context.getBrowser())

    match box item with
    | :? IWebElement as elem ->
        actions.ContextClick(elem).Perform() |> ignore
    | :? string as cssSelector ->
        wait (*context.config.logger*) context.config.elementTimeout (fun _ ->
            let elem = elementC context cssSelector
            actions.ContextClick(elem).Perform()
            true)
    | _ ->
        let message = sprintf "Can't rightClick %O because it is not a string or webelement" item
        raise (CanopyNotStringOrElementException message)

(* documented/actions *)
let rightClick item =
    rightClickC (context ()) item

(* documented/actions *)
let checkC context item =
    try
        match box item with
        | :? IWebElement as elem ->
            if not elem.Selected then clickC context elem
        | :? string as cssSelector ->
            waitFor (fun _ ->
                let selected = (elementC context cssSelector).Selected
                if not selected then
                    clickC context cssSelector
                (elementC context cssSelector).Selected)
        | _ ->
            let message = sprintf "Can't read %O because it is not a string or element" item
            raise (CanopyNotStringOrElementException message)
    with
    | :? CanopyElementNotFoundException as ex -> raise (CanopyCheckFailedException(sprintf "%s%sfailed to check %O." ex.Message Environment.NewLine item))
    | :? WebDriverTimeoutException -> raise (CanopyCheckFailedException(sprintf "failed to check %O." item))

(* documented/actions *)
let check item =
    checkC (context ()) item

(* documented/actions *)
let uncheckC context item =
    try
        match box item with
        | :? IWebElement as elem ->
            if elem.Selected then clickC context elem
        | :? string as cssSelector ->
            waitFor (fun _ ->
                if (elementC context cssSelector).Selected then
                    clickC context cssSelector
                not (elementC context cssSelector).Selected)
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
    uncheckC (context ()) item

//hoverin
(* documented/actions *)
let hoverC (context : Context) selector =
    let actions = Actions (context.getBrowser())
    let e = elementC context selector
    actions.MoveToElement(e).Perform()

(* documented/actions *)
let hover selector =
    hoverC (context ()) selector

//draggin
(* documented/actions *)
let dragC (context: Context) cssSelectorA cssSelectorB =
    wait (*context.config.logger*) context.config.elementTimeout (fun _ ->
        let a = elementC context cssSelectorA
        let b = elementC context cssSelectorB
        (new Actions(context.getBrowser())).DragAndDrop(a, b).Perform()
        true)

(* documented/actions *)
let drag cssSelectorA cssSelectorB =
    dragC (context ()) cssSelectorA cssSelectorB

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
    pinB (browser ()) direction

(* documented/actions *)
let pinToMonitorC (context : Context) n =
    let n' = if n < 1 then 1 else n
    if Screen.monitorCount >= n' then
        let workingArea =  Screen.allScreensWorkingArea.[n'-1]
        context.getBrowser().Manage().Window.Position <- new System.Drawing.Point(workingArea.X, 0)
        context.getBrowser().Manage().Window.Maximize()
    else
        raise(CanopyException(sprintf "Monitor %d is not detected" n))

(* documented/actions *)
let pinToMonitor n =
    pinToMonitorC (context ()) n

let internal firefoxDriverService config =
    let service = Firefox.FirefoxDriverService.CreateDefaultService()
    service.HideCommandPromptWindow <- config.hideCommandPromptWindow
    service

let internal firefoxWithUserAgent (config: CanopyConfig) (userAgent: string) =
    let profile = FirefoxProfile()
    profile.SetPreference("general.useragent.override", userAgent)
    let options = Firefox.FirefoxOptions()
    options.Profile <- profile
    new FirefoxDriver(firefoxDriverService config, options, config.elementTimeout)
    :> IWebDriver

let internal chromeDriverService config =
    let service = Chrome.ChromeDriverService.CreateDefaultService(config.paths.chromeDir)
    service.HideCommandPromptWindow <- config.hideCommandPromptWindow
    service

let internal chromeWithUserAgent (config: CanopyConfig) userAgent =
    let options = Chrome.ChromeOptions()
    options.AddArgument("--user-agent=" + userAgent)
    new Chrome.ChromeDriver(chromeDriverService config, options)
    :> IWebDriver

let internal ieDriverService (config: CanopyConfig) =
    let service = IE.InternetExplorerDriverService.CreateDefaultService(config.paths.ieDir)
    service.HideCommandPromptWindow <- config.hideCommandPromptWindow
    service

let internal edgeDriverService (config: CanopyConfig) =
    let service = Edge.EdgeDriverService.CreateDefaultService(config.paths.edgeDir)
    service.HideCommandPromptWindow <- config.hideCommandPromptWindow
    service

let internal safariDriverService (config: CanopyConfig) =
    let service = Safari.SafariDriverService.CreateDefaultService(config.paths.safariDir)
    service.HideCommandPromptWindow <- config.hideCommandPromptWindow
    service

#nowarn "44"

let private startUnsynchronised config mode =
    let browser =
        match mode with
        | IE ->
            new IE.InternetExplorerDriver(ieDriverService config) :> IWebDriver
        | IEWithOptions options ->
            new IE.InternetExplorerDriver(ieDriverService config, options) :> IWebDriver
        | IEWithOptionsAndTimeSpan(options, timeSpan) ->
            new IE.InternetExplorerDriver(ieDriverService config, options, timeSpan) :> IWebDriver
        | EdgeBETA
        | Edge ->
            new Edge.EdgeDriver(edgeDriverService config) :> IWebDriver
        | Chrome ->
            let options = Chrome.ChromeOptions()
            options.AddArgument("--disable-extensions")
            options.AddArgument("disable-infobars")
            options.AddArgument("test-type") //https://code.google.com/p/chromedriver/issues/detail?id=799
            new Chrome.ChromeDriver(chromeDriverService config, options) :> IWebDriver
        | ChromeWithOptions options ->
            new Chrome.ChromeDriver(chromeDriverService config, options) :> IWebDriver
        | ChromeWithUserAgent userAgent ->
            chromeWithUserAgent config userAgent
        | ChromeWithOptionsAndTimeSpan(options, timeSpan) ->
            new Chrome.ChromeDriver(chromeDriverService config, options, timeSpan) :> IWebDriver
        | ChromeHeadless ->
            let options = Chrome.ChromeOptions()
            options.AddArgument("--disable-extensions")
            options.AddArgument("disable-infobars")
            options.AddArgument("test-type") //https://code.google.com/p/chromedriver/issues/detail?id=799
            options.AddArgument("--headless")
            new Chrome.ChromeDriver(chromeDriverService config, options) :> IWebDriver
        | Chromium ->
            let options = Chrome.ChromeOptions()
            options.AddArgument("--disable-extensions")
            options.AddArgument("disable-infobars")
            options.AddArgument("test-type") //https://code.google.com/p/chromedriver/issues/detail?id=799
            new Chrome.ChromeDriver(chromeDriverService config, options) :> IWebDriver
        | ChromiumWithOptions options ->
            new Chrome.ChromeDriver(chromeDriverService config, options) :> IWebDriver
        | Firefox ->
            new FirefoxDriver(firefoxDriverService config) :> IWebDriver
        | FirefoxWithPath path ->
            let options = new Firefox.FirefoxOptions()
            options.BrowserExecutableLocation <- path
            new FirefoxDriver(firefoxDriverService config, options, config.elementTimeout) :> IWebDriver
        | FirefoxWithUserAgent userAgent ->
            firefoxWithUserAgent config userAgent
        | FirefoxWithOptions options ->
            new FirefoxDriver(firefoxDriverService config, options, config.elementTimeout) :> IWebDriver
        | FirefoxWithPathAndTimeSpan(path, timespan) ->
            let options = new Firefox.FirefoxOptions()
            options.BrowserExecutableLocation <- path
            new FirefoxDriver(firefoxDriverService config, options, timespan) :> IWebDriver
        | FirefoxWithOptionsAndTimeSpan(options, timespan) ->
            new FirefoxDriver(firefoxDriverService config, options, timespan) :> IWebDriver
        | FirefoxHeadless ->
            let options = new Firefox.FirefoxOptions()
            options.AddArgument("--headless")
            new FirefoxDriver(firefoxDriverService config, options, config.elementTimeout) :> IWebDriver
        | Safari ->
            new Safari.SafariDriver(safariDriverService config) :> IWebDriver
        | Remote(url, capabilities) ->
            new Remote.RemoteWebDriver(new Uri(url), capabilities) :> IWebDriver

    if config.autoPinBrowserRightOnLaunch then
        pinB browser Right

    browser

let startWithConfigPure config mode =
    startUnsynchronised config mode

(* documented/actions *)
let startWithConfig config mode =
    let browser =
        startUnsynchronised config mode

    Context.mutate<obj> (Context.addCurrentBrowser browser)

(* documented/actions *)
let start (mode: BrowserStartMode) =
    ignore (startWithConfig defaultConfig mode)

(* documented/actions *)
/// Acts on global
let switchTo browser =
    Context.mutate<obj> (Context.setCurrent browser)
    |> ignore

(* documented/actions *)
let switchToTabC (context: Context) number =
    wait (*context.config.logger*) context.config.pageTimeout (fun _ ->
        let number = number - 1
        let tabs = context.getBrowser().WindowHandles;
        if tabs |> Seq.length >= number then
            context.getBrowser().SwitchTo().Window(tabs.[number]) |> ignore
            true
        else
            false)

(* documented/actions *)
let switchToTab number =
    switchToTabC (context ()) number

(* documented/actions *)
let closeTabC context number =
    switchToTabC context number
    context.getBrowser().Close()

(* documented/actions *)
let closeTab number =
    closeTabC (context ()) number

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
    positionBrowserB (browser ()) left top width height

let internal innerSizeB (browser: IWebDriver) =
    let jsBrowser = browser :?> IJavaScriptExecutor
    let innerWidth = System.Int32.Parse(jsBrowser.ExecuteScript("return window.innerWidth").ToString())
    let innerHeight = System.Int32.Parse(jsBrowser.ExecuteScript("return window.innerHeight").ToString())
    innerWidth, innerHeight

(* documented/actions *)
let resizeB browser size =
    let width,height = size
    let innerWidth, innerHeight = innerSizeB browser
    let newWidth = browser.Manage().Window.Size.Width - innerWidth + width
    let newHeight = browser.Manage().Window.Size.Height - innerHeight + height
    browser.Manage().Window.Size <- System.Drawing.Size(newWidth, newHeight)

(* documented/actions *)
let resize size =
    resizeB (browser ()) size

(* documented/actions *)
let rotateB browser =
    let innerWidth, innerHeight = innerSizeB browser
    resizeB browser (innerHeight, innerWidth)

(* documented/actions *)
let rotate () =
    rotateB (browser ())

(* documented/actions *)
let quit browser =
    // TODO: this should be in the Reporters namespace
    //reporter.Quit()
    match box browser with
    | :? IWebDriver as b ->
        b.Quit()
    | :? Option<IWebDriver> as opt ->
        opt |> Option.iter (fun b -> b.Quit())
    | _ ->
        let c = context ()
        c.browsers |> List.iter (fun b -> b.Quit())

(* documented/actions *)
let currentUrlB (browser: IWebDriver) =
    browser.Url

(* documented/actions *)
let currentUrl () =
    currentUrlB (browser ())

(* documented/assertions *)
let onnC (context: Context) (u: string) =
    let urlPath (u: string) =
        let url = match u with
                  | x when x.StartsWith("http") -> u  //leave absolute urls alone
                  | _ -> "http://host/" + u.Trim('/') //ensure valid uri
        let uriBuilder = new System.UriBuilder(url)
        uriBuilder.Path.TrimEnd('/') //get the path part removing trailing slashes
    wait (*context.config.logger*) context.config.pageTimeout (fun _ ->
        context.getBrowser().Url = u
        || urlPath (context.getBrowser().Url) = urlPath(u))

(* documented/assertions *)
let onn (u: string) =
    onnC (context ()) u

(* documented/assertions *)
let onC context (u: string) =
    try
        onnC context u
    with ex ->
        let browser = context.getBrowser()
        if not (browser.Url.Contains u) then
            let message = sprintf "On check failed, expected substring '%s', but browser's URL was '%s'." u browser.Url
            raise (CanopyOnException message)

(* documented/assertions *)
let on (u: string) =
    onC (context ()) u

(* documented/assertions *)
let uriB (browser: IWebDriver) (uri: Uri) =
    browser.Navigate().GoToUrl(uri)

(* documented/assertions *)
let uri (uri: Uri) =
    uriB (browser ()) uri

(* documented/actions *)
let urlB (browser: IWebDriver) (u: string) =
   browser.Navigate().GoToUrl(u)

(* documented/actions *)
let url u =
    urlB (browser ()) u

(* documented/actions *)
let titleB (browser: IWebDriver) =
    browser.Title

(* documented/actions *)
let title () =
    titleB (browser ())

(* documented/actions *)
let reloadB browser =
    currentUrlB browser |> urlB browser

(* documented/actions *)
let reload () =
    reloadB (browser ())

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
    navigateB (browser ()) direction

(* documented/actions *)
let addFinder finder =
    Context.configure (obj()) (CanopyConfig.addFinder finder)

//hints
let internal addHintFinder hints finder =
    hints |> Seq.append (Seq.singleton finder)

// TODO: remove global variable mutation
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

(* documented/actions *)
let waitForElementC context cssSelector =
    waitFor (fun _ ->
        someElementC context cssSelector |> Option.isSome)

(* documented/actions *)
let waitForElement cssSelector =
    waitForElementC (context ()) cssSelector

/// Extensions for Context<'config> to get the DSL API on the context instance;
/// this makes it easier to use the context and browser in your parallel tests.
type Context<'config> with
    member x.screenshot directory filename =
        screenshotC x directory filename
    member x.js script =
        jsC x script
    member x.puts text =
        putsC x text
    member x.highlight cssSelector =
        highlightC x cssSelector
    member x.describe text =
        describeC x text
    member x.elements cssSelector =
        elementsC x cssSelector
    member x.element cssSelector =
        elementC x cssSelector
    member x.elementById elementId =
        elementByIdC x elementId
    member x.unreliableElements cssSelector =
        unreliableElementsC x cssSelector
    member x.unreliableElement cssSelector =
        unreliableElementC x cssSelector
    member x.elementWithin cssSelector elem =
        elementWithinC x cssSelector elem
    member x.elementsWithText cssSelector regex =
        elementsWithTextC x cssSelector regex
    member x.elementWithText cssSelector regex =
        elementWithTextC x cssSelector regex
    member x.parent elem =
        parentC x elem
    member x.elementsWithin cssSelector elem =
        elementsWithinC x cssSelector elem
    member x.unreliableElementsWithin cssSelector elem =
        unreliableElementsWithinC x cssSelector elem
    member x.someElement cssSelector =
        someElementC x cssSelector
    member x.someElementWithin cssSelector elem =
        someElementWithinC x cssSelector elem
    member x.someParent elem =
        someParentC x elem
    member x.nth index cssSelector =
        nthC x index cssSelector
    member x.item index cssSelector =
        itemC x index cssSelector
    member x.first cssSelector =
        firstC x cssSelector
    member x.last cssSelector =
        lastC x cssSelector
    member x.write text item =
        writeC x text item
    member x.read item =
        readC x item
    member x.clear item =
        clearC x item
    member x.press key =
        pressC x key
    member x.alert () =
        alertC x
    member x.acceptAlert () =
        acceptAlertC x
    member x.dismissAlert () =
        dismissAlertC x
    member x.fastTextFromCSS selector =
        fastTextFromCSSC x selector
    member x.click item =
        clickC x item
    member x.doubleClick item =
        doubleClickC x item
    member x.modifierClick modifier item =
        modifierClickC x modifier item
    member x.ctrlClick item =
        ctrlClickC x item
    member x.shiftClick item =
        shiftClickC x item
    member x.rightClick item =
        rightClickC x item
    member x.check item =
        checkC x item
    member x.uncheck item =
        uncheckC x item
    member x.hover item =
        hoverC x item
    member x.drag cssSelectorA cssSelectorB =
        dragC x cssSelectorA cssSelectorB
    member x.pin direction =
        pinB (x.getBrowser()) direction
    member x.pinToMonitor number =
        pinToMonitorC x number
    member x.switchToTab number =
        switchToTabC x number
    member x.closeTab number =
        closeTabC x number
    member x.positionBrowser left top width height =
        positionBrowserB (x.getBrowser()) left top width height
    member x.resize size =
        resizeB (x.getBrowser()) size
    member x.rotate () =
        rotateB (x.getBrowser())
    member x.currentUrl () =
        currentUrlB (x.getBrowser())
    member x.onn url =
        onnC x url
    member x.on url =
        onC x url
    member x.title () =
        titleB (x.getBrowser())
    member x.uri uri =
        uriB (x.getBrowser()) uri
    member x.url url =
        urlB (x.getBrowser()) url
    member x.reload () =
        reloadB (x.getBrowser())
    member x.navigate direction =
        navigateB (x.getBrowser()) direction
    member x.waitForElement cssSelector =
        waitForElementC x cssSelector
