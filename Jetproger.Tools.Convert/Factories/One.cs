using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Factories
{
    public static class OneExtensions<T> where T : class
    {
        private static readonly T[] Holder = { null };

        public static void Set(T value)
        {
            f.one.to(Holder, value);
        }

        public static T Get()
        {
            return f.one.of(Holder);
        }

        public static T GetSet(SimpleCreator creator)
        {
            var value = Get();
            if (value != null) return value;
            Set((T)creator.Create());
            return Get();
        }
    }

    public static class OneExtensions
    {
        private static MethodInfo _M(Type type, string name) { return type.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name == name); }
        private static Type _T(Type type) { return GenericTypes.GetOrAdd(type, x => typeof(OneExtensions<>).MakeGenericType(x)); }
        private static readonly ConcurrentDictionary<Type, MethodInfo> GetSetMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> GetMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> SetMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, Type> GenericTypes = new ConcurrentDictionary<Type, Type>();

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

    public static class OneMethods
    { 
        public static T of<T>(this f.IOneExpander e, T[] holder, Func<T> factory) where T : class
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

        public static T of<T>(this f.IOneExpander e, T?[] holder, Func<T> factory) where T : struct
        {
            if (holder[0] == null)
            {
                lock (holder)
                {
                    if (holder[0] == null) holder[0] = factory();
                }
            }
            return (T) holder[0];
        }

        public static T of<T>(this f.IOneExpander e, T[] holder) where T : class
        {
            lock (holder)
            {
                return holder[0];
            }
        }

        public static T of<T>(this f.IOneExpander e, T?[] holder) where T : struct
        {
            lock (holder)
            {
                return holder[0] ?? default(T);
            }
        }

        public static void to<T>(this f.IOneExpander e, T[] holder, T value) where T : class
        {
            lock (holder)
            {
                holder[0] = value;
            }
        }

        public static void to<T>(this f.IOneExpander e, T?[] holder, T value) where T : struct
        {
            lock (holder)
            {
                holder[0] = value;
            }
        }
    }
}