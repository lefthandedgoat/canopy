module canopy.reporters

open System
open OpenQA.Selenium
open types

type IReporter =
   abstract member testStart : string -> unit
   abstract member pass : string -> unit
   abstract member fail : Exception -> string -> byte [] -> string -> unit
   abstract member todo : string -> unit
   abstract member skip : string -> unit
   abstract member testEnd : string -> unit
   abstract member describe : string -> unit
   abstract member contextStart : string -> unit
   abstract member contextEnd : string -> unit
   abstract member summary : int -> int -> int -> int -> int -> unit
   abstract member write : string -> unit
   abstract member suggestSelectors : string -> string list -> unit
   abstract member quit : unit -> unit
   abstract member suiteBegin : unit -> unit
   abstract member suiteEnd : unit -> unit
   abstract member coverage : string -> byte [] -> string -> unit
   abstract member setEnvironment : string -> unit

type ConsoleReporter() =
    interface IReporter with
        member this.pass _ =
            Console.ForegroundColor <- ConsoleColor.Green
            Console.WriteLine("Passed");
            Console.ResetColor()

        member this.fail ex id ss url =
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

        member this.describe text =
            let now = DateTime.Now.ToString()
            Console.WriteLine (sprintf "%s: %s" now text)

        member this.contextStart c = Console.WriteLine (String.Format("context: {0}", c))

        member this.contextEnd c = ()

        member this.summary minutes seconds passed failed skipped =
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

        member this.write text =
            let now = DateTime.Now.ToString()
            Console.WriteLine (sprintf "%s: %s" now text)

        member this.suggestSelectors selector suggestions =
            Console.ForegroundColor <- ConsoleColor.Yellow
            Console.WriteLine("Couldn't find any elements with selector '{0}', did you mean:", selector)
            suggestions |> List.iter (fun suggestion -> Console.WriteLine("\t{0}", suggestion))
            Console.ResetColor()
        member this.testStart id =
            Console.ForegroundColor <- ConsoleColor.DarkCyan
            Console.WriteLine("Test: {0}", id)
            Console.ResetColor()

        member this.testEnd id = ()

        member this.quit () = ()

        member this.suiteBegin () = ()

        member this.suiteEnd () = ()

        member this.coverage url ss _ = ()

        member this.todo _ = ()

        member this.skip id =
            Console.ForegroundColor <- ConsoleColor.Yellow
            Console.WriteLine("Skipped");
            Console.ResetColor()

        member this.setEnvironment env = ()

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
        consoleReporter.describe temcityReport

    interface IReporter with
        member this.pass id = consoleReporter.pass id

        member this.fail ex id ss url =
            let mutable image = ""
            if not (Array.isEmpty ss) && logImagesToConsole then
                image <- String.Format("canopy-image({0})", Convert.ToBase64String(ss))

            teamcityReport (sprintf "testFailed name='%s' message='%s' details='%s'"
                                (tcFriendlyMessage id)
                                (tcFriendlyMessage ex.Message)
                                (tcFriendlyMessage image))
            consoleReporter.fail ex id ss url

        member this.describe d =
            teamcityReport (sprintf "message text='%s' status='NORMAL'" (tcFriendlyMessage d))
            consoleReporter.describe d

        member this.contextStart c =
            teamcityReport (sprintf "testSuiteStarted name='%s'" (tcFriendlyMessage c))
            consoleReporter.contextStart c

        member this.contextEnd c =
            teamcityReport (sprintf "testSuiteFinished name='%s'" (tcFriendlyMessage c))
            consoleReporter.contextEnd c

        member this.summary minutes seconds passed failed skipped = consoleReporter.summary minutes seconds passed failed skipped

        member this.write w = consoleReporter.write w

        member this.suggestSelectors selector suggestions = consoleReporter.suggestSelectors selector suggestions

        member this.testStart id = teamcityReport (sprintf "testStarted name='%s'" (tcFriendlyMessage id))

        member this.testEnd id = teamcityReport (sprintf "testFinished name='%s'" (tcFriendlyMessage id))

        member this.quit () = ()

        member this.suiteBegin () = ()

        member this.suiteEnd () = ()

        member this.coverage url ss _ = ()

        member this.todo _ = ()

        member this.skip id = teamcityReport (sprintf "testIgnored name='%s'" (tcFriendlyMessage id))

        member this.setEnvironment env = ()

