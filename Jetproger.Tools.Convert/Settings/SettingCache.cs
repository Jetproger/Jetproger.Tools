using System.Collections.Concurrent;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Settings
{
    public class SettingCache
    {
        private readonly ConcurrentDictionary<string, _Setting> _cache = new ConcurrentDictionary<string, _Setting>();

        public void SetValue(string key, object value)
        {
            _cache.AddOrUpdate(normof(key), x => new _Setting(value.As<string>(), true), (x, y) => new _Setting(value.As<string>(), true));
        }

        public bool IsDeclared(string key, string defaultValue)
        {
            return GetSetting(key, defaultValue).IsDeclared;
        }

        public string GetValue(string key, string defaultValue)
        {
            return GetSetting(key, defaultValue).Value;
        }

        private _Setting GetSetting(string key, string defaultValue)
        {
            return _cache.GetOrAdd(normof(key), x => new _Setting(defaultValue, false));
        }

        private static string normof(string key)
        {
            return (key ?? string.Empty).ToLower().Trim(' ', '\n', '\r', 't');
        }

        #region inner types

        private class _Setting
        {
            public readonly bool IsDeclared;
            public readonly string Value;
            public _Setting(string value, bool isDeclared)
            {
                IsDeclared = isDeclared;
                Value = value;
            }
        }

        #endregion
    }
}