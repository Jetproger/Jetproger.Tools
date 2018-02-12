using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using System.Text;
using System.Web;
using Jetproger.Tools.Plugin.DI;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Tools
{
    public static class DI
    {
        private static readonly UnityConfigProxy[] UnityConfigHolder = { null };
        private static readonly string[] CurrentAssemblyNameHolder = { null };
        private static readonly Assembly[] CurrentAssemblyHolder = { null };
        private static readonly IUnityContainer[] ContainerHolder = { null };

        public static Type TypeOf<T>()
        {
            return Container.Resolve<T>().GetType();
        }

        public static Type TypeOf(Type type)
        {
            return Container.Resolve(type).GetType();
        }

        public static T Resolve<T>(Action<T> initialize)
        {
            var instance = Resolve<T>();
            initialize(instance);
            return instance;
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static object Resolve(Type type, Action<object> initialize)
        {
            var instance = Resolve(type);
            initialize(instance);
            return instance;
        }

        public static object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        public static IMethodReturn AOPExecute(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var next = getNext();
            if (next == null) return input.CreateMethodReturn(null);
            var result = next.Invoke(input, getNext);
            if (result == null) return input.CreateMethodReturn(null);
            if (result.Exception == null) return result;
            throw result.Exception;
        }

        public static string AOPDeclaration(IMethodInvocation input)
        {
            var method = (MethodInfo)input.MethodBase;
            var parameters = method.GetParameters();
            var sb = new StringBuilder();
            sb.AppendFormat("{0} {1}.{2}(", method.ReturnType, method.ReflectedType.FullName, method.Name);
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.AppendFormat("{0}{1} {2}", (i > 0 ? ", " : ""), parameters[i].ParameterType, parameters[i].Name);
            }
            sb.Append(")");
            return sb.ToString();
        }

        public static string AOPProfile(IMethodInvocation input)
        {
            var method = (MethodInfo)input.MethodBase;
            var parameters = method.GetParameters();
            var arguments = input.Arguments;
            var sb = new StringBuilder();
            sb.AppendFormat("{0} {1}.{2}(", method.ReturnType, method.ReflectedType.FullName, method.Name);
            for (int i = 0; i < parameters.Length; i++)
            {
                var value = i < arguments.Count ? (arguments[i] != null ? arguments[i].ToString() : "<null>") : "<>";
                sb.AppendFormat("{0}{1} {2}: \"{3}\"", (i > 0 ? ", " : ""), parameters[i].ParameterType, parameters[i].Name, value);
            }
            sb.Append(")");
            return sb.ToString();
        }

        private static IUnityContainer Container
        {
            get
            {
                if (ContainerHolder[0] == null)
                {
                    lock (ContainerHolder)
                    {
                        if (ContainerHolder[0] == null)
                        {
                            var section = new InmemoryUnityConfigurationSection();
                            section.DeserializeSection(UnityConfig.GetUnityConfigXml());
                            var container = new UnityContainer();
                            section.Configure(container);
                            ContainerHolder[0] = container;
                        }
                    }
                }
                return ContainerHolder[0];
            }
        }

        private static UnityConfigProxy UnityConfig
        {
            get
            {
                if (UnityConfigHolder[0] == null)
                {
                    lock (UnityConfigHolder)
                    {
                        if (UnityConfigHolder[0] == null)
                        {
                            UnityConfigHolder[0] = GetProxy();
                        }
                    }
                }
                return UnityConfigHolder[0];
            }
        }

        private static UnityConfigProxy GetProxy()
        {
            CreateChannel();
            var type = typeof(UnityConfigProxy);
            var portName = $"{(type.FullName ?? string.Empty).Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
            var objectUri = type.Name.ToLower();
            var uri = $"ipc://{portName}/{objectUri}";
            var proxy = TryGetExistsProxy(type, uri);
            if (proxy != null) return proxy;
            CreateUnityConfig();
            return (UnityConfigProxy)Activator.GetObject(type, uri);
        }

        private static void CreateChannel()
        {
            var type = typeof(Type);
            foreach (var entry in RemotingConfiguration.GetRegisteredWellKnownServiceTypes())
            {
                if (entry.ObjectType == type) return;
            }
            var portName = $"{AppDomain.CurrentDomain.FriendlyName.Replace(".", "-").ToLower()}-{Process.GetCurrentProcess().Id}";
            var objectUri = type.Name.ToLower();
            var client = new BinaryClientFormatterSinkProvider();
            var server = new BinaryServerFormatterSinkProvider
            {
                TypeFilterLevel = TypeFilterLevel.Full
            };
            var config = new Hashtable {
                ["name"] = string.Empty,
                ["portName"] = portName,
                ["tokenImpersonationLevel"] = TokenImpersonationLevel.Impersonation,
                ["impersonate"] = true,
                ["useDefaultCredentials"] = true,
                ["secure"] = true,
                ["typeFilterLevel"] = TypeFilterLevel.Full
            };
            var ipcChannel = new IpcChannel(config, client, server);
            ChannelServices.RegisterChannel(ipcChannel, true);
            RemotingConfiguration.RegisterWellKnownServiceType(type, objectUri, WellKnownObjectMode.SingleCall);
        }

        private static UnityConfigProxy TryGetExistsProxy(Type type, string uri)
        {
            try
            {
                var proxy = (UnityConfigProxy)Activator.GetObject(type, uri);
                proxy.CreateChannel();
                return proxy;
            }
            catch
            {
                return null;
            }
        }

        private static void CreateUnityConfig()
        {
            var setup = new AppDomainSetup {
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                LoaderOptimization = LoaderOptimization.SingleDomain,
                ShadowCopyFiles = "true"
            };
            var name = $"f__{(typeof(UnityConfigXml)).AssemblyQualifiedName}";
            var domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
            var type = typeof(UnityConfigProxy);
            var assemblyName = type.Assembly.GetName().Name;
            var typeName = type.FullName ?? string.Empty;
            var instance = domain.CreateInstanceAndUnwrap(assemblyName, typeName) as UnityConfigProxy;
            instance?.CreateChannel();
        }

        public static string GetUnityConfigXml()
        {
            var configuration = new UnityConfiguration();
            foreach (var assembly in GetAssemblies())
            {
                configuration.unity.AddAssembly(assembly.GetName().Name);
            }
            foreach (var type in GetTypes())
            {
                configuration.unity.AddNamespace(type.Namespace);
                var register = new Register { type = type.Name };
                var typeLifetime = GetLifetime(type);
                if (!string.IsNullOrWhiteSpace(typeLifetime)) register.lifetime = new UnityLifeTimeItem { type = typeLifetime };
                var mapOf = GetMapOf(type);
                if (!string.IsNullOrWhiteSpace(mapOf) && configuration.unity.container.Registers.All(x => x.type != mapOf))
                {
                    register.type = mapOf;
                    register.mapTo = type.Name;
                }
                configuration.unity.container.Registers.Add(register);
            }
            return configuration.ToXml();
        }

        private static IEnumerable<Type> GetTypes()
        {
            var dependencyInjectionItemType = typeof(IDependencyInjectionItem);
            foreach (var assembly in GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsInterface || type.IsAbstract) continue;
                    if (dependencyInjectionItemType.IsAssignableFrom(type)) yield return type;
                }
            }
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return CurrentAssembly;
            foreach (var fileName in Directory.GetFiles(AppDir(), "*.dll"))
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
                        if (CurrentAssemblyHolder[0] == null) CurrentAssemblyHolder[0] = typeof(DI).Assembly;
                    }
                }
                return CurrentAssemblyHolder[0];
            }
        }

        private static string AppDir()
        {
            return (HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("~"));
        }
    }
}