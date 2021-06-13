namespace Score

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks
open Config
open Saturn

module Controller =

    let extractCpf (ctx : HttpContext) (input : APIv1.GetScoreInput) =
        task {
            let cnf = Controller.getConfig ctx
            let cpf = CPF.fromString input.cpf

            let createScoreData = fun (score: int) ->
                {
                    cpf = cpf
                    score = score
                    createdAt = System.DateTimeOffset.Now
                }

            let modelOrNot = (Extractor.extract cpf) |> Result.map createScoreData

            match modelOrNot with
            |   Ok model ->
                let! modified = Database.insertOrUpdate cnf.connectionString model
                let validate = fun m ->
                    if m = 1 then Ok model
                    else Error (System.Exception "Data not written")

                return modified |> Result.bind validate
            |   Error ex ->
                return Error ex
        }

    let getCpf (ctx : HttpContext) (cpf : string) =
        task {
            let cnf = Controller.getConfig ctx
            let! result = Database.getByCpf cnf.connectionString (CPF.fromString cpf)
            return result |> Result.bind (fun x -> Util.Option.toResult x (System.Exception "Score not found"))
        }

    let resource = controller {
       create (Util.API.EndpointWithBody extractCpf APIv1.modelToApi)
       show (Util.API.EndpointOneArg getCpf APIv1.modelToApi)
    }
