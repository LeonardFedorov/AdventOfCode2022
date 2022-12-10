[<AutoOpen>]
module Day10

open System
open System.IO

//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day10Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
        |> Array.map (fun line -> (line.[0..3], snd (Int32.TryParse(line.[4..]))))

    //Part 1
    let cycleCount =
        let countOp opcode = 
            Array.fold (fun counter item -> if fst item = opcode then counter + 1 else counter) 0 sourceData
        countOp "noop" + 2 * countOp "addx"

    let signalResult =
        let mutable results = Array.create (cycleCount + 1) 1
        let mutable cycleCount = 0

        Array.iter (
            fun (opcode, value) ->
                results.[cycleCount+1] <- results.[cycleCount]
                if opcode ="noop" then  
                    cycleCount <- cycleCount + 1
                elif opcode = "addx" then
                    results.[cycleCount+2] <- results.[cycleCount+1] + value
                    cycleCount <- cycleCount + 2
                else
                    failwith "Unknown opcode"
        ) sourceData
        
        results

    //Part 2
    let screenOutput =
        Array.init 240 (fun pixel -> if Math.Abs(pixel % 40 - signalResult.[pixel]) <= 1 then '#' else '.')
        |> String

    //Output
    let part1 = Array.fold (fun total value -> value * signalResult.[value - 1] + total) 0 [|20;60;100;140;180;220|]

    Console.WriteLine("Part 1: " + part1.ToString())
    Console.WriteLine("Part 2: "  )
    for i in 0 .. screenOutput.Length/40 - 1 do
        Console.WriteLine(screenOutput.[40*i..40*(i+1) - 1])

    10