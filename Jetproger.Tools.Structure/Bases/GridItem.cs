using System;

namespace Jetproger.Tools.Structure.Bases
{
    public class GridItem : MarshalByRefObject
    {
        public string Code { get; set; }
        public string GridCode { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Category { get; set; }
        public string BeforeChange { get; set; }
        public string AfterChange { get; set; }
        public CustomType Type { get; set; }
        public int Ordinal { get; set; }
        public int Width { get; set; }
        public int Colspan { get; set; }
        public bool Visible { get; set; }
        public bool ReadOnly { get; set; }
        public bool Null { get; set; }
        public bool Stored { get; set; }
        public string Permission { get; set; }
        public Grid Grid { get; set; }

        public override string ToString()
        {
            return $"{GridCode}&{Code}";
        }
    }
}