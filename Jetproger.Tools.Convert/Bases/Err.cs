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
        public static void GuardNull(this Je.IErrExpander exp, Func<bool> isError, string paramName)
        {
            if (isError()) throw new ArgumentNullException(paramName);
        }

        public static void GuardNull(this Je.IErrExpander exp, bool isError, string paramName)
        {
            if (isError) throw new ArgumentNullException(paramName);
        }

        public static void GuardArg(this Je.IErrExpander exp, Func<bool> isError, string paramName)
        {
            if (isError()) throw new ArgumentException(paramName);
        }

        public static void GuardArg(this Je.IErrExpander exp, bool isError, string paramName)
        {
            if (isError) throw new ArgumentException(paramName);
        }

        public static void GuardRange(this Je.IErrExpander exp, Func<bool> isError, string paramName)
        {
            if (isError()) throw new ArgumentOutOfRangeException(paramName);
        }

        public static void GuardRange(this Je.IErrExpander exp, bool isError,  string paramName)
        {
            if (isError) throw new ArgumentOutOfRangeException(paramName);
        }

        public static void GuardTypeNotFound(this Je.IErrExpander exp, Func<bool> isError, string typeName)
        {
            if (isError()) throw new TypeNotFoundException(typeName);
        }

        public static void GuardTypeNotFound(this Je.IErrExpander exp, bool isError, string typeName)
        {
            if (isError) throw new TypeNotFoundException(typeName);
        }

        public static void GuardTypeNotSubtype(this Je.IErrExpander exp, Func<bool> isError, string type, string baseType)
        {
            if (isError()) throw new TypeNotSubtypeException(type, baseType);
        }

        public static void GuardTypeNotSubtype(this Je.IErrExpander exp, bool isError, string type, string baseType)
        {
            if (isError) throw new TypeNotSubtypeException(type, baseType);
        }

        public static void GuardCommandNotDefined(this Je.IErrExpander exp, Func<bool> isError, Guid session)
        {
            if (isError()) throw new CommandNotDefinedException(session);
        }

        public static void GuardCommandNotDefined(this Je.IErrExpander exp, bool isError, Guid session)
        {
            if (isError) throw new CommandNotDefinedException(session);
        }

        public static void Guard<T>(this Je.IErrExpander exp, Func<bool> isError) where T : Exception, new()
        {
            if (isError()) throw new T();
        }

        public static void Guard<T>(this Je.IErrExpander exp, bool isError) where T : Exception, new()
        {
            if (isError) throw new T();
        }

        public static void Guard(this Je.IErrExpander exp, Func<bool> isError, Exception exception)
        {
            if (isError()) throw exception;
        }

        public static void Guard(this Je.IErrExpander exp, bool isError, Exception exception)
        {
            if (isError) throw exception;
        }
    }


    public class TypeNotFoundException : Exception { public TypeNotFoundException(string typeName) : base(string.Format(J_<TypeNotFoundName>.Sz, typeName)) { } }
    public class TypeNotSubtypeException : Exception { public TypeNotSubtypeException(string type, string baseType) : base(string.Format(J_<TypeNotSubtypeName>.Sz, type, baseType)) { } }
    public class AppConfigAppHostException : Exception { public AppConfigAppHostException() : base(J_<AppConfigAppHostName>.Sz) { } }
    public class CommandNotDefinedException : Exception { public CommandNotDefinedException(Guid session) : base(string.Format(J_<CommandNotDefinedName>.Sz, session)) { } }
    public class MssqlCommandException : Exception { public MssqlCommandException(string command, string error) : base(string.Format(J_<MssqlCommandName>.Sz, command, error)) { } }
    public class CertificateStoreException : Exception { public CertificateStoreException(string thumbprint, string comment) : base(string.Format(J_<CertificateStoreName>.Sz, comment, thumbprint)) { } }
    public class CertificateKeyException : Exception { public CertificateKeyException(string thumbprint, string comment) : base(string.Format(J_<CertificateKeyName>.Sz, comment, thumbprint)) { } }
    public class AppPortNotSpecifiedException : Exception { public AppPortNotSpecifiedException() : base(J_<AppPortNotSpecifiedName>.Sz) { } }
}