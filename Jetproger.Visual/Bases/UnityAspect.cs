using System;
using System.Collections.Generic;
using System.Reflection;

namespace Jetproger.Tools.Inject.Bases
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class UnityAspectAttribute : Attribute
    {
        private readonly string _callHandlerTypeName;

        public int Order { get; set; }
        public bool Enabled { get; set; }

        protected UnityAspectAttribute(Type callHandlerType)
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