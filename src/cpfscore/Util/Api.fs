namespace Util

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks
open Saturn
open Config

module API =

    let private callApi (f: _ -> System.Threading.Tasks.Task<Result<_, exn>>) m ctx =
        task {
            let! r = f ()
            match r with
            |    Ok s ->
                return! Controller.json ctx (m s)
            |    Error ex ->
                return raise ex
        }

    let EndpointOneArg<'T, 'R, 'P> (ctrlerFunc : (HttpContext) -> ('T) -> System.Threading.Tasks.Task<Result<'P, exn>>) (apiObjToObj : ('P) -> 'R) =
        fun (ctx : HttpContext) (d: 'T) -> task {
            return! callApi (fun () -> ctrlerFunc ctx d) apiObjToObj ctx
        }

    let EndpointWithBody (ctrlerFunc : (HttpContext) -> (_) -> System.Threading.Tasks.Task<Result<_, exn>>) apiObjToObj =
        fun (ctx : HttpContext) -> task {
            let! input = Controller.getModel ctx
            return! callApi (fun () -> ctrlerFunc ctx input) apiObjToObj ctx
        }

    let Endpoint (ctrlerFun : (HttpContext) -> System.Threading.Tasks.Task<Result<_, exn>>) apiObjToObj =
        fun (ctx : HttpContext) -> task {
            return! callApi (fun () -> ctrlerFun ctx) apiObjToObj ctx
        }
