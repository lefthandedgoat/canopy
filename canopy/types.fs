module canopy.types

open System
open OpenQA.Selenium

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

//directions
type direction =
    | Left
    | Right
    | FullScreen

//browser
type BrowserStartMode =
    | Firefox
    | FirefoxWithProfile of Firefox.FirefoxProfile
    | FirefoxWithUserAgent of string
    | IE
    | IEWithOptions of IE.InternetExplorerOptions
    | IEWithOptionsAndTimeSpan of IE.InternetExplorerOptions * TimeSpan
    | Chrome
    | ChromeWithOptions of Chrome.ChromeOptions
    | ChromeWithOptionsAndTimeSpan of Chrome.ChromeOptions * TimeSpan
    | ChromeWithUserAgent of string
    | PhantomJS
    | PhantomJSProxyNone

type Test (description: string, func : (unit -> unit), number : int) =
    member x.Description = description
    member x.Func = func
    member x.Number = number

type suite () = class
    let mutable context : string = null
    let mutable once = fun () -> ()
    let mutable before = fun () -> ()
    let mutable after = fun () -> ()
    let mutable lastly = fun () -> () 
    let mutable tests : Test list = []
    let mutable wips : Test list = []
    let mutable manys : Test list = []

    member x.Context
        with get() = context
        and set(value) = context <- value
    member x.Once
        with get() = once
        and set(value) = once <- value
    member x.Before
        with get() = before
        and set(value) = before <- value
    member x.After
        with get() = after
        and set(value) = after <- value
    member x.Lastly
        with get() = lastly
        and set(value) = lastly <- value
    member x.Tests
        with get() = tests
        and set(value) = tests <- value
    member x.Wips
        with get() = wips
        and set(value) = wips <- value
    member x.Manys
        with get() = manys
        and set(value) = manys <- value
end