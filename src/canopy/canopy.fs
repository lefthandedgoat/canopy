module canopy.classic

open canopy.types
open canopy.parallell.functions
open OpenQA.Selenium

let mutable (browser : IWebDriver) = null

let mutable (failureMessage : string) = null
let mutable searchedFor : (string * string) list = []

(* documented/actions *)
let firefox = firefox
(* documented/actions *)
let aurora = aurora
(* documented/actions *)
let ie = ie
(* documented/actions *)
let edgeBETA = edgeBETA
(* documented/actions *)
let chrome = chrome
(* documented/actions *)
let chromium = chromium
(* documented/actions *)
let safari = safari

let mutable browsers = []

//misc
(* documented/actions *)
let failsWith message = failureMessage <- message

(* documented/actions *)
let screenshot directory filename = screenshot directory filename browser

(* documented/actions *)
let js script = js script browser

(* documented/actions *)
let sleep seconds = sleep seconds

(* documented/actions *)
let puts text = puts text

(* documented/actions *)
let highlight cssSelector = highlight cssSelector

(* documented/actions *)
let describe text = describe text browser

(* documented/actions *)
let waitFor2 message f = waitFor2 message f

(* documented/actions *)
let waitFor = waitFor2 "Condition not met in given amount of time. If you want to increase the time, put compareTimeout <- 10.0 anywhere before a test to increase the timeout"

//get elements
(* documented/actions *)
let elements cssSelector = elements cssSelector browser

(* documented/actions *)
let element cssSelector = element cssSelector browser

(* documented/actions *)
let unreliableElements cssSelector = unreliableElements cssSelector browser

(* documented/actions *)
let unreliableElement cssSelector = unreliableElement cssSelector browser

(* documented/actions *)
let elementWithin cssSelector elem = elementWithin cssSelector elem browser

(* documented/actions *)
let elementsWithText cssSelector regex = elementsWithText cssSelector regex browser

(* documented/actions *)
let elementWithText cssSelector regex = elementWithText cssSelector regex browser

(* documented/actions *)
let parent elem = parent elem browser

(* documented/actions *)
let elementsWithin cssSelector elem = elementsWithin cssSelector elem browser

(* documented/actions *)
let unreliableElementsWithin cssSelector elem = unreliableElementsWithin cssSelector elem browser

(* documented/actions *)
let someElement cssSelector = someElement cssSelector browser

(* documented/actions *)
let someElementWithin cssSelector elem = someElementWithin cssSelector elem browser

(* documented/actions *)
let someParent elem = someParent elem browser

(* documented/actions *)
let nth index cssSelector = nth index cssSelector browser

(* documented/actions *)
let first cssSelector = first cssSelector browser

(* documented/actions *)
let last cssSelector = last cssSelector browser

//read/write

(* documented/actions *)
let ( << ) item text = write item text browser

(* documented/actions *)
let read item = read item browser

(* documented/actions *)
let clear item = clear item browser

//status
(* documented/assertions *)
let selected item = selected item browser

(* documented/assertions *)
let deselected item = deselected item browser

//keyboard
(* documented/actions *)
let tab = tab
(* documented/actions *)
let enter = enter
(* documented/actions *)
let down = down
(* documented/actions *)
let up = up
(* documented/actions *)
let left = left
(* documented/actions *)
let right = right
(* documented/actions *)
let esc = esc

(* documented/actions *)
let press key = press key browser

//alerts
(* documented/actions *)
let alert() = alert browser

(* documented/actions *)
let acceptAlert() = acceptAlert browser

(* documented/actions *)
let dismissAlert() = dismissAlert browser

(* documented/actions *)
let fastTextFromCSS selector = fastTextFromCSS selector browser

//assertions
(* documented/assertions *)
let ( == ) item value = equals item value browser

(* documented/assertions *)
let ( != ) cssSelector value = notEquals cssSelector value browser

(* documented/assertions *)
let ( *= ) cssSelector value = oneOrManyEquals cssSelector value browser

