using System;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Inject.Bases;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Jetproger.Tools.Process.Aspects
{
    public class SecurityCallHandler : ICallHandler
    {
        public string PermissionCode { get; set; }
        public bool FullOnly { get; set; }
        public bool Enabled { get; set; }
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (!Enabled || string.IsNullOrWhiteSpace(PermissionCode)) return Ex.Inject.Call(input, getNext);
            //var access = CoreMethods.SecurityContext.Access(PermissionCode);
            //if (FullOnly && access != EAccess.Full) access = EAccess.None;
            //if (access == EAccess.None) throw new Exception(CoreMethods.Enum.AsString(access));
            return Ex.Inject.Call(input, getNext);
        }
    }
}