namespace Canopy

open OpenQA.Selenium

/// Runtime state for parallel support; you can access `.conf` for a configuration
/// value of your choice.
type Context<'config> =
    {
        conf: 'config
        browser: IWebDriver
    }

/// Module for manipulating the `Context` value.
module Context =
    /// Creates a new Context object, with a `conf` value of your choice and a
    /// browser.
    let create conf (browser: IWebDriver) =
        { conf = conf; browser = browser }

/// Extensions for Context<'config> to get the DSL API on the context instance;
/// this makes it easier to use the context and browser in your parallel tests.
type Context<'config> with
    member x.screenshot directory filename =
        screenshotB x.browser directory filename
    member x.js script =
        jsB x.browser script
    member x.puts text =
        putsB x.browser text
    member x.highlight cssSelector =
        highlightB x.browser cssSelector
    member x.describe text =
        describeB x.browser text
    member x.elements cssSelector =
        elementsB x.browser cssSelector
    member x.unreliableElements cssSelector =
        unreliableElementsB x.browser cssSelector
    member x.unreliableElement cssSelector =
        unreliableElementB x.browser cssSelector
    member x.elementWithin cssSelector elem =
        elementWithinB x.browser cssSelector elem
    member x.elementsWithText cssSelector regex =
        elementsWithTextB x.browser cssSelector regex
    member x.elementWithText cssSelector regex =
        elementWithTextB x.browser cssSelector regex
    member x.parent elem =
        parentB x.browser elem
    member x.elementsWithin cssSelector elem =
        elementsWithinB x.browser cssSelector elem
    member x.unreliableElementsWithin cssSelector elem =
        unreliableElementsWithinB x.browser cssSelector elem
    member x.someElement cssSelector =
        someElementB x.browser cssSelector
    member x.someElementWithin cssSelector elem =
        someElementWithinB x.browser cssSelector elem
    member x.someParent elem =
        someParentB x.browser elem
    member x.nth index cssSelector =
        nthB x.browser index cssSelector
    member x.item index cssSelector =
        itemB x.browser index cssSelector
    member x.first cssSelector =
        firstB x.browser cssSelector
    member x.last cssSelector =
        lastB x.browser cssSelector
    member x.write text item =
        writeB x.browser text item
    member x.read item =
        readB x.browser item
    member x.clear item =
        clearB x.browser item
    member x.press key =
        pressB x.browser key
    member x.alert () =
        alertB x.browser
    member x.acceptAlert () =
        acceptAlertB x.browser
    member x.dismissAlert () =
        dismissAlertB x.browser
    member x.fastTextFromCSS selector =
        fastTextFromCSSB x.browser selector
    member x.click item =
        clickB x.browser item
    member x.doubleClick item =
        doubleClickB x.browser item
    member x.modifierClick modifier item =
        modifierClickB x.browser modifier item
    member x.ctrlClick item =
        ctrlClickB x.browser item
    member x.shiftClick item =
        shiftClickB x.browser item
    member x.rightClick item =
        rightClickB x.browser item
    member x.check item =
        checkB x.browser item
    member x.uncheck item =
        uncheckB x.browser item
    member x.hover item =
        hoverB x.browser item
    member x.drag cssSelectorA cssSelectorB =
        dragB x.browser cssSelectorA cssSelectorB
    member x.pin direction =
        pinB x.browser direction
    member x.pinToMonitor number =
        pinToMonitorB x.browser number
    member x.switchToTab number =
        switchToTabB x.browser number
    member x.closeTab number =
        closeTabB x.browser number
    member x.positionBrowser left top width height =
        positionBrowserB x.browser left top width height
    member x.resize size =
        resizeB x.browser size
    member x.rotate () =
        rotateB x.browser
    member x.currentUrl () =
        currentUrlB x.browser
    member x.onn url =
        onnB x.browser url
    member x.on url =
        onB x.browser url
    member x.title () =
        titleB x.browser
    member x.uri uri =
        uriB x.browser uri
    member x.url url =
        urlB x.browser url
    member x.reload () =
        reloadB x.browser
    member x.navigate direction =
        navigateB x.browser direction
    member x.waitForElement cssSelector =
        waitForElementB x.browser cssSelector
