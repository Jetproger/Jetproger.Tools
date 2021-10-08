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
            TraceExtensions.WriteToFileLogger(NlogConfig.GetMainTraceName(), new Jc.Ticket { IsException = false, Text = message, Description = message});
        }

        public override void Write(object message)
        {
            WriteLine(message);
        }

        public override void WriteLine(object message)
        {
            Ticket ticket;
            var jce = message as Je.Exception;
            if (jce != null)
            {
                ticket = new Ticket { IsException = true, Text = jce.Text, Description = jce.Description };
                if (string.IsNullOrWhiteSpace(ticket.Text) && string.IsNullOrWhiteSpace(ticket.Description)) return;
                TraceExtensions.WriteToFileLogger(NlogConfig.GetMainTraceName(), ticket);
                return;
            }
            var exception = message as Exception;
            if (exception != null)
            {
                var ex = new Je.Exception(exception);
                ticket = new Ticket { IsException = true, Text = ex.Text, Description = ex.Description };
                if (string.IsNullOrWhiteSpace(ticket.Text) && string.IsNullOrWhiteSpace(ticket.Description)) return;
                TraceExtensions.WriteToFileLogger(NlogConfig.GetMainTraceName(), ticket);
                return;
            }
            ticket = message as Jc.Ticket;
            if (ticket != null) {
                var type = ticket.GetType();
                var loggerName = type == typeof(Jc.Ticket) ? NlogConfig.GetMainTraceName() : type.Name;
                if (string.IsNullOrWhiteSpace(ticket.Text) && string.IsNullOrWhiteSpace(ticket.Description)) return;
                TraceExtensions.WriteToFileLogger(loggerName, ticket);
                return;
            }
            var text = message != null && message != DBNull.Value ? message.ToString() : null;
            if (!string.IsNullOrWhiteSpace(text)) TraceExtensions.WriteToFileLogger(NlogConfig.GetMainTraceName(), new Jc.Ticket { IsException = false, Text = text, Description = text });
        }
    }
}