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

        public static T GetSet(FuncWrapper creator)
        {
            var value = Get();
            if (value != null) return value;
            value = (T)creator.Execute();
            Set(value);
            return value;
        }

        public static T Get()
        {
            return OneExtensions.Get(null, Holder);
        }

        public static void Set(T value)
        {
            OneExtensions.Set(null, Holder, value);
        }
    }

    public static class OneExtensions
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
            return typeof(OneExtensions<>).MakeGenericType(type);
        }

        public static T Get<T>(this f.IOneExpander e, T[] holder, Func<T> factory) where T : class
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

        public static T Get<T>(this f.IOneExpander e, T?[] holder, Func<T> factory) where T : struct
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

        public static T Get<T>(this f.IOneExpander e, T[] holder) where T : class
        {
            lock (holder)
            {
                return holder[0];
            }
        }

        public static T Get<T>(this f.IOneExpander e, T?[] holder) where T : struct
        {
            lock (holder)
            {
                return holder[0] ?? default(T);
            }
        }

        public static void Set<T>(this f.IOneExpander e, T[] holder, T value) where T : class
        {
            lock (holder)
            {
                holder[0] = value;
            }
        }

        public static void Set<T>(this f.IOneExpander e, T?[] holder, T value) where T : struct
        {
            lock (holder)
            {
                holder[0] = value;
            }
        }
    }

    [Serializable]
    public class FuncWrapper
    {
        private Func<object> _func;

        public FuncWrapper()
        {
        }

        public FuncWrapper(Func<object> func)
        {
            _func = func;
        }

        public object Execute()
        {
            return _func();
        }
    }
}