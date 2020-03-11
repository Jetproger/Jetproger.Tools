using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Trace.Bases;

namespace je
{
    public class TraceRemote : RemoteCaller
    {
        public void SetAppUser(string appUser)
        {
            TraceCore.SetAppUser(appUser);
        }

        public void RegisterLogger(string loggerName)
        {
            TraceCore.RegisterLogger(loggerName);
        }

        public void Write(string loggerName, ExTicket ticket)
        {
            TraceCore.Write(loggerName, ticket);
        }
    }
}