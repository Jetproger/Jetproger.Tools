using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Commanders
{
    public class CommandService : ServiceBase
    {
        private HttpSelfHostServer _server;

        public CommandService()
        {
            ServiceName = k<AppServiceName>.key;
        }

        public void Start()
        {
            OnStart(new string[0]);
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (_server != null) return;
                //GTINNlogTracer.Run();
                f.err.Guard<AppPortNotSpecifiedException>(string.IsNullOrWhiteSpace(k<AppPort>.key));
                var hostName = GetHostName();
                var config = new HttpSelfHostConfiguration(hostName)
                {
                    MaxReceivedMessageSize = int.MaxValue,
                    MaxBufferSize = int.MaxValue
                };
                var hostNameComparisonMode = (HostNameComparisonMode)k<AppHostNameComparisonMode>.key.As<int>();
                config.HostNameComparisonMode = hostNameComparisonMode;
                config.Routes.MapHttpRoute(
                    name: "cmd",
                    routeTemplate: "jetproger/v1/cmd",
                    defaults: new { controller = "Command", action = "ExecuteCommand", name = RouteParameter.Optional }
                );
                _server = new HttpSelfHostServer(config);
                var task = _server.OpenAsync();
                f.log.To("Service start");
                f.log.To($"Service listening: {hostName}");
                Task.WaitAll(task);
            }
            catch (Exception e)
            {
                f.log.To("Error start service");
                f.log.To(e);
            }
        }

        protected override void OnStop()
        {
            _server = null; 
        }

        private static string GetHostName()
        {
            var cert = f.cry.App;
            string host = k<AppHost>.key;
            return cert != null ? $"https://{host}:{k<AppPort>.key.As<int>()}" : $"http://{host}:{k<AppPort>.key.As<int>()}";
        }
    }
}
namespace Jetproger.Tools.AppConfig
{
    public class AppServiceName : ConfigSetting
    {
        public AppServiceName() : base("jetproger-api") { }
    }
}