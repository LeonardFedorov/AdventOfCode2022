[<AutoOpen>]
module Day19

open System
open System.IO

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day19Input.txt")
    let splitbyScanner = fileStream.ReadToEnd().Split([|"\r\n\r\n"|], StringSplitOptions.None)

    //Break apart the scan data such that we have the beacon cordinates stored as integer arrays
    Array.map (fun (scanData: string) -> scanData.Split([|"\r\n"|], StringSplitOptions.None)) splitbyScanner
    |> Array.map (fun (scanData: string[]) -> 
                      //Set the array length one shorter as we will discard the scanner labels in the input data
                      Array.init (scanData.Length - 1) (fun i -> 
                                                                Array.map (fun number -> Int32.Parse(number)) (scanData.[i+1].Split(','))
                                                        )
                  )

//Part 1


//Part 2


//Entry point
let main projectDir =

    let sourceData = getText projectDir

    Console.WriteLine("Part 1: Not Implemented."  )
    Console.WriteLine("Part 2: Not Implemented."  )
    19