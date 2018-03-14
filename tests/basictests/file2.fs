module file2

open Canopy.Core
open Canopy.Runner.Runner
open Canopy.Runner.Runner.Operators
open Canopy.Expect.Operators

let testpage = "http://lefthandedgoat.github.io/canopy/testpages/"

let all() =
    context "file2"

    "from file 2 1" &&&&& fun _ ->
        url testpage
        "#welcome" == "Welcome"

    "from file 2 2" &&&&& fun _ ->
        url testpage
        "#welcome" == "Welcome"
