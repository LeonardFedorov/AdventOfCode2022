[<AutoOpen>]
module Day7

open System
open System.IO

type folderObject =
    | File of int * string // size and name
    | Folder of int * string * folderObject[] // size, name, sub-object

    member this.Size =
        match this with
            | File(size, _) -> size
            | Folder(size, _, _) -> size


//Entry point
let main projectDir =

    //Common
    let sourceData =
        let fileStream = new StreamReader(projectDir + "\\Day7Input.txt")
        fileStream.ReadToEnd().Split("$ ", StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun commandBlock -> commandBlock.Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
        //The array now has entry for each command, which consists of an array whose first element is the command
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
                                        let slash = Array.findIndexBack (fun char -> char = '/') (path.ToCharArray())
                                        path.[0..(slash-1)]
                                    else
                                        path + "/" + commandText.[3..]
                              else path

                let updatedList = if commandText = "ls" then
                                      (path, thisCommand.[1..]) :: lsResults
                                  else
                                      lsResults

                directoryIter (command + 1) newPath updatedList

        // / behaves a bit different so extract this one manually
        directoryIter 2 "" [("/", sourceData.[1].[1..])]
        |> List.rev

    let rec setupObject path (objectText: string) : folderObject =            
         
         let setupFile (fileInfo: string[]) =
            File(Int32.Parse(fileInfo.[0]), fileInfo.[1])
            
         let setupFolder (splitString: string[]) =
            
            let newPath = path + "/" + splitString.[1]
            let folderInfo = snd (List.find (fun (itemPath, list) -> itemPath = newPath) directoryList)

            let subObjectList = Array.init folderInfo.Length (fun i -> setupObject newPath folderInfo.[i])

            let size = Array.fold (fun total (item: folderObject) -> total + item.Size) 0 subObjectList

            Folder(size, splitString.[1], subObjectList)

         let splitString = objectText.Split(' ')

         if splitString.[0] = "dir" then
            setupFolder splitString
         else
            setupFile splitString
            

    let directoryParse = 

    //Part 2



    //Output


    Console.WriteLine("Part 1: Not Implemented."  )
    Console.WriteLine("Part 2: Not Implemented."  )
    7