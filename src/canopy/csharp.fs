namespace canopy.csharp

open canopy.runner
open canopy

type canopy () =

    static member browsers = canopy.core.browsers

    static member browser = canopy.types.browser

    //runner stuff
    static member context description = context description

    static member once (f : System.Action) = once (fun _ -> f.Invoke())

    static member before (f : System.Action) = before (fun _ -> f.Invoke())

    static member after (f : System.Action) = after (fun _ -> f.Invoke())

    static member lastly (f : System.Action) = lastly (fun _ -> f.Invoke())

    static member test description (f : System.Action) = description &&& fun _ -> f.Invoke()

    static member wip description (f : System.Action) = description &&&& fun _ -> f.Invoke()

    static member skip description (f : System.Action) = description &&! canopy.runner.skipped
                
    static member run () = canopy.runner.run ()
        

    //core stuff
    static member start b = canopy.core.start b

    static member url url = canopy.core.url url

    static member quit () = canopy.core.quit ()

    //element stuff
    static member element selector = canopy.core.element selector

    static member read selector = canopy.core.read selector

    static member clear selector = canopy.core.clear selector

    static member write selector value = selector << value

    //assertions
    static member eq selector value = selector == value

    static member equality value1 value2 = value1 === value2

    static member count selector value = count selector value