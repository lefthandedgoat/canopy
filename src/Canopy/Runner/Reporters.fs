module Canopy.Runner.Reporters

open Canopy
open FSharp.Reflection
open OpenQA.Selenium
open System

let internal toString (x:'a) =
    match FSharpValue.GetUnionFields(x, typeof<'a>) with
    | case, _ ->
        case.Name

type Test(description: string, func: (unit -> unit), number: int) =
    member x.Description = description
    member x.Func = func
    member x.Number = number
    member x.Id = if description = null then (String.Format("Test #{0}", number)) else description

type Suite () = class
    member val Context : string = null with get, set
    member val TotalTestsCount : int = 0 with get, set
    member val Once = fun () -> () with get, set
    member val Before = fun () -> () with get, set
    member val After = fun () -> () with get, set
    member val Lastly = fun () -> () with get, set
    member val OnPass = fun () -> () with get, set
    member val OnFail = fun () -> () with get, set
    member val Tests : Test list = [] with get, set
    member val Wips : Test list = [] with get, set
    member val Manys : Test list = [] with get, set
    member val Always : Test list = [] with get, set
    member val IsParallel = false with get, set
    member this.Clone() = this.MemberwiseClone() :?> Suite
end

type Result =
    | Pass
    | Fail of Exception
    | Skip
    | Todo
    | FailFast
    | Failed

type IReporter =
   abstract member TestStart : string -> unit
   abstract member Pass : string -> unit
   abstract member Fail : Exception -> string -> byte [] -> string -> unit
   abstract member Todo : string -> unit
   abstract member Skip : string -> unit
   abstract member TestEnd : string -> unit
   abstract member Describe : string -> unit
   abstract member ContextStart : string -> unit
   abstract member ContextEnd : string -> unit
   abstract member Summary : int -> int -> int -> int -> int -> unit
   abstract member Write : string -> unit
   abstract member SuggestSelectors : string -> string list -> unit
   abstract member Quit : unit -> unit
   abstract member SuiteBegin : unit -> unit
   abstract member SuiteEnd : unit -> unit
   abstract member SetEnvironment : string -> unit

type ConsoleReporter() =
    interface IReporter with
        member this.Pass _ =
            Console.ForegroundColor <- ConsoleColor.Green
            Console.WriteLine("Passed");
            Console.ResetColor()

        member this.Fail ex id ss url =
            Console.ForegroundColor <- ConsoleColor.Red
            Console.WriteLine("Error: ");
            Console.ResetColor()
            Console.WriteLine(ex.Message);
            Console.Write("Url: ");
            Console.WriteLine(url);
            Console.WriteLine("Exception details: ");
            ex.ToString().Split([| "\r\n"; "\n" |], StringSplitOptions.None)
            |> Array.iter (fun trace ->
                Console.ResetColor()
                if trace.Contains(".FSharp.") || trace.Contains("canopy.core") || trace.Contains("OpenQA.Selenium") then
                    Console.WriteLine(trace)
                else
                    if trace.Contains(":line") then
                        let beginning = trace.Split([| ":line" |], StringSplitOptions.None).[0]
                        let line = trace.Split([| ":line" |], StringSplitOptions.None).[1]
                        Console.Write(beginning)
                        Console.ForegroundColor <- ConsoleColor.DarkGreen
                        Console.WriteLine(":line" + line)
                    else
                        Console.ForegroundColor <- ConsoleColor.DarkGreen
                        Console.WriteLine(trace)
                Console.ResetColor())

        member this.Describe text =
            let now = DateTime.Now.ToString()
            Console.WriteLine (sprintf "%s: %s" now text)

        member this.ContextStart c = Console.WriteLine (String.Format("context: {0}", c))

        member this.ContextEnd c = ()

        member this.Summary minutes seconds passed failed skipped =
            Console.WriteLine()
            Console.WriteLine("{0} minutes {1} seconds to execute", minutes, seconds)
            if failed = 0 then
                Console.ForegroundColor <- ConsoleColor.Green
            Console.WriteLine("{0} passed", passed)
            Console.ResetColor()
            if skipped > 0 then
                Console.ForegroundColor <- ConsoleColor.Yellow
            Console.WriteLine("{0} skipped", skipped)
            Console.ResetColor()
            if failed > 0 then
                Console.ForegroundColor <- ConsoleColor.Red
            Console.WriteLine("{0} failed", failed)
            Console.ResetColor()

        member this.Write text =
            let now = DateTime.Now.ToString()
            Console.WriteLine (sprintf "%s: %s" now text)

        member this.SuggestSelectors selector suggestions =
            Console.ForegroundColor <- ConsoleColor.Yellow
            Console.WriteLine("Couldn't find any elements with selector '{0}', did you mean:", selector)
            suggestions |> List.iter (fun suggestion -> Console.WriteLine("\t{0}", suggestion))
            Console.ResetColor()
        member this.TestStart id =
            Console.ForegroundColor <- ConsoleColor.DarkCyan
            Console.WriteLine("Test: {0}", id)
            Console.ResetColor()

        member this.TestEnd id = ()

        member this.Quit () = ()

        member this.SuiteBegin () = ()

        member this.SuiteEnd () = ()

        member this.Todo _ = ()

        member this.Skip id =
            Console.ForegroundColor <- ConsoleColor.Yellow
            Console.WriteLine("Skipped");
            Console.ResetColor()

        member this.SetEnvironment env = ()

type TeamCityReporter(?logImagesToConsole: bool) =
    let logImagesToConsole = defaultArg logImagesToConsole true

    let flowId = System.Guid.NewGuid().ToString()

    let consoleReporter : IReporter = new ConsoleReporter() :> IReporter
    let tcFriendlyMessage (message : string) =
        let message = message.Replace("|", "||")
        let message = message.Replace("'", "|'")
        let message = message.Replace("\n", "|n")
        let message = message.Replace("\r", "|r")
        let message = message.Replace(System.Environment.NewLine, "|r|n")
        let message = message.Replace("\u", "|u")
        let message = message.Replace("[", "|[")
        let message = message.Replace("]", "|]")
        message

    let teamcityReport text =
        let temcityReport = sprintf "##teamcity[%s flowId='%s']" text flowId
        consoleReporter.Describe temcityReport

    interface IReporter with
        member this.Pass id = consoleReporter.Pass id

        member this.Fail ex id ss url =
            let mutable image = ""
            if not (Array.isEmpty ss) && logImagesToConsole then
                image <- String.Format("canopy-image({0})", Convert.ToBase64String(ss))

            teamcityReport (sprintf "testFailed name='%s' message='%s' details='%s'"
                                (tcFriendlyMessage id)
                                (tcFriendlyMessage ex.Message)
                                (tcFriendlyMessage image))
            consoleReporter.Fail ex id ss url

        member this.Describe d =
            teamcityReport (sprintf "message text='%s' status='NORMAL'" (tcFriendlyMessage d))
            consoleReporter.Describe d

        member this.ContextStart c =
            teamcityReport (sprintf "testSuiteStarted name='%s'" (tcFriendlyMessage c))
            consoleReporter.ContextStart c

        member this.ContextEnd c =
            teamcityReport (sprintf "testSuiteFinished name='%s'" (tcFriendlyMessage c))
            consoleReporter.ContextEnd c

        member this.Summary minutes seconds passed failed skipped = consoleReporter.Summary minutes seconds passed failed skipped

        member this.Write w = consoleReporter.Write w

        member this.SuggestSelectors selector suggestions = consoleReporter.SuggestSelectors selector suggestions

        member this.TestStart id = teamcityReport (sprintf "testStarted name='%s'" (tcFriendlyMessage id))

        member this.TestEnd id = teamcityReport (sprintf "testFinished name='%s'" (tcFriendlyMessage id))

        member this.Quit () = ()

        member this.SuiteBegin () = ()

        member this.SuiteEnd () = ()

        member this.Todo _ = ()

        member this.Skip id = teamcityReport (sprintf "testIgnored name='%s'" (tcFriendlyMessage id))

        member this.SetEnvironment env = ()

#nowarn "44"

type LiveHtmlReporter(browser: BrowserStartMode, driverPath : string, ?pinBrowserRight0: bool) =
    let pinBrowserRight = defaultArg pinBrowserRight0 true
    let consoleReporter : IReporter = new ConsoleReporter() :> IReporter

    let _browser =
        //copy pasta!
        match browser with
        | IE ->
            new IE.InternetExplorerDriver(driverPath) :> IWebDriver
        | IEWithOptions options ->
            new IE.InternetExplorerDriver(driverPath, options) :> IWebDriver
        | IEWithOptionsAndTimeSpan(options, timeSpan) ->
            new IE.InternetExplorerDriver(driverPath, options, timeSpan) :> IWebDriver
        | EdgeBETA
        | Edge ->
            new Edge.EdgeDriver(driverPath) :> IWebDriver
        | Chrome
        | Chromium ->
            let options = Chrome.ChromeOptions()
            options.AddArguments("--disable-extensions")
            options.AddArgument("disable-infobars")
            options.AddArgument("test-type") //https://code.google.com/p/chromedriver/issues/detail?id=799
            new Chrome.ChromeDriver(driverPath, options) :> IWebDriver
        | ChromeHeadless ->
            let options = Chrome.ChromeOptions()
            options.AddArgument("--disable-extensions")
            options.AddArgument("disable-infobars")
            options.AddArgument("test-type") //https://code.google.com/p/chromedriver/issues/detail?id=799
            options.AddArgument("--headless")
            new Chrome.ChromeDriver(driverPath, options) :> IWebDriver
        | ChromeWithOptions options ->
            new Chrome.ChromeDriver(driverPath, options) :> IWebDriver
        | ChromeWithOptionsAndTimeSpan(options, timeSpan) ->
            new Chrome.ChromeDriver(driverPath, options, timeSpan) :> IWebDriver
        | ChromeWithUserAgent userAgent ->
            raise(System.Exception("Sorry ChromeWithUserAgent can't be used for LiveHtmlReporter"))
        | ChromiumWithOptions options ->
            new Chrome.ChromeDriver(driverPath, options) :> IWebDriver
        | Firefox ->
            new Firefox.FirefoxDriver() :> IWebDriver
        | FirefoxWithOptions options ->
            new Firefox.FirefoxDriver(options) :> IWebDriver
        | FirefoxWithPath path ->
            let options = new Firefox.FirefoxOptions()
            options.BrowserExecutableLocation <- path
            new Firefox.FirefoxDriver(options) :> IWebDriver
        | FirefoxWithUserAgent userAgent ->
            raise(System.Exception("Sorry FirefoxWithUserAgent can't be used for LiveHtmlReporter"))
        | FirefoxWithPathAndTimeSpan(path, timespan) ->
            let options = new Firefox.FirefoxOptions()
            options.BrowserExecutableLocation <- path
            new Firefox.FirefoxDriver(Firefox.FirefoxDriverService.CreateDefaultService(), options, timespan) :> IWebDriver
        | FirefoxWithOptionsAndTimeSpan(options, timespan) ->
            new Firefox.FirefoxDriver(Firefox.FirefoxDriverService.CreateDefaultService(), options, timespan) :> IWebDriver
        | FirefoxHeadless ->
            let options = new Firefox.FirefoxOptions()
            options.AddArgument("--headless")
            new Firefox.FirefoxDriver(options) :> IWebDriver
        | Safari ->
            new Safari.SafariDriver() :> IWebDriver
        | Remote(_,_) -> raise(System.Exception("Sorry Remote can't be used for LiveHtmlReporter"))

    let pin () =
        let (w, h) = Screen.getPrimaryScreenResolution ()
        let maxWidth = w / 2
        _browser.Manage().Window.Size <- new System.Drawing.Size(maxWidth,h)
        _browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 0),0)

    let tryPin() =
        if pinBrowserRight then do pin()

    do tryPin()

    let mutable context = System.String.Empty
    let mutable canQuit = false
    let mutable contexts : string list = []
    let mutable environment = System.String.Empty
    let testStopWatch = System.Diagnostics.Stopwatch()
    let contextStopWatch = System.Diagnostics.Stopwatch()
    let jsEncode value = System.Web.HttpUtility.JavaScriptStringEncode(value)

    new() = LiveHtmlReporter(Firefox, String.Empty)
    new(browser : BrowserStartMode) = LiveHtmlReporter(browser, String.Empty)
    new (browser : BrowserStartMode, driverPath : string) = LiveHtmlReporter(browser, driverPath, true)

    member this.browser
        with get () = _browser

    member val reportPath = None with get, set
    member val reportTemplateUrl = @"http://lefthandedgoat.github.com/canopy/reporttemplatep.html" with get, set
    member this.js script = (_browser :?> IJavaScriptExecutor).ExecuteScript(script)
    member this.reportHtml () = (this.js "return $('*').html();").ToString()
    member private this.swallowedJS script = try (_browser :?> IJavaScriptExecutor).ExecuteScript(script) |> ignore with | ex -> ()
    member this.saveReportHtml directory filename =
        if not <| System.IO.Directory.Exists(directory)
            then System.IO.Directory.CreateDirectory(directory) |> ignore
        IO.File.WriteAllText(System.IO.Path.Combine(directory,filename + ".html"), this.reportHtml())

    member this.commonFail ctx (ex:Exception) id ss url =
        let encodedId = jsEncode id
        this.swallowedJS (sprintf "updateTestInContext('%s', '%s','Fail', '%s');" ctx encodedId (Convert.ToBase64String(ss)))
        let stack = sprintf "%s%s%s" ex.Message System.Environment.NewLine ex.StackTrace
        let stack = jsEncode stack
        this.swallowedJS (sprintf "addStackToTest ('%s', '%s', '%s');" ctx encodedId stack)
        this.swallowedJS (sprintf "addUrlToTest ('%s', '%s', '%s');" ctx encodedId url)
        consoleReporter.Fail ex id ss url

    member this.passWithContext ctx id =
        let encodedId = jsEncode id
        let context = jsEncode ctx
        this.swallowedJS (sprintf "updateTestInContext('%s', '%s', 'Pass', '%s');" context encodedId "")
        consoleReporter.Pass id

    member this.failWithContext ctx ex id ss url =
        let context = jsEncode ctx
        this.commonFail context ex id ss url

    member this.writeWithContext ctx w id =
        let encodedId = jsEncode id
        let encoded = jsEncode w
        let context = jsEncode ctx
        this.swallowedJS (sprintf "addMessageToTestByName ('%s', '%s', '%s');" context encodedId encoded)
        consoleReporter.Write w

    member this.testStartWithContext ctx id =
        let encodedId = jsEncode id
        let context = jsEncode ctx
        this.swallowedJS (sprintf "addToContext ('%s', '%s');" context encodedId)
        consoleReporter.TestStart id

    member this.testEndWithContext ctx id minutes seconds =
        let encodedId = jsEncode id
        let context = jsEncode ctx
        this.swallowedJS (sprintf "addTimeToTest ('%s', '%s', '%im %is');" context encodedId minutes seconds)

    member this.todoWithContext ctx id =
        let encodedId = jsEncode id
        let context = jsEncode ctx
        this.swallowedJS (sprintf "updateTestInContext('%s', '%s', 'Todo', '%s');" context encodedId "")

    member this.skipWithContext ctx id =
        let encodedId = jsEncode id
        let context = jsEncode ctx
        this.swallowedJS (sprintf "updateTestInContext('%s', '%s', 'Skip', '%s');" context encodedId "")
        consoleReporter.Skip id

    interface IReporter with
        member this.Pass id =
            let encodedId = jsEncode id
            this.swallowedJS (sprintf "updateTestInContext('%s', '%s', 'Pass', '%s');" context encodedId "")
            consoleReporter.Pass id

        member this.Fail ex id ss url = this.commonFail context ex id ss url

        member this.Describe d =
            let encoded = jsEncode d
            this.swallowedJS (sprintf "addMessageToTest ('%s', '%s');" context encoded)
            consoleReporter.Describe d

        member this.ContextStart c =
            contextStopWatch.Reset()
            contextStopWatch.Start()
            contexts <- c :: contexts
            context <- jsEncode c
            this.swallowedJS (sprintf "addContext('%s');" context)
            this.swallowedJS (sprintf "collapseContextsExcept('%s');" context)
            consoleReporter.ContextStart c

        member this.ContextEnd c =
            contextStopWatch.Stop()
            let ellapsed = contextStopWatch.Elapsed
            this.swallowedJS (sprintf "addTimeToContext ('%s', '%im %is');" context ellapsed.Minutes ellapsed.Seconds)
            consoleReporter.ContextEnd c

        member this.Summary minutes seconds passed failed skipped =
            this.swallowedJS (sprintf "setTotalTime ('%im %is');" minutes seconds)
            consoleReporter.Summary minutes seconds passed failed skipped

        member this.Write w =
            let encoded = jsEncode w
            this.swallowedJS (sprintf "addMessageToTest ('%s', '%s');" context encoded)
            consoleReporter.Write w

        member this.SuggestSelectors selector suggestions =
            consoleReporter.SuggestSelectors selector suggestions

        member this.TestStart id =
            testStopWatch.Reset()
            testStopWatch.Start()
            let encodedId = jsEncode id
            this.swallowedJS (sprintf "addToContext ('%s', '%s');" context encodedId)
            consoleReporter.TestStart id

        member this.TestEnd id =
            let encodedId = jsEncode id
            testStopWatch.Stop()
            let ellapsed = testStopWatch.Elapsed
            this.swallowedJS (sprintf "addTimeToTest ('%s', '%s', '%im %is');" context encodedId ellapsed.Minutes ellapsed.Seconds)

        member this.Quit () =
          match this.reportPath with
            | Some(path) ->
              let reportFileInfo = new IO.FileInfo(path)
              this.saveReportHtml reportFileInfo.Directory.FullName reportFileInfo.Name
            | None -> consoleReporter.Write "Not saving report"

          if canQuit then _browser.Quit()

        member this.SuiteBegin () =
            _browser.Navigate().GoToUrl(this.reportTemplateUrl)
            this.swallowedJS (sprintf "setStartTime ('%s');" (String.Format("{0:F}", System.DateTime.Now)))
            if environment <> String.Empty then this.swallowedJS (sprintf "setEnvironment ('%s');" environment)

        member this.SuiteEnd () =
            canQuit <- true
            this.swallowedJS (sprintf "collapseContextsExcept('%s');" "") //cheap hack to collapse all contexts at the end of a run

        member this.Todo id =
            let encodedId = jsEncode id
            this.swallowedJS (sprintf "updateTestInContext('%s', '%s', 'Todo', '%s');" context encodedId "")

        member this.Skip id =
            let encodedId = jsEncode id
            this.swallowedJS (sprintf "updateTestInContext('%s', '%s', 'Skip', '%s');" context encodedId "")
            consoleReporter.Skip id

        member this.SetEnvironment env =
            environment <- env

