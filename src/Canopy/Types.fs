[<AutoOpen>]
module Canopy.Types

open System
open Canopy.Logging
open OpenQA.Selenium

type CanopyException =
    inherit exn
    new (message) = { inherit Exception(message) }
    new (message, inner: exn) = { inherit Exception(message, inner) }
type CanopyReadOnlyException(message) = inherit CanopyException(message)
type CanopyOptionNotFoundException(message) = inherit CanopyException(message)
type CanopySelectionFailedExeception(message) = inherit CanopyException(message)
type CanopyDeselectionFailedException(message) = inherit CanopyException(message)
type CanopyElementNotFoundException(message) = inherit CanopyException(message)
type CanopyMoreThanOneElementFoundException(message) = inherit CanopyException(message)
type CanopyEqualityFailedException(message) = inherit CanopyException(message)
type CanopyNotEqualsFailedException(message) = inherit CanopyException(message)
type CanopyValueNotInListException(message) = inherit CanopyException(message)
type CanopyValueInListException(message) = inherit CanopyException(message)
type CanopyContainsFailedException(message) = inherit CanopyException(message)
type CanopyNotContainsFailedException(message) = inherit CanopyException(message)
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
type CanopySkipTestException() = inherit CanopyException(String.Empty)
type CanopyNoBrowserException(message) = inherit CanopyException(message)

type Direction =
    | Left
    | Right
    | FullScreen

type BrowserStartMode =
    | Firefox
    | FirefoxWithPath of path:string
    | FirefoxWithUserAgent of userAgent:string
    | FirefoxWithPathAndTimeSpan of path:string * TimeSpan
    | FirefoxWithOptionsAndTimeSpan of options:Firefox.FirefoxOptions * TimeSpan
    | FirefoxWithOptions of options:Firefox.FirefoxOptions
    | FirefoxHeadless
    | IE
    | IEWithOptions of options:IE.InternetExplorerOptions
    | IEWithOptionsAndTimeSpan of options:IE.InternetExplorerOptions * TimeSpan
    | [<Obsolete "Use BrowserStartMode.Edge">] EdgeBETA
    | Edge
    | Chrome
    | ChromeWithOptions of options:Chrome.ChromeOptions
    | ChromeWithOptionsAndTimeSpan of options:Chrome.ChromeOptions * TimeSpan
    | ChromeWithUserAgent of userAgent:string
    | ChromeHeadless
    | Chromium
    | ChromiumWithOptions of options:Chrome.ChromeOptions
    | Safari
    | Remote of string * ICapabilities


type Logger with
    member x.writeLevel level messageFactory =
        // TO CONSIDER: returning the async
        x.logWithAck level messageFactory
        |> Async.RunSynchronously

    member x.write messageFactory =
        x.writeLevel Info messageFactory
