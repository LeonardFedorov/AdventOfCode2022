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
let rollMultipliers = [|1L;3L;6L;7L;6L;3L;1L|]

let getNewPositions currentPos = Array.init 7 (fun i -> updatePos currentPos (i+3))

let rec game2P1Turn (p1Pos, p2Pos) (p1Score, p2Score) =
    
    //Check if player 2 won on their previous turn before processing player 1's next turn
    if p2Score >= scoreLimit then
        (1L, 0L)
    else
        getNewPositions p1Pos
        |> Array.map (fun newPos -> game2P2Turn (newPos, p2Pos) (p1Score + newPos, p2Score)) 
        |> Array.fold2 (fun (gameCounts, p1Wins) multiplicity (thisCount, thisWins) -> (gameCounts + multiplicity*thisCount, p1Wins + multiplicity*thisWins)) (0L,0L) rollMultipliers

and game2P2Turn (p1Pos, p2Pos) (p1Score, p2Score) =

    //Check if player 1 won on their previous turn before processing player 2's next turn
    if p1Score >= scoreLimit then
        (1L, 1L)
    else
        getNewPositions p2Pos
        |> Array.map (fun newPos -> game2P1Turn (p1Pos, newPos) (p1Score, p2Score + newPos)) 
        |> Array.fold2 (fun (gameCounts, p1Wins) multiplicity (thisCount, thisWins) -> (gameCounts + multiplicity*thisCount, p1Wins + multiplicity*thisWins)) (0L,0L) rollMultipliers

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