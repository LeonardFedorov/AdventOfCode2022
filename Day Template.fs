﻿[<AutoOpen>]
module DayX

open System
open System.IO

//Entry point
let main projectDir =

    //Common
    let getText projectDir =
        let fileStream = new StreamReader(projectDir + "\\DayXInput.txt")
        fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)


    //Part 1


    //Part 2


    let sourceData = getText projectDir

    Console.WriteLine("Part 1: Not Implemented."  )
    Console.WriteLine("Part 2: Not Implemented."  )
    -1