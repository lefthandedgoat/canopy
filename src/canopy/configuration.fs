module canopy.configuration
open reporters

//runner related
let failFast = ref false

let mutable chromeDir = @"c:\"
let mutable ieDir = @"c:\"
let mutable phantomJSDir = @"c:\"
let mutable elementTimeout = 10.0
let mutable compareTimeout = 10.0
let mutable pageTimeout = 10.0
let mutable wipSleep = 1.0
let mutable runFailedContextsFirst = false
let mutable reporter : IReporter = new ConsoleReporter() :> IReporter
let mutable disableSuggestOtherSelectors = false
let mutable autoPinBrowserRightOnLaunch = true
let mutable throwIfMoreThanOneElement = false
let mutable configuredFinders = finders.defaultFinders