(* documented/assertions *)
let ( *!= ) cssSelector value = noneOfManyNotEquals cssSelector value browser

(* documented/assertions *)
let contains (value1 : string) (value2 : string) = contains value1 value2

(* documented/assertions *)
let containsInsensitive (value1 : string) (value2 : string) = containsInsensitive value1 value2

(* documented/assertions *)
let notContains (value1 : string) (value2 : string) = notContains value1 value2

(* documented/assertions *)
let count cssSelector count' = count cssSelector count' browser

(* documented/assertions *)
let ( =~ ) cssSelector pattern = regexEquals cssSelector pattern browser

(* documented/assertions *)
let ( !=~ ) cssSelector pattern = regexNotEquals cssSelector pattern browser

(* documented/assertions *)
let ( *~ ) cssSelector pattern = oneOrManyRegexEquals cssSelector pattern browser

(* documented/assertions *)
let is expected actual = is expected actual

(* documented/assertions *)
let (===) expected actual = is expected actual

(* documented/assertions *)
let displayed item = displayed item browser

(* documented/assertions *)
let notDisplayed item = notDisplayed item browser

(* documented/assertions *)
let enabled item = enabled item browser

(* documented/assertions *)
let disabled item = disabled item browser

(* documented/assertions *)
let fadedIn cssSelector = fadedIn cssSelector browser

//clicking/checking
(* documented/actions *)
let click item = click item browser

(* documented/actions *)
let doubleClick item = doubleClick item browser

(* documented/actions *)
let ctrlClick item = ctrlClick item browser

(* documented/actions *)
let shiftClick item = shiftClick item browser

(* documented/actions *)
let rightClick item = rightClick item browser

(* documented/actions *)
let check item = check item browser

(* documented/actions *)
let uncheck item = uncheck item browser

//hoverin
(* documented/actions *)
let hover selector = hover selector browser

//draggin
(* documented/actions *)
let (-->) cssSelectorA cssSelectorB = drag cssSelectorA cssSelectorB browser

(* documented/actions *)
let drag cssSelectorA cssSelectorB = drag cssSelectorA cssSelectorB

//browser related
(* documented/actions *)
let pin direction = pin direction browser

(* documented/actions *)
let start b =
    browser <- start b
    browsers <- browsers @ [browser]

(* documented/actions *)
let switchTo b = browser <- b

(* documented/actions *)
let switchToTab number = switchToTab number browser

(* documented/actions *)
let closeTab number = closeTab number browser

(* documented/actions *)
let tile browsers = tile browsers

(* documented/actions *)
let positionBrowser left top width height = positionBrowser left top width height browser

(* documented/actions *)
let resize size = resize size browser

(* documented/actions *)
let rotate() = rotate browser

(* documented/actions *)
let quit browser =
    canopy.configuration.reporter.quit()
    match box browser with
    | :? OpenQA.Selenium.IWebDriver as b -> b.Quit()
    | _ -> browsers |> List.iter (fun b -> b.Quit())

(* documented/actions *)
let currentUrl() = currentUrl browser

(* documented/assertions *)
let onn (u: string) = onn u browser

(* documented/assertions *)
let on (u: string) = on u browser

(* documented/actions *)
let ( !^ ) (u : string) = url u browser

(* documented/actions *)
let url u = !^ u

(* documented/actions *)
let title() = title browser

(* documented/actions *)
let reload () = reload browser

(* documented/actions *)
let back = back
(* documented/actions *)
let forward = forward

(* documented/actions *)
let navigate direction = navigate browser direction

(* documented/actions *)
let addFinder finder = addFinder finder browser

//hints
(* documented/actions *)
let css = css
(* documented/actions *)
let xpath = xpath
(* documented/actions *)
let jquery = jquery
(* documented/actions *)
let label = label
(* documented/actions *)
let text = text
(* documented/actions *)
let value = value

let skip message = skip message browser

(* documented/actions *)
let waitForElement cssSelector = waitForElement cssSelector browser
