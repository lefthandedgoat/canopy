
#r "node_modules/fable-core/Fable.Core.dll"

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

let jq = importDefault<obj> "jquery"
let find selector = jq $ selector
let append selector element = (find selector)?append(element) |> ignore
let css selector property value =  (find selector)?css(property, value) |> ignore
let click selector f =  (find selector)?click(f) |> ignore
let value selector = (find selector)?``val``() 
let remove selector = (find selector)?remove() |> ignore

let inputs = """
<div id="canopy_companion" style="position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;">
  <input type="text" id="selector" value="">
  <input type="button" id="go" value="Go">
</div>"""
append "body" inputs

click "#go" (fun _ ->   
  let selector = value "#selector"  
  css selector "background-color" "red")

let killBorders () =
  remove "#canopy_companion_border_top"
  remove "#canopy_companion_border_bottom"
  remove "#canopy_companion_border_left"
  remove "#canopy_companion_border_right"

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