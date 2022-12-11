open System

let list = [1;2;3]

let pair = (1,2)

let getTarget (line:string) = Int32.Parse(line.[line.LastIndexOf(' ') + 1..])

getTarget "    If false: throw to monkey 56"