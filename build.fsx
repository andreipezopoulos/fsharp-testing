#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open System.Threading

let appPath = "./src/cpfscore/" |> Path.getFullName
let projectPath = Path.combine appPath "cpfscore.fsproj"


Target.create "Clean" ignore

Target.create "Restore" (fun _ ->
    DotNet.restore id projectPath
)

Target.create "Build" (fun _ ->
    DotNet.build id projectPath
)

Target.create "Run" (fun _ ->
  System.Environment.SetEnvironmentVariable ("CPF_SCORE_SALT", "$2a$11$4MU2UTxKwJxSC2fcnz.jwO")

  let server = async {
    DotNet.exec (fun p -> { p with WorkingDirectory = appPath } ) "watch" "run" |> ignore
  }
  let browser = async {
    Thread.Sleep 5000
    Process.start (fun i -> { i with FileName = "http://localhost:8085" }) |> ignore
  }

  [ server; browser]
  |> Async.Parallel
  |> Async.RunSynchronously
  |> ignore
)

"Clean"
  ==> "Restore"
  ==> "Build"

"Clean"
  ==> "Restore"
  ==> "Run"

Target.runOrDefault "Build"
