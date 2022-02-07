module Lib.Validations

type validation<'a> = 'a -> bool * string

let requiredString input =
    let isValid = input <> ""
    (isValid, "This field is required")

let minLength length (input: string) =
    let isValid = input.Length >= length
    (isValid, sprintf "This field should contain at least %d characters" length)
