namespace Util

module Option =
    let toResult x e =
        match x with
        |    Some y -> Ok y
        |    None -> Error e
