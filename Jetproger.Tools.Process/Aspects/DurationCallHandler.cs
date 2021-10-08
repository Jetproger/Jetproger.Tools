using System;
using System.Diagnostics;
using Jetproger.Tools.Cache.Bases;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Inject.Bases;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Jetproger.Tools.Process.Aspects
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
                return Ex.Inject.Call(input, getNext);
            }
            var methodName = Ex.Inject.InfoEx(input);
            var watch = new Stopwatch();
            IMethodReturn result = null;
            watch.Start();
            try
            {
                result = Ex.Inject.Call(input, getNext);
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
                var typedMessage = Ex.Dotnet.CreateInstance(TraceType, new object[] { durationMessage, null }) as Jc.Ticket;
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

    public class DurationAspectAttribute : UnityAspectAttribute
    {
        public DurationAspectAttribute() : base(typeof(DurationCallHandler)) { }

        public string TraceType { get; set; }
    }
}