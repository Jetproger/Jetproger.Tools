using System;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Caches
{
    public static class CacheExtensions
    {
        private static readonly CacheStore CacheClient = f.one.of(CacheClientHolder, () => new CacheStore());
        private static readonly CacheStore[] CacheClientHolder = { null };
        private static readonly CacheStore CacheServer = t<CacheStore>.one();
        private static readonly CacheContext Context = t<CacheContext>.one();

        #region func

        public static T of<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9), func); }
        public static T of<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4, p5, p6, p7, p8), func); }
        public static T of<T, T_, T0, T1, T2, T3, T4, T5, T6, T7>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4, p5, p6, p7), func); }
        public static T of<T, T_, T0, T1, T2, T3, T4, T5, T6>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4, p5, p6), func); }
        public static T of<T, T_, T0, T1, T2, T3, T4, T5>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4, p5), func); }
        public static T of<T, T_, T0, T1, T2, T3, T4>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4), func); }
        public static T of<T, T_, T0, T1, T2, T3>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3), func); }
        public static T of<T, T_, T0, T1, T2>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime, p_, p0, p1, p2), func); }
        public static T of<T, T_, T0, T1>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime, p_, p0, p1), func); }
        public static T of<T, T_, T0>(this f.IMemExpander expander, string name, T_ p_, T0 p0, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime, p_, p0), func); }
        public static T of<T, T_>(this f.IMemExpander expander, string name, T_ p_, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime, p_), func); }
        public static T of<T>(this f.IMemExpander expander, string name, Func<T> func, string lifetime = null) { return of(expander, keyof(expander, name, lifetime), func); }

        #endregion

        #region get

        public static bool of<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, out T p) { return of(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9), out p); }
        public static bool of<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, out T p) { return of(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4, p5, p6, p7, p8), out p); }
        public static bool of<T, T_, T0, T1, T2, T3, T4, T5, T6, T7>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, out T p) { return of(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4, p5, p6, p7), out p); }
        public static bool of<T, T_, T0, T1, T2, T3, T4, T5, T6>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, out T p) { return of(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4, p5, p6), out p); }
        public static bool of<T, T_, T0, T1, T2, T3, T4, T5>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, out T p) { return of(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4, p5), out p); }
        public static bool of<T, T_, T0, T1, T2, T3, T4>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, out T p) { return of(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4), out p); }
        public static bool of<T, T_, T0, T1, T2, T3>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, out T p) { return of(expander, keyof(expander, name, null, p_, p0, p1, p2, p3), out p); }
        public static bool of<T, T_, T0, T1, T2>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, out T p) { return of(expander, keyof(expander, name, null, p_, p0, p1, p2), out p); }
        public static bool of<T, T_, T0, T1>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, out T p) { return of(expander, keyof(expander, name, null, p_, p0, p1), out p); }
        public static bool of<T, T_, T0>(this f.IMemExpander expander, string name, T_ p_, T0 p0, out T p) { return of(expander, keyof(expander, name, null, p_, p0), out p); }
        public static bool of<T, T_>(this f.IMemExpander expander, string name, T_ p_, out T p) { return of(expander, keyof(expander, name, null, p_), out p); }
        public static bool of<T>(this f.IMemExpander expander, string name, out T p) { return of(expander, keyof(expander, name, null), out p); }

        #endregion

        #region set

        public static void to<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9), value); }
        public static void to<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4, p5, p6, p7, p8), value); }
        public static void to<T, T_, T0, T1, T2, T3, T4, T5, T6, T7>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4, p5, p6, p7), value); }
        public static void to<T, T_, T0, T1, T2, T3, T4, T5, T6>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4, p5, p6), value); }
        public static void to<T, T_, T0, T1, T2, T3, T4, T5>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4, p5), value); }
        public static void to<T, T_, T0, T1, T2, T3, T4>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3, p4), value); }
        public static void to<T, T_, T0, T1, T2, T3>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime, p_, p0, p1, p2, p3), value); }
        public static void to<T, T_, T0, T1, T2>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime, p_, p0, p1, p2), value); }
        public static void to<T, T_, T0, T1>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime, p_, p0, p1), value); }
        public static void to<T, T_, T0>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime, p_, p0), value); }
        public static void to<T, T_>(this f.IMemExpander expander, string name, T_ p_, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime, p_), value); }
        public static void to<T>(this f.IMemExpander expander, string name, T value, string lifetime = null) { to(expander, keyof(expander, name, lifetime), value); }

        #endregion

        #region remove

        public static void to<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9) { to(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9)); }
        public static void to<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8) { to(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4, p5, p6, p7, p8)); }
        public static void to<T, T_, T0, T1, T2, T3, T4, T5, T6, T7>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7) { to(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4, p5, p6, p7)); }
        public static void to<T, T_, T0, T1, T2, T3, T4, T5, T6>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6) { to(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4, p5, p6)); }
        public static void to<T, T_, T0, T1, T2, T3, T4, T5>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5) { to(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4, p5)); }
        public static void to<T, T_, T0, T1, T2, T3, T4>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3, T4 p4) { to(expander, keyof(expander, name, null, p_, p0, p1, p2, p3, p4)); }
        public static void to<T, T_, T0, T1, T2, T3>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2, T3 p3) { to(expander, keyof(expander, name, null, p_, p0, p1, p2, p3)); }
        public static void to<T, T_, T0, T1, T2>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1, T2 p2) { to(expander, keyof(expander, name, null, p_, p0, p1, p2)); }
        public static void to<T, T_, T0, T1>(this f.IMemExpander expander, string name, T_ p_, T0 p0, T1 p1) { to(expander, keyof(expander, name, null, p_, p0, p1)); }
        public static void to<T, T_, T0>(this f.IMemExpander expander, string name, T_ p_, T0 p0) { to(expander, keyof(expander, name, null, p_, p0)); }
        public static void to<T, T_>(this f.IMemExpander expander, string name, T_ p_) { to(expander, keyof(expander, name, null, p_)); }
        public static void to<T>(this f.IMemExpander expander, string name) { to(expander, keyof(expander, name, null)); }


        #endregion

        #region command

        public static bool of(this f.IMemExpander expander, IParameterizedCommand cmd, out object result)
        {
            result = null;
            var lifetime = Context.LifetimeOf(cmd.GetType());
            if (lifetime == null) return false;
            var record = cmd.As<CommandRecord>();
            record.Lifetime = lifetime;
            return of(expander, record, out result);
        }

        public static void to(this f.IMemExpander expander, IParameterizedCommand cmd, object result)
        {
            var lifetime = Context.LifetimeOf(cmd.GetType());
            if (lifetime == null) return;
            var record = cmd.As<CommandRecord>();
            record.Lifetime = lifetime;
            to(expander, record, result);
        }

        public static void to(this f.IMemExpander expander, IParameterizedCommand cmd)
        {
            var lifetime = Context.LifetimeOf(cmd.GetType());
            if (lifetime == null) return;
            var record = cmd.As<CommandRecord>();
            record.Lifetime = lifetime;
            to(expander, record);
        }

        #endregion

        #region base 

        public static T of<T>(this f.IMemExpander expander, CommandRecord key, Func<T> func)
        {
            if (of(expander, key, out T result)) return result;
            result = func();
            to(expander, key, result);
            return result;
        }

        public static bool of<T>(this f.IMemExpander expander, CommandRecord key, out T result)
        {
            var isSuccess = of(expander, key, out object o);
            result = o.As<T>();
            return isSuccess;
        }

        public static bool of(this f.IMemExpander expander, CommandRecord key, out object result)
        {
            return serverof(key, out result) || clientof(key, out result);
        }

        public static void to(this f.IMemExpander expander, CommandRecord key, object result)
        {
            if (!serverto(key, result)) clientto(key, result);
        }

        public static void to(this f.IMemExpander expander, CommandRecord key)
        {
            if (!serverto(key)) clientto(key);
        }

        public static CommandRecord keyof(this f.IMemExpander expander, string name, string lifetime, params object[] args)
        {
            args = args ?? new object[0];
            var len = args.Length;
            return new CommandRecord
            {
                Name = name ?? string.Empty,
                Lifetime = !string.IsNullOrWhiteSpace(lifetime) ? lifetime : f.sys.printof<DefaultCacheLifetime>(),
                P_ = len > 0 && args[0] != null && f.sys.issimple(args[0].GetType()) ? args[0].As<string>() : string.Empty,
                P0 = len > 1 && args[1] != null && f.sys.issimple(args[1].GetType()) ? args[1].As<string>() : string.Empty,
                P1 = len > 2 && args[2] != null && f.sys.issimple(args[2].GetType()) ? args[2].As<string>() : string.Empty,
                P2 = len > 3 && args[3] != null && f.sys.issimple(args[3].GetType()) ? args[3].As<string>() : string.Empty,
                P3 = len > 4 && args[4] != null && f.sys.issimple(args[4].GetType()) ? args[4].As<string>() : string.Empty,
                P4 = len > 5 && args[5] != null && f.sys.issimple(args[5].GetType()) ? args[5].As<string>() : string.Empty,
                P5 = len > 6 && args[6] != null && f.sys.issimple(args[6].GetType()) ? args[6].As<string>() : string.Empty,
                P6 = len > 7 && args[7] != null && f.sys.issimple(args[7].GetType()) ? args[7].As<string>() : string.Empty,
                P7 = len > 8 && args[8] != null && f.sys.issimple(args[8].GetType()) ? args[8].As<string>() : string.Empty,
                P8 = len > 9 && args[9] != null && f.sys.issimple(args[9].GetType()) ? args[9].As<string>() : string.Empty,
                P9 = len > 10 && args[10] != null && f.sys.issimple(args[10].GetType()) ? args[10].As<string>() : string.Empty,
            };
        }

        private static bool clientof(CommandRecord key, out object result)
        {
            result = CacheClient.Get(key, out result);
            return true;
        }

        private static bool serverof(CommandRecord key, out object result)
        {
            result = null;
            try
            {
                result = CacheServer.Get(key, out result);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void clientto(CommandRecord key, object result)
        {
            CacheClient.Set(key, result); 
        }

        private static bool serverto(CommandRecord key, object result)
        {  
            try
            {
                CacheServer.Set(key, result);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void clientto(CommandRecord key)
        {
            CacheClient.Set(key);
        }

        private static bool serverto(CommandRecord key)
        {
            try
            {
                CacheServer.Set(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}