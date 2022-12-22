[<AutoOpen>]
module Day22

open System
open System.IO

type Instruction =
    | Turn of char
    | Move of int

//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day22Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n\r\n"|], StringSplitOptions.None)

    let maze = 
        let rawData = sourceData.[0].Split([|"\r\n"|], StringSplitOptions.None)
        let maxRow = Array.map (fun row -> String.length row) rawData
                     |> Array.max
        
        Array2D.init rawData.Length maxRow (fun i j -> if j < rawData.[i].Length then rawData.[i].[j] else ' ')

    let instructions =
        
        let rec buildList sourceString list =
            
            //If the string is empty, then nothing left to parse so return the list
            if Array.length sourceString = 0 then
                list
            else

                let maybeTurnIndex = Array.tryFindIndexBack (fun char -> char = 'L' || char = 'R') sourceString

                //If there's no turning instruction left, then the entire string consists of a single numeric instruction
                if maybeTurnIndex.IsNone then
                    Instruction.Move(Int32.Parse(String sourceString)) :: list
                else
                    let turnIndex = maybeTurnIndex.Value
                    let updatedList = Instruction.Turn(sourceString.[turnIndex]) :: if turnIndex = sourceString.Length - 1 then
                                                                                       list
                                                                                    else
                                                                                       Instruction.Move(Int32.Parse(String sourceString.[turnIndex + 1..])) :: list
                    
                    buildList sourceString.[..turnIndex - 1] updatedList
        
        let instructionString = sourceData.[1].ToCharArray()
        buildList instructionString List.empty<Instruction>

    let facings = [|(0,1);(1,0);(0,-1);(-1,0)|] //Clockwise starting from facing right. These indicies match the output values in the puzzle description

    //Part 1

    let tracePath =

        let rec performInstruction currentPos currentFacing instructionList =

            let rec move position facing count =

                //Offset the point, keeping it in bounds of the maze using the wrapping behavior
                let offset (x,y) (a,b) = 
                    (realMod (x + a) (Array2D.length1 maze), realMod (y + b) (Array2D.length2 maze))

                let rec traverseVoid position facing =
                    let (x,y) = offset position facings.[facing]
                    match maze.[x,y] with
                        | ' ' -> traverseVoid (x,y) facing
                        | '.' -> ('.',(x,y))
                        | '#' -> ('#',(x,y))
                        | _ -> failwith "Unkown maze char"

                if count = 0 then
                    position
                else
                    let (x,y) = offset position facings.[facing]
                    match maze.[x,y] with
                        | '#' -> position //halt if a wall is reached
                        | ' ' -> match traverseVoid (x,y) facing with
                                    | ('#', _) -> position //don't move if a wall on the far side of the void
                                    | ('.', newPos) -> move newPos facing (count - 1) //move to the far side and proceed
                                    | _ -> failwith "Unexpected void walk result"
                        | '.' -> move (x,y) facing (count - 1)
                        | _ -> failwith "Unkown maze char"
                       
            let turn facing turnchar =
                match turnchar with
                    | 'L' -> realMod (facing - 1) facings.Length
                    | 'R' -> realMod (facing + 1) facings.Length
                    | _ -> failwith "Unexpected turn char"
            
            //If no more instructions, then finish
            if List.isEmpty instructionList then
                (currentPos, currentFacing)
            else //otherwise, execute the next instruction
                let newFacing = match List.head instructionList with
                                    | Move(x) -> currentFacing
                                    | Turn(x) -> turn currentFacing x

                let newPos = match List.head instructionList with
                                | Move(x) -> move currentPos currentFacing x
                                | Turn(x) -> currentPos

                performInstruction newPos newFacing (List.tail instructionList)


        let startPosition = (0, Array.findIndex (fun char -> char = '.') maze.[0,*])
        performInstruction startPosition 0 instructions

    let part1Answer = 
        let (x,y) = fst tracePath
        (x + 1) * 1000 + 4 * (y + 1) + (snd tracePath)

    //Part 2






    //Output

    Console.WriteLine("Part 1: " + part1Answer.ToString())
    Console.WriteLine("Part 2: Not Implemented."  )
    22