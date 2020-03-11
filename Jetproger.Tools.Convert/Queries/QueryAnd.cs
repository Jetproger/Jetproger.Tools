using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Datasets;

namespace Jc
{
    [Serializable][DataContract]
    public class QueryAnd
    {
        private Type _colType = typeof(string);
        private int _col = -1;

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public QueryOp Operator { get; set; }

        [DataMember]
        public object Sample { get; set; }

        public void ResolveField(DataField[] cols)
        {
            var name = Name.ToLower();
            for (int i = 0; i < cols.Length; i++)
            {
                var col = cols[i];
                if (col.Name.ToLower() != name) continue;
                _colType = col.Style.AsType();
                _col = i; 
            }
        }

        public bool IsMatch(DataTuple row)
        {
            return _col >= 0 && _col < row.Values.Length && QueryMatch.Is(row.Values[_col].As(_colType), Operator, Sample);
        }
    }
}