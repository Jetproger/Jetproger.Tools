using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Structure.Bases
{
    public class Face : MarshalByRefObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Permission { get; set; }
        public List<FaceItem> Items { get; set; }

        public Face()
        {
            Items = new List<FaceItem>();
        }

        public override string ToString()
        {
            return Code;
        }

        public IEnumerable<FaceItem> AllItems()
        {
            foreach (var x in (Items ?? new List<FaceItem>()))
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