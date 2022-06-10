using System.Collections.Concurrent;
using System.Collections.Generic;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Settings
{
    public class SettingCache
    {
        private readonly ConcurrentDictionary<string, setting> _cache = new ConcurrentDictionary<string, setting>();

        public void Set(string key, object value)
        {
            var setting = new setting(value.As<string>(), true);
            _cache.AddOrUpdate(key, x => setting, (x, y) => setting);
        }

        public bool Is(string key, string defaultValue)
        {
            return GetSetting(key, defaultValue).is_declared;
        }

        public string Get(string key, string defaultValue)
        {
            return GetSetting(key, defaultValue).value;
        }

        private setting GetSetting(string key, string defaultValue)
        {
            var setting = new setting(defaultValue, false);
            setting = _cache.GetOrAdd(key, x => setting);
            return setting;
        }

        private class setting
        {
            public readonly bool is_declared;
            public readonly string value;
            public setting(string value, bool is_declared)
            {
                this.is_declared = is_declared;
                this.value = value;
            }
        }
    }
}