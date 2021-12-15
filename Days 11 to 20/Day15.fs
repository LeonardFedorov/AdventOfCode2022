[<AutoOpen>]
module Day15

open System
open System.IO

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day15Input.txt")
    let splitText = fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    Array2D.init splitText.Length splitText.[0].Length (fun i j -> int splitText.[i].[j] - int '0' )

//Part 1


//Part 2


//Entry point
let main projectDir =

    let sourceData = getText projectDir

    Console.WriteLine("Part 1: Not Implemented."  )
    Console.WriteLine("Part 2: Not Implemented."  )
    15