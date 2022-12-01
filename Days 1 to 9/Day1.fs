[<AutoOpen>]
module Day1

open System
open System.IO

//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day1Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n\r\n"|], StringSplitOptions.None) //Split by double new line first to separate each elf
        |> Array.map (fun elf -> elf.Split([|"\r\n"|], StringSplitOptions.None)) //Split each line
        |> Array.map (fun elf -> Array.map(fun value -> Int64.Parse(value)) elf) //Convert each item to an actual number

    let elfTotals = 
        Array.map (fun elf -> Array.sum elf) sourceData //Calculate each elf's total
        |> Array.sortDescending //sort into descending order, as this will be useful later

    //Since the array is sorted, the part 2 answer is just the sum of the first 3 elements
    let part2Result = Array.sum elfTotals.[0..2]

    //Output
    Console.WriteLine("Part 1: " + elfTotals.[0].ToString())
    Console.WriteLine("Part 2: " + part2Result.ToString())
    1