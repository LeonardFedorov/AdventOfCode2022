[<AutoOpen>]
module DayX

open System
open System.IO

let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\DayXInput.txt")
    fileStream.ReadToEnd()
 

let mainDayX projectDir =
    let sourceData = getText projectDir

    Console.WriteLine("Part 1: "  )
    Console.WriteLine("Part 2: "  )
    1