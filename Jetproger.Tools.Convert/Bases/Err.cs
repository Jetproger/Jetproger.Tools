using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Jetproger.Tools.AppResource;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class f
    { 
        public static void err<T>(Func<bool> isError, params object[] args) where T : Exception
        {
            if (isError()) throw (T)Activator.CreateInstance(typeof(T), args);
        }

        public static void err<T>(bool isError, params object[] args) where T : Exception
        {
            if (isError) throw (T)Activator.CreateInstance(typeof(T), args);
        }

        public static void err(Func<bool> isError, Exception exception)
        {
            if (isError()) throw exception;
        }

        public static void err(bool isError, Exception exception)
        {
            if (isError) throw exception;
        }
    }

    public class ContainerNotFoundException : Exception { public ContainerNotFoundException(string containerId) : base(string.Format(k<ContainerNotFoundName>.As<string>(), containerId)) { } }
    public class TypeNotFoundException : Exception { public TypeNotFoundException(string typeName) : base(string.Format(k<TypeNotFoundName>.As<string>(), typeName)) { } }
    public class TypeNotSubtypeException : Exception { public TypeNotSubtypeException(string type, string baseType) : base(string.Format(k<TypeNotSubtypeName>.As<string>(), type, baseType)) { } }
    public class AppConfigAppHostException : Exception { public AppConfigAppHostException() : base(k<AppConfigAppHostName>.As<string>()) { } }
    public class CommandNotDefinedException : Exception { public CommandNotDefinedException(Guid session) : base(string.Format(k<CommandNotDefinedName>.As<string>(), session)) { } }
    public class MssqlCommandException : Exception { public MssqlCommandException(string command, string error) : base(string.Format(k<MssqlCommandName>.As<string>(), command, error)) { } }
    public class CertificateStoreException : Exception { public CertificateStoreException(string thumbprint, string comment) : base(string.Format(k<CertificateStoreName>.As<string>(), comment, thumbprint)) { } }
    public class CertificateKeyException : Exception { public CertificateKeyException(string thumbprint, string comment) : base(string.Format(k<CertificateKeyName>.As<string>(), comment, thumbprint)) { } }
    public class AppPortNotSpecifiedException : Exception { public AppPortNotSpecifiedException() : base(k<AppPortNotSpecifiedName>.As<string>()) { } }
}