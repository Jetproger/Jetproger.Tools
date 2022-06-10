using System.Configuration; 
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Settings
{
    public abstract class ConfigSetting : Setting
    { 
        protected ConfigSetting(string defaultValue)
        {
            var key = GetType().Name.ToLower();
            Value = ConfigSettings.Get(key, defaultValue);
            IsDeclared = ConfigSettings.Is(key, defaultValue);
        }
    }

    public static class ConfigSettings
    {

        private static readonly SettingCache Cache = new SettingCache();

        public static void Initialize()
        {
            foreach (string s in ConfigurationManager.AppSettings.AllKeys)
            {
                var key = s.ToLower();
                Cache.Set(key, ConfigurationManager.AppSettings[key]);
            }
        }

        public static bool Is(string key, string defaultValue)
        {
            return Cache.Is(key, defaultValue);
        }

        public static string Get(string key, string defaultValue)
        {
            return Cache.Get(key, defaultValue);
        }
    }
}

namespace Jetproger.Tools.AppConfig
{
    public class Culture : ConfigSetting { public Culture() : base("en-US") { } }
}