using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commanders
{
    [DataContract, Serializable]
    public class CommandRequest
    {
        [DataMember]public Guid Session { get; set; } 
        [DataMember]public string Result { get; set; }
        [DataMember]public string Command { get; set; }
        [DataMember]public string Parameter { get; set; }
        [DataMember]public string Document { get; set; }

        public CommandRequest() : this(Guid.NewGuid()) { } 

        public CommandRequest(Guid session)
        {
            Session = session;
        }

        public CommandRequest(CommandRequest request)
        {
            Session = request.Session;
            Result = request.Result;
            Command = request.Command;
            Parameter = request.Parameter;
            Document = request.Document;
        }

        public object DeserializeParameter()
        {
            var parameterType = f.sys.classof(Parameter);
            if (parameterType == null) throw new TypeNotFoundException(Parameter);
            return f.xml.to(Document, parameterType);
        }
    }
}