namespace Canopy.CSharp

open Canopy
open Canopy.Operators
open Canopy.Runner
open Canopy.Runner.Runner
open Canopy.Runner.Runner.Operators
open Canopy.Expect
open Canopy.Expect.Operators

type Canopy() =

    static member browsers =
        Context.getContext().browsers

    static member browser =
        match Context.getContext().browser with
        | Some browser ->
            browser
        | None ->
            failwith "Canopy is not yet configured with a browser"

    //runner stuff
    static member context description = Runner.context description

    static member once (f: System.Action) = Runner.once (fun _ -> f.Invoke())

    static member before (f : System.Action) = Runner.before (fun _ -> f.Invoke())

    static member after (f : System.Action) = Runner.after (fun _ -> f.Invoke())

    static member lastly (f : System.Action) = Runner.lastly (fun _ -> f.Invoke())

    static member onPass (f : System.Action) = Runner.onPass (fun _ -> f.Invoke())

    static member onFail (f : System.Action) = Runner.onFail (fun _ -> f.Invoke())

    static member test description (f : System.Action) = description &&& fun _ -> f.Invoke()

    static member wip description (f : System.Action) = description &&&& fun _ -> f.Invoke()

    static member skip description (f : System.Action) = description &&! CanopyRunnerConfig.skipped

    static member run () = Runner.run ()

    static member runFor browsers = Runner.runFor browsers

    //core stuff
    static member start (config, b) = startWithConfig config b
    static member start b = start b

    static member pin direction = pin direction

    static member pinToMonitor direction = pinToMonitor direction

    static member switchTo b = switchTo b

    static member switchToTab number = switchToTab number

    static member closeTab number = closeTab number

    static member tile browsers = tile browsers

    static member positionBrowser left top width height = positionBrowser left top width height

    static member resize size = resize size

    static member rotate () = rotate ()

    static member url destination = url destination

    static member currentUrl () = currentUrl ()

    static member addFinder finder = addFinder finder

    static member title () = title ()

    static member reload () = reload ()

    static member navigate direction = navigate direction

    static member quit () = quit ()

    static member screenshot directory filename = screenshot directory filename

    static member js script = js script

    static member sleep seconds = sleep seconds

    static member puts text = puts text

    static member highlight cssSelector = highlight cssSelector

    static member describe text = describe text

    static member waitForMessage message (f: System.Predicate<obj>) =
        waitForMessage message (fun _ -> f.Invoke())

    static member waitFor2 message (f: System.Predicate<obj>) =
        waitForMessage message (fun _ -> f.Invoke())

    static member waitFor (f : System.Predicate<obj>) =
        waitFor (fun _ -> f.Invoke())

    static member waitForElement selector = waitForElement selector

    //element stuff
    static member unreliableElement selector = unreliableElement selector

    static member unreliableElements selector = unreliableElements selector

    static member element selector = element selector

    static member elements selector = elements selector

    static member elementWithin selector element = elementWithin selector element

    static member elementsWithin selector element = elementsWithin selector element

    static member elementWithText selector regex = elementWithText selector regex

    static member elementsWithText selector regex = elementsWithText selector regex

    static member parent element = parent element

    static member unreliableElementsWithin selector element =
        unreliableElementsWithin selector element

    static member someElement selector =
      match someElement selector with
      | None -> null
      | Some element -> element

    static member someElementWithin selector element =
      match someElementWithin selector element with
      | None -> null
      | Some element -> element

    static member someParent element =
      match someParent element with
      | None -> null
      | Some element -> element

    static member nth index selector = nth index selector

    static member first selector = first selector

    static member last index selector = last selector

    static member read selector = read selector

    static member clear selector = clear selector

    static member write selector value = write value selector

    static member click selector = click selector

    static member doubleClick selector = doubleClick selector

    static member ctrlClick selector = ctrlClick selector

    static member shiftClick selector = shiftClick selector

    static member rightClick selector = rightClick selector

    static member check selector = check  selector

    static member uncheck selector = uncheck selector

    static member hover selector = hover selector

    static member drag selector = drag selector

    static member press key = press key

    static member alert () = alert ()

    static member acceptAlert () = acceptAlert ()

    static member dismissAlert () = dismissAlert ()

    static member fastTextFromCSS selector = fastTextFromCSS selector

    //assertions
    static member eq selector value = selector == value

    static member notEq selector value = selector != value

    static member starEq selector value = selector *= value

    static member starNotEq selector value = selector *!= value

    static member starEqRegex selector value = selector *~ value

    static member eqRegex selector value = selector =~ value

    static member notEqRegex selector value = selector !=~ value

    static member contains value1 value2 = contains value1 value2

    static member notContains value1 value2 = notContains value1 value2

    static member selected selector = selected selector

    static member deselected selector = deselected selector

    static member displayed selector = displayed selector

    static member notDisplayed selector = notDisplayed selector

    static member enabled selector = enabled selector

    static member disabled selector = disabled selector

    static member fadedIn selector = fadedIn selector

    static member equality value1 value2 = value1 === value2

    static member count selector value = count selector value

    static member onn url = onn url

    static member on url = on url