using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Trace.Bases
{
    public class TraceRemote : Jc.RemoteCaller
    {
        public void SetAppUser(string appUser)
        {
            TraceCore.SetAppUser(appUser);
        }

        public void RegisterLogger(string loggerName)
        {
            TraceCore.RegisterLogger(loggerName);
        }

        public void Write(string loggerName, Jc.Ticket ticket)
        {
            TraceCore.Write(loggerName, ticket);
        }
    }
}