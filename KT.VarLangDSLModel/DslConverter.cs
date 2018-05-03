
using System;
using System.Collections.Generic;

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

        public List<Tuple<string,string>> BuildStatements (List<Tuple<string,string>> attrs)
        {
            var s = new HashSet<string>();
            foreach(Tuple<string,string> attr in attrs)
                s.Add(attr.Item1);
            return new List<Tuple<string,string>>();
        }
    }
}
