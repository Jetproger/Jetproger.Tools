using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;
using System.Security.Principal;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Ex
    {
        public static T ReadOne<T>(T[] holder)
        {
            lock (holder)
            {
                return holder[0];
            }
        }

        public static void WriteOne<T>(T[] holder, T value)
        {
            lock (holder)
            {
                holder[0] = value;
            }
        }

        public static T GetOne<T>(T[] holder, Func<T> factory) where T : class
        {
            if (holder[0] == null)
            {
                lock (holder)
                {
                    if (holder[0] == null) holder[0] = factory();
                }
            }
            return holder[0];
        }

        public static T GetOne<T>(T?[] holder, Func<T> factory) where T : struct
        {
            if (holder[0] == null)
            {
                lock (holder)
                {
                    if (holder[0] == null) holder[0] = factory();
                }
            }
            return (T)holder[0];
        }

        public static One<TKey, TValue> GetOne<TKey, TValue>(Func<TKey, TValue> factory)
        {
            return new One<TKey, TValue>(factory);
        }

        public static T GetOne<T>() where T : OneProxy
        {
            return One<T>.Get(GetProxy<T>);
        }

        private static T GetProxy<T>() where T : OneProxy
        {
            CreateChannel();
            var type = typeof(T);
            var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
            var objectUri = type.Name.ToLower();
            var uri = $"ipc://{portName}/{objectUri}";
            var proxy = TryGetExistsProxy<T>(type, uri);
            if (proxy != null) return proxy;
            CreateOne<T>();
            return (T)Activator.GetObject(type, uri);
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

        private static T TryGetExistsProxy<T>(Type type, string uri) where T : OneProxy
        {
            try
            {
                var proxy = (T)Activator.GetObject(type, uri);
                proxy.CreateChannel();
                return proxy;
            }
            catch
            {
                return null;
            }
        }

        private static void CreateOne<T>() where T : OneProxy
        {
            var setup = new AppDomainSetup
            {
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                LoaderOptimization = LoaderOptimization.SingleDomain,
                ShadowCopyFiles = "true"
            };
            var name = $"f__{(typeof(T)).AssemblyQualifiedName}";
            var domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
            var type = typeof(T);
            var assemblyName = type.Assembly.GetName().Name;
            var typeName = type.FullName ?? string.Empty;
            var instance = domain.CreateInstanceAndUnwrap(assemblyName, typeName) as T;
            instance?.CreateChannel();
        }
    }

    public sealed class One<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;
        private readonly Func<TKey, TValue> _factory;

        public One(Func<TKey, TValue> factory)
        {
            _factory = factory ?? (x => default(TValue));
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!_dictionary.ContainsKey(key))
                {
                    lock (_dictionary)
                    {
                        if (!_dictionary.ContainsKey(key))
                        {
                            _dictionary.Add(key, _factory(key));
                        }
                    }
                }
                return _dictionary[key];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return GetPairs().GetEnumerator();
        }

        private IEnumerable<KeyValuePair<TKey, TValue>> GetPairs()
        {
            lock (_dictionary)
            {
                foreach (KeyValuePair<TKey, TValue> pair in _dictionary)
                {
                    yield return pair;
                }
            }
        }
    }

    public class One<T> where T : class
    {
        private static readonly T[] Holder = { null };

        public static T Get(Func<T> factory)
        {
            if (Holder[0] == null)
            {
                lock (Holder)
                {
                    if (Holder[0] == null) Holder[0] = factory();
                }
            }
            return Holder[0];
        }
    }

    public class OneProxy : MarshalByRefObject
    {
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void CreateChannel()
        {
            var type = GetType();
            foreach (var entry in RemotingConfiguration.GetRegisteredWellKnownServiceTypes())
            {
                if (entry.ObjectType == type) return;
            }
            var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
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
    }
}