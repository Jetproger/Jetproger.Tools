using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Resource.Bases;

namespace Jetproger.Tools.Process.Commands
{
    public static class CommandFactory
    {
        private static readonly CommandPool[] PoolHolder = { null };

        public static CommandWorker GetWorker()
        {
            return Pool.Get();
        }

        public static void InitCommandPool(int size)
        {
            InitCommandPool(GetPool(size));
        }

        private static void InitCommandPool(CommandPool pool)
        {
            while (!pool.IsFull)
            {
                Thread.Sleep(111);
            }
        }

        private static CommandPool GetPool(int size)
        {
            if (PoolHolder[0] == null)
            {
                lock (PoolHolder)
                {
                    if (PoolHolder[0] == null) PoolHolder[0] = GetProxy(size);
                }
            }
            return PoolHolder[0];
        }

        private static CommandPool Pool
        {
            get
            {
                if (PoolHolder[0] == null)
                {
                    lock (PoolHolder)
                    {
                        if (PoolHolder[0] == null) PoolHolder[0] = GetProxy(4);
                    }
                }
                return PoolHolder[0];
            }
        }

        private static CommandPool GetProxy(int size)
        {
            CreateChannel();
            var type = typeof(CommandPoolProxy);
            var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{System.Diagnostics.Process.GetCurrentProcess().Id}";
            var objectUri = type.Name.ToLower();
            var uri = $"ipc://{portName}/{objectUri}";
            var maxReservedDomains = Ex.Fi<MaxReservedDomainSetting>.Text.As<int>();
            if (maxReservedDomains < 1) maxReservedDomains = size;
            if (maxReservedDomains < 1) maxReservedDomains = 1;
            var proxy = TryGetExistsProxy(type, uri);
            if (proxy != null) return proxy.GetPool(maxReservedDomains);
            CreateProxy();
            proxy = (CommandPoolProxy)Activator.GetObject(type, uri);
            return proxy.GetPool(maxReservedDomains);
        }

        private static void CreateChannel()
        {
            var type = typeof(Type);
            foreach (var entry in RemotingConfiguration.GetRegisteredWellKnownServiceTypes())
            {
                if (entry.ObjectType == type) return;
            }
            var portName = $"{AppDomain.CurrentDomain.FriendlyName.Replace(".", "-").ToLower()}-{System.Diagnostics.Process.GetCurrentProcess().Id}";
            var objectUri = type.Name.ToLower();
            var client = new BinaryClientFormatterSinkProvider();
            var server = new BinaryServerFormatterSinkProvider
            {
                TypeFilterLevel = TypeFilterLevel.Full
            };
            var config = new Hashtable {
                ["name"] = string.Empty,
                ["portName"] = portName,
                ["tokenImpersonationLevel"] = TokenImpersonationLevel.Impersonation,
                ["impersonate"] = true,
                ["useDefaultCredentials"] = true,
                ["secure"] = true,
                ["typeFilterLevel"] = TypeFilterLevel.Full
            };
            var ipcChannel = new IpcChannel(config, client, server);
            ChannelServices.RegisterChannel(ipcChannel, true);
            RemotingConfiguration.RegisterWellKnownServiceType(type, objectUri, WellKnownObjectMode.SingleCall);
        }

        private static CommandPoolProxy TryGetExistsProxy(Type type, string uri)
        {
            try
            {
                var proxy = (CommandPoolProxy)Activator.GetObject(type, uri);
                proxy.CreateChannel();
                return proxy;
            }
            catch
            {
                return null;
            }
        }

        private static void CreateProxy()
        {
            var setup = new AppDomainSetup
            {
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                LoaderOptimization = LoaderOptimization.SingleDomain,
                ShadowCopyFiles = "true"
            };
            var name = $"f__{(typeof(CommandPoolProxy)).AssemblyQualifiedName}";
            var domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
            var type = typeof(CommandPoolProxy);
            var assemblyName = type.Assembly.GetName().Name;
            var typeName = type.FullName ?? string.Empty;
            var instance = domain.CreateInstanceAndUnwrap(assemblyName, typeName) as CommandPoolProxy;
            instance?.CreateChannel();
        }
    }
}