[<AutoOpen>]
module Day17

open System
open System.IO

type target = {minX: int; maxX: int; minY: int; maxY: int}

//Common
let getMinMax (inputString: string) =
    let numbers = inputString.Split([|".."|], StringSplitOptions.None)
    (Int32.Parse(numbers.[0]), Int32.Parse(numbers.[1]))

let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day17Input.txt")
    let textString = fileStream.ReadToEnd()

    //Get positions of key items
    let fstEQ = Array.findIndex (fun x -> x = '=') (textString.ToCharArray())
    let sndEQ = Array.findIndexBack (fun x -> x = '=') (textString.ToCharArray())
    let comma = Array.findIndex (fun x -> x = ',') (textString.ToCharArray())

    let (xMin, xMax) = getMinMax textString.[(fstEQ+1)..(comma-1)]
    let (yMin, yMax) = getMinMax textString.[(sndEQ+1)..]

    {minX = xMin; maxX = xMax; minY = yMin; maxY = yMax}

//Part 1
let rec traceDownwardPathIter velocity depth targetArea =
    let newDepth = depth + velocity
    if newDepth < targetArea.minY then
        false
    elif newDepth <= targetArea.maxY then
        true
    else
        traceDownwardPathIter (velocity - 1) newDepth targetArea


let traceDownwardPath initialVelocity targetArea =
    traceDownwardPathIter -initialVelocity 0 targetArea    

let findMaxYHeight targetArea =
    let highestVelocityHit = Array.init (-targetArea.minY + 1) (fun i -> traceDownwardPath (i+1) targetArea)
                             |> Array.findIndexBack (fun x -> x)
    
    (highestVelocityHit * (highestVelocityHit + 1))/2

//Part 2


//Entry point
let main projectDir =

    let targetArea = getText projectDir

    let maxYHeight = findMaxYHeight targetArea

    Console.WriteLine("Part 1: " + maxYHeight.ToString())
    Console.WriteLine("Part 2: Not Implemented."  )
    17