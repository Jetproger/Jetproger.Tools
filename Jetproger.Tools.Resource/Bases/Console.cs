using System;
using Jetproger.Tools.Resource.Bases;

namespace Tools
{
    public static partial class Resource
    {
        private static readonly bool?[] IsReadConsole = { null };

        public static ResourceItem<T> GetCommandLineValue<T>(string key, T defaultValue = default(T))
        {
            string text;
            key = key.ToLower();
            var isDeclared = Cache.Get("commandlinearguments", key, out text);
            if (!isDeclared) return new ResourceItem<T>(key, $"@{key}", false, false, defaultValue);
            T value;
            var isValid = Methods.TryStringAs(text, defaultValue, out value);
            return new ResourceItem<T>(key, text, true, isValid, value);
        }

        public static void ReadCommandLineArguments(string[] args)
        {
            lock (IsReadConsole)
            {
                if (IsReadConsole[0] == null)
                {
                    IsReadConsole[0] = true;
                    foreach (var arg in args)
                    {
                        string key, value;
                        if (!ParseArgument(arg, out key, out value)) continue;
                        key = key.ToLower();
                        Cache.Get("commandlinearguments", key, () => value);
                    }
                }
            }
        }

        private static bool ParseArgument(string arg, out string key, out string value)
        {
            key = null;
            value = null;
            if (string.IsNullOrWhiteSpace(arg)) return false;
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
            if (colonIndex < arg.Length) value = arg.Substring(colonIndex);
            return true;
        }
    }
}