open System
open System.Diagnostics
open System.Threading.Tasks

// This detecting OS piece came from http://www.fssnip.net/7OP/title/Detect-operating-system
type OS =
        | OSX
        | Windows
        | Linux

let getOS =
        match int Environment.OSVersion.Platform with
        | 4 | 128 -> Linux
        | 6       -> OSX
        | _       -> Windows

// Thank you alexandru
// https://github.com/alexandru/alexn.org/blob/master/_snippets/2020-12-06-execute-shell-command-in-fsharp.md
type CommandResult = {
  ExitCode: int;
  StandardOutput: string;
  StandardError: string
}

let executeCommand executable args =
  async {
    let startInfo = ProcessStartInfo()
    startInfo.FileName <- executable
    for a in args do
      startInfo.ArgumentList.Add(a)
    startInfo.RedirectStandardOutput <- true
    startInfo.RedirectStandardError <- true
    startInfo.UseShellExecute <- false
    startInfo.CreateNoWindow <- true
    use p = new Process()
    p.StartInfo <- startInfo
    p.Start() |> ignore

    let outTask = Task.WhenAll([|
      p.StandardOutput.ReadToEndAsync();
      p.StandardError.ReadToEndAsync()
    |])

    do! p.WaitForExitAsync() |> Async.AwaitTask
    let! out = outTask |> Async.AwaitTask
    return {
      ExitCode = p.ExitCode;
      StandardOutput = out.[0];
      StandardError = out.[1]
    }
  }

let executeShellCommand command =
    printfn "> %s" command
    match getOS with
    | OS.Windows -> executeCommand "cmd.exe" [command] // I haven't tested, I hope that works =)
    | OS.Linux | OS.OSX -> executeCommand "/usr/bin/env" [ "-S"; "bash"; "-c"; command ]

let execute command =
    let r = executeShellCommand command |> Async.RunSynchronously
    if r.ExitCode = 0 then
        printfn "%s" r.StandardOutput
    else
        eprintfn "%s" r.StandardError
        Environment.Exit(r.ExitCode)
