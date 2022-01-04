﻿[<AutoOpen>]
module Day15

open System
open System.IO

type dirs =
    | up = 0
    | right = 1
    | down = 2
    | left = 3

//Weight is defined as an option, with None being used to represent +infinity
type pointData = {pos: int * int; weight: option<int>; heuristic: int; cameFrom: dirs}
type workingState = {mutable activePoints: pointData list; mutable array: pointData[,]}

//Common - new
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day15Input.txt")
    let splitText = fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    Array2D.init splitText.Length splitText.[0].Length (fun i j -> int splitText.[i].[j] - int '0' )

let getHeuristic (i,j) (sourceArray: int[,]) = (Array2D.length1 sourceArray) - i + (Array2D.length2 sourceArray) - j

let inBounds (i,j) (sourceArray: int[,]) = i >= 0 && i < (Array2D.length1 sourceArray) && j >= 0 && j < (Array2D.length2 sourceArray)


//Find the current active point of minimum weight
let lt (a: option<int>) (b: option<int>) =
    match a with
        | None -> false
        | _ -> if b.IsNone then true else a < b

//Add function to allow for adding weight to heuristic given weight might be None
let add (a: option<int>) (b: int) =
    if a.IsNone then None else Some ((a.Value) + b)

(*
let rec findMinPointIter (i, j) (minPoint: pointData) workingState =
    
    //See if the weight + heuristic of the current point is less than the lowest seen
    let newMin = if lt (add workingState.array.[i,j].weight workingState.array.[i,j].heuristic) (add minPoint.weight minPoint.heuristic) then
                    workingState.array.[i,j]
                 else
                    minPoint

    //Continue the iteration within the bounds of the working state's max point
    if i = workingState.maxI then
        if j = workingState.maxJ then
            newMin
        else
            findMinPointIter (0, j+1) newMin workingState
    else
        findMinPointIter (i+1, j) newMin workingState

let findMinPoint workingState =
    findMinPointIter (0,0) workingState.array.[0,0] workingState

*)

let getMinPoint (pointList: pointData list) =
    //Since we are working in the list of points visited, we know that none of them will have None for a weight
    let (min, minIndex, index) = List.fold (fun (min, minIndex, index) x -> if x.weight.Value + x.heuristic < min then (x.weight.Value + x.heuristic, index, index + 1) 
                                                                            else (min, minIndex, index + 1)
                                           ) (pointList.Head.weight.Value + pointList.Head.heuristic, 0, 1) pointList.Tail
    minIndex

let extractItem index pathList =
    let (front, back) = List.splitAt (index) pathList
    (back.Head, front @ back.Tail)

//Get the next points to be looked at   
let getNextPoints (currentPoint: pointData) sourceArray =
    
    let (i,j) = currentPoint.pos

    let dirArray = [|(int currentPoint.cameFrom + 3) % 4 ; int currentPoint.cameFrom ; (int currentPoint.cameFrom + 1) % 4  |]    
    Array.map (fun dir -> (dir, match enum<dirs>dir with
                                    | dirs.up -> (i+1, j)
                                    | dirs.right -> (i, j-1)
                                    | dirs.down -> (i-1,j)
                                    | dirs.left -> (i, j+1)
                                    | _ -> failwith "invalid point passed")) dirArray
              
    |> Array.fold (fun newPoints (dir, (newI, newJ)) -> if inBounds (newI, newJ) sourceArray then
                                                            {pos = (newI, newJ); weight = add currentPoint.weight sourceArray.[newI,newJ]; heuristic = getHeuristic (newI, newJ) sourceArray; cameFrom = enum<dirs>dir} :: newPoints
                                                        else
                                                           newPoints
                  ) List.empty<pointData>       

let rec calculateStep workingState sourceArray =
    
    //Find the point of minimum weight in the active points list, and remove it from the list
    let nextPointIndex = getMinPoint workingState.activePoints
    let (nextPoint, cutList) = extractItem nextPointIndex workingState.activePoints
    workingState.activePoints <- cutList
    let newPoints = getNextPoints nextPoint sourceArray

    List.iter (fun point -> 
                    let (i,j) = point.pos
                    //If our recent evaluation of the point has lower weight than we have seen previously, update the array with the new value and re-add the point to the active consideration list
                    if lt point.weight workingState.array.[i,j].weight then
                        workingState.array.[i,j] <- point
                        workingState.activePoints <- point :: workingState.activePoints
                    else
                        ()
    
               ) newPoints

    if (workingState.array.[Array2D.length1 sourceArray - 1, Array2D.length2 sourceArray - 1].weight.IsSome) then
        workingState.array.[Array2D.length1 sourceArray - 1, Array2D.length2 sourceArray - 1].weight.Value
    else
        calculateStep workingState sourceArray

let findShortestPath (sourceArray: int[,]) =
    //Explicitly populate the first point to start off the iteration
    let firstPoint = {pos = (0,0); weight = Some 0; heuristic = getHeuristic (0,0) sourceArray; cameFrom = dirs.up}

    let mutable workingData = {activePoints = [firstPoint];
                               array = Array2D.init (Array2D.length1 sourceArray) (Array2D.length2 sourceArray) 
                                                    (fun i j -> {pos = (i,j); weight = None; heuristic = 0; cameFrom = dirs.up}) //Initialise heuristic as 0, this will get set as the points are considered
                               }
    
    workingData.array.[0,0] <- firstPoint
    calculateStep workingData sourceArray

//Part 1


//Part 2
let buildPart2 (sourceData: int[,]) copies =

    let dimI = Array2D.length1 sourceData
    let dimJ = Array2D.length2 sourceData

    Array2D.init (dimI * copies) (dimJ * copies) (fun i j -> sourceData.[i % dimI, j % dimJ])


//Entry point
let main projectDir =

    let sourceData = getText projectDir
    let part2Data = buildPart2 sourceData

    let part1 = findShortestPath sourceData

    Console.WriteLine("Part 1: " + part1.ToString() )
    Console.WriteLine("Part 2: Not Implemented."  )
    15