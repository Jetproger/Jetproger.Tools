using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Tools
{
    public static partial class Metadata
    {
        public static FieldInfo GetField(object value, string fieldName)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetField(assemblyName, typeName, fieldName);
        }

        public static FieldInfo GetField<T>(string fieldName)
        {
            return GetField(typeof(T), fieldName);
        }

        public static FieldInfo GetField(Type type, string fieldName)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetField(assemblyName, typeName, fieldName);
        }

        public static FieldInfo GetField(string name, string fieldName)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetField(assemblyName, typeName, fieldName);
        }

        public static FieldInfo GetField(string assemblyName, string typeName, string fieldName)
        {
            return Cache.Get("field", assemblyName ?? "", typeName ?? "", fieldName ?? "", () =>
            {
                foreach (FieldInfo fieldInfo in GetFields(assemblyName, typeName))
                {
                    if (fieldInfo.Name == fieldName) return fieldInfo;
                }
                return null;
            });
        }

        public static FieldInfo[] GetFields(object value)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetFields(assemblyName, typeName);
        }

        public static FieldInfo[] GetFields<T>()
        {
            return GetFields(typeof(T));
        }

        public static FieldInfo[] GetFields(Type type)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetFields(assemblyName, typeName);
        }

        public static FieldInfo[] GetFields(string name)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetFields(assemblyName, typeName);
        }

        public static FieldInfo[] GetFields(string assemblyName, string typeName)
        {
            return Cache.Get("fields", assemblyName ?? "", typeName ?? "", () =>
            {
                var type = GetType(assemblyName, typeName);
                return type == null ? new FieldInfo[0] : (type.IsEnum ? type.GetFields() : type.GetFields(BindingFlags.Instance | BindingFlags.Public));
            });
        }

        public static MethodInfo GetMethod(object value, string methodName)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetMethod(assemblyName, typeName, methodName);
        }

        public static MethodInfo GetMethod<T>(string methodName)
        {
            return GetMethod(typeof(T), methodName);
        }

        public static MethodInfo GetMethod(Type type, string methodName)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetMethod(assemblyName, typeName, methodName);
        }

        public static MethodInfo GetMethod(string name, string methodName)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetMethod(assemblyName, typeName, methodName);
        }

        public static MethodInfo GetMethod(string assemblyName, string typeName, string methodName)
        {
            return Cache.Get("method", assemblyName ?? "", typeName ?? "", methodName ?? "", () =>
            {
                foreach (MethodInfo methodInfo in GetMethods(assemblyName, typeName))
                {
                    if (methodInfo.Name == methodName) return methodInfo;
                }
                return null;
            });
        }

        public static MethodInfo[] GetMethods(object value)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetMethods(assemblyName, typeName);
        }

        public static MethodInfo[] GetMethods<T>()
        {
            return GetMethods(typeof(T));
        }

        public static MethodInfo[] GetMethods(Type type)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetMethods(assemblyName, typeName);
        }

        public static MethodInfo[] GetMethods(string name)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetMethods(assemblyName, typeName);
        }

        public static MethodInfo[] GetMethods(string assemblyName, string typeName)
        {
            return Cache.Get("methods", assemblyName ?? "", typeName ?? "", () =>
            {
                var type = GetType(assemblyName, typeName);
                return type?.GetMethods(BindingFlags.Instance | BindingFlags.Public) ?? new MethodInfo[0];
            });
        }

        public static PropertyInfo GetProperty(object value, string propertyName)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetProperty(assemblyName, typeName, propertyName);
        }

        public static PropertyInfo GetProperty<T>(string propertyName)
        {
            return GetProperty(typeof(T), propertyName);
        }

        public static PropertyInfo GetProperty(Type type, string propertyName)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetProperty(assemblyName, typeName, propertyName);
        }

        public static PropertyInfo GetProperty(string name, string propertyName)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetProperty(assemblyName, typeName, propertyName);
        }

        public static PropertyInfo GetProperty(string assemblyName, string typeName, string propertyName)
        {
            return Cache.Get("property", assemblyName ?? "", typeName ?? "", propertyName ?? "", () =>
            {
                foreach (PropertyInfo propertyInfo in GetProperties(assemblyName, typeName))
                {
                    if (propertyInfo.Name == propertyName) return propertyInfo;
                }
                return null;
            });
        }

        public static PropertyInfo[] GetProperties(object value)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetProperties<T>()
        {
            return GetProperties(typeof(T));
        }

        public static PropertyInfo[] GetProperties(Type type)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetProperties(string name)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetProperties(string assemblyName, string typeName)
        {
            return Cache.Get("properties", assemblyName ?? "", typeName ?? "", "properties", () =>
            {
                var type = GetType(assemblyName, typeName);
                return type?.GetProperties(BindingFlags.Instance | BindingFlags.Public) ?? new PropertyInfo[0];
            });
        }

        public static PropertyInfo[] GetSelfProperties(object value)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetSelfProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSelfProperties<T>()
        {
            return GetSelfProperties(typeof(T));
        }

        public static PropertyInfo[] GetSelfProperties(Type type)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetSelfProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSelfProperties(string name)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetSelfProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSelfProperties(string assemblyName, string typeName)
        {
            return Cache.Get("selfproperties", assemblyName ?? "", typeName ?? "", "properties", () =>
            {
                var type = GetType(assemblyName, typeName);
                return type?.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly) ?? new PropertyInfo[0];
            });
        }

        public static PropertyInfo[] GetSimpleProperties(object value)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetSimpleProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSimpleProperties<T>()
        {
            return GetSimpleProperties(typeof(T));
        }

        public static PropertyInfo[] GetSimpleProperties(Type type)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetSimpleProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSimpleProperties(string name)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetSimpleProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSimpleProperties(string assemblyName, string typeName)
        {
            return Cache.Get("simpleproperties", assemblyName ?? "", typeName ?? "", "properties", () =>
            {
                return GetProperties(assemblyName, typeName).Where(p => p.PropertyType.IsSimple()).ToArray();
            });
        }

        public static T GetTypeAttribute<T>(object value) where T : Attribute
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetTypeAttribute<T>(assemblyName, typeName);
        }

        public static T GetTypeAttribute<T>(Type type) where T : Attribute
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetTypeAttribute<T>(assemblyName, typeName);
        }

        public static T GetTypeAttribute<T>(string name) where T : Attribute
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetTypeAttribute<T>(assemblyName, typeName);
        }

        public static T GetTypeAttribute<T>(string assemblyName, string typeName) where T : Attribute
        {
            return Cache.Get("typeattribute", assemblyName ?? "", typeName ?? "", () =>
            {
                var type = GetType(assemblyName, typeName);
                var customAttributes = type?.GetCustomAttributes(typeof(T), true);
                return customAttributes?.Length > 0 ? customAttributes[0] as T : default(T);
            });
        }

        public static Attribute[] GetTypeAttributes(object value)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetTypeAttributes(assemblyName, typeName);
        }

        public static Attribute[] GetTypeAttributes(Type type)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetTypeAttributes(assemblyName, typeName);
        }

        public static Attribute[] GetTypeAttributes(string name)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetTypeAttributes(assemblyName, typeName);
        }

        public static Attribute[] GetTypeAttributes(string assemblyName, string typeName)
        {
            return Cache.Get("typeattributes", assemblyName ?? "", typeName ?? "", () =>
            {
                var type = GetType(assemblyName, typeName);
                if (type == null) return new Attribute[0];
                var list = new List<Attribute>();
                foreach (var item in type.GetCustomAttributes(true))
                {
                    var attribute = item as Attribute;
                    if (attribute != null) list.Add(attribute);
                }
                return list.ToArray();
            });
        }

        public static T GetFieldAttribute<T>(object value, string fieldName) where T : Attribute
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetFieldAttribute<T>(assemblyName, typeName, fieldName);
        }

        public static T GetFieldAttribute<T>(Type type, string fieldName) where T : Attribute
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetFieldAttribute<T>(assemblyName, typeName, fieldName);
        }

        public static T GetFieldAttribute<T>(string name, string fieldName) where T : Attribute
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetFieldAttribute<T>(assemblyName, typeName, fieldName);
        }

        public static T GetFieldAttribute<T>(string assemblyName, string typeName, string fieldName) where T : Attribute
        {
            return Cache.Get("fieldattribute", assemblyName ?? "", typeName ?? "", fieldName ?? "", () =>
            {
                var fieldInfo = GetField(assemblyName, typeName, fieldName);
                var customAttributes = fieldInfo.GetCustomAttributes(typeof(T), true);
                return customAttributes.Length > 0 ? customAttributes[0] as T : default(T);
            });
        }

        public static Attribute[] GetFieldAttributes(object value, string fieldName)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetFieldAttributes(assemblyName, typeName, fieldName);
        }

        public static Attribute[] GetFieldAttributes(Type type, string fieldName)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetFieldAttributes(assemblyName, typeName, fieldName);
        }

        public static Attribute[] GetFieldAttributes(string name, string fieldName)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetFieldAttributes(assemblyName, typeName, fieldName);
        }

        public static Attribute[] GetFieldAttributes(string assemblyName, string typeName, string fieldName)
        {
            return Cache.Get("fieldattributes", assemblyName ?? "", typeName ?? "", fieldName ?? "", () =>
            {
                var fieldInfo = GetField(assemblyName, typeName, fieldName);
                if (fieldInfo == null) return new Attribute[0];
                var list = new List<Attribute>();
                foreach (var item in fieldInfo.GetCustomAttributes(true))
                {
                    var attribute = item as Attribute;
                    if (attribute != null) list.Add(attribute);
                }
                return list.ToArray();
            });
        }

        public static T GetMethodAttribute<T>(object value, string methodName) where T : Attribute
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetMethodAttribute<T>(assemblyName, typeName, methodName);
        }

        public static T GetMethodAttribute<T>(Type type, string methodName) where T : Attribute
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetMethodAttribute<T>(assemblyName, typeName, methodName);
        }

        public static T GetMethodAttribute<T>(string name, string methodName) where T : Attribute
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetMethodAttribute<T>(assemblyName, typeName, methodName);
        }

        public static T GetMethodAttribute<T>(string assemblyName, string typeName, string methodName) where T : Attribute
        {
            return Cache.Get("methodattribute", assemblyName ?? "", typeName ?? "", methodName ?? "", () =>
            {
                var methodInfo = GetMethod(assemblyName, typeName, methodName);
                var customAttributes = methodInfo.GetCustomAttributes(typeof(T), true);
                return customAttributes.Length > 0 ? customAttributes[0] as T : default(T);
            });
        }

        public static Attribute[] GetMethodAttributes(object value, string methodName)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetMethodAttributes(assemblyName, typeName, methodName);
        }

        public static Attribute[] GetMethodAttributes(Type type, string methodName)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetMethodAttributes(assemblyName, typeName, methodName);
        }

        public static Attribute[] GetMethodAttributes(string name, string methodName)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetMethodAttributes(assemblyName, typeName, methodName);
        }

        public static Attribute[] GetMethodAttributes(string assemblyName, string typeName, string methodName)
        {
            return Cache.Get("methodattributes", assemblyName ?? "", typeName ?? "", methodName ?? "", () =>
            {
                var methodInfo = GetMethod(assemblyName, typeName, methodName);
                if (methodInfo == null) return new Attribute[0];
                var list = new List<Attribute>();
                foreach (var item in methodInfo.GetCustomAttributes(true))
                {
                    var attribute = item as Attribute;
                    if (attribute != null) list.Add(attribute);
                }
                return list.ToArray();
            });
        }

        public static T GetPropertyAttribute<T>(object value, string propertyName) where T : Attribute
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetPropertyAttribute<T>(assemblyName, typeName, propertyName);
        }

        public static T GetPropertyAttribute<T>(Type type, string propertyName) where T : Attribute
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetPropertyAttribute<T>(assemblyName, typeName, propertyName);
        }

        public static T GetPropertyAttribute<T>(string name, string propertyName) where T : Attribute
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetPropertyAttribute<T>(assemblyName, typeName, propertyName);
        }

        public static T GetPropertyAttribute<T>(string assemblyName, string typeName, string propertyName) where T : Attribute
        {
            return Cache.Get("propertyattribute", assemblyName ?? "", typeName ?? "", propertyName ?? "", () =>
            {
                var propertyInfo = GetProperty(assemblyName, typeName, propertyName);
                var customAttributes = propertyInfo?.GetCustomAttributes(typeof(T), true);
                return customAttributes?.Length > 0 ? customAttributes[0] as T : default(T);
            });
        }

        public static Attribute[] GetPropertyAttributes(object value, string propertyName)
        {
            string assemblyName, typeName, valueName;
            GetNames(value, out assemblyName, out typeName, out valueName);
            return GetPropertyAttributes(assemblyName, typeName, propertyName);
        }

        public static Attribute[] GetPropertyAttributes(Type type, string propertyName)
        {
            string assemblyName, typeName;
            GetNames(type, out assemblyName, out typeName);
            return GetPropertyAttributes(assemblyName, typeName, propertyName);
        }

        public static Attribute[] GetPropertyAttributes(string name, string propertyName)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetPropertyAttributes(assemblyName, typeName, propertyName);
        }

        public static Attribute[] GetPropertyAttributes(string assemblyName, string typeName, string propertyName)
        {
            return Cache.Get("propertyattributes", assemblyName ?? "", typeName ?? "", propertyName ?? "", () =>
            {
                var propertyInfo = GetProperty(assemblyName, typeName, propertyName);
                if (propertyInfo == null) return new Attribute[0];
                var list = new List<Attribute>();
                foreach (var item in propertyInfo.GetCustomAttributes(true))
                {
                    var attribute = item as Attribute;
                    if (attribute != null) list.Add(attribute);
                }
                return list.ToArray();
            });
        }

        public static string GetMemberName(LambdaExpression memberSelector)
        {
            if (memberSelector == null) return null;
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

        public static Type GetType(string name)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            return GetType(assemblyName, typeName);
        }

        public static void SplitName(string name, out string assemblyName, out string typeName, out string value)
        {
            var names = (name ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            assemblyName = names.Length > 0 ? names[0] : string.Empty;
            typeName = names.Length > 1 ? names[1] : string.Empty;
            value = names.Length > 2 ? names[2] : string.Empty;
        }

        public static Type GetType(string assemblyName, string typeName)
        {
            return Cache.Get("types", assemblyName ?? "", typeName ?? "", () =>
            {
                if (string.IsNullOrWhiteSpace(typeName)) return null;
                assemblyName = assemblyName ?? "";
                typeName = typeName ?? "";
                Assembly assembly;
                Type type;
                if (!string.IsNullOrWhiteSpace(assemblyName))
                {
                    assembly = GetAssembly(assemblyName);
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
                        assembly = GetAssembly(assemblyName);
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
            });
        }

        public static string GetName(object value)
        {
            if (value == null) return string.Empty;
            var type = value.GetType();
            var stringValue = type.IsSimple() ? $", {value.AsString()}" : string.Empty;
            return $"{GetName(type)}{stringValue}";
        }

        public static string GetName<T>()
        {
            return GetName(typeof(T));
        }

        public static string GetName(Type type)
        {
            return type != null ? $"{Path.GetFileNameWithoutExtension(type.Assembly.Location)}, {type}" : string.Empty;
        }

        public static string GetName(string assemblyName, string typeName)
        {
            return $"{assemblyName}, {typeName}";
        }

        public static void GetNames(object value, out string assemblyName, out string typeName, out string valueName)
        {
            assemblyName = null;
            typeName = null;
            valueName = null;
            if (value == null) return;
            var type = value.GetType();
            valueName = value.AsString();
            GetNames(type, out assemblyName, out typeName);
        }

        public static void GetNames<T>(out string assemblyName, out string typeName)
        {
            GetNames(typeof(T), out assemblyName, out typeName);
        }

        public static void GetNames(Type type, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = null;
            if (type == null) return;
            if (type.ToString().ToLower().Contains("dynamicmodule")) type = type.BaseType;
            if (type == null) return;
            assemblyName = Path.GetFileNameWithoutExtension(type.Assembly.Location);
            typeName = type.ToString();
        }

        public static void GetNames(string metaName, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = null;
            if (string.IsNullOrWhiteSpace(metaName)) return;
            var names = metaName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            assemblyName = names.Length > 0 ? names[0] : null;
            typeName = names.Length > 1 ? names[1] : null;
        }

        public static T CreateInstance<T>(string name, object[] args = null)
        {
            return (T)CreateInstance(name, args);
        }

        public static object CreateInstance(string name, object[] args = null)
        {
            string assemblyName, typeName, value;
            SplitName(name, out assemblyName, out typeName, out value);
            if (string.IsNullOrWhiteSpace(value)) return CreateInstance(assemblyName, typeName, args);
            var type = GetType(assemblyName, typeName);
            return IsSimple(type) ? Convert.As(value, type) : CreateInstance(assemblyName, typeName, args);
        }

        public static T CreateInstance<T>(string assemblyName, string typeName, object[] args = null)
        {
            return (T)CreateInstance(assemblyName, typeName, args);
        }

        public static object CreateInstance(string assemblyName, string typeName, object[] args = null)
        {
            return CreateInstance(GetType(assemblyName, typeName), args);
        }

        public static T CreateInstance<T>(object[] args = null)
        {
            return (T)CreateInstance(typeof(T), args);
        }

        public static object CreateInstance(Type type, object[] args = null)
        {
            if (type == null) return null;
            if (type.IsString()) return Activator.CreateInstance(type, new char[0]);
            if (args == null || args.Length == 0) return Activator.CreateInstance(type, true);
            return Activator.CreateInstance(type, args);
        }

        public static Assembly GetAssembly(string assemblyName)
        {
            return Cache.Get("assemblies", (assemblyName ?? "").ToLower(), () => assemblyName != string.Empty ? LoadAssembly(assemblyName) : null);
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
            return (HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("~")) ?? string.Empty;
        }

        public static object GetValue(object instance, string propertyName)
        {
            if (instance == null) return null;
            var type = instance.GetType();
            var name = Property.ToNormName(propertyName);
            var tuple = Property.GetTupleSetter(type, name);
            if (tuple == null) return null;
            var getter = tuple.Item2;
            return getter.DynamicInvoke(instance);
        }

        public static void SetValue(object instance, string propertyName, object value)
        {
            if (instance == null) return;
            var type = instance.GetType();
            var name = Property.ToNormName(propertyName);
            var tuple = Property.GetTupleSetter(type, name);
            if (tuple == null) return;
            var propertyType = tuple.Item1;
            value = Metadata.Convert.As(value, propertyType);
            var setter = tuple.Item3;
            setter.DynamicInvoke(instance, value);
        }
    }
}