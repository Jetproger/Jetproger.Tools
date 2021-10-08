using System;
using System.Collections.Generic;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Commands
{
    public static class CommandExtensions
    {
        public static CommandTicket Trace(this Je.ICmdExpander x, string message, string comment = null)
        {
            return new CommandTicket { Category = ECommandTicket.Trace, Message = message, Comment = comment };
        }

        public static CommandTicket Debug(this Je.ICmdExpander x, string message, string comment = null)
        {
            return new CommandTicket { Category = ECommandTicket.Debug, Message = message, Comment = comment };
        }

        public static CommandTicket Info(this Je.ICmdExpander x, string message, string comment = null)
        {
            return new CommandTicket { Category = ECommandTicket.Info, Message = message, Comment = comment };
        }

        public static CommandTicket Warn(this Je.ICmdExpander x, string message, string comment = null)
        {
            return new CommandTicket { Category = ECommandTicket.Warn, Message = message, Comment = comment };
        }

        public static CommandTicket Error(this Je.ICmdExpander x, string message, string comment = null)
        {
            return !string.IsNullOrWhiteSpace(message) || !string.IsNullOrWhiteSpace(comment) ? new CommandTicket { Category = ECommandTicket.Error, Message = message, Comment = comment } : null;
        }

        public static string ErrorMessage(this Je.ICmdExpander x, Exception e)
        {
            return e != null ? (ErrorMessage(e as GTINException) ?? e.Message) : null;
        }

        private static string ErrorMessage(GTINException e)
        {
            return e != null ? e.Description : null;
        }

        public static string ErrorComment(this Je.ICmdExpander x, Exception e)
        {
            return e != null ? (e is CommandException ? e.ToString() : ((e as GTINException) ?? new GTINException(e)).Description) : null;
        }

        public static CommandTicket Error(this Je.ICmdExpander x, Exception e)
        {
            return e != null ? new CommandTicket { Category = ECommandTicket.Error, Message = e.Message, Comment = e is CommandException ? e.ToString() : ((e as GTINException) ?? new GTINException(e)).Description } : null;
        }

        public static CommandException Exception(this Je.ICmdExpander x, CommandTicket ticket)
        {
            return ticket != null ? new CommandException(ticket) : null;
        }

        public static CommandException Exception(this Je.ICmdExpander x, string message, string comment = null)
        {
            return !string.IsNullOrWhiteSpace(message) || !string.IsNullOrWhiteSpace(comment) ? new CommandException(message, comment) : null;
        }

        public static CommandTicket[] Report(this Je.ICmdExpander x, params CommandTicket[] tickets)
        {
            return tickets;
        }

        public static CommandResponse BeginExecute<T>(this Je.ICmdExpander x, ICommand command, CommandRequest request) where T : BaseCommander
        {
            return BaseCommander<T>.Instance.BeginExecute(command, request);
        }

        public static CommandTicket FirstError(this Je.ICmdExpander x, IEnumerable<CommandTicket> tickets)
        {
            foreach (CommandTicket ticket in (tickets ?? new CommandTicket[0]))
            {
                if (ticket.Category == ECommandTicket.Error) return ticket;
            }
            return null;
        }

        public static CommandTicket LastError(this Je.ICommandExpander x, IEnumerable<CommandTicket> tickets)
        {
            var error = (CommandTicket)null;
            foreach (CommandTicket ticket in (tickets ?? new CommandTicket[0]))
            {
                if (ticket.Category == ECommandTicket.Error) error = ticket;
            }
            return error;
        }

        public static bool Log(this Je.ICommandExpander x, CommandTicket[] tickets)
        {
            var result = false;
            foreach (CommandTicket ticket in (tickets ?? new CommandTicket[0]))
            {
                var resultTicket = Log(x, ticket);
                if (!result && resultTicket) result = true;
            }
            return result;
        }

        public static bool Log(this Je.ICommandExpander x, CommandTicket ticket)
        {
            if (ticket == null) return false;
            if (string.IsNullOrWhiteSpace(ticket.Message) && string.IsNullOrWhiteSpace(ticket.Comment)) return false;
            var s = string.Format("{0}{1}{2}", ticket.Message, Environment.NewLine, ticket.Comment);
            switch (ticket.Category) {
                case ECommandTicket.Error: return LogError(s);
                default: return LogTrace(s);
            }
        }

        private static bool LogError(string s)
        {
            Je.log.Error(s); return true;
        }

        private static bool LogTrace(string s)
        {
            Je.log.Trace(s); return true;
        }
    }
}