#load "common.fsx"
open Common

let path = "src/docker"
execute (sprintf "docker build -f %s/Dockerfile . --tag cpfscore" path)
execute (sprintf "docker-compose --project-directory . -f %s/docker-compose.yaml up -d" path)
printfn "Don't forget to execute 'docker-compose --project-directory . -f %s/docker-compose.yaml down' when you are done testing" path
