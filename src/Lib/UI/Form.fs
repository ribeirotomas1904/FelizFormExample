namespace Lib.UI

open Fable.Core.JsInterop
open Feliz
open Lib.Hooks

type FormProperty =
    interface
    end

type FormProps<'a, 'b> =
    abstract fields: Field<'a, 'b> list option
    abstract onSubmit: (Browser.Types.Event -> unit) option
    abstract children: list<ReactElement> option

// That should be abstracted if possible in Lib.UI.Core for avoid repetition in each new UI component
[<AutoOpen>]
module private Core =
    let mkAttr (key: string) (value: 'a) : FormProperty = unbox (key, value)
    let unboxProperties (properties: FormProperty list) : FormProps<'a, 'b> = !!properties |> createObj |> unbox

type form =
    static member fields(xs: Field<'a, 'b> list) : FormProperty = mkAttr "fields" xs
    static member onSubmit(fn: Browser.Types.Event -> unit) : FormProperty = mkAttr "onSubmit" fn
    static member children(xs: ReactElement list) : FormProperty = mkAttr "children" xs

type UI =

    [<ReactComponent>]
    static member form(properties: FormProperty list) : ReactElement =

        let form (props: FormProps<'a, 'b>) =
            let fields = Option.defaultValue [] props.fields
            let onSubmit = Option.defaultValue ignore props.onSubmit
            let children = Option.defaultValue [] props.children

            Html.form [
                prop.onSubmit (fun e ->
                    e.preventDefault ()

                    fields
                    |> List.tryFind (fun x -> x.isValid () |> not)
                    |> function
                        | None ->
                            onSubmit e

                            fields
                            |> List.iter (fun x ->
                                x.blur ()
                                x.reset ()

                            )

                        | Some field ->
                            fields |> List.iter (fun x -> x.validate ())
                            field.focus ()

                )

                prop.children children
            ]

        form (unboxProperties properties)
