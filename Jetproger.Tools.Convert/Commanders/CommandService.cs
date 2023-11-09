//using System;
//using System.ServiceModel;
//using System.ServiceProcess;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.SelfHost;
//using Jetproger.Tools.AppConfig;
//using Jetproger.Tools.Convert.Bases;
//using Jetproger.Tools.Convert.Converts;
//using Jetproger.Tools.Convert.Settings;
//
//namespace Jetproger.Tools.Convert.Commanders
//{
//    public class CommandService : ServiceBase
//    {
//        private HttpSelfHostServer _server;
//
//        public CommandService()
//        {
//            ServiceName = k<AppServiceName>.As<string>();
//        }
//
//        public void Start()
//        {
//            OnStart(new string[0]);
//        }
//
//        public void Start(string[] args)
//        {
//            OnStart(args);
//        }
//
//        protected override void OnStart(string[] args)
//        {
//            try
//            {
//                if (_server != null) return;
//                //GTINNlogTracer.Run();
//                f.err<AppPortNotSpecifiedException>(string.IsNullOrWhiteSpace(k<AppPort>.As<string>()));
//                var hostName = GetHostName();
//                var config = new HttpSelfHostConfiguration(hostName)
//                {
//                    MaxReceivedMessageSize = int.MaxValue,
//                    MaxBufferSize = int.MaxValue
//                };
//                var hostNameComparisonMode = (HostNameComparisonMode)k<AppHostNameComparisonMode>.As<int>();
//                config.HostNameComparisonMode = hostNameComparisonMode;
//                config.Routes.MapHttpRoute(
//                    name: "cmd",
//                    routeTemplate: "jetproger/v1/cmd",
//                    defaults: new { controller = "Command", action = "ExecuteCommand", name = RouteParameter.Optional }
//                );
//                _server = new HttpSelfHostServer(config);
//                var task = _server.OpenAsync();
//                f.log("Service start");
//                f.log($"Service listening: {hostName}");
//                Task.WaitAll(task);
//            }
//            catch (Exception e)
//            {
//                f.log("Error start service");
//                f.log(e);
//            }
//        }
//
//        protected override void OnStop()
//        {
//            _server = null; 
//        }
//
//        private static string GetHostName()
//        {
//            var cert = f.cry.app;
//            string host = k<AppHost>.As<string>();
//            return cert != null ? $"https://{host}:{k<AppPort>.As<int>()}" : $"http://{host}:{k<AppPort>.As<int>()}";
//        }
//    }
//}
//namespace Jetproger.Tools.AppConfig
//{
//    public class AppServiceName : ConfigSetting
//    {
//        public AppServiceName() : base("jetproger-api") { }
//    }
//}