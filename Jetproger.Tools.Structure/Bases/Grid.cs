using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Structure.Bases
{
    public class Grid : MarshalByRefObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Renderer { get; set; }
        public string Permission { get; set; }
        public List<GridItem> Items { get; set; }

        public Grid()
        {
            Items = new List<GridItem>();
        }

        public override string ToString()
        {
            return Code;
        }
    }
}