[<AutoOpen>]
module Day11

open System
open System.IO

type mProps =
    | number = 0
    | transF = 1
    | cond = 2
    | tTarget = 3
    | fTarget = 4
    | initalItems = 5

//Entry point
let main projectDir =

    //Utility
    let divisibilityTest n input = 
        (input % n) = 0

    let integerDivide a b =
        (a - (a % b))/b

    let add a b = a + b
    let mult a b = a * b

    let singleApply func a = func a
    let doubleApply func a = func a a

    let boredom = 3

    let parseOps (line: string[]) =
        let relevantPart = line.[line.Length - 3..]
        let usedFunction = match relevantPart.[1] with
                              | "+" -> add
                              | "*" -> mult
                              | _ -> failwith "Unknown operator"

        let isDouble = relevantPart.[2] = "old"
        
        if isDouble then
            doubleApply usedFunction
        else
            singleApply (usedFunction (Int32.Parse(relevantPart.[2]) ) )

    //Data Import
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day11Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n\r\n"|], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun line -> line.Split("\r\n"))


    //Part 1
    let monkeyInfo =
        Array.init sourceData.Length (fun i -> 
            let monkeyData = sourceData.[i]
            let initialItems = monkeyData.[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
            let itemStack = List.init (initialItems.Length - 2) (fun j -> Int32.Parse(initialItems.[initialItems.Length - j - 1].Replace(',',' ')))
            
            let getLastNumber (line:string) = Int32.Parse(line.[line.LastIndexOf(' ') + 1..])

            let opLine = monkeyData.[2].Split(' ', StringSplitOptions.RemoveEmptyEntries) 

            //Monkey number, transition function, condition, trueTarget, falseTarget, initialItems
            (i, 
             parseOps opLine,
             divisibilityTest (getLastNumber (monkeyData.[3])), 
             getLastNumber (monkeyData.[4]), 
             getLastNumber (monkeyData.[5]), 
             itemStack
            )
        )

    let simulateMonkeysP1 steps =

        let initialItem (_,_,_,_,_, itemList) = itemList
        let mutable itemStates = Array.init monkeyInfo.Length (fun i -> initialItem (monkeyInfo.[i]))
        let mutable inspectionCounts = Array.create monkeyInfo.Length 0

        let rec simulateMonkeysIter steps =
            
            if steps = 0 then
                (itemStates, inspectionCounts)
            else
                
                let rec monkeyIter monkeyIndex = 
                    
                    if monkeyIndex = monkeyInfo.Length then
                        ()
                    else
                        let (number, transition, condition, trueT, falseT, initialItems) = monkeyInfo.[monkeyIndex]
                        
                        let rec itemIter itemList =

                            if List.isEmpty itemList then
                                ()
                            else
                                let item = List.head itemList
                                let newItem = integerDivide (transition item) boredom        

                                let targetMonkey = if condition newItem then trueT else falseT
                                itemStates.[targetMonkey] <- newItem :: itemStates.[targetMonkey]
                                inspectionCounts.[monkeyIndex] <- inspectionCounts.[monkeyIndex] + 1

                                itemIter (List.tail itemList)
                    
                        let currentItems = itemStates.[monkeyIndex]
                        itemStates.[monkeyIndex] <- List.empty<int>
                        itemIter (List.rev currentItems)
                        
                        monkeyIter (monkeyIndex + 1)
                        
                monkeyIter 0

                simulateMonkeysIter (steps - 1)
        

        simulateMonkeysIter steps

    let part1Result = snd (simulateMonkeysP1 20)
                      |> Array.sortDescending

    let part1Answer = part1Result.[0] * part1Result.[1]

    //Part 2







    //Output

    Console.WriteLine("Part 1: " + part1Answer.ToString() )
    Console.WriteLine("Part 2: Not Implemented."  )
    11