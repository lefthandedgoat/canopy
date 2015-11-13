module canopy.reporters

open System
open OpenQA.Selenium
open types

type IReporter =
   abstract member testStart : string -> unit
   abstract member pass : unit -> unit
   abstract member fail : Exception -> string -> byte [] -> string -> unit
   abstract member todo : unit -> unit
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
   abstract member coverage : string -> byte [] -> unit
   abstract member setEnvironment : string -> unit

type ConsoleReporter() =   
    interface IReporter with               
        member this.pass () = 
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
            Console.WriteLine("Stack: ");
            ex.StackTrace.Split([| "\r\n"; "\n" |], StringSplitOptions.None)
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

        member this.describe d = Console.WriteLine d          
        
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
        
        member this.write w = Console.WriteLine w        
        
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

        member this.coverage url ss = ()
        
        member this.todo () = ()

        member this.skip id =
            Console.ForegroundColor <- ConsoleColor.Yellow
            Console.WriteLine("Skipped");
            Console.ResetColor()

        member this.setEnvironment env = ()

type TeamCityReporter() =

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
        member this.pass () = consoleReporter.pass ()
    
        member this.fail ex id ss url =
            let mutable image = ""
            if not (Array.isEmpty ss) then
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
        
        member this.coverage url ss = ()

        member this.todo () = ()

        member this.skip id = teamcityReport (sprintf "testIgnored name='%s'" (tcFriendlyMessage id))

        member this.setEnvironment env = ()

type LiveHtmlReporter(browser : BrowserStartMode, driverPath : string) =
    let consoleReporter : IReporter = new ConsoleReporter() :> IReporter
    
    let _browser =    
        //copy pasta!
        match browser with
        | IE -> new OpenQA.Selenium.IE.InternetExplorerDriver(driverPath) :> IWebDriver
        | IEWithOptions options ->new OpenQA.Selenium.IE.InternetExplorerDriver(driverPath, options) :> IWebDriver
        | IEWithOptionsAndTimeSpan(options, timeSpan) -> new OpenQA.Selenium.IE.InternetExplorerDriver(driverPath, options, timeSpan) :> IWebDriver
        | Chrome -> 
                let options = OpenQA.Selenium.Chrome.ChromeOptions()
                options.AddArgument("test-type") //https://code.google.com/p/chromedriver/issues/detail?id=799
                new OpenQA.Selenium.Chrome.ChromeDriver(driverPath, options) :> IWebDriver
        | ChromeWithOptions options -> new OpenQA.Selenium.Chrome.ChromeDriver(driverPath, options) :> IWebDriver        
        | ChromeWithOptionsAndTimeSpan(options, timeSpan) -> new OpenQA.Selenium.Chrome.ChromeDriver(driverPath, options, timeSpan) :> IWebDriver
        | Firefox -> new OpenQA.Selenium.Firefox.FirefoxDriver() :> IWebDriver
        | FirefoxWithProfile profile -> new OpenQA.Selenium.Firefox.FirefoxDriver(profile) :> IWebDriver
        | FirefoxWithPath path -> new OpenQA.Selenium.Firefox.FirefoxDriver(new Firefox.FirefoxBinary(path), Firefox.FirefoxProfile()) :> IWebDriver
        | ChromeWithUserAgent userAgent -> raise(System.Exception("Sorry ChromeWithUserAgent can't be used for LiveHtmlReporter"))
        | FirefoxWithUserAgent userAgent -> raise(System.Exception("Sorry FirefoxWithUserAgent can't be used for LiveHtmlReporter"))
        | FirefoxWithPathAndTimeSpan(path, timespan) -> new OpenQA.Selenium.Firefox.FirefoxDriver(new Firefox.FirefoxBinary(path), Firefox.FirefoxProfile(), timespan) :> IWebDriver
        | Safari -> new OpenQA.Selenium.Safari.SafariDriver() :> IWebDriver
        | PhantomJS | PhantomJSProxyNone -> raise(System.Exception("Sorry PhantomJS can't be used for LiveHtmlReporter"))
        | Remote(_,_) -> raise(System.Exception("Sorry Remote can't be used for LiveHtmlReporter"))
                
    let pin () =   
        let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
        let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
        let maxWidth = w / 2    
        _browser.Manage().Window.Size <- new System.Drawing.Size(maxWidth,h);        
        _browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 0),0);
    do pin()
       
    let mutable context = System.String.Empty
    let mutable test = System.String.Empty
    let mutable canQuit = false
    let mutable contexts : string list = []
    let mutable environment = System.String.Empty
    let testStopWatch = System.Diagnostics.Stopwatch()
    let contextStopWatch = System.Diagnostics.Stopwatch()
    let jsEncode value = System.Web.HttpUtility.JavaScriptStringEncode(value)

    new() = LiveHtmlReporter(Firefox, String.Empty)
    new(browser : BrowserStartMode) = LiveHtmlReporter(browser, String.Empty)

    member this.browser
        with get () = _browser

    member val reportPath = None with get, set
    member val reportTemplateUrl = @"http://lefthandedgoat.github.com/canopy/reporttemplate.html" with get, set
    member this.js script = (_browser :?> IJavaScriptExecutor).ExecuteScript(script)
    member this.reportHtml () = (this.js "return $('*').html();").ToString()
    member private this.swallowedJS script = try (_browser :?> IJavaScriptExecutor).ExecuteScript(script) |> ignore with | ex -> ()
    member this.saveReportHtml directory filename =    
        if not <| System.IO.Directory.Exists(directory) 
            then System.IO.Directory.CreateDirectory(directory) |> ignore
        IO.File.WriteAllText(System.IO.Path.Combine(directory,filename + ".html"), this.reportHtml())
    
    interface IReporter with               
        member this.pass () =
            this.swallowedJS (sprintf "updateTestInContext('%s', 'Pass', '%s');" context "")
            consoleReporter.pass ()

        member this.fail ex id ss url =
            this.swallowedJS (sprintf "updateTestInContext('%s', 'Fail', '%s');" context (Convert.ToBase64String(ss)))
            let stack = sprintf "%s%s%s" ex.Message System.Environment.NewLine ex.StackTrace
            let stack = jsEncode stack
            this.swallowedJS (sprintf "addStackToTest ('%s', '%s');" context stack)
            this.swallowedJS (sprintf "addUrlToTest ('%s', '%s');" context url)
            consoleReporter.fail ex id ss url

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
            test <- jsEncode id
            this.swallowedJS (sprintf "addToContext ('%s', '%s');" context test)
            consoleReporter.testStart id
            
        member this.testEnd id =
            testStopWatch.Stop()
            let ellapsed = testStopWatch.Elapsed
            this.swallowedJS (sprintf "addTimeToTest ('%s', '%im %is');" context ellapsed.Minutes ellapsed.Seconds)

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

        member this.coverage url ss = 
            if (contexts |> List.exists (fun c -> c = "Coverage Reports")) = false then
                contexts <- "Coverage Reports" :: contexts
                this.swallowedJS (sprintf "addContext('%s');" "Coverage Reports")
            this.swallowedJS (sprintf "addToContext ('%s', '%s');" "Coverage Reports" url)
            this.swallowedJS (sprintf "updateTestInContext('%s', 'Pass', '%s');" "Coverage Reports" (Convert.ToBase64String(ss)))

        member this.todo () = 
            this.swallowedJS (sprintf "updateTestInContext('%s', 'Todo', '%s');" context "")

        member this.skip id = 
            this.swallowedJS (sprintf "updateTestInContext('%s', 'Skip', '%s');" context "")
            consoleReporter.skip id

        member this.setEnvironment env =
            environment <- env