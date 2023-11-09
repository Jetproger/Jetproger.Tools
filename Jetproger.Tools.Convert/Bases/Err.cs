using System;
using Jetproger.Tools.AppResource;

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

    [Serializable]
    public class CommandNotDefinedException : Exception
    {
        public CommandNotDefinedException() : base(nameof(CommandNotDefinedException)) { }

        public static bool IsCommandNotDefined(Exception e)
        {
            if (e == null) return false;
            if (e is CommandNotDefinedException) return true;
            return e.Message != null && e.Message.Contains(nameof(CommandNotDefinedException));
        }
    }

    [Serializable]public class ContainerNotFoundException : Exception { public ContainerNotFoundException(string containerId) : base(string.Format(k<ContainerNotFoundName>.key(), containerId)) { } }
    [Serializable]public class TypeNotFoundException : Exception { public TypeNotFoundException(string typeName) : base(string.Format(k<TypeNotFoundName>.key(), typeName)) { } }
    [Serializable]public class TypeNotSubtypeException : Exception { public TypeNotSubtypeException(string type, string baseType) : base(string.Format(k<TypeNotSubtypeName>.key(), type, baseType)) { } }
    [Serializable]public class AppConfigAppHostException : Exception { public AppConfigAppHostException() : base(k<AppConfigAppHostName>.key()) { } }
    [Serializable]public class MssqlCommandException : Exception { public MssqlCommandException(string command, string error) : base(string.Format(k<MssqlCommandName>.key(), command, error)) { } }
    [Serializable]public class CertificateStoreException : Exception { public CertificateStoreException(string thumbprint, string comment) : base(string.Format(k<CertificateStoreName>.key(), comment, thumbprint)) { } }
    [Serializable]public class CertificateKeyException : Exception { public CertificateKeyException(string thumbprint, string comment) : base(string.Format(k<CertificateKeyName>.key(), comment, thumbprint)) { } }
    [Serializable] public class AppPortNotSpecifiedException : Exception { public AppPortNotSpecifiedException() : base(k<AppPortNotSpecifiedName>.key()) { } }
}