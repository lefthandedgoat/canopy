module configuration
open reporters

//runner related
let failFast = ref false
let suggestions = ref true

let mutable chromeDir = @"c:\"
let mutable ieDir = @"c:\"
let mutable elementTimeout = 10.0
let mutable compareTimeout = 10.0
let mutable pageTimeout = 10.0
let mutable wipSleep = 1
let mutable runFailedContextsFirst = false
let mutable reporter : IReporter = new ConsoleReporter() :> IReporter