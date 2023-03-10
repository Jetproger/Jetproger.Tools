using System;
using System.Runtime.Serialization;

namespace Jetproger.Tools.Convert.Commanders
{
    [DataContract, Serializable]
    public class CommandRequest
    {
        [DataMember] public Guid Session { get; set; }

        [DataMember] public string Command { get; set; }

        [DataMember] public string Value { get; set; }
    }
}