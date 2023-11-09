using System;
using System.Collections;
using System.Collections.Generic;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Convert.Caches
{
    public class CacheStore : MarshalByRefObject
    {
        private readonly _N _cache = new _N();

        public bool Get(CommandRecord record, out object result)
        {
            var holder = GetHolder(record);
            result = holder.Result;
            Clear();
            return holder.Lifetime != null;
        }

        public void Set(CommandRecord record, object result)
        {
            var holder = GetHolder(record);
            holder.Result = result;
            holder.Lifetime = (f.sys.valueof(record.Lifetime) as ICacheLifetime) ?? new DefaultCacheLifetime();
            Clear();
        }

        public void Set(CommandRecord record)
        {
            var dict = _cache
            .Get(record.Name ?? string.Empty)
            .Get(record.P_ ?? string.Empty)
            .Get(record.P0 ?? string.Empty)
            .Get(record.P1 ?? string.Empty)
            .Get(record.P2 ?? string.Empty)
            .Get(record.P3 ?? string.Empty)
            .Get(record.P4 ?? string.Empty)
            .Get(record.P5 ?? string.Empty)
            .Get(record.P6 ?? string.Empty)
            .Get(record.P7 ?? string.Empty)
            .Get(record.P8 ?? string.Empty);
            Clear();
            lock (dict)
            {
                if (!dict.ContainsKey(record.P9)) return;
                var holder = dict[record.P9];
                dict.Remove(record.P9);
                holder.Lifetime = null;
                holder.Result = null;
            }
        }

        private _R GetHolder(CommandRecord record)
        {
            return _cache
            .Get(record.Name ?? string.Empty)
            .Get(record.P_ ?? string.Empty)
            .Get(record.P0 ?? string.Empty)
            .Get(record.P1 ?? string.Empty)
            .Get(record.P2 ?? string.Empty)
            .Get(record.P3 ?? string.Empty)
            .Get(record.P4 ?? string.Empty)
            .Get(record.P5 ?? string.Empty)
            .Get(record.P6 ?? string.Empty)
            .Get(record.P7 ?? string.Empty)
            .Get(record.P8 ?? string.Empty)
            .Get(record.P9 ?? string.Empty);
        }

        private void Clear()
        {
            var proc = (Action)BeginClear;
            proc.BeginInvoke(EndClear, proc);
        }

        private void BeginClear()
        {
            try
            {
                BeginClear(_cache);
            }
            catch (Exception e)
            {
                f.log(e);
            }
        }

        private void BeginClear(IDictionary dict)
        {
            var deleted = new ArrayList();
            foreach (DictionaryEntry entry in dict)
            {
                if (entry.Value is IDictionary dictValue)
                {
                    BeginClear(dictValue);
                    if (dictValue.Count == 0) deleted.Add(entry.Key);
                }
                if (entry.Value is _R holder)
                {
                    if (holder.Lifetime == null || holder.Lifetime.IsExpired())
                    {
                        deleted.Add(entry.Key);
                        holder.Lifetime = null;
                        holder.Result = null;
                    }
                }
            }
            foreach (var key in deleted) dict.Remove(key);
        }

        private void EndClear(IAsyncResult asyncResult)
        {
            try
            {
                ((Action)asyncResult.AsyncState).EndInvoke(asyncResult);
            }
            catch (Exception e)
            {
                f.log(e);
            }
        }

        #region inner types

        private class _N : Dictionary<string, _V> { public _V Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _V()); return this[key]; } } }
        private class _V : Dictionary<string, _0> { public _0 Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _0()); return this[key]; } } }
        private class _0 : Dictionary<string, _1> { public _1 Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _1()); return this[key]; } } }
        private class _1 : Dictionary<string, _2> { public _2 Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _2()); return this[key]; } } }
        private class _2 : Dictionary<string, _3> { public _3 Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _3()); return this[key]; } } }
        private class _3 : Dictionary<string, _4> { public _4 Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _4()); return this[key]; } } }
        private class _4 : Dictionary<string, _5> { public _5 Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _5()); return this[key]; } } }
        private class _5 : Dictionary<string, _6> { public _6 Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _6()); return this[key]; } } }
        private class _6 : Dictionary<string, _7> { public _7 Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _7()); return this[key]; } } }
        private class _7 : Dictionary<string, _8> { public _8 Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _8()); return this[key]; } } }
        private class _8 : Dictionary<string, _9> { public _9 Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _9()); return this[key]; } } }
        private class _9 : Dictionary<string, _R> { public _R Get(string key) { lock (this) { if (!ContainsKey(key)) Add(key, new _R()); return this[key]; } } }
        private class _R { public ICacheLifetime Lifetime; public object Result; }


        #endregion
    }

    public class DefaultCacheLifetime : ICacheLifetime
    {
        public bool IsExpired() { return (DateTime.UtcNow - _timestamp) >= Timespan; }
        private static readonly TimeSpan Timespan = TimeSpan.FromMinutes(20);
        private readonly DateTime _timestamp = DateTime.UtcNow;
    }

    public class InfinityCacheLifetime : ICacheLifetime
    {
        public bool IsExpired() { return false; }
    }

    public interface ICacheLifetime
    {
        bool IsExpired();
    }
}