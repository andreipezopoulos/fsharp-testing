module Server

open Saturn
open Config
open Crypto
open Error

let endpointPipe = pipeline {
    plug head
    plug requestId
}

let app = application {
    pipe_through endpointPipe
    error_handler ErrorHandler.error_pipe
    use_router Router.appRouter
    url (getEnviromentVar ConfigVar.URL)
    memory_cache
    use_gzip
    use_config (fun _ -> { connectionString = getEnviromentVar ConfigVar.CONNECTION_STRING } )
}

let checkSomeEssentialFunctions =
    hashCpf "111111111" |> ignore

[<EntryPoint>]
let main _ =
    checkAllEnviromentVar
    checkSomeEssentialFunctions

    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code
