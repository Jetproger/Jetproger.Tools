using System;
using System.Diagnostics;
using Jetproger.Tools.Cache.Bases;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Plugin.DI;
using Jetproger.Tools.Trace.Bases;
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
                Write($"{methodName}: {result}{Environment.NewLine}Duration: {watch.ElapsedMilliseconds} мс{Environment.NewLine}");
            }
        }

        private void Write(string durationMessage)
        {
            try
            {
                if (!WriteTyped(durationMessage)) System.Diagnostics.Trace.WriteLine(durationMessage);
            }
            catch
            {
                System.Threading.Thread.Sleep(111);
            }
        }

        private bool WriteTyped(string durationMessage)
        {
            try
            {
                var typedMessage = Ex.Dotnet.CreateInstance(TraceType, new object[] { durationMessage, null }) as TypedMessage;
                if (typedMessage == null) return false;
                System.Diagnostics.Trace.WriteLine(typedMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class DurationAspectAttribute : AspectAttribute
    {
        public DurationAspectAttribute() : base(typeof(DurationCallHandler)) { }

        public string TraceType { get; set; }
    }
}