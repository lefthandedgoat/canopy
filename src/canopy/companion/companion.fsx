
#r "node_modules/fable-core/Fable.Core.dll"

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

let jq = importDefault<obj> "jquery"
let go = jq $ ("#go")

let detail = jq $ (".detail")

detail ? click(fun _ ->   
  printfn "I have been clicked!!"
  ()) |> ignore

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