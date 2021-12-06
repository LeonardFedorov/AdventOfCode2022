[<AutoOpen>]
module Day1

open System
open System.IO

let getText =
    let fileStream = new StreamReader("C:\Documents\Advent Of Code\Day1Input.txt")
    fileStream.ReadToEnd().Split('\n')
    |> Array.map (fun a -> Int32.Parse(a))

//Key observation: if we are comparing sums of n consecutive elements, 
//                 then s_i+1 > s_i iff a[i+n] > a[i]
let part12 n (sourceArray: int[]) =
    Seq.init (sourceArray.Length - n) (fun i -> if sourceArray.[i+n] > sourceArray.[i] 
                                                then 1 else 0)
    |> Seq.sum

let mainDay1 =
    let sourceData = getText
    Console.WriteLine("Part 1: " + (part12 1 sourceData).ToString() )
    Console.WriteLine("Part 2: " + (part12 3 sourceData).ToString() )
    1