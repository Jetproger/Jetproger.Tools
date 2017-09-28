using System;
using Jetproger.Tools.Metadata.Bases;

namespace Tools
{
    public static partial class Metadata
    {
        private static class Cache
        {
            private static readonly CacheLoader[] CacheLoaders = {null};

            private static CacheLoader Remote
            {
                get
                {
                    if (CacheLoaders[0] == null)
                    {
                        lock (CacheLoaders)
                        {
                            if (CacheLoaders[0] == null)
                            {
                                CacheLoaders[0] = InitializeCache();
                            }
                        }
                    }
                    return CacheLoaders[0];
                }
            }

            private static CacheLoader InitializeCache()
            {
                var setup = new AppDomainSetup
                {
                    ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                    ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    LoaderOptimization = LoaderOptimization.SingleDomain,
                    ShadowCopyFiles = "true"
                };
                var name = $"f__{(typeof(CacheCore)).AssemblyQualifiedName}";
                var domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
                var type = typeof (CacheLoader);
                var assemblyName = type.Assembly.GetName().Name;
                var typeName = type.FullName ?? string.Empty;
                var instance = domain.CreateInstanceAndUnwrap(assemblyName, typeName) as CacheLoader;
                return instance;
            }

            #region Get or add

            public static T Get<T, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5,
                T6 p6, T7 p7, T8 p8, T9 p9, Func<T> func, int lifetime = 0)
            {
                return TryGetOrAdd(new object[] {p0, p1, p2, p3, p4, p5, p6, p7, p8, p9}, func, lifetime);
            }

            public static T Get<T, T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6,
                T7 p7, T8 p8, Func<T> func, int lifetime = 0)
            {
                return TryGetOrAdd(new object[] {p0, p1, p2, p3, p4, p5, p6, p7, p8}, func, lifetime);
            }

            public static T Get<T, T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6,
                T7 p7, Func<T> func, int lifetime = 0)
            {
                return TryGetOrAdd(new object[] {p0, p1, p2, p3, p4, p5, p6, p7}, func, lifetime);
            }

            public static T Get<T, T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6,
                Func<T> func, int lifetime = 0)
            {
                return TryGetOrAdd(new object[] {p0, p1, p2, p3, p4, p5, p6}, func, lifetime);
            }

            public static T Get<T, T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, Func<T> func,
                int lifetime = 0)
            {
                return TryGetOrAdd(new object[] {p0, p1, p2, p3, p4, p5}, func, lifetime);
            }

            public static T Get<T, T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, Func<T> func, int lifetime = 0)
            {
                return TryGetOrAdd(new object[] {p0, p1, p2, p3, p4}, func, lifetime);
            }

            public static T Get<T, T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3, Func<T> func, int lifetime = 0)
            {
                return TryGetOrAdd(new object[] {p0, p1, p2, p3}, func, lifetime);
            }

            public static T Get<T, T0, T1, T2>(T0 p0, T1 p1, T2 p2, Func<T> func, int lifetime = 0)
            {
                return TryGetOrAdd(new object[] {p0, p1, p2}, func, lifetime);
            }

            public static T Get<T, T0, T1>(T0 p0, T1 p1, Func<T> func, int lifetime = 0)
            {
                return TryGetOrAdd(new object[] {p0, p1}, func, lifetime);
            }

            public static T Get<T, T0>(T0 p0, Func<T> func, int lifetime = 0)
            {
                return TryGetOrAdd(new object[] {p0}, func, lifetime);
            }

            #endregion

            public static T TryGetOrAdd<T>(object[] keys, Func<T> func, int lifetime)
            {
                T value;
                if (TryGet(keys, out value)) return value;
                value = func();
                var type = value != null ? value.GetType() : typeof (T);
                object obj = value;
                if (obj != null && !Methods.IsSimple(type)) obj = Methods.SerializeJson(value);
                var stringKeys = Methods.GetStringKeys(keys);
                Remote.Add(stringKeys, obj);
                return value;
            }

            public static bool TryGet<T>(object[] keys, out T p)
            {
                object value;
                var result = TryGet(keys, typeof (T), out value);
                p = result ? (T) value : default(T);
                return result;
            }

            public static bool TryGet(object[] keys, Type resultType, out object value)
            {
                value = null;
                if (keys == null || keys.Length == 0) return false;
                var stringKeys = Methods.GetStringKeys(keys);
                var result = Remote.Get(stringKeys, out value);
                if (value == DBNull.Value) value = null;
                if (value != null && !Methods.IsSimple(resultType)) value = Methods.DeserializeJson(value.ToString(), resultType);
                return result;
            }
        }
    }
}