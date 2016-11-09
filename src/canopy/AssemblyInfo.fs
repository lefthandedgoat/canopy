namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("canopy")>]
[<assembly: AssemblyProductAttribute("canopy")>]
[<assembly: AssemblyDescriptionAttribute("F# web testing framework")>]
[<assembly: AssemblyVersionAttribute("1.0.2")>]
[<assembly: AssemblyFileVersionAttribute("1.0.2")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0.2"
    let [<Literal>] InformationalVersion = "1.0.2"
