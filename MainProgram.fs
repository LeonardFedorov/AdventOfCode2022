open System

[<EntryPoint>]
let main argv =
    
    let rec mainiter projectDir codeSum =
    
        Console.WriteLine("\nSelect day to run calculation for:")
        let selection = Console.ReadLine()
        Console.Write("\n")
        
        Console.WriteLine("Day " + selection + " Results:")

        let timer = new System.Diagnostics.Stopwatch()
        timer.Start()

        let code = match selection with
                    | "1"  -> Day1.main projectDir
                    | "2"  -> Day2.main projectDir
                    | "3"  -> Day3.main projectDir
                    | "4"  -> Day4.main projectDir
                    | "5"  -> Day5.main projectDir
                    | "6"  -> Day6.main projectDir
                    | "7"  -> Day7.main projectDir
                    | "8"  -> Day8.main projectDir
                    | "9"  -> Day9.main projectDir
                    | "10" -> Day10.main projectDir
                    | "11" -> Day11.main projectDir
                    | "12" -> Day12.main projectDir
                    | "13" -> Day13.main projectDir
                    | "14" -> Day14.main projectDir
                    | "15" -> Day15.main projectDir
                    | "16" -> Day16.main projectDir
                    | "17" -> Day17.main projectDir
                    | "18" -> Day18.main projectDir
                    | "19" -> Day19.main projectDir
                    | "20" -> Day20.main projectDir
                    | "21" -> Day21.main projectDir
                    | "22" -> Day22.main projectDir
                    | "23" -> Day23.main projectDir
                    | "24" -> Day24.main projectDir
                    | "25" -> Day25.main projectDir
                    |  _   -> Console.WriteLine("Unrecognised input code.")
                              -1

        timer.Stop()
        let timeTaken = float(timer.ElapsedTicks) / float(System.Diagnostics.Stopwatch.Frequency)
        Console.WriteLine("\nTime Taken: " + (timeTaken).ToString() + " seconds")
  
        Console.WriteLine("\nCompute another day? y/n")
        let response = Console.ReadLine()
        match response with
            | "y" | "Y" -> mainiter projectDir (codeSum + code)
            | _ -> codeSum + code

    let projectDir = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\InputData"

    Console.WriteLine("#####################")
    Console.WriteLine("#Advent Of Code 2022#")
    Console.WriteLine("#####################")

    mainiter projectDir 0