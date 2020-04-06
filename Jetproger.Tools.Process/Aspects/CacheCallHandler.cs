using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Jetproger.Tools.Cache.Bases;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Inject.Bases;
using Microsoft.Practices.Unity.InterceptionExtension;
using Newtonsoft.Json;

namespace Jetproger.Tools.Process.Aspects
{
    public class CacheCallHandler : ICallHandler
    {
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }
        public string Properties { get; set; }
        public bool Enabled { get; set; }
        public int LifeTime { get; set; }
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (!Enabled) return Ex.Inject.Call(input, getNext);
            var methodId = Ex.Inject.Info(input, TypeName);
            var type = input.Target.GetType().BaseType;
            var keys = new List<object>();
            if (!string.IsNullOrWhiteSpace(AssemblyName) && !string.IsNullOrWhiteSpace(TypeName))
            {
                keys.Add(AssemblyName);
                keys.Add(TypeName);
                keys.Add(methodId);
                keys.AddRange(GetKeys(input.Target, type));
                Cache.Bases.Cache.ClearEx(keys.ToArray());
                return input.CreateMethodReturn(Ex.Inject.Call(input, getNext).ReturnValue);
            }
            string assemblyName, typeName;
            Ex.Dotnet.ParseName(type, out assemblyName, out typeName);
            keys.Add(assemblyName);
            keys.Add(typeName);
            keys.Add(methodId);
            keys.AddRange(GetKeys(input.Target, type));
            var parameters = keys.ToArray();
            object objectValue;
            if (Ex.Cache.Read(parameters, out objectValue))
            {
                var returnType = ((MethodInfo)input.MethodBase).ReturnType;
                var returnValue = ValueExtensions.IsPrimitive(returnType) ? objectValue.As(returnType) : DeserializeJson(objectValue.ToString(), returnType);
                return input.CreateMethodReturn(returnValue);
            }
            var value = Ex.Inject.Call(input, getNext).ReturnValue;
            Cache.Bases.Cache.WriteEx(parameters, value, LifeTime);
            return input.CreateMethodReturn(value);
        }

        private object[] GetKeys(object source, Type type)
        {
            if (string.IsNullOrWhiteSpace(Properties)) return new object[0];
            var properties = Properties.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var keys = new List<object>();
            foreach (var name in properties)
            {
                var p = Ex.Dotnet.GetProperty(type, name);
                if (p == null) continue;
                keys.Add(p.GetValue(source, null));
            }
            return keys.ToArray();
        }

        private static object DeserializeJson(string json, Type resultType)
        {
            if (ValueExtensions.IsPrimitive(resultType))
            {
                return json.As(resultType);
            }
            using (var sr = new StringReader(json))
            {
                return JsonSerializer.Deserialize(sr, resultType);
            }
        }

        private static readonly JsonSerializer JsonSerializer = new JsonSerializer {
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
    }

    public class CacheAspectAttribute : UnityAspectAttribute
    {
        public CacheAspectAttribute() : base(typeof(CacheCallHandler)) { }
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }
        public string Properties { get; set; }
        public int LifeTime { get; set; }
    }
}