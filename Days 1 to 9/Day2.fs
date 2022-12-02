[<AutoOpen>]
module Day2

open System
open System.IO

//Entry point
let main projectDir =

    //Functions common to the both parts
    let standardise item =
        match item with
            | 'A'| 'X' -> "Rock"
            | 'B'| 'Y' -> "Paper"
            | 'C'| 'Z' -> "Scissors"
            | _ -> failwith "Unknown input object"

    let itemPoints item =
        match item with
            | "Rock" -> 1
            | "Paper" -> 2
            | "Scissors" -> 3
            | _ -> failwith "Unknown item being scored"

    //Returns which item wins - 1 for item1 and 2 for item2. Returns 0 if a draw
    let winningItem item1 item2 =
        if item1 = item2 then 0
        else match (item1, item2) with
                | ("Rock", "Scissors") -> 1
                | ("Rock", "Paper") -> 2
                | ("Paper", "Rock") -> 1
                | ("Paper", "Scissors") -> 2
                | ("Scissors", "Paper") -> 1
                | ("Scissors", "Rock") -> 2
                | _ -> failwith "Unknown matchup"

    let roundResult player1 player2 =
        let itemScore = itemPoints player2
        let resultScore = match winningItem player1 player2 with
                            | 0 -> 3
                            | 1 -> 0
                            | 2 -> 6
                            | _ -> failwith "Unexpected result from winningItem"

        itemScore + resultScore

    //Read data in as store as a tuple
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day2Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
        |> Array.map (fun pairing -> (standardise pairing.[0], pairing.[2]))
    
    //Part 1

    let standardisedInput =
        Array.map (fun (a,b) -> (a, standardise b)) sourceData

    let part1Result = 
        Array.fold (fun totalScore currentRound -> totalScore + roundResult (fst currentRound) (snd currentRound)) 0 standardisedInput

    //Part 2

    let getBetterItem item =
        match item with
            | "Rock" -> "Paper"
            | "Paper" -> "Scissors"
            | "Scissors" -> "Rock"
            | _ -> failwith "Unknown item"

    let getWorseItem item = 
        match item with
            | "Rock" -> "Scissors"
            | "Paper" -> "Rock"
            | "Scissors" -> "Paper"
            | _ -> failwith "Unknown item"

    let getMove player1Item player2Move =
        match player2Move with
            | 'X' -> getWorseItem player1Item
            | 'Y' -> player1Item
            | 'Z' -> getBetterItem player1Item
            | _ -> failwith "Unexpected strategy suggestion"

    let part2Result =
        Array.fold (fun totalScore (player1Item, player2Strat) -> totalScore + roundResult player1Item (getMove player1Item player2Strat)) 0 sourceData

    //Output

    Console.WriteLine("Part 1: " + part1Result.ToString())
    Console.WriteLine("Part 2: " + part2Result.ToString())
    2