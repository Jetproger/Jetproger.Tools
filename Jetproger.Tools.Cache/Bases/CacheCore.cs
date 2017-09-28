using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;

namespace Jetproger.Tools.Cache.Bases
{
    public static class CacheCore
    {
        private static readonly ConcurrentDictionary<string, object> Values = new ConcurrentDictionary<string, object>();

        public static void Add(string[] keys, object value, int lifetime)
        {
            if (keys == null || keys.Length == 0) return;
            var lastIndex = keys.Length - 1;
            var currentKey = string.Empty;
            var dict = Values;
            var counter = 0;
            foreach (var key in keys)
            {
                currentKey = key ?? string.Empty;
                if (counter++ == lastIndex) break;
                dict = (ConcurrentDictionary<string, object>)(dict.AddOrUpdate(currentKey, x => new ConcurrentDictionary<string, object>(),
                (x, y) => y is ConcurrentDictionary<string, object> ? y : new ConcurrentDictionary<string, object>()));
            }
            dict.AddOrUpdate(currentKey, x => value, (x, y) => value);
            if (lifetime > 0) AsyncOff(keys, lifetime);
        }

        public static bool Get(string[] keys, out object value)
        {
            value = null;
            if (keys == null || keys.Length == 0) return false;
            var lastIndex = keys.Length - 1;
            var o = (object)null;
            var dict = Values;
            var counter = 0;
            foreach (var key in keys)
            {
                if (!dict.TryGetValue(key ?? string.Empty, out o)) return false;
                if (counter++ == lastIndex) break;
                dict = o as ConcurrentDictionary<string, object>;
                if (dict == null) return false;
            }
            value = o;
            return true;
        }

        public static void Off(string[] keys)
        {
            if (keys == null || keys.Length == 0) return;
            var lastIndex = keys.Length - 1;
            var currentKey = string.Empty;
            var dict = Values;
            var counter = 0;
            foreach (var key in keys)
            {
                currentKey = key ?? string.Empty;
                object value;
                if (!dict.TryGetValue(currentKey, out value)) return;
                if (counter++ == lastIndex) break;
                dict = value as ConcurrentDictionary<string, object>;
                if (dict == null) return;
            }
            object o;
            dict.TryRemove(currentKey, out o);
            AsyncClear(o);
        }

        private static void AsyncOff(string[] keys, int lifetime)
        {
            try
            {
                var proc = new Action<string[], int>(BeginOff);
                proc.BeginInvoke(keys, lifetime, EndOff, proc);
            }
            catch
            {
                Thread.Sleep(111);
            }
        }

        private static void BeginOff(string[] keys, int lifetime)
        {
            try
            {
                Thread.Sleep(lifetime);
                Off(keys);
            }
            catch
            {
                Thread.Sleep(111);
            }
        }

        private static void EndOff(IAsyncResult asyncResult)
        {
            try
            {
                ((Action<string[], int>)asyncResult.AsyncState).EndInvoke(asyncResult);
            }
            catch
            {
                Thread.Sleep(111);
            }
        }
        private static void AsyncClear(object o)
        {
            try
            {
                var proc = new Action<object>(BeginClear);
                proc.BeginInvoke(o, EndClear, proc);
            }
            catch
            {
                Thread.Sleep(111);
            }
        }

        private static void BeginClear(object o)
        {
            try
            {
                Clear(o);
                GarbageCollect();
            }
            catch
            {
                Thread.Sleep(111);
            }
        }

        private static void EndClear(IAsyncResult asyncResult)
        {
            try
            {
                ((Action<object>)asyncResult.AsyncState).EndInvoke(asyncResult);
            }
            catch
            {
                Thread.Sleep(111);
            }
        }

        private static void Clear(object o)
        {
            try
            {
                var disposable = o as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                    return;
                }
                var list = o as IList;
                if (list != null)
                {
                    lock (list)
                    {
                        foreach (var item in list) Clear(item);
                        list.Clear();
                        return;
                    }
                }
                var dict = o as IDictionary;
                if (dict != null)
                {
                    lock (dict)
                    {
                        foreach (var item in dict.Values) Clear(item);
                        dict.Clear();
                        return;
                    }
                }
            }
            catch
            {
                Thread.Sleep(111);
            }
        }

        private static void GarbageCollect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect(2);
            Thread.Sleep(111);
        }
    }
}