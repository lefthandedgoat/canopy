module canopy.reporters

open System
open OpenQA.Selenium
open types

type IReporter =
   abstract member testStart : string -> unit
   abstract member pass : unit -> unit
   abstract member fail : Exception -> string -> byte [] -> unit
   abstract member todo : unit -> unit
   abstract member skip : unit -> unit
   abstract member testEnd : string -> unit
   abstract member describe : string -> unit
   abstract member contextStart : string -> unit
   abstract member contextEnd : string -> unit
   abstract member summary : int -> int -> int -> int -> unit
   abstract member write : string -> unit
   abstract member suggestSelectors : string -> string list -> unit
   abstract member quit : unit -> unit
   abstract member suiteBegin : unit -> unit
   abstract member suiteEnd : unit -> unit
   abstract member coverage : string -> byte [] -> unit

type ConsoleReporter() =   
    interface IReporter with               
        member this.pass () = 
            Console.ForegroundColor <- ConsoleColor.Green
            Console.WriteLine("Passed");
            Console.ResetColor()

        member this.fail ex id ss = 
            Console.ForegroundColor <- ConsoleColor.Red
            Console.WriteLine("Error: ");
            Console.ResetColor()
            Console.WriteLine(ex.Message);
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
        
        member this.summary minutes seconds passed failed =
            Console.WriteLine()
            Console.WriteLine("{0} minutes {1} seconds to execute", minutes, seconds)
            if failed = 0 then
                Console.ForegroundColor <- ConsoleColor.Green
            Console.WriteLine("{0} passed", passed)
            Console.ResetColor()
            if failed > 0 then
                Console.ForegroundColor <- ConsoleColor.Red        
            Console.WriteLine("{0} failed", failed)    
            Console.ResetColor()        
        
        member this.write w = Console.WriteLine w        
        
        member this.suggestSelectors selector suggestions = 
            Console.ForegroundColor <- ConsoleColor.DarkYellow                    
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

        member this.skip () = ()

type TeamCityReporter() =
    let consoleReporter : IReporter = new ConsoleReporter() :> IReporter
        
    interface IReporter with               
        member this.pass () = consoleReporter.pass ()
    
        member this.fail ex id ss =         
            consoleReporter.describe (String.Format("##teamcity[testFailed name='{0}' message='{1}']", id, ex.Message))
            consoleReporter.fail ex id ss
    
        member this.describe d = 
            consoleReporter.describe (String.Format("##teamcity[message text='{0}' status='NORMAL']", d))
            consoleReporter.describe d          
    
        member this.contextStart c = 
            consoleReporter.describe (String.Format("##teamcity[testSuiteStarted name='{0}']", c))
            consoleReporter.contextStart c
    
        member this.contextEnd c = 
            consoleReporter.describe (String.Format("##teamcity[testSuiteFinished name='{0}']", c))
            consoleReporter.contextEnd c
    
        member this.summary minutes seconds passed failed =consoleReporter.summary minutes seconds passed failed        
    
        member this.write w = consoleReporter.write w        
    
        member this.suggestSelectors selector suggestions = consoleReporter.suggestSelectors selector suggestions
    
        member this.testStart id = consoleReporter.describe (String.Format("##teamcity[testStarted name='{0}']", id))
    
        member this.testEnd id = consoleReporter.describe (String.Format("##teamcity[testFinished name='{0}']", id))

        member this.quit () = ()
        
        member this.suiteBegin () = ()

        member this.suiteEnd () = ()
        
        member this.coverage url ss = ()

        member this.todo () = ()

        member this.skip () = ()

