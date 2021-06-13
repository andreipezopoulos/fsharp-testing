namespace Score

module Extractor =

    let private rnd = System.Random()

    let extract (cpf: CPF) : Result<int, exn> =
        let score = 1 + (abs (rnd.Next ())) % 1000
        Ok score
