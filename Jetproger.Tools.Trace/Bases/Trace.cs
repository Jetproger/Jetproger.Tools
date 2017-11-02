using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using Jetproger.Tools.Trace.Bases;

namespace Tools
{
    public static class Trace
    {
        private static readonly TraceProxy[] GlobalTraceHolder = { null };
        private static readonly NlogTraceListener[] NlogHolder = { null };
        internal static string AppUser;

        public static void SetAppUser(string appUser)
        {
            GlobalTrace.SetAppUser(appUser);
        }

        public static void Run()
        {
            RunNlog();
            GlobalTrace.RegisterLogger(NlogConfig.GetMainTraceName());
        }

        public static void Run<T>() where T : TypedMessage
        {
            RunNlog();
            GlobalTrace.RegisterLogger((typeof(T)).Name);
        }

        private static void RunNlog()
        {
            if (NlogHolder[0] == null)
            {
                lock (NlogHolder)
                {
                    if (NlogHolder[0] == null)
                    {
                        NlogHolder[0] = new NlogTraceListener();
                        System.Diagnostics.Trace.Listeners.Add(NlogHolder[0]);
                    }
                }
            }
        }

        internal static TraceProxy GlobalTrace
        {
            get
            {
                if (GlobalTraceHolder[0] == null)
                {
                    lock (GlobalTraceHolder)
                    {
                        if (GlobalTraceHolder[0] == null) GlobalTraceHolder[0] = GetProxy();
                    }
                }
                return GlobalTraceHolder[0];
            }
        }

        private static TraceProxy GetProxy()
        {
            CreateChannel();
            var type = typeof(TraceProxy);
            var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
            var objectUri = type.Name.ToLower();
            var uri = $"ipc://{portName}/{objectUri}";
            var proxy = TryGetExistsProxy(type, uri);
            if (proxy != null) return proxy;
            CreateTrace();
            return (TraceProxy)Activator.GetObject(type, uri);
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
            var config = new Hashtable
            {
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

        private static TraceProxy TryGetExistsProxy(Type type, string uri)
        {
            try
            {
                var proxy = (TraceProxy)Activator.GetObject(type, uri);
                proxy.CreateChannel();
                return proxy;
            }
            catch
            {
                return null;
            }
        }

        private static void CreateTrace()
        {
            var setup = new AppDomainSetup
            {
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                LoaderOptimization = LoaderOptimization.SingleDomain,
                ShadowCopyFiles = "true"
            };
            var name = $"f__{(typeof(Trace)).AssemblyQualifiedName}";
            var domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
            var type = typeof(TraceProxy);
            var assemblyName = type.Assembly.GetName().Name;
            var typeName = type.FullName ?? string.Empty;
            var instance = domain.CreateInstanceAndUnwrap(assemblyName, typeName) as TraceProxy;
            instance?.CreateChannel();
        }
    }
}