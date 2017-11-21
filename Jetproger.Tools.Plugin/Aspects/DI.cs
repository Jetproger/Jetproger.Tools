using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using Jetproger.Tools.Plugin.Aspects;
using Microsoft.Practices.Unity;

namespace Tools
{
    public static class DI
    {
        private static readonly UnityConfigProxy[] UnityConfigHolder = { null };
        private static readonly IUnityContainer[] ContainerHolder = { null };

        public static Type TypeOf<T>()
        {
            return Container.Resolve<T>().GetType();
        }

        public static Type TypeOf(Type type)
        {
            return Container.Resolve(type).GetType();
        }

        public static T Resolve<T>(Action<T> initialize)
        {
            var instance = Resolve<T>();
            initialize(instance);
            return instance;
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static object Resolve(Type type, Action<object> initialize)
        {
            var instance = Resolve(type);
            initialize(instance);
            return instance;
        }

        public static object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        private static IUnityContainer Container
        {
            get
            {
                if (ContainerHolder[0] == null)
                {
                    lock (ContainerHolder)
                    {
                        if (ContainerHolder[0] == null)
                        {
                            var section = new InmemoryUnityConfigurationSection();
                            section.DeserializeSection(UnityConfig.GetUnityConfigXml());
                            var container = new UnityContainer();
                            section.Configure(container);
                            ContainerHolder[0] = container;
                        }
                    }
                }
                return ContainerHolder[0];
            }
        }

        private static UnityConfigProxy UnityConfig
        {
            get
            {
                if (UnityConfigHolder[0] == null)
                {
                    lock (UnityConfigHolder)
                    {
                        if (UnityConfigHolder[0] == null)
                        {
                            UnityConfigHolder[0] = GetProxy();
                        }
                    }
                }
                return UnityConfigHolder[0];
            }
        }

        private static UnityConfigProxy GetProxy()
        {
            CreateChannel();
            var type = typeof(UnityConfigProxy);
            var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
            var objectUri = type.Name.ToLower();
            var uri = $"ipc://{portName}/{objectUri}";
            var proxy = TryGetExistsProxy(type, uri);
            if (proxy != null) return proxy;
            CreateUnityConfig();
            return (UnityConfigProxy)Activator.GetObject(type, uri);
        }

        private static void CreateChannel()
        {
            var type = typeof(Type);
            foreach (var entry in RemotingConfiguration.GetRegisteredWellKnownServiceTypes())
            {
                if (entry.ObjectType == type) return;
            }
            var portName = $"{AppDomain.CurrentDomain.FriendlyName.Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
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

        private static UnityConfigProxy TryGetExistsProxy(Type type, string uri)
        {
            try
            {
                var proxy = (UnityConfigProxy)Activator.GetObject(type, uri);
                proxy.CreateChannel();
                return proxy;
            }
            catch
            {
                return null;
            }
        }

        private static void CreateUnityConfig()
        {
            var setup = new AppDomainSetup {
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                LoaderOptimization = LoaderOptimization.SingleDomain,
                ShadowCopyFiles = "true"
            };
            var name = $"f__{(typeof(UnityConfigXml)).AssemblyQualifiedName}";
            var domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
            var type = typeof(UnityConfigProxy);
            var assemblyName = type.Assembly.GetName().Name;
            var typeName = type.FullName ?? string.Empty;
            var instance = domain.CreateInstanceAndUnwrap(assemblyName, typeName) as UnityConfigProxy;
            instance?.CreateChannel();
        }
    }
}