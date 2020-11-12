using System;
using System.Collections.Generic;
using System.Linq;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Bases
{
    public abstract class ConsoleSetting<T> : MarshalByRefObject, ISetting
    {
        protected readonly IEnumerable<string> Keys;
        public bool IsDeclared { get; private set; }
        public T Value { get; private set; }

        protected ConsoleSetting(T defaultValue, params string[] keys)
        {
            var list = new List<string>();
            list.Add(GetType().Name);
            list.AddRange(keys);
            for (int i = 0; i < list.Count; i++) list[i] = (list[i] ?? string.Empty).ToLower();
            Keys = list;
            IsDeclared = ConsoleExtensions.IsDeclared(Keys);
            Value = GetValue(defaultValue);
        }

        protected virtual T GetValue(T defaultValue)
        {
            return ConsoleExtensions.GetValue(Keys, defaultValue);
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

    public static class ConsoleExtensions
    {
        private static readonly Dictionary<string, string> Arguments = new Dictionary<string, string>();

        public static void ParseArguments(string[] args)
        {
            lock (Arguments)
            {
                Arguments.Clear();
                foreach (var arg in args)
                {
                    string key, value;
                    if (!ParseArgument(arg, out key, out value)) continue;
                    key = key.ToLower();
                    if (!Arguments.ContainsKey(key)) Arguments.Add(key, value);
                }
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

        public static bool IsDeclared(IEnumerable<string> keys)
        {
            lock (Arguments)
            {
                try
                {
                    return keys.Any(arg => Arguments.ContainsKey(arg));
                }
                catch
                {
                    return false;
                }
            }
        }

        public static T GetValue<T>(IEnumerable<string> keys, T defaultValue)
        {
            lock (Arguments)
            {
                try
                {
                    foreach (var key in keys)
                    {
                        if (Arguments.ContainsKey(key)) return Arguments[key].As<T>();
                    }
                    return defaultValue;
                }
                catch
                {
                    return defaultValue;
                }
            }
        }
    }
}
namespace Jetproger.Tools.AppConsole
{
    public class Uninstall : ConsoleSetting<string> { public Uninstall() : base(string.Empty, "u") { } }
    public class Install : ConsoleSetting<string> { public Install() : base(string.Empty, "i") { } }
}