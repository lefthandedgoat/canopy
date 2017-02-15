namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("canopy")>]
[<assembly: AssemblyProductAttribute("canopy")>]
[<assembly: AssemblyDescriptionAttribute("F# web testing framework")>]
[<assembly: AssemblyVersionAttribute("1.1.3")>]
[<assembly: AssemblyFileVersionAttribute("1.1.3")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.1.3"
    let [<Literal>] InformationalVersion = "1.1.3"
