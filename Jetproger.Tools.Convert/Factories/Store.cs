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

        public static TValue GetSet(TKey key, FuncArgWrapper creator)
        {
            return Store.GetOrAdd(key, (TValue)creator.Execute(key));
        } 

        public static TValue Get(TKey key)
        {
            TValue value;
            return Store.TryGetValue(key, out value) ? value : Je.sys.DefaultOf<TValue>();
        }

        public static void Set(TKey key, TValue value)
        {
            Store.TryAdd(key, value);
        }
    }

    public static class StoreExtensions
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> GetMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> SetMethods = new ConcurrentDictionary<Type, MethodInfo>();

        public static object GetSet(Type keyType, Type valueType, object key, FuncArgWrapper creator)
        {
            return GetMethods.GetOrAdd(GenericOf(keyType, valueType), x => FindMethod(x, "GetSet"))?.Invoke(null, new[] { key, creator });
        }

        public static object Get(Type keyType, Type valueType, object key)
        {
            return GetMethods.GetOrAdd(GenericOf(keyType, valueType), x => FindMethod(x, "Get"))?.Invoke(null, new[] { key });
        }

        public static void Set(Type keyType, Type valueType, object key, object value)
        {
            SetMethods.GetOrAdd(GenericOf(keyType, valueType), x => FindMethod(x, "Set"))?.Invoke(null, new[] { key, value });
        }

        private static MethodInfo FindMethod(Type type, string name)
        {
            return type.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name == name);
        }

        private static Type GenericOf(Type keyType, Type valueType)
        {
            return typeof(StoreExtensions<,>).MakeGenericType(keyType, valueType);
        }
    }

    [Serializable]
    public class FuncArgWrapper
    {
        private Func<object, object> _func;

        public FuncArgWrapper()
        {
        }

        public FuncArgWrapper(Func<object, object> func)
        {
            _func = func;
        }

        public object Execute(object arg)
        {
            return _func(arg);
        }
    }
}