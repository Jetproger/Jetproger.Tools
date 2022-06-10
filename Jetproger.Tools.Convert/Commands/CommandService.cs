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

namespace Jetproger.Tools.Convert.Commands
{
    public class CommandService : ServiceBase
    {
        private HttpSelfHostServer _server;

        public CommandService()
        {
            ServiceName = J_<AppServiceName>.Sz;
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
                Je.err.Guard<AppPortNotSpecifiedException>(string.IsNullOrWhiteSpace(J_<AppPort>.Sz));
                var hostName = GetHostName();
                var config = new HttpSelfHostConfiguration(hostName)
                {
                    MaxReceivedMessageSize = int.MaxValue,
                    MaxBufferSize = int.MaxValue
                };
                var hostNameComparisonMode = (HostNameComparisonMode)J_<AppHostNameComparisonMode>.Sz.As<int>();
                config.HostNameComparisonMode = hostNameComparisonMode;
                config.Routes.MapHttpRoute(
                    name: "cmd",
                    routeTemplate: "kiz/v1/cmd",
                    defaults: new { controller = "Command", action = "ExecuteCommand", name = RouteParameter.Optional }
                );
                _server = new HttpSelfHostServer(config);
                var task = _server.OpenAsync();
                Je.log.To("Service start");
                Je.log.To($"Service listening: {hostName}");
                Task.WaitAll(task);
            }
            catch (Exception e)
            {
                Je.log.To("Error start service");
                Je.log.To(e);
            }
        }

        protected override void OnStop()
        {
            _server = null; 
        }

        private static string GetHostName()
        {
            var cert = Je.cry.App;
            string host = J_<AppHost>.Sz;
            return cert != null ? $"https://{host}:{J_<AppPort>.Sz.As<int>()}" : $"http://{host}:{J_<AppPort>.Sz.As<int>()}";
        }
    }
}

namespace Jetproger.Tools.AppConfig
{
    public class AppServiceName : ConfigSetting { public AppServiceName() : base("jetproger-api") { } }
}