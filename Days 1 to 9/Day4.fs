[<AutoOpen>]
module Day4

open System
open System.IO

//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day4Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
        |> Array.map (fun elfPair -> elfPair.Split([|',';'-'|], StringSplitOptions.None))
        |> Array.map (fun valueList -> Array.map (fun value -> Int64.Parse(value)) valueList)

    //Take the list of values and return a pair saying whether they (contain, overlap)
    let IsContainOrOverlap ([|p1Start; p1End; p2Start; p2End|]: int64[]) =

        (
        ((p1Start <= p2Start) && (p1End >= p2End)) || ((p2Start <= p1Start) && (p2End >= p1End)) //Contain
        ,
        ((p1End >= p2Start) && (p1Start <= p2End)) || ((p2End >= p1Start) && (p2Start <= p1End)) //Overlap
        )

    //Apply the above calculation to the set of ranges, then count how many trues are present for each branch
    let results =
        Array.map (fun valueList -> IsContainOrOverlap valueList) sourceData
        |> Array.fold (fun (count1, count2) (value1, value2) -> let newCount1 = count1 + if value1 then 1 else 0
                                                                let newCount2 = count2 + if value2 then 1 else 0
                                                                (newCount1, newCount2)     
                       ) (0,0)

    //Output

    Console.WriteLine("Part 1: " + (fst results).ToString())
    Console.WriteLine("Part 2: " + (snd results).ToString())
    4