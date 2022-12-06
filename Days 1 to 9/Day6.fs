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

    let findFirstMarker markerLength =
        [|markerLength .. sourceData.Length|] //Input data is 1 indexed, so will use that here and adjust when we read the string
        |> Array.find (fun i -> allDistinct sourceData.[i - markerLength .. i - 1])  

    //Output
    Console.WriteLine("Part 1: " + (findFirstMarker 4).ToString() )
    Console.WriteLine("Part 2: " + (findFirstMarker 14).ToString()  )
    6