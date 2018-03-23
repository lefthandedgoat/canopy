module canopy.classic.core

open canopy.types

let mutable (failureMessage : string) = null
let mutable wipTest = false
let mutable searchedFor : (string * string) list = []

(* documented/actions *)
let firefox = canopy.parallell.functions.firefox
(* documented/actions *)
let aurora = canopy.parallell.functions.aurora
(* documented/actions *)
let ie = canopy.parallell.functions.ie
(* documented/actions *)
let edgeBETA = canopy.parallell.functions.edgeBETA
(* documented/actions *)
let chrome = canopy.parallell.functions.chrome
(* documented/actions *)
let chromium = canopy.parallell.functions.chromium 
(* documented/actions *)
let safari = canopy.parallell.functions.safari

let mutable browsers = []

//misc
(* documented/actions *)
let failsWith message = failureMessage <- message

(* documented/actions *)
let screenshot directory filename = canopy.parallell.functions.screenshot directory filename browser

(* documented/actions *)
let js script = canopy.parallell.functions.js script browser

(* documented/actions *)
let sleep seconds = canopy.parallell.functions.sleep seconds

(* documented/actions *)
let puts text = canopy.parallell.functions.puts text

(* documented/actions *)
let highlight cssSelector = canopy.parallell.functions.highlight cssSelector

(* documented/actions *)
let describe text = canopy.parallell.functions.describe text browser

(* documented/actions *)
let waitFor2 message f = canopy.parallell.functions.waitFor2 message f

(* documented/actions *)
let waitFor = waitFor2 "Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout"

//get elements
(* documented/actions *)
let elements cssSelector = canopy.parallell.functions.elements cssSelector browser

(* documented/actions *)
let element cssSelector = canopy.parallell.functions.element cssSelector browser

(* documented/actions *)
let unreliableElements cssSelector = canopy.parallell.functions.unreliableElements cssSelector browser

(* documented/actions *)
let unreliableElement cssSelector = canopy.parallell.functions.unreliableElement cssSelector browser

(* documented/actions *)
let elementWithin cssSelector elem = canopy.parallell.functions.elementWithin cssSelector elem browser

(* documented/actions *)
let elementsWithText cssSelector regex = canopy.parallell.functions.elementsWithText cssSelector regex browser

(* documented/actions *)
let elementWithText cssSelector regex = canopy.parallell.functions.elementWithText cssSelector regex browser

(* documented/actions *)
let parent elem = canopy.parallell.functions.parent elem browser

(* documented/actions *)
let elementsWithin cssSelector elem = canopy.parallell.functions.elementsWithin cssSelector elem browser

(* documented/actions *)
let unreliableElementsWithin cssSelector elem = canopy.parallell.functions.unreliableElementsWithin cssSelector elem browser

(* documented/actions *)
let someElement cssSelector = canopy.parallell.functions.someElement cssSelector browser

(* documented/actions *)
let someElementWithin cssSelector elem = canopy.parallell.functions.someElementWithin cssSelector elem browser

(* documented/actions *)
let someParent elem = canopy.parallell.functions.someParent elem browser

(* documented/actions *)
let nth index cssSelector = canopy.parallell.functions.nth index cssSelector browser

(* documented/actions *)
let first cssSelector = canopy.parallell.functions.first cssSelector browser

(* documented/actions *)
let last cssSelector = canopy.parallell.functions.last cssSelector browser

//read/write

(* documented/actions *)
let ( << ) item text = canopy.parallell.functions.write item text browser

(* documented/actions *)
let read item = canopy.parallell.functions.read item browser

(* documented/actions *)
let clear item = canopy.parallell.functions.clear item browser

//status
(* documented/assertions *)
let selected item = canopy.parallell.functions.selected item browser

(* documented/assertions *)
let deselected item = canopy.parallell.functions.deselected item browser

//keyboard
(* documented/actions *)
let tab = canopy.parallell.functions.tab
(* documented/actions *)
let enter = canopy.parallell.functions.enter
(* documented/actions *)
let down = canopy.parallell.functions.down
(* documented/actions *)
let up = canopy.parallell.functions.up
(* documented/actions *)
let left = canopy.parallell.functions.left
(* documented/actions *)
let right = canopy.parallell.functions.right
(* documented/actions *)
let esc = canopy.parallell.functions.esc

(* documented/actions *)
let press key = canopy.parallell.functions.press key browser

//alerts
(* documented/actions *)
let alert() = canopy.parallell.functions.alert browser

(* documented/actions *)
let acceptAlert() = canopy.parallell.functions.acceptAlert browser

(* documented/actions *)
let dismissAlert() = canopy.parallell.functions.dismissAlert browser

(* documented/actions *)
let fastTextFromCSS selector = canopy.parallell.functions.fastTextFromCSS selector browser

//assertions
(* documented/assertions *)
let ( == ) item value = canopy.parallell.functions.equals item value browser

(* documented/assertions *)
let ( != ) cssSelector value = canopy.parallell.functions.notEquals cssSelector value browser 

(* documented/assertions *)
let ( *= ) cssSelector value = canopy.parallell.functions.oneOrManyEquals cssSelector value browser

(* documented/assertions *)
let ( *!= ) cssSelector value = canopy.parallell.functions.noneOfManyNotEquals cssSelector value browser

