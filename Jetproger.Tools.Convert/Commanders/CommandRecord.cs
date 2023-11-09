using System;
using System.Runtime.Serialization;

namespace Jetproger.Tools.Convert.Commanders
{
    [Serializable]
    [DataContract]
    public class CommandRecord
    {
        [DataMember] public string Lifetime { get; set; }
        [DataMember] public string Name { get; set; }

        [DataMember] public string P_ { get; set; }
        [DataMember] public string P0 { get; set; }
        [DataMember] public string P1 { get; set; }
        [DataMember] public string P2 { get; set; }
        [DataMember] public string P3 { get; set; }
        [DataMember] public string P4 { get; set; }
        [DataMember] public string P5 { get; set; }
        [DataMember] public string P6 { get; set; }
        [DataMember] public string P7 { get; set; }
        [DataMember] public string P8 { get; set; }
        [DataMember] public string P9 { get; set; }
    }
}