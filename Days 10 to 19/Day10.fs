[<AutoOpen>]
module Day10

open System
open System.IO

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day10Input.txt")
    fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)

let isOpenChar char =
    match char with
        | '(' | '[' | '{' | '<' -> true
        | _ -> false

let getExpectedClose opener =
    match opener with
        | '(' -> ')'
        | '[' -> ']'
        | '{' -> '}'
        | '<' -> '>'
        |  _  -> '0' //Default case should never arise

//Part 1

let p1scoreValue closer =
    match closer with
        | ')' -> 3
        | ']' -> 57
        | '}' -> 1197
        | '>' -> 25137
        |  _  -> 0 //Default case should never arise

let errorScore (chunkString: string) =
    Array.fold (fun (openChunks: char list, errorScore: int) currentChar 
                    -> if errorScore > 0 then (openChunks, errorScore) 
                       elif isOpenChar currentChar then (currentChar :: openChunks, errorScore)   
                       elif currentChar = getExpectedClose openChunks.Head then (openChunks.Tail, errorScore)
                       else (openChunks, p1scoreValue currentChar)
                ) (List.Empty, 0) (chunkString.ToCharArray())        

//Part 2

let p2scoreValue closer =
    match closer with
        | ')' -> 1L
        | ']' -> 2L
        | '}' -> 3L
        | '>' -> 4L
        |  _  -> 0L //Default case should never arise

let getOpenChunks (chunkString: string) =
    Array.fold (fun (openChunks: char list) currentChar -> if isOpenChar currentChar then (currentChar :: openChunks)   
                                                           elif currentChar = getExpectedClose openChunks.Head then openChunks.Tail
                                                           else failwith "This shouldn't happen?"
                                                           ) List.Empty (chunkString.ToCharArray())

let closeScore (chunkString: string) =
    getOpenChunks chunkString
    |> List.fold (fun s opener -> s*5L + p2scoreValue(getExpectedClose opener)) 0L

//Entry point
let main projectDir =
    let sourceData = getText projectDir

    let errorScores = Array.map (fun x -> snd (errorScore x)) sourceData
    let incompleteStrings = Array.filter (fun x -> snd (errorScore x) = 0) sourceData
    let part2Array = Array.map (fun x -> closeScore x) incompleteStrings
                     |> Array.sort

    Console.WriteLine("Part 1: " + (Array.sum errorScores).ToString())
    Console.WriteLine("Part 2: " + part2Array.[(part2Array.Length - 1)/2].ToString() )
    10