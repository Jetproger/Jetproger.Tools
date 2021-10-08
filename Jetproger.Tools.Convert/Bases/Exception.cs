using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Je
    {
        public class Exception : System.Exception
        {
            private readonly string _webExceptionString;
            private readonly System.Exception _exception;
            private string _description;
            private string _text;

            public Exception(Ticket ticket)
            {
                _exception = new System.Exception(ticket.Message);
                _description = ticket.Comment;
                _text = ticket.Message;
            }

            public Exception(string errorMessage) : this(new System.Exception(errorMessage))
            {

            }

            public Exception() : this((Exception)null)
            {

            }

            public Exception(System.Exception exception)
            {
                var jce = exception as Exception;
                if (jce != null)
                {
                    _exception = jce.GetException();
                    return;
                }
                _exception = FindException(exception);
                _webExceptionString = ReadWebExceptionString();
            }

            public override string Message
            {
                get { return string.Empty; }
            }

            public override IDictionary Data
            {
                get { return _exception != null ? _exception.Data : base.Data; }
            }

            public override string Source
            {
                get { return _exception != null ? _exception.Source : base.Source; }
                set { if (_exception != null) _exception.Source = value; else base.Source = value; }
            }

            public override string StackTrace
            {
                get { return _exception != null ? _exception.StackTrace : base.StackTrace; }
            }

            public override string HelpLink
            {
                get { return _exception != null ? _exception.HelpLink : base.HelpLink; }
                set { if (_exception != null) _exception.HelpLink = value; else base.HelpLink = value; }
            }

            public override System.Exception GetBaseException()
            {
                return _exception != null ? _exception.GetBaseException() : base.GetBaseException();
            }

            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if (_exception != null) _exception.GetObjectData(info, context); else base.GetObjectData(info, context);
            }

            public override bool Equals(object obj)
            {
                return _exception != null ? _exception.Equals(obj) : base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return _exception != null ? _exception.GetHashCode() : base.GetHashCode();
            }

            public override string ToString()
            {
                return string.Empty;
            }

            public System.Exception GetException()
            {
                return _exception;
            }

            public bool IsEmpty
            {
                get { return _exception == null; }
            }

            public string Text
            {
                get { return _text ?? (_text = _exception != null ? _exception.Message : string.Empty); }
            }

            public string Description
            {
                get
                {
                    if (_description == null)
                    {
                        if (_description == null)
                        {
                            if (_exception == null) _description = string.Empty;
                        }
                        if (_description == null)
                        {
                            var we = _exception as WebException;
                            if (we != null) _description = _webExceptionString;
                        }
                        if (_description == null)
                        {
                            var e = _exception;
                            var sb = new StringBuilder();
                            sb.AppendLine(e.ToString());
                            while (e != null)
                            {
                                sb.AppendLine(e.Message);
                                e = e.InnerException;
                            }
                            _description = sb.ToString();
                        }
                    }
                    return _description;
                }
            }

            private static System.Exception FindException(object obj)
            {
                var ae = obj as AggregateException;
                if (ae == null) return obj as Exception;
                if (ae.InnerExceptions.Count > 0)
                {
                    return ae.InnerExceptions[0].InnerException ?? ae.InnerExceptions[0];
                }
                return null;
            }

            private string ReadWebExceptionString()
            {
                var we = _exception as WebException;
                if (we != null && we.Response != null)
                {
                    var stream = we.Response.GetResponseStream();
                    if (stream != null) return (new StreamReader(stream)).ReadToEnd();
                }
                return null;
            }
        }
    }
}