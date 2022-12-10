[<AutoOpen>]
module Day9

open System
open System.IO

//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day9Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
        |> Array.map (fun line -> (line.[0] , Int32.Parse(line.[2..])))
        |> List.ofArray

    //Part 1

    let findTailPath (ropeLength: int) =
        
        let customAbs (value: int) = Math.Abs(value)
        let customSign (value: int) = Math.Sign(value)

        let rec processInstruction ropePos pathSoFar direction count =
            
            if count = 0 then
                (ropePos, pathSoFar)
            else            

            let updatedHead = 
                let (hx, hy) = List.head ropePos
                match direction with
                    | 'U' -> (hx, hy + 1)
                    | 'D' -> (hx, hy - 1)
                    | 'L' -> (hx - 1, hy)
                    | 'R' -> (hx + 1, hy)
                    | _ -> failwith "Unexpected direction"

            let updatePoint (hx, hy) (tx, ty) =
                let moveTail = (customAbs(hx - tx) > 1) || (customAbs(hy - ty) > 1)
                let (txd, tyd) = if moveTail then (customSign(hx - tx), customSign(hy - ty)) else (0,0)
                (tx + txd , ty + tyd)

            let mutable newRope = [updatedHead] 

            List.iter (fun point -> newRope <- (updatePoint (List.head newRope) point) :: newRope) (List.tail ropePos)

            let moveTail = List.head newRope <> List.head pathSoFar

            let newPath = if moveTail then
                                List.head newRope :: pathSoFar
                            else pathSoFar

            processInstruction (List.rev newRope) newPath direction (count - 1)
        
        let rec processPathIter instructions pathSoFar ropePos =
            
            if List.isEmpty instructions then
                pathSoFar
            else
                let (direction, count) = List.head instructions
                let (newRope, newPath) = processInstruction ropePos pathSoFar direction count
                processPathIter (List.tail instructions) newPath newRope

        let initialRope = List.init ropeLength (fun i -> (0,0))
        processPathIter sourceData [(0,0)] initialRope

    //Output
    let p1Result = findTailPath 2
                   |> List.distinct
                   |> List.length

    let p2Result = findTailPath 10
                   |> List.distinct
                   |> List.length

    Console.WriteLine("Part 1: " + p1Result.ToString())
    Console.WriteLine("Part 2: " + p2Result.ToString())
    9