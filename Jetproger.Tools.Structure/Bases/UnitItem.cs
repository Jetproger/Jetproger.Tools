using System;

namespace Jetproger.Tools.Structure.Bases
{
    public class UnitItem : MarshalByRefObject
    {
        public string Code { get; set; }
        public string UnitCode { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Permission { get; set; }
        public CustomType Type { get; set; }
        public int Length { get; set; }
        public bool Unique { get; set; }
        public bool Index { get; set; }
        public bool Null { get; set; }
        public Unit Unit { get; set; }

        public override string ToString()
        {
            return $"{UnitCode}&{Code}";
        }
    }
}