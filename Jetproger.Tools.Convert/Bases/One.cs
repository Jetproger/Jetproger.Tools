using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Convert.Bases
{
    public static class OneExtensions
    {
        public static T Get<T>(this Jc.IOneExpander exp, T[] holder, Func<T> factory) where T : class
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

        public static T Get<T>(this Jc.IOneExpander exp, T?[] holder, Func<T> factory) where T : struct
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

        public static T Get<T>(this Jc.IOneExpander exp, T[] holder) where T : class
        {
            lock (holder)
            {
                return holder[0];
            }
        }

        public static T Get<T>(this Jc.IOneExpander exp, T?[] holder) where T : struct
        {
            lock (holder)
            {
                return holder[0] ?? default(T);
            }
        }

        public static void Reset<T>(this Jc.IOneExpander exp, T[] holder) where T : class
        {
            lock (holder)
            {
                holder[0] = null;
            }
        }

        public static void Reset<T>(this Jc.IOneExpander exp, T?[] holder) where T : struct
        {
            lock (holder)
            {
                holder[0] = null;
            }
        }
    }
}

namespace Jc
{
    public static class One<T> where T : class, new()
    {
        private static readonly T[] Holder = { null };

        public static T Ge
        {
            get
            {
                if (Holder[0] == null)
                {
                    lock (Holder)
                    {
                        if (Holder[0] == null) Holder[0] = new T();
                    }
                }
                return Holder[0];
            }
        }

        public static T Get(Func<T> factory)
        {
            if (Holder[0] == null)
            {
                lock (Holder)
                {
                    if (Holder[0] == null) Holder[0] = factory();
                }
            }
            return Holder[0];
        }

        public static void Reset()
        {
            lock (Holder)
            {
                Holder[0] = null;
            }
        }
    }

    public static class One<TKey, TValue>
    {
        private static readonly Dictionary<TKey, TValue> Pairs = new Dictionary<TKey, TValue>();

        public static bool Get(TKey key, out TValue value)
        {
            lock (Pairs)
            {
                value = Pairs.ContainsKey(key) ? Pairs[key] : default(TValue);
                return Pairs.ContainsKey(key);
            }
        }

        public static TValue Get(TKey key, Func<TKey, TValue> factory)
        {
            if (!Pairs.ContainsKey(key))
            {
                lock (Pairs)
                {
                    if (!Pairs.ContainsKey(key)) Pairs.Add(key, factory(key));
                }
            }
            return Pairs[key];
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> Get()
        {
            lock (Pairs)
            {
                foreach (KeyValuePair<TKey, TValue> pair in Pairs)
                {
                    yield return pair;
                }
            }
        }

        public static void Reset(TKey key)
        {
            lock (Pairs)
            {
                if (Pairs.ContainsKey(key)) Pairs.Remove(key);
            }
        }

        public static void Reset()
        {
            lock (Pairs)
            {
                Pairs.Clear();
            }
        }
    }
}