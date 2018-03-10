module config

canopy.configuration.compareTimeout <- 3.0
canopy.configuration.elementTimeout <- 3.0
canopy.configuration.pageTimeout <- 3.0

//optionally pick a different style of reporter
//http://lefthandedgoat.github.io/canopy/reporting.html

let server = "localhost"
let port = ":8080"
let prefix = "http://"
let root = prefix + server + port