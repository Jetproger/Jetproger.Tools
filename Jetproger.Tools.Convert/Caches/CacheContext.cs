using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Caches
{
    public class CacheContext : MarshalByRefObject
    {
        private static readonly string DefaultCacheLifetimeName = f.sys.printof<DefaultCacheLifetime>();
        private readonly Dictionary<Type, string>[] _lifetimes = { null };

        public string LifetimeOf(Type type)
        {
            var lifetimes = _lifetimes[0];
            if (_lifetimes[0] == null) {
                lock (_lifetimes) {
                    if (_lifetimes[0] == null) _lifetimes[0] = LoadLifetimes();
                    lifetimes = _lifetimes[0];
                }
            }
            return lifetimes.ContainsKey(type) ? lifetimes[type] : null;
        }

        private Dictionary<Type, string> LoadLifetimes()
        {
            var lifetimes = new Dictionary<Type, string>();
            foreach (KeyValuePair<Type, string> pair in LoadConfigLifetimes())
            {
                if (!lifetimes.ContainsKey(pair.Key)) lifetimes.Add(pair.Key, pair.Value);
            }
            foreach (KeyValuePair<Type, string> pair in LoadAttributeLifetimes())
            {
                if (!lifetimes.ContainsKey(pair.Key)) lifetimes.Add(pair.Key, pair.Value);
            }
            return lifetimes;
        }

        private IEnumerable<KeyValuePair<Type, string>> LoadAttributeLifetimes()
        {
            foreach (var pair in f.sys.attrall<CacheAttribute>())
            {
                yield return new KeyValuePair<Type, string>(pair.Type, !string.IsNullOrWhiteSpace(pair.Attribute.Lifetime) ? pair.Attribute.Lifetime : DefaultCacheLifetimeName);
            }
        }

        private IEnumerable<KeyValuePair<Type, string>> LoadConfigLifetimes()
        {
            var file = f.fss.pathnameextof("cache.config");
            if (!File.Exists(file)) yield break;
            var xml = File.ReadAllText(file, Encoding.UTF8);
            var config = xml.As<SimpleXml>().As<CacheConfig>();
            foreach (ConfigEntry entry in config.Entries)
            {
                var type = f.sys.classof(entry.Type);
                if (type == null) continue;
                var lifetime = entry.Value.As<string>();
                yield return  new KeyValuePair<Type, string>(type, !string.IsNullOrWhiteSpace(lifetime) ? lifetime : DefaultCacheLifetimeName);
            }
        }
    }
}