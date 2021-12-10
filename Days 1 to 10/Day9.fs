[<AutoOpen>]
module Day9

open System
open System.IO

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day9Input.txt")
    fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    |> Array.map (fun stringRow -> Array.map (fun c -> int c - int '0') (stringRow.ToCharArray()) )

let boundsTest i j (sourceArray: 'a [][]) =
    i >= 0 && i < sourceArray.Length && j >= 0 && j < sourceArray.[i].Length

 //Part 1

let testPoint i j (sourceArray: int [][]) point =
    if boundsTest i j sourceArray then
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

let considerPoint sourceArray (i, j) (basinList, considerQueue) =   
    //If the point to be considered is out of bounds, equal to 9, or already in the basin
    //then no updates are required
    if not (boundsTest i j sourceArray) then
        (basinList, considerQueue)
    elif sourceArray.[i].[j] = 9 then
        (basinList, considerQueue)
    elif List.exists (fun x -> x = (i, j)) basinList then
        (basinList, considerQueue)
    else
        //If the point doesn't satisfy the above criteria, it is a new member of the basin so add it
        //to the basin list and the consideration queue for later investigation
        ((i, j) :: basinList, (i, j) :: considerQueue)

//Primary iterator to extract items from the considerQueue in turn, ends when no further items are in the queue
let rec basinIter sourceArray (basinList, (considerQueue: (int * int) list) ) =
    let (i, j) = considerQueue.Head
    let remainingPoints = considerQueue.Tail

    let iteratedLists = (basinList, remainingPoints)
                        |> considerPoint sourceArray (i-1, j)
                        |> considerPoint sourceArray (i+1, j)
                        |> considerPoint sourceArray (i, j-1)
                        |> considerPoint sourceArray (i, j+1)
    
    if (snd iteratedLists).IsEmpty then
        fst iteratedLists
    else
        basinIter sourceArray iteratedLists

//Wrapper to initialise the iterator with a starting point in the basin
let basinSearch sourceArray startPoint =
    basinIter sourceArray ([startPoint], [startPoint])

let rec findBasins sourceArray (startPointList: (int*int) list) basinsList =
    if startPointList.IsEmpty then
        basinsList
    else
        let startPoint = startPointList.Head
        let newBasinsList = (basinIter sourceArray ([startPoint], [startPoint])) :: basinsList
        let newStartPointList = List.except (Seq.cast<(int * int)> newBasinsList.Head) startPointList.Tail

        if newStartPointList.IsEmpty then
            newBasinsList
        else
            findBasins sourceArray newStartPointList newBasinsList

 //Entry point
let main projectDir =

    let sourceData = getText projectDir
    let lowsList = getLowList sourceData
    let riskSum = List.fold(fun s (i,j) -> s + 1 + sourceData.[i].[j]) 0 lowsList
    let basinList = findBasins sourceData lowsList List.empty<(int*int) list>
    //Get the top 3
    let (a,b,c) = List.fold (fun (a,b,c) (basin: (int*int) list) -> if basin.Length > a then (basin.Length, a, b)
                                                                    elif basin.Length > b then (a, basin.Length, b) 
                                                                    elif basin.Length > c then (a, b, basin.Length)
                                                                    else (a,b,c) 
                            ) (0,0,0) basinList

    Console.WriteLine("Part 1: " + riskSum.ToString() )
    Console.WriteLine("Part 2: " + (a*b*c).ToString()  )
    9