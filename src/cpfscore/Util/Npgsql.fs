namespace Util

open Npgsql.FSharp
open FSharp.Control.Tasks
open System.Threading.Tasks

module Npgsql =

    let extractAsyncQueryResultSafe (x : Task<_>) : Task<Result<_, exn>> = task {
        try
            let! y = x
            return Ok y
        with ex ->
            return Error ex
    }

    let extractAsyncQuerySingleResultSafe (x : Task<_>) : Task<Result<_ option, exn>> = task {
        try
            let! y = x
            return Ok (Some y)
        with ex ->
            if (ex :? NoResultsException) then return Ok None
            else return Error ex
    }
