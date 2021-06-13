namespace Score

open System.Threading.Tasks
open FSharp.Control.Tasks
open FSharp.Core
open Crypto
open Npgsql.FSharp
open Util.Npgsql

module Database =

    let private formatCpfToPersistedCpf cpf = cpf |> string |> hashCpf
    let private notConnectedException = System.Exception "Database is not working properly"

    let isConnected connectionString : Task<Result<unit, exn>> =
        task {
            let queryExecution =
                connectionString
                |> Sql.connect
                |> Sql.query "SELECT 1 as one"
                |> Sql.executeRowAsync (fun read -> read.int "one")

            let! one = extractAsyncQuerySingleResultSafe queryExecution
            let result =
                one
                |> Result.bind (fun x -> Util.Option.toResult x notConnectedException)
                |> Result.map (fun x -> x = 1)
                |> Result.bind (fun x ->
                    if x then Ok ()
                    else Error notConnectedException
                )

            return result
        }

    let getByCpf connectionString (cpf : CPF) : Task<Result<ScoreData option, exn>> =
        task {
            let queryExecution =
                connectionString
                |> Sql.connect
                |> Sql.query "SELECT cpf, score, created_at FROM score WHERE cpf=@cpf ORDER BY created_at DESC LIMIT 1"
                |> Sql.parameters ["cpf", Sql.string (formatCpfToPersistedCpf cpf)]
                |> Sql.executeRowAsync (fun read -> {
                    cpf = cpf
                    score = read.int "score"
                    createdAt = read.datetimeOffset "created_at"
                })

            return! extractAsyncQuerySingleResultSafe queryExecution
        }

    let insertOrUpdate connectionString (scoreData : ScoreData) : Task<Result<int, exn>> =
        task {
            let queryExecution =
                connectionString
                |> Sql.connect
                |> Sql.query "INSERT INTO score(cpf, score, created_at) VALUES(@cpf, @score, @created_at)"
                |> Sql.parameters [
                    "cpf", Sql.string (formatCpfToPersistedCpf scoreData.cpf)
                    "score", Sql.int scoreData.score
                    "created_at", Sql.timestamptz scoreData.createdAt
                ]
                |> Sql.executeNonQueryAsync

            return! extractAsyncQueryResultSafe queryExecution
        }
