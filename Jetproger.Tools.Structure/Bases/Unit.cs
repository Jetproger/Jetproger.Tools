using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public class Unit : MarshalByRefObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Permission { get; set; }
        public List<UnitItem> Items { get; set; }

        public Unit()
        {
            Items = new List<UnitItem>();
        }

        public override string ToString()
        {
            return Code;
        }
    }
}