module canopy.finders

open OpenQA.Selenium
open SizSelCsZzz

//have to use the ReadonlyCollection<IWebElement> because thats what selenium uses and it wont cast to seq<IWebElement> or something, and type inference isnt playing nice
//basically a hack because I dont know a better way
let findByCss (cssSelector : string) (f : (By -> System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>)) =
    try
        f(By.CssSelector(cssSelector)) |> List.ofSeq
    with | ex -> []
    
let findByJQuery cssSelector f =
    try
        f(ByJQuery.CssSelector(cssSelector)) |> List.ofSeq
    with | ex -> []

let findByXpath xpath f =
    try
        f(By.XPath(xpath)) |> List.ofSeq
    with | ex -> []

let findByLabel locator f =
    let isInputField (element : IWebElement) =
        element.TagName = "input" && element.GetAttribute("type") <> "hidden"
    
    let isField (element : IWebElement) =
        element.TagName = "select" || element.TagName = "textarea" || isInputField element

    let firstFollowingField (label : IWebElement) =
        let followingElements = label.FindElements(By.XPath("./following-sibling::*[1]")) |> Seq.toList
        match followingElements with
            | head :: tail when isField head-> [head]
            | _ -> []
    try
        let labels = f(By.XPath(sprintf """.//label[text() = "%s"]""" locator))
        if (Seq.isEmpty labels) then
            []
        else
            let (label : IWebElement) = (labels |> List.ofSeq).Head
            match label.GetAttribute("for") with
            | null -> firstFollowingField (labels |> List.ofSeq).Head
            | id -> f(By.Id(id)) |> List.ofSeq
    with | _ -> []

let findByText text f =
    try
        f(By.XPath(sprintf """.//*[text() = "%s"]""" text)) |> List.ofSeq
    with | _ -> []

let findByValue value f =
    try
        findByCss (sprintf """*[value="%s"]""" value) f |> List.ofSeq        
    with | _ -> []

//you can use this as an example to how to extend canopy by creating your own set of finders, tweaking the current collection, or adding/removing
let mutable defaultFinders = 
    (fun cssSelector f ->
        seq {
            yield findByCss     cssSelector f
            yield findByValue   cssSelector f
            yield findByXpath   cssSelector f
            yield findByLabel   cssSelector f
            yield findByText    cssSelector f
            yield findByJQuery  cssSelector f
        }
    )

let addedHints = System.Collections.Generic.Dictionary<string, string list>()
let hints = new System.Collections.Generic.Dictionary<string, seq<(string -> (By -> System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>) -> IWebElement list)>>()