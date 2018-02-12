using System;
using System.Diagnostics;
using Microsoft.Practices.Unity.InterceptionExtension;
using TDI = Tools.DI;

namespace Jetproger.Tools.Plugin.Aspects
{
    public class DurationCallHandler : ICallHandler
    {
        public string TraceType { get; set; }
        public bool Enabled { get; set; }
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (!Enabled)
            {
                return TDI.AOPExecute(input, getNext);
            }
            var methodName = TDI.AOPProfile(input);
            var watch = new Stopwatch();
            IMethodReturn result = null;
            watch.Start();
            try
            {
                result = TDI.AOPExecute(input, getNext);
                return result;
            }
            finally
            {
                watch.Stop();
                System.Diagnostics.Trace.WriteLine($"{methodName}: {result}{Environment.NewLine}Duration: {watch.ElapsedMilliseconds} мс{Environment.NewLine}");
            }
        }
    }
}