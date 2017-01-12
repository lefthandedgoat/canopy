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

    //assertions
    static member equals selector value = selector == value

    static member count selector value = count selector value