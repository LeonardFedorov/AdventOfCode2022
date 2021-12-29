[<AutoOpen>]
module Day18

open System
open System.IO

type snailPart = {left: snailPart option; right: snailPart option; value: int}
                 static member defaultValue = {left = None; right = None; value = -1}

//Common
let rec parseSnailNumber (input: string) =
    let intParse = Int32.TryParse(input)
    if fst intParse then
        Some {snailPart.defaultValue with value = snd intParse}
    else
        let (separatorPlace, bracketCount, index) = Array.fold (fun (place, bracketCount, i) char -> match char with 
                                                                                                     | '[' -> (place, bracketCount + 1, i + 1)
                                                                                                     | ']' -> (place, bracketCount - 1, i + 1)
                                                                                                     | ',' -> if bracketCount = 1 then (i, bracketCount, i + 1) else (place, bracketCount, i + 1)
                                                                                                     | _ -> (place, bracketCount, i + 1)
                                                         ) (-1, 0, 0) (input.ToCharArray())
        Some {left = parseSnailNumber input.[1..(separatorPlace - 1)]; right = parseSnailNumber input.[(separatorPlace + 1)..(input.Length - 2)]; value = -1}

let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day18Input.txt")
    fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    |> Array.map (fun x -> parseSnailNumber x)

//Perform the split operation
let rec doSplit snailNumber =
    
    if snailNumber.left.IsNone then
        let oldValue = snailNumber.value
        if oldValue > 9 then ({left = Some {snailPart.defaultValue with value = oldValue/2};
                                        right = Some {snailPart.defaultValue with value = oldValue/2 + (oldValue % 2)}; 
                                        value = -1}, true) 
        else (snailNumber, false)                   
    else 
        let leftSplit = doSplit snailNumber.left.Value
        if snd leftSplit then
            ({snailNumber with left = Some (fst leftSplit)}, true)
        else
            let rightSplit = doSplit snailNumber.right.Value
            ({snailNumber with right = Some (fst rightSplit)}, snd rightSplit)

//Perform the explode operation
let rec doExplode snailNumber depth =
    
    //If we are looking at a leaf, no further checking needed
    if snailNumber.left.IsNone then
        (snailNumber, false)
    else
        let oldLeft = snailNumber.left.Value
        let oldRight = snailNumber.right.Value

        //if the depth is >= 3, then explode the next pair down
        if depth >= 3 then
            //Check the left item in the current pair to see if it is a leaf. We know that not both of them are
            if oldLeft.left.IsValue then
                


            
        else
            


            (snailNumber, true)


//Entry point
let main projectDir =

    let testItem = doSplit (parseSnailNumber "[[4,2],12").Value

    let sourceData = getText projectDir

    Console.WriteLine("Part 1: Not Implemented."  )
    Console.WriteLine("Part 2: Not Implemented."  )
    18