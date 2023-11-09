using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Commanders
{
    [DataContract]
    [Serializable]
    public class CommandRequest
    {
        [DataMember] public Guid Session { get; set; } 
        [DataMember] public string Command { get; set; }
        [DataMember] public string Value { get; set; }
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