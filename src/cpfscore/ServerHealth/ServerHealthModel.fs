namespace ServerHealth

open Giraffe.Core
open Saturn
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks

type HealthStatus = OK | NOK

type HealthResponse = {
    status: HealthStatus
}
