[<AutoOpen>]
module Day4

open System
open System.IO

//Function to transform the string of a bingo card into an int typed 2d array
let parseArray (arrayString: string) =
    let arrayOfArrays = arrayString.Split([|"\r\n"|], StringSplitOptions.None)
                        |> Array.map (fun x -> x.Split([|" "|], StringSplitOptions.RemoveEmptyEntries))
    Array2D.init arrayOfArrays.Length arrayOfArrays.[0].Length (fun i j -> Int32.Parse(arrayOfArrays.[i].[j]))

//Function to get and parse the problem input. 
//The result is a pair whose first element is the called numbers, and the second is a list of 2d arrays (the bingo cards)
let day4input projectDir =
    let fileStream = new StreamReader(projectDir + "\Day4Input.txt")
    //Do a split on double newlines to separate out the grids from each other first
    let stringRead = fileStream.ReadToEnd().Split([|"\r\n\r\n"|], StringSplitOptions.None)

    //Separate out the numbers feed and the bingo grids
    let theNumbers = Array.map (fun x -> Int32.Parse(x)) (stringRead.[0].Split(','))
    let theGrids = List.init (stringRead.Length - 1) (fun i -> parseArray stringRead.[i+1])
    (theNumbers, theGrids)

//function to find index of value in a 2d array. Returns (-1,-1) if not found
let ArrayFind2D (sourceArray: int[,]) value =
    let columnMatches = Array.init (Array2D.length1 sourceArray) (fun i -> Array.tryFindIndex (fun x -> x = value) (sourceArray.[i,*]) )
    let matchingRow = Array.tryFindIndex(fun x -> x <> None) columnMatches
    if matchingRow = None then (-1, -1) 
    else (matchingRow.Value, columnMatches.[matchingRow.Value].Value)

//when checking if the grid is winning, only need to check the row and column of the most recent update
let hasWon (boolGrid: bool[,]) gridRef =
    let rowTest = Array.fold (fun s x -> s && x) true boolGrid.[fst gridRef,*]
    let columnTest = Array.fold (fun s x -> s && x) true boolGrid.[*,snd gridRef]
    rowTest || columnTest

//cast the arrays to sequences to allow for simple linear processing as the order of summation doesn't matter
let scoreBoard grid liveGrid =
    Seq.fold2 (fun s value bool -> if bool = false then s + value else s) 0 (Seq.cast<int> grid) (Seq.cast<bool> liveGrid)

let rec timeToWinIter grid (theNumbers: int[]) (liveGrid: bool[,]) step =
    //If the number was not found, no need to perform any updates or checks so proceed immediately to next loop
    let gridRef = ArrayFind2D grid theNumbers.[step]
    if gridRef = (-1, -1) then timeToWinIter grid theNumbers liveGrid (step + 1) 
    else let newLiveGrid = Array2D.mapi (fun i j x -> if i = fst gridRef && j = snd gridRef then true else x) liveGrid
         if hasWon newLiveGrid gridRef then (step, scoreBoard grid newLiveGrid)
         else timeToWinIter grid theNumbers newLiveGrid (step + 1)

//Wrapper for iterator to calculate the bingo card's progress 
let timeToWin (grid: int[,]) (theNumbers: int[]) =
    timeToWinIter grid theNumbers (Array2D.create (Array2D.length1 grid) (Array2D.length2 grid) false) 0

//Split out accumulator update logic when analysing the final results for clarity
let updateAccumulator (accTime, accScore) (newTime, newScore) (compare: int -> int -> bool) =
    if compare newTime accTime then (newTime, newScore) else (accTime, accScore)


//Entry point
let main projectDir =
    let sourceData = day4input projectDir
    let result = Array.init (snd sourceData).Length (fun i -> timeToWin (snd sourceData).[i] (fst sourceData))
                 
    let part1 = Array.fold (fun acc newR -> updateAccumulator acc newR (fun x y -> x < y)) ((fst sourceData).Length + 1, 0) result
    let part2 = Array.fold (fun acc newR -> updateAccumulator acc newR (fun x y -> x > y)) (0, 0) result

    Console.WriteLine("The best board has score of " + (snd part1).ToString() + " after a final call of " + (fst sourceData).[fst part1].ToString() + ".")
    Console.WriteLine("The worst board has score of " + (snd part2).ToString() + " after a final call of " + (fst sourceData).[fst part2].ToString() + ".")
    Console.WriteLine("Multiplication has been left as an exercise to the reader.")

    4 //expected integer return code