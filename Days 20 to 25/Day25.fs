[<AutoOpen>]
module Day25

open System
open System.IO

//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day25Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    
    let digitTranslate digit =
        match digit with
            | '2' -> 2L
            | '1' -> 1L
            | '0' -> 0L
            | '-' -> -1L
            | '=' -> -2L
            | _ -> failwith "Unknown digit"

    let numberTranslate value =
        match value with
            | 2L -> '2'
            | 1L -> '1'
            | 0L -> '0'
            | -1L -> '-'
            | -2L -> '='
            | _ -> failwith "Unknown value"

    //Part 1
    let cursedMod n p = 
        let rawMod = n % p
        if rawMod > p/2L then rawMod - p
        else rawMod

    let fivesToDec (n: string) =
        Array.foldBack (fun digit (exp, total) -> (exp + 1, total + (pown 5L exp) * digitTranslate digit)) (n.ToCharArray()) (0,0L)
        |> snd

    let decToFives n =
        let rec translateIter word remainder =
            if remainder = 0L then
                word
            else
                let slice = cursedMod remainder 5L
                translateIter ((numberTranslate slice) :: word) ((remainder - slice) / 5L)

        translateIter List.empty<char> n
        |> Array.ofList
        
    let part1Answer = 
        Array.map (fun line -> fivesToDec line) sourceData
        |> Array.sum
        |> decToFives


    //Part 2



    //Output

    Console.WriteLine("Part 1: " + String(part1Answer)  )
    Console.WriteLine("Part 2: Not Implemented."  )
    25