module Config

open BCrypt.Net

type Config = {
    connectionString : string
}

type EnvVar = {
    default_: string option
    name: string
    example: unit -> string
}

type ConfigVar = SALT=0 | CONNECTION_STRING=1 | URL=2

let private configVarConfiguration x =
    match x with
    | ConfigVar.SALT ->
        {
            default_ = None
            name = "CPF_SCORE_SALT"
            example = (fun () ->
                let exampleSalt = BCrypt.Net.BCrypt.GenerateSalt ()
                sprintf "Here's an example of a salt: [%s]" exampleSalt
            )
        }
    | ConfigVar.CONNECTION_STRING ->
        {
            default_ = None
            name = "CPF_SCORE_CONNECTION_STRING"
            example = (fun () -> "Host=localhost; Database=test; Username=username; Password=password; (for more information: https://zaid-ajaj.github.io/Npgsql.FSharp/#/usecases/connection-string)")
        }
    | ConfigVar.URL ->
        {
            default_ = Some "http://0.0.0.0:8085"
            name = "CPF_SCORE_URL"
            example = (fun () -> "http://0.0.0.0:8085")
        }
    | _ ->
        raise (System.Exception "You forgot to configure some enum value from ConfigValue")

let private enumToList<'a> = (System.Enum.GetValues(typeof<'a>) :?> ('a [])) |> Array.toList

let private getSingleVarFromCfg cfg =
    let v = System.Environment.GetEnvironmentVariable cfg.name
    if v = null then
        match cfg.default_ with
        |   Some x ->
            // TODO Log properly
            printfn "Warning: default value for enviroment variable %s is being used (%s)" cfg.name x
            Some x
        |   None -> None
    else
        Some v
    
let private getAllEnviromentVar : Map<ConfigVar, string option> =
    let values = enumToList<ConfigVar>
    let f = fun acc x -> Map.add x (getSingleVarFromCfg (configVarConfiguration x)) acc
    List.fold f Map.empty values

let private allEnviromentVar = lazy(getAllEnviromentVar)

let getEnviromentVar n =
    let var = Map.find n (allEnviromentVar.Force ())
    match var with
    |   Some x -> x
    |   None ->
        let cfg = configVarConfiguration n
        let msg = sprintf "Variable %s should be set. Example value: %s" (cfg.name) (cfg.example ())
        raise <| System.Exception msg

let checkAllEnviromentVar =
    enumToList<ConfigVar> |> List.map (fun x -> getEnviromentVar x) |> ignore
