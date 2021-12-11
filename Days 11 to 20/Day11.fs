[<AutoOpen>]
module Day11

open System
open System.IO

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day11Input.txt")
    fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    |> Array.map (fun string -> Array.map (fun char -> int char - int '0') (string.ToCharArray()) )
 
//Part 1

//2d fold to get a list of the co-ordinates where the 10s are
let get10sfromRow sourceRow lowList i =
    Array.fold (fun (list, j) x -> if x = 10 then ((i,j) :: list, j+1) else (list, j+1)) (lowList, 0) sourceRow
    |> fst

let get10List sourceArray =   
    Array.fold (fun (list, i) x -> ((get10sfromRow x list i), i+1) ) (List.empty<(int * int)> , 0) sourceArray
    |> fst

let isAdjacent (a,b) (c,d) = abs(a - c) <= 1 && abs(b - d) <= 1

//Get a point's new value after processing neighbouring flashes
let updatePoint (a,b) element (flashList: (int*int) list) =

    //If the octopus has already flashed this round, or is flashing now, then don't need to calculate further on it
    if element = 0 || element = 10 then 0
    else 
        let flashCount = List.filter (fun flashPoint -> isAdjacent (a,b) flashPoint) flashList 
                         |> List.length
        min 10 (element + flashCount)

let rec processFlashes (octopusMap: int[][], flashCount: int) (flashList: (int*int) list) =
    if flashList.IsEmpty then
        (octopusMap, flashCount)
    else
        let newOctopusMap = Array.mapi (fun i row -> Array.mapi (fun j element -> updatePoint (i,j) (octopusMap.[i].[j]) flashList) row ) octopusMap
        processFlashes (newOctopusMap, flashCount + flashList.Length) (get10List newOctopusMap)
   

let rec calculateCycles (octopusMap, flashCount: int) iter =
    if iter = 0 then 
        flashCount
    else
        //First, the natural increase of 1 to every octopus
        let newOctopusMap = Array.map (fun row -> Array.map (fun element -> element + 1) row) octopusMap
        
        let cycleResult = processFlashes (newOctopusMap, flashCount) (get10List newOctopusMap)

        calculateCycles cycleResult (iter - 1)

//Part 2

//Entry point
let main projectDir =

    let sourceData = getText projectDir

    let flashes = calculateCycles (sourceData, 0) 100

    Console.WriteLine("Part 1: " + flashes.ToString() )
    Console.WriteLine("Part 2: Not Implemented."  )
    -1