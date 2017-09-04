
// Spike DSL on CLR for Market Extract 
// Given a declarative statement of extract,
// produce one or more SQL Statements

open System
open System.Collections.Generic
open FSharp.Collections

type ExColumn() =
    [<DefaultValue>] val mutable Name : string
    [<DefaultValue>] val mutable ExColumnName : string
    [<DefaultValue>] val mutable DbObjectName : string

type ExTable() = 
    [<DefaultValue>] val mutable TableId : string
    let mutable exCols = new Dictionary<string, ExColumn>()
    member this.AddCol (col:ExColumn) =
                let key = col.ExColumnName
                exCols.Add(key, col)

type ExModel() = 
    [<DefaultValue>] val mutable ModelId : string
    let mutable exColumnList : ExColumn list = []
    member this.ExColumnList with get() = exColumnList
  
    let mutable ExTableDict = new Dictionary<string, ExTable>()
    member this.AddCol (col:ExColumn) =
            exColumnList <- col :: exColumnList
            let xtab =
                if ExTableDict.ContainsKey(col.DbObjectName) = true then
                    //printfn "HasTable %s" col.DbObjectName
                    ExTableDict.[col.DbObjectName]
                else
                    //printfn "DoesNotHaveTable %s" col.DbObjectName
                    let tab = new ExTable()
                    tab.TableId <- col.DbObjectName
                    ExTableDict.Add( col.DbObjectName, tab)
                    tab 
            xtab.AddCol col

let dsl_extract = new ExModel()

let save name = 
    dsl_extract.ModelId <- name

let rec show_cols (colList:ExColumn list) =
    match colList with
        | head :: tail -> show_cols tail; printfn "%s" head.ExColumnName
        | [] -> printfn ":"


let show label = 
    printfn "%s" dsl_extract.ModelId
    show_cols dsl_extract.ExColumnList



let source = 1 

let variable nameStr sourceKey tabStr = 
    let me = new ExColumn()
    me.Name <- nameStr
    me.ExColumnName <- nameStr
    me.DbObjectName <- tabStr
    dsl_extract.AddCol me
    ()


[<EntryPoint>]
let main argv =
    //EXAMPLE DSL

    variable "var1.1" source "pardat_sale"
    variable "var1.2" source "pardat_sale"

    variable "var2.1" source "land_sale"
    variable "var2.2" source "land_sale"

    variable "var3.1" source "dweldat_sale"
    variable "var3.2" source "dweldat_sale"


    save "myextract"

    show "label"
    
    0 // return an integer exit code
