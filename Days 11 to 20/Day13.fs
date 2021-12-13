[<AutoOpen>]
module Day13

open System
open System.IO

type foldDir =
| x = 0
| y = 1

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day13Input.txt")
    let inputStrings = fileStream.ReadToEnd().Split([|"\r\n\r\n"|], StringSplitOptions.None)

    let points = inputStrings.[0].Split([|"\r\n"|], StringSplitOptions.RemoveEmptyEntries)
                 |> Array.map (fun x -> let coords = x.Split(',')
                                        (Int32.Parse(coords.[0]) , Int32.Parse(coords.[1]))  )
                 |> List.ofArray
 
    let folds = inputStrings.[1].Split([|"\r\n"|], StringSplitOptions.RemoveEmptyEntries)
                |> Array.map (fun x -> ( (if x.[11] = 'x' then foldDir.x else foldDir.y) , Int32.Parse(x.[13..])))
                |> List.ofArray

    (points, folds)

let foldMap (x,y) (foldType, foldLine) =

    if foldType = foldDir.x then
        if x < foldLine then
            (x,y)
        else
            (foldLine - (x - foldLine) , y)
    else
        if y < foldLine then
            (x,y)
        else
            (x, foldLine - (y - foldLine) )

let foldPoint point newPoints currFold =

    let foldedPoint = foldMap point currFold

    //if the point is already in the list, then don't add it
    if List.tryFind (fun x -> x = foldedPoint) newPoints = None
        then foldedPoint :: newPoints
    else
        newPoints

let rec foldPaper points (foldList: (foldDir*int) list) count =
    if foldList.IsEmpty then
        points
    else
        let nextFold = foldList.Head
        let newPoints = List.fold (fun acc x -> foldPoint x acc nextFold) List.Empty points

        Console.WriteLine("There are " + newPoints.Length.ToString() + " points on the sheet after " + count.ToString() + " folds.")
        foldPaper newPoints foldList.Tail (count + 1)

let PrintArray (sourceArray: char[][]) =
    
    Array.map (fun (row: char[]) -> String(row) ) sourceArray
    |> Array.iter (fun row -> Console.WriteLine(row))   
    |> ignore
    
//Entry point
let main projectDir =

    let (sourcePoints, foldList) = getText projectDir

    Console.WriteLine("There are initially " + sourcePoints.Length.ToString() + " points on the sheet.")

    let finalPoints = foldPaper sourcePoints foldList 1

    let (maxX , maxY) = List.fold (fun (aX,aY) (x,y) -> (max aX (x+1), max aY (y+1))) (0,0) finalPoints

    Console.Write("\nFinal Result: \n\n")
    let output = Array.init maxY (fun j -> (Array.init maxX (fun i -> if (None <> List.tryFind (fun x -> x = (i,j)) finalPoints) then '#' else '-' ) ) )
                 |> PrintArray    
    
    13