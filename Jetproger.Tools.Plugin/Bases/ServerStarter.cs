using Jetproger.Tools.Cache.Bases;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Trace.Bases;

namespace Jetproger.Tools.Plugin.Bases
{
    public class ServerStarter : Starter
    {
        public override void RegisterSettings(string[] args)
        {
            Ex.Dotnet.RegisterSettings(args);
        }

        protected override void RegisterLoggerContinue<T>()
        {
            Ex.Trace.RegisterFileLogger<T>();
        }

        protected override void RegisterLogger()
        {
            Ex.Trace.RegisterFileLogger();
        }

        protected virtual void RegisterInject()
        {
        }

        protected virtual void CreateCommandPool()
        {
        }
    }
}