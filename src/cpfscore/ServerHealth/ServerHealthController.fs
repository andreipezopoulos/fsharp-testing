namespace ServerHealth

open Giraffe.Core
open Saturn
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks

module Controller =
    let healthCheck (ctx : HttpContext) =
        task {
            let cnf : Config.Config = Controller.getConfig ctx
            let! connected = Score.Database.isConnected cnf.connectionString
            return connected |> Result.map (fun _ -> { status = HealthStatus.OK })
        }

    let resource = controller {
        index (Util.API.Endpoint healthCheck APIv1.modelToApi)
    }
