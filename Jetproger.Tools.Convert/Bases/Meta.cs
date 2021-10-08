using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Web;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Bases
{
    public static class MetaExtensions
    {
        public static ResourceManager GetResourceManager(this Je.ISysExpander e, string resourceName, string assemblyName)
        {
            try
            {
                var assembly = GetAssembly(e, assemblyName);
                var baseName = $"{assemblyName}.Bases.{resourceName}";
                return new ResourceManager(baseName, assembly);
            }
            catch
            {
                return null;
            }
        }

        public static string GetResourceValue(this Je.ISysExpander e, ResourceManager resourceManager, string resourceKey)
        {
            try
            {
                return resourceManager.GetString(resourceKey) ?? string.Empty;
            }
            catch
            {
                return null;
            }
        }

        public static T CreateInstance<T>(this Je.ISysExpander e, string metaName, object[] args = null)
        {
            return (T)CreateInstance(e, metaName, args);
        }

        public static object CreateInstance(this Je.ISysExpander e, string metaName, object[] args = null)
        {
            string assemblyName, typeName, value;
            ResolveTypeName(e, metaName, out assemblyName, out typeName, out value);
            if (string.IsNullOrWhiteSpace(value)) return CreateInstance(e, assemblyName, typeName, args);
            var type = ResolveType(e, assemblyName, typeName);
            return IsSimple(e, type) ? value.As(type) : CreateInstance(e, assemblyName, typeName, args);
        }

        public static T CreateInstance<T>(this Je.ISysExpander e, string assemblyName, string typeName, object[] args = null)
        {
            return (T)CreateInstance(e, assemblyName, typeName, args);
        }

        public static object CreateInstance(this Je.ISysExpander e, string assemblyName, string typeName, object[] args = null)
        {
            return CreateInstance(e, ResolveType(e, assemblyName, typeName), args);
        }

        public static T CreateInstance<T>(this Je.ISysExpander e, object[] args = null)
        {
            return (T)CreateInstance(e, typeof(T), args);
        }

        public static object CreateInstance(this Je.ISysExpander e, Type type, object[] args = null)
        {
            if (type == null) return null;
            if (type == typeof(string)) return Activator.CreateInstance(type, new char[0]);
            if (args == null || args.Length == 0) return Activator.CreateInstance(type, true);
            return Activator.CreateInstance(type, args);
        }

        public static Type ResolveType(this Je.ISysExpander e, string name)
        {
            string assemblyName, typeName, value;
            ResolveTypeName(e, name, out assemblyName, out typeName, out value);
            return ResolveType(e, assemblyName, typeName);
        }

        public static Type ResolveType(this Je.ISysExpander e, string assemblyName, string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName)) return null;
            assemblyName = assemblyName ?? string.Empty;
            typeName = typeName ?? string.Empty;
            Assembly assembly;
            Type type;
            if (!string.IsNullOrWhiteSpace(assemblyName))
            {
                assembly = GetAssembly(e, assemblyName);
                if (assembly != null)
                {
                    type = assembly.GetType(typeName, false, true);
                    if (type != null) return type;
                }
            }
            type = Type.GetType(typeName, false, true);
            if (type != null) return type;
            var pos = typeName.Length;
            while (true)
            {
                try
                {
                    pos = typeName.LastIndexOf(".", pos - 1, StringComparison.Ordinal);
                    if (pos < 1) return null;
                    assemblyName = typeName.Substring(0, pos);
                    assembly = GetAssembly(e, assemblyName);
                    if (assembly == null) continue;
                    type = assembly.GetType(typeName);
                    if (type != null) return type;
                    var libraryName = assemblyName + (!assemblyName.EndsWith(".dll") ? ".dll" : string.Empty) + ", ";
                    type = assembly.GetType(typeName.Replace(libraryName, string.Empty));
                    if (type != null) return type;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static Assembly GetAssembly(this Je.ISysExpander e, string assemblyName)
        {
            return assemblyName != string.Empty ? LoadAssembly(assemblyName) : null;
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
                name = Path.Combine(FileStore.AppDir(), name);
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

        public static void ResolveTypeName(this Je.ISysExpander e, object value, out string assemblyName, out string typeName, out string valueName)
        {
            assemblyName = null;
            typeName = null;
            valueName = null;
            if (value == null) return;
            var type = value.GetType();
            valueName = value.As<string>();
            ResolveTypeName(e, type, out assemblyName, out typeName);
        }

        public static void ResolveTypeName<T>(this Je.ISysExpander e, out string assemblyName, out string typeName)
        {
            ResolveTypeName(e, typeof(T), out assemblyName, out typeName);
        }

        public static void ResolveTypeName(this Je.ISysExpander e, Type type, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = null;
            if (type == null) return;
            if (type.ToString().ToLower().Contains("dynamicmodule")) type = type.BaseType;
            if (type == null) return;
            assemblyName = Path.GetFileNameWithoutExtension(type.Assembly.Location);
            typeName = type.ToString();
        }

        public static void ResolveTypeName(this Je.ISysExpander e, string metaName, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = null;
            if (string.IsNullOrWhiteSpace(metaName)) return;
            var names = metaName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            assemblyName = names.Length > 0 ? names[0] : null;
            typeName = names.Length > 1 ? names[1] : null;
        }

        public static void ResolveTypeName(this Je.ISysExpander e, string metaName, out string assemblyName, out string typeName, out string value)
        {
            var names = (metaName ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            assemblyName = names.Length > 0 ? names[0] : string.Empty;
            typeName = names.Length > 1 ? names[1] : string.Empty;
            value = names.Length > 2 ? names[2] : string.Empty;
        }

        public static string MakeTypeName(this Je.ISysExpander e, object value)
        {
            if (value == null) return string.Empty;
            var type = value.GetType();
            var stringValue = IsSimple(e, type) ? $", {value.As<string>()}" : string.Empty;
            return $"{MakeTypeName(e, type)}{stringValue}";
        }

        public static string MakeTypeName<T>(this Je.ISysExpander e)
        {
            return MakeTypeName(e, typeof(T));
        }

        public static string MakeTypeName(this Je.ISysExpander e, Type type)
        {
            return type != null ? $"{Path.GetFileNameWithoutExtension(type.Assembly.Location)}, {type}" : string.Empty;
        }

        public static string MakeTypeName(this Je.ISysExpander e, string assemblyName, string typeName)
        {
            return $"{assemblyName}, {typeName}";
        }

        public static string GetMemberName(this Je.ISysExpander e, LambdaExpression memberSelector)
        {
            if (memberSelector == null)
            {
                return null;
            }
            var currentExpression = memberSelector.Body;
            while (true)
            {
                switch (currentExpression.NodeType)
                {
                    case ExpressionType.ArrayLength: return "Length";
                    case ExpressionType.Parameter: return ((ParameterExpression)currentExpression).Name;
                    case ExpressionType.Call: return ((MethodCallExpression)currentExpression).Method.Name;
                    case ExpressionType.MemberAccess: return ((MemberExpression)currentExpression).Member.Name;
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked: currentExpression = ((UnaryExpression)currentExpression).Operand; break;
                    case ExpressionType.Invoke: currentExpression = ((InvocationExpression)currentExpression).Expression; break;
                    default: return null;
                }
            }
        }

        public static bool IsList(this Je.ISysExpander e, Type type)
        {
            if (type == null) return false;
            var genericType = GenericOf(e, type);
            return genericType != null && type.GetGenericTypeDefinition() == typeof(List<>) && genericType.IsClass && !typeof(IEnumerable).IsAssignableFrom(genericType);
        }

        public static T DefaultOf<T>(this Je.ISysExpander e)
        {
            return (T)DefaultOf(e, typeof(T));
        }

        public static object DefaultOf(this Je.ISysExpander e, Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool IsTypeOf(this Je.ISysExpander e, Type type, Type sample)
        {
            return type == sample || type.IsSubclassOf(sample) || type.GetInterfaces().Any(x => x == sample);
        }

        public static Type GenericOf(this Je.ISysExpander e, Type type)
        {
            var types = type.GetGenericArguments();
            if (types.Length > 0) return types[0];
            return type.HasElementType ? type.GetElementType() : null;
        }

        public static bool IsSimple(this Je.ISysExpander e, Type type)
        {
            return type != null && (type.IsEnum || SimpleTypes.Contains(type));
        }

        private static readonly HashSet<Type> SimpleTypes = new HashSet<Type>
        {
            typeof(string), typeof(char), typeof(char?),
            typeof(bool), typeof(bool?),
            typeof(byte), typeof(byte?), typeof(sbyte), typeof(sbyte?),
            typeof(short), typeof(short?), typeof(ushort), typeof(ushort?),
            typeof(int), typeof(int?), typeof(uint), typeof(uint?),
            typeof(long), typeof(long?), typeof(ulong), typeof(ulong?),
            typeof(Guid), typeof(Guid?),
            typeof(float), typeof(float?),
            typeof(decimal), typeof(decimal?),
            typeof(double), typeof(double?),
            typeof(DateTime), typeof(DateTime?),
            typeof(byte[]), typeof(char[])
        };
    }
}