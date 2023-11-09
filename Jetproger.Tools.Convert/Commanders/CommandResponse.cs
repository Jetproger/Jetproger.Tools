using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Commanders
{
    [Serializable]
    [DataContract]
    public class CommandResponse
    {
        [DataMember] public Guid Session { get; set; } 
        [DataMember] public string Report { get; set; } 
        [DataMember] public string Result { get; set; }
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