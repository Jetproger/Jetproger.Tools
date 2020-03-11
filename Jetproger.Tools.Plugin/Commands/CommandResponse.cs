using System;
using System.Text;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Plugin.Commands
{
    public class CommandResponse
    {
        public CommandResponse()
        {

        }

        public CommandResponse(Exception e)
        {
            if (e == null) return;
            Error = e.Message;
            ErrorNote = e.As<string>();
        }

        public Guid SessionId { get; set; }
        public string Source { get; set; }
        public string Json { get; set; }
        public string Error { get; set; }
        public string ErrorNote { get; set; }
        public string[] Messages { get; set; }

        public bool IsFinished { get; set; }
        public bool IsJson => !string.IsNullOrWhiteSpace(Json);
        public bool IsError => !string.IsNullOrWhiteSpace(Error);
        public bool IsMessage => Messages != null && Messages.Length > 0;

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (IsMessage)
            {
                sb.AppendLine("Messages:");
                foreach (var message in Messages)
                {
                    sb.AppendLine("  " + message);
                }
            }
            if (IsError)
            {
                sb.AppendLine("Error:");
                if (sb.Length > 0) sb.AppendLine();
                sb.AppendLine(Error);
                sb.AppendLine();
                sb.AppendLine(ErrorNote);
            }
            return sb.Length > 0 ? $"Source: {Source}{Environment.NewLine}{sb}" : string.Empty;
        }
    }
}