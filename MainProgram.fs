open System

[<EntryPoint>]
let main argv =

    Console.WriteLine("Select day to run calculation for:")
    let selection = Console.ReadLine()
    Console.Write("\n")

    match selection with
    | "1" -> Console.WriteLine("Day " + selection + " Results:")
             Day1.mainDay1
    | "2" -> Console.WriteLine("Day " + selection + " Results:")
             Day2.mainDay2
    | "3" -> Console.WriteLine("Day " + selection + " Results:")
             Day3.mainDay3
    | "4" -> Console.WriteLine("Day " + selection + " Results:")
             Day4.mainDay4
    | "5" -> Console.WriteLine("Day " + selection + " Results:")
             Day5.mainDay5
    | "6" -> Console.WriteLine("Day " + selection + " Results:")
             Day6.mainDay6

    | _ -> Console.WriteLine("Unrecognised input code.")
           -1

