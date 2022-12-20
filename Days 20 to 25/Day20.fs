[<AutoOpen>]
module Day20

open System
open System.IO

//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day20Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
        |> Array.map (fun line -> Int64.Parse(line))

    let arrayLength = sourceData.Length    
    
    //F#'s native mod returns a residue with the same sign as the dividend, however for this we want to use the minimal non-negative CSR
    let realMod a b =
        let rawMod = a % b
        rawMod + if rawMod < 0L then b else 0L

    //Move the item at index in the workingArray according to its value
    let rec moveItem index (workingArray: (int64 * int)[]) =
        let currentItem = workingArray.[index]
        let offset = int (realMod (fst currentItem) (int64 (arrayLength - 1)))
        if offset <> 0 then
            let newPlace = int (realMod (int64 (index + offset)) (int64 (arrayLength - 1)))
            
            //Shuffle along all the items in between where we are moving the item from and to
            if newPlace <> index then
                let loopDirection = Math.Sign(newPlace - index)
                for i in index .. loopDirection .. newPlace do
                    if i = newPlace then
                        workingArray.[i] <- currentItem
                    else
                        workingArray.[i] <- workingArray.[i + loopDirection]

    //Perform a full run over all the items in the array, moving them according to their original order
    let runCycle workingArray =
        for i in 0 .. arrayLength - 1 do
            let targetIndex = Array.findIndex (fun (value, rank) -> rank = i) workingArray
            moveItem targetIndex workingArray

    //Extract the answer from an array whose computations are finished
    let getAnswer workingArray =
        let zeroPos = Array.findIndex (fun (value, rank) -> value = 0L) workingArray
        Array.map (fun value -> fst workingArray.[(zeroPos + value) % arrayLength]) [|1000;2000;3000|]
        |> Array.sum

    //Part 1

    let mutable workingArray1 = Array.init arrayLength (fun i -> (sourceData.[i], i))

    runCycle workingArray1
    let zeroPos1 = Array.findIndex (fun (value, rank) -> value = 0L) workingArray1
    let part1Answer = getAnswer workingArray1

    //Part 2

    let decryptionKey = 811589153L
    let cycleCount = 10

    let mutable workingArray2 = Array.init arrayLength (fun i -> (decryptionKey * sourceData.[i], i))

    for j in 1 .. cycleCount do
        runCycle workingArray2

    let part2Answer = getAnswer workingArray2

    //Output

    Console.WriteLine("Part 1: " + part1Answer.ToString())
    Console.WriteLine("Part 2: " + part2Answer.ToString())
    20