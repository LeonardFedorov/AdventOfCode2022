[<AutoOpen>]
module Day7

open System
open System.IO

let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day7Input.txt")
    fileStream.ReadToEnd().Split(',')
    |> Array.map (fun x -> Int32.Parse(x))
    |> Array.sort
 
let sumOfDistances numberArray n =
    Array.fold(fun s x -> s + abs(x - n)) 0 numberArray

let sumOfTriangleDistances numberArray n =
    Array.fold(fun s x -> s + abs(x-n)*(abs(x-n)+1)/2) 0 numberArray

let mainDay7 projectDir =
    let sourceData = getText projectDir
    let median = sourceData.[(sourceData.Length + 1)/2 - 1]
    let part2 = Array.init (Array.max sourceData) (fun i -> sumOfTriangleDistances sourceData i)
                |> Array.min
                
    Console.WriteLine("Part 1: " + (sumOfDistances sourceData median).ToString() )
    Console.WriteLine("Part 2: " + part2.ToString() )
    1