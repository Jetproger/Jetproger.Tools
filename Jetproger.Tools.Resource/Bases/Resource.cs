using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Web;
using Jetproger.Tools.Resource.Bases;

namespace Tools
{
    public static partial class Resource
    {
        private static readonly Assembly[] CurrentAssemblyHolder = { null };

        public static ResourceItem<string> GetResourceName(string resourceKey, string defaultValue = null, string assemblyName = null)
        {
            return GetResource(assemblyName, "ResourceNames", resourceKey, defaultValue ?? $"@{resourceKey}");
        }

        public static ResourceItem<string> GetResourceDescription(string resourceKey, string defaultValue = null, string assemblyName = null)
        {
            return GetResource(assemblyName, "ResourceDescriptions", resourceKey, defaultValue ?? $"@{resourceKey}");
        }

        public static ResourceItem<string> GetResourceShortcut(string resourceKey, string defaultValue = null, string assemblyName = null)
        {
            return GetResource(assemblyName, "ResourceShortcuts", resourceKey, defaultValue ?? $"@{resourceKey}");
        }

        public static ResourceItem<string> GetResourceSpecify(string resourceKey, string defaultValue = null, string assemblyName = null)
        {
            return GetResource(assemblyName, "ResourceSpecifies", resourceKey, defaultValue ?? $"@{resourceKey}");
        }

        public static ResourceItem<Image> GetResourceImage(string resourceKey, string assemblyName = null)
        {
            var item = GetResource(assemblyName, "ResourcePictures", resourceKey, null);
            var image = !string.IsNullOrWhiteSpace(item.Text) ? Methods.AsImage(item.Text) : Methods.GetDefaultImage();
            return new ResourceItem<Image>(item.Key, item.Text, item.IsDeclared, true, image);
        }

        public static ResourceItem<Icon> GetResourceIcon(string resourceKey, string assemblyName = null)
        {
            var item = GetResource(assemblyName, "ResourcePictures", resourceKey, null);
            var icon = !string.IsNullOrWhiteSpace(item.Text) ? Methods.AsIcon(item.Text) : Methods.GetDefaultIcon();
            return new ResourceItem<Icon>(item.Key, item.Text, item.IsDeclared, true, icon);
        }

        private static ResourceItem<string> GetResource(string assemblyName, string resourceName, string resourceKey, string defaultValue)
        {
            assemblyName = GetAssembly(assemblyName).GetName().Name;
            var isDeclared = false;
            var value = Cache.Get((assemblyName ?? string.Empty).ToLower(), (resourceName ?? string.Empty).ToLower(), (resourceKey ?? string.Empty).ToLower(), () =>
            {
                var baseName = $"{assemblyName}.Resx.{resourceName}";
                var rm = GetResourceManager(baseName, assemblyName);
                string resource;
                isDeclared = TryGetResource(rm, resourceKey, defaultValue, out resource);
                return resource;
            });
            return new ResourceItem<string>(resourceKey, value, isDeclared, true, value);
        }

        private static bool TryGetResource(ResourceManager resourceManager, string resourceKey, string defaultValue, out string value)
        {
            try
            {
                value = resourceManager.GetString(resourceKey) ?? defaultValue;
                return true;
            }
            catch
            {
                value = defaultValue;
                return false;
            }
        }

        private static ResourceManager GetResourceManager(string baseName, string assemblyName)
        {
            return Cache.Get("resourcemanagers", (assemblyName ?? string.Empty).ToLower(), (baseName ?? string.Empty).ToLower(), () => TryGetResourceManager(baseName, assemblyName));
        }

        private static ResourceManager TryGetResourceManager(string baseName, string assemblyName)
        {
            try
            {
                return new ResourceManager(baseName ?? string.Empty, GetAssembly(assemblyName));
            }
            catch
            {
                return null;
            }
        }

        private static Assembly GetAssembly(string assemblyName)
        {
            return Cache.Get("assemblies", (assemblyName ?? "").ToLower(), () => (!string.IsNullOrWhiteSpace(assemblyName) ? LoadAssembly(assemblyName) : null) ?? CurrentAssembly);
        }

        private static Assembly LoadAssembly(string assemblyName)
        {
            var name = assemblyName.ToLower();
            if (!name.EndsWith(".dll"))
            {
                var assembly = TryLoadAssemblyWithoutExtension(assemblyName);
                if (assembly != null) return assembly;
                name = name + ".dll";
            }
            if (name == Path.GetFileName(name))
            {
                var httpContext = HttpContext.Current;
                if (httpContext != null) name = Path.Combine("bin", name);
                name = Path.Combine(AppDir(), name);
            }
            return TryLoadAssemblyWithExtension(name);
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

        private static Assembly TryLoadAssemblyWithExtension(string assemblyName)
        {
            try
            {
                return File.Exists(assemblyName) ? Assembly.LoadFrom(assemblyName) : null;
            }
            catch
            {
                return null;
            }
        }

        private static string AppDir()
        {
            return HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("~");
        }

        private static Assembly CurrentAssembly
        {
            get
            {
                if (CurrentAssemblyHolder[0] == null)
                {
                    lock (CurrentAssemblyHolder)
                    {
                        if (CurrentAssemblyHolder[0] == null)
                        {
                            CurrentAssemblyHolder[0] = typeof (Toolx).Assembly;
                        }
                    }
                }
                return CurrentAssemblyHolder[0];
            }
        }
    }
}