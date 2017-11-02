using System;
using System.Diagnostics;
using System.Text;
using Log = Tools.Trace;

namespace Jetproger.Tools.Trace.Bases
{
    public class NlogTraceListener : TraceListener
    {
        public override void Write(object message)
        {
            WriteLine(message);
        }

        public override void WriteLine(object message)
        {
            var exception = message as Exception;
            if (exception != null)
            {
                Log.GlobalTrace.Error(GetExceptionAsString(exception), NlogConfig.GetMainTraceName());
                return;
            }
            var typedMessage = message as TypedMessage;
            if (typedMessage != null)
            {
                Log.GlobalTrace.Trace(typedMessage.Message, message.GetType().Name);
                return;
            }
            Log.GlobalTrace.Trace(message.ToString(), NlogConfig.GetMainTraceName());
        }

        public override void Write(string message)
        {
            WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            Log.GlobalTrace.Trace(message, NlogConfig.GetMainTraceName());
        }

        private static string GetExceptionAsString(Exception e)
        {
            var sb = new StringBuilder();
            sb.AppendLine(e.ToString());
            while (e != null)
            {
                sb.AppendLine(e.Message);
                e = e.InnerException;
            }
            return sb.ToString();
        }
    }
}