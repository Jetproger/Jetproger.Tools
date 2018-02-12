using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Jetproger.Tools.Convert.Bases;
using Microsoft.Practices.Unity.InterceptionExtension;
using Newtonsoft.Json;
using TDI = Tools.DI;
using TM = Tools.Metadata;
using TC = Tools.Cache;
using Tools;

namespace Jetproger.Tools.Plugin.Aspects
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
            if (!Enabled) return TDI.AOPExecute(input, getNext);
            var methodId = TDI.AOPDeclaration(input);
            var type = input.Target.GetType().BaseType;
            var keys = new List<object>();
            if (!string.IsNullOrWhiteSpace(AssemblyName) && !string.IsNullOrWhiteSpace(TypeName))
            {
                keys.Add(AssemblyName);
                keys.Add(TypeName);
                keys.Add(methodId);
                keys.AddRange(GetKeys(input.Target, type));
                TC.TryRemove(keys.ToArray());
                return input.CreateMethodReturn(TDI.AOPExecute(input, getNext).ReturnValue);
            }
            string assemblyName, typeName;
            TM.GetNames(type, out assemblyName, out typeName);
            keys.Add(assemblyName);
            keys.Add(typeName);
            keys.Add(methodId);
            keys.AddRange(GetKeys(input.Target, type));
            var parameters = keys.ToArray();
            string stringValue;
            if (TC.TryGet(parameters, out stringValue))
            {
                var returnType = ((MethodInfo)input.MethodBase).ReturnType;
                var returnValue = returnType.IsSimple() ? stringValue.As(returnType) : DeserializeJson(stringValue, returnType);
                return input.CreateMethodReturn(returnValue);
            }
            var value = TDI.AOPExecute(input, getNext).ReturnValue;
            TC.TryAdd(parameters, value, LifeTime);
            return input.CreateMethodReturn(value);
        }

        private object[] GetKeys(object source, Type type)
        {
            if (string.IsNullOrWhiteSpace(Properties)) return new object[0];
            var properties = Properties.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var keys = new List<object>();
            foreach (var name in properties)
            {
                var p = TM.GetProperty(type, name);
                if (p == null) continue;
                keys.Add(p.GetValue(source, null));
            }
            return keys.ToArray();
        }

        private static object DeserializeJson(string json, Type resultType)
        {
            if (resultType.IsSimple())
            {
                return json.As(resultType);
            }
            using (var sr = new StringReader(json))
            {
                return JsonSerializer.Deserialize(sr, resultType);
            }
        }

        private static readonly JsonSerializer JsonSerializer = new JsonSerializer
        {
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
    }
}