using System;
using System.Reflection;
using System.Resources;
using System.Threading;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Cache.Bases
{
    public static class Cache
    {
        private static readonly CacheProxy GlobalCache = Ex.GetOne<CacheProxy>();

        #region Read

        public static bool Read<T, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, out T p)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 }, out p);
        }

        public static bool Read<T, T0, T1, T2, T3, T4, T5, T6, T7, T8>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, out T p)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 }, out p);
        }

        public static bool Read<T, T0, T1, T2, T3, T4, T5, T6, T7>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, out T p)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7 }, out p);
        }

        public static bool Read<T, T0, T1, T2, T3, T4, T5, T6>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, out T p)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4, p5, p6 }, out p);
        }

        public static bool Read<T, T0, T1, T2, T3, T4, T5>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, out T p)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4, p5 }, out p);
        }

        public static bool Read<T, T0, T1, T2, T3, T4>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, out T p)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4 }, out p);
        }

        public static bool Read<T, T0, T1, T2, T3>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, out T p)
        {
            return ReadEx(new object[] { p0, p1, p2, p3 }, out p);
        }

        public static bool Read<T, T0, T1, T2>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, out T p)
        {
            return ReadEx(new object[] { p0, p1, p2 }, out p);
        }

        public static bool Read<T, T0, T1>(this ICacheExpander expander, T0 p0, T1 p1, out T p)
        {
            return ReadEx(new object[] { p0, p1 }, out p);
        }

        public static bool Read<T, T0>(this ICacheExpander expander, T0 p0, out T p)
        {
            return ReadEx(new object[] { p0 }, out p);
        }


        #endregion

        #region Read or write

        public static T Read<T, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, Func<T> func, int lifetime = 0)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 }, func, lifetime);
        }

        public static T Read<T, T0, T1, T2, T3, T4, T5, T6, T7, T8>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, Func<T> func, int lifetime = 0)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 }, func, lifetime);
        }

        public static T Read<T, T0, T1, T2, T3, T4, T5, T6, T7>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, Func<T> func, int lifetime = 0)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7 }, func, lifetime);
        }

        public static T Read<T, T0, T1, T2, T3, T4, T5, T6>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, Func<T> func, int lifetime = 0)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4, p5, p6 }, func, lifetime);
        }

        public static T Read<T, T0, T1, T2, T3, T4, T5>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, Func<T> func, int lifetime = 0)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4, p5 }, func, lifetime);
        }

        public static T Read<T, T0, T1, T2, T3, T4>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, Func<T> func, int lifetime = 0)
        {
            return ReadEx(new object[] { p0, p1, p2, p3, p4 }, func, lifetime);
        }

        public static T Read<T, T0, T1, T2, T3>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, Func<T> func, int lifetime = 0)
        {
            return ReadEx(new object[] { p0, p1, p2, p3 }, func, lifetime);
        }

        public static T Read<T, T0, T1, T2>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, Func<T> func, int lifetime = 0)
        {
            return ReadEx(new object[] { p0, p1, p2 }, func, lifetime);
        }

        public static T Read<T, T0, T1>(this ICacheExpander expander, T0 p0, T1 p1, Func<T> func, int lifetime = 0)
        {
            return ReadEx(new object[] { p0, p1 }, func, lifetime);
        }

        public static T Read<T, T0>(this ICacheExpander expander, T0 p0, Func<T> func, int lifetime = 0)
        {
            return ReadEx(new object[] { p0 }, func, lifetime);
        }


        #endregion

        #region Write

        public static void Write<T, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, Func<T> func, int lifetime = 0)
        {
            WriteEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 }, func(), lifetime);
        }

        public static void Write<T, T0, T1, T2, T3, T4, T5, T6, T7, T8>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, Func<T> func, int lifetime = 0)
        {
            WriteEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 }, func(), lifetime);
        }

        public static void Write<T, T0, T1, T2, T3, T4, T5, T6, T7>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, Func<T> func, int lifetime = 0)
        {
            WriteEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7 }, func(), lifetime);
        }

        public static void Write<T, T0, T1, T2, T3, T4, T5, T6>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, Func<T> func, int lifetime = 0)
        {
            WriteEx(new object[] { p0, p1, p2, p3, p4, p5, p6 }, func(), lifetime);
        }

        public static void Write<T, T0, T1, T2, T3, T4, T5>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, Func<T> func, int lifetime = 0)
        {
            WriteEx(new object[] { p0, p1, p2, p3, p4, p5 }, func(), lifetime);
        }

        public static void Write<T, T0, T1, T2, T3, T4>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, Func<T> func, int lifetime = 0)
        {
            WriteEx(new object[] { p0, p1, p2, p3, p4 }, func(), lifetime);
        }

        public static void Write<T, T0, T1, T2, T3>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, Func<T> func, int lifetime = 0)
        {
            WriteEx(new object[] { p0, p1, p2, p3 }, func(), lifetime);
        }

        public static void Write<T, T0, T1, T2>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, Func<T> func, int lifetime = 0)
        {
            WriteEx(new object[] { p0, p1, p2 }, func(), lifetime);
        }

        public static void Write<T, T0, T1>(this ICacheExpander expander, T0 p0, T1 p1, Func<T> func, int lifetime = 0)
        {
            WriteEx(new object[] { p0, p1 }, func(), lifetime);
        }

        public static void Write<T, T0>(this ICacheExpander expander, T0 p0, Func<T> func, int lifetime = 0)
        {
            WriteEx(new object[] { p0 }, func(), lifetime);
        }

        #endregion

        #region Clear

        public static void Clear<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
        {
            ClearEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 });
        }

        public static void Clear<T0, T1, T2, T3, T4, T5, T6, T7, T8>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            ClearEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 });
        }

        public static void Clear<T0, T1, T2, T3, T4, T5, T6, T7>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            ClearEx(new object[] { p0, p1, p2, p3, p4, p5, p6, p7 });
        }

        public static void Clear<T0, T1, T2, T3, T4, T5, T6>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            ClearEx(new object[] { p0, p1, p2, p3, p4, p5, p6 });
        }

        public static void Clear<T0, T1, T2, T3, T4, T5>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            ClearEx(new object[] { p0, p1, p2, p3, p4, p5 });
        }

        public static void Clear<T0, T1, T2, T3, T4>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            ClearEx(new object[] { p0, p1, p2, p3, p4 });
        }

        public static void Clear<T0, T1, T2, T3>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2, T3 p3)
        {
            ClearEx(new object[] { p0, p1, p2, p3 });
        }

        public static void Clear<T0, T1, T2>(this ICacheExpander expander, T0 p0, T1 p1, T2 p2)
        {
            ClearEx(new object[] { p0, p1, p2 });
        }

        public static void Clear<T0, T1>(this ICacheExpander expander, T0 p0, T1 p1)
        {
            ClearEx(new object[] { p0, p1 });
        }

        public static void Clear<T0>(this ICacheExpander expander, T0 p0)
        {
            ClearEx(new object[] { p0 });
        }

        #endregion

        public static void ClearEx(object[] keys)
        {
            if (keys == null || keys.Length == 0) return;
            keys = NormalizeKeys(keys);
            GlobalCache.Clear(keys);
        }

        public static T ReadEx<T>(object[] keys, Func<T> func, int lifetime)
        {
            if (keys == null || keys.Length == 0) return default(T);
            T value;
            if (ReadEx(keys, out value)) return value;
            value = func();
            WriteEx(keys, value, lifetime);
            return value;
        }

        public static void WriteEx<T>(object[] keys, T value, int lifetime)
        {
            if (keys == null || keys.Length == 0) return;
            var cacheValue = IsNative(value) ? value : (object)(Ex.Value.WriteEx<T>(value));
            keys = NormalizeKeys(keys);
            try
            {
                GlobalCache.Write(keys, cacheValue, lifetime);
            }
            catch
            {
                Thread.Sleep(111);
            }
        }

        public static bool ReadEx<T>(object[] keys, out T p)
        {
            p = default(T);
            if (keys == null || keys.Length == 0) return false;
            keys = NormalizeKeys(keys);
            object value;
            if (GlobalCache.Read(keys, out value))
            {
                p = value is ValueSet ? Ex.Value.ReadEx<T>((ValueSet)value) : value.As<T>();
                return true;
            }
            return false;
        }

        private static object[] NormalizeKeys(object[] keys)
        {
            var normalKeys = new object[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                if (key == null || key == DBNull.Value) key = string.Empty;
                var type = key.GetType();
                if (!ValueExtensions.IsPrimitive(type)) key = string.Empty;
                normalKeys[i] = key;
            }
            return normalKeys;
        }

        private static bool IsNative(object value)
        {
            var type = value != null && value != DBNull.Value ? value.GetType() : null;
            return type == null || ValueExtensions.IsPrimitive(type) || value is ValueSet || value is Attribute || value is Type || value is Assembly || value is ResourceManager ||
                value is PropertyInfo || value is PropertyInfo[] ||
                value is MethodInfo || value is MethodInfo[] ||
                value is FieldInfo || value is FieldInfo[];
        }
    }
}