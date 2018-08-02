using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Jetproger.Tools.Injection.Bases
{
    public static class UnityXml
    {
        private static readonly string[] CurrentAssemblyNameHolder = { null };
        private static readonly Assembly[] CurrentAssemblyHolder = { null };
        private static readonly string[] XmlHolder = { null };

        public static string Xml
        {
            get
            {
                if (XmlHolder[0] == null)
                {
                    lock (XmlHolder)
                    {
                        if (XmlHolder[0] == null)
                        {
                            WriteXmlToFile();
                            XmlHolder[0] = ReadXmlFromFile();
                        }
                    }
                }
                return XmlHolder[0];
            }
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
                        var aspectAttribute = attribute as AspectAttribute;
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
            var dependencyInjectionItemType = typeof(IInjection);
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
            if (assembly != null)
            {
                return assembly;
            }
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
                var attribute = item as MapOfDependencyInjectionAttribute;
                if (attribute != null) return !string.IsNullOrWhiteSpace(attribute.MapOf) ? attribute.MapOf : null;
            }
            return null;
        }

        private static string GetLifetime(Type type)
        {
            foreach (var item in type.GetCustomAttributes(true))
            {
                var attribute = item as LifetimeDependencyInjectionAttribute;
                if (attribute != null) return !string.IsNullOrWhiteSpace(attribute.Lifetime) ? attribute.Lifetime : null;
            }
            return null;
        }

        private static string CurrentAssemblyName
        {
            get
            {
                if (CurrentAssemblyNameHolder[0] == null)
                {
                    lock (CurrentAssemblyNameHolder)
                    {
                        if (CurrentAssemblyNameHolder[0] == null) CurrentAssemblyNameHolder[0] = CurrentAssembly.GetName().Name;
                    }
                }
                return CurrentAssemblyNameHolder[0];
            }
        }

        private static Assembly CurrentAssembly
        {
            get
            {
                if (CurrentAssemblyHolder[0] == null)
                {
                    lock (CurrentAssemblyHolder)
                    {
                        if (CurrentAssemblyHolder[0] == null) CurrentAssemblyHolder[0] = typeof(InjectionExtensions).Assembly;
                    }
                }
                return CurrentAssemblyHolder[0];
            }
        }
    }
}