using System;
using System.Collections.Generic;
using System.Linq;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Commands
{ 
    public static class CommandExtensions
    {
        public static CommandMessage TraceMsg(this Je.ICmdExpander exp, string message, string comment = null)
        {
            return !IsEmptyStrings(message, comment) ? new CommandMessage { Category = ECommandMessage.Trace, Message = message, Comment = comment } : null;
        }

        public static CommandMessage DebugMsg(this Je.ICmdExpander exp, string message, string comment = null)
        {
            return !IsEmptyStrings(message, comment) ? new CommandMessage { Category = ECommandMessage.Debug, Message = message, Comment = comment } : null;
        }

        public static CommandMessage InfoMsg(this Je.ICmdExpander exp, string message, string comment = null)
        {
            return !IsEmptyStrings(message, comment) ? new CommandMessage { Category = ECommandMessage.Info, Message = message, Comment = comment } : null;
        }

        public static CommandMessage WarnMsg(this Je.ICmdExpander exp, string message, string comment = null)
        {
            return !IsEmptyStrings(message, comment) ? new CommandMessage { Category = ECommandMessage.Warn, Message = message, Comment = comment } : null;
        }

        public static CommandMessage ErrorMsg(this Je.ICmdExpander exp, string message, string comment = null)
        {
            return !IsEmptyStrings(message, comment) ? new CommandMessage { Category = ECommandMessage.Error, Message = message, Comment = comment } : null;
        }

        private static bool IsEmptyStrings(string s1, string s2)
        {
            return string.IsNullOrWhiteSpace(s1) && string.IsNullOrWhiteSpace(s2);
        }

        public static CommandExecuteEventArgs ErrorEventArgs(this Je.ICmdExpander exp, ICommand command, IEnumerable<Exception> exceptions)
        {
            var arr = ExceptionsOf(exceptions); return new CommandExecuteEventArgs { Command = command, IsSuccess = arr == null, Exceptions = arr };
        }

        public static CommandExecuteEventArgs ErrorEventArgs(this Je.ICmdExpander exp, ICommand command, params Exception[] exceptions)
        {
            var arr = ExceptionsOf(exceptions); return new CommandExecuteEventArgs { Command = command, IsSuccess = arr == null, Exceptions = arr };
        }

        public static CommandExecuteEventArgs EmptyEventArgs(this Je.ICmdExpander exp, ICommand command)
        {
            return new CommandExecuteEventArgs { IsSuccess = true };
        }

        private static Exception[] ExceptionsOf(IEnumerable<Exception> exceptions)
        {
            var arr = (exceptions ?? new Exception[0]).Where(x => x != null).ToArray(); return arr.Length > 0 ? arr : null;
        }

        public static CommandMessage[] MessagesOf(this Je.ICmdExpander exp, params CommandMessage[] messages)
        {
            return messages;
        }
    }
}