namespace Lib.Hooks

open Browser.Types
open Feliz
open Lib.Validations

type Field<'a> =
    {
        ref: IRefValue<HTMLInputElement option>
        value: 'a
        setValue: 'a -> unit
        focus: unit -> unit
        blur: unit -> unit
        errors: string list
        validate: unit -> unit
        clearErrors: unit -> unit
        isValid: unit -> bool
        reset: unit -> unit
    }

type Hooks =

    [<Hook>]
    static member useField(initialValue: 'a, ?validations: validation<'a> list) : Field<'a> =
        let validations = Option.defaultValue [] validations

        let value, setValue = React.useState initialValue
        let errors, setErrors = React.useState<string list> []

        let ref = React.useInputRef ()

        let focus () =
            ref.current
            |> Option.iter (fun element -> element.focus ())

        let blur () =
            ref.current
            |> Option.iter (fun element -> element.blur ())

        let clearErrors () = setErrors []

        let getErrors input =
            validations
            |> List.fold
                (fun errors validation ->
                    match validation input with
                    | true, _ -> errors
                    | false, errorMessage -> errorMessage :: errors)
                []
            |> List.rev

        let isValid () = getErrors value |> List.isEmpty

        let validate () = getErrors value |> setErrors

        let reset () = setValue initialValue

        {
            ref = ref
            value = value
            setValue = setValue
            errors = errors
            validate = validate
            clearErrors = clearErrors
            focus = focus
            blur = blur
            isValid = isValid
            reset = reset
        }
