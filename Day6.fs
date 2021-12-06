[<AutoOpen>]
module Day6

open System
open System.IO

let fishStateCount = 9

let arrayCount array n =
    Array.fold (fun s x -> if x = n then s + 1L else s) 0L array

let getText =
    let fileStream = new StreamReader("C:\Documents\Advent Of Code\Day6Input.txt")
    fileStream.ReadToEnd().Split(',')
    |> Array.map (fun a -> Int64.Parse(a))

let fishUpdate (fishArray: int64[]) =
    Array.init fishArray.Length (fun i -> match i with
                                          | 6 -> fishArray.[0] + fishArray.[7]
                                          | 8 -> fishArray.[0]
                                          | x -> fishArray.[x + 1]
                                )

let rec fishSim fishArray iters =

    if iters = 0 then
        Array.sum fishArray
    else
        fishSim (fishUpdate fishArray) (iters - 1)

let mainDay6 =
    let rawSourceData = getText
    let sourceData = Array.init fishStateCount (fun i -> arrayCount rawSourceData (int64(i)))
    

    Console.WriteLine("Part 1: " + (fishSim sourceData 80).ToString() + " fish" )
    Console.WriteLine("Part 2: " + (fishSim sourceData 256).ToString() + " fish")
    6