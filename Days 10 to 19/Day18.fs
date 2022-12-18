[<AutoOpen>]
module Day18

open System
open System.IO

//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day18Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
        |> Array.map (fun line -> Array.map (fun item -> Int32.Parse(item)) (line.Split(',')))
        
    let isAdjacent point1 point2 =
        
        let combined = Array.zip point1 point2

        let similarPoints = Array.fold (fun count (coord1, coord2) -> if coord1 = coord2 then count + 1 else count) 0 combined

        if similarPoints <> 2 then 
            false
        else
            let (diffPoint1, diffPoint2) = Array.find(fun (coord1, coord2) -> coord1 <> coord2) combined
            Math.Abs(diffPoint1 - diffPoint2: int) = 1

    let mapObject input = Array.fold (fun (count, structure) point -> 
                            let adjacencyCount = List.fold (fun adjCount structurePoint -> if isAdjacent point structurePoint then adjCount + 1 else adjCount) 0 structure
                            (count + (6 - 2*adjacencyCount), point :: structure)
                            ) (0, List.empty<int[]>) input
    
    //Part 1
    let part1Answer = mapObject sourceData

    //Part 2
    let (maxX, maxY, maxZ) = Array.fold (fun (maxX, maxY, maxZ) [|x;y;z|] -> (max maxX x, max maxY y, max maxZ z)) (0,0,0) sourceData

    let mutable fillArray = Array.init (maxX + 3) (fun i -> Array.init (maxY + 3) (fun j -> Array.init (maxZ + 3) (fun k -> '-')))

    //offset the point coordinates by 1 so that we have a buffer around the edge
    List.iter (fun [|x;y;z|] -> fillArray.[x+1].[y+1].[z+1] <- '+') (snd part1Answer)

    let volumeFlood =

        let rec volumeFloodIter pointsToCheck =

            let getAdjacentPoints (px,py,pz) =          
                [(1,0,0);(-1,0,0);(0,1,0);(0,-1,0);(0,0,1);(0,0,-1)]
                |> List.map (fun (ox,oy,oz) -> (px + ox, py + oy, pz + oz))
                |> List.filter (fun (x,y,z) -> x >= 0 && x <= maxX + 2 && y >= 0 && y <= maxY + 2 && z >= 0 && z <= maxZ + 2) //filter to only points within the bounds of the array
                |> List.filter (fun (x,y,z) -> fillArray.[x].[y].[z] = '-') //filter to only unchecked points

            let (x,y,z) = List.head pointsToCheck

            fillArray.[x].[y].[z] <- '#'
            let newList = getAdjacentPoints (x,y,z) @ (List.tail pointsToCheck)

            if List.isEmpty newList then 
                ()
            else
                volumeFloodIter newList
    
        volumeFloodIter [(0,0,0)]
    

    let mutable innerPoints = List.empty
    
    for i in 0 .. fillArray.Length - 1 do
        for j in 0 .. fillArray.[0].Length - 1 do
            for k in 0 .. fillArray.[0].[0].Length - 1 do
                if fillArray.[i].[j].[k] = '-' then innerPoints <- [|i;j;k|] :: innerPoints

    let innerSurface = fst (mapObject (Array.ofList innerPoints))

    let part2Answer = (fst part1Answer) - innerSurface

    //Output

    Console.WriteLine("Part 1: " + (fst part1Answer).ToString() )
    Console.WriteLine("Part 2: " + part2Answer.ToString() )
    18