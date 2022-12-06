[<AutoOpen>]
module Day6

open System
open System.IO

//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day6Input.txt")
        fileStream.ReadToEnd().ToCharArray()

    let allDistinct (array: char []) = 
        (Array.distinct array).Length = array.Length

    let findFirstMarker start markerLength =
        //Input data is 1 indexed, so will use that here and adjust when we read the string
        Array.find (fun i -> allDistinct sourceData.[i - markerLength .. i - 1]) [|start + markerLength .. sourceData.Length|] 

    //Output
    let part1 = findFirstMarker 0 4
    let part2 = findFirstMarker (part1 - 4) 14

    Console.WriteLine("Part 1: " + part1.ToString() )
    Console.WriteLine("Part 2: " + part2.ToString()  )
    6