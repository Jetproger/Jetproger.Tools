using System;
using Jetproger.Tools.Cache.Bases;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Plugin.DI;
using Jetproger.Tools.Trace.Bases;
using Microsoft.Practices.Unity.InterceptionExtension;
using TDI = Tools.DI;

namespace Jetproger.Tools.Plugin.Aspects
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
                return TDI.AOPExecute(input, getNext);
            }
            var methodName = TDI.AOPProfile(input);
            IMethodReturn result;
            try
            {
                result = TDI.AOPExecute(input, getNext);
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
                var typedMessage = Ex.Dotnet.CreateInstance(TraceType, new object[] { null, e }) as TypedMessage;
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

    public class ExceptionAspectAttribute : AspectAttribute
    {
        public ExceptionAspectAttribute() : base(typeof (ExceptionCallHandler)) { }

        public string TraceType { get; set; }
    }
}