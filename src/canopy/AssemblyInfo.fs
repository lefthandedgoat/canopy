﻿namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("canopy")>]
[<assembly: AssemblyProductAttribute("canopy")>]
[<assembly: AssemblyDescriptionAttribute("F# web testing framework")>]
[<assembly: AssemblyVersionAttribute("0.9.49")>]
[<assembly: AssemblyFileVersionAttribute("0.9.49")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.9.49"
