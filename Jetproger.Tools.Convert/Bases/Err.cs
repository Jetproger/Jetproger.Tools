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
    public static class ErrorExtensions
    {
        public static void GuardNull(this f.IErrExpander exp, Func<bool> isError, string paramName)
        {
            if (isError()) throw new ArgumentNullException(paramName);
        }

        public static void GuardNull(this f.IErrExpander exp, bool isError, string paramName)
        {
            if (isError) throw new ArgumentNullException(paramName);
        }

        public static void GuardArg(this f.IErrExpander exp, Func<bool> isError, string paramName)
        {
            if (isError()) throw new ArgumentException(paramName);
        }

        public static void GuardArg(this f.IErrExpander exp, bool isError, string paramName)
        {
            if (isError) throw new ArgumentException(paramName);
        }

        public static void GuardRange(this f.IErrExpander exp, Func<bool> isError, string paramName)
        {
            if (isError()) throw new ArgumentOutOfRangeException(paramName);
        }

        public static void GuardRange(this f.IErrExpander exp, bool isError,  string paramName)
        {
            if (isError) throw new ArgumentOutOfRangeException(paramName);
        }

        public static void GuardTypeNotFound(this f.IErrExpander exp, Func<bool> isError, string typeName)
        {
            if (isError()) throw new TypeNotFoundException(typeName);
        }

        public static void GuardTypeNotFound(this f.IErrExpander exp, bool isError, string typeName)
        {
            if (isError) throw new TypeNotFoundException(typeName);
        }

        public static void GuardTypeNotSubtype(this f.IErrExpander exp, Func<bool> isError, string type, string baseType)
        {
            if (isError()) throw new TypeNotSubtypeException(type, baseType);
        }

        public static void GuardTypeNotSubtype(this f.IErrExpander exp, bool isError, string type, string baseType)
        {
            if (isError) throw new TypeNotSubtypeException(type, baseType);
        }

        public static void GuardCommandNotDefined(this f.IErrExpander exp, Func<bool> isError, Guid session)
        {
            if (isError()) throw new CommandNotDefinedException(session);
        }

        public static void GuardCommandNotDefined(this f.IErrExpander exp, bool isError, Guid session)
        {
            if (isError) throw new CommandNotDefinedException(session);
        }

        public static void Guard<T>(this f.IErrExpander exp, Func<bool> isError) where T : Exception, new()
        {
            if (isError()) throw new T();
        }

        public static void Guard<T>(this f.IErrExpander exp, bool isError) where T : Exception, new()
        {
            if (isError) throw new T();
        }

        public static void Guard(this f.IErrExpander exp, Func<bool> isError, Exception exception)
        {
            if (isError()) throw exception;
        }

        public static void Guard(this f.IErrExpander exp, bool isError, Exception exception)
        {
            if (isError) throw exception;
        }
    }


    public class TypeNotFoundException : Exception { public TypeNotFoundException(string typeName) : base(string.Format(k<TypeNotFoundName>.key, typeName)) { } }
    public class TypeNotSubtypeException : Exception { public TypeNotSubtypeException(string type, string baseType) : base(string.Format(k<TypeNotSubtypeName>.key, type, baseType)) { } }
    public class AppConfigAppHostException : Exception { public AppConfigAppHostException() : base(k<AppConfigAppHostName>.key) { } }
    public class CommandNotDefinedException : Exception { public CommandNotDefinedException(Guid session) : base(string.Format(k<CommandNotDefinedName>.key, session)) { } }
    public class MssqlCommandException : Exception { public MssqlCommandException(string command, string error) : base(string.Format(k<MssqlCommandName>.key, command, error)) { } }
    public class CertificateStoreException : Exception { public CertificateStoreException(string thumbprint, string comment) : base(string.Format(k<CertificateStoreName>.key, comment, thumbprint)) { } }
    public class CertificateKeyException : Exception { public CertificateKeyException(string thumbprint, string comment) : base(string.Format(k<CertificateKeyName>.key, comment, thumbprint)) { } }
    public class AppPortNotSpecifiedException : Exception { public AppPortNotSpecifiedException() : base(k<AppPortNotSpecifiedName>.key) { } }
}