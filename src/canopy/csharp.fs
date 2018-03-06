namespace canopy.csharp

open canopy.runner
open canopy
open canopy.core.Assert
open canopy.core.Assert.Operators

type canopy () =

    static member browsers = browsers

    static member browser = canopy.types.browser

    //runner stuff
    static member context description = context description

    static member once (f : System.Action) = once (fun _ -> f.Invoke())

    static member before (f : System.Action) = before (fun _ -> f.Invoke())

    static member after (f : System.Action) = after (fun _ -> f.Invoke())

    static member lastly (f : System.Action) = lastly (fun _ -> f.Invoke())

    static member onPass (f : System.Action) = onPass (fun _ -> f.Invoke())

    static member onFail (f : System.Action) = onFail (fun _ -> f.Invoke())

    static member test description (f : System.Action) = description &&& fun _ -> f.Invoke()

    static member wip description (f : System.Action) = description &&&& fun _ -> f.Invoke()

    static member skip description (f : System.Action) = description &&! canopy.runner.skipped
                
    static member run () = canopy.runner.run ()

    static member runFor browsers = canopy.runner.runFor browsers
        
    //core stuff
    static member start b = start b

    static member pin direction = pinB browser direction

    static member pinToMonitor direction = pinToMonitorB browser direction

    static member switchTo b = switchTo b

    static member switchToTab number = switchToTabB browser number

    static member closeTab number = closeTabB browser number

    static member tile browsers = tile browsers

    static member positionBrowser left top width height = positionBrowser left top width height

    static member resize size = resizeB browser size

    static member rotate () = rotateB browser

    static member url destination = url destination

    static member currentUrl () = currentUrl ()

    static member coverage url = coverage url

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

    static member waitFor2 message (f : System.Predicate<obj>) = waitFor2 message (fun _ -> f.Invoke())

    static member waitFor (f : System.Predicate<obj>) = waitFor (fun _ -> f.Invoke())

    static member waitForElement selector = waitForElement selector

    //element stuff
    static member unreliableElement selector = unreliableElementB browser selector

    static member unreliableElements selector = unreliableElementsB browser selector

    static member element selector = elementB browser selector

    static member elements selector = elementsB browser selector

    static member elementWithin selector element = elementWithinB browser selector element
    
    static member elementsWithin selector element = elementsWithinB browser selector element

    static member elementWithText selector regex = elementWithTextB browser selector regex

    static member elementsWithText selector regex = elementsWithTextB browser selector regex

    static member parent element = parentB browser element

    static member unreliableElementsWithin selector element = unreliableElementsWithinB browser selector element

    static member someElement selector = 
      match someElementB browser selector with
      | None -> null
      | Some element -> element

    static member someElementWithin selector element = 
      match someElementWithinB browser selector element with
      | None -> null
      | Some element -> element

    static member someParent element = 
      match someParentB browser element with
      | None -> null
      | Some element -> element
    
    static member nth index selector = nthB browser index selector

    static member first selector = firstB browser selector

    static member last index selector = lastB browser selector

    static member read selector = readB browser selector

    static member clear selector = clearB browser selector

    static member write selector value = selector << value

    static member click selector = clickB browser selector
    
    static member doubleClick selector = doubleClickB browser selector

    static member ctrlClick selector = ctrlClickB browser selector

    static member shiftClick selector = shiftClickB browser selector

    static member rightClick selector = rightClickB browser selector

    static member check selector = checkB browser selector

    static member uncheck selector = uncheckB browser selector
    
    static member hover selector = hoverB browser selector

    static member drag selector = dragB browser selector
               
    static member press key = pressB browser key

    static member alert () = alertB browser

    static member acceptAlert () = acceptAlertB browser

    static member dismissAlert () = dismissAlertB browser

    static member fastTextFromCSS selector = fastTextFromCSSB browser selector      

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

    static member selected selector = selectedB browser selector

    static member deselected selector = deselectedB browser selector

    static member displayed selector = displayedB browser selector

    static member notDisplayed selector = notDisplayedB browser selector

    static member enabled selector = enabledB browser selector

    static member disabled selector = disabled selector

    static member fadedIn selector = fadedIn selector

    static member equality value1 value2 = value1 === value2

    static member count selector value = countB browser selector value

    static member onn url = onn url

    static member on url = on url