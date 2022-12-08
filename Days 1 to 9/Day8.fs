[<AutoOpen>]
module Day8

open System
open System.IO

//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day8Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)

    let forestWidth = sourceData.[0].Length
    let forestHeight = sourceData.Length

    let sourceData2D =
        Array2D.init forestHeight forestWidth (fun i j -> int sourceData.[i].[j] - 48)

    //Part 1
    let getHiddenTrees =
        //A 0 tree will be hidden. Start with all trees as hidden, and we will mark off those that are visible
        let mutable hiddenMap = Array.init forestHeight (fun i -> Array.create forestWidth 0)

        let mutable currentMax = 0

        //If the tree is taller than the current max trees found, mark as visible
        //Also, update the current max
        let loopStep i j = 
            if sourceData2D.[i,j] > currentMax then hiddenMap.[i].[j] <- 1
                                                    currentMax <- sourceData2D.[i,j]

        //First by row from left to right, then right to left
        for i in 0 .. forestHeight - 1 do
            currentMax <- -1
            for j in 0 .. forestWidth - 1 do loopStep i j
            currentMax <- -1
            for j in forestWidth - 1 .. -1 .. 0  do loopStep i j
 
        //Now by column bottom to top, then top to bottom
        for j in 0 .. forestWidth - 1 do
            currentMax <- -1
            for i in forestHeight - 1 .. -1 .. 0 do loopStep i j   
            currentMax <- -1
            for i in 0 .. forestHeight - 1 do loopStep i j

        hiddenMap

    //Part 2
    let scenicScores =
        
        let scenicScore i j =

            let thisTree = sourceData2D.[i,j]
            
            //Find the first tree that is at least as tall as the start tree. If no such tree exists,
            //then the whole row is to be counted
            let scenicScore lineToEnd =
                let search = Array.tryFindIndex (fun tree -> tree >= thisTree) lineToEnd
                if search = None then
                    lineToEnd.Length
                else 
                    search.Value + 1

            //Array slicers to the rescue
            let rightScore = scenicScore sourceData2D.[i,j+1..]
            let leftScore = scenicScore (Array.rev sourceData2D.[i,..j-1])
            let downScore = scenicScore sourceData2D.[i+1..,j]
            let upScore = scenicScore (Array.rev sourceData2D.[..i-1,j])
    
            rightScore * leftScore * downScore * upScore

        //We know trees on the edge can't have scenic score, so we'll only search the interior
        Array.init (forestHeight - 2) (fun i -> Array.init (forestWidth - 2) (fun j -> scenicScore (i+1) (j+1) ))

    //Output

    let visibleTreeCount = Array.fold (fun total line -> total + Array.sum line) 0 getHiddenTrees
    let maxScenicScore = Array.max (Array.map (fun line -> Array.max line) scenicScores)

    Console.WriteLine("Part 1: " + visibleTreeCount.ToString())
    Console.WriteLine("Part 2: " + maxScenicScore.ToString())
    8