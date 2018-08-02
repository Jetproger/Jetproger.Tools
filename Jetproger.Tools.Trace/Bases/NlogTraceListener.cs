using System;
using System.Diagnostics;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Trace.Bases
{
    public class NlogTraceListener : TraceListener
    {
        public void Write()
        {
        }

        public override void Write(string message)
        {
            WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            TraceExtensions.WriteToFileLogger(NlogConfig.GetMainTraceName(), new ExTicket { IsException = false, Text = message, Description = message});
        }

        public override void Write(object message)
        {
            WriteLine(message);
        }

        public override void WriteLine(object message)
        {
            var exException = message as ExException;
            if (exException != null)
            {
                var ticket = new ExTicket { IsException = true, Text = exException.Text, Description = exException.Description };
                if (string.IsNullOrWhiteSpace(ticket.Text) && string.IsNullOrWhiteSpace(ticket.Description)) return;
                TraceExtensions.WriteToFileLogger(NlogConfig.GetMainTraceName(), ticket);
                return;
            }
            var exception = message as Exception;
            if (exception != null)
            {
                var ex = new ExException(exception);
                var ticket = new ExTicket { IsException = true, Text = ex.Text, Description = ex.Description };
                if (string.IsNullOrWhiteSpace(ticket.Text) && string.IsNullOrWhiteSpace(ticket.Description)) return;
                TraceExtensions.WriteToFileLogger(NlogConfig.GetMainTraceName(), ticket);
                return;
            }
            var exTicket = message as ExTicket;
            if (exTicket != null)
            {
                var type = exTicket.GetType();
                var loggerName = type == typeof(ExTicket) ? NlogConfig.GetMainTraceName() : type.Name;
                if (string.IsNullOrWhiteSpace(exTicket.Text) && string.IsNullOrWhiteSpace(exTicket.Description)) return;
                TraceExtensions.WriteToFileLogger(loggerName, exTicket);
                return;
            }
            var text = message != null && message != DBNull.Value ? message.ToString() : null;
            if (!string.IsNullOrWhiteSpace(text)) TraceExtensions.WriteToFileLogger(NlogConfig.GetMainTraceName(), new ExTicket { IsException = false, Text = text, Description = text });
        }
    }
}