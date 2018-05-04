using System;
using System.Collections.Generic;
using System.Text;

namespace KT.VarLangDSLModel
{
    public class ExtractColumn
    {
        private Tuple<string,string,string> cm;
        public ExtractColumn(string name, string columnName, string dbObjectName) =>
            cm = new Tuple<string,string,string>(name,columnName,dbObjectName);
        public string VarName { get { return cm.Item1; } }
        public string ColName { get { return cm.Item2; } }
        public string TabName { get { return cm.Item3; } }
    }
    public class ExtractTable
    {
        private Tuple<string,List<ExtractColumn>> tm;
        public ExtractTable(string modelId, List<ExtractColumn> columns) =>
            tm = new Tuple<string,List<ExtractColumn>>(modelId,columns);
        public IReadOnlyList<ExtractColumn> Get() { return tm.Item2.AsReadOnly(); }
        public string Name { get { return tm.Item1; } }
    }
    public class ExtractModel
    {
        private Tuple<string,List<ExtractTable>> em;
        public ExtractModel(string modelId, List<ExtractTable> tables) =>
            em = new Tuple<string,List<ExtractTable>>(modelId,tables);
        public IReadOnlyList<ExtractTable> Get() { return em.Item2.AsReadOnly(); }       
    }

    public struct NodeCondition
    {
        public string Operator;
        public string leftOperand;
        public string rightOperand;
    }
    public class RootNodeLabel
    {
        private Tuple<string,string,string,List<string>,List<NodeCondition>> nl;
        public RootNodeLabel(string config, string label, string selectable,
                    List<string> selectableKey, List<NodeCondition> conditions) =>
            nl = new Tuple<string,string,string,List<string>,List<NodeCondition>>
                        (config,label,selectable,selectableKey,conditions);
        public string Config { get { return nl.Item1; } }
        public string Label { get { return nl.Item2; } } 
        public string Selectable { get { return nl.Item3; } }
        public IReadOnlyList<string> SelectableKey { get { return nl.Item4.AsReadOnly(); } }        
    }
    public enum NodeRelationEnum : int
    {
        Associative = 200,
        Containment = 201
    }
    public class NodeRelation
    {
        private Tuple<string,NodeRelationEnum,string,List<string>,List<NodeCondition>,string,List<string>> nr;
        
        public NodeRelation(string config, NodeRelationEnum relation, string selectable, 
                    List<string> selectableKey, List<NodeCondition> conditions,
                    string link, List<string> linkKey) =>
            nr = new Tuple<string,NodeRelationEnum,string,List<string>,List<NodeCondition>,string,List<string>>
                        (config,relation,selectable,selectableKey,conditions,link,linkKey);
        public string Config { get { return nr.Item1; } }
        public string Selectable { get { return nr.Item3; } }  
        public string Link { get { return nr.Item6; } }
        public IReadOnlyList<string> LinkKey { get { return nr.Item7.AsReadOnly(); } }
    }    
}
