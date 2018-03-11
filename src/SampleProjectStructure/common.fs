module common

//anything that is useful globally

open canopy
open config

type users =
    | Administrator
    | Client
    | Manager
    | SalesPerson

type clients =
    | ABCCorp
    | Acme
    | XYZCorp

//selectors
let modal = ".noty_text"

//helpers
let logOff () =
    click "Log off"
    on root