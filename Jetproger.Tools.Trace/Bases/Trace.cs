using System;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Trace.Bases
{
    public static class TraceExtensions
    {
        private static NlogTraceListener FileLogger { get { return f.one.Get(FileLoggerHolder, InitializeFileLogger); } }

        private static readonly NlogTraceListener[] FileLoggerHolder = { null };

        public static void SetAppUser(this ITraceExpander expander, string appUser)
        {
            Jc.Rpc<TraceRemote>.Ge.SetAppUser(appUser);
        }

        public static void RegisterFileLogger(this ITraceExpander expander)
        {
            InitializeFileLogger(FileLogger);
            Jc.Rpc<TraceRemote>.Ge.RegisterLogger(NlogConfig.GetMainTraceName());
        }

        public static void RegisterFileLogger<T>(this ITraceExpander expander) where T : Jc.Ticket
        {
            InitializeFileLogger(FileLogger);
            Jc.Rpc<TraceRemote>.Ge.RegisterLogger((typeof(T)).Name);
        }

        public static void Write(this ITraceExpander expander, object message)
        {
            InitializeFileLogger(FileLogger);
            var ticket = new Ticket();
            var jce = message as f.Exception;
            if (jce != null)
            {
                WriteTicket(ticket, true, jce.Text, jce.Description);
                return;
            }
            var exception = message as Exception;
            if (exception != null)
            {
                var ex = new f.Exception(exception);
                WriteTicket(ticket, true, ex.Text, ex.Description);
                return;
            }
            var msgTicket = message as Ticket;
            if (msgTicket != null)
            {
                WriteTicket(ticket, msgTicket.IsException, msgTicket.Text, msgTicket.Description);
                return;
            }
            var text = message != null && message != DBNull.Value ? message.ToString() : null;
            WriteTicket(ticket, false, text, text);
        }

        public static void Write<T>(this ITraceExpander expander, object message) where T : Ticket, new()
        {
            InitializeFileLogger(FileLogger);
            var ticket = new T();
            var jce = message as f.Exception;
            if (jce != null)
            {
                WriteTicket(ticket, true, jce.Text, jce.Description);
                return;
            }
            var exception = message as Exception;
            if (exception != null)
            {
                var ex = new f.Exception(exception);
                WriteTicket(ticket, true, ex.Text, ex.Description);
                return;
            }
            var msgTicket = message as Ticket;
            if (msgTicket != null)
            {
                WriteTicket(ticket, msgTicket.IsException, msgTicket.Text, msgTicket.Description);
                return;
            }
            var text = message != null && message != DBNull.Value ? message.ToString() : null;
            WriteTicket(ticket, false, text, text);
        }

        private static void WriteTicket(Ticket ticket, bool isException, string text, string description)
        {
            ticket.IsException = isException;
            ticket.Text = text;
            ticket.Description = description;
            if (!string.IsNullOrWhiteSpace(ticket.Text) || !string.IsNullOrWhiteSpace(ticket.Description)) System.Diagnostics.Trace.WriteLine(ticket);
        }

        public static void WriteToFileLogger(string loggerName, Ticket ticket)
        {
            Jc.Rpc<TraceRemote>.Ge.Write(loggerName, ticket);
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