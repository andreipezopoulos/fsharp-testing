module Crypto

open BCrypt.Net
open Config

let hashCpf cpf =
    let salt = getEnviromentVar ConfigVar.SALT
    BCrypt.Net.BCrypt.HashPassword (cpf, salt)
