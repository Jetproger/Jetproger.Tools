using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public class Tree : MarshalByRefObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Permission { get; set; }
        public List<TreeItem> Items { get; set; }

        public override string ToString()
        {
            return Code;
        }

        public IEnumerable<TreeItem> AllItems()
        {
            foreach (var x in (Items ?? new List<TreeItem>()))
            {
                yield return x;
                foreach (var y in x.AllItems())
                {
                    yield return y;
                }
            }
        }
    }
}