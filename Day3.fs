[<AutoOpen>]
module Day3

open System
open System.IO

//COMMON

//Read the input in from the source file
let getText =
    let fileStream = new StreamReader("C:\Documents\Advent Of Code\Day3Input.txt")
    fileStream.ReadToEnd().Split('\n')

//Idea courtesy of Hayes - original was much jankier and involved doing a count of 1s compared against
//n/2 which involved some rounding trickiness to get right
let commonChar (sourceData: string list) = 
    Array.init sourceData.[0].Length (fun i -> (List.fold (fun s (x: string) -> s + 2 * (int x.[i] - int '0') - 1)) 0 sourceData)
    |> Array.map (fun x -> if x > 0 then 1
                           elif x = 0 then -1
                           else 0)

let binToDec (digitArray: int[]) =
    Array.fold (fun s x -> 2*s + x) 0 digitArray

//PART 1

let getGamma (commonChars: int[]) =
    //If equal numbers of 1s and 0s (i.e. -1), then output a 1
    Array.map (fun x -> if x <> 0 then 1 else 0) commonChars
    |> binToDec

let getEpsilon (commonChars: int[]) =
    //If equal 1s and 0s, (i.e. -1) then output a 0
    Array.map (fun x -> if x = 0 then 1 else 0) commonChars
    |> binToDec

//PART 2

let co2filter (value: string) (commonChars: int[]) i =
    if commonChars.[i] = -1 then (int value.[i] - int '0') = 0 
                            else (int value.[i] - int '0') = (1 - commonChars.[i]) 

let o2filter (value: string) (commonChars: int[]) i =
    if commonChars.[i] = -1 then (int value.[i] - int '0') = 1 
                            else (int value.[i] - int '0') = commonChars.[i]

let rec part2compIter filterFunc stringList (commonChars: int[]) i =
    if i = commonChars.Length then failwith "loop running too long"
    let filteredList = List.filter (fun x -> filterFunc x commonChars i) stringList
    if filteredList.Length = 1 then filteredList.Head 
                               else part2compIter filterFunc filteredList (commonChar filteredList) (i + 1)

let part2comp filterFunc sourceData (commonChars: int[]) =
    let (targetValue: string) = part2compIter filterFunc (List.ofArray sourceData) commonChars  0
    targetValue.ToCharArray()
    |> Array.map (fun x -> int x - int '0')
    |> binToDec

let mainDay3 =
    let sourceData = getText
    let commonChars = commonChar (List.ofArray sourceData)

    let gamma = getGamma commonChars
    let epsilon = getEpsilon commonChars
    let co2scrubber = part2comp co2filter sourceData commonChars
    let o2gen = part2comp o2filter sourceData commonChars

    Console.WriteLine("Part 1: " + " gamma = " + gamma.ToString() + 
                      ", epsilon = " + epsilon.ToString() + 
                      ", product = " + (gamma*epsilon).ToString() )
    Console.WriteLine("Part 2: " + " co2 = " + co2scrubber.ToString() + 
                      ", o2 = " + o2gen.ToString() + 
                      ", product = " + (co2scrubber*o2gen).ToString() )

    3 //expected integer return code