using System;
using System.Diagnostics;
using Jetproger.Tools.Cache.Bases;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Injection.Bases;
using Microsoft.Practices.Unity.InterceptionExtension;

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
                return Ex.Inject.AOPExecute(input, getNext);
            }
            var methodName = Ex.Inject.AOPProfile(input);
            var watch = new Stopwatch();
            IMethodReturn result = null;
            watch.Start();
            try
            {
                result = Ex.Inject.AOPExecute(input, getNext);
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
                var typedMessage = Ex.Dotnet.CreateInstance(TraceType, new object[] { durationMessage, null }) as ExTicket;
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