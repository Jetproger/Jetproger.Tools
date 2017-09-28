using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Tools
{
    public static partial class Metadata
    {
        private static class Property
        {
            private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Tuple<Type, Delegate, Delegate>>> Setters = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Tuple<Type, Delegate, Delegate>>>();


            public static Tuple<Type, Delegate, Delegate> GetTupleSetter(Type type, string name)
            {
                var dictionaryDelegates = GetDictionaryDelegates(type);
                if (dictionaryDelegates.Count == 0) GenerateSetters(type, dictionaryDelegates);
                Tuple<Type, Delegate, Delegate> tuple;
                return dictionaryDelegates.TryGetValue(name, out tuple) ? tuple : null;
            }

            public static void GenerateSetters(Type type, ConcurrentDictionary<string, Tuple<Type, Delegate, Delegate>> dictionaryDelegates)
            {
                foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
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

            public static ConcurrentDictionary<string, Tuple<Type, Delegate, Delegate>> GetDictionaryDelegates(Type type)
            {
                return Setters.GetOrAdd(type, x => new ConcurrentDictionary<string, Tuple<Type, Delegate, Delegate>>());
            }

            public static string ToNormName(string name)
            {
                return name.Replace("_", "").ToLower();
            }
        }
    }
}