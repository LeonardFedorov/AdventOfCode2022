[<AutoOpen>]
module Day21

open System
open System.IO

type Monkey =
    | Number of name: string * value : int64
    | Compound of name: string * leftOperand: string * rightOperand: string * operation: (int64 -> int64 -> int64) * opIcon: string

    member this.MonkeyName =
        match this with
            | Number (name, _) -> name
            | Compound (name, _, _, _, _) -> name

    member this.HasOperand(monkey: Monkey) =
        match this with
            | Number (_,_) -> false
            | Compound (_, lOp, rOp, _, _) -> lOp = monkey.MonkeyName || rOp = monkey.MonkeyName

//Entry point
let main projectDir =

    //Common
    //Create standard functions to wrap infix operators
    let add a b = a + b
    let mul a b = a * b
    let sub a b = a - b
    let div a b = a / b

    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day21Input.txt")
        fileStream.ReadToEnd().Split([|"\r\n"|], StringSplitOptions.None)
        |> Array.map (fun line -> line.Split(' '))
        |> Array.map (fun line -> if line.Length = 2 then
                                    Monkey.Number(line.[0].[0..3],Int64.Parse(line.[1]))
                                  elif line.Length = 4 then
                                    Monkey.Compound(
                                        line.[0].[0..3],
                                        line.[1],
                                        line.[3],
                                        match line.[2] with
                                            | "+" -> add
                                            | "-" -> sub
                                            | "*" -> mul
                                            | "/" -> div
                                            | _ -> failwith "Unknown operator"
                                            ,
                                        line.[2]
                                        )
                                  else failwith "Unknown input line"
                       )

    let rec evaluateMonkey name =
        let targetMonkey = Array.find (fun (monkey: Monkey) -> name = monkey.MonkeyName) sourceData
        match targetMonkey with
            | Number(_, value) -> value
            | Compound(_, lOp, rOp, func, _) -> func (evaluateMonkey lOp) (evaluateMonkey rOp)

    //Part 1

    let part1Answer = evaluateMonkey "root"

    //Part 2

    //Ran this code to validate that every monkey only appears as an operand of at most one other monkey, which proves there are no forked dependencies leading to root.
    //This proves that the approach used for part 2 is sufficient
    //Since the value isn't used, this calculation isn't normally executed at all
    let integrityCheck =
        let calc = Array.map (fun (monkey: Monkey) -> Array.length (Array.filter (fun (otherMonkey: Monkey) -> otherMonkey.HasOperand(monkey)) sourceData)) sourceData
        (Array.max calc, Array.min calc)

    let dependencyList =
        let baseMonkey = Array.find (fun (monkey: Monkey) -> monkey.MonkeyName = "humn") sourceData
        
        let rec findNextMonkey listSoFar =
            
            let prevMonkey = List.head listSoFar
            let nextMonkey = Array.tryFind (fun (monkey: Monkey) -> monkey.HasOperand(prevMonkey)) sourceData

            if nextMonkey.IsNone then
                listSoFar
            else
                findNextMonkey (nextMonkey.Value :: listSoFar)

        findNextMonkey [baseMonkey]

    let part2Answer =

        //Evalute the value of the monkey on the other branch from the one we are progressing so we can back calculate
        //the value we need from the branch that is being pursued
        let getOtherBranch (current: Monkey) (next: Monkey) =
            let (leftOp, rightOp) = match current with
                                        | Number (_, _) -> failwith ("Expected operation monkey at branch")
                                        | Compound (_, lOp, rOp, _, _) -> (lOp, rOp)   
                                        
            if next.MonkeyName = leftOp then
                evaluateMonkey rightOp
            elif next.MonkeyName = rightOp then
                evaluateMonkey leftOp
            else
                failwith "Monkey not found"

        //Invert the function of the current monkey to work out the required value for the input given as a None
        let invert func lOp rOp target =
            match (func, lOp, rOp) with
                | ("+", Some x, None) -> target - x
                | ("+", None, Some x) -> target - x
                | ("-", Some x, None) -> x - target
                | ("-", None, Some x) -> target + x
                | ("*", Some x, None) -> target / x
                | ("*", None, Some x) -> target / x
                | ("/", Some x, None) -> x / target
                | ("/", None, Some x) -> x * target
                | (_, _, _) -> failwith "Unexpected inversion parameters"
                 
        let rec executeStep (remainingList: Monkey list) targetValue =
            
            if List.length remainingList = 1 then
                targetValue
            else
                let current = List.head remainingList
                let next = remainingList.[1].MonkeyName

                match current with
                    | Number (_, _) -> failwith "Stack monkey should be compound"
                    | Compound (_, lOp, rOp, _, op) ->
                        let newValue = 
                            if next = lOp then
                                invert op None (Some (evaluateMonkey rOp)) targetValue
                            else
                                invert op (Some (evaluateMonkey lOp)) None targetValue

                        executeStep (List.tail remainingList) newValue

        let root = List.head dependencyList
        let next = dependencyList.[1]

        executeStep (List.tail dependencyList) (getOtherBranch root next)

    //Output

    Console.WriteLine("Part 1: " + part1Answer.ToString())
    Console.WriteLine("Part 2: " + part2Answer.ToString())
    21