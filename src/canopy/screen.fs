namespace canopy
module screen =

    open System

    type ScreenBoundary =
        { width : int
          height: int
          x: int
          y: int
          size: Drawing.Size
        }

#if NETSTANDARD2_0
#else
    module microsoftDotNet =
        let getPrimaryScreenResolution () =
            let h = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height
            let w = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width
            (w, h)

        let getPrimaryScreenBounds () =
            { width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width
              height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height
              x = System.Windows.Forms.Screen.PrimaryScreen.Bounds.X
              y = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y
              size = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size
            }

        let monitorCount () =
            System.Windows.Forms.SystemInformation.MonitorCount

        let allScreensWorkingArea () =
            System.Windows.Forms.Screen.AllScreens
            |> Array.map(fun x -> x.WorkingArea)
#endif

    module mono =
        let mutable screenWidth = 1280
        let mutable screenHeight = 800

        let getPrimaryScreenResolution () =
            (screenWidth, screenHeight)

        let getPrimaryScreenBounds () =
            { width = screenWidth
              height = screenHeight
              x = 0
              y = 0
              size = Drawing.Size(screenWidth, screenHeight)
            }

        let monitorCount () =
            1

        let allScreensWorkingArea () =
            [| Drawing.Rectangle(0, 0, screenWidth, screenHeight)
            |]


    let private is64BitMono =
        Environment.Is64BitProcess && Type.GetType("Mono.Runtime") <> null

    let getPrimaryScreenResolution () =
#if NETSTANDARD2_0
        mono.getPrimaryScreenResolution ()
#else
        if is64BitMono then mono.getPrimaryScreenResolution ()
        else microsoftDotNet.getPrimaryScreenResolution ()
#endif

    let getPrimaryScreenBounds () =
#if NETSTANDARD2_0
        mono.getPrimaryScreenBounds ()
#else
        if is64BitMono then mono.getPrimaryScreenBounds ()
        else microsoftDotNet.getPrimaryScreenBounds ()
#endif

    let monitorCount =
#if NETSTANDARD2_0
        mono.monitorCount ()
#else
        if is64BitMono then mono.monitorCount ()
        else microsoftDotNet.monitorCount ()
#endif

    let allScreensWorkingArea =
#if NETSTANDARD2_0
        mono.allScreensWorkingArea ()
#else
        if is64BitMono then mono.allScreensWorkingArea ()
        else microsoftDotNet.allScreensWorkingArea ()
#endif
