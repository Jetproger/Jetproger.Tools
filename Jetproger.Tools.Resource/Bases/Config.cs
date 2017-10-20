using System.Configuration;
using Jetproger.Tools.Resource.Bases;
namespace Tools
{
    public static partial class Resource
    {
        private static readonly bool?[] IsReadConfig = { null };

        public static ResourceItem<T> GetConfigurationValue<T>(string key, T defaultValue = default(T))
        {
            string text;
            key = key.ToLower();
            var isDeclared = Cache.Get("appconfiguration", key, out text);
            if (!isDeclared) return new ResourceItem<T>(key, $"@{key}", false, false, defaultValue);
            T value;
            var isValid = Methods.TryStringAs(text, defaultValue, out value);
            return new ResourceItem<T>(key, text, true, isValid, value);
        }

        public static void ReadAppConfiguration()
        {
            lock (IsReadConfig)
            {
                if (IsReadConfig[0] == null)
                {
                    IsReadConfig[0] = true;
                    foreach (var key in ConfigurationManager.AppSettings.AllKeys)
                    {
                        Cache.Get("appconfiguration", key.ToLower(), () => ConfigurationManager.AppSettings[key]);
                    }
                }
            }
        }
    }
}