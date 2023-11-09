using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Factories
{
    public static class StoreExtensions<TKey, TValue>
    {
        
        private static readonly ConcurrentDictionary<TKey, TValue> Store = new ConcurrentDictionary<TKey, TValue>();

        public static void Set(TKey key, TValue value)
        {
            Store.TryAdd(key, value);
        }

        public static TValue Get(TKey key)
        {
            return Store.TryGetValue(key, out var value) ? value : f.sys.defof<TValue>();
        }

        public static TValue GetSet(TKey key, ParamCreator creator)
        {
            return Store.GetOrAdd(key, (TValue)creator.Create(key));
        }
    }

    public static class StoreExtensions
    {
        private static Type _T(Type keyType, Type valueType) { return __T(keyType).GetOrAdd(valueType, x => typeof(StoreExtensions<,>).MakeGenericType(keyType, valueType)); }
        private static ConcurrentDictionary<Type, Type> __T(Type keyType) { return GenericTypes.GetOrAdd(keyType, x => new ConcurrentDictionary<Type, Type>()); }
        private static MethodInfo _M(Type type, string name) { return type.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name == name); }
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, Type>> GenericTypes = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, Type>>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> GetSetMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> GetMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> SetMethods = new ConcurrentDictionary<Type, MethodInfo>();

        public static void Set(Type keyType, Type valueType, object key, object value)
        {
            SetMethods.GetOrAdd(_T(keyType, valueType), x => _M(x, "Set"))?.Invoke(null, new object[] { key, value });
        }

        public static object Get(Type keyType, Type valueType, object key)
        {
            return GetMethods.GetOrAdd(_T(keyType, valueType), x => _M(x, "Get"))?.Invoke(null, new object[] { key });
        }

        public static object GetSet(Type keyType, Type valueType, object key, ParamCreator creator)
        {
            return GetSetMethods.GetOrAdd(_T(keyType, valueType), x => _M(x, "GetSet"))?.Invoke(null, new object[] { key, creator });
        }
    }
}