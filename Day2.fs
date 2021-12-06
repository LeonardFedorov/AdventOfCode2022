[<AutoOpen>]
module Day2

open System
open System.IO

//COMMON

//Read the input in from the source file
let getText =
    let fileStream = new StreamReader("C:\Documents\Advent Of Code\Day2Input.txt")
    fileStream.ReadToEnd().Split('\n')

//PART 1

//Perform the update step
let p1posUpdate (f,d) (instruction: string) =
    let instructionSplit = instruction.Split(' ')
    let value = Int32.Parse(instructionSplit.[1]) 
    match instructionSplit.[0] with
    | "forward" -> (f + value, d)
    | "down" -> (f, d + value)
    | "up" -> (f, d - value)
    | _ -> (f, d)

//Evaluate the answer
let part1 =
    let finalPos = Array.fold (fun (f,d) x -> (p1posUpdate (f,d) x)) (0,0) getText
    (fst finalPos) * (snd finalPos)

//PART 2

//Extract the final answer from the end state
let finalAnswer (f,d,a) = f*d

//Perform the update step
let p2posUpdate (f,d,a) (instruction: string) =
    let instructionSplit = instruction.Split(' ')
    let value = Int32.Parse(instructionSplit.[1]) 
    match instructionSplit.[0] with
    | "forward" -> (f + value, d + (a * value), a)
    | "down" -> (f, d , a + value)
    | "up" -> (f, d, a - value)
    | _ -> (f,d,a)

//Evalutate the answer
let part2 =
    Array.fold (fun (f,d,a) x -> (p2posUpdate (f,d,a) x)) (0,0,0) getText
    |> finalAnswer 

let mainDay2 =
    Console.WriteLine("Part 1: " + part1.ToString())
    Console.WriteLine("Part 2: " + part2.ToString())
    2 // return an integer exit code