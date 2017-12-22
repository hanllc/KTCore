using System;
using System.Collections.Generic;

namespace KT.VarLangDSLModel
{
    public class ExtractColumn
    {
        private Tuple<string,string,string> cm;
        public ExtractColumn(string name, string columnName, string dbObjectName) =>
            cm = new Tuple<string,string,string>(name,columnName,dbObjectName);
    }
    public class ExtractTable
    {
        private Tuple<string,List<ExtractColumn>> tm;
        public ExtractTable(string modelId, List<ExtractColumn> columns) =>
            tm = new Tuple<string,List<ExtractColumn>>(modelId,columns);
    }
    public class ExtractModel
    {
        private Tuple<string,List<ExtractTable>> em;
        public ExtractModel(string modelId, List<ExtractTable> tables) =>
            em = new Tuple<string,List<ExtractTable>>(modelId,tables);       
    }

    public struct NodeCondition
    {
        string Operator;
        string leftOperand;
        string rightOperand;
    }
    public class RootNodeLabel
    {
        private Tuple<string,string,string,List<string>,List<NodeCondition>> nl;
        public RootNodeLabel(string config, string label, string selectable,
                    List<string> selectableKey, List<NodeCondition> conditions) =>
            nl = new Tuple<string,string,string,List<string>,List<NodeCondition>>
                        (config,label,selectable,selectableKey,conditions);
        
    }
    public class NodeRelation
    {
        private Tuple<string,string,string,List<string>,List<NodeCondition>,string,List<string>> nr;
        
        public NodeRelation(string config, string relation, string selectable, 
                    List<string> selectableKey, List<NodeCondition> conditions,
                    string link, List<string> linkKey) =>
            nr = new Tuple<string,string,string,List<string>,List<NodeCondition>,string,List<string>>
                        (config,relation,selectable,selectableKey,conditions,link,linkKey);
        
    }
    
}

