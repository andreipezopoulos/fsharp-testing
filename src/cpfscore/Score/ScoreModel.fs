namespace Score

type CPF = uint64

module CPF =

    let private invalidFormat = System.Exception "Invalid cpf format"

    let fromString (cpf : string) =
        try
            let parsedCpf = cpf.Replace(".", "").Replace("-", "").Replace("/", "")
            if (String.length parsedCpf) <> 11 then
                raise invalidFormat
            else
                parsedCpf |> uint64
        with
        | _ -> raise invalidFormat

    let toString (cpf : CPF) =
        sprintf "%03u%03u%03u%02u" (cpf / 100000000UL) (cpf / 100000UL % 1000UL) (cpf / 100UL % 1000UL) (cpf % 100UL)

type ScoreData = {
   cpf: CPF
   score: int
   createdAt: System.DateTimeOffset
}
