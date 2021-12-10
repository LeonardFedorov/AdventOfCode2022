[<AutoOpen>]
module Day9

open System
open System.IO

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day9Input.txt")
    fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    |> Array.map (fun stringRow -> Array.map (fun c -> int c - int '0') (stringRow.ToCharArray()) )
 
 //Part 1

let testPoint i j (sourceArray: int [][]) point =
    if i >= 0 && i < sourceArray.Length &&
       j >= 0 && j < sourceArray.[i].Length then
        sourceArray.[i].[j] > point
    else
        true
    
let isPit i j (sourceArray: int[][]) =
    let element = sourceArray.[i].[j]

    testPoint (i-1) j sourceArray element &&
    testPoint (i+1) j sourceArray element &&
    testPoint i (j-1) sourceArray element &&
    testPoint i (j+1) sourceArray element

let getLowsfromRow sourceRow lowList sourceArray i =
    Array.fold (fun (list, j) x -> if (isPit i j sourceArray) then ((i,j) :: list, j+1) else (list, j+1)) (lowList, 0) sourceRow
    |> fst

let getLowList sourceArray =   
    Array.fold (fun (list, i) x -> ((getLowsfromRow x list sourceArray i), i+1) ) (List.empty<(int * int)> , 0) sourceArray
    |> fst

 //Part 2



 //Entry point
let main projectDir =

    let sourceData = getText projectDir
    let lowsList = getLowList sourceData
    let riskSum = List.fold(fun s (i,j) -> s + 1 + sourceData.[i].[j]) 0 lowsList



    Console.WriteLine("Part 1: " + riskSum.ToString() )
    Console.WriteLine("Part 2: Not Implemented."  )
    9