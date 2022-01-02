[<AutoOpen>]
module Day18

open System
open System.IO

type snailPart = 
    | Value of int32
    | Node of snailPart * snailPart

    override this.ToString() =
        match this with
            | Value(x) -> x.ToString()
            | Node(left, right) -> "[" + left.ToString() + "," + right.ToString() + "]"

type pushDir =
    | noDebris = 0
    | left = 1
    | right = 2

//Common

let isValue snailPart =
    match snailPart with
        | Value(x) -> true
        | Node(x,y) -> false

let rec parseSnailNumber (input: string) =
    let intParse = Int32.TryParse(input)
    //If we are looking at an integer, then this is a terminal node. Otherwise, proceed to parse the pair text
    if fst intParse then
        Value(snd intParse) 
    else
        //Find where the comma which is scoped to the outermost bracket is located
        let (separatorPlace, bracketCount, index) = Array.fold (fun (place, bracketCount, i) char -> match char with 
                                                                                                     | '[' -> (place, bracketCount + 1, i + 1)
                                                                                                     | ']' -> (place, bracketCount - 1, i + 1)
                                                                                                     | ',' -> if bracketCount = 1 then (i, bracketCount, i + 1) else (place, bracketCount, i + 1)
                                                                                                     | _ -> (place, bracketCount, i + 1)
                                                         ) (-1, 0, 0) (input.ToCharArray())
        //Slice the string either side of the highest scoped comma and exclude the outermost brackets
        Node(parseSnailNumber input.[1..(separatorPlace - 1)], parseSnailNumber input.[(separatorPlace + 1)..(input.Length - 2)])

let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day18Input.txt")
    fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
    |> Array.map (fun x -> parseSnailNumber x)
    |> List.ofArray

//Perform the split operation
let rec doSplit (snailNumber: snailPart) =
    
    match snailNumber with
        | Value(x) ->
            if x > 9 then (Node(Value(x/2), Value(x/2 + (x % 2))) , true)
            else (snailNumber, false)                   
        | Node(left, right) ->
            let leftSplit = doSplit left
            if snd leftSplit then
                (Node(fst leftSplit, right), true)
            else
                let rightSplit = doSplit right
                if snd rightSplit then
                    (Node(left, fst rightSplit), true)
                else
                    (snailNumber, false) 
                
//Explosions

//Search for the left(/right)-most number from a given node to add the explosion debris amount to
let rec addToLeft snailNumber debris =
    match snailNumber with
        | Value(x) -> Value(x + debris)
        | Node(left, right) -> Node(addToLeft left debris, right)

let rec addToRight snailNumber debris =
    match snailNumber with
        | Value(x) -> Value(x + debris)
        | Node(left, right) -> Node(left, addToRight right debris)

       
//Perform the explode operation
let rec doExplode snailNumber depth =
    
    let noExplodeResult = (snailNumber, false, pushDir.noDebris, 0)

    match snailNumber with
         //If we are looking at a leaf, no further checking needed
        | Value(x) -> noExplodeResult
        | Node(left, right) ->
            //if the depth is >= 3, then explode the next pair down
            if depth >= 4 then
                match left with
                    | Node(Value(a), Value(b)) ->
                        (Node(Value(0), addToLeft right b), true, pushDir.left, a)
                    | Value(x) ->

                        match right with
                            | Node(Value(a), Value(b)) ->
                                (Node(addToRight left a, Value(0)), true, pushDir.right, b)
                            | Value(x) -> noExplodeResult
                            | _ -> failwith "Encountered pair at depth greater than 4"

                    | _ -> failwith "Encountered pair at depth greater than 4"
              
            else
                //Search on the left branch for explosions first, and only check the right if one isn't found
                let (newNumber, exploded, direction, debris) = doExplode left (depth + 1)
                if exploded then 
                    if direction = pushDir.right then
                        (Node(newNumber, addToLeft right debris), exploded, pushDir.noDebris, 0)
                    else
                        (Node(newNumber, right), exploded, direction, debris)
                else 
                    let (newNumber, exploded, direction, debris) = doExplode right (depth + 1)
                    if exploded then
                        if direction = pushDir.left then
                            (Node(addToRight left debris, newNumber), exploded, pushDir.noDebris, 0)
                        else
                            (Node(left, newNumber), exploded, direction, debris)
                    else
                        noExplodeResult

let callExplode snailNumber =
    //push and debris are ignored since if they are still non-zero the value has fallen out of the number
    let (resultNumber, exploded, push, debris) = doExplode snailNumber 1
    (resultNumber, exploded)

//Reduction and addition
let rec reduceNumber snailNumber =
    let (explodedNumber, explodeResult) = callExplode snailNumber
    let (finalNumber, finalResult) = if not explodeResult then doSplit explodedNumber
                                     else (explodedNumber, explodeResult)

    if finalResult then reduceNumber finalNumber
    else finalNumber

let snailAdd a b =
    reduceNumber (Node(a,b))

let rec magnitude snailNumber =
    match snailNumber with
        | Value(x) -> x
        | Node(left, right) -> 3 * (magnitude left) + 2 * (magnitude right)

let addToAll snailNumber numberList =
    List.map (fun x -> magnitude (snailAdd snailNumber x)) numberList
    |> List.max

//Entry point
let main projectDir =

    let sourceData = getText projectDir

    let part1Sum = List.fold (fun s x -> snailAdd s x) sourceData.Head sourceData.Tail
    let maxSum = List.map (fun x -> addToAll x sourceData) sourceData
                 |> List.max

    Console.WriteLine("Part 1: " + (magnitude part1Sum).ToString())
    Console.WriteLine("Part 2: " + maxSum.ToString()  )
    18