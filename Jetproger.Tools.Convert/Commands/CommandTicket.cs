using System;
using System.Runtime.Serialization; 

namespace Jetproger.Tools.Convert.Commands
{
    [DataContract, Serializable]
    public class CommandTicket
    {
        [DataMember]public ECommandTicket Category { get; set; }  
        [DataMember]public string Message { get; set; } 
        [DataMember]public string Comment { get; set; }
    } 
    [DataContract, Serializable]
    public enum ECommandTicket
    {
        [DataMember]Trace,
        [DataMember]Debug,
        [DataMember]Info,
        [DataMember]Warn,
        [DataMember]Error
    }

    public class CommandException : Exception
    {
        private readonly string _message;
        private readonly string _comment;

        public CommandException(string message, string comment)
        {
            _message = message;
            _comment = comment;
        }

        public CommandException(CommandTicket ticket)
        {
            _message = ticket.Message;
            _comment = ticket.Comment;
        }

        public override string Message
        {
            get { return !string.IsNullOrWhiteSpace(_message) ? _message : base.Message; }
        }

        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(_comment) ? _comment : base.ToString();
        }
    }
}