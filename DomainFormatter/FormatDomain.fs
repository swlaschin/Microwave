module FormatDomain

open System.IO
open FSharp.Literate
open FSharp.Formatting.Razor

// ----------------------------------------------------------------------------
// SETUP
// ----------------------------------------------------------------------------

let ( ++ ) parent file = Path.Combine(parent,file)

/// Return path relative to the current file location
let relative subdir = Directory.GetCurrentDirectory() ++ subdir

let ensureOutputDir outputDir =
    // Create output directories & copy content files there
    // (We have two sets of samples in "output" and "output-all" directories,
    //  for simplicitly, this just creates them & copies content there)
    if not (Directory.Exists(relative outputDir)) then
      Directory.CreateDirectory(relative outputDir) |> ignore


/// Processes a single F# file and produce HTML output
let setup outputDir (filename:string) =
  ensureOutputDir outputDir
  let fileWithoutExt = Path.GetFileNameWithoutExtension(filename)
  let inputFile = relative filename
  let outputFile = outputDir ++ sprintf "%s.html" fileWithoutExt
  let output = relative outputFile
  inputFile,output


/// Processes a single F# file and produce HTML output
let processFsAsHtml outputDir filename =
  let inputFile,output = setup outputDir filename
  let template = relative "template-file.html"
  RazorLiterate.ProcessScriptFile(inputFile, template, output,lineNumbers=false)

/// Processes a single Markdown document and produce HTML output
let processMdAsHtml outputDir filename  =
  let inputFile,output = setup outputDir filename
  let template = relative "template-file.html"
  RazorLiterate.ProcessMarkdown(inputFile, template, output,lineNumbers=false)

let isDomainFile (filename:string) =
    filename.ToLower().Contains("domain")

let processDirectory outputDir inputDir =
    printfn "Formatting directory %s. Outputting to: %s" inputDir outputDir
    for filename in Directory.EnumerateFiles(inputDir) do
        printf "... %s" filename
        match Path.GetExtension(filename) with
        | ".md" ->
            processMdAsHtml outputDir filename
            printfn "...Done"
        | ".fsi" ->
            processFsAsHtml outputDir filename
            printfn "...Done"
        | ".fs" ->
            let sigFile = filename.Replace(".fs",".fsi")
            if File.Exists(sigFile) then
                printfn "...Skipped"
            else if isDomainFile filename then
                processFsAsHtml outputDir filename
                printfn "...Done"
            else
                printfn "...Skipped"
        | _ ->
            printfn "...Skipped"


