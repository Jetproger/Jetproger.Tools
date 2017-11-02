using System;
using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace Jetproger.Tools.Trace.Bases
{
    [LayoutRenderer("Computer")]
    public class ComputerLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder sb, LogEventInfo logEvent)
        {
            sb.Append(Environment.MachineName);
        }
    }
}