type LiveHtmlReporter(browser : BrowserStartMode, driverPath : string, ?pinBrowserRight0: bool) =    
    let pinBrowserRight = defaultArg pinBrowserRight0 true
    let consoleReporter : IReporter = new ConsoleReporter() :> IReporter

    let _browser =
        //copy pasta!
        match browser with
        | IE -> new IE.InternetExplorerDriver(driverPath) :> IWebDriver
        | IEWithOptions options -> new IE.InternetExplorerDriver(driverPath, options) :> IWebDriver
        | IEWithOptionsAndTimeSpan(options, timeSpan) -> new IE.InternetExplorerDriver(driverPath, options, timeSpan) :> IWebDriver
        | EdgeBETA -> new Edge.EdgeDriver(driverPath) :> IWebDriver
        | Chrome | Chromium ->
                let options = Chrome.ChromeOptions()
                options.AddArguments("--disable-extensions")
                options.AddArgument("disable-infobars")
                options.AddArgument("test-type") //https://code.google.com/p/chromedriver/issues/detail?id=799                
                new Chrome.ChromeDriver(driverPath, options) :> IWebDriver
        | ChromeWithOptions options -> new Chrome.ChromeDriver(driverPath, options) :> IWebDriver
        | ChromeWithOptionsAndTimeSpan(options, timeSpan) -> new Chrome.ChromeDriver(driverPath, options, timeSpan) :> IWebDriver
        | ChromeWithUserAgent userAgent -> raise(System.Exception("Sorry ChromeWithUserAgent can't be used for LiveHtmlReporter"))
        | ChromiumWithOptions options -> new Chrome.ChromeDriver(driverPath, options) :> IWebDriver
        | Firefox -> new Firefox.FirefoxDriver() :> IWebDriver
        | FirefoxWithProfile profile -> new Firefox.FirefoxDriver(profile) :> IWebDriver
        | FirefoxWithPath path -> 
          let options = new Firefox.FirefoxOptions()
          options.BrowserExecutableLocation <- path
          new Firefox.FirefoxDriver(options) :> IWebDriver
        | FirefoxWithUserAgent userAgent -> raise(System.Exception("Sorry FirefoxWithUserAgent can't be used for LiveHtmlReporter"))
        | FirefoxWithPathAndTimeSpan(path, timespan) -> 
          let options = new Firefox.FirefoxOptions()
          options.BrowserExecutableLocation <- path
          new Firefox.FirefoxDriver(Firefox.FirefoxDriverService.CreateDefaultService(), options, timespan) :> IWebDriver
        | FirefoxWithProfileAndTimeSpan(profile, timespan) -> 
          let options = new Firefox.FirefoxOptions()
          options.Profile <- profile
          new Firefox.FirefoxDriver(Firefox.FirefoxDriverService.CreateDefaultService(), options, timespan) :> IWebDriver
        | Safari -> new Safari.SafariDriver() :> IWebDriver
        | PhantomJS | PhantomJSProxyNone -> raise(System.Exception("Sorry PhantomJS can't be used for LiveHtmlReporter"))
        | Remote(_,_) -> raise(System.Exception("Sorry Remote can't be used for LiveHtmlReporter"))

    let pin () =
        let (w, h) = canopy.screen.getPrimaryScreenResolution ()
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
        consoleReporter.fail ex id ss url

    member this.passWithContext ctx id =
        let encodedId = jsEncode id
        let context = jsEncode ctx
        this.swallowedJS (sprintf "updateTestInContext('%s', '%s', 'Pass', '%s');" context encodedId "")
        consoleReporter.pass id

    member this.failWithContext ctx ex id ss url =
        let context = jsEncode ctx
        this.commonFail context ex id ss url

    member this.writeWithContext ctx w id =
        let encodedId = jsEncode id
        let encoded = jsEncode w
        let context = jsEncode ctx
        this.swallowedJS (sprintf "addMessageToTestByName ('%s', '%s', '%s');" context encodedId encoded)
        consoleReporter.write w

    member this.testStartWithContext ctx id =
        let encodedId = jsEncode id
        let context = jsEncode ctx
        this.swallowedJS (sprintf "addToContext ('%s', '%s');" context encodedId)
        consoleReporter.testStart id

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
        consoleReporter.skip id

    interface IReporter with
        member this.pass id =
            let encodedId = jsEncode id
            this.swallowedJS (sprintf "updateTestInContext('%s', '%s', 'Pass', '%s');" context encodedId "")
            consoleReporter.pass id

        member this.fail ex id ss url = this.commonFail context ex id ss url

        member this.describe d =
            let encoded = jsEncode d
            this.swallowedJS (sprintf "addMessageToTest ('%s', '%s');" context encoded)
            consoleReporter.describe d

        member this.contextStart c =
            contextStopWatch.Reset()
            contextStopWatch.Start()
            contexts <- c :: contexts
            context <- jsEncode c
            this.swallowedJS (sprintf "addContext('%s');" context)
            this.swallowedJS (sprintf "collapseContextsExcept('%s');" context)
            consoleReporter.contextStart c

        member this.contextEnd c =
            contextStopWatch.Stop()
            let ellapsed = contextStopWatch.Elapsed
            this.swallowedJS (sprintf "addTimeToContext ('%s', '%im %is');" context ellapsed.Minutes ellapsed.Seconds)
            consoleReporter.contextEnd c

        member this.summary minutes seconds passed failed skipped =
            this.swallowedJS (sprintf "setTotalTime ('%im %is');" minutes seconds)
            consoleReporter.summary minutes seconds passed failed skipped

        member this.write w =
            let encoded = jsEncode w
            this.swallowedJS (sprintf "addMessageToTest ('%s', '%s');" context encoded)
            consoleReporter.write w

        member this.suggestSelectors selector suggestions =
            consoleReporter.suggestSelectors selector suggestions

        member this.testStart id =
            testStopWatch.Reset()
            testStopWatch.Start()
            let encodedId = jsEncode id
            this.swallowedJS (sprintf "addToContext ('%s', '%s');" context encodedId)
            consoleReporter.testStart id

        member this.testEnd id =
            let encodedId = jsEncode id
            testStopWatch.Stop()
            let ellapsed = testStopWatch.Elapsed
            this.swallowedJS (sprintf "addTimeToTest ('%s', '%s', '%im %is');" context encodedId ellapsed.Minutes ellapsed.Seconds)

        member this.quit () =
          match this.reportPath with
            | Some(path) ->
              let reportFileInfo = new IO.FileInfo(path)
              this.saveReportHtml reportFileInfo.Directory.FullName reportFileInfo.Name
            | None -> consoleReporter.write "Not saving report"

          if canQuit then _browser.Quit()

        member this.suiteBegin () =
            _browser.Navigate().GoToUrl(this.reportTemplateUrl)
            this.swallowedJS (sprintf "setStartTime ('%s');" (String.Format("{0:F}", System.DateTime.Now)))
            if environment <> String.Empty then this.swallowedJS (sprintf "setEnvironment ('%s');" environment)

        member this.suiteEnd () =
            canQuit <- true
            this.swallowedJS (sprintf "collapseContextsExcept('%s');" "") //cheap hack to collapse all contexts at the end of a run

        member this.coverage url ss id =
            let encodedId = jsEncode id
            if (contexts |> List.exists (fun c -> c = "Coverage Reports")) = false then
                contexts <- "Coverage Reports" :: contexts
                this.swallowedJS (sprintf "addContext('%s');" "Coverage Reports")
            this.swallowedJS (sprintf "addToContext ('%s', '%s');" "Coverage Reports" url)
            this.swallowedJS (sprintf "updateTestInContext('%s', '%s', 'Pass', '%s');" "Coverage Reports" encodedId (Convert.ToBase64String(ss)))

        member this.todo id =
            let encodedId = jsEncode id
            this.swallowedJS (sprintf "updateTestInContext('%s', '%s', 'Todo', '%s');" context encodedId "")

        member this.skip id =
            let encodedId = jsEncode id
            this.swallowedJS (sprintf "updateTestInContext('%s', '%s', 'Skip', '%s');" context encodedId "")
            consoleReporter.skip id

        member this.setEnvironment env =
            environment <- env