type LiveHtmlReporter(browser : BrowserStartMode, driverPath : string) =
    let consoleReporter : IReporter = new ConsoleReporter() :> IReporter
    
    let _browser =    
        //copy pasta!
        match browser with
        | IE -> new OpenQA.Selenium.IE.InternetExplorerDriver(driverPath) :> IWebDriver
        | IEWithOptions options ->new OpenQA.Selenium.IE.InternetExplorerDriver(driverPath, options) :> IWebDriver
        | IEWithOptionsAndTimeSpan(options, timeSpan) -> new OpenQA.Selenium.IE.InternetExplorerDriver(driverPath, options, timeSpan) :> IWebDriver
        | Chrome -> new OpenQA.Selenium.Chrome.ChromeDriver(driverPath) :> IWebDriver
        | ChromeWithOptions options -> new OpenQA.Selenium.Chrome.ChromeDriver(driverPath, options) :> IWebDriver        
        | ChromeWithOptionsAndTimeSpan(options, timeSpan) -> new OpenQA.Selenium.Chrome.ChromeDriver(driverPath, options, timeSpan) :> IWebDriver
        | Firefox -> new OpenQA.Selenium.Firefox.FirefoxDriver() :> IWebDriver
        | FirefoxWithProfile profile -> new OpenQA.Selenium.Firefox.FirefoxDriver(profile) :> IWebDriver
        | FirefoxWithPath path -> new OpenQA.Selenium.Firefox.FirefoxDriver(Firefox.FirefoxBinary(path), Firefox.FirefoxProfile()) :> IWebDriver
        | ChromeWithUserAgent userAgent -> raise(System.Exception("Sorry ChromeWithUserAgent can't be used for LiveHtmlReporter"))
        | FirefoxWithUserAgent userAgent -> raise(System.Exception("Sorry FirefoxWithUserAgent can't be used for LiveHtmlReporter"))
        | PhantomJS | PhantomJSProxyNone -> raise(System.Exception("Sorry PhantomJS can't be used for LiveHtmlReporter"))
                
    let pin () =   
        let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
        let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
        let maxWidth = w / 2    
        _browser.Manage().Window.Size <- new System.Drawing.Size(maxWidth,h);        
        _browser.Manage().Window.Position <- new System.Drawing.Point((maxWidth * 0),0);
    do pin()
       
    let mutable context = System.String.Empty;
    let mutable test = System.String.Empty;
    let mutable canQuit = false
    let mutable contexts : string list = []

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
            this.swallowedJS (sprintf "addToContext('%s', 'Pass', '%s', '%s');" context test "")
            consoleReporter.pass ()

        member this.fail ex id ss =
            this.swallowedJS (sprintf "addToContext('%s', 'Fail', '%s', '%s');" context test (Convert.ToBase64String(ss)))
            consoleReporter.fail ex id ss

        member this.describe d = 
            consoleReporter.describe d
          
        member this.contextStart c = 
            contexts <- c :: contexts
            context <- System.Web.HttpUtility.JavaScriptStringEncode(c)
            this.swallowedJS (sprintf "addContext('%s');" context)
            this.swallowedJS (sprintf "collapseContextsExcept('%s');" context)
            consoleReporter.contextStart c

        member this.contextEnd c = 
            consoleReporter.contextEnd c

        member this.summary minutes seconds passed failed =                        
            consoleReporter.summary minutes seconds passed failed
        
        member this.write w = 
            consoleReporter.write w
        
        member this.suggestSelectors selector suggestions = 
            consoleReporter.suggestSelectors selector suggestions

        member this.testStart id = 
            test <- System.Web.HttpUtility.JavaScriptStringEncode(id)
            consoleReporter.testStart id
            
        member this.testEnd id = ()

        member this.quit () = 
          match this.reportPath with
            | Some(path) ->
              let reportFileInfo = new IO.FileInfo(path)
              this.saveReportHtml reportFileInfo.Directory.FullName reportFileInfo.Name
            | None -> consoleReporter.write "Not saving report"

          if canQuit then _browser.Quit()
        
        member this.suiteBegin () = _browser.Navigate().GoToUrl(this.reportTemplateUrl)

        member this.suiteEnd () = 
            canQuit <- true
            this.swallowedJS (sprintf "collapseContextsExcept('%s');" "") //cheap hack to collapse all contexts at the end of a run

        member this.coverage url ss = 
            if (contexts |> List.exists (fun c -> c = "Coverage Reports")) = false then
                contexts <- "Coverage Reports" :: contexts
                this.swallowedJS (sprintf "addContext('%s');" "Coverage Reports")
            this.swallowedJS (sprintf "addToContext('%s', 'Pass', '%s', '%s');" "Coverage Reports" url (Convert.ToBase64String(ss)))

        member this.todo () = 
            this.swallowedJS (sprintf "addToContext('%s', 'Todo', '%s', '%s');" context test "")

        member this.skip () = 
            this.swallowedJS (sprintf "addToContext('%s', 'Skip', '%s', '%s');" context test "")