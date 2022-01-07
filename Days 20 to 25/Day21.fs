[<AutoOpen>]
module Day21

open System
open System.IO

//Common
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day21Input.txt")
    fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    |> Array.map (fun (line: string) -> let spaceSplit = line.Split(' ')
                                        Int32.Parse spaceSplit.[spaceSplit.Length - 1]
                  )

let updatePos oldPos roll = 1 + (oldPos + roll - 1) % 10

//Part 1
let game1Die rollNumber =
    (rollNumber - 1) % 100 + 1 + (rollNumber + 1 - 1) % 100 + 1 + (rollNumber + 2 - 1) % 100 + 1   

let rec runGame1 (p1Pos, p2Pos) (p1Score, p2Score) diceRoll =
    
    let newP1Pos = updatePos p1Pos (game1Die (diceRoll + 1))
    let newP2Pos = updatePos p2Pos (game1Die (diceRoll + 4))
    let newP1Score = p1Score + newP1Pos
    let newP2Score = p2Score + newP2Pos

    if newP1Score >= 1000 then
        p2Score * (diceRoll + 3) 
    elif newP2Score >= 1000 then
        newP1Score * (diceRoll + 6)
    else
        runGame1 (newP1Pos, newP2Pos) (newP1Score, newP2Score) (diceRoll + 6)

//Part 2
let scoreLimit = 21

//let rollMultipliers = [|(3,1);(4,3);(5,6);(6,10);(7,6);(8,3);(9,1)|]

let getNewPositions currentPos = (updatePos currentPos 3, updatePos currentPos 4, updatePos currentPos 5, updatePos currentPos 6, updatePos currentPos 7, updatePos currentPos 8, updatePos currentPos 9) 

let rec game2P1Turn (p1Pos, p2Pos) (p1Score, p2Score) =
    
    //Check if player 2 won on their previous turn before processing player 1's next turn
    if p2Score >= scoreLimit then
        (1L, 0L)
    else
        let (newPos1, newPos2, newPos3, newPos4, newPos5, newPos6, newPos7) = getNewPositions p1Pos

        let (v1GameCount, v1P1Wins) = game2P2Turn (newPos1, p2Pos) (p1Score + newPos1, p2Score)
        let (v2GameCount, v2P1Wins) = game2P2Turn (newPos2, p2Pos) (p1Score + newPos2, p2Score)
        let (v3GameCount, v3P1Wins) = game2P2Turn (newPos3, p2Pos) (p1Score + newPos3, p2Score)
        let (v4GameCount, v4P1Wins) = game2P2Turn (newPos4, p2Pos) (p1Score + newPos4, p2Score)
        let (v5GameCount, v5P1Wins) = game2P2Turn (newPos5, p2Pos) (p1Score + newPos5, p2Score)
        let (v6GameCount, v6P1Wins) = game2P2Turn (newPos6, p2Pos) (p1Score + newPos6, p2Score)
        let (v7GameCount, v7P1Wins) = game2P2Turn (newPos7, p2Pos) (p1Score + newPos7, p2Score)
   
        (v1GameCount + 3L*v2GameCount + 6L*v3GameCount + 7L*v4GameCount + 6L*v5GameCount + 3L*v6GameCount + v7GameCount, 
         v1P1Wins + 3L*v2P1Wins + 6L*v3P1Wins + 7L*v4P1Wins + 6L*v5P1Wins + 3L*v6P1Wins + v7P1Wins)

and game2P2Turn (p1Pos, p2Pos) (p1Score, p2Score) =

    //Check if player 1 won on their previous turn before processing player 2's next turn
    if p1Score >= scoreLimit then
        (1L, 1L)
    else
        let (newPos1, newPos2, newPos3, newPos4, newPos5, newPos6, newPos7) = getNewPositions p2Pos

        let (v1GameCount, v1P1Wins) = game2P1Turn (p1Pos, newPos1) (p1Score, p2Score + newPos1)
        let (v2GameCount, v2P1Wins) = game2P1Turn (p1Pos, newPos2) (p1Score, p2Score + newPos2)
        let (v3GameCount, v3P1Wins) = game2P1Turn (p1Pos, newPos3) (p1Score, p2Score + newPos3)
        let (v4GameCount, v4P1Wins) = game2P1Turn (p1Pos, newPos4) (p1Score, p2Score + newPos4)
        let (v5GameCount, v5P1Wins) = game2P1Turn (p1Pos, newPos5) (p1Score, p2Score + newPos5)
        let (v6GameCount, v6P1Wins) = game2P1Turn (p1Pos, newPos6) (p1Score, p2Score + newPos6)
        let (v7GameCount, v7P1Wins) = game2P1Turn (p1Pos, newPos7) (p1Score, p2Score + newPos7)

        (v1GameCount + 3L*v2GameCount + 6L*v3GameCount + 7L*v4GameCount + 6L*v5GameCount + 3L*v6GameCount + v7GameCount, 
         v1P1Wins + 3L*v2P1Wins + 6L*v3P1Wins + 7L*v4P1Wins + 6L*v5P1Wins + 3L*v6P1Wins + v7P1Wins)

//Entry point
let main projectDir =

    let sourceData = getText projectDir

    let part1 = runGame1 (sourceData.[0], sourceData.[1]) (0,0) 0
    let part2 = game2P1Turn (sourceData.[0], sourceData.[1]) (0,0)

    let p1Wins = snd part2
    let p2Wins = fst part2 - p1Wins

    Console.WriteLine("Part 1: " + part1.ToString())
    Console.WriteLine("Part 2: " + (max p1Wins p2Wins).ToString())
    21