(* documented/assertions *)
let contains (value1 : string) (value2 : string) = canopy.parallell.functions.contains value1 value2

(* documented/assertions *)
let containsInsensitive (value1 : string) (value2 : string) = canopy.parallell.functions.containsInsensitive value1 value2

(* documented/assertions *)
let notContains (value1 : string) (value2 : string) = canopy.parallell.functions.notContains value1 value2

(* documented/assertions *)
let count cssSelector count = canopy.parallell.functions.count cssSelector count browser

(* documented/assertions *)
let ( =~ ) cssSelector pattern = canopy.parallell.functions.regexEquals cssSelector pattern browser

(* documented/assertions *)
let ( !=~ ) cssSelector pattern = canopy.parallell.functions.regexNotEquals cssSelector pattern browser
 
(* documented/assertions *)
let ( *~ ) cssSelector pattern = canopy.parallell.functions.oneOrManyRegexEquals cssSelector pattern browser

(* documented/assertions *)
let is expected actual = canopy.parallell.functions.is expected actual 

(* documented/assertions *)
let (===) expected actual = is expected actual

(* documented/assertions *)
let displayed item = canopy.parallell.functions.displayed item browser

(* documented/assertions *)
let notDisplayed item = canopy.parallell.functions.notDisplayed item browser
   
(* documented/assertions *)
let enabled item = canopy.parallell.functions.enabled item browser

(* documented/assertions *)
let disabled item = canopy.parallell.functions.disabled item browser

(* documented/assertions *)
let fadedIn cssSelector = canopy.parallell.functions.fadedIn cssSelector browser

//clicking/checking
(* documented/actions *)
let click item = canopy.parallell.functions.click item browser

(* documented/actions *)
let doubleClick item = canopy.parallell.functions.doubleClick item browser

(* documented/actions *)
let ctrlClick item = canopy.parallell.functions.ctrlClick item browser

(* documented/actions *)
let shiftClick item = canopy.parallell.functions.shiftClick item browser

(* documented/actions *)
let rightClick item = canopy.parallell.functions.rightClick item browser

(* documented/actions *)
let check item = canopy.parallell.functions.check item browser

(* documented/actions *)
let uncheck item = canopy.parallell.functions.uncheck item browser

//hoverin
(* documented/actions *)
let hover selector = canopy.parallell.functions.hover selector browser

//draggin
(* documented/actions *)
let (-->) cssSelectorA cssSelectorB = canopy.parallell.functions.drag cssSelectorA cssSelectorB browser

(* documented/actions *)
let drag cssSelectorA cssSelectorB = canopy.parallell.functions.drag cssSelectorA cssSelectorB

//browser related
(* documented/actions *)
let pin direction = canopy.parallell.functions.pin direction browser

(* documented/actions *)
let pinToMonitor n = canopy.parallell.functions.pinToMonitor n browser
    
(* documented/actions *)
let start b =
    browser <- canopy.parallell.functions.start b
    browsers <- browsers @ [browser]

(* documented/actions *)
let switchTo b = browser <- b

(* documented/actions *)
let switchToTab number = canopy.parallell.functions.switchToTab number browser

(* documented/actions *)
let closeTab number = canopy.parallell.functions.closeTab number browser

(* documented/actions *)
let tile browsers = canopy.parallell.functions.tile browsers

(* documented/actions *)
let positionBrowser left top width height = canopy.parallell.functions.positionBrowser left top width height browser

(* documented/actions *)
let resize size = canopy.parallell.functions.resize size browser

(* documented/actions *)
let rotate() = canopy.parallell.functions.rotate browser

(* documented/actions *)
let quit browser =
    canopy.classic.configuration.reporter.quit()
    match box browser with
    | :? OpenQA.Selenium.IWebDriver as b -> b.Quit()
    | _ -> browsers |> List.iter (fun b -> b.Quit())

(* documented/actions *)
let currentUrl() = canopy.parallell.functions.currentUrl browser

(* documented/assertions *)
let onn (u: string) = canopy.parallell.functions.onn u browser

(* documented/assertions *)
let on (u: string) = canopy.parallell.functions.on u browser

(* documented/actions *)
let ( !^ ) (u : string) = canopy.parallell.functions.url u browser

(* documented/actions *)
let url u = !^ u

(* documented/actions *)
let title() = canopy.parallell.functions.title browser

(* documented/actions *)
let reload () = canopy.parallell.functions.reload browser

(* documented/actions *)
let back = canopy.parallell.functions.back
(* documented/actions *)
let forward = canopy.parallell.functions.forward

(* documented/actions *)
let navigate direction = canopy.parallell.functions.navigate browser direction

(* documented/actions *)
let addFinder finder = canopy.parallell.functions.addFinder finder

//hints
(* documented/actions *)
let css = canopy.parallell.functions.css
(* documented/actions *)
let xpath = canopy.parallell.functions.xpath
(* documented/actions *)
let jquery = canopy.parallell.functions.jquery
(* documented/actions *)
let label = canopy.parallell.functions.label
(* documented/actions *)
let text = canopy.parallell.functions.text
(* documented/actions *)
let value = canopy.parallell.functions.value

let skip message = canopy.parallell.functions.skip

(* documented/actions *)
let waitForElement cssSelector = canopy.parallell.functions.waitForElement 
