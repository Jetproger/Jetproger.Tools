using System;
using System.IO;
using System.Net;
using System.Text;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commanders
{
    public class CommandException : Exception
    {
        public override string Message { get { return !string.IsNullOrWhiteSpace(_message) ? _message : base.Message; } } 
        private readonly string _message;

        public CommandException(string message)
        {
            _message = message;
        }

        public CommandException(CommandMessage commandMessage)
        {
            _message = commandMessage.Message;
        }

        public CommandException(Exception exception)
        { 
            var commandException = exception as CommandException;
            if (commandException != null)
            {
                _message = commandException._message;
                return;
            }
            exception = FirstExceptionOf(exception);
            _message = exception.As<string>();
        }

        public override string ToString()
        {
            return Message;
        }

        private static Exception FirstExceptionOf(object obj)
        {
            var ae = obj as AggregateException;
            if (ae == null) return obj as Exception;
            return ae.InnerExceptions.Count > 0 ? ae.InnerExceptions[0].InnerException ?? ae.InnerExceptions[0] : null;
        }
    }

    public class TypeNotFoundException : Exception { public TypeNotFoundException(string typeName) : base(string.Format(@"Не найден тип ""{0}""", typeName)) { } }
    public class TypeNotSubtypeException : Exception { public TypeNotSubtypeException(string type, string baseType) : base(string.Format(@"Тип ""{0}"" не является подтипом ""{1}""", type, baseType)) { } }
    public class CommandNotDefinedException : Exception { public CommandNotDefinedException(Guid session) : base(string.Format("Сессия [{0}]: не определена команда для выполнения(CommandRequest.Command)", session)) { } }
    public class MssqlCommandException : Exception { public MssqlCommandException(string command, string error) : base(string.Format("{0}: {1}", command, error)) { } }
    public class CertificateStoreException : Exception { public CertificateStoreException(string thumbprint, string comment) : base(string.Format("Не найден сертификат {0}=[{1}] в хранилище текущего пользователя", comment, thumbprint)) { } }
    public class CertificateKeyException : Exception { public CertificateKeyException(string thumbprint, string comment) : base(string.Format("Не найден ключевой носитель для сертификата {0}=[{1}]", comment, thumbprint)) { } }
}