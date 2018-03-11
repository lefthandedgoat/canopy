/// Assertions
module Canopy.Assert

open OpenQA.Selenium
open System
open Canopy

let equal browser item value =
    match box item with
    | :? IAlert as alert ->
        let text = alert.Text
        if text <> value then
            alert.Dismiss()
            raise (CanopyEqualityFailedException(sprintf "equality check failed.  expected: %s, got: %s" value text))
    | :? string as cssSelector ->
        let bestvalue = ref ""
        try
            wait compareTimeout (fun _ ->
                let readvalue = readB browser cssSelector
                if readvalue <> value && readvalue <> "" then
                    bestvalue := readvalue
                    false
                else
                    readvalue = value)
        with
        | :? CanopyElementNotFoundException as ex ->
            let message = sprintf "%s%sequality check failed.  expected: %s, got: %s" ex.Message Environment.NewLine value !bestvalue
            raise (CanopyEqualityFailedException message)
        | :? WebDriverTimeoutException ->
            let message = sprintf "equality check failed.  expected: %s, got: %s" value !bestvalue
            raise (CanopyEqualityFailedException message)
    | _ ->
        let message = sprintf "Can't check equality on %O because it is not a string or alert" item
        raise (CanopyNotStringOrElementException message)

let notEqual browser cssSelector value =
    try
        wait compareTimeout (fun _ -> (readB browser cssSelector) <> value)
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%snot equals check failed.  expected NOT: %s, got: " ex.Message Environment.NewLine value
        raise (CanopyNotEqualsFailedException message)
    | :? WebDriverTimeoutException ->
        let gotten = readB browser cssSelector
        let message = sprintf "not equals check failed.  expected NOT: %s, got: %s" value gotten
        raise (CanopyNotEqualsFailedException message)

let atLeastOneEqual browser cssSelector value =
    try
        wait compareTimeout (fun _ ->
             cssSelector
             |> elementsB browser
             |> Seq.exists(fun element -> textOf element = value))
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%scan't find %s in list %s%sgot: " ex.Message Environment.NewLine value cssSelector Environment.NewLine
        raise (CanopyValueNotInListException message)
    | :? WebDriverTimeoutException ->
        let sb = new System.Text.StringBuilder()
        cssSelector
        |> elementsB browser
        |> List.iter (fun e -> Printf.bprintf sb "%s%s" (textOf e) Environment.NewLine)
        let message = sprintf "can't find %s in list %s%sgot: %s" value cssSelector Environment.NewLine (sb.ToString())
        raise (CanopyValueNotInListException message)

let noElementEquals browser cssSelector value =
    try
        wait compareTimeout (fun _ ->
            cssSelector
            |> elementsB browser
            |> Seq.exists (fun element -> textOf element = value |> not))
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%sfound check failed" ex.Message Environment.NewLine
        raise (CanopyValueInListException message)
    | :? WebDriverTimeoutException ->
        let message = sprintf "found %s in list %s, expected not to" value cssSelector
        raise (CanopyValueInListException message)

let selectedB browser item =
    let selected cssSelector (elem: IWebElement) =
        if not <| elem.Selected then
            let message = sprintf "Element selected failed, %s not selected." cssSelector
            raise (CanopySelectionFailedExeception message)

    match box item with
    | :? IWebElement as elem ->
        selected elem.TagName elem
    | :? string as cssSelector ->
        elementB browser cssSelector
        |> selected cssSelector
    | _ ->
        let message = sprintf "Can't check selected on %O because it is not a string or element." item
        raise (CanopyNotStringOrElementException message)

(* documented/assertions *)
let selected item =
    selectedB browser item

(* documented/assertions *)
let deselectedB browser item =
    let deselected cssSelector (elem : IWebElement) =
        if elem.Selected then raise (CanopyDeselectionFailedException(sprintf "element deselected failed, %s selected." cssSelector))

    match box item with
    | :? IWebElement as elem ->
        deselected elem.TagName elem
    | :? string as cssSelector ->
        elementB browser cssSelector
        |> deselected cssSelector
    | _ ->
        let message = sprintf "Can't check deselected on %O because it is not a string or element." item
        raise (CanopyNotStringOrElementException(message))

(* documented/assertions *)
let deselected item =
    deselectedB browser item

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
let countB browser cssSelector count =
    try
        wait compareTimeout (fun _ ->
            (unreliableElementsB browser cssSelector).Length = count)
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%scount failed. expected: %i got: %i" ex.Message Environment.NewLine count 0
        raise (CanopyCountException message)

    | :? WebDriverTimeoutException ->
        let message = sprintf "count failed. expected: %i got: %i" count (unreliableElementsB browser cssSelector).Length
        raise (CanopyCountException message)

(* documented/assertions *)
let count cssSelector count =
    countB browser cssSelector count

let matches browser cssSelector regexPattern =
    try
        wait compareTimeout (fun _ ->
            let value = readB browser cssSelector
            regexMatch regexPattern value)
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%sregex equality check failed.  expected: %s, got:" ex.Message Environment.NewLine regexPattern
        raise (CanopyEqualityFailedException message)
    | :? WebDriverTimeoutException ->
        let value = readB browser cssSelector
        let message =
            sprintf "regex equality check failed. Expected to match: /%s/, got: %s"
                    regexPattern value
        raise (CanopyEqualityFailedException message)

