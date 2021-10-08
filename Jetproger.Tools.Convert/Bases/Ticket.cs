using System;
using System.Runtime.Serialization;

namespace Jetproger.Tools.Convert.Bases
{
    [Serializable, DataContract]
    public class Ticket
    {
        [DataMember]public bool IsError { get; set; } 

        [DataMember]public string Message { get; set; }

        [DataMember]public string Comment { get; set; }
    }
}