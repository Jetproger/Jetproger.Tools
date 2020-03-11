using System;
using System.Runtime.Serialization;

namespace Jc
{
    [Serializable]
    [DataContract]
    public class DataField
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DataStyle Style { get; set; }
    }
}