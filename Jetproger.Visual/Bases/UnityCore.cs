using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Factories;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Jetproger.Tools.Inject.Bases
{
    public static class UnityCore
    {
        private static string CurrentAssemblyName { get { return f.one.Get(CurrentAssemblyNameHolder, () => CurrentAssembly.GetName().Name); } }
        private static Assembly CurrentAssembly { get { return f.one.Get(CurrentAssemblyHolder, () => typeof(UnityCore).Assembly); } }
        public static string Config => f.one.Get(ConfigHolder, RegisterConfig);
        private static readonly string[] CurrentAssemblyNameHolder = { null };
        private static readonly Assembly[] CurrentAssemblyHolder = { null };
        private static readonly string[] ConfigHolder = { null };
        private static readonly string[] XmlHolder = { null };

        private static string RegisterConfig()
        {
            WriteXmlToFile();
            return ReadXmlFromFile();
        }

        private static string ReadXmlFromFile()
        {
            var fileName = UnityConfiguration.GetUnityConfigurationFileName();
            var s = File.ReadAllText(fileName);
            var doc = new XmlDocument();
            doc.LoadXml(s);
            var root = doc.GetElementsByTagName("unity")[0];
            return root.OuterXml;
        }

        private static void WriteXmlToFile()
        {
            var callHandlerType = typeof(ICallHandler);
            var configuration = new UnityConfiguration();
            configuration.OfXml();
            foreach (var assembly in GetAssemblies())
            {
                configuration.unity.AddAssembly(assembly.GetName().Name);
            }

            foreach (var type in GetTypes())
            {
                configuration.unity.AddNamespace(type.Namespace);
                if (callHandlerType.IsAssignableFrom(type)) continue;

                foreach (MethodInfo m in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!m.IsVirtual) continue;
                    if (!m.IsPublic && !m.IsFamily) continue;
                    foreach (var attribute in m.GetCustomAttributes(true))
                    {
                        var aspectAttribute = attribute as UnityAspectAttribute;
                        if (aspectAttribute == null) continue;
                        configuration.unity.container.interception.AddPolicies(new Policy(type.FullName, m.Name, aspectAttribute.GetCallHandlerTypeName(), aspectAttribute.GetUnityProperties()));
                    }
                }

                var register = new Register { type = type.Name };
                var typeLifetime = GetLifetime(type);
                if (!string.IsNullOrWhiteSpace(typeLifetime)) register.lifetime = new UnityLifeTimeItem { type = typeLifetime };
                var mapOf = GetMapOf(type);
                if (!string.IsNullOrWhiteSpace(mapOf))
                {
                    register.type = mapOf;
                    register.mapTo = type.Name;
                }
                if (configuration.unity.container.Registers.All(x => x.type != register.type)) configuration.unity.container.Registers.Add(register);
            }
            configuration.ToXml();
        }

        private static IEnumerable<Type> GetTypes()
        {
            var dependencyInjectionItemType = typeof(IUnity);
            var callHandlerType = typeof(ICallHandler);
            foreach (var assembly in GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsInterface || type.IsAbstract) continue;
                    if (dependencyInjectionItemType.IsAssignableFrom(type) || callHandlerType.IsAssignableFrom(type)) yield return type;
                }
            }
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return CurrentAssembly;
            var appDir = HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("~");
            foreach (var fileName in Directory.GetFiles(appDir, "*.dll"))
            {
                var assembly = TryLoadAssembly(fileName);
                if (assembly == null) continue;
                if (!HasReferenceOnCurrentAssembly(assembly)) continue;
                yield return assembly;
            }
        }

        private static Assembly TryLoadAssembly(string fileName)
        {
            var assembly = TryLoadAssemblyWithExtension(fileName);
            if (assembly != null) return assembly;
            var ext = ".dll";
            if (fileName.EndsWith(ext)) fileName = fileName.Substring(0, fileName.Length - ext.Length);
            return TryLoadAssemblyWithoutExtension(fileName);
        }

        private static Assembly TryLoadAssemblyWithExtension(string assemblyName)
        {
            try
            {
                return System.IO.File.Exists(assemblyName) ? Assembly.LoadFrom(assemblyName) : null;
            }
            catch
            {
                return null;
            }
        }

        private static Assembly TryLoadAssemblyWithoutExtension(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch
            {
                return null;
            }
        }

        private static bool HasReferenceOnCurrentAssembly(Assembly assembly)
        {
            return assembly.GetReferencedAssemblies().Any(x => x.Name == CurrentAssemblyName);
        }

        private static string GetMapOf(Type type)
        {
            foreach (var item in type.GetCustomAttributes(true))
            {
                var attribute = item as UnityMapAttribute;
                if (attribute != null) return !string.IsNullOrWhiteSpace(attribute.MapOf) ? attribute.MapOf : null;
            }
            return null;
        }

        private static string GetLifetime(Type type)
        {
            foreach (var item in type.GetCustomAttributes(true))
            {
                var attribute = item as UnityLifetimeAttribute;
                if (attribute != null) return !string.IsNullOrWhiteSpace(attribute.Lifetime) ? attribute.Lifetime : null;
            }
            return null;
        }
    }
}