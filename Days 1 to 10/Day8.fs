[<AutoOpen>]
module Day8

open System
open System.IO

let numChars = 7

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

//Short hand so equality tests don't need to be written in lambda
let eq x y = x = y
let eqStrLength x (y:string) = x = y.Length

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

//Find the missing character in the 6 char words
let findMissingChar (word:string) =
    Array.init numChars (fun i -> word.Contains (indexToChar i))
    |> Array.findIndex (eq false)
    |> indexToChar

let findA (clueNumbers: string[]) charMap =
    let one = Array.find (eqStrLength 2) clueNumbers
    let seven = Array.find (eqStrLength 3) clueNumbers

    let aChar = Array.find (fun x -> not (String.exists (eq x) one )) (seven.ToCharArray())
    (aChar , 'a') :: charMap

let getUniqueCountChars (charCounts: int[]) charMap =  
    let eMap = ((indexToChar ( Array.findIndex (eq 4) charCounts)) , 'e')
    let bMap = ((indexToChar ( Array.findIndex (eq 6) charCounts)) , 'b')
    let fMap = ((indexToChar ( Array.findIndex (eq 9) charCounts)) , 'f')
    fMap :: bMap :: eMap :: charMap

let findC (charCounts: int[]) charMap =
    let cIndex = Array.mapi (fun i x -> (x = 8) && (not (indexToChar i = (unMapChar 'a' charMap) ))) charCounts
                |> Array.findIndex (eq true)
    ( (indexToChar cIndex ), 'c') :: charMap

let findD (clueNumbers: string[]) charMap =
    
    let missingChars = Array.map (fun x -> findMissingChar x) (Array.filter (eqStrLength (numChars - 1)) clueNumbers)
    let dImage = Array.map (fun x -> mapChar x charMap) missingChars //'0' will tell us which entry is not mapped yet
                 |> Array.findIndex (eq '0')

    ((missingChars.[dImage]), 'd') :: charMap

let findG charMap =
    let gChar = Array.init numChars (fun i -> mapChar (indexToChar i) charMap)
                |> Array.findIndex (eq '0')
                |> indexToChar
    (gChar, 'g') :: charMap

let getCharMapping (clueNumbers: string[]) (charCounts: int[]) =
    
    List.empty<(char * char)>
    |> findA clueNumbers
    |> getUniqueCountChars charCounts
    |> findC charCounts
    |> findD clueNumbers
    |> findG 

let part2Evaluator (sourceLine: string[]) =
    //Slice up the source data into more useful pieces
    let delim = Array.findIndex (fun x -> x = "|") sourceLine
    let clueNumbers = Array.init delim (fun i -> sourceLine.[i])
    let answerNumbers = Array.init (sourceLine.Length - delim - 1) (fun i -> sourceLine.[i + delim + 1])

    //Count how many times each char appears in the data as this is key to the deduction process
    let charCounts = Array.init numChars (fun i -> Array.fold (fun count string -> if String.exists (eq (indexToChar i)) string then count + 1 else count) 0 clueNumbers)
    let charMap = getCharMapping clueNumbers charCounts

    //Use the char map to translate the digits, and then convert to a single base 10 number
    Array.map (fun x -> translateString x charMap) answerNumbers
    |> Array.fold (fun s x-> 10*s + x) 0

//Entry point
let main projectDir =

    let sourceData = getText projectDir
    let part1 = Array.sum (Array.map (fun x -> count1478s x) sourceData)
    let part2 = Array.sum (Array.map (fun x -> part2Evaluator x) sourceData)

    Console.WriteLine("Part 1: " + part1.ToString() )
    Console.WriteLine("Part 2: " + part2.ToString()  )
    8

//Deduction steps:
//Character in 7 but not 1 is the a                                                                                     :a
//Count how many times each char appears. 4, 6 and 9 occurences are the true e, b, f                                    :b,e,f
//The character that is not the a but occurs 8 times in the c                                                           :c
//The character that is missing from a string that is missing one and is not the c or e is the d                        :d
//The remaining character is the g                                                                                      :g