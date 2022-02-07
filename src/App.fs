[<AutoOpen>]
module App

open Feliz
open Feliz.MaterialUI

open Lib.Hooks
open Lib.UI
open Lib.Validations

// In case of wanting to turn this component into a more generic one
// it should be refactored into a component t'hat accept a list as props
// and lives inside Lib.UI
[<ReactComponent>]
let TextField
    (props: {| label: string
               field: Field<string> |})
    =
    Mui.textField [
        prop.style [ style.minHeight 80 ]
        textField.label props.label
        textField.variant.outlined
        textField.inputRef props.field.ref
        textField.value props.field.value
        textField.onChange props.field.setValue
        // prop.onFocus (ignore >> props.clearErrors)
        prop.onBlur (ignore >> props.field.validate)

        match props.field.errors with
        | [] -> ()
        | errorMessage :: _ ->
            textField.error true
            textField.helperText errorMessage
    ]


[<ReactComponent>]
let App () =
    let nameField = Hooks.useField ("", [ requiredString; minLength 10 ])
    let nicknameField = Hooks.useField ("", [ requiredString; minLength 3 ])

    Html.div [

        prop.style [
            style.display.flex
            style.flexDirection.column
            // style.maxWidth 400
            style.padding 10
        ]

        prop.children [

            UI.form [
                form.fields [ nameField; nicknameField ]
                form.onSubmit (fun _ -> Browser.Dom.window.alert nameField.value)
                form.children [

                    TextField {| label = "Name"; field = nameField |}
                    TextField
                        {|
                            label = "Nickname"
                            field = nicknameField
                        |}

                    Html.br []
                    Html.br []

                    Mui.button [
                        button.disableRipple true
                        prop.text "Submit"
                        prop.type'.submit
                        button.color.primary
                        button.variant.contained
                    ]
                ]
            ]
        ]
    ]
