using System;
using System.Collections.Generic;
using System.Diagnostics;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Traces
{
    public static class TraceExtensions
    {
        private static readonly TraceContext Context = t<TraceContext>.one();

        public static void Log(StackTrace stack, Exception e)
        { 
            Context.Write(TypesOf(stack), e.As<CommandMessage>().Info, "error");
        }

        public static void Log(StackTrace stack, CommandMessage m)
        {
            Context.Write(TypesOf(stack), m.Message, m.Category);
        }

        public static void Log(StackTrace stack, string s)
        {
            Context.Write(TypesOf(stack), s, "trace");
        }

        private static Type[] TypesOf(StackTrace stack)
        {
            var types = new List<Type>();
            for (int i = 0; i < stack.FrameCount; i++)
            {
                var type = stack.GetFrame(i).GetMethod().DeclaringType;
                if (type != null) types.Add(type);
            }
            return types.ToArray();
        }
    }
}