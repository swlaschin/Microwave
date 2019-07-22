module Program

open System

[<EntryPoint>]
let main argv =

    match argv with
    | [| |] ->
        printfn "Pass a source directory as parameter"
    | [| inputDir |] ->
        // assume that input is /src/MyProject
        // make output is /doc
        let outputDir = IO.Path.Combine(inputDir,"..","doc")
        FormatDomain.processDirectory outputDir inputDir
    | _ ->
        printfn "Pass a single source directory as parameter"

    0 // return an integer exit code


