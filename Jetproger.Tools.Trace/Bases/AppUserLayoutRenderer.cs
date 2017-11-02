using System.Text;
using NLog;
using NLog.LayoutRenderers;
using Log = Tools.Trace;

namespace Jetproger.Tools.Trace.Bases
{
    [LayoutRenderer("AppUser")]
    public class AppUserLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder sb, LogEventInfo logEvent)
        {
            sb.Append(!string.IsNullOrWhiteSpace(Log.AppUser) ? Log.AppUser : "<undefined>");
        }
    }
}