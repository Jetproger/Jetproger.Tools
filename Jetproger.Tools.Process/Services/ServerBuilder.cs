using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.MsmqIntegration;
using Jetproger.Tools.Process.Bases;

namespace Jetproger.Tools.Process.Services
{
    public class ServerBuilder
    {
        private readonly string _bindingName;
        private readonly string _host;
        private readonly int _port;

        public ServerBuilder()
        {
            _bindingName = Methods.ConfigAsString("ServiceBinding", "NetTcpBinding");
            _host = Methods.ConfigAsString("ServiceHost", "127.0.0.1");
            _port = Methods.ConfigAsInt("ServicePort", 12345);
        }

        public ServerBuilder(string host, int port)
        {
            _bindingName = Methods.ConfigAsString("ServiceBinding", "NetTcpBinding");
            _host = !string.IsNullOrWhiteSpace(host) ? host : Methods.ConfigAsString("ServiceHost", "127.0.0.1");
            _port = port > 0 && port < short.MaxValue ? port : Methods.ConfigAsInt("ServicePort", 12345);
        }

        public string GetUrl()
        {
            return $"{GetProtocol()}://{_host}:{_port}/jetproger/commandservice";
        }

        private string GetProtocol()
        {
            return _bindingName.ToLower().StartsWith("nettcp") ? "net.tcp" : "http";
        }

        public Binding GetClientBinding()
        {
            return GetBinding(_bindingName);
        }

        public Binding GetServerBinding()
        {
            var binding = GetBinding(_bindingName);
            binding.SendTimeout = TimeSpan.MaxValue;
            binding.ReceiveTimeout = TimeSpan.MaxValue; //TimeSpan.FromHours(1);
            return binding;
        }

        public ServiceThrottlingBehavior GetThrottlingBehavior()
        {
            var serviceThrottlingBehavior = new ServiceThrottlingBehavior();
            serviceThrottlingBehavior.MaxConcurrentCalls = serviceThrottlingBehavior.MaxConcurrentCalls * 2;
            serviceThrottlingBehavior.MaxConcurrentSessions = short.MaxValue;
            serviceThrottlingBehavior.MaxConcurrentInstances = serviceThrottlingBehavior.MaxConcurrentSessions + serviceThrottlingBehavior.MaxConcurrentCalls;
            return serviceThrottlingBehavior;
        }

        public void ConfigureChannelFactory(ChannelFactory<ICommandService> factory)
        {
            factory.Endpoint.Binding.SendTimeout = TimeSpan.MaxValue;
            factory.Endpoint.Binding.OpenTimeout = TimeSpan.MaxValue;
            factory.Endpoint.Binding.ReceiveTimeout = TimeSpan.MaxValue;
            foreach (OperationDescription op in factory.Endpoint.Contract.Operations)
            {
                var behavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (behavior != null) behavior.MaxItemsInObjectGraph = int.MaxValue;
            }
        }

        private static Binding GetBinding(string name)
        {
            name = name.ToLower();
            if (!name.EndsWith("binding")) name += "binding";
            switch (name)
            {
                case "basichttpbinding": return GetBasicHttpBinding();
                case "basichttpcontextbinding": return GetBasicHttpContextBinding();
                case "netnamedpipebinding": return GetNetNamedPipeBinding();
                case "netmsmqbinding": return GetNetMsmqBinding();
                case "netpeertcpbinding": return GetNetPeerTcpBinding();
                case "nettcpcontextbinding": return GetNetTcpContextBinding();
                case "msmqintegrationbinding": return GetMsmqIntegrationBinding();
                case "wsdualhttpbinding": return GetWSDualHttpBinding();
                case "wsfederationhttpbinding": return GetWSFederationHttpBinding();
                case "wshttpbinding": return GetWSHttpBinding();
                case "wshttpcontextbinding": return GetWSHttpContextBinding();
                default: return GetNetTcpBinding();
            }
        }

        private static Binding GetBasicHttpBinding()
        {
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = BasicHttpSecurityMode.None;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
            binding.TransferMode = TransferMode.Buffered;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            return binding;
        }

        private static Binding GetBasicHttpContextBinding()
        {
            var binding = new BasicHttpContextBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = BasicHttpSecurityMode.None;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
            binding.TransferMode = TransferMode.Buffered;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            return binding;
        }

        private static Binding GetNetNamedPipeBinding()
        {
            var binding = new NetNamedPipeBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = NetNamedPipeSecurityMode.None;
            binding.TransferMode = TransferMode.Buffered;
            binding.MaxConnections = short.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            return binding;
        }

        private static Binding GetNetMsmqBinding()
        {
            var binding = new NetMsmqBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = NetMsmqSecurityMode.None;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            return binding;
        }

        private static Binding GetNetPeerTcpBinding()
        {
            var binding = new NetPeerTcpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            return binding;
        }

        private static Binding GetMsmqIntegrationBinding()
        {
            var binding = new MsmqIntegrationBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = MsmqIntegrationSecurityMode.None;
            return binding;
        }

        private static Binding GetNetTcpBinding()
        {
            var binding = new NetTcpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            binding.TransferMode = TransferMode.Buffered;
            binding.ListenBacklog = int.MaxValue;
            binding.MaxConnections = short.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            return binding;
        }

        private static Binding GetNetTcpContextBinding()
        {
            var binding = new NetTcpContextBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            binding.TransferMode = TransferMode.Buffered;
            binding.ListenBacklog = int.MaxValue;
            binding.MaxConnections = short.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            return binding;
        }

        private static Binding GetWSDualHttpBinding()
        {
            var binding = new WSDualHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = WSDualHttpSecurityMode.None;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            return binding;
        }

        private static Binding GetWSFederationHttpBinding()
        {
            var binding = new WSFederationHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = WSFederationHttpSecurityMode.None;
            return binding;
        }

        private static Binding GetWSHttpBinding()
        {
            var binding = new WSHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            return binding;
        }

        private static Binding GetWSHttpContextBinding()
        {
            var binding = new WSHttpContextBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            return binding;
        }
    }
}