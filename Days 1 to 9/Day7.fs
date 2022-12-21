[<AutoOpen>]
module Day7

open System
open System.IO

//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day7Input.txt")
        fileStream.ReadToEnd().Split("$ ", StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun commandBlock -> commandBlock.Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
        //The array has an entry for each command, which consists of an array whose first element is the command
        //and each successive line is the list of results

    //Part 1
    let directoryList =
        
        let rec directoryIter command (path: string) lsResults =
            
            if command = sourceData.Length then
                lsResults
            else
                let thisCommand = sourceData.[command]
                let commandText = thisCommand.[0]
                
                let newPath = if commandText.[0..1] = "cd" then
                                    if commandText.[3..4] = ".." then
                                        let slash = Array.findIndexBack (fun char -> char = '/') (path.ToCharArray().[0..path.Length-2])
                                        path.[0..(slash)]
                                    else
                                        path + commandText.[3..] + "/"
                              else path

                let updatedList = if commandText = "ls" then
                                      (path, thisCommand.[1..]) :: lsResults
                                  else
                                      lsResults

                directoryIter (command + 1) newPath updatedList

        // / behaves a bit different so extract this one manually
        directoryIter 2 "/" [("/", sourceData.[1].[1..])]
        |> List.rev
        |> Array.ofList

    let rec evaluateFolder (path: string) =
        
        let folderData = Array.find (fun folderInfo -> fst folderInfo = path) directoryList
        Array.map (fun folderObject -> evaluteObject path folderObject) (snd folderData)
        |> Array.sum

    and evaluteObject (path: string) (folderObject: string) =
        let splitObject = folderObject.Split(' ')

        if splitObject.[0] = "dir" then
            evaluateFolder (path + splitObject.[1] + "/")
        else
            Int32.Parse(splitObject.[0])

    let folderSizes = Array.map (fun folderInfo -> evaluateFolder (fst folderInfo)) directoryList
    
    let part1Answer = Array.filter (fun size -> size <= 100000) folderSizes
                      |> Array.sum

    //Part 2

    let freeSpace = 70000000 - (Array.max folderSizes)
    let requiredSpace = 30000000 - freeSpace

    let part2Answer = Array.filter (fun size -> size >= requiredSpace) folderSizes
                      |> Array.min

    //Output


    Console.WriteLine("Part 1: " + part1Answer.ToString())
    Console.WriteLine("Part 2: " + part2Answer.ToString())
    7