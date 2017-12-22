
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

let dsl_extract = ExModel()

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
    let me = ExColumn()
    me.Name <- nameStr
    me.ExColumnName <- nameStr
    me.DbObjectName <- tabStr
    dsl_extract.AddCol me
    ()


//CAMA TREE CONFIG
type NodeLabel() =
    [<DefaultValue>] val mutable ConfigId : string
    [<DefaultValue>] val mutable Label : string
    [<DefaultValue>] val mutable Selectable : string
    [<DefaultValue>] val mutable KeyList : string list
    [<DefaultValue>] val mutable CondList : unit list
    
 

//let nodelist = System.Collections.Generic.List<NodeLabel>()
let mutable nodelist : NodeLabel list = []

let node_label uid labelTok label dbObjTok dbObj keyTok keyList condTok condList = 
    let node = NodeLabel()
    node.ConfigId <- uid
    node.Label <- label
    node.Selectable <- dbObj
    node.KeyList <- keyList
    node.CondList <- condList
    nodelist <- node :: nodelist
    ()
let label = 100
let selectable = 101
let selectable_key = 102
let condition = 103

let eq a b = ()

let node_label_attr a b c d e = ()

let node_rel v1 v2 v3 v4 v5 v6 v7 v8 v9 v10= ()

let associative = 200
let containment = 201
let link = 202

let rec getnode (name:string, nList : NodeLabel list) : NodeLabel = 
    match nList with
    | head :: tail -> 
                        if head.ConfigId = name then head 
                        else getnode(name,tail)

    | [] -> printfn "error in function getnode, name not found"
            NodeLabel()

let sql conf:string = 
    let nl = getnode (conf, nodelist)  
    nl.Selectable

[<EntryPoint>]
let main argv =
    //EXAMPLE EXTRACT DSL

    variable "var1.1" source "pardat_sale"
    variable "var1.2" source "pardat_sale"

    variable "var2.1" source "land_sale"
    variable "var2.2" source "land_sale"

    variable "var3.1" source "dweldat_sale"
    variable "var3.2" source "dweldat_sale"


    save "myextract"

    show "label"


   //CAMA TREE CONFIG   
    node_label "res_sales" label "sales" selectable "sales" selectable_key ["salekey"; "jur"] condition [eq "cur" "Y"] 

    //node_label_attr "res_sales" label "sales" selectable "pardat_sale" 

    node_rel "res_sales" associative "pardat_sale" selectable_key ["salekey"; "jur"]                condition [eq "cur" "Y"] link "sales" ["salekey";"jur"]
    node_rel "res_sales" containment "dweldat_sale" selectable_key ["salekey"; "jur"; "card"]       condition [eq "cur" "Y"] link "sales" ["salekey";"jur"]
    node_rel "res_sales" containment "addn_sale" selectable_key ["salekey"; "jur"; "card"; "lline"] condition [eq "cur" "Y"] link "sales" ["salekey";"jur"]
    node_rel "res_sales" containment "oby_sale" selectable_key ["salekey"; "jur"; "card"; "lline"]  condition [eq "cur" "Y"] link "sales" ["salekey";"jur"]
    node_rel "res_sales" containment "land_sale" selectable_key ["salekey"; "jur"; "lline"]         condition [eq "cur" "Y"] link "sales" ["salekey";"jur"]
    node_rel "res_sales" containment "aprval_sale" selectable_key ["salekey"; "jur"]                condition [eq "cur" "Y"] link "sales" ["salekey";"jur"]


    
    // SQL generation
    let res = sql "res_sales"
    printfn "%s" res
    0 // return an integer exit code
