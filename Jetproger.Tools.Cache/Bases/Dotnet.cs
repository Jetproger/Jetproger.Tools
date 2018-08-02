using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Web;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Cache.Bases
{
    public static class DotnetExtensions
    {
        public static FieldInfo GetField(this IDotnetExpander expander, object value, string fieldName)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetField(assemblyName, typeName, fieldName);
        }

        public static FieldInfo GetField<T>(this IDotnetExpander expander, string fieldName)
        {
            return expander.GetField(typeof(T), fieldName);
        }

        public static FieldInfo GetField(this IDotnetExpander expander, Type type, string fieldName)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetField(assemblyName, typeName, fieldName);
        }

        public static FieldInfo GetField(this IDotnetExpander expander, string name, string fieldName)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetField(assemblyName, typeName, fieldName);
        }

        public static FieldInfo GetField(this IDotnetExpander expander, string assemblyName, string typeName, string fieldName)
        {
            return Ex.Cache.Read("field", assemblyName ?? "", typeName ?? "", fieldName ?? "", () =>
            {
                foreach (FieldInfo fieldInfo in expander.GetFields(assemblyName, typeName))
                {
                    if (fieldInfo.Name == fieldName) return fieldInfo;
                }
                return null;
            });
        }

        public static FieldInfo[] GetFields(this IDotnetExpander expander, object value)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetFields(assemblyName, typeName);
        }

        public static FieldInfo[] GetFields<T>(this IDotnetExpander expander)
        {
            return expander.GetFields(typeof(T));
        }

        public static FieldInfo[] GetFields(this IDotnetExpander expander, Type type)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetFields(assemblyName, typeName);
        }

        public static FieldInfo[] GetFields(this IDotnetExpander expander, string name)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetFields(assemblyName, typeName);
        }

        public static FieldInfo[] GetFields(this IDotnetExpander expander, string assemblyName, string typeName)
        {
            return Ex.Cache.Read("fields", assemblyName ?? "", typeName ?? "", () =>
            {
                var type = expander.GetType(assemblyName, typeName);
                return type == null ? new FieldInfo[0] : (type.IsEnum ? type.GetFields() : type.GetFields(BindingFlags.Instance | BindingFlags.Public));
            });
        }

        public static MethodInfo GetMethod(this IDotnetExpander expander, object value, string methodName)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetMethod(assemblyName, typeName, methodName);
        }

        public static MethodInfo GetMethod<T>(this IDotnetExpander expander, string methodName)
        {
            return expander.GetMethod(typeof(T), methodName);
        }

        public static MethodInfo GetMethod(this IDotnetExpander expander, Type type, string methodName)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetMethod(assemblyName, typeName, methodName);
        }

        public static MethodInfo GetMethod(this IDotnetExpander expander, string name, string methodName)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetMethod(assemblyName, typeName, methodName);
        }

        public static MethodInfo GetMethod(this IDotnetExpander expander, string assemblyName, string typeName, string methodName)
        {
            return Ex.Cache.Read("method", assemblyName ?? "", typeName ?? "", methodName ?? "", () =>
            {
                foreach (MethodInfo methodInfo in expander.GetMethods(assemblyName, typeName))
                {
                    if (methodInfo.Name == methodName) return methodInfo;
                }
                return null;
            });
        }

        public static MethodInfo[] GetMethods(this IDotnetExpander expander, object value)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetMethods(assemblyName, typeName);
        }

        public static MethodInfo[] GetMethods<T>(this IDotnetExpander expander)
        {
            return expander.GetMethods(typeof(T));
        }

        public static MethodInfo[] GetMethods(this IDotnetExpander expander, Type type)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetMethods(assemblyName, typeName);
        }

        public static MethodInfo[] GetMethods(this IDotnetExpander expander, string name)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetMethods(assemblyName, typeName);
        }

        public static MethodInfo[] GetMethods(this IDotnetExpander expander, string assemblyName, string typeName)
        {
            return Ex.Cache.Read("methods", assemblyName ?? "", typeName ?? "", () =>
            {
                var type = expander.GetType(assemblyName, typeName);
                return type?.GetMethods(BindingFlags.Instance | BindingFlags.Public) ?? new MethodInfo[0];
            });
        }

        public static PropertyInfo GetProperty(this IDotnetExpander expander, object value, string propertyName)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetProperty(assemblyName, typeName, propertyName);
        }

        public static PropertyInfo GetProperty<T>(this IDotnetExpander expander, string propertyName)
        {
            return expander.GetProperty(typeof(T), propertyName);
        }

        public static PropertyInfo GetProperty(this IDotnetExpander expander, Type type, string propertyName)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetProperty(assemblyName, typeName, propertyName);
        }

        public static PropertyInfo GetProperty(this IDotnetExpander expander, string name, string propertyName)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetProperty(assemblyName, typeName, propertyName);
        }

        public static PropertyInfo GetProperty(this IDotnetExpander expander, string assemblyName, string typeName, string propertyName)
        {
            return Ex.Cache.Read("property", assemblyName ?? "", typeName ?? "", propertyName ?? "", () =>
            {
                foreach (PropertyInfo propertyInfo in expander.GetProperties(assemblyName, typeName))
                {
                    if (propertyInfo.Name == propertyName) return propertyInfo;
                }
                return null;
            });
        }

        public static PropertyInfo[] GetProperties(this IDotnetExpander expander, object value)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetProperties<T>(this IDotnetExpander expander)
        {
            return expander.GetProperties(typeof(T));
        }

        public static PropertyInfo[] GetProperties(this IDotnetExpander expander, Type type)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetProperties(this IDotnetExpander expander, string name)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetProperties(this IDotnetExpander expander, string assemblyName, string typeName)
        {
            return Ex.Cache.Read("properties", assemblyName ?? "", typeName ?? "", "properties", () =>
            {
                var type = expander.GetType(assemblyName, typeName);
                return type?.GetProperties(BindingFlags.Instance | BindingFlags.Public) ?? new PropertyInfo[0];
            });
        }

        public static PropertyInfo[] GetSelfProperties(this IDotnetExpander expander, object value)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetSelfProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSelfProperties<T>(this IDotnetExpander expander)
        {
            return expander.GetSelfProperties(typeof(T));
        }

        public static PropertyInfo[] GetSelfProperties(this IDotnetExpander expander, Type type)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetSelfProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSelfProperties(this IDotnetExpander expander, string name)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetSelfProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSelfProperties(this IDotnetExpander expander, string assemblyName, string typeName)
        {
            return Ex.Cache.Read("selfproperties", assemblyName ?? "", typeName ?? "", "properties", () =>
            {
                var type = expander.GetType(assemblyName, typeName);
                return type?.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly) ?? new PropertyInfo[0];
            });
        }

        public static PropertyInfo[] GetSimpleProperties(this IDotnetExpander expander, object value)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetSimpleProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSimpleProperties<T>(this IDotnetExpander expander)
        {
            return expander.GetSimpleProperties(typeof(T));
        }

        public static PropertyInfo[] GetSimpleProperties(this IDotnetExpander expander, Type type)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetSimpleProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSimpleProperties(this IDotnetExpander expander, string name)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetSimpleProperties(assemblyName, typeName);
        }

        public static PropertyInfo[] GetSimpleProperties(this IDotnetExpander expander, string assemblyName, string typeName)
        {
            return Ex.Cache.Read("simpleproperties", assemblyName ?? "", typeName ?? "", "properties", () =>
            {
                return expander.GetProperties(assemblyName, typeName).Where(p => ValueExtensions.IsPrimitive(p.PropertyType)).ToArray();
            });
        }

        public static T GetTypeAttribute<T>(this IDotnetExpander expander, object value) where T : Attribute
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetTypeAttribute<T>(assemblyName, typeName);
        }

        public static T GetTypeAttribute<T>(this IDotnetExpander expander, Type type) where T : Attribute
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetTypeAttribute<T>(assemblyName, typeName);
        }

        public static T GetTypeAttribute<T>(this IDotnetExpander expander, string name) where T : Attribute
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetTypeAttribute<T>(assemblyName, typeName);
        }

        public static T GetTypeAttribute<T>(this IDotnetExpander expander, string assemblyName, string typeName) where T : Attribute
        {
            return Ex.Cache.Read("typeattribute", assemblyName ?? "", typeName ?? "", () =>
            {
                var type = expander.GetType(assemblyName, typeName);
                var customAttributes = type?.GetCustomAttributes(typeof(T), true);
                return customAttributes?.Length > 0 ? customAttributes[0] as T : default(T);
            });
        }

        public static Attribute[] GetTypeAttributes(this IDotnetExpander expander, object value)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetTypeAttributes(assemblyName, typeName);
        }

        public static Attribute[] GetTypeAttributes(this IDotnetExpander expander, Type type)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetTypeAttributes(assemblyName, typeName);
        }

        public static Attribute[] GetTypeAttributes(this IDotnetExpander expander, string name)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetTypeAttributes(assemblyName, typeName);
        }

        public static Attribute[] GetTypeAttributes(this IDotnetExpander expander, string assemblyName, string typeName)
        {
            return Ex.Cache.Read("typeattributes", assemblyName ?? "", typeName ?? "", () =>
            {
                var type = expander.GetType(assemblyName, typeName);
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

        public static T GetFieldAttribute<T>(this IDotnetExpander expander, object value, string fieldName) where T : Attribute
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetFieldAttribute<T>(assemblyName, typeName, fieldName);
        }

        public static T GetFieldAttribute<T>(this IDotnetExpander expander, Type type, string fieldName) where T : Attribute
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetFieldAttribute<T>(assemblyName, typeName, fieldName);
        }

        public static T GetFieldAttribute<T>(this IDotnetExpander expander, string name, string fieldName) where T : Attribute
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetFieldAttribute<T>(assemblyName, typeName, fieldName);
        }

        public static T GetFieldAttribute<T>(this IDotnetExpander expander, string assemblyName, string typeName, string fieldName) where T : Attribute
        {
            return Ex.Cache.Read("fieldattribute", assemblyName ?? "", typeName ?? "", fieldName ?? "", () =>
            {
                var fieldInfo = expander.GetField(assemblyName, typeName, fieldName);
                var customAttributes = fieldInfo.GetCustomAttributes(typeof(T), true);
                return customAttributes.Length > 0 ? customAttributes[0] as T : default(T);
            });
        }

        public static Attribute[] GetFieldAttributes(this IDotnetExpander expander, object value, string fieldName)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetFieldAttributes(assemblyName, typeName, fieldName);
        }

        public static Attribute[] GetFieldAttributes(this IDotnetExpander expander, Type type, string fieldName)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetFieldAttributes(assemblyName, typeName, fieldName);
        }

        public static Attribute[] GetFieldAttributes(this IDotnetExpander expander, string name, string fieldName)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetFieldAttributes(assemblyName, typeName, fieldName);
        }

        public static Attribute[] GetFieldAttributes(this IDotnetExpander expander, string assemblyName, string typeName, string fieldName)
        {
            return Ex.Cache.Read("fieldattributes", assemblyName ?? "", typeName ?? "", fieldName ?? "", () =>
            {
                var fieldInfo = expander.GetField(assemblyName, typeName, fieldName);
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

        public static T GetMethodAttribute<T>(this IDotnetExpander expander, object value, string methodName) where T : Attribute
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetMethodAttribute<T>(assemblyName, typeName, methodName);
        }

        public static T GetMethodAttribute<T>(this IDotnetExpander expander, Type type, string methodName) where T : Attribute
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetMethodAttribute<T>(assemblyName, typeName, methodName);
        }

        public static T GetMethodAttribute<T>(this IDotnetExpander expander, string name, string methodName) where T : Attribute
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetMethodAttribute<T>(assemblyName, typeName, methodName);
        }

        public static T GetMethodAttribute<T>(this IDotnetExpander expander, string assemblyName, string typeName, string methodName) where T : Attribute
        {
            return Ex.Cache.Read("methodattribute", assemblyName ?? "", typeName ?? "", methodName ?? "", () =>
            {
                var methodInfo = expander.GetMethod(assemblyName, typeName, methodName);
                var customAttributes = methodInfo.GetCustomAttributes(typeof(T), true);
                return customAttributes.Length > 0 ? customAttributes[0] as T : default(T);
            });
        }

        public static Attribute[] GetMethodAttributes(this IDotnetExpander expander, object value, string methodName)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetMethodAttributes(assemblyName, typeName, methodName);
        }

        public static Attribute[] GetMethodAttributes(this IDotnetExpander expander, Type type, string methodName)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetMethodAttributes(assemblyName, typeName, methodName);
        }

        public static Attribute[] GetMethodAttributes(this IDotnetExpander expander, string name, string methodName)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetMethodAttributes(assemblyName, typeName, methodName);
        }

        public static Attribute[] GetMethodAttributes(this IDotnetExpander expander, string assemblyName, string typeName, string methodName)
        {
            return Ex.Cache.Read("methodattributes", assemblyName ?? "", typeName ?? "", methodName ?? "", () => {
                var methodInfo = expander.GetMethod(assemblyName, typeName, methodName);
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

        public static T GetPropertyAttribute<T>(this IDotnetExpander expander, object value, string propertyName) where T : Attribute
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetPropertyAttribute<T>(assemblyName, typeName, propertyName);
        }

        public static T GetPropertyAttribute<T>(this IDotnetExpander expander, Type type, string propertyName) where T : Attribute
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetPropertyAttribute<T>(assemblyName, typeName, propertyName);
        }

        public static T GetPropertyAttribute<T>(this IDotnetExpander expander, string name, string propertyName) where T : Attribute
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetPropertyAttribute<T>(assemblyName, typeName, propertyName);
        }

        public static T GetPropertyAttribute<T>(this IDotnetExpander expander, string assemblyName, string typeName, string propertyName) where T : Attribute
        {
            return Ex.Cache.Read("propertyattribute", assemblyName ?? "", typeName ?? "", propertyName ?? "", () =>
            {
                var propertyInfo = expander.GetProperty(assemblyName, typeName, propertyName);
                var customAttributes = propertyInfo?.GetCustomAttributes(typeof(T), true);
                return customAttributes?.Length > 0 ? customAttributes[0] as T : default(T);
            });
        }

        public static Attribute[] GetPropertyAttributes(this IDotnetExpander expander, object value, string propertyName)
        {
            string assemblyName, typeName, valueName;
            expander.ParseName(value, out assemblyName, out typeName, out valueName);
            return expander.GetPropertyAttributes(assemblyName, typeName, propertyName);
        }

        public static Attribute[] GetPropertyAttributes(this IDotnetExpander expander, Type type, string propertyName)
        {
            string assemblyName, typeName;
            expander.ParseName(type, out assemblyName, out typeName);
            return expander.GetPropertyAttributes(assemblyName, typeName, propertyName);
        }

        public static Attribute[] GetPropertyAttributes(this IDotnetExpander expander, string name, string propertyName)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetPropertyAttributes(assemblyName, typeName, propertyName);
        }

        public static Attribute[] GetPropertyAttributes(this IDotnetExpander expander, string assemblyName, string typeName, string propertyName)
        {
            return Ex.Cache.Read("propertyattributes", assemblyName ?? "", typeName ?? "", propertyName ?? "", () =>
            {
                var propertyInfo = expander.GetProperty(assemblyName, typeName, propertyName);
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

        public static string GetMemberName(this IDotnetExpander expander, LambdaExpression memberSelector)
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

        public static object GetConsole(this IDotnetExpander expander, Type type)
        {
            return Ex.Cache.Read("consolesettings", type.AssemblyQualifiedName, () => {
                var setting = (ExSetting)CreateInstance(expander, type);
                string name;
                setting.IsDeclare = Ex.Cache.Read("commandlinearguments", setting.Code.ToLower(), out name);
                if (setting.IsDeclare) setting.Name = name;
                setting.Validate();
                return setting;
            });
        }

        public static object GetConfig(this IDotnetExpander expander, Type type)
        {
            return Ex.Cache.Read("configsettings", type.AssemblyQualifiedName, () =>
            {
                var setting = (ExSetting)CreateInstance(expander, type);
                string name;
                setting.IsDeclare = Ex.Cache.Read("appconfiguration", setting.Code.ToLower(), out name);
                if (setting.IsDeclare) setting.Name = name;
                setting.Validate();
                return setting;
            });
        }

        public static object GetResource(this IDotnetExpander expander, Type type)
        {
            return Ex.Cache.Read("resourcesettings", type.AssemblyQualifiedName, () =>
            {
                var setting = (ExSetting)CreateInstance(expander, type);
                setting.Name = GetSettingValue(setting, setting.Name, GetResourceNames(expander, setting.AssemblyName, setting.Code));
                setting.Description = GetSettingValue(setting, setting.Description, GetResourceDescriptions(expander, setting.AssemblyName, setting.Code));
                setting.Shortcut = GetSettingValue(setting, setting.Shortcut, GetResourceShortcuts(expander, setting.AssemblyName, setting.Code));
                setting.SpecName = GetSettingValue(setting, setting.SpecName, GetResourceSpecifies(expander, setting.AssemblyName, setting.Code));
                setting.Picture = System.Convert.FromBase64String(GetSettingValue(setting, ImageExtensions.DefaultImageString, GetResourcePictures(expander, setting.AssemblyName, setting.Code)));
                setting.Validate();
                return setting;
            });
        }

        private static string GetSettingValue(ExSetting setting, string currentValue, string newValue)
        {
            if (newValue == null) return currentValue;
            setting.IsDeclare = true;
            return !string.IsNullOrWhiteSpace(newValue) ? newValue : currentValue;
        }

        private static string GetResourceNames(IDotnetExpander expander, string assemblyName, string resourceKey)
        {
            return GetResource(expander, assemblyName, "ResourceNames", resourceKey);
        }

        private static string GetResourceDescriptions(IDotnetExpander expander, string assemblyName, string resourceKey)
        {
            return GetResource(expander, assemblyName, "ResourceDescriptions", resourceKey);
        }

        private static string GetResourceShortcuts(IDotnetExpander expander, string assemblyName, string resourceKey)
        {
            return GetResource(expander, assemblyName, "ResourceShortcuts", resourceKey);
        }

        private static string GetResourceSpecifies(IDotnetExpander expander, string assemblyName, string resourceKey)
        {
            return GetResource(expander, assemblyName, "ResourceSpecifies", resourceKey);
        }

        private static string GetResourcePictures(IDotnetExpander expander, string assemblyName, string resourceKey)
        {
            return GetResource(expander, assemblyName, "ResourcePictures", resourceKey);
        }

        private static string GetResource(IDotnetExpander expander, string assemblyName, string resourceName, string resourceKey)
        {
            assemblyName = GetAssembly(expander, assemblyName).GetName().Name;
            return Ex.Cache.Read("resources", (assemblyName ?? string.Empty).ToLower(), (resourceName ?? string.Empty).ToLower(), (resourceKey ?? string.Empty).ToLower(), () =>
            {
                var baseName = $"{assemblyName}.Resx.{resourceName}";
                var rm = GetResourceManager(expander, baseName, assemblyName);
                return GetResource(rm, resourceKey);
            });
        }

        private static ResourceManager GetResourceManager(IDotnetExpander expander, string baseName, string assemblyName)
        {
            return Ex.Cache.Read("resourcemanagers", (assemblyName ?? string.Empty).ToLower(), (baseName ?? string.Empty).ToLower(), () =>
            {
                try
                {
                    return new ResourceManager(baseName ?? string.Empty, GetAssembly(expander, assemblyName));
                }
                catch
                {
                    return null;
                }
            });
        }

        private static string GetResource(ResourceManager resourceManager, string resourceKey)
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

        public static Type GetType(this IDotnetExpander expander, string name)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            return expander.GetType(assemblyName, typeName);
        }

        public static Type GetType(this IDotnetExpander expander, string assemblyName, string typeName)
        {
            return Ex.Cache.Read("types", assemblyName ?? "", typeName ?? "", () =>
            {
                if (string.IsNullOrWhiteSpace(typeName)) return null;
                assemblyName = assemblyName ?? "";
                typeName = typeName ?? "";
                Assembly assembly;
                Type type;
                if (!string.IsNullOrWhiteSpace(assemblyName))
                {
                    assembly = expander.GetAssembly(assemblyName);
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
                        assembly = expander.GetAssembly(assemblyName);
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

        public static T CreateInstance<T>(this IDotnetExpander expander, string name, object[] args = null)
        {
            return (T)expander.CreateInstance(name, args);
        }

        public static object CreateInstance(this IDotnetExpander expander, string name, object[] args = null)
        {
            string assemblyName, typeName, value;
            expander.ParseName(name, out assemblyName, out typeName, out value);
            if (string.IsNullOrWhiteSpace(value)) return expander.CreateInstance(assemblyName, typeName, args);
            var type = expander.GetType(assemblyName, typeName);
            return ValueExtensions.IsPrimitive(type) ? value.As(type) : expander.CreateInstance(assemblyName, typeName, args);
        }

        public static T CreateInstance<T>(this IDotnetExpander expander, string assemblyName, string typeName, object[] args = null)
        {
            return (T)expander.CreateInstance(assemblyName, typeName, args);
        }

        public static object CreateInstance(this IDotnetExpander expander, string assemblyName, string typeName, object[] args = null)
        {
            return expander.CreateInstance(expander.GetType(assemblyName, typeName), args);
        }

        public static T CreateInstance<T>(this IDotnetExpander expander, object[] args = null)
        {
            return (T)expander.CreateInstance(typeof(T), args);
        }

        public static object CreateInstance(this IDotnetExpander expander, Type type, object[] args = null)
        {
            if (type == null) return null;
            if (type == typeof(string)) return Activator.CreateInstance(type, new char[0]);
            if (args == null || args.Length == 0) return Activator.CreateInstance(type, true);
            return Activator.CreateInstance(type, args);
        }

        public static Assembly GetAssembly(this IDotnetExpander expander, string assemblyName)
        {
            return Ex.Cache.Read("assemblies", (assemblyName ?? "").ToLower(), () => assemblyName != string.Empty ? LoadAssembly(assemblyName) : null);
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

        public static object GetValue(this IDotnetExpander expander, object instance, string propertyName)
        {
            if (instance == null) return null;
            var type = instance.GetType();
            var name = Property.ToNormName(propertyName);
            var tuple = Property.GetTupleSetter(type, name);
            if (tuple == null) return null;
            var getter = tuple.Item2;
            return getter.DynamicInvoke(instance);
        }

        public static void SetValue(this IDotnetExpander expander, object instance, string propertyName, object value)
        {
            if (instance == null) return;
            var type = instance.GetType();
            var name = Property.ToNormName(propertyName);
            var tuple = Property.GetTupleSetter(type, name);
            if (tuple == null) return;
            var propertyType = tuple.Item1;
            value = value.As(propertyType);
            var setter = tuple.Item3;
            setter.DynamicInvoke(instance, value);
        }

        public static string BuildName(this IDotnetExpander expander, object value)
        {
            if (value == null) return string.Empty;
            var type = value.GetType();
            var stringValue = ValueExtensions.IsPrimitive(type) ? $", {value.As<string>()}" : string.Empty;
            return $"{expander.BuildName(type)}{stringValue}";
        }

        public static string BuildName<T>(this IDotnetExpander expander)
        {
            return expander.BuildName(typeof(T));
        }

        public static string BuildName(this IDotnetExpander expander, Type type)
        {
            return type != null ? $"{Path.GetFileNameWithoutExtension(type.Assembly.Location)}, {type}" : string.Empty;
        }

        public static string BuildName(this IDotnetExpander expander, string assemblyName, string typeName)
        {
            return $"{assemblyName}, {typeName}";
        }

        public static void ParseName(this IDotnetExpander expander, string name, out string assemblyName, out string typeName, out string value)
        {
            var names = (name ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            assemblyName = names.Length > 0 ? names[0] : string.Empty;
            typeName = names.Length > 1 ? names[1] : string.Empty;
            value = names.Length > 2 ? names[2] : string.Empty;
        }

        public static void ParseName(this IDotnetExpander expander, object value, out string assemblyName, out string typeName, out string valueName)
        {
            assemblyName = null;
            typeName = null;
            valueName = null;
            if (value == null) return;
            var type = value.GetType();
            valueName = value.As<string>();
            expander.ParseName(type, out assemblyName, out typeName);
        }

        public static void ParseName<T>(this IDotnetExpander expander, out string assemblyName, out string typeName)
        {
            expander.ParseName(typeof(T), out assemblyName, out typeName);
        }

        public static void ParseName(this IDotnetExpander expander, Type type, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = null;
            if (type == null) return;
            if (type.ToString().ToLower().Contains("dynamicmodule")) type = type.BaseType;
            if (type == null) return;
            assemblyName = Path.GetFileNameWithoutExtension(type.Assembly.Location);
            typeName = type.ToString();
        }

        public static void ParseName(this IDotnetExpander expander, string metaName, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = null;
            if (string.IsNullOrWhiteSpace(metaName)) return;
            var names = metaName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            assemblyName = names.Length > 0 ? names[0] : null;
            typeName = names.Length > 1 ? names[1] : null;
        }

        public static void RegisterSettings(this IDotnetExpander expander, string[] args)
        {
            Ex.RegisterResourceSettingFactory(new ResourceSettingFactory());
            Ex.RegisterConsoleSettingFactory(new ConsoleSettingFactory());
            Ex.RegisterConfigSettingFactory(new ConfigSettingFactory());
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                Ex.Cache.Read("appconfiguration", key.ToLower(), () => ConfigurationManager.AppSettings[key]);
            }
            foreach (var arg in args)
            {
                string key, value;
                if (!ParseArgument(arg, out key, out value)) continue;
                Ex.Cache.Read("commandlinearguments", key.ToLower(), () => value);
            }
        }

        private static bool ParseArgument(string arg, out string key, out string value)
        {
            key = null;
            value = null;
            if (string.IsNullOrWhiteSpace(arg)) return false;
            var colonIndex = arg.IndexOf(":", StringComparison.Ordinal);
            if (colonIndex < 0)
            {
                key = arg;
                return true;
            }
            if (colonIndex == 0)
            {
                return false;
            }
            key = arg.Substring(0, colonIndex);
            colonIndex++;
            if (colonIndex < arg.Length) value = arg.Substring(colonIndex);
            return true;
        }
    }

    public class ResourceSettingFactory : ISettingFactory
    {
        public object CreateSetting(Type type) { return Ex.Dotnet.GetResource(type); }
    }

    public class ConfigSettingFactory : ISettingFactory
    {
        public object CreateSetting(Type type) { return Ex.Dotnet.GetConfig(type); }
    }

    public class ConsoleSettingFactory : ISettingFactory
    {
        public object CreateSetting(Type type) { return Ex.Dotnet.GetConsole(type); }
    }
}