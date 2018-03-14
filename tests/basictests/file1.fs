module file1

open Canopy.Core
open Canopy.Runner.Runner
open Canopy.Runner.Runner.Operators
open Canopy.Expect.Operators

let testpage = "http://lefthandedgoat.github.io/canopy/testpages/"

let all() =
    context "file1"

    "from file 1 2" &&& fun _ ->
        url testpage
        "#welcome" == "Welcome"

    "from file 1 2" &&& fun _ ->
        url testpage
        "#welcome" == "Welcome"