[<AutoOpen>]
module Day2

open System
open System.IO

//Items are valued at their intrinsic score
type items =
    | Rock = 1
    | Scissors = 3
    | Paper = 2

//Entry point
let main projectDir =      

    //Functions common to the both parts

    //This array is set up so that each item is placed at the index of its score, and that each item beats
    //the item that preceeds it and loses to the item that follows it
    let itemSequence = [|items.Scissors; items.Rock; items.Paper; items.Scissors; items.Rock|]

    let standardise item =
        match item with
            | 'A'| 'X' -> items.Rock
            | 'B'| 'Y' -> items.Paper
            | 'C'| 'Z' -> items.Scissors
            | _ -> failwith "Unknown input object"

    //Returns which item wins - 1 for item1 and 2 for item2. Returns 0 if a draw
    let winningItem item1 item2 =
        if item1 = item2 then 0
        elif itemSequence.[int item1 - 1] = item2 then 1
        else 2

    let roundResult player1 player2 =
        let resultScore = match winningItem player1 player2 with
                            | 0 -> 3
                            | 1 -> 0
                            | 2 -> 6
                            | _ -> failwith "Unexpected result from winningItem"

        int player2 + resultScore

    //Read data in as store as a tuple. Convert player 1's value straight to items, but leave player 2's as it has different
    //meanings in parts 1 and 2.
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

    let getBetterItem item = itemSequence.[int item + 1]

    let getWorseItem item = itemSequence.[int item - 1]

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