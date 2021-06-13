module Router

open Saturn
open Giraffe.Core

let appRouter = router {
    forward "/health" ServerHealth.Controller.resource
    forward "/score" Score.Controller.resource
}
