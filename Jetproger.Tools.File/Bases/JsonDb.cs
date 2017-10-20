using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using Jetproger.Tools.File.Jsons;

namespace Tools
{
    public static partial class File
    {
        public static JsonWorkWhere<T> From<T>()
        {
            return new JsonWorkWhere<T>();
        }

        internal static class JsonDb
        {
            private static readonly HashSet<Type> LocalTypes = new HashSet<Type>();
            private static readonly JsonProxy[] GlobalJsonHolder = { null };
            private static readonly JsonProxy[] LocalJsonHolder = { null };

            private static JsonProxy LocalJson
            {
                get
                {
                    if (LocalJsonHolder[0] == null)
                    {
                        lock (LocalJsonHolder)
                        {
                            if (LocalJsonHolder[0] == null) LocalJsonHolder[0] = new JsonProxy();
                        }
                    }
                    return LocalJsonHolder[0];
                }
            }

            private static JsonProxy GlobalJson
            {
                get
                {
                    if (GlobalJsonHolder[0] == null)
                    {
                        lock (GlobalJsonHolder)
                        {
                            if (GlobalJsonHolder[0] == null) GlobalJsonHolder[0] = GetProxy();
                        }
                    }
                    return GlobalJsonHolder[0];
                }
            }

            private static JsonProxy GetProxy()
            {
                CreateChannel();
                var type = typeof(JsonProxy);
                var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
                var objectUri = type.Name.ToLower();
                var uri = $"ipc://{portName}/{objectUri}";
                var proxy = TryGetExistsProxy(type, uri);
                if (proxy != null) return proxy;
                CreateJson();
                return (JsonProxy)Activator.GetObject(type, uri);
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

            private static JsonProxy TryGetExistsProxy(Type type, string uri)
            {
                try
                {
                    var proxy = (JsonProxy)Activator.GetObject(type, uri);
                    proxy.CreateChannel();
                    return proxy;
                }
                catch
                {
                    return null;
                }
            }

            private static void CreateJson()
            {
                var setup = new AppDomainSetup
                {
                    ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                    ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    LoaderOptimization = LoaderOptimization.SingleDomain,
                    ShadowCopyFiles = "true"
                };
                var name = $"f__{(typeof(JsonFile)).AssemblyQualifiedName}";
                var domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
                var type = typeof(JsonProxy);
                var assemblyName = type.Assembly.GetName().Name;
                var typeName = type.FullName ?? string.Empty;
                var instance = domain.CreateInstanceAndUnwrap(assemblyName, typeName) as JsonProxy;
                instance?.CreateChannel();
            }

            public static JsonSet Execute(Type itemType, JsonWork work)
            {
                if (LocalTypes.Contains(itemType))
                {
                    return LocalJson.Execute(itemType, work);
                }
                try
                {
                    return GlobalJson.Execute(itemType, work);
                }
                catch (Exception)
                {
                    if (!LocalTypes.Contains(itemType)) LocalTypes.Add(itemType);
                    return LocalJson.Execute(itemType, work);
                }
            }
        }
    }
}