[<AutoOpen>]
module Day17

open System
open System.IO

type target = {minX: int; maxX: int; minY: int; maxY: int}

//Functions to work with triangle numbers to help velocity to distance calculations calculations
let triangle x = x * (x + 1) / 2

let reverseTriangle x =
    let xFloat = float x
    let result = (-1.0 + sqrt(1.0 + 8.0 * xFloat))/2.0
    int (Math.Floor result) - 1

//Parse input data
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

//Trace the path of each velocity candidate
let rec traceDownwardPathIter (xV, yV) (xPos, yPos) targetArea =
    
    //Calculate new position
    let newX = xPos + xV
    let newY = yPos + yV

    if newX > targetArea.maxX then
        false
    else
        if newY < targetArea.minY then
            false
        elif newX >= targetArea.minX && newY <= targetArea.maxY then
            true
        else
            traceDownwardPathIter (max (xV - 1) 0, yV - 1) (newX, newY) targetArea

let traceDownwardPath initialVelocity targetArea =
    traceDownwardPathIter initialVelocity (0,0) targetArea    

//Entry point
let main projectDir =

    let targetArea = getText projectDir

    //Define the velocity bounds we need to test between
    //Assumption: minX > 0, minY < 0

    let minXVelocity = reverseTriangle targetArea.minX //Any lower x will drop straight down before reaching the target area
    let maxXVelocity = targetArea.maxX //Any higher x will immediately overshoot the target area
    let minYVelocity = targetArea.minY //Any "lower" (i.e. higher negative velocity) will instantly overshoot the target area
    let maxYVelocity = -targetArea.minY - 1 //Any higher initial upwards velocity will directly overshoot the target area as it returns past the 0 line
    
    //Build arrays of candidates to test
    let results = Array.init (maxXVelocity - minXVelocity + 1) (fun x -> Array.init (maxYVelocity - minYVelocity + 1) (fun y -> (x + minXVelocity, y + minYVelocity) ))
                  |> Array.map (fun row -> Array.map (fun cell -> traceDownwardPath cell targetArea) row)

    //Find highest y velocity to solve part 1
    let maxYIndex = Array.map (fun row -> Array.tryFindIndexBack (fun result -> result) row) results
                    |> Array.max //None compares less than any valid option int, so finding max will filter these out
    let maxYVelocity = maxYIndex.Value + minYVelocity

    //Count how many trues are in the array to get part 2
    let hitCount = Array.map (fun row -> Array.fold (fun s result -> if result then s + 1 else s) 0 row) results
                   |> Array.sum

    Console.WriteLine("Part 1: " + (triangle maxYVelocity).ToString())
    Console.WriteLine("Part 2: " + hitCount.ToString())
    17