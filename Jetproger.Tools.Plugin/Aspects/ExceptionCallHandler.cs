using System;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Trace.Bases;
using Microsoft.Practices.Unity.InterceptionExtension;
using TDI = Tools.DI;
using MD = Tools.Metadata;

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
                var typedMessage = MD.CreateInstance(TraceType, new object[] { null, e }) as TypedMessage;
                if (typedMessage != null) System.Diagnostics.Trace.WriteLine(typedMessage.Error);
                System.Diagnostics.Trace.WriteLine($"{methodName}:{Environment.NewLine}{e.Message}{Environment.NewLine}{e.AsString()}{Environment.NewLine}");
                result = input.CreateMethodReturn(null);
                return result;
            }
        }
    }
}