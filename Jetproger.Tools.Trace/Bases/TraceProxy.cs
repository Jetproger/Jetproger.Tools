using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Trace.Bases
{
    public class TraceProxy : OneProxy
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