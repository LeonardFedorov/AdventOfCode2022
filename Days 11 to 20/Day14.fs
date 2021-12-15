[<AutoOpen>]
module Day14

open System
open System.IO

//Common

let rec performUpdate (polyPairCounts: int64[]) numbersToTargets (numbersToSources: int[][]) iter =
    if iter = 0 then
        polyPairCounts
    else
        let newPolyPairCounts = Array.mapi (fun i pairCount -> Array.fold (fun s x -> s + polyPairCounts.[x]) 0L numbersToSources.[i]) polyPairCounts

        performUpdate newPolyPairCounts numbersToTargets numbersToSources (iter - 1)

//Entry point
let main projectDir =

    //Read the text file and get the data
    let fileStream = new StreamReader(projectDir + "\\Day14Input.txt")
    let parsedText = fileStream.ReadToEnd().Split([|"\r\n\r\n"|], StringSplitOptions.None)

    let mappingText = parsedText.[1].Split("\r\n", StringSplitOptions.None)
    let numbersToLetters =  Array.map (fun (mapString: string) -> mapString.[0..1]) mappingText
    let numbersToLettersIndexed = Array.indexed numbersToLetters

    let numbersToTargets = Array.map (fun (mapString: string) -> [|Array.findIndex (fun (x: string) -> x.[0] = mapString.[0] && x.[1] = mapString.[6]) numbersToLetters ; 
                                                                   Array.findIndex (fun (x: string) -> x.[0] = mapString.[6] && x.[1] = mapString.[1]) numbersToLetters |]
                                     ) mappingText
    let numbersToTargetsIndexed = Array.indexed numbersToTargets
    let numbersToSources = Array.init numbersToTargets.Length (fun i -> Array.filter (fun (index: int, element: int[]) -> None <> Array.tryFind (fun item -> item = i) element) numbersToTargetsIndexed 
                                                                        |> Array.map (fun x -> fst x) 
                                                              )

    let initialPoly = Array.pairwise (parsedText.[0].ToCharArray())
                      |> Array.map (fun (a,b) -> Array.findIndex (fun (x: string) -> a = x.[0] && b = x.[1]) numbersToLetters)

    //Implicitly assuming that every letter can be obtained from some mapping
    let listOfLetters = Array.distinct (Array.map (fun (x: string) -> x.[6]) mappingText)
    let listOfLetterSources = Array.map (fun x -> Array.filter (fun (index, y: string) -> y.[0] = x ) numbersToLettersIndexed
                                                  |> Array.map (fun z -> fst z)
                                        ) listOfLetters
    let indexOfLast = Array.findIndex (fun x -> x = parsedText.[0].[parsedText.[0].Length - 1]) listOfLetters

    //Perform the iterations - part 1 is 10 cycles and part 2 = 10 +30 = 40 cycles
    let initialPairCounts = Array.init numbersToSources.Length (fun i -> int64 (Array.filter (fun x -> x = i) initialPoly).Length)

    let part1Counts = performUpdate initialPairCounts numbersToTargets numbersToSources 10
    let part2Counts = performUpdate part1Counts numbersToTargets numbersToSources 30

    let part1LetterCounts = Array.mapi (fun i (sources: int[]) -> Array.fold (fun s source -> s + part1Counts.[source]) 0L sources + (if i = indexOfLast then 1L else 0L)) listOfLetterSources
    let part2LetterCounts = Array.mapi (fun i (sources: int[]) -> Array.fold (fun s source -> s + part2Counts.[source]) 0L sources + (if i = indexOfLast then 1L else 0L)) listOfLetterSources

    Console.WriteLine("Part 1: " + (Array.max part1LetterCounts - Array.min part1LetterCounts).ToString())
    Console.WriteLine("Part 2: " + (Array.max part2LetterCounts - Array.min part2LetterCounts).ToString())
    -1