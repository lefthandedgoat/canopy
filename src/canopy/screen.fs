module canopy.screen

open System

let mutable screenWidth = 1920
let mutable screenHeight = 1080
let mutable monitorCount = 1

type ScreenBoundary =
    { 
        width : int
        height: int
        x: int
        y: int
        size: Drawing.Size
    }    
        
let getPrimaryScreenResolution () = screenWidth, screenHeight

let getPrimaryScreenBounds () =
    { 
        width = screenWidth
        height = screenHeight
        x = 0
        y = 0
        size = Drawing.Size(screenWidth, screenHeight)
    }
