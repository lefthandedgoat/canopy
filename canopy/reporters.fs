module reporters

open System

type IReporter =
   abstract member pass : unit -> unit
   abstract member fail : Exception -> unit
   abstract member describe : string -> unit
   abstract member context : string -> unit
   abstract member summary : int -> int -> int -> unit
   abstract member write : string -> unit
   abstract member suggestSelectors : string -> string list -> unit

type ConsoleReporter() =
    interface IReporter with       
        member this.pass () = 
            Console.ForegroundColor <- ConsoleColor.Green
            Console.WriteLine("Passed");
            Console.ResetColor()

        member this.fail ex = 
            Console.ForegroundColor <- ConsoleColor.Red
            Console.WriteLine("Error: ");
            Console.ResetColor()
            Console.WriteLine(ex.Message);

        member this.describe d = Console.WriteLine d
          
        member this.context c = Console.WriteLine (String.Format("context: {0}", c))

        member this.summary seconds passed failed =
            Console.WriteLine()
            Console.WriteLine("{0} seconds to execute", seconds)
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