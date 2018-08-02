using System;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Trace.Bases
{
    public static class TraceExtensions
    {
        private static NlogTraceListener FileLogger { get { return Ex.GetOne(FileLoggerHolder, InitializeFileLogger); } }
        private static readonly NlogTraceListener[] FileLoggerHolder = { null };
        private static readonly TraceProxy GlobalTrace = Ex.GetOne<TraceProxy>();

        public static void SetAppUser(this ITraceExpander expander, string appUser)
        {
            GlobalTrace.SetAppUser(appUser);
        }

        public static void RegisterFileLogger(this ITraceExpander expander)
        {
            InitializeFileLogger(FileLogger);
            GlobalTrace.RegisterLogger(NlogConfig.GetMainTraceName());
        }

        public static void RegisterFileLogger<T>(this ITraceExpander expander) where T : ExTicket
        {
            InitializeFileLogger(FileLogger);
            GlobalTrace.RegisterLogger((typeof(T)).Name);
        }

        public static void Write(this ITraceExpander expander, object message)
        {
            InitializeFileLogger(FileLogger);
            var ticket = new ExTicket();
            var exException = message as ExException;
            if (exException != null)
            {
                WriteTicket(ticket, true, exException.Text, exException.Description);
                return;
            }
            var exception = message as Exception;
            if (exception != null)
            {
                var ex = new ExException(exception);
                WriteTicket(ticket, true, ex.Text, ex.Description);
                return;
            }
            var exTicket = message as ExTicket;
            if (exTicket != null)
            {
                WriteTicket(ticket, exTicket.IsException, exTicket.Text, exTicket.Description);
                return;
            }
            var text = message != null && message != DBNull.Value ? message.ToString() : null;
            WriteTicket(ticket, false, text, text);
        }

        public static void Write<T>(this ITraceExpander expander, object message) where T : ExTicket, new()
        {
            InitializeFileLogger(FileLogger);
            var ticket = new T();
            var exException = message as ExException;
            if (exException != null)
            {
                WriteTicket(ticket, true, exException.Text, exException.Description);
                return;
            }
            var exception = message as Exception;
            if (exception != null)
            {
                var ex = new ExException(exception);
                WriteTicket(ticket, true, ex.Text, ex.Description);
                return;
            }
            var exTicket = message as ExTicket;
            if (exTicket != null)
            {
                WriteTicket(ticket, exTicket.IsException, exTicket.Text, exTicket.Description);
                return;
            }
            var text = message != null && message != DBNull.Value ? message.ToString() : null;
            WriteTicket(ticket, false, text, text);
        }

        private static void WriteTicket(ExTicket ticket, bool isException, string text, string description)
        {
            ticket.IsException = isException;
            ticket.Text = text;
            ticket.Description = description;
            if (!string.IsNullOrWhiteSpace(ticket.Text) || !string.IsNullOrWhiteSpace(ticket.Description)) System.Diagnostics.Trace.WriteLine(ticket);
        }

        public static void WriteToFileLogger(string loggerName, ExTicket ticket)
        {
            GlobalTrace.Write(loggerName, ticket);
        }

        private static NlogTraceListener InitializeFileLogger()
        {
            var fileLogger = new NlogTraceListener();
            lock (System.Diagnostics.Trace.Listeners)
            {
                System.Diagnostics.Trace.Listeners.Add(fileLogger);
            }
            return fileLogger;
        }

        private static void InitializeFileLogger(NlogTraceListener fileLogger)
        {
        }
    }
}