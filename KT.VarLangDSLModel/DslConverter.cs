
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
            from.Append(croot.Selectable + " ");
            //for each table involved in primary extract
            //  form the approprite join with the rootnode
            var joins = MakeJoins().ToString();
            from.AppendLine(joins);

            var select = new StringBuilder();
            select.Append("SELECT * ");
            //for each table involved in primary extract
            //  form the approprite selects

            select.Append(from);
            var newL = new Tuple<string,string> ("primary", select.ToString());
            resultL.Add(newL);
            return resultL;

            StringBuilder MakeJoins()
            {
                // add join if match on config, link label, need data from the table
                var crelateL = from rel in relations
                                where (rel.Config == config && rel.Link == croot.Label)
                                select rel;
                var crelateL2 = from rel2 in crelateL
                                where s.Contains(rel2.Selectable)
                                select rel2;
                //for each join selectable
                // add selectable name and join conditions
                var jterm = new StringBuilder();
                foreach (NodeRelation j in crelateL2)
                {
                    jterm.Append("LEFT OUTER JOIN " + j.Selectable);
                    jterm.Append(" ON ");
                    var lkeyL = croot.SelectableKey;
                    var rkeyL = j.LinkKey;
                    for(int i=0;i<lkeyL.Count;i++)
                    {
                        if (i!=0) jterm.Append(" and ");
                        var on = $"{croot.Selectable}.{lkeyL[i]} = {j.Selectable}.{rkeyL[i]} ";
                        jterm.AppendLine(on);
                    } 
                }
                return jterm;
            }

        }
    }
}
