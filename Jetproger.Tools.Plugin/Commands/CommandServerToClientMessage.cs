using Jetproger.Tools.Trace.Bases;

namespace Jetproger.Tools.Plugin.Commands
{
    public abstract class CommandServerToClientMessage : TypedMessage
    {
        protected CommandServerToClientMessage(string message) : base(message)
        {
        }
    }
}