using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Factories
{
    public static class PoolExtensions<T>
    {
        private static readonly ManualResetEvent Mre = new ManualResetEvent(false);
        private static readonly ConcurrentBag<T> Pool = new ConcurrentBag<T>();
        private static FuncWrapper _creator;
        private static long _ticks;
        private static int _size;

        static PoolExtensions()
        {
            _size = 1;
            (new Thread(Filling) { IsBackground = true }).Start();
        }

        public static T Get(int size)
        {
            _size = size > 0 ? size : _size;
            return Get();
        }

        public static T GetSet(FuncWrapper creator)
        {
            _creator = creator;
            return Get();
        }

        public static T Get(int size, FuncWrapper creator)
        {
            _size = size > 0 ? size : _size;
            _creator = creator;
            return Get();
        }

        private static T Get()
        {
            T instance;
            instance = Pool.TryTake(out instance) ? instance : CreateInstance();
            Mre.Set();
            return instance;
        }

        public static void Set(T instance)
        {
            Pool.Add(instance);
        }

        private static void Filling()
        {
            while (true)
            {
                if (Interlocked.Increment(ref _ticks) >= long.MaxValue)
                {
                    break;
                }
                if (Pool.Count < _size)
                {
                    Pool.Add(CreateInstance());
                    continue;
                }
                Mre.WaitOne();
                Mre.Reset();
            }
        }

        private static T CreateInstance()
        {
            return _creator != null ? (T)_creator.Execute() : Je.sys.CreateInstance<T>();
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

        public static object Get(Type type, int size)
        {
            return GetMethods.GetOrAdd(GenericOf(type), x => FindMethod(x, "Get"))?.Invoke(null, new object[] { size });
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