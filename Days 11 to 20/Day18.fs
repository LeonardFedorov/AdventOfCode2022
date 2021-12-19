[<AutoOpen>]
module Day18

open System
open System.IO

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day18Input.txt")
    fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)


let doSplit snailNumber =
    
    snailNumber

//Part 2


//Entry point
let main projectDir =

    let sourceData = getText projectDir

    Console.WriteLine("Part 1: Not Implemented."  )
    Console.WriteLine("Part 2: Not Implemented."  )
    18