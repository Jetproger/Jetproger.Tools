using System;
using Jetproger.Tools.Trace.Bases;

namespace Jetproger.Tools.Plugin.Commands
{
    public class CommandServerToClientMessage : TypedMessage
    {
        public CommandServerToClientMessage(string message, Exception exception) : base(message, exception) { }
        public CommandServerToClientMessage(Exception exception) : this(null, exception) { }
        public CommandServerToClientMessage(string message) : this(message, null) { }
    }
}