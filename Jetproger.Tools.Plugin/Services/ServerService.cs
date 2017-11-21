using System.ServiceModel;
using System.ServiceModel.Description;
using Jetproger.Tools.Plugin.Bases;
using Jetproger.Tools.Plugin.Commands;

namespace Jetproger.Tools.Plugin.Services
{
    public class ServerService : StarterService
    {
        private ServiceHost _host;

        public ServerService()
        {
            ServiceName = Methods.ConfigAsString("ServiceName", "Jetproger.Tools.Service");
            var builder = new ServerBuilder();
            _host = new ServiceHost(typeof(CommandService));
            _host.AddServiceEndpoint(typeof(ICommandService), builder.GetServerBinding(), builder.GetUrl());
            _host.Description.Behaviors.Add(builder.GetThrottlingBehavior());
            var serviceDebugBehavior = _host.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (serviceDebugBehavior != null) serviceDebugBehavior.IncludeExceptionDetailInFaults = true;
        }

        protected override void OnStart(string[] args)
        {
            _host?.Open();
        }

        protected override void OnStop()
        {
            if (_host == null) return;
            _host.Close();
            _host = null;
        }
    }
}