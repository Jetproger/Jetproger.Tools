using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace Jetproger.Tools.Trace.Bases
{
    [LayoutRenderer("AppUser")]
    public class AppUserLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder sb, LogEventInfo logEvent)
        {
            sb.Append(!string.IsNullOrWhiteSpace(TraceCore.AppUser) ? TraceCore.AppUser : "<undefined>");
        }
    }
}