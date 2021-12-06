[<AutoOpen>]
module Day5

open System
open System.IO

type lineDirection =
| Horizontal = 0
| Vertical = 1
| upDiagonal = 2
| downDiagonal = 3

type lineData = {dir: lineDirection; maxValue: int; line: (int * int) * (int * int)}

//Parse an individual line as a lineData
let lineParse (lineString: string) =
    //Parse the line coordinates and convert the coordinates to ints
    let parsedLine = lineString.Split([|" -> "|], StringSplitOptions.None)
                     |> Array.map (fun x -> x.Split(','))
    let point1 = (Int32.Parse(parsedLine.[0].[0]), Int32.Parse(parsedLine.[0].[1]))
    let point2 = (Int32.Parse(parsedLine.[1].[0]), Int32.Parse(parsedLine.[1].[1]))

    //Determine the direction of the line
    let direction = if fst point1 = fst point2 then lineDirection.Vertical
                    elif snd point1 = snd point2 then lineDirection.Horizontal
                    elif (fst point1 + snd point1) = (fst point2 + snd point2) then lineDirection.downDiagonal
                    else lineDirection.upDiagonal

    //Calculate the max coordinate present so we can size our array appropriately later
    let valueMax = max (max (max (fst point1) (snd point1)) (fst point2)) (snd point2)

    //Swap the points around if needed so that the lines are stored in a consistent left to right direction
    let directedLine = if direction <> lineDirection.Vertical then 
                          if fst point1 < fst point2 then (point1, point2) else (point2, point1)
                       else 
                          if snd point1 < snd point2 then (point1, point2) else (point2, point1)

    {dir = direction; maxValue = valueMax; line = directedLine}

//Get the input data and parse it into an array of lineDatas   
let day5input =
    let fileStream = new StreamReader("C:\Documents\Advent of Code\Day5Input.txt")
    fileStream.ReadToEnd().Split([|"\n"|], StringSplitOptions.None)
    |> Array.map (fun x -> lineParse x)

//test if the point (i,j) lies on the line specified
let isOnLine i j (line: lineData) =
    let point1 = fst line.line
    let point2 = snd line.line
    if line.dir = lineDirection.Horizontal then
        (j = snd point1) && (i >= fst point1) && (i <= fst point2) 
    elif line.dir = lineDirection.Vertical then
        (i = fst point1) && (j >= snd point1) && (j <= snd point2) 
    elif line.dir = lineDirection.downDiagonal then
        (i + j = fst point1 + snd point1) && (i >= fst point1) && (i <= fst point2) 
    elif line.dir = lineDirection.upDiagonal then
        (i - fst point1 = j - snd point1) && (i >= fst point1) && (i <= fst point2) 
    else
        false

let updateMap map line =
    Array2D.mapi (fun i j x -> if (isOnLine i j line) then x + 1 else x) map
    
let countPoints map value =
    Seq.fold (fun s x -> s + if x >= value then 1 else 0) 0 (Seq.cast<int> map)

let mainDay5 =
    let sourceData = day5input
    let maxDim = Array.fold (fun s x -> max s x.maxValue) 0 sourceData
    let ventMap = Array.fold (fun map line -> updateMap map line) (Array2D.create maxDim maxDim 0) sourceData
//    let ventMap2 = Array2D.init maxDim maxDim (fun i j -> Array.fold (fun s x -> if isOnLine i j x then s + 1 else s) 0 sourceData)
    
    Console.WriteLine("Maximum coordinate found: " + maxDim.ToString())
    Console.WriteLine("Count of 2+ locations: " + (countPoints ventMap 2).ToString())

    5 //expected integer return code