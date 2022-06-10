using System;
using System.Collections.Generic;
using System.Linq;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Settings
{
    public abstract class ConsoleSetting : Setting
    {
        protected ConsoleSetting(string defaultValue, params string[] keys)
        {
            var list = new List<string>();
            list.Add(GetType().Name.ToLower());
            list.AddRange(keys);
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = (list[i] ?? string.Empty).ToLower();
            }
            IsDeclared = ConsoleSettings.Is(list, defaultValue);
            Value = ConsoleSettings.Get(list, defaultValue);
        }
    }

    public static class ConsoleSettings
    {
        private static readonly SettingCache Cache = new SettingCache();

        public static void Initialize(string[] args)
        {
            foreach (var arg in args)
            {
                if (!ParseArgument(arg, out var key, out var value)) continue;
                key = key.ToLower();
                Cache.Set(key, value);
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
            if (colonIndex == 0)
            {
                return false;
            }
            key = arg.Substring(0, colonIndex);
            colonIndex++;
            if (colonIndex < arg.Length)
            {
                value = arg.Substring(colonIndex);
            }
            return true;
        }

        public static bool Is(IEnumerable<string> keys, string defaultValue)
        {
            foreach (var key in keys)
            {
                if (Cache.Is(key, defaultValue)) return true;
            }
            return false;
        }

        public static string Get(IEnumerable<string> keys, string defaultValue)
        {
            foreach (var key in keys)
            {
                if (Cache.Is(key, defaultValue)) return Cache.Get(key, defaultValue);
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