type JUnitReporter(resultFilePath:string) =

    let consoleReporter : IReporter = new ConsoleReporter() :> IReporter

    let testStopWatch = System.Diagnostics.Stopwatch()
    let testTimes = ResizeArray<string * float>()
    let passedTests = ResizeArray<string>()
    let failedTests = ResizeArray<Exception * string>()

    interface IReporter with

        member this.Pass id =
            consoleReporter.Pass id
            passedTests.Add(id)

        member this.Fail ex id ss url =
            consoleReporter.Fail ex id ss url
            failedTests.Add(ex, id)

        member this.Describe d =
            consoleReporter.Describe d

        member this.ContextStart c =
            consoleReporter.ContextStart c

        member this.ContextEnd c =
            consoleReporter.ContextEnd c

        member this.Summary minutes seconds passed failed skipped =
            consoleReporter.Summary minutes seconds passed failed skipped
            let getTestTime test =
                testTimes.Find(fun (t, _) -> test = t) |> snd
            let passedTestsXml =
                passedTests
                |> Seq.map(fun id -> sprintf "<testcase name=\"%s\" time=\"%.3f\"/>" id (getTestTime id))
            let failedTestsXml =
                failedTests
                |> Seq.map(fun (ex, id) ->
                    Security.SecurityElement.Escape ex.Message
                    |> sprintf "<testcase name=\"%s\" time=\"%.3f\"><failure>%s</failure></testcase>" id (getTestTime id))
            let testCount = passed + failed
            let testTimeSum = testTimes |> Seq.sumBy snd
            let allTestsXml = String.Join(String.Empty, Seq.concat [passedTestsXml; failedTestsXml])
            let xml =
                sprintf "<testsuite name=\"canopy\" tests=\"%i\" time=\"%.3f\">%s</testsuite>" testCount testTimeSum allTestsXml
            let resultFile = System.IO.FileInfo(resultFilePath)
            resultFile.Directory.Create()
            consoleReporter.Write <| sprintf "Saving results to %s" resultFilePath
            let enc = new System.Text.UTF8Encoding(false)
            let bytes = enc.GetBytes xml
            use fs = new System.IO.FileStream(resultFilePath, System.IO.FileMode.OpenOrCreate)
            fs.Write(bytes, 0, bytes.Length)

        member this.Write w =
            consoleReporter.Write w

        member this.SuggestSelectors selector suggestions =
            consoleReporter.SuggestSelectors selector suggestions

        member this.TestStart id =
            consoleReporter.TestStart id
            testStopWatch.Reset()
            testStopWatch.Start()

        member this.TestEnd id =
            testStopWatch.Stop()
            let elapsedSeconds = float testStopWatch.ElapsedMilliseconds / 1000.
            testTimes.Add(id, elapsedSeconds)

        member this.Quit () = ()
        member this.SuiteBegin () = ()
        member this.SuiteEnd () = ()
        member this.Todo _ = ()
        member this.Skip id = ()
        member this.SetEnvironment env = ()
