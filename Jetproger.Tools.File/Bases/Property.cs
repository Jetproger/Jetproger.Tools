using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Tools
{
    public static partial class File
    {
        private static class Property
        {
            private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Tuple<Type, Delegate, Delegate>>> Setters = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Tuple<Type, Delegate, Delegate>>>();
            private static readonly ConcurrentDictionary<Type, PropertyInfo[]> Properties = new ConcurrentDictionary<Type, PropertyInfo[]>();

            private static Tuple<Type, Delegate, Delegate> GetTupleSetter(Type type, string name)
            {
                var dictionaryDelegates = GetDictionaryDelegates(type);
                if (dictionaryDelegates.Count == 0) GenerateSetters(type, dictionaryDelegates);
                Tuple<Type, Delegate, Delegate> tuple;
                return dictionaryDelegates.TryGetValue(name, out tuple) ? tuple : null;
            }

            private static void GenerateSetters(Type type, ConcurrentDictionary<string, Tuple<Type, Delegate, Delegate>> dictionaryDelegates)
            {
                foreach (var p in GetProperties(type))
                {
                    if (!p.CanWrite) continue;
                    var name = ToNormName(p.Name);
                    var targetExp = Expression.Parameter(type, "target");
                    var valueExp = Expression.Parameter(p.PropertyType, "value");
                    var propertyExp = Expression.Property(targetExp, p);
                    var getter = Expression.Lambda(propertyExp, targetExp).Compile();
                    var assignExp = Expression.Assign(propertyExp, valueExp);
                    var setter = Expression.Lambda(assignExp, targetExp, valueExp).Compile();
                    dictionaryDelegates.TryAdd(name, new Tuple<Type, Delegate, Delegate>(p.PropertyType, getter, setter));
                }
            }

            private static ConcurrentDictionary<string, Tuple<Type, Delegate, Delegate>> GetDictionaryDelegates(Type type)
            {
                return Setters.GetOrAdd(type, x => new ConcurrentDictionary<string, Tuple<Type, Delegate, Delegate>>());
            }

            private static string ToNormName(string name)
            {
                return name.Replace("_", "").ToLower();
            }

            public static object GetValue(object instance, string propertyName)
            {
                if (instance == null) return null;
                var type = instance.GetType();
                var name = ToNormName(propertyName);
                var tuple = GetTupleSetter(type, name);
                if (tuple == null) return null;
                var getter = tuple.Item2;
                return getter.DynamicInvoke(instance);
            }

            public static void SetValue(object instance, string propertyName, object value)
            {
                if (instance == null) return;
                var type = instance.GetType();
                var name = ToNormName(propertyName);
                var tuple = GetTupleSetter(type, name);
                if (tuple == null) return;
                var propertyType = tuple.Item1;
                value = value.As(propertyType);
                var setter = tuple.Item3;
                setter.DynamicInvoke(instance, value);
            }

            public static PropertyInfo[] GetProperties(Type type)
            {
                return Properties.GetOrAdd(type, x => { return (x.GetProperties(BindingFlags.Instance | BindingFlags.Public)).Where(p => p.PropertyType.IsSimple()).ToArray(); });
            }
        }
    }
}