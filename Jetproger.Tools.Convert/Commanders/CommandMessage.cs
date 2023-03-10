using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text; 

namespace Jetproger.Tools.Convert.Commanders
{
    [Serializable, DataContract]
    public class CommandMessage
    {
        [DataMember] public string Category { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public string Info { get; set; }
        [DataMember] public string Status { get; set; }

        public CommandMessage()
        {
            Category = ECommandMessage.Trace.ToString();
        }

        public CommandMessage(string message)
        {
            Info = message;
            Message = message;
            Category = ECommandMessage.Trace.ToString();
        }

        public CommandMessage(string message, ECommandMessage category)
        {
            Info = message;
            Message = message;
            Category = category.ToString();
        }

        public CommandMessage(Exception e)
        {
            Category = ECommandMessage.Error.ToString();
            Message = e.Message;
            e = GetFirstException(e);
            Info = WebExceptionAsString(e as WebException) ?? ExceptionAsString(e);
            Status = e.Data.Contains("CommandMessageStatus") && e.Data["CommandMessageStatus"] != null ? e.Data["CommandMessageStatus"].ToString() : Status;
        }

        private static Exception GetFirstException(object obj)
        {
            var ae = obj as AggregateException;
            if (ae == null) return obj as Exception;
            return ae.InnerExceptions.Count > 0 ? ae.InnerExceptions[0].InnerException ?? ae.InnerExceptions[0] : null;
        }

        private string WebExceptionAsString(WebException we)
        {
            if (we == null) return null;
            if (we.Response == null) return we.ToString();
            Status = we.Response is HttpWebResponse httpWebResponse ? ((int)httpWebResponse.StatusCode).ToString() : Status;
            var responseStream = we.Response.GetResponseStream();
            if (responseStream == null) return we.ToString();
            using (var sr = new StreamReader(responseStream))
            {
                var s = sr.ReadToEnd();
                return !string.IsNullOrWhiteSpace(s) ? s : we.ToString();
            }
        }

        private static string ExceptionAsString(Exception e)
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
    }

    public enum ECommandMessage { Trace, Debug, Info, Warn, Error }
}