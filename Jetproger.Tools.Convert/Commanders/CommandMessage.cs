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
    public class CommandMessage
    {
        [DataMember] public CommandMessage[] Messages { get; set; }
        [DataMember] public ECommandMessage Category { get; set; }
        [DataMember] public string Message { get; set; }

        public CommandMessage()
        {
            Category = ECommandMessage.Trace;
        }

        public CommandMessage(string message)
        {
            Message = message;
            Category = ECommandMessage.Trace;
        }

        public CommandMessage(string message, ECommandMessage category)
        {
            Message = message;
            Category = category;
        }

        public CommandMessage(params CommandMessage[] messages)
        {
            Messages = messages;
            Category = ECommandMessage.Trace;
        }

        public CommandMessage(params Exception[] exceptions)
        {
            Category = ECommandMessage.Trace;
            exceptions = exceptions ?? new Exception[0];
            var oneException = exceptions.Length == 1 ? exceptions[0] : null;
            if (oneException != null)
            {
                Message = ((oneException as CommandException) ?? new CommandException(oneException)).Message;
                Category = ECommandMessage.Error;
            }
            var messages = new List<CommandMessage>();
            foreach (Exception exception in exceptions)
            {
                var commandException = (exception as CommandException) ?? new CommandException(exception);
                messages.Add(new CommandMessage { Message = commandException.Message, Category = ECommandMessage.Error });
            }
            Messages = messages.ToArray();
        }
    }

    [DataContract, Serializable]
    public enum ECommandMessage
    {
        [DataMember] Trace,
        [DataMember] Debug,
        [DataMember] Info,
        [DataMember] Warn,
        [DataMember] Error
    }
}