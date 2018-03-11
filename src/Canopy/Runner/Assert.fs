/// Assertions
module Canopy.Runner.Assert

open OpenQA.Selenium
open System
open Canopy

let equalC (context: Context) item value =
    match box item with
    | :? IAlert as alert ->
        let text = alert.Text
        if text <> value then
            alert.Dismiss()
            let message = sprintf "equality check failed.  expected: %s, got: %s" value text
            raise (CanopyEqualityFailedException message)
    | :? string as cssSelector ->
        let bestvalue = ref ""
        try
            wait context.config.compareTimeout (fun _ ->
                let readvalue = readC context cssSelector
                if readvalue <> value && readvalue <> "" then
                    bestvalue := readvalue
                    false
                else
                    readvalue = value)
        with
        | :? CanopyElementNotFoundException as ex ->
            let message = sprintf "%s%sequality check failed. Expected: %s, got: %s" ex.Message Environment.NewLine value !bestvalue
            raise (CanopyEqualityFailedException message)
        | :? WebDriverTimeoutException ->
            let message = sprintf "Equality check failed. Expected: %s, got: %s" value !bestvalue
            raise (CanopyEqualityFailedException message)
    | _ ->
        let message = sprintf "Can't check equality on %O because it is not a string or alert" item
        raise (CanopyNotStringOrElementException message)

let equal item value =
    equalC (context ()) item value

let notEqualC (context: Context) cssSelector value =
    try
        wait context.config.compareTimeout (fun _ -> readC context cssSelector <> value)
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%snot equals check failed.  expected NOT: %s, got: " ex.Message Environment.NewLine value
        raise (CanopyNotEqualsFailedException message)
    | :? WebDriverTimeoutException ->
        let gotten = readC context cssSelector
        let message = sprintf "Not-equals check failed.  expected NOT: %s, got: %s" value gotten
        raise (CanopyNotEqualsFailedException message)

let notEqual cssSelector value =
    notEqualC (context ()) cssSelector value

let atLeastOneEqualC (context: Context) cssSelector value =
    try
        wait context.config.compareTimeout (fun _ ->
             cssSelector
             |> elementsC context
             |> Seq.exists (fun element -> textOf element = value))
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%scan't find %s in list %s%sgot: " ex.Message Environment.NewLine value cssSelector Environment.NewLine
        raise (CanopyValueNotInListException message)
    | :? WebDriverTimeoutException ->
        let sb = new System.Text.StringBuilder()
        cssSelector
        |> elementsC context
        |> List.iter (fun e -> Printf.bprintf sb "%s%s" (textOf e) Environment.NewLine)
        let message = sprintf "can't find %s in list %s%sgot: %s" value cssSelector Environment.NewLine (sb.ToString())
        raise (CanopyValueNotInListException message)

let atLeastOneEqual cssSelector value =
    atLeastOneEqualC (context ()) cssSelector value

let noElementEqualsC (context: Context) cssSelector value =
    try
        wait context.config.compareTimeout (fun _ ->
            cssSelector
            |> elementsC context
            |> Seq.exists (fun element -> textOf element <> value))
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%sfound check failed" ex.Message Environment.NewLine
        raise (CanopyValueInListException message)
    | :? WebDriverTimeoutException ->
        let message = sprintf "found %s in list %s, expected not to" value cssSelector
        raise (CanopyValueInListException message)

let noElementEquals cssSelector value =
    noElementEqualsC (context ()) cssSelector value

let selectedC (context: Context) item =
    let selected cssSelector (elem: IWebElement) =
        if not <| elem.Selected then
            let message = sprintf "Element selected failed, %s not selected." cssSelector
            raise (CanopySelectionFailedExeception message)

    match box item with
    | :? IWebElement as elem ->
        selected elem.TagName elem
    | :? string as cssSelector ->
        elementC context cssSelector
        |> selected cssSelector
    | _ ->
        let message = sprintf "Can't check selected on %O because it is not a string or element." item
        raise (CanopyNotStringOrElementException message)

(* documented/assertions *)
let selected item =
    selectedC (context ()) item

(* documented/assertions *)
let deselectedC (context: Context) item =
    let deselected cssSelector (elem : IWebElement) =
        if elem.Selected then
            let message = sprintf "element deselected failed, %s selected." cssSelector
            raise (CanopyDeselectionFailedException message)

    match box item with
    | :? IWebElement as elem ->
        deselected elem.TagName elem
    | :? string as cssSelector ->
        elementC context cssSelector
        |> deselected cssSelector
    | _ ->
        let message = sprintf "Can't check deselected on %O because it is not a string or element." item
        raise (CanopyNotStringOrElementException(message))

