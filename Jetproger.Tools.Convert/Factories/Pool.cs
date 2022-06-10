using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Jetproger.Tools.Convert.Factories
{
    public static class PoolExtensions<T>
    {
        private static readonly ConcurrentBag<T> Pool = new ConcurrentBag<T>();

        public static void Set(T value)
        {
            Pool.Add(value);
        }

        public static T GetSet(FuncWrapper creator)
        {
            T value;
            return Get(out value) ? value : (T)creator.Execute();
        }

        public static T Get()
        {
            T value;
            return Get(out value) ? value : default(T);
        }

        private static bool Get(out T value)
        {
            value = default(T);
            var counter = 0;
            while (true)
            {
                if (counter++ > 3) break;
                if (Pool.TryTake(out value)) return true;
                Thread.Sleep(999);
            }
            return false;
        }
    }

    public static class PoolExtensions
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> GetMethods = new ConcurrentDictionary<Type, MethodInfo>();

        private static readonly ConcurrentDictionary<Type, MethodInfo> SetMethods = new ConcurrentDictionary<Type, MethodInfo>();

        public static object GetSet(Type type, FuncWrapper creator)
        {
            return GetMethods.GetOrAdd(GenericOf(type), x => FindMethod(x, "GetSet"))?.Invoke(null, new[] { creator as object });
        }

        public static object Get(Type type)
        {
            return GetMethods.GetOrAdd(GenericOf(type), x => FindMethod(x, "Get"))?.Invoke(null, new object[0]);
        }

        public static void Set(Type type, object value)
        {
            SetMethods.GetOrAdd(GenericOf(type), x => FindMethod(x, "Set"))?.Invoke(null, new[] { value });
        }

        private static MethodInfo FindMethod(Type type, string name)
        { 
            return type.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name == name);
        }

        private static Type GenericOf(Type type)
        {
            return typeof(PoolExtensions<>).MakeGenericType(type);
        }
    }
}