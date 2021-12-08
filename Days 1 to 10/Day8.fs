[<AutoOpen>]
module Day8

open System
open System.IO

//Each row is a string array. Entries 0-9 inclusive contain the screen output, 10 contains the delimiter
//and 11-14 inclusive contain the output values
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day8Input.txt")
    fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    |> Array.map (fun x -> x.Split([|" "|], StringSplitOptions.RemoveEmptyEntries))

//Part 1

let uniqueLength length =
    match length with
        | 2 | 3 | 4 | 7 -> 1
        | _ -> 0
 
let count1478s (sequence: string[]) =
    let delim = Array.findIndex (fun x -> x = "|") sequence
    let answerNumbers = Array.init (sequence.Length - delim - 1) (fun i -> sequence.[i + delim + 1])
    Array.fold (fun count (x:string) -> count + (uniqueLength x.Length)) 0 answerNumbers    


//Part 2

//Function to map 0 based array indicies to letters starting from a
let indexToChar index = char (index + int 'a')

let mapChar char (map: (char*char) list) =
    List.fold (fun s x -> if char = fst x then snd x else s) '0' map

let unMapChar char (map: (char*char) list) =
    List.fold (fun s x -> if char = snd x then fst x else s) '0' map

let convertString numberString =
    match numberString with
        | "abcefg" -> 0
        | "cf" -> 1
        | "acdeg" -> 2
        | "acdfg" -> 3
        | "bcdf" -> 4
        | "abdfg" -> 5
        | "abdefg" -> 6
        | "acf" -> 7
        | "abcdefg" -> 8
        | "abcdfg" -> 9
        | _ -> failwith "Unknown string passed to conversion."

let translateString (numberString:string) charMap =
    let mappedString = Array.map (fun x -> mapChar x charMap) (numberString.ToCharArray())
    new String(Array.sort mappedString)
    |> convertString

let getCharMapping (clueNumbers: string[]) =
    
    List.empty<(char * char)>

let part2Evaluator (sourceLine: string[]) =
    //Slice up the source data into more useful pieces
    let delim = Array.findIndex (fun x -> x = "|") sourceLine
    let clueNumbers = Array.init delim (fun i -> sourceLine.[i])
    let answerNumbers = Array.init (sourceLine.Length - delim - 1) (fun i -> sourceLine.[i + delim + 1])

    //Count how many times each char appears in the data as this is key to the deduction process
    let charCounts = Array.init 7 (fun i -> Array.fold (fun count string -> if String.exists (fun c -> c = (indexToChar i)) string then count + 1 else count) 0 clueNumbers)
    let charMap = getCharMapping clueNumbers

    //Use the char map to translate the digits, and then convert to a single base 10 number
    Array.map (fun x -> translateString x charMap) answerNumbers
    |> Array.fold (fun s x-> 10*s + x) 0


//Entry point
let main projectDir =

    let sourceData = getText projectDir
    let part1 = Array.sum (Array.map (fun x -> count1478s x) sourceData)
    let part2 = Array.sum (Array.map (fun x -> part2Evaluator x) sourceData)

    Console.WriteLine("Part 1: " + part1.ToString() )
    Console.WriteLine("Part 2: Not Implemented."  )
    8

//Deduction steps (v1):
//First, identify the 1,4,7 and 8 based on their unique lengths
//Then, the character that appears in the 7 but not the 1 must be the true a                                            :a
//Then, look for the character that appears in all bar 1, this the true f, and the string it appears in is the 2        :f
//Comparing this character with the 1 allows the true c to be deduced                                                   :c
//The character in the 4 which is not the true f, and is not in common with the 2 is the true b                         :b
//The true e is the only character present in only 3 numbers                                                            :e

//Deduction steps (v2):
//Count how many times each char appears. 4, 6 and 9 occurences are the true e, b, f                                    :b,e,f
//Character in 7 but not 1 is the a                                                                                     :a
//The character that is not the a but occurs 8 times in the c                                                           :c
//The character that is missing from a string that is missing one and is not the c or e is the d                        :d
//The remaining character is the g                                                                                      :g