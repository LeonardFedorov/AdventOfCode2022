[<AutoOpen>]
module Day5

open System
open System.IO

//Entry point
let main projectDir =

    //Split the text stream by the double line break to separate the move list from the config
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day5Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n\r\n"|], StringSplitOptions.None)
        |> Array.map (fun section -> section.Split([|"\r\n"|], StringSplitOptions.None))

    //Get the list of moves
    let moveList =
        Array.map (fun (line: string) -> line.Split([|"move ";" from ";" to "|], StringSplitOptions.RemoveEmptyEntries)) sourceData.[1]
        |> Array.map (fun valueList -> Array.map (fun value -> Int32.Parse(value)) valueList) 
        
    //Build the initial box state
    let initialBoxList (source: string []) = 
        let stackCount = 9
        let mutable boxBuild = Array.init stackCount (fun i -> List.empty<char>)
        //Set up the initial state of the boxList
        for i in source.Length - 2 .. -1 .. 0 do
            for j in 1 .. stackCount do
                match source.[i].[4*j - 3] with
                    | ' ' -> ()
                    | c -> boxBuild.[j-1] <- c :: boxBuild.[j-1]
        boxBuild

    let rec moveBoxes (boxList: char list []) count blockSize source destination =
        if count = 0 then ()
        else
            boxList.[destination] <- List.take blockSize boxList.[source] @ boxList.[destination]
            boxList.[source] <- List.skip blockSize boxList.[source]
            moveBoxes boxList (count - blockSize) blockSize source destination

    let readTopBoxes (boxList: char list []) =
        Array.init (boxList.Length) (fun i -> boxList.[i].Head)
        |> String

    let part1 =
        let mutable boxState = initialBoxList sourceData.[0]
        Array.iter (fun [|count; source; destination|] -> moveBoxes boxState count 1 (source-1) (destination-1)) moveList
        readTopBoxes boxState

    let part2 =
        let mutable boxState = initialBoxList sourceData.[0]
        Array.iter (fun [|count; source; destination|] -> moveBoxes boxState count count (source-1) (destination-1)) moveList
        readTopBoxes boxState

    Console.WriteLine("Part 1: " + part1)
    Console.WriteLine("Part 2: " + part2)
    5