(* documented/assertions *)
let deselected item =
    deselectedC (context ()) item

(* documented/assertions *)
let contains (value1: string) (value2: string) =
    if not (value2.Contains value1) then
        let message = sprintf "contains check failed.  %s does not contain %s" value2 value1
        raise (CanopyContainsFailedException message)

(* documented/assertions *)
let containsInsensitive (value1: string) (value2: string) =
    let rules = StringComparison.InvariantCultureIgnoreCase
    let contains = value2.IndexOf(value1, rules)
    if contains < 0 then
        let message = sprintf "contains insensitive check failed.  %s does not contain %s" value2 value1
        raise (CanopyContainsFailedException message)

(* documented/assertions *)
let notContains (value1: string) (value2: string) =
    if value2.Contains value1 then
        let message = sprintf "notContains check failed.  %s does contain %s" value2 value1
        raise (CanopyNotContainsFailedException message)

(* documented/assertions *)
let countC (context: Context) cssSelector count =
    try
        wait context.config.compareTimeout (fun _ ->
            (unreliableElementsC context cssSelector).Length = count)
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%scount failed. expected: %i got: %i" ex.Message Environment.NewLine count 0
        raise (CanopyCountException message)

    | :? WebDriverTimeoutException ->
        let found = unreliableElementsC context cssSelector
        let message = sprintf "Count failed. expected: %i got: %i" count found.Length
        raise (CanopyCountException message)

(* documented/assertions *)
let count cssSelector count =
    countC (context ()) cssSelector count

let matchesC (context: Context) cssSelector regexPattern =
    try
        wait context.config.compareTimeout (fun _ ->
            let value = readC context cssSelector
            regexMatch regexPattern value)
    with
    | :? CanopyElementNotFoundException as ex ->
        let message =
            sprintf "%s%sRegEx equality check failed. Expected to match: /%s/, but did not find any matching elements for the selector '%s'."
                    ex.Message Environment.NewLine regexPattern cssSelector
        raise (CanopyEqualityFailedException message)
    | :? WebDriverTimeoutException ->
        let value = readC context cssSelector
        let message =
            sprintf "RegEx equality check failed. Expected to match: /%s/, got: '%s'"
                    regexPattern value
        raise (CanopyEqualityFailedException message)

let matches cssSelector regexPattern =
    matchesC (context ()) cssSelector regexPattern

let matchesNotC (context: Context) cssSelector regexPattern =
    try
        wait context.config.compareTimeout (fun _ ->
            let value = readC context cssSelector
            not (regexMatch regexPattern value))
    with
    | :? CanopyElementNotFoundException as ex ->
        let message =
            sprintf "%s%sregex not equality check failed. Expected it not to match: /%s/, but did not find elements for the selector '%s' to match against."
                    ex.Message Environment.NewLine regexPattern cssSelector
        raise (CanopyEqualityFailedException message)
    | :? WebDriverTimeoutException ->
        let value = readC context cssSelector
        let message =
            sprintf "Regex negative equality check failed. Expected it not to match: /%s/, got: %s"
                    regexPattern value
        raise (CanopyEqualityFailedException message)

let matchesNot cssSelector regexPattern =
    matchesNotC (context ()) cssSelector regexPattern

let atLeastOneMatchesC (context: Context) cssSelector regexPattern =
    try
        wait context.config.compareTimeout (fun _ ->
            cssSelector
            |> elementsC context
            |> Seq.exists(fun element -> regexMatch regexPattern (textOf element)))
    with
    | :? CanopyElementNotFoundException as ex ->
        let message =
            sprintf "%s%scan't regex find %s in list %s%sgot: "
                    ex.Message Environment.NewLine regexPattern cssSelector Environment.NewLine
        raise (CanopyValueNotInListException message)
    | :? WebDriverTimeoutException ->
        let sb = new System.Text.StringBuilder()
        cssSelector
        |> elementsC context
        |> List.iter (fun e -> Printf.bprintf sb "%s%s" (textOf e) Environment.NewLine)
        let message =
            sprintf "can't regex find %s in list %s%sgot: %s"
                    regexPattern cssSelector Environment.NewLine (sb.ToString())
        raise (CanopyValueNotInListException message)

let atLeastOneMatches cssSelector regexPattern =
    atLeastOneMatchesC (context ()) cssSelector regexPattern

