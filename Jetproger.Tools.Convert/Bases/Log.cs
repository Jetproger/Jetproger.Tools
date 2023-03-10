using System;
using System.Diagnostics;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class f // for configure use BaseCommand inheritors
    {
        public static void log(Exception exception)
        {
            if (exception != null) Trace.WriteLine(new LogHolder(new StackTrace(), exception));
        }

        public static void log(string message)
        {
            if (!string.IsNullOrWhiteSpace(message)) Trace.WriteLine(new LogHolder(new StackTrace(), message));
        }
    }

    [Serializable]
    public class LogHolder
    {
        public override string ToString() { return Message; }
        public readonly StackTrace Stack;
        public readonly string Message;
        public readonly bool IsError;
        public LogHolder(StackTrace stack, object message)
        {
            Stack = stack;
            IsError = message is Exception;
            Message = IsError ? (new CommandMessage(message as Exception)).Info : message.ToString();
        }
    }
}