using System.Collections.Concurrent;

namespace Jetproger.Tools.Metadata.Bases
{
    public static class CacheCore
    {
        private static readonly ConcurrentDictionary<string, object> Values = new ConcurrentDictionary<string, object>();

        public static void Add(string[] keys, object value)
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
    }
}