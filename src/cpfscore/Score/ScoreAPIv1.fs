namespace Score

module APIv1 =

    type GetScoreInput = {
        cpf: string
    }

    type ScoreDataV1 = {
        cpf: string
        score: int
        createdAt: string
    }

    let modelToApi (d : ScoreData) : ScoreDataV1 = {
        cpf = CPF.toString d.cpf
        score = d.score
        createdAt = d.createdAt.ToString("u")
    }
