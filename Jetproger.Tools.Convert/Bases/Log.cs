using System;
using System.Diagnostics;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Traces;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class f
    {
        public static void log(Exception e)
        {
            if (e != null) TraceExtensions.Log(new StackTrace(), e);
        }

        public static void log(CommandMessage m)
        {
            if (m != null) TraceExtensions.Log(new StackTrace(), m);
        }

        public static void log(string s)
        {
            if (!string.IsNullOrWhiteSpace(s)) TraceExtensions.Log(new StackTrace(), s);
        }
    }
}