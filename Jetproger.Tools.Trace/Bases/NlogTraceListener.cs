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
                if (!string.IsNullOrWhiteSpace(typedMessage.Message)) Log.GlobalTrace.Trace(typedMessage.Message, message.GetType().Name);
                if (!string.IsNullOrWhiteSpace(typedMessage.Error)) Log.GlobalTrace.Error(typedMessage.Error, message.GetType().Name);
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

        public static string GetExceptionAsString(Exception e)
        {
            if (e == null)
            {
                return null;
            }
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