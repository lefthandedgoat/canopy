[<AutoOpen>]
module canopy.types

open System
open OpenQA.Selenium
open Microsoft.FSharp.Reflection

type CanopyException(message) = inherit Exception(message)
type CanopyReadOnlyException(message) = inherit CanopyException(message)
type CanopyOptionNotFoundException(message) = inherit CanopyException(message)
type CanopySelectionFailedExeception(message) = inherit CanopyException(message)
type CanopyDeselectionFailedException(message) = inherit CanopyException(message)
type CanopyWaitForException(message) = inherit CanopyException(message)
type CanopyElementNotFoundException(message) = inherit CanopyException(message)
type CanopyMoreThanOneElementFoundException(message) = inherit CanopyException(message)
type CanopyEqualityFailedException(message) = inherit CanopyException(message)
type CanopyNotEqualsFailedException(message) = inherit CanopyException(message)
type CanopyValueNotInListException(message) = inherit CanopyException(message)
type CanopyValueInListException(message) = inherit CanopyException(message)
type CanopyContainsFailedException(message) = inherit CanopyException(message)
type CanopyCountException(message) = inherit CanopyException(message)
type CanopyDisplayedFailedException(message) = inherit CanopyException(message)
type CanopyNotDisplayedFailedException(message) = inherit CanopyException(message)
type CanopyEnabledFailedException(message) = inherit CanopyException(message)
type CanopyDisabledFailedException(message) = inherit CanopyException(message)
type CanopyNotStringOrElementException(message) = inherit CanopyException(message)
type CanopyOnException(message) = inherit CanopyException(message)
type CanopyCheckFailedException(message) = inherit CanopyException(message)
type CanopyUncheckFailedException(message) = inherit CanopyException(message)
type CanopyReadException(message) = inherit CanopyException(message)

//directions
type direction =
    | Left
    | Right
    | FullScreen

//browser
type BrowserStartMode =
    | Firefox
    | FirefoxWithProfile of Firefox.FirefoxProfile
    | FirefoxWithPath of string
    | FirefoxWithUserAgent of string
    | FirefoxWithPathAndTimeSpan of string * TimeSpan
    | IE
    | IEWithOptions of IE.InternetExplorerOptions
    | IEWithOptionsAndTimeSpan of IE.InternetExplorerOptions * TimeSpan
    | Chrome
    | ChromeWithOptions of Chrome.ChromeOptions
    | ChromeWithOptionsAndTimeSpan of Chrome.ChromeOptions * TimeSpan
    | ChromeWithUserAgent of string
    | Safari    
    | PhantomJS
    | PhantomJSProxyNone
    | Remote of string * ICapabilities
  
let toString (x:'a) = 
    match FSharpValue.GetUnionFields(x, typeof<'a>) with
    | case, _ -> case.Name

type Test (description: string, func : (unit -> unit), number : int) =
    member x.Description = description
    member x.Func = func
    member x.Number = number
    member x.Id = if description = null then (String.Format("Test #{0}", number)) else description

type suite () = class
    member val Context : string = null with get, set
    member val TotalTestsCount : int = 0 with get, set
    member val Once = fun () -> () with get, set
    member val Before = fun () -> () with get, set
    member val After = fun () -> () with get, set
    member val Lastly = fun () -> () with get, set
    member val Tests : Test list = [] with get, set
    member val Wips : Test list = [] with get, set
    member val Manys : Test list = [] with get, set
    member val Always : Test list = [] with get, set
    member val IsParallel = false with get, set
end