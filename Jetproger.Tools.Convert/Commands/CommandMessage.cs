using System; 
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text; 

namespace Jetproger.Tools.Convert.Commands
{
    [DataContract, Serializable]
    public class CommandMessage
    {
        [DataMember]public ECommandMessage Category { get; set; } 
        [DataMember]public string Message { get; set; } 
        [DataMember]public string Comment { get; set; }
    } 
    [DataContract, Serializable]
    public enum ECommandMessage
    {
        [DataMember]Trace,
        [DataMember]Debug,
        [DataMember]Info,
        [DataMember]Warn,
        [DataMember]Error
    }
}