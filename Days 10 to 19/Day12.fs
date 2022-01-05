[<AutoOpen>]
module Day12

open System
open System.IO

let startCave = "start"
let endCave = "end"

//Common
let buildNeighbourlist place pathMap =
    //Fold through the path map, building up the neighbour list
    //Exclude start as we never want to path back to the start, and don't calculate neighbours for the end
    if place = endCave then [endCave]
    else
        place :: Array.fold (fun neighbours (path: string[]) ->  if path.[0] = place && path.[1] <> startCave then
                                                                    path.[1] :: neighbours
                                                                 elif path.[1] = place && path.[0] <> startCave then
                                                                    path.[0] :: neighbours
                                                                 else
                                                                    neighbours
                            ) List.empty pathMap
        
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day12Input.txt")
    let pathMap = fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
                  |> Array.map (fun x -> x.Split('-'))

    //Build a list of all of the places
    let placeList = Array.fold (fun list (path: string[]) -> path.[0] :: path.[1] :: list) List.Empty pathMap
                    |> List.distinct
 
    //Now build a list that is headed by each location and then contains it's connections thereafter
    List.map (fun place -> buildNeighbourlist place pathMap) placeList

let isBigCave (place: string) =
    int place.[0] < int '[' //ASCII char immediately after the upper cases

let smallCaveAllowed (place: string) smallCaveMax pathSoFar =
    let smallCavesOnly = List.filter (fun x -> not (isBigCave x)) pathSoFar

    //If this cave hasn't been visited before, then it's allowed
    if None = List.tryFind (fun x -> x = place) smallCavesOnly then
        true
    else
        //Find if there is a cave that has been visited multiple times in the past
        let multipleCave = List.tryFind (fun x -> (List.filter(fun y -> x = y) smallCavesOnly).Length > 1) smallCavesOnly
        //If no multiple caves are found, then we can add this cave so long as the max cave allowance is greater than 1
        if multipleCave = None then
            smallCaveMax > 1
        //If the cave that has been visited multiple times is the cave we are currently checking, and the number of times that we have visited it
        elif multipleCave.Value = place && (List.filter (fun x -> multipleCave.Value = x) smallCavesOnly).Length < smallCaveMax then
            true
        else
            false

let rec dispatchSearches neighbourLists smallCaveMax pathSoFar pathsFound (nextPlaces: string list) =
    if nextPlaces.Length = 0 then
        pathsFound
    else 
        let updatedPathsFound = getNeighbours neighbourLists smallCaveMax (nextPlaces.Head :: pathSoFar) pathsFound
        dispatchSearches neighbourLists smallCaveMax pathSoFar updatedPathsFound nextPlaces.Tail

and getNeighbours (neighbourLists: string list list) smallCaveMax (pathSoFar: string list) (pathsFound: string list list) =
    let place = pathSoFar.Head

    if place = endCave then pathSoFar :: pathsFound
    else
        let nextPlaces = List.find (fun (list: string list) -> list.Head = place) neighbourLists
                         |> List.tail
                         |> List.filter (fun candidate -> isBigCave candidate || smallCaveAllowed candidate smallCaveMax pathSoFar)

        //If there are no valid next places, abandon the path and return nothing new
        if nextPlaces.Length = 0 then 
            pathsFound
        else 
            dispatchSearches neighbourLists smallCaveMax pathSoFar pathsFound nextPlaces

//Entry point
let main projectDir =

    let neighboursLists = getText projectDir

    let listOfPathsP1 = getNeighbours neighboursLists 1 [startCave] list.Empty
    let listOfPathsP2 = getNeighbours neighboursLists 2 [startCave] list.Empty

    Console.WriteLine("Part 1: " + listOfPathsP1.Length.ToString() )
    Console.WriteLine("Part 2: " + listOfPathsP2.Length.ToString() )
    12