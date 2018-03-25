module canopy.parallell.instanced

open canopy.parallell.functions
open OpenQA.Selenium

type Instance() =
    let mutable _browser : IWebDriver = null

    member x.browser
      with get () = if _browser <> null then _browser else failwith "You must start a browser for Instance Mode"
      and set (value) = _browser <- value
    
    member x.screenshot directory filename = screenshot directory filename x.browser 

    member x.js script = js script x.browser

    member x.sleep sleep seconds = sleep seconds

    member x.puts text = puts text x.browser
    
    member x.highlight cssSelector = highlight cssSelector x.browser

    member x.describe text = describe text x.browser



    


    
    
    
    
    
    
    
    
    member x.elements cssSelector = elements cssSelector x.browser
    
    member x.element cssSelector = element cssSelector x.browser

    member x.unreliableElements cssSelector = unreliableElements cssSelector x.browser
    
    member x.unreliableElement cssSelector = unreliableElement cssSelector x.browser
    
    member x.elementWithin cssSelector elem = elementWithin cssSelector elem x.browser
    
    member x.elementsWithText cssSelector regex = elementsWithText cssSelector regex x.browser
    
    member x.elementWithText cssSelector regex = elementWithText cssSelector regex x.browser
    
    member x.parent elem = parent elem x.browser
    
    member x.elementsWithin cssSelector elem = elementsWithin cssSelector elem x.browser
    
    member x.unreliableElementsWithin cssSelector elem = unreliableElementsWithin cssSelector elem x.browser
    
    member x.someElement cssSelector = someElement cssSelector x.browser
    
    member x.someElementWithin cssSelector elem = someElementWithin cssSelector elem x.browser
    
    member x.someParent elem = someParent elem x.browser
    
    member x.nth index cssSelector = nth index cssSelector x.browser
    
    member x.first cssSelector = first cssSelector x.browser
    
    member x.last cssSelector = last cssSelector x.browser
    
    member x.write text item = write text item x.browser
    
    member x.read item = read item x.browser
    
    member x.clear item = clear item x.browser
    
    member x.press key = press key x.browser
    
    member x.alert () = alert x.browser
    
    member x.acceptAlert () = acceptAlert x.browser
    
    member x.dismissAlert () = dismissAlert x.browser
    
    member x.fastTextFromCSS selector = fastTextFromCSS selector x.browser
    
    member x.click item = click item x.browser
    
    member x.doubleClick item = doubleClick item x.browser
    
    member x.ctrlClick item = ctrlClick item x.browser
    
    member x.shiftClick item = shiftClick item x.browser
    
    member x.rightClick item = rightClick item x.browser
    
    member x.check item = check item x.browser
    
    member x.uncheck item = uncheck item x.browser
    
    member x.hover item = hover item x.browser
    
    member x.drag cssSelectorA cssSelectorB = drag cssSelectorA cssSelectorB x.browser
    
    member x.pin direction = pin direction x.browser
    
    member x.pinToMonitor number = pinToMonitor number x.browser
    
    member x.switchToTab number = switchToTab number x.browser
    
    member x.closeTab number = closeTab number x.browser
    
    member x.positionBrowser left top width height = positionBrowser left top width height x.browser
    
    member x.resize size = resize size x.browser
    
    member x.rotate () = rotate x.browser
    
    member x.currentUrl () = currentUrl x.browser
    
    member x.onn url = onn url x.browser
    
    member x.on url = on url x.browser
    
    member x.title () = title x.browser
            
    member x.url url' = url url' x.browser
    
    member x.reload () = reload x.browser
    
    member x.navigate direction = navigate x.browser
    
    member x.waitForElement cssSelector = waitForElement cssSelector x.browser

    member x.quit () = quit x.browser

    member x.start browserStartMode = x.browser <- start browserStartMode

    member x.equals item value = equals item value x.browser