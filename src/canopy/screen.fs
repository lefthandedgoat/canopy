namespace canopy
module screen =

  open System

  type ScreenBoundary = { 
    Width : int
    Height: int
    X: int
    Y: int
    Size: Drawing.Size
  }

  module microsoftDotNet =
    let getPrimaryScreenResolution () =
      let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height
      let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width
      (w, h)

    let getPrimaryScreenBounds () =
      { 
        Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width
        Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height
        X = System.Windows.Forms.Screen.PrimaryScreen.Bounds.X
        Y = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y
        Size = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size
      }

    let monitorCount () = System.Windows.Forms.SystemInformation.MonitorCount

    let allScreensWorkingArea () =
      System.Windows.Forms.Screen.AllScreens
      |> Array.map(fun x -> x.WorkingArea)

  module mono =
    let mutable screenWidth = 1280
    let mutable screenHeight = 800

    let getPrimaryScreenResolution () = (screenWidth, screenHeight)
    
    let getPrimaryScreenBounds () =
      { 
        Width = screenWidth
        Height = screenHeight
        X = 0
        Y = 0
        Size = Drawing.Size(screenWidth, screenHeight)
      }

    let monitorCount () = 1

    let allScreensWorkingArea () = [| Drawing.Rectangle(0, 0, screenWidth, screenHeight) |]

  let private is64BitMono = Environment.Is64BitProcess && Type.GetType("Mono.Runtime") <> null

  let getPrimaryScreenResolution () =
    if is64BitMono then mono.getPrimaryScreenResolution ()
    else microsoftDotNet.getPrimaryScreenResolution ()

  let getPrimaryScreenBounds () =
    if is64BitMono then mono.getPrimaryScreenBounds ()
    else microsoftDotNet.getPrimaryScreenBounds ()

  let monitorCount =
    if is64BitMono then mono.monitorCount ()
    else microsoftDotNet.monitorCount ()

  let allScreensWorkingArea =
    if is64BitMono then mono.allScreensWorkingArea ()
    else microsoftDotNet.allScreensWorkingArea ()