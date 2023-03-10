using System;
using System.Collections.Generic;
using System.Linq;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Settings
{
    public abstract class ConsoleSetting : Setting
    {
        private static readonly SettingCache Cache = new SettingCache();

        protected ConsoleSetting(string defaultValue, params string[] keys)
        {
            var list = new List<string> { GetType().Name };
            list.AddRange(keys);
            IsDeclared = GetDeclared(list, defaultValue);
            Value = GetValue(list, defaultValue);
        }

        public static void Parse(string[] args)
        {
            foreach (var arg in args)
            {
                if (ParseArgument(arg, out var key, out var value)) Cache.SetValue(key, value);
            }
        }

        private static bool ParseArgument(string arg, out string key, out string value)
        {
            key = null;
            value = null;
            if (string.IsNullOrWhiteSpace(arg)) return false;
            arg = arg.ToLower();
            var colonIndex = arg.IndexOf(":", StringComparison.Ordinal);
            if (colonIndex < 0)
            {
                key = arg;
                return true;
            }
            if (colonIndex == 0) return false;
            key = arg.Substring(0, colonIndex);
            colonIndex++;
            if (colonIndex < arg.Length)
            {
                value = arg.Substring(colonIndex);
            }
            return true;
        }

        private static bool GetDeclared(IEnumerable<string> keys, string defaultValue)
        {
            return keys.Any(key => Cache.IsDeclared(key, defaultValue));
        }

        private static string GetValue(IEnumerable<string> keys, string defaultValue)
        {
            foreach (var key in keys)
            {
                if (Cache.IsDeclared(key, defaultValue)) return Cache.GetValue(key, defaultValue);
            }
            return defaultValue;
        }
    }
}

namespace Jetproger.Tools.AppConsole
{
    public class Uninstall : ConsoleSetting { public Uninstall() : base(string.Empty, "u") { } } 
    public class Install : ConsoleSetting { public Install() : base(string.Empty, "i") { } }
}