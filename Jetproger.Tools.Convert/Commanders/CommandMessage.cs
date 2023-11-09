using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text; 

namespace Jetproger.Tools.Convert.Commanders
{
    [Serializable]
    [DataContract]
    public class CommandReport
    {
        [DataMember] public CommandMessage[] Messages { get; set; } 
    }

    [Serializable]
    [DataContract]
    public class CommandMessage
    {
        [DataMember] public string Category { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public string Info { get; set; }
        [DataMember] public string Status { get; set; }
    }

    public enum ECommandMessage
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error
    }
}