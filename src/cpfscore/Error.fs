namespace Error

open Saturn

type Error500 = {
    msg: string
    source: string
    stackTrace: string
}

module ErrorHandler =

    let error_pipe (ex: exn) _ = pipeline {
        set_status_code 500
        json { msg = ex.Message; source = ex.Source; stackTrace = ex.StackTrace }
    }