(* documented/assertions *)
let is expected actual =
    if expected <> actual then
        let message = sprintf "equality check failed. Expected: %O, got: %O" expected actual
        raise (CanopyEqualityFailedException message)

let private shown (elem: IWebElement) =
    let opacity = elem.GetCssValue "opacity"
    let display = elem.GetCssValue "display"
    display <> "none" && opacity = "1"

(* documented/assertions *)
let displayedC (context: Context) item =
    try
        wait context.config.compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element ->
                shown element
            | :? string as cssSelector ->
                shown (elementC context cssSelector)
            | _ ->
                let message = sprintf "Can't check displayed on %O because it is not a string or webelement" item
                raise (CanopyNotStringOrElementException message))
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%sdisplay check for %O failed." ex.Message Environment.NewLine item
        raise (CanopyDisplayedFailedException message)

    | :? WebDriverTimeoutException ->
        let message = sprintf "display check for %O failed." item
        raise (CanopyDisplayedFailedException message)

(* documented/assertions *)
let displayed item =
    displayedC (context ()) item

(* documented/assertions *)
let notDisplayedC (context: Context) item =
    try
        wait context.config.compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element ->
                not (shown element)
            | :? string as cssSelector ->
                   (unreliableElementsC context cssSelector |> List.isEmpty)
                || not (unreliableElementC context cssSelector |> shown)
            | _ ->
                let message = sprintf "Can't check notDisplayed on %O because it is not a string or webelement" item
                raise (CanopyNotStringOrElementException message))
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%snotDisplayed-check for %O failed." ex.Message Environment.NewLine item
        raise (CanopyNotDisplayedFailedException message)
    | :? WebDriverTimeoutException ->
        let message = sprintf "notDisplayed-check for %O failed." item
        raise (CanopyNotDisplayedFailedException message)

(* documented/assertions *)
let notDisplayed item =
    notDisplayedC (context ()) item

(* documented/assertions *)
let enabledC (context: Context) item =
    try
        wait context.config.compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element ->
                element.Enabled
            | :? string as cssSelector ->
                (elementC context cssSelector).Enabled
            | _ ->
                let message = sprintf "Can't check enabled on %O because it is not a string or webelement" item
                raise (CanopyNotStringOrElementException message))
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%sEnabled-check for %O failed." ex.Message Environment.NewLine item
        raise (CanopyEnabledFailedException message)
    | :? WebDriverTimeoutException ->
        let message = sprintf "Enabled-check for %O failed." item
        raise (CanopyEnabledFailedException message)

(* documented/assertions *)
let enabled item =
    enabledC (context ()) item

(* documented/assertions *)
let disabledC (context: Context) item =
    try
        wait context.config.compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element ->
                not element.Enabled
            | :? string as cssSelector ->
                not (elementC context cssSelector).Enabled
            | _ ->
                let message = sprintf "Can't check disabled on %O because it is not a string or webelement" item
                raise (CanopyNotStringOrElementException message))
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%sdisabled check for %O failed." ex.Message Environment.NewLine item
        raise (CanopyDisabledFailedException message)
    | :? WebDriverTimeoutException ->
        let message = sprintf "disabled check for %O failed." item
        raise (CanopyDisabledFailedException message)

(* documented/assertions *)
let disabled item =
    disabledC (context ()) item

(* documented/assertions *)
let fadedInB browser cssSelector =
    fun _ ->
        shown (elementC (context ()) cssSelector)

(* documented/assertions *)
let fadedIn cssSelector =
    fadedInB browser cssSelector

/// Infix operators for assertions; these use the global browser instance and are thus
/// not thread-safe.
module Operators =
    (* documented/assertions *)
    let ( == ) item value = equal item value
    (* documented/assertions *)
    let ( != ) cssSelector value = notEqual cssSelector value
    (* documented/assertions *)
    let ( *= ) cssSelector value = atLeastOneEqual cssSelector value
    (* documented/assertions *)
    let ( *!= ) cssSelector value = noElementEquals cssSelector value
    (* documented/assertions *)
    let ( =~ ) cssSelector pattern = matches cssSelector pattern
    (* documented/assertions *)
    let ( !=~ ) cssSelector pattern = matchesNot cssSelector pattern
    (* documented/assertions *)
    let ( *~ ) cssSelector pattern = atLeastOneMatches cssSelector pattern
    (* documented/assertions *)
    let (===) expected actual = is expected actual
