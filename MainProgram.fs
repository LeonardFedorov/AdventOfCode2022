open System

let rec mainiter codeSum =

    Console.WriteLine("\nSelect day to run calculation for:")
    let selection = Console.ReadLine()
    Console.Write("\n")
    let projectDir = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\InputData"

    let code = match selection with
                | "1" -> Console.WriteLine("Day " + selection + " Results:")
                         Day1.main projectDir
                | "2" -> Console.WriteLine("Day " + selection + " Results:")
                         Day2.main projectDir
                | "3" -> Console.WriteLine("Day " + selection + " Results:")
                         Day3.main projectDir
                | "4" -> Console.WriteLine("Day " + selection + " Results:")
                         Day4.main projectDir
                | "5" -> Console.WriteLine("Day " + selection + " Results:")
                         Day5.main projectDir
                | "6" -> Console.WriteLine("Day " + selection + " Results:")
                         Day6.main projectDir
                | "7" -> Console.WriteLine("Day " + selection + " Results:")
                         Day7.main projectDir
                | "8" -> Console.WriteLine("Day " + selection + " Results:")
                         Day8.main projectDir

                | _ -> Console.WriteLine("Unrecognised input code.")
                       -1

    Console.WriteLine("\nCompute another day? y/n")
    let response = Console.ReadLine()
    match response with
        | "y" | "Y" -> mainiter (codeSum + code)
        | _ -> codeSum + code

[<EntryPoint>]
let main argv =
    
    Console.WriteLine("#####################")
    Console.WriteLine("#Advent Of Code 2021#")
    Console.WriteLine("#####################")

    mainiter 0