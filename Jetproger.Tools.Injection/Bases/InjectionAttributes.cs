using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jetproger.Tools.Injection.Bases
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MapOfDependencyInjectionAttribute : Attribute
    {
        public string MapOf
        {
            get; private set;
        }

        public MapOfDependencyInjectionAttribute(string mapOf)
        {
            MapOf = mapOf;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class LifetimeDependencyInjectionAttribute : Attribute
    {
        public string Lifetime
        {
            get; private set;
        }

        public LifetimeDependencyInjectionAttribute(string lifetime)
        {
            Lifetime = lifetime;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class AspectAttribute : Attribute
    {
        private readonly string _callHandlerTypeName;

        public int Order { get; set; }

        public bool Enabled { get; set; }

        protected AspectAttribute(Type callHandlerType)
        {
            _callHandlerTypeName = callHandlerType.Name;
        }

        public string GetCallHandlerTypeName()
        {
            return _callHandlerTypeName;
        }

        public IEnumerable<UnityPropertyItem> GetUnityProperties()
        {
            try
            {
                var list = new List<UnityPropertyItem>();
                foreach (var p in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (p.Name == "TypeId") continue;
                    var value = p.GetValue(this)?.ToString();
                    if (value == null) continue;
                    list.Add(new UnityPropertyItem { name = p.Name, value = value });
                }
                return list.ToArray();
            }
            catch
            {
                return new UnityPropertyItem[0];
            }
        }
    }
}