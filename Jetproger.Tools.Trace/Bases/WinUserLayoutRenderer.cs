using System;
using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace Jetproger.Tools.Trace.Bases
{
    [LayoutRenderer("WinUser")]
    public class WinUserLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder sb, LogEventInfo logEvent)
        {
            var userDomainName = Environment.UserDomainName;
            userDomainName = userDomainName + (!string.IsNullOrWhiteSpace(userDomainName) ? @"\" : "");
            var value = $@"{userDomainName}{Environment.UserName}";
            sb.Append(value);
        }
    }
}