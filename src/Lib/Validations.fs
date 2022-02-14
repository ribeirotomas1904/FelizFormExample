module Lib.Validations

type Validation<'a> =
    | NonValid of 'a
    | Valid

type validationFunc<'a> = 'a -> Validation<'a>

type parseFunc<'a, 'b> = 'a -> option<'b>

let requiredString input =
    if input <> "" then Valid else NonValid "É obrigatório o preenchimento deste campo"

let minLength length (input: string) =
    if input.Length >= length then Valid else NonValid (sprintf "This field should contain at least %d characters" length)
