using System;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Process.Commands
{
    public class CommandServerToClientMessage : Ticket
    {
        public CommandServerToClientMessage(Exception exception) : this(null, exception) { }
        public CommandServerToClientMessage(string message) : this(message, null) { }
        public CommandServerToClientMessage(string message, Exception exception)
        {
            IsException = exception != null;
            var e = new f.Exception(exception);
            Text = IsException ? e.Text : message;
            Description = IsException ? e.Description : message;
        }
    }
}