open System

[<EntryPoint>]
let main argv =
    
    let fileDir = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\InputData"

    let rec mainiter codeSum =
    
        Console.WriteLine("\nSelect day to run calculation for:")
        let selection = Console.ReadLine()
        Console.Write("\n")
        
        Console.WriteLine("Day " + selection + " Results:")

        let timer = new Diagnostics.Stopwatch()
        timer.Start()

        let code = match selection with
                    | "1"  -> Day1.main fileDir
                    | "2"  -> Day2.main fileDir
                    | "3"  -> Day3.main fileDir
                    | "4"  -> Day4.main fileDir
                    | "5"  -> Day5.main fileDir
                    | "6"  -> Day6.main fileDir
                    | "7"  -> Day7.main fileDir
                    | "8"  -> Day8.main fileDir
                    | "9"  -> Day9.main fileDir
                    | "10" -> Day10.main fileDir
                    | "11" -> Day11.main fileDir
                    | "12" -> Day12.main fileDir
                    | "13" -> Day13.main fileDir
                    | "14" -> Day14.main fileDir
                    | "15" -> Day15.main fileDir
                    | "16" -> Day16.main fileDir
                    | "17" -> Day17.main fileDir
                    | "18" -> Day18.main fileDir
                    | "19" -> Day19.main fileDir
                    | "20" -> Day20.main fileDir
                    | "21" -> Day21.main fileDir
                    | "22" -> Day22.main fileDir
                    | "23" -> Day23.main fileDir
                    | "24" -> Day24.main fileDir
                    | "25" -> Day25.main fileDir
                    |  _   -> Console.WriteLine("Unrecognised input code.")
                              -1
    
        timer.Stop()
        let timeTaken = (float timer.ElapsedTicks) / (float Diagnostics.Stopwatch.Frequency)
        Console.WriteLine("\nTime taken: " + timeTaken.ToString() + " seconds")

        Console.WriteLine("\nCompute another day? y/n")
        let response = Console.ReadLine()
        match response with
            | "y" | "Y" -> mainiter (codeSum + code)
            | _ -> codeSum + code
 
    Console.WriteLine("#####################")
    Console.WriteLine("#Advent Of Code 2022#")
    Console.WriteLine("#####################")

    mainiter  0