using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public class TreeItem : MarshalByRefObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Icon { get; set; }
        public string Spec { get; set; }
        public string Loop { get; set; }
        public int Ordinal { get; set; }
        public bool Aggregator { get; set; }
        public string Shortcut { get; set; }
        public string Location { get; set; }
        public string TaskCode { get; set; }
        public string TreeCode { get; set; }
        public string ParentCode { get; set; }
        public string Permission { get; set; }
        public Task Task { get; set; }
        public Tree Tree { get; set; }
        public TreeItem Parent { get; set; }
        public List<TreeItem> Items { get; set; }

        public override string ToString()
        {
            return $"{TreeCode}\n{Code}";
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