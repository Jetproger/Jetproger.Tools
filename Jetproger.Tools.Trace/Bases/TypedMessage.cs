using System;

namespace Jetproger.Tools.Trace.Bases
{
    public abstract class TypedMessage
    {
        public readonly string Message;
        public readonly string Error;

        protected TypedMessage(string message, Exception exception)
        {
            Message = (message ?? string.Empty).Trim(' ', '\t', '\r', '\n');
            Error = NlogTraceListener.GetExceptionAsString(exception);
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}