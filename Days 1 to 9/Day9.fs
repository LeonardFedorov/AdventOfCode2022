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

        let rec processInstruction (hx, hy) (tx, ty) pathSoFar direction count =
            
            if count = 0 then
                ((hx, hy), (tx, ty), pathSoFar)
            else

                let (hx2, hy2) = match direction with
                                    | 'U' -> (hx, hy + 1)
                                    | 'D' -> (hx, hy - 1)
                                    | 'L' -> (hx - 1, hy)
                                    | 'R' -> (hx + 1, hy)
                                    | _ -> failwith "Unexpected direction"

                let moveTail = (customAbs(hx2 - tx) > ropeLength) || (customAbs(hy2 - ty) > ropeLength)

                let (txd, tyd) = if not moveTail then (0,0)
                                 else 
                                     (customSign(hx2 - tx), customSign(hy2 - ty))

                let newTail = (tx + txd , ty + tyd)                            
                let newPath = if moveTail then
                                  newTail :: pathSoFar
                              else pathSoFar

                processInstruction (hx2, hy2) newTail newPath direction (count - 1)
        
        let rec processPathIter instructions pathSoFar headPos tailPos =
            
            if List.isEmpty instructions then
                pathSoFar
            else
                let (direction, count) = List.head instructions
                let (newHead, newTail, newPath) = processInstruction headPos tailPos pathSoFar direction count
                processPathIter (List.tail instructions) newPath newHead newTail

        processPathIter sourceData [(0,0)] (0,0) (0,0)

    //Output
    let p1Result = findTailPath 2
                   |> List.distinct
                   |> List.length

    let p2Result = findTailPath 9
                   |> List.distinct
                   |> List.length

    Console.WriteLine("Part 1: " + p1Result.ToString())
    Console.WriteLine("Part 2: " + p2Result.ToString())
    9