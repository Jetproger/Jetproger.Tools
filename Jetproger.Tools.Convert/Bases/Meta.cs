using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Bases
{
    public static class MetaExtensions
    {
        private static readonly HashSet<Type> SimpleTypes = new HashSet<Type> { typeof(string), typeof(char), typeof(char?), typeof(bool), typeof(bool?), typeof(byte), typeof(byte?), typeof(sbyte), typeof(sbyte?), typeof(short), typeof(short?), typeof(ushort), typeof(ushort?), typeof(int), typeof(int?), typeof(uint), typeof(uint?), typeof(long), typeof(long?), typeof(ulong), typeof(ulong?), typeof(Guid), typeof(Guid?), typeof(float), typeof(float?), typeof(decimal), typeof(decimal?), typeof(double), typeof(double?), typeof(DateTime), typeof(DateTime?) };

        public static bool IsSimple(this Jc.IMetaExpander exp, Type type)
        {
            return type != null && (type.IsEnum || SimpleTypes.Contains(type));
        }

        public static bool IsList(this Jc.IMetaExpander exp, Type type)
        {
            if (type == null) return false;
            var typeGe = GenericOf(exp, type);
            return typeGe != null && type.GetGenericTypeDefinition() == typeof(List<>) && typeGe.IsClass && !typeof(IEnumerable).IsAssignableFrom(typeGe);
        }

        public static object DefaultOf(this Jc.IMetaExpander exp, Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool IsTypeOf(this Jc.IMetaExpander exp, Type type, Type sample)
        {
            return type == sample || type.IsSubclassOf(sample) || type.GetInterfaces().Any(interfaceType => interfaceType == sample);
        }

        public static Type GenericOf(this Jc.IMetaExpander exp, Type type)
        {
            var types = type.GetGenericArguments();
            if (types.Length > 0) return types[0];
            return type.HasElementType ? type.GetElementType() : null;
        }

        public static Type ResolveType(this Jc.IMetaExpander exp, string name)
        {
            string assemblyName, typeName, value;
            ResolveTypeName(exp, name, out assemblyName, out typeName, out value);
            return ResolveType(exp, assemblyName, typeName);
        }

        public static Type ResolveType(this Jc.IMetaExpander exp, string assemblyName, string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName)) return null;
            assemblyName = assemblyName ?? string.Empty;
            typeName = typeName ?? string.Empty;
            Assembly assembly;
            Type type;
            if (!string.IsNullOrWhiteSpace(assemblyName))
            {
                assembly = GetAssembly(exp, assemblyName);
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
                    assembly = GetAssembly(exp, assemblyName);
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

        public static T CreateInstance<T>(this Jc.IMetaExpander exp, string metaName, object[] args = null)
        {
            return (T)CreateInstance(exp, metaName, args);
        }

        public static object CreateInstance(this Jc.IMetaExpander exp, string metaName, object[] args = null)
        {
            string assemblyName, typeName, value;
            ResolveTypeName(exp, metaName, out assemblyName, out typeName, out value);
            if (string.IsNullOrWhiteSpace(value)) return CreateInstance(exp, assemblyName, typeName, args);
            var type = ResolveType(exp, assemblyName, typeName);
            return ValueExtensions.IsPrimitive(type) ? value.As(type) : CreateInstance(exp, assemblyName, typeName, args);
        }

        public static T CreateInstance<T>(this Jc.IMetaExpander exp, string assemblyName, string typeName, object[] args = null)
        {
            return (T)CreateInstance(exp, assemblyName, typeName, args);
        }

        public static object CreateInstance(this Jc.IMetaExpander exp, string assemblyName, string typeName, object[] args = null)
        {
            return CreateInstance(exp, ResolveType(exp, assemblyName, typeName), args);
        }

        public static T CreateInstance<T>(this Jc.IMetaExpander exp, object[] args = null)
        {
            return (T)CreateInstance(exp, typeof(T), args);
        }

        public static object CreateInstance(this Jc.IMetaExpander exp, Type type, object[] args = null)
        {
            if (type == null) return null;
            if (type == typeof(string)) return Activator.CreateInstance(type, new char[0]);
            if (args == null || args.Length == 0) return Activator.CreateInstance(type, true);
            return Activator.CreateInstance(type, args);
        }

        public static Assembly GetAssembly(this Jc.IMetaExpander exp, string assemblyName)
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
                name = Path.Combine(Je.File.AppDir(), name);
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

        public static void ResolveTypeName(this Jc.IMetaExpander exp, object value, out string assemblyName, out string typeName, out string valueName)
        {
            assemblyName = null;
            typeName = null;
            valueName = null;
            if (value == null) return;
            var type = value.GetType();
            valueName = value.As<string>();
            ResolveTypeName(exp, type, out assemblyName, out typeName);
        }

        public static void ResolveTypeName<T>(this Jc.IMetaExpander exp, out string assemblyName, out string typeName)
        {
            ResolveTypeName(exp, typeof(T), out assemblyName, out typeName);
        }

        public static void ResolveTypeName(this Jc.IMetaExpander exp, Type type, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = null;
            if (type == null) return;
            if (type.ToString().ToLower().Contains("dynamicmodule")) type = type.BaseType;
            if (type == null) return;
            assemblyName = Path.GetFileNameWithoutExtension(type.Assembly.Location);
            typeName = type.ToString();
        }

        public static void ResolveTypeName(this Jc.IMetaExpander exp, string metaName, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = null;
            if (string.IsNullOrWhiteSpace(metaName)) return;
            var names = metaName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            assemblyName = names.Length > 0 ? names[0] : null;
            typeName = names.Length > 1 ? names[1] : null;
        }

        public static void ResolveTypeName(this Jc.IMetaExpander exp, string metaName, out string assemblyName, out string typeName, out string value)
        {
            var names = (metaName ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            assemblyName = names.Length > 0 ? names[0] : string.Empty;
            typeName = names.Length > 1 ? names[1] : string.Empty;
            value = names.Length > 2 ? names[2] : string.Empty;
        }

        public static string MakeTypeName(this Jc.IMetaExpander exp, object value)
        {
            if (value == null) return string.Empty;
            var type = value.GetType();
            var stringValue = ValueExtensions.IsPrimitive(type) ? $", {value.As<string>()}" : string.Empty;
            return $"{MakeTypeName(exp, type)}{stringValue}";
        }

        public static string MakeTypeName<T>(this Jc.IMetaExpander exp)
        {
            return MakeTypeName(exp, typeof(T));
        }

        public static string MakeTypeName(this Jc.IMetaExpander exp, Type type)
        {
            return type != null ? $"{Path.GetFileNameWithoutExtension(type.Assembly.Location)}, {type}" : string.Empty;
        }

        public static string MakeTypeName(this Jc.IMetaExpander exp, string assemblyName, string typeName)
        {
            return $"{assemblyName}, {typeName}";
        }

        public static string GetMemberName(this Jc.IMetaExpander exp, LambdaExpression memberSelector)
        {
            if (memberSelector == null) return null;
            var currentExpression = memberSelector.Body;
            while (true) {
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
    }
}