type JUnitReporter(resultFilePath:string) =

    let consoleReporter : IReporter = new ConsoleReporter() :> IReporter

    let testStopWatch = System.Diagnostics.Stopwatch()
    let testTimes = ResizeArray<string * float>()
    let passedTests = ResizeArray<string>()
    let failedTests = ResizeArray<Exception * string>()

    interface IReporter with

        member this.pass id =
            consoleReporter.pass id
            passedTests.Add(id)

        member this.fail ex id ss url =
            consoleReporter.fail ex id ss url
            failedTests.Add(ex, id)

        member this.describe d =
            consoleReporter.describe d

        member this.contextStart c =
            consoleReporter.contextStart c

        member this.contextEnd c =
            consoleReporter.contextEnd c

        member this.summary minutes seconds passed failed skipped =
            consoleReporter.summary minutes seconds passed failed skipped
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
            consoleReporter.write <| sprintf "Saving results to %s" resultFilePath
            let enc = new System.Text.UTF8Encoding(false)
            let bytes = enc.GetBytes xml
            use fs = new System.IO.FileStream(resultFilePath, System.IO.FileMode.OpenOrCreate)
            fs.Write(bytes, 0, bytes.Length)

        member this.write w =
            consoleReporter.write w

        member this.suggestSelectors selector suggestions =
            consoleReporter.suggestSelectors selector suggestions

        member this.testStart id =
            consoleReporter.testStart id
            testStopWatch.Reset()
            testStopWatch.Start()

        member this.testEnd id =
            testStopWatch.Stop()
            let elapsedSeconds = float testStopWatch.ElapsedMilliseconds / 1000.
            testTimes.Add(id, elapsedSeconds)

        member this.quit () = ()
        member this.suiteBegin () = ()
        member this.suiteEnd () = ()
        member this.coverage url ss _ = ()
        member this.todo _ = ()
        member this.skip id = ()
        member this.setEnvironment env = ()
