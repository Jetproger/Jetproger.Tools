using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using Jetproger.Tools.Cache.Bases;

namespace Tools
{
    public static partial class Cache
    {
        private static readonly HashSet<Type> LocalTypes = new HashSet<Type>();
        private static readonly CacheProxy[] GlobalCacheHolder = { null };
        private static readonly CacheProxy[] LocalCacheHolder = { null };

        private static CacheProxy LocalCache
        {
            get
            {
                if (LocalCacheHolder[0] == null)
                {
                    lock (LocalCacheHolder)
                    {
                        if (LocalCacheHolder[0] == null) LocalCacheHolder[0] = new CacheProxy();
                    }
                }
                return LocalCacheHolder[0];
            }
        }

        private static CacheProxy GlobalCache
        {
            get
            {
                if (GlobalCacheHolder[0] == null)
                {
                    lock (GlobalCacheHolder)
                    {
                        if (GlobalCacheHolder[0] == null) GlobalCacheHolder[0] = GetProxy();
                    }
                }
                return GlobalCacheHolder[0];
            }
        }

        private static CacheProxy GetProxy()
        {
            CreateChannel();
            var type = typeof(CacheProxy);
            var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
            var objectUri = type.Name.ToLower();
            var uri = $"ipc://{portName}/{objectUri}";
            var proxy = TryGetExistsProxy(type, uri);
            if (proxy != null) return proxy;
            CreateCache();
            return (CacheProxy)Activator.GetObject(type, uri);
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

        private static CacheProxy TryGetExistsProxy(Type type, string uri)
        {
            try
            {
                var proxy = (CacheProxy)Activator.GetObject(type, uri);
                proxy.CreateChannel();
                return proxy;
            }
            catch
            {
                return null;
            }
        }

        private static void CreateCache()
        {
            var setup = new AppDomainSetup {
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                LoaderOptimization = LoaderOptimization.SingleDomain,
                ShadowCopyFiles = "true"
            };
            var name = $"f__{(typeof(CacheCore)).AssemblyQualifiedName}";
            var domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
            var type = typeof(CacheProxy);
            var assemblyName = type.Assembly.GetName().Name;
            var typeName = type.FullName ?? string.Empty;
            var instance = domain.CreateInstanceAndUnwrap(assemblyName, typeName) as CacheProxy;
            instance?.CreateChannel();
        }

        #region Get

        public static bool Get<T, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, out T p)
        {
            return TryGet(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 }, out p);
        }

