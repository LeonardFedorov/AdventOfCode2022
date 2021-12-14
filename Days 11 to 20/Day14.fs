[<AutoOpen>]
module Day14

open System
open System.IO

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day14Input.txt")
    let parsedText = fileStream.ReadToEnd().Split([|"\r\n\r\n"|], StringSplitOptions.None)
    
    let mappingList = Array.map (fun (instruction: string) -> (instruction.[0], instruction.[1], instruction.[6]) ) (parsedText.[1].Split([|"\r\n"|], StringSplitOptions.None))

    (parsedText.[0].ToCharArray(), mappingList)

//Part 1
let expandUpdate prevChar currChar mappingList polyList =
    let charMap = Array.tryFind (fun (a,b,c) -> a = prevChar && b = currChar) mappingList
    if charMap = None then
        currChar :: polyList
    else
        //Extract the third element of the tuple as this is our value to map to
        let (a,b,c) = charMap.Value
        currChar :: c :: polyList

let rec performUpdate polymerString mappingList iter =
    if iter = 0 then
        polymerString
    else
        
        let newPolymerString = fst (List.fold (fun ((poly: char list), prevChar) currChar -> if poly.IsEmpty then ([currChar], currChar) 
                                                                                             else (expandUpdate prevChar currChar mappingList poly, currChar)
                                                                                             ) (List.empty, '0')  polymerString)

        performUpdate (List.rev newPolymerString) mappingList (iter - 1)

//Part 2


//Entry point
let main projectDir =

    let (initialPolymer, mappingList) = getText projectDir

    let output = performUpdate (List.ofArray initialPolymer) mappingList 10

    let charCounts = List.countBy id output
    let maxChar = List.fold (fun (accChar, accCount) (char, count) -> if count > accCount then (char, count) else (accChar, accCount)) ('0',0) charCounts
    let minChar = List.fold (fun (accChar, accCount) (char, count) -> if count < accCount then (char, count) else (accChar, accCount)) ('0',output.Length) charCounts

    Console.WriteLine("Part 1: frequency delta = " + (snd maxChar - snd minChar).ToString())
    Console.WriteLine("Part 2: Not Implemented."  )
    -1