
// Spike DSL on CLR for Market Extract 
// Given a declarative statement of extract,
// produce one or more SQL Statements


//open System
open System.Collections.Generic
open FSharp.Collections
open KT.VarLangDSLModel

//[<CLIMutable>]
type ExColumn =
    {
        Name : string
        ExColumnName : string
        DbObjectName : string
    }

type ExTable() = 
    [<DefaultValue>] val mutable TableId : string
    let mutable exCols = new Dictionary<string, ExColumn>()
    member this.AddCol (col:ExColumn) =
                let key = col.ExColumnName
                exCols.Add(key, col)
    member this.ColumnList = 
        let mutable mylist = []
        for entry in exCols do 
            mylist <- entry.Value :: mylist
        mylist



type ExModel() = 
    [<DefaultValue>] val mutable ModelId : string
    let mutable exColumnList : ExColumn list = []
    member this.ExColumnList with get() = exColumnList
  
    let mutable ExTableDict = new Dictionary<string, ExTable>()
    member this.ExTableList = 
        let mutable mylist = []
        for entry in ExTableDict do
            mylist <- entry.Value :: mylist 
        mylist

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
    dsl_extract.AddCol {
        Name = nameStr;
        ExColumnName = nameStr;
        DbObjectName = tabStr
    } 

//convert F# to C# model
//only so SQL gen code can be done in C#

let make_ctable_extract ( tm:ExTable ) = 
    let tlist = List<ExtractColumn>()
    for entry in tm.ColumnList do
        let tcol = 
            ExtractColumn(entry.Name, entry.ExColumnName, entry.DbObjectName)
        tlist.Add (tcol)
    ExtractTable(tm.TableId, tlist)

let make_cmodel_extract ( em:ExModel ) = 
    let tlist = List<ExtractTable>()
    for entry in em.ExTableList do
        let newExtractTable = make_ctable_extract(entry)
        tlist.Add ( newExtractTable )
    tlist

//CAMA TREE CONFIG DSL
//perhaps move to its own module

type NodeLabel() =
    [<DefaultValue>] val mutable ConfigId : string
    [<DefaultValue>] val mutable Label : string
    [<DefaultValue>] val mutable Selectable : string
    [<DefaultValue>] val mutable KeyList : string list
    [<DefaultValue>] val mutable CondList : (string * string * string) list
    
 
let mutable cond_tuples =[]
let eq a b = 
    let newtup = ("eq", a, b)
    cond_tuples <- newtup ::cond_tuples
()
let mutable nodelist : NodeLabel list = []

let node_label uid labelTok label dbObjTok dbObj keyTok keyList condTok condList = 
    let node = NodeLabel()
    node.ConfigId <- uid
    node.Label <- label
    node.Selectable <- dbObj
    node.KeyList <- keyList
    node.CondList <- cond_tuples
    nodelist <- node :: nodelist
    cond_tuples <- []
    ()
let label = 100
let selectable = 101
let selectable_key = 102
let condition = 103



type NodeRel = {
    ConfigId : string
    Relation : int
    Selectable : string
    KeyList : string list
    CondList : (string * string * string) list
    LinkLabel : string
    LinkKeyList : string list
}

let mutable relatelist : NodeRel list = []
 
let node_rel uid relTypeTok selItem selKeyTok keyList condTok condList linkTok linkItem linkKeyList =

    let rel = {
        ConfigId = uid;
        Relation = relTypeTok
        Selectable = selItem;
        KeyList = keyList;
        CondList = cond_tuples;
        LinkLabel = linkItem;
        LinkKeyList = linkKeyList
    }
    relatelist <- rel ::relatelist
    cond_tuples <- []

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



//convert F# to C# model
//only so SQL gen code can be done in C#

let make_condlist condlist =
    let resL = List<NodeCondition>()
    for m in condlist do
        let (op, l, r) = m
        let mutable a = NodeCondition()
        a.Operator <- op
        a.leftOperand <- l
        a.rightOperand <- r
        resL.Add(a)
    resL
let newrelate (relate : NodeRel) : NodeRelation =
    NodeRelation (relate.ConfigId,
                    enum<NodeRelationEnum>relate.Relation,
                    relate.Selectable,
                    List(relate.KeyList),
                    make_condlist relate.CondList,
                    relate.LinkLabel,
                    List(relate.LinkKeyList))

//need to build the other two lists
let make_csys_relate ( myrelatelist ) = 
    let tlist = List<NodeRelation>()
    for entry in myrelatelist do
        let newItem = newrelate entry
        tlist.Add ( newItem )
    tlist


let newroot (root: NodeLabel) : RootNodeLabel  =
    RootNodeLabel (root.ConfigId,
                    root.Label,
                    root.Selectable,
                    List(root.KeyList),
                    make_condlist root.CondList)

let make_csys_root myrootlist  = 
    let tlist = List<RootNodeLabel>()
    for entry in myrootlist do
        let newItem = newroot entry
        tlist.Add ( newItem )
    tlist

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
 

    node_rel "res_sales" associative "pardat_sale" selectable_key ["salekey"; "jur"]                condition [eq "cur" "Y"] link "sales" ["salekey";"jur"]
    node_rel "res_sales" containment "dweldat_sale" selectable_key ["salekey"; "jur"; "card"]       condition [eq "cur" "Y"; eq "cur" "Y"] link "sales" ["salekey";"jur"]
    node_rel "res_sales" containment "addn_sale" selectable_key ["salekey"; "jur"; "card"; "lline"] condition [eq "cur" "Y"] link "sales" ["salekey";"jur"]
    node_rel "res_sales" containment "oby_sale" selectable_key ["salekey"; "jur"; "card"; "lline"]  condition [eq "cur" "Y"] link "sales" ["salekey";"jur"]
    node_rel "res_sales" containment "land_sale" selectable_key ["salekey"; "jur"; "lline"]         condition [eq "cur" "Y"] link "sales" ["salekey";"jur"]
    node_rel "res_sales" containment "aprval_sale" selectable_key ["salekey"; "jur"]                condition [eq "cur" "Y"] link "sales" ["salekey";"jur"]

    
    // SQL generation
    let res = sql "res_sales"
    printfn "%s" res

    let cmodel_extract = make_cmodel_extract dsl_extract

    let cmodel_root = make_csys_root nodelist

    let cmodel_relate = make_csys_relate relatelist

    

    let con = Converter()
    let sl = con.BuildAttributes cmodel_extract
    let s2 = con.BuildStatements ("res_sales", cmodel_root, cmodel_relate, sl)
    let stmt = s2.Item 0
    let (x, y) = stmt
    let z = y
    let z2 = z

    0 // return an integer exit code
