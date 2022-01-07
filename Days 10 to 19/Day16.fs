[<AutoOpen>]
module Day16

open System
open System.IO

type valuePacket = {version: int64; value: int64}
type opPacket = {version: int64; opType: int64; operands: packet list}

and packet =
    | Value of valuePacket
    | Operator of opPacket

//Input data parsing
let getText projectDir =
    let fileStream = new StreamReader(projectDir + "\\Day16Input.txt")
    fileStream.ReadToEnd()

let mapDigit (x: char) =
    match x with
        | '0' -> "0000"
        | '1' -> "0001"
        | '2' -> "0010"
        | '3' -> "0011"
        | '4' -> "0100"
        | '5' -> "0101"
        | '6' -> "0110"
        | '7' -> "0111"
        | '8' -> "1000"
        | '9' -> "1001"
        | 'A' -> "1010"
        | 'B' -> "1011"
        | 'C' -> "1100"
        | 'D' -> "1101"
        | 'E' -> "1110"
        | 'F' -> "1111"
        | _ -> failwith "Illegal character in input string"

let hexToBin (hexInput: string) =
    let binaryString = Array.map (fun x -> mapDigit x) (hexInput.ToCharArray())
    String.Join("", binaryString)

let binToDec (digits: string) =
    Array.fold (fun s x -> 2L * s + (int64 x - int64 '0')) 0L (digits.ToCharArray())

//Packet parsing

//Value Packets
let rec getPacketValueIter (inputString: string) =
    
    if inputString.[0] = '0' then
        (inputString.[1..4], inputString.[5..])
    else
        let (binaryString, residualString) = getPacketValueIter inputString.[5..]
        (inputString.[1..4] + binaryString, residualString)

let getPacketValue (inputString: string) =
    let (binaryString, residualString) = getPacketValueIter inputString
    (binToDec binaryString, residualString)             

//Operator Packets
let rec getNextnPackets (inputString: string) count packetList =
    if count = 0L then
        //Reverse the list since it's been built backwards
        (List.rev packetList, inputString)
    else
        let (nextPacket, residualString) = parsePacket inputString
        getNextnPackets residualString (count - 1L) (nextPacket :: packetList)

and getLengthOfPackets (inputString: string) charsLeft packetList =
    if charsLeft < 0L then
        failwith "Unexpected packet parse by length"
    elif charsLeft = 0L then
        //Reverse the list since it's been built backwards
        (List.rev packetList, inputString)
    else
        let (nextPacket, residualString) = parsePacket inputString
        getLengthOfPackets residualString (charsLeft - int64 (inputString.Length - residualString.Length)) (nextPacket :: packetList)        
 
//Extract the first complete packet from the string and return the rest of the string thereafter for further analysis
and parsePacket (inputString: string) : packet * string =
    
    let pVersion = binToDec inputString.[0..2]
    let pType = binToDec inputString.[3..5]

    if pType = 4L then
        let (pValue, residualString) = getPacketValue inputString.[6..]
        (Value({version = pVersion; value = pValue}), residualString)
    else
        let lengthType = inputString.[6]
        let (pOperands, residualString) = if lengthType = '1' then
                                              let packetCount = binToDec inputString.[7..17]
                                              getNextnPackets inputString.[18..] packetCount List.Empty
                                          else
                                              let packetCharCount = binToDec inputString.[7..21] 
                                              getLengthOfPackets inputString.[22..] packetCharCount List.Empty

        (Operator({version = pVersion; opType = pType; operands = pOperands}), residualString)

//Part 1
let rec versionSum (targetPacket: packet) =
    match targetPacket with
        | Value({version = pVersion}) -> pVersion
        | Operator({version = pVersion; operands = operandList}) -> pVersion + List.fold (fun s x -> s + versionSum x) 0L operandList

//Part 2
let rec evaluatePacket (targetPacket: packet) =
    match targetPacket with
        | Value({value = pValue}) -> pValue
        | Operator({opType = pType; operands = operandList}) -> 
            let evaluatedList = List.map (fun x -> evaluatePacket x) operandList
            match pType with
                | 0L -> List.sum evaluatedList
                | 1L -> List.fold (fun s x -> s * x) 1L evaluatedList
                | 2L -> List.min evaluatedList
                | 3L -> List.max evaluatedList
                | 5L -> if evaluatedList.Head > evaluatedList.Tail.Head then 1L else 0L
                | 6L -> if evaluatedList.Head < evaluatedList.Tail.Head then 1L else 0L
                | 7L -> if evaluatedList.Head = evaluatedList.Tail.Head then 1L else 0L
                | _ -> failwith "Unknown operator type"

//Entry point
let main projectDir =

    let sourceHex = getText projectDir
    let sourceBin = hexToBin sourceHex
  
    let (parsedPackets, residualString) = parsePacket sourceBin
    if residualString.Contains('1') then failwith "Incomplete parse"

    Console.WriteLine("Part 1: " + (versionSum parsedPackets).ToString())
    Console.WriteLine("Part 2: " + (evaluatePacket parsedPackets).ToString())
    16