namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("canopy")>]
[<assembly: AssemblyProductAttribute("canopy")>]
[<assembly: AssemblyDescriptionAttribute("F# web testing framework")>]
[<assembly: AssemblyVersionAttribute("1.0.5")>]
[<assembly: AssemblyFileVersionAttribute("1.0.5")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0.5"
    let [<Literal>] InformationalVersion = "1.0.5"
