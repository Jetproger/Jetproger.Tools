using System;
using Jetproger.Tools.Cache.Bases;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Inject.Bases;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Jetproger.Tools.Process.Aspects
{
    public class ExceptionCallHandler : ICallHandler
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
            IMethodReturn result;
            try
            {
                result = Ex.Inject.Call(input, getNext);
                return result;
            }
            catch (Exception e)
            {
                Write(e, methodName);
                result = input.CreateMethodReturn(null);
                return result;
            }
        }

        private void Write(Exception e, string methodName)
        {
            try
            {
                if (WriteTyped(e)) return;
                System.Diagnostics.Trace.WriteLine($"{methodName}:{Environment.NewLine}{e.Message}{Environment.NewLine}{e.As<string>()}{Environment.NewLine}");
            }
            catch
            {
                System.Threading.Thread.Sleep(111);
            }
        }

        private bool WriteTyped(Exception e)
        {
            try
            {
                var typedMessage = Ex.Dotnet.CreateInstance(TraceType, new object[] { null, e }) as Jc.Ticket;
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

    public class ExceptionAspectAttribute : UnityAspectAttribute
    {
        public ExceptionAspectAttribute() : base(typeof (ExceptionCallHandler)) { }

        public string TraceType { get; set; }
    }
}