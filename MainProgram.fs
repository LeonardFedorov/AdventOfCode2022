﻿open System

let rec mainiter projectDir codeSum =

    Console.WriteLine("\nSelect day to run calculation for:")
    let selection = Console.ReadLine()
    Console.Write("\n")
    
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
                | "9" -> Console.WriteLine("Day " + selection + " Results:")
                         Day9.main projectDir
                | "10" -> Console.WriteLine("Day " + selection + " Results:")
                          Day10.main projectDir
                | "11" -> Console.WriteLine("Day " + selection + " Results:")
                          Day11.main projectDir
                | "12" -> Console.WriteLine("Day " + selection + " Results:")
                          Day12.main projectDir
                | "13" -> Console.WriteLine("Day " + selection + " Results:")
                          Day13.main projectDir
                | "14" -> Console.WriteLine("Day " + selection + " Results:")
                          Day14.main projectDir
                | "15" -> Console.WriteLine("Day " + selection + " Results:")
                          Day15.main projectDir
                | "16" -> Console.WriteLine("Day " + selection + " Results:")
                          Day16.main projectDir
                | "17" -> Console.WriteLine("Day " + selection + " Results:")
                          Day17.main projectDir
                | "18" -> Console.WriteLine("Day " + selection + " Results:")
                          Day18.main projectDir
                | "19" -> Console.WriteLine("Day " + selection + " Results:")
                          Day19.main projectDir
                | "20" -> Console.WriteLine("Day " + selection + " Results:")
                          Day20.main projectDir
                | "21" -> Console.WriteLine("Day " + selection + " Results:")
                          Day21.main projectDir
                | "22" -> Console.WriteLine("Day " + selection + " Results:")
                          Day22.main projectDir
                | "23" -> Console.WriteLine("Day " + selection + " Results:")
                          Day23.main projectDir
                | "24" -> Console.WriteLine("Day " + selection + " Results:")
                          Day24.main projectDir
                | "25" -> Console.WriteLine("Day " + selection + " Results:")
                          Day25.main projectDir
                | _ -> Console.WriteLine("Unrecognised input code.")
                       -1

    Console.WriteLine("\nCompute another day? y/n")
    let response = Console.ReadLine()
    match response with
        | "y" | "Y" -> mainiter projectDir (codeSum + code)
        | _ -> codeSum + code

[<EntryPoint>]
let main argv =
    
    let projectDir = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\InputData"

    Console.WriteLine("#####################")
    Console.WriteLine("#Advent Of Code 2022#")
    Console.WriteLine("#####################")

    mainiter projectDir 0