[<AutoOpen>]
module Day3

open System
open System.IO

//Entry point
let main projectDir =

    //Common functions

    //Test if the item c is present in every array.
    //Search for arrays where the item cannot be found. If no array exists where the item cannot be found,
    //then it exists in all of them.
    let findIteminAllArrays c arrays =
        None = Array.tryFind (fun array -> None = Array.tryFind (fun x -> x = c) array) arrays    
        
    let findCommonChar (strings: char[][]) =
        let otherStrings = strings.[1..]
        Array.find (fun c -> findIteminAllArrays c otherStrings) strings.[0]

    let priorityScore (c: char) =
        match int c with
            | c when c >= 65 && c <= 90 -> c - (65-27)
            | c when c >= 97 && c <= 122 -> c - (97-1)
            | _ -> failwith "Unknown char found"      

    //File Load
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day3Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
        |> Array.map (fun word -> word.ToCharArray())

    //Part 1
    let part1 = 
        let splitInHalf (backpack: char[]) =
            let size = backpack.Length
            if size % 2 <> 0 then failwith "Odd backpack found"
            else [|backpack.[0..size/2-1]; backpack.[size/2..]|]

        Array.map (fun backpack -> splitInHalf backpack) sourceData
        |> Array.map (fun pair -> priorityScore (findCommonChar pair))
        |> Array.sum

    //Part 2
    let part2 =
        let groupSize = 3
        if sourceData.Length % groupSize <> 0 then failwith "Invalid party size"
        Array.init (sourceData.Length/groupSize) (fun i -> sourceData.[groupSize * i.. groupSize * i + groupSize - 1])
        |> Array.map (fun group -> priorityScore (findCommonChar group))
        |> Array.sum

    //Output
    
    Console.WriteLine("Part 1: " + part1.ToString())
    Console.WriteLine("Part 2: " + part2.ToString())
    3