let matchesNot browser cssSelector regexPattern =
    try
        wait compareTimeout (fun _ ->
            let value = readB browser cssSelector
            regexMatch regexPattern value |> not)
    with
    | :? CanopyElementNotFoundException as ex ->
        let message = sprintf "%s%sregex not equality check failed.  expected NOT: %s, got:" ex.Message Environment.NewLine regexPattern
        raise (CanopyEqualityFailedException message)
    | :? WebDriverTimeoutException ->
        let value = readB browser cssSelector
        let message =
            sprintf "Regex negative equality check failed. Expected it not to match: /%s/, got: %s"
                    regexPattern value
        raise (CanopyEqualityFailedException message)


let atLeastOneMatches browser cssSelector regexPattern =
    try
        wait compareTimeout (fun _ ->
            cssSelector
            |> elementsB browser
            |> Seq.exists(fun element -> regexMatch regexPattern (textOf element)))
    with
        | :? CanopyElementNotFoundException as ex ->
            let message = sprintf "%s%scan't regex find %s in list %s%sgot: " ex.Message Environment.NewLine regexPattern cssSelector Environment.NewLine
            raise (CanopyValueNotInListException message)
        | :? WebDriverTimeoutException ->
            let sb = new System.Text.StringBuilder()
            cssSelector
            |> elementsB browser
            |> List.iter (fun e -> Printf.bprintf sb "%s%s" (textOf e) Environment.NewLine)
            let message =
                sprintf "can't regex find %s in list %s%sgot: %s"
                        regexPattern cssSelector Environment.NewLine (sb.ToString())
            raise (CanopyValueNotInListException message)

(* documented/assertions *)
let is expected actual =
    if expected <> actual then
        let message = sprintf "equality check failed.  expected: %O, got: %O" expected actual
        raise (CanopyEqualityFailedException message)

let private shown (elem: IWebElement) =
    let opacity = elem.GetCssValue("opacity")
    let display = elem.GetCssValue("display")
    display <> "none" && opacity = "1"

(* documented/assertions *)
let displayedB browser item =
    try
        wait compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element ->
                shown element
            | :? string as cssSelector ->
                shown (elementB browser cssSelector)
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
    displayedB browser item

(* documented/assertions *)
let notDisplayedB browser item =
    try
        wait compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element ->
                not (shown element)
            | :? string as cssSelector ->
                   (unreliableElementsB browser cssSelector |> List.isEmpty)
                || not (unreliableElementB browser cssSelector |> shown)
            | _ ->
                let message = sprintf "Can't check notDisplayed on %O because it is not a string or webelement" item
                raise (CanopyNotStringOrElementException message))
    with
    | :? CanopyElementNotFoundException as ex -> raise (CanopyNotDisplayedFailedException(sprintf "%s%snotDisplay check for %O failed." ex.Message Environment.NewLine item))
    | :? WebDriverTimeoutException -> raise (CanopyNotDisplayedFailedException(sprintf "notDisplay check for %O failed." item))

(* documented/assertions *)
let notDisplayed item =
    notDisplayedB browser item

(* documented/assertions *)
let enabledB browser item =
    try
        wait compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element ->
                element.Enabled
            | :? string as cssSelector ->
                (elementB browser cssSelector).Enabled
            | _ ->
                let message = sprintf "Can't check enabled on %O because it is not a string or webelement" item
                raise (CanopyNotStringOrElementException message))
    with
    | :? CanopyElementNotFoundException as ex -> raise (CanopyEnabledFailedException(sprintf "%s%senabled check for %O failed." ex.Message Environment.NewLine item))
    | :? WebDriverTimeoutException -> raise (CanopyEnabledFailedException(sprintf "enabled check for %O failed." item))

(* documented/assertions *)
let enabled item =
    enabledB browser item

(* documented/assertions *)
let disabledB browser item =
    try
        wait compareTimeout (fun _ ->
            match box item with
            | :? IWebElement as element ->
                not element.Enabled
            | :? string as cssSelector ->
                not (elementB browser cssSelector).Enabled
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
    disabledB browser item

(* documented/assertions *)
let fadedInB browser cssSelector =
    fun _ ->
        shown (elementB browser cssSelector)

(* documented/assertions *)
let fadedIn cssSelector =
    fadedInB browser cssSelector

/// Infix operators for assertions; these use the global browser instance and are thus
/// not thread-safe.
module Operators =
    (* documented/assertions *)
    let ( == ) item value = equal browser item value
    (* documented/assertions *)
    let ( != ) cssSelector value = notEqual browser cssSelector value
    (* documented/assertions *)
    let ( *= ) cssSelector value = atLeastOneEqual browser cssSelector value
    (* documented/assertions *)
    let ( *!= ) cssSelector value = noElementEquals browser cssSelector value
    (* documented/assertions *)
    let ( =~ ) cssSelector pattern = matches browser cssSelector pattern
    (* documented/assertions *)
    let ( !=~ ) cssSelector pattern = matchesNot browser cssSelector pattern
    (* documented/assertions *)
    let ( *~ ) cssSelector pattern = atLeastOneMatches browser cssSelector pattern
    (* documented/assertions *)
    let (===) expected actual = is expected actual

