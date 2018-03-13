module file1

open canopy.classic.core
open canopy.classic.runner

let testpage = "http://lefthandedgoat.github.io/canopy/testpages/"

let all() =
    context "file1"

    "from file 1 2" &&& fun _ ->
        url testpage
        "#welcome" == "Welcome"

    "from file 1 2" &&& fun _ ->
        url testpage
        "#welcome" == "Welcome"
