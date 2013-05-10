module canopy.reporters

open System
open OpenQA.Selenium

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
            Console.WriteLine(ex.StackTrace);

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
            Console.WriteLine("Couldnt find any elements with selector '{0}', did you mean:", selector)
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

type HtmlReporter() =
    let consoleReporter : IReporter = new ConsoleReporter() :> IReporter    
    let mutable results = String.Empty;
    let mutable html = 
            "<html>
            <!DOCTYPE html>
            <head>
                <title>canopy html report</title>
                <style type='text/css'>
                    div
                    {
                        border-style: solid;
                        border-width: 1px;
                    }
                    div.passed, div.passedContext
                    {
                        border-color: Green;
                        border-width: 3px;
                    }
                    div.failed, div.failedContext
                    {
                        border-color: Red;
                        border-width: 3px;
                    }
                    div.skipped, div.skippedContext
                    {
                        border-color: Orange;
                        border-width: 3px;
                    }
                </style>
            </head>
            <body>
                <div id='report'>
                    <div id='top'>
                        <div id='checkboxes'>
                            <input type='checkbox' id='passed' checked='checked'/>Passed
                            <input type='checkbox' id='failed' checked='checked'/>Failed
                            <input type='checkbox' id='skipped' checked='checked'/>Skipped
                        </div>
                        <div id='summary'>
                            <span>{{total}} total</span> <span>{{passed}} passed</span> <span>{{failed}} failed</span> <span>{{skipped}} skipped</span>
                        </div>
                        <div id='time'>
                            <span>{{minutes}} minutes</span>
                            <span>{{seconds}} seconds</span>
                        </div>
                    </div>
                    <div id='results'>
                        {{results}}
                    </div>
                </div>
                <script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js'></script>
                <script type='text/javascript'>
                    $('#passed').change(function () {
                        if ($(this).is(':checked')) {
                            $('.passed').show();
                        } else {
                            $('.passed').hide();
                        }
                    });
                    $('#failed').change(function () {
                        if ($(this).is(':checked')) {
                            $('.failed').show();
                        } else {
                            $('.failed').hide();
                        }
                    });
                    $('#skipped').change(function () {
                        if ($(this).is(':checked')) {
                            $('.skipped').show();
                        } else {
                            $('.skipped').hide();
                        }
                    });
                </script>
            </body>
            </html>"
    
    interface IReporter with               
        member this.pass () =consoleReporter.pass ()

        member this.fail ex id ss= consoleReporter.fail ex id ss

        member this.describe d = consoleReporter.describe d
          
        member this.contextStart c = consoleReporter.contextStart c

        member this.contextEnd c = consoleReporter.contextEnd c

        member this.summary minutes seconds passed failed =
            html <- html.Replace("{{minutes}}", minutes.ToString())
            html <- html.Replace("{{seconds}}", seconds.ToString())
            html <- html.Replace("{{total}}", (passed + failed).ToString())
            html <- html.Replace("{{passed}}", passed.ToString())
            html <- html.Replace("{{failed}}", failed.ToString())
            html <- html.Replace("{{results}}", results.ToString())
            let p = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\canopy\" + DateTime.Now.ToString("MMM-d_HH-mm-ss-fff") + ".html"
            using (new System.IO.StreamWriter(p)) (fun writer ->
                writer.Write(html);
            )
            consoleReporter.summary minutes seconds passed failed
        
        member this.write w = consoleReporter.write w
        
        member this.suggestSelectors selector suggestions = consoleReporter.suggestSelectors selector suggestions

        member this.testStart id = 
            consoleReporter.testStart id
            results <- String.Format("{0}</br>{1}", results, id)

        member this.testEnd id = ()

        member this.quit () = ()
        
        member this.suiteBegin () = ()

        member this.suiteEnd () = ()
        
        member this.coverage url ss = ()

        member this.todo () = ()

        member this.skip () = ()

type LiveHtmlReporter() =
    let consoleReporter : IReporter = new ConsoleReporter() :> IReporter    
    let browser = new OpenQA.Selenium.Firefox.FirefoxDriver() :> IWebDriver    
    let js script = try (browser :?> IJavaScriptExecutor).ExecuteScript(script) |> ignore with | ex -> ()
    let mutable context = System.String.Empty;
    let mutable test = System.String.Empty;
    let mutable canQuit = false
    let mutable contexts : string list = []

    interface IReporter with               
        member this.pass () =
            js (sprintf "addToContext('%s', 'Pass', '%s', '%s');" context test "")
            consoleReporter.pass ()

        member this.fail ex id ss =
            js (sprintf "addToContext('%s', 'Fail', '%s', '%s');" context test (Convert.ToBase64String(ss)))
            consoleReporter.fail ex id ss

        member this.describe d = 
            consoleReporter.describe d
          
        member this.contextStart c = 
            contexts <- c :: contexts
            context <- c
            js (sprintf "addContext('%s');" context)
            js (sprintf "collapseContextsExcept('%s');" context)
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
            test <- id
            consoleReporter.testStart id
            
        member this.testEnd id = ()

        member this.quit () = if canQuit then browser.Quit()
        
        member this.suiteBegin () = browser.Navigate().GoToUrl(@"http://lefthandedgoat.github.com/canopy/reporttemplate.html")
        //member this.suiteBegin () = browser.Navigate().GoToUrl(@"file:///C:/projects/canopy/reporttemplate.html")

        member this.suiteEnd () = 
            canQuit <- true
            js (sprintf "collapseContextsExcept('%s');" "") //cheap hack to collapse all contexts at the end of a run

        member this.coverage url ss = 
            if (contexts |> List.exists (fun c -> c = "Coverage Reports")) = false then
                contexts <- "Coverage Reports" :: contexts
                js (sprintf "addContext('%s');" "Coverage Reports")
            js (sprintf "addToContext('%s', 'Pass', '%s', '%s');" "Coverage Reports" url (Convert.ToBase64String(ss)))

        member this.todo () = 
            js (sprintf "addToContext('%s', 'Todo', '%s', '%s');" context test "")

        member this.skip () = 
            js (sprintf "addToContext('%s', 'Skip', '%s', '%s');" context test "")