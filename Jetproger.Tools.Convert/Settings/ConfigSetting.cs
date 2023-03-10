using System.Configuration; 
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Settings
{
    public abstract class ConfigSetting : Setting
    {
        private static readonly SettingCache Cache = new SettingCache();

        protected ConfigSetting(string defaultValue)
        {
            var key = GetType().Name;
            Value = Cache.GetValue(key, defaultValue);
            IsDeclared = Cache.IsDeclared(key, defaultValue);
        }

        public static void Parse()
        {
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                Cache.SetValue(key, ConfigurationManager.AppSettings[key]);
            }
        }
    }
}

namespace Jetproger.Tools.AppConfig
{
    public class Culture : ConfigSetting { public Culture() : base("en-US") { } }
}