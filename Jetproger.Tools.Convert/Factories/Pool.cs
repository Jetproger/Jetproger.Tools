using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Factories
{
    public static class PoolExtensions<T>
    {
        private static readonly ConcurrentBag<T> Pool = new ConcurrentBag<T>();

        public static void Set(T value)
        {
            Pool.Add(value);
        }

        public static T Get()
        {
            return Pool.TryTake(out var value) ? value : f.sys.defof<T>();
        }

        public static T GetSet(SimpleCreator creator)
        {
            return Pool.TryTake(out var value) ? value : (T)creator.Create();
        }
    }

    public static class PoolExtensions
    {
        private static MethodInfo _M(Type type, string name) { return type.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name == name); }
        
        private static Type _T(Type type) { return GenericTypes.GetOrAdd(type, x => typeof(PoolExtensions<>).MakeGenericType(x)); }
        private static readonly ConcurrentDictionary<Type, Type> GenericTypes = new ConcurrentDictionary<Type, Type>();

        private static readonly ConcurrentDictionary<Type, MethodInfo> GetSetMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> GetMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> SetMethods = new ConcurrentDictionary<Type, MethodInfo>();

        public static void Set(Type type, object value)
        {
            SetMethods.GetOrAdd(_T(type), x => _M(x, "Set"))?.Invoke(null, new object[] { value });
        }

        public static object Get(Type type)
        {
            return GetMethods.GetOrAdd(_T(type), x => _M(x, "Get"))?.Invoke(null, new object[0]);
        }

        public static object GetSet(Type type, SimpleCreator creator)
        {
            return GetSetMethods.GetOrAdd(_T(type), x => _M(x, "GetSet"))?.Invoke(null, new object[] { creator });
        }
    }
}