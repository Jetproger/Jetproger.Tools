using System;
using System.Runtime.Serialization;

namespace Jetproger.Tools.Convert.Commands
{
    [DataContract, Serializable]
    public class CommandRequest
    {
        [DataMember]public Guid Session { get; set; } 

        [DataMember]public string Command { get; set; }

        [DataMember]public string Document { get; set; }
    }
}