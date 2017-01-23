
#r "node_modules/fable-core/Fable.Core.dll"

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

type Self = { self : obj }

let jq = importDefault<obj> "jquery"
let find selector = jq $ selector
let append selector element = (find selector)?append(element) |> ignore
let css selector property value =  (find selector)?css(property, value) |> ignore
let click selector f =  (find selector)?click(f) |> ignore
let value selector = (find selector)?``val``() 
let set selector value = (find selector)?``val``(value) |> ignore
let tag element = (element?prop("tagName") |> string).ToUpper()
let remove selector = (find selector)?remove() |> ignore
let hide selector = (find selector)?hide() |> ignore
let exists selector = 
  let value = (find selector)?("length") |> sprintf "%O" |> int
  value > 0
let bind element event (f : (obj -> unit)) = 
  let element = jq $ element  
  element?bind(event, { self = element }, f) |> ignore
let bindEach selector event (f : (obj -> unit)) = 
  let elements = find selector
  jq?each(elements, fun index element -> bind element event f) |> ignore
let bool whatever = 
  match sprintf "%O" whatever with
  | "true" -> true
  | _      -> false
let px value = sprintf "%ipx" value
let toInt value = value |> sprintf "%O" |> int

let border_width = 5
let border_padding = 2

let inputs = """
<div id="canopy_companion" class="canopy_companion_module">
  <input type="text" id="selector" class="canopy_companion_module" value="">
  <input type="button" id="go" class="canopy_companion_module" value="Go">
  <input type="button" id="clear" class="canopy_companion_module" value="Clear">
  <input type="button" id="close" class="canopy_companion_module" value="X">
</div>"""

let top =    (jq $ "<div>")?addClass("canopy_companion_border")?addClass("canopy_companion_border_top")
let bottom = (jq $ "<div>")?addClass("canopy_companion_border")?addClass("canopy_companion_border_bottom")
let left =   (jq $ "<div>")?addClass("canopy_companion_border")?addClass("canopy_companion_border_left")
let right =  (jq $ "<div>")?addClass("canopy_companion_border")?addClass("canopy_companion_border_right")

let border position heightValue widthvalue topValue leftValue =
  let element = find (sprintf ".canopy_companion_border_%s" position)
  element
    ?css("height", px heightValue)
    ?css("width", px widthvalue)
    ?css("top", px topValue)
    ?css("left", px leftValue)
    ?show()
    |> ignore

let createBorders elements = 
  jq?each(elements, fun index element ->
    let clone = jq $ element
    let position = clone?offset()
    let top = position?("top") |> toInt
    let left = position?("left")  |> toInt
    let width = clone?outerWidth() |> toInt
    let height = clone?outerHeight() |> toInt
        
    border "top"    border_width (width + border_padding * 2 + border_width * 2) (top - border_width - border_padding) (left - border_padding - border_width)
    border "bottom" border_width (width + border_padding * 2 + border_width * 2) (top + height + border_padding) (left - border_padding - border_width)
    border "left"   (height + border_padding * 2) border_width (top - border_padding) (left - border_padding - border_width)
    border "right"  (height + border_padding * 2) border_width (top - border_padding) (left + width + border_padding)
  ) |> ignore
  
let mouseEnter event = 
  let element = jq $ event?data?self
  let tag = tag element
  if tag <> "BODY" && tag <> "HTML" then
    hide ".canopy_companion_border"
    createBorders element

let mouseLeave event = 
  let element = jq $ event?data?self
  let tag = tag element
  if tag <> "BODY" && tag <> "HTML" then
    hide ".canopy_companion_border"

let blockClick element =
  let clone = jq $ element
  let position = clone?offset()
  let top = position?("top") |> toInt
  let left = position?("left")  |> toInt
  let width = clone?outerWidth() |> toInt
  let height = clone?outerHeight() |> toInt

  let block = 
    (jq $ "<div>")
      ?css("position", "absolute")
      ?css("z-index",  "9999999")
      ?css("width",  px width)
      ?css("height", px height)
      ?css("top",    px top)
      ?css("left",   px left)
      ?css("background-color", "")

  append "body" block
  window.setTimeout((fun _ -> block?remove()), 400.) |> ignore

let mouseDown event = 
  let element = jq $ event?data?self
  let tag = tag element
  if tag <> "BODY" && tag <> "HTML" then
    blockClick element

if not (exists "#canopy_companion") then
  append "body" top
  append "body" bottom
  append "body" left
  append "body" right

  bindEach "*:not(.canopy_companion_module)" "mouseenter" mouseEnter
  bindEach "*:not(.canopy_companion_module)" "mouseleave" mouseLeave
  bindEach "*:not(.canopy_companion_module)" "mousedown" mouseDown
  
  append "body" inputs

  click "#canopy_companion #go" (fun _ ->   
    let selector = value "#selector"  
    hide ".canopy_companion_border"
    createBorders (find selector))

  click "#canopy_companion #clear" (fun _ ->
    set "#selector" ""
    hide ".canopy_companion_border")

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