namespace System
open System.Reflection

[<assembly: AssemblyProductAttribute("canopy")>]
[<assembly: AssemblyTitleAttribute("canopy")>]
[<assembly: AssemblyDescriptionAttribute("A simple framework in f# on top of selenium for writing UI automation and tests. Change Log at https://github.com/lefthandedgoat/canopy/wiki/Change-Log")>]
[<assembly: AssemblyVersionAttribute("0.9.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.9.1"
