using System;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Plugin.Commands
{
    public class CommandServerToClientMessage : ExTicket
    {
        public CommandServerToClientMessage(Exception exception) : this(null, exception) { }
        public CommandServerToClientMessage(string message) : this(message, null) { }
        public CommandServerToClientMessage(string message, Exception exception)
        {
            IsException = exception != null;
            var e = new ExException(exception);
            Text = IsException ? e.Text : message;
            Description = IsException ? e.Description : message;
        }
    }
}