using System;
using System.Configuration;
using System.Collections.Generic;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Bases
{
    public abstract class ConfigSetting<T> : MarshalByRefObject, ISetting
    {
        public bool IsDeclared { get; private set; }
        public T Value { get; private set; }
        private readonly string _typeName;

        protected ConfigSetting(T defaultValue)
        {
            _typeName = GetType().Name;
            IsDeclared = ConfigExtensions.IsDeclared(_typeName);
            Value = GetValue(defaultValue);
        }

        protected virtual T GetValue(T defaultValue)
        {
            return ConfigExtensions.GetValue(_typeName, defaultValue);
        }

        bool ISetting.IsDeclared()
        {
            return IsDeclared;
        }

        string ISetting.GetValue()
        {
            return Value.As<string>();
        }
    }

    public static class ConfigExtensions
    {
        private static Dictionary<string, string> AppSettings { get { return Je.One.Get(AppSettingsHolder, GetAppSettings); } }
        private static readonly Dictionary<string, string>[] AppSettingsHolder = { null };

        private static Dictionary<string, string> GetAppSettings()
        {
            var dict = new Dictionary<string, string>();
            foreach (string s in ConfigurationManager.AppSettings.AllKeys)
            {
                var key = s.ToLower();
                if (!dict.ContainsKey(key)) dict.Add(key, ConfigurationManager.AppSettings[key]);
            }
            return dict;
        }

        public static bool IsDeclared(string key)
        {
            lock (AppSettingsHolder)
            {
                try
                {
                    return AppSettings.ContainsKey(key.ToLower());
                }
                catch
                {
                    return false;
                }
            }
        }

        public static T GetValue<T>(string key, T defaultValue)
        {
            lock (AppSettingsHolder)
            {
                try
                {
                    key = key.ToLower();
                    return AppSettings.ContainsKey(key) ? AppSettings[key].As<T>() : defaultValue;
                }
                catch
                {
                    return defaultValue;
                }
            }
        }
    }
}

namespace Jetproger.Tools.AppConfig
{
    public class ConnectionString : ConfigSetting<string> { public ConnectionString() : base("") { } }
    public class ServerHost : ConfigSetting<string> { public ServerHost() : base("127.0.0.1") { } }
}