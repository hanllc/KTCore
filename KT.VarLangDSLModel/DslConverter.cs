
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KT.VarLangDSLModel
{
    public class Converter 
    {
        public List<Tuple<string,string>> BuildAttributes ( List<ExtractTable> tables )
        {
            var rl = new List<Tuple<string,string>>();
            foreach (ExtractTable t in tables)
            {
                var name = t.Name;
                foreach (ExtractColumn c in t.Get())
                {
                    var s = $"{t.Name}.{c.ColName} AS {c.VarName}";
                    var tp = new Tuple<string,string>($"{t.Name}",s);
                    rl.Add (tp);
                }
            }
            return rl;
        }

        public List<Tuple<string,string>> BuildStatements (
            string config, List<RootNodeLabel> roots, List<NodeRelation> relations,
            List<Tuple<string,string>> attrs
        )
        {
            var resultL = new List<Tuple<string,string>>();

            var s = new HashSet<string>();
            foreach(Tuple<string,string> attr in attrs)
                s.Add(attr.Item1);

            var crootL = from root in roots
                        where root.Config == config
                        select root;
            var croot = crootL.First();
            var from = new StringBuilder();
            from.Append("FROM ");
            from.Append(croot.Selectable);
            var select = new StringBuilder();
            select.Append("SELECT * ");
            select.Append(from);
            var newL = new Tuple<string,string> ("primary", select.ToString());
            resultL.Add(newL);
            return resultL;
        }
    }
}
