using System;
using System.Runtime.Serialization;

namespace Jetproger.Tools.Convert.Commanders
{
    [DataContract, Serializable]
    public class CommandResponse
    {
        [DataMember]public Guid Session { get; set; }

        [DataMember]public string Result { get; set; }

        [DataMember]public CommandMessage[] Report { get; set; }
    }
}