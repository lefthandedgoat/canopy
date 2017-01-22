
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
let set selector value = (find selector)?``val``(value) |> ignore
let remove selector = (find selector)?remove() |> ignore
let exists selector = 
  let value = (find selector)?("length") |> sprintf "%O" |> int
  value > 0

let border_width = 5
let border_padding = 2

let inputs = """
<div id="canopy_companion">
  <input type="text" id="selector" value="">
  <input type="button" id="go" value="Go">
  <input type="button" id="clear" value="Clear">
  <input type="button" id="close" value="X">
</div>"""

type Self = { self : obj }

let mouseDown element = ()

let px value = sprintf "%ipx" value

let toInt value = value |> sprintf "%O" |> int

let border heightValue widthvalue topValue leftValue =
  let element = jq $ "<div>"
  element?addClass("canopy_companion_border")
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
        
    border border_width (width + border_padding * 2 + border_width * 2) (top - border_width - border_padding) (left - border_padding - border_width)
    border (border_width + 6) (width + border_padding * 2 + border_width * 2 - 5) (top + height + border_padding) (left - border_padding - border_width)
    border (height + border_padding * 2) border_width (top - border_padding) (left - border_padding - border_width)
    border (height + border_padding * 2) border_width (top - border_padding) (left + width + border_padding)
  ) |> ignore
  
if not (exists "#canopy_companion") then
  append "body" inputs

  click "#canopy_companion #go" (fun _ ->   
    let selector = value "#selector"  
    remove ".canopy_companion_border"
    createBorders (find selector))

  click "#canopy_companion #clear" (fun _ ->
    set "#selector" ""
    remove ".canopy_companion_border")

  click "#canopy_companion #close" (fun _ -> 
    remove ".canopy_companion_border"
    remove "#canopy_companion")

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