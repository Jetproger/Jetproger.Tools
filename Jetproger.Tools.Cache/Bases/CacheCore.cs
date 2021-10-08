using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;

namespace Jetproger.Tools.Cache.Bases
{
    public static class CacheCore
    {
        private static readonly ConcurrentDictionary<object, object> Values = new ConcurrentDictionary<object, object>();

        public static void Write(object[] keys, object value, int lifetime)
        {
            if (keys == null || keys.Length == 0) return;
            var lastIndex = keys.Length - 1;
            var currentKey = (object)null;
            var dict = Values;
            var counter = 0;
            foreach (var key in keys)
            {
                currentKey = key ?? string.Empty;
                if (counter++ == lastIndex) break;
                dict = (ConcurrentDictionary<object, object>)(dict.AddOrUpdate(currentKey, AddValueFactory, UpdateValueFactory));
            }
            dict.AddOrUpdate(currentKey ?? string.Empty, x => value, (x, y) => value);
            if (lifetime > 0) TryClear(keys, lifetime);
        }

        private static object AddValueFactory(object key)
        {
            return new ConcurrentDictionary<object, object>();
        }

        private static object UpdateValueFactory(object key, object value)
        {
            return value is ConcurrentDictionary<object, object> ? value : new ConcurrentDictionary<object, object>();
        }

        public static bool Read(object[] keys, out object value)
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
                dict = o as ConcurrentDictionary<object, object>;
                if (dict == null) return false;
            }
            value = o;
            return true;
        }

        public static void Clear(object[] keys)
        {
            if (keys == null || keys.Length == 0) return;
            var lastIndex = keys.Length - 1;
            var currentKey = (object)null;
            var dict = Values;
            var counter = 0;
            foreach (var key in keys)
            {
                currentKey = key ?? string.Empty;
                object value;
                if (!dict.TryGetValue(currentKey, out value)) return;
                if (counter++ == lastIndex) break;
                dict = value as ConcurrentDictionary<object, object>;
                if (dict == null) return;
            }
            object o;
            dict.TryRemove(currentKey ?? string.Empty, out o);
            TryClearAll(o);
        }

        private static void TryClear(object[] keys, int lifetime)
        {
            try
            {
                var proc = new Action<object[], int>(BeginClear);
                proc.BeginInvoke(keys, lifetime, EndClear, proc);
            }
            catch
            {
                Thread.Sleep(111);
            }
        }

        private static void BeginClear(object[] keys, int lifetime)
        {
            try
            {
                Thread.Sleep(lifetime);
                Clear(keys);
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
                ((Action<object[], int>)asyncResult.AsyncState).EndInvoke(asyncResult);
            }
            catch
            {
                Thread.Sleep(111);
            }
        }

        private static void TryClearAll(object o)
        {
            try
            {
                var proc = new Action<object>(BeginClearAll);
                proc.BeginInvoke(o, EndClearAll, proc);
            }
            catch
            {
                Thread.Sleep(111);
            }
        }

        private static void BeginClearAll(object o)
        {
            try
            {
                ClearAll(o);
                GarbageCollect();
            }
            catch
            {
                Thread.Sleep(111);
            }
        }

        private static void EndClearAll(IAsyncResult asyncResult)
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

        private static void ClearAll(object o)
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
                        foreach (var item in list) ClearAll(item);
                        list.Clear();
                        return;
                    }
                }
                var dict = o as IDictionary;
                if (dict != null)
                {
                    lock (dict)
                    {
                        foreach (var item in dict.Values) ClearAll(item);
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