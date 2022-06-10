using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Jetproger.Tools.AppResource;
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

        public static CommandMessage ErrToMsg(this Je.IErrExpander exp, Exception e)
        {
            return e != null ? new CommandMessage { Category = ECommandMessage.Error, Message = e.Message, Comment = ErrToStr(exp, e) } : null;
        }

        public static Exception MsgToErr(this Je.IErrExpander exp, CommandMessage msg)
        {
            return msg.Category == ECommandMessage.Error ? new Exception(msg.Message) : null;
        }

        public static Exception MsgToErr(this Je.IErrExpander exp, IEnumerable<CommandMessage> messages)
        {
            var msg = FirstErrOf(exp, messages);
            return msg != null ? new Exception(msg.Message) : null;
        }

        public static string ErrToStr(this Je.IErrExpander exp, Exception exception)
        {
            exception = FirstErrOf(exp, exception);
            var msg = WebToStr(exp, exception as WebException);
            if (msg != null) return msg;
            return StackOf(exp, exception);
        }

        public static string WebToStr(this Je.IErrExpander exp, WebException we)
        {
            if (we == null) return null;
            if (we.Response == null) return "Response is empty";
            var responseStream = we.Response.GetResponseStream();
            if (responseStream == null) return "Response stream unavailable";
            using (var sr = new StreamReader(responseStream)) { return sr.ReadToEnd(); }
        }

        public static string StackOf(this Je.IErrExpander exp, Exception e)
        {
            var sb = new StringBuilder();
            sb.AppendLine(e.ToString());
            while (e != null)
            {
                sb.AppendLine(e.Message);
                e = e.InnerException;
            }
            return sb.ToString();
        }

        public static Exception FirstErrOf(this Je.IErrExpander exp, object obj)
        {
            var ae = obj as AggregateException;
            if (ae == null) return obj as Exception;
            return ae.InnerExceptions.Count > 0 ? ae.InnerExceptions[0].InnerException ?? ae.InnerExceptions[0] : null;
        }

        public static CommandMessage FirstErrOf(this Je.IErrExpander exp, IEnumerable<CommandMessage> messages)
        {
            return (messages ?? new CommandMessage[0]).FirstOrDefault(x => x.Category == ECommandMessage.Error);
        }

        public static CommandMessage LastErrOf(this Je.IErrExpander exp, IEnumerable<CommandMessage> messages)
        {
            var error = (CommandMessage)null;
            foreach (CommandMessage message in (messages ?? new CommandMessage[0]))
            {
                if (message.Category == ECommandMessage.Error) error = message;
            }
            return error;
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