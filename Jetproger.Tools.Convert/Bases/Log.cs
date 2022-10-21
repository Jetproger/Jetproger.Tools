using System;
using System.Diagnostics;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Bases
{
    public static class LogExtensions
    {
        public static void To(this Je.ILogExpander exp, object message)
        {
            if (message == null) return;
            var s = message.ToString();
            if (string.IsNullOrWhiteSpace(s)) return;
            var holder = (message as LogHolder) ?? new LogHolder(message, new StackTrace());
            Trace.WriteLine(holder);
        }

        public static CommandMessage[] Of(this Je.ILogExpander exp, ICommand command)
        {
            return command != null ? command.LogExecute() : new CommandMessage[0];
        }

        public static void RegisterTracer(this Je.ILogExpander exp, TraceListener tracer)
        {
            if (tracer == null) return;
            var i = IndexOfListeners(tracer);
            if (i >= 0) return;
            lock (Trace.Listeners) { Trace.Listeners.Add(tracer); }
        }

        public static void UnregisterTracer(this Je.ILogExpander exp, TraceListener tracer)
        {
            var i = IndexOfListeners(tracer);
            if (i < 0) return;
            lock (Trace.Listeners) { Trace.Listeners.RemoveAt(i); }
        }

        private static int IndexOfListeners(TraceListener tracer)
        {
            lock (Trace.Listeners)
            {
                for (int i = 0; i < Trace.Listeners.Count; i++)
                {
                    if (ReferenceEquals(tracer, Trace.Listeners[i])) return i;
                }
                return -1;
            }
        }
    }

    [Serializable]
    public class LogHolder
    {
        public readonly StackTrace Stack;
        public readonly object Message;

        public LogHolder(object message, StackTrace stack)
        {
            Message = message;
            Stack = stack;
        }

        public override string ToString()
        {
            return Message != null ? Message.ToString() : base.ToString();
        }
    }
}