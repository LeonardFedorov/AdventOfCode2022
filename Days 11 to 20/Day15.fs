[<AutoOpen>]
module Day15

open System
open System.IO

type dirs =
    | up = 0
    | right = 1
    | down = 2
    | left = 3

type pathCandidate = {head: (int*int); weight: int; heuristic: int; cameFrom: dirs}

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day15Input.txt")
    let splitText = fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    Array2D.init splitText.Length splitText.[0].Length (fun i j -> int splitText.[i].[j] - int '0' )

let getHeuristic (i,j) (sourceArray: int[,]) = (Array2D.length1 sourceArray)- i + (Array2D.length2 sourceArray) - j

let inBounds (i,j) (sourceArray: int[,]) = i >= 0 && i < (Array2D.length1 sourceArray) && j >= 0 && j < (Array2D.length2 sourceArray)

let buildPath (i,j) prevWeight dir (sourceArray: int[,]) =
    {head = (i,j); weight = prevWeight + sourceArray.[i, j]; heuristic = getHeuristic (i,j) sourceArray; cameFrom = dir}

let getNextPoints (currentPoint: pathCandidate) sourceArray =
    let (i,j) = currentPoint.head
    [|(int currentPoint.cameFrom + 3) % 4 ; int currentPoint.cameFrom ; (int currentPoint.cameFrom + 1) % 4  |]    
    |> Array.map (fun dir -> (dir, match enum<dirs>dir with
                                    | dirs.up -> (i, j+1)
                                    | dirs.right -> (i-1, j)
                                    | dirs.down -> (i,j-1)
                                    | dirs.left -> (i+1, j)
                                    | _ -> failwith "invalid point passed"))
              
    |> Array.fold (fun newPoints (dir, point) -> if inBounds point sourceArray then
                                                    (buildPath point currentPoint.weight (enum<dirs>dir) sourceArray) :: newPoints
                                                 else
                                                    newPoints
                  ) List.empty<pathCandidate>
    |> Array.ofList                                                    

//Todo: find minimum point in current list and return array containing the new points but not that existing point.
//      can probably write array initialiser to do this

//Part 1




//Part 2


//Entry point
let main projectDir =

    let sourceData = getText projectDir

    Console.WriteLine("Part 1: Not Implemented."  )
    Console.WriteLine("Part 2: Not Implemented."  )
    15