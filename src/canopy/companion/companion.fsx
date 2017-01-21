
#r "node_modules/fable-core/Fable.Core.dll"

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

let jq = importDefault<obj> "jquery"
  
let helper = """
<div id="canopy_companion" style="position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;">
  <input type="text" id="selector" value="">
  <input type="button" id="go" value="Go">
</div>"""
(jq $ ("body"))?append(helper) |> ignore

let go = jq $ ("#go")

go ? click(fun _ ->   
  let selector = (jq $ ("#selector"))?``val``()  
  let elements = jq $ selector
  elements?css("background-color", "red") |> ignore
  ()) |> ignore

//let mutable tests : (string * (unit -> unit)) list = []
//let ( &&& ) desc f = tests <- List.append tests [desc, f]
//let run () = tests |> List.map (fun (desc, f) -> (printfn "%s" desc; f ()))
//let (==) value1 value2 = System.Console.WriteLine("{0} expected: {1} got: {2}", (value1 = value2), value2, value1)

//optionally inject jquery if not already there
//on mouse over highlight element
//on click generate selectors and create a list, sort by shortest and most specific (determined by how many dom elements are recieved by it
//figure out how to click but not page transition (blockClickON https://github.com/cantino/selectorgadget/blob/2c9f31102f5f2ecb0f621fa7215d6ebc10d78171/lib/js/core/core.js.coffee)
//Init a npm thing, requis js and all that stuff

//make fake task


(* mouseover code
window.onclick = function(element) {
  var target = element.target
  var element = {className:target.className, id:target.id, text:target.innerText || target.textContent };
  console.log(element);
};
*)