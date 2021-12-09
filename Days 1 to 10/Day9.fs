[<AutoOpen>]
module Day9

open System
open System.IO

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day9Input.txt")
    let stringArray = fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    Array2D.init stringArray.[0].Length stringArray.Length (fun i j -> int stringArray.[i].[j] - int '0')
 
 //Part 1

let testPoint i j (sourceArray: int [,]) point =
    if i >= 0 && i < sourceArray.GetLength(0) &&
       j >= 0 && j < sourceArray.GetLength(1) then
        sourceArray.[i,j] > point
    else
        true
    
let isPit i j (sourceArray: int [,]) =
    let element = sourceArray.[i,j]

    testPoint (i-1) j sourceArray element &&
    testPoint (i+1) j sourceArray element &&
    testPoint i (j-1) sourceArray element &&
    testPoint i (j+1) sourceArray element

let part1Compute sourceArray =   
    Array2D.mapi(fun i j x -> if isPit i j sourceArray then x + 1 else 0) sourceArray
    |> Seq.cast<int>
    |> Seq.sum 

 //Part 2



 //Entry point
let main projectDir =

    let sourceData = getText projectDir

    Console.WriteLine("Part 1: " + (part1Compute sourceData).ToString()  )
    Console.WriteLine("Part 2: Not Implemented."  )
    9