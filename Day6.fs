[<AutoOpen>]
module Day6

open System
open System.IO

let getText =
    let fileStream = new StreamReader("C:\Documents\Advent Of Code\Day6Input.txt")
    fileStream.ReadToEnd().Split(',')
    |> Array.map (fun a -> Int32.Parse(a))


let fishSim sourceData iters =


    0


let mainDay6 =
    let sourceData = getText


    Console.WriteLine("Part 1: " + (part12 1).ToString() + " fish" )
    Console.WriteLine("Part 2: " + (part12 3).ToString() + " fish")
    6