        public static bool Get<T, T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, out T p)
        {
            return TryGet(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 }, out p);
        }

        public static bool Get<T, T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, out T p)
        {
            return TryGet(new object[] { p0, p1, p2, p3, p4, p5, p6, p7 }, out p);
        }

        public static bool Get<T, T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, out T p)
        {
            return TryGet(new object[] { p0, p1, p2, p3, p4, p5, p6 }, out p);
        }

        public static bool Get<T, T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, out T p)
        {
            return TryGet(new object[] { p0, p1, p2, p3, p4, p5 }, out p);
        }

        public static bool Get<T, T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, out T p)
        {
            return TryGet(new object[] { p0, p1, p2, p3, p4 }, out p);
        }

        public static bool Get<T, T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3, out T p)
        {
            return TryGet(new object[] { p0, p1, p2, p3 }, out p);
        }

        public static bool Get<T, T0, T1, T2>(T0 p0, T1 p1, T2 p2, out T p)
        {
            return TryGet(new object[] { p0, p1, p2 }, out p);
        }

        public static bool Get<T, T0, T1>(T0 p0, T1 p1, out T p)
        {
            return TryGet(new object[] { p0, p1 }, out p);
        }

        public static bool Get<T, T0>(T0 p0, out T p)
        {
            return TryGet(new object[] { p0 }, out p);
        }


        #endregion

        #region Get or add

        public static T Get<T, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, Func<T> func, int lifetime = 0)
        {
            return TryGetOrAdd(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 }, func, lifetime);
        }

        public static T Get<T, T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, Func<T> func, int lifetime = 0)
        {
            return TryGetOrAdd(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 }, func, lifetime);
        }

        public static T Get<T, T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, Func<T> func, int lifetime = 0)
        {
            return TryGetOrAdd(new object[] { p0, p1, p2, p3, p4, p5, p6, p7 }, func, lifetime);
        }

        public static T Get<T, T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, Func<T> func, int lifetime = 0)
        {
            return TryGetOrAdd(new object[] { p0, p1, p2, p3, p4, p5, p6 }, func, lifetime);
        }

        public static T Get<T, T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, Func<T> func, int lifetime = 0)
        {
            return TryGetOrAdd(new object[] { p0, p1, p2, p3, p4, p5 }, func, lifetime);
        }

        public static T Get<T, T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, Func<T> func, int lifetime = 0)
        {
            return TryGetOrAdd(new object[] { p0, p1, p2, p3, p4 }, func, lifetime);
        }

        public static T Get<T, T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3, Func<T> func, int lifetime = 0)
        {
            return TryGetOrAdd(new object[] { p0, p1, p2, p3 }, func, lifetime);
        }

        public static T Get<T, T0, T1, T2>(T0 p0, T1 p1, T2 p2, Func<T> func, int lifetime = 0)
        {
            return TryGetOrAdd(new object[] { p0, p1, p2 }, func, lifetime);
        }

        public static T Get<T, T0, T1>(T0 p0, T1 p1, Func<T> func, int lifetime = 0)
        {
            return TryGetOrAdd(new object[] { p0, p1 }, func, lifetime);
        }

        public static T Get<T, T0>(T0 p0, Func<T> func, int lifetime = 0)
        {
            return TryGetOrAdd(new object[] { p0 }, func, lifetime);
        }

        #endregion

        #region Add

        public static void Add<T, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, Func<T> func, int lifetime = 0)
        {
            TryAdd(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 }, func(), lifetime);
        }

        public static void Add<T, T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, Func<T> func, int lifetime = 0)
        {
            TryAdd(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 }, func(), lifetime);
        }

        public static void Add<T, T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, Func<T> func, int lifetime = 0)
        {
            TryAdd(new object[] { p0, p1, p2, p3, p4, p5, p6, p7 }, func(), lifetime);
        }

        public static void Add<T, T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, Func<T> func, int lifetime = 0)
        {
            TryAdd(new object[] { p0, p1, p2, p3, p4, p5, p6 }, func(), lifetime);
        }

        public static void Add<T, T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, Func<T> func, int lifetime = 0)
        {
            TryAdd(new object[] { p0, p1, p2, p3, p4, p5 }, func(), lifetime);
        }

        public static void Add<T, T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, Func<T> func, int lifetime = 0)
        {
            TryAdd(new object[] { p0, p1, p2, p3, p4 }, func(), lifetime);
        }

        public static void Add<T, T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3, Func<T> func, int lifetime = 0)
        {
            TryAdd(new object[] { p0, p1, p2, p3 }, func(), lifetime);
        }

        public static void Add<T, T0, T1, T2>(T0 p0, T1 p1, T2 p2, Func<T> func, int lifetime = 0)
        {
            TryAdd(new object[] { p0, p1, p2 }, func(), lifetime);
        }

        public static void Add<T, T0, T1>(T0 p0, T1 p1, Func<T> func, int lifetime = 0)
        {
            TryAdd(new object[] { p0, p1 }, func(), lifetime);
        }

        public static void Add<T, T0>(T0 p0, Func<T> func, int lifetime = 0)
        {
            TryAdd(new object[] { p0 }, func(), lifetime);
        }

        #endregion

        #region Off

        public static void Off<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
        {
            TryRemove(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 });
        }

        public static void Off<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            TryRemove(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 });
        }

        public static void Off<T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            TryRemove(new object[] { p0, p1, p2, p3, p4, p5, p6, p7 });
        }

        public static void Off<T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            TryRemove(new object[] { p0, p1, p2, p3, p4, p5, p6 });
        }

        public static void Off<T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            TryRemove(new object[] { p0, p1, p2, p3, p4, p5 });
        }

        public static void Off<T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            TryRemove(new object[] { p0, p1, p2, p3, p4 });
        }

        public static void Off<T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3)
        {
            TryRemove(new object[] { p0, p1, p2, p3 });
        }

        public static void Off<T0, T1, T2>(T0 p0, T1 p1, T2 p2)
        {
            TryRemove(new object[] { p0, p1, p2 });
        }

        public static void Off<T0, T1>(T0 p0, T1 p1)
        {
            TryRemove(new object[] { p0, p1 });
        }

        public static void Off<T0>(T0 p0)
        {
            TryRemove(new object[] { p0 });
        }

        #endregion

        public static void TryRemove(object[] keys)
        {
            if (keys == null || keys.Length == 0) return;
            var stringKeys = Methods.GetStringKeys(keys);
            GlobalCache.Off(stringKeys);
            LocalCache.Off(stringKeys);
        }

        public static T TryGetOrAdd<T>(object[] keys, Func<T> func, int lifetime)
        {
            if (keys == null || keys.Length == 0) return default(T);
            T value;
            if (TryGet(keys, out value)) return value;
            value = func();
            TryAdd(keys, value, lifetime);
            return value;
        }

        public static bool TryGet<T>(object[] keys, out T p)
        {
            object value;
            var result = TryGet(keys, typeof(T), out value);
            p = result ? (T)value : default(T);
            return result;
        }

        public static void TryAdd<T>(object[] keys, T value, int lifetime)
        {
            if (keys == null || keys.Length == 0) return;
            var type = value != null ? value.GetType() : typeof(T);
            var genericArgumentType = GetGenericArgumentType(type);
            var stringKeys = Methods.GetStringKeys(keys);
            if (LocalTypes.Contains(type) || (genericArgumentType != null && LocalTypes.Contains(genericArgumentType)))
            {
                LocalCache.Add(stringKeys, value, lifetime);
                return;
            }
            try
            {
                GlobalCache.Add(stringKeys, value, lifetime);
            }
            catch (Exception)
            {
                if (genericArgumentType != null && !LocalTypes.Contains(genericArgumentType)) LocalTypes.Add(genericArgumentType);
                if (!LocalTypes.Contains(type)) LocalTypes.Add(type);
                LocalCache.Add(stringKeys, value, lifetime);
            }
        }

        public static bool TryGet(object[] keys, Type resultType, out object value)
        {
            value = null;
            if (keys == null || keys.Length == 0) return false;
            var stringKeys = Methods.GetStringKeys(keys);
            var genericArgumentType = GetGenericArgumentType(resultType);
            var cache = LocalTypes.Contains(resultType) || (genericArgumentType != null && LocalTypes.Contains(genericArgumentType)) ? LocalCache : GlobalCache;
            return cache.Get(stringKeys, out value);
        }

        private static Type GetGenericArgumentType(Type type)
        {
            var types = type.GetGenericArguments();
            if (types.Length > 0) return types[0];
            return type.HasElementType ? type.GetElementType() : null;
        }
    }
}