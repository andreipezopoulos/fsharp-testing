namespace ServerHealth

module APIv1 =

    type HealthResponseV1 = {
        status: string
    }

    let modelToApi (m : HealthResponse) : HealthResponseV1 = {
        status =
            match m.status with
            |    HealthStatus.OK -> "OK"
            |    HealthStatus.NOK -> "NOK"
    }
