using System;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Plugin.Commands
{
    public class CommandServerToClientMessage : Jc.Ticket
    {
        public CommandServerToClientMessage(Exception exception) : this(null, exception) { }
        public CommandServerToClientMessage(string message) : this(message, null) { }
        public CommandServerToClientMessage(string message, Exception exception)
        {
            IsException = exception != null;
            var e = new Jc.Exception(exception);
            Text = IsException ? e.Text : message;
            Description = IsException ? e.Description : message;
        }
    }
}