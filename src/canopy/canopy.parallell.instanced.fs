module canopy.parallell.instanced

open canopy.parallell.functions
open OpenQA.Selenium

type Instance() =
    let mutable _browser : IWebDriver = null
    
    let mutable browsers : IWebDriver list = []

    member x.browser
      with get () = if _browser <> null then _browser else failwith "You must start a browser for Instance Mode"
      and set (value) = _browser <- value
       

    member x.firefox = firefox
    
    member x.aurora = aurora
    
    member x.ie = ie
    
    member x.edgeBETA = edgeBETA
    
    member x.chrome = chrome
    
    member x.chromium = chromium 
    
    member x.safari = safari

    member x.screenshot directory filename = screenshot directory filename x.browser 

    member x.js script = js script x.browser

    member x.sleep seconds = sleep seconds

    member x.puts text = puts text x.browser
    
    member x.highlight cssSelector = highlight cssSelector x.browser

    member x.describe text = describe text x.browser
  
    member x.waitFor2 message f = waitFor2 message f

    member x.waitFor = waitFor2 "Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout"

    member x.waitForElement cssSelector = waitForElement cssSelector x.browser

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

    //read/write
    member x.write item text = write item text x.browser

    member x.read item = read item x.browser

    member x.clear item = clear item x.browser

    //status
    member x.selected item = selected item x.browser

    member x.deselected item = deselected item x.browser

    //keyboard
    member x.tab = tab
    
    member x.enter = enter
    
    member x.down = down
    
    member x.up = up
    
    member x.left = left
    
    member x.right = right
    
    member x.esc = esc
        
    member x.press key = press key x.browser

    //alerts
    member x.alert() = alert x.browser

    member x.acceptAlert() = acceptAlert x.browser

    member x.dismissAlert() = dismissAlert x.browser

    member x.fastTextFromCSS selector = fastTextFromCSS selector x.browser

    //assertions
    member x.equals item value = equals item value x.browser

    member x.notEquals cssSelector value = notEquals cssSelector value x.browser 

    member x.oneOrManyEquals cssSelector value = oneOrManyEquals cssSelector value x.browser

    member x.noneOfManyNotEquals cssSelector value = noneOfManyNotEquals cssSelector value x.browser

    member x.contains (value1 : string) (value2 : string) = contains value1 value2

    member x.containsInsensitive (value1 : string) (value2 : string) = containsInsensitive value1 value2

    member x.notContains (value1 : string) (value2 : string) = notContains value1 value2

    member x.count cssSelector count' = count cssSelector count' x.browser

    member x.regexEquals cssSelector pattern = regexEquals cssSelector pattern x.browser

    member x.regexNotEquals cssSelector pattern = regexNotEquals cssSelector pattern x.browser
 
    member x.oneOrManyRegexEquals cssSelector pattern = oneOrManyRegexEquals cssSelector pattern x.browser

    member x.is expected actual = is expected actual 
    
    member x.displayed item = displayed item x.browser

    member x.notDisplayed item = notDisplayed item x.browser
   
    member x.enabled item = enabled item x.browser

    member x.disabled item = disabled item x.browser

    member x.fadedIn cssSelector = fadedIn cssSelector x.browser

    //clicking/checking
    member x.click item = click item x.browser

    member x.doubleClick item = doubleClick item x.browser

    member x.ctrlClick item = ctrlClick item x.browser

    member x.shiftClick item = shiftClick item x.browser

    member x.rightClick item = rightClick item x.browser

    member x.check item = check item x.browser

    member x.uncheck item = uncheck item x.browser

    //hoverin
    member x.hover selector = hover selector x.browser

    //draggin
    member x.drag cssSelectorA cssSelectorB = drag cssSelectorA cssSelectorB x.browser

    //browser related
    member x.pin direction = pin direction x.browser
    
    member x.start b =
        x.browser <- start b
        browsers <- browsers @ [x.browser]

    member x.switchTo b = x.browser <- b

    member x.switchToTab number = switchToTab number x.browser

    member x.closeTab number = closeTab number x.browser

    member x.tile browsers = tile browsers

    member x.positionBrowser left top width height = positionBrowser left top width height x.browser

    member x.resize size = resize size x.browser

    member x.rotate() = rotate x.browser

    member x.quit browser =
        canopy.configuration.reporter.quit()
        match box x.browser with
        | :? OpenQA.Selenium.IWebDriver as b -> b.Quit()
        | _ -> browsers |> List.iter (fun b -> b.Quit())

    member x.currentUrl() = currentUrl x.browser

    member x.onn (u: string) = onn u x.browser

    member x.on (u: string) = on u x.browser
    
    member x.url u = url u x.browser

    member x.title() = title x.browser

    member x.reload () = reload x.browser

    member x.back = back
    
    member x.forward = forward

    member x.navigate direction = navigate x.browser direction

    member x.addFinder finder = addFinder finder x.browser

    //hints
    member x.css = css
    
    member x.xpath = xpath
    
    member x.jquery = jquery
    
    member x.label = label
    
    member x.text = text
    
    member x.value = value

    member x.skip message = skip