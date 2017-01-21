
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

let border_width = 5
let border_padding = 2

let inputs = """
<div id="canopy_companion" style="position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;">
  <input type="text" id="selector" value="">
  <input type="button" id="go" value="Go">
</div>"""

append "body" inputs

type Self = { self : obj }

let mouseDown element = ()

let removeBorders () =
  remove ".canopy_companion_border_top"
  remove ".canopy_companion_border_bottom"
  remove ".canopy_companion_border_left"
  remove ".canopy_companion_border_right"

let px value = sprintf "%ipx" value

let toInt value = value |> sprintf "%O" |> int

let border position heightValue widthvalue topValue leftValue =
  let element = jq $ "<div>"
  let class' = sprintf "canopy_companion_border_%s" position
  element?addClass(class')
    ?css("height", px heightValue)
    ?css("width", px widthvalue)
    ?css("top", px topValue)
    ?css("left", px leftValue)
    ?css("background-color", "#F00 !important") |> ignore
  
  append "body" element

let createBorders elements = 
  jq?each(elements, fun index element ->
    let clone = jq $ element
    let position = clone?offset()
    let top = position?("top") |> toInt
    let left = position?("left")  |> toInt
    let width = clone?outerWidth() |> toInt
    let height = clone?outerHeight() |> toInt
    
    border "top"    border_width (width + border_padding * 2 + border_width * 2) (top - border_width - border_padding) (left - border_padding - border_width)
    border "bottom" (border_width + 6) (width + border_padding * 2 + border_width * 2 - 5) (top + height + border_padding) (left - border_padding - border_width)
    border "left"   (height + border_padding * 2) border_width (top - border_padding) (left - border_padding - border_width)
    border "right"   (height + border_padding * 2) border_width (top - border_padding) (left + width + border_padding)
        
//    (find ".canopy_companion_border_top")   ?get(0)?target_elem <- element
//    (find ".canopy_companion_border_bottom")?get(0)?target_elem <- element
//    (find ".canopy_companion_border_left")  ?get(0)?target_elem <- element
//    (find ".canopy_companion_border_right") ?get(0)?target_elem <- element
  ) |> ignore
  

click "#go" (fun _ ->   
  let selector = value "#selector"  
  removeBorders ()
  createBorders (find selector))

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