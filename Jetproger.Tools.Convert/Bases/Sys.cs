using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web;
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Bases
{
    public static class SysExtensions
    {
        private static readonly string SYS_ICON = @"AAABAAEAEBAAAAAAGABoAwAAFgAAACgAAAAQAAAAIAAAAAEAGAAAAAAAAAMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACyYzmARyl9RSh7RCd6RCd3QiZyQCVvPiRsPCNpOyJmOSFjNyBhNh9gNR8AAAAAAAC4ZjvYoIPYoIPQjmvHeE+4ZjueVzKGSit4QiZ3QiZ4QiZ3QiZbMh1hNh8AAAAAAAC/aj3oybjiuaTbqY7TlXTJfVW+aj2eVzKDSSp4QiZsPCOMTi17RCdjNyAAAAAAAADEcUXht6Hw29DiuaTZo4fQj2zHeE62ZTqSUS9jNyDCbD67aDwAAABmOSEAAAAAAADHeE/Mg13iuaTw29DiuaTYoIPOiWTDb0KSUS/Zo4fSknEAAADOiGNpOyIAAAAAAADLgVrz4djKf1fiuaPw2s/ht6HUl3egWTPhtqDjvKcAAADiuaPQjmtsPCMAAAAAAADOimb+/fzz4djFc0jiuaPlwK2HSyvbqY59RSh9RSjdrZTnxrXTlXRvPiQAAAAAAADRkW/+/fz+/fzw29DFc0jFc0ju1snz49rw29Dt1Mfrz8HpyrrWnH1yQCUAAAAAAADUmHj+/fz+/fz+/fz9+vj68/D47+rnxLKyYzmtYDfhtqDrz8HZo4Z3QiYAAAAAAADXn4H+/fz+/fz+/fz+/fz9+vj68/DDb0LbqY/FdEmtYDft1Mfcq5F6RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz9+vjEcUXu1srbqY+yYznw29Dfspp7RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fzw29DEcUXDb0LnxLLz49rhtqB9RSgAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fz+/fz9+vj68/D47+r16OHz49qARykAAAAAAADZo4bZo4bZo4bZo4bXn4HUmHjRkW/OimbLgVrHeE/EcUW/aj24ZjuyYzkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//wAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAAD//wAA";
        private static readonly string SYS_IMAGE = @"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIDSURBVDhPpZLrS5NhGMb3j4SWh0oRQVExD4gonkDpg4hGYKxG6WBogkMZKgPNCEVJFBGdGETEvgwyO9DJE5syZw3PIlPEE9pgBCLZ5XvdMB8Ew8gXbl54nuf63dd90OGSnwCahxbPRNPAPMw9Xpg6ZmF46kZZ0xSKzJPIrhpDWsVnpBhGkKx3nAX8Pv7z1zg8OoY/cITdn4fwbf/C0kYAN3Ma/w3gWfZL5kzTKBxjWyK2DftwI9tyMYCZKXbNHaD91bLYJrDXsYbrWfUKwJrPE9M2M1OcVzOOpHI7Jr376Hi9ogHqFIANO0/MmmmbmSmm9a8ze+I4MrNWAdjtoJgWcx+PSzg166yZZ8xM8XvXDix9c4jIqFYAjoriBV9AhEPv1mH/sonogha0afbZMMZz+yreTGyhpusHwtNNCsA5U1zS4BLxzJIfg299qO32Ir7UJtZfftyATqeT+8o2D8JSjQrAJblrncYL7ZJ2+bfaFnC/1S1NjL3diRat7qrO7wLRP3HjWsojBeComDEo5mNjuweFGvjWg2EBhCbpkW78htSHHwRyNdmgAFzPEee2iFkzayy2OLXzT4gr6UdUnlXrullsxxQ+kx0g8BTA3aZlButjSTyjODq/WcQcW/B/Je4OQhLvKQDnzN1mp0nnkvAhR8VuMzNrpm1mpjgkoVwB/v8DTgDQASA1MVpwzwAAAABJRU5ErkJggg==";

        public static void threadof(this f.ISysExpander exp, Action working)
        { 
            t<ReusableThread>.few().Execute(working);
        }

        public static Thread threadof(this f.ISysExpander exp, Action working, long periodMilliseconds)
        {
            var timespan = TimeSpan.FromMilliseconds(periodMilliseconds);
            var thread = new Thread(() =>
            {
                for (var i = long.MinValue; i < long.MaxValue; i++)
                {
                    try { working(); }
                    catch (Exception e) { f.log(e); }
                    finally { Thread.Sleep(timespan); }
                }
            });
            thread.IsBackground = true;
            thread.Start();
            return thread;
        }

        public static ICommand commandof(this f.ISysExpander exp, string commandTypeName)
        {
            var commandType = classof(exp, commandTypeName); 
            f.err<TypeNotFoundException>(commandType == null, commandTypeName);
            var command = valueof(exp, commandType) as ICommand;
            f.err<TypeNotSubtypeException>(command == null, commandTypeName, typeof(ICommand).FullName);
            return command;
        }

        public static T[] arrayof<T>(this f.ISysExpander exp, params T[] elements)
        {
            return elements;
        }

        public static List<T> listof<T>(this f.ISysExpander exp, params T[] elements)
        {
            var list = new List<T>();
            list.AddRange(elements);
            return list;
        }

        public static int indexof(this f.ISysExpander exp, IEnumerable items, object item)
        {
            var i = 0;
            foreach (object obj in items)
            {
                if (ReferenceEquals(obj, item)) return i;
                i++;
            }
            return -1;
        }

        public static ResourceManager resourceof(this f.ISysExpander e, string resourceName, string assemblyName)
        {
            try
            {
                var assembly = moduleof(e, assemblyName);
                var baseName = $"{assemblyName}.Bases.{resourceName}";
                return new ResourceManager(baseName, assembly);
            }
            catch
            {
                return null;
            }
        }

        public static string valueof(this f.ISysExpander e, ResourceManager resourceManager, string resourceKey)
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

        public static T valueof<T>(this f.ISysExpander e, string metaName, object[] args = null)
        {
            return (T)valueof(e, metaName, args);
        }

        public static object valueof(this f.ISysExpander e, string metaName, object[] args = null)
        {
            string assemblyName, typeName, value;
            nameof(e, metaName, out assemblyName, out typeName, out value);
            if (string.IsNullOrWhiteSpace(value)) return valueof(e, assemblyName, typeName, args);
            var type = classof(e, assemblyName, typeName);
            return issimple(e, type) ? value.As(type) : valueof(e, assemblyName, typeName, args);
        }

        public static T valueof<T>(this f.ISysExpander e, string assemblyName, string typeName, object[] args = null)
        {
            return (T)valueof(e, assemblyName, typeName, args);
        }

        public static object valueof(this f.ISysExpander e, string assemblyName, string typeName, object[] args = null)
        {
            return valueof(e, classof(e, assemblyName, typeName), args);
        }

        public static T valueof<T>(this f.ISysExpander e, object[] args = null)
        {
            return (T)valueof(e, typeof(T), args);
        }

        public static object valueof(this f.ISysExpander e, Type type, object[] args = null)
        {
            if (type == null) return null;
            if (type == typeof(string)) return Activator.CreateInstance(type, new char[0]);
            if (args == null || args.Length == 0) return Activator.CreateInstance(type, true);
            return Activator.CreateInstance(type, args);
        }

        public static Type classof(this f.ISysExpander e, string name)
        {
            string assemblyName, typeName, value;
            nameof(e, name, out assemblyName, out typeName, out value);
            return classof(e, assemblyName, typeName);
        }

        public static Type classof(this f.ISysExpander e, string assemblyName, string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName)) return null;
            assemblyName = assemblyName ?? string.Empty;
            typeName = typeName ?? string.Empty;
            Assembly assembly;
            Type type;
            if (!string.IsNullOrWhiteSpace(assemblyName))
            {
                assembly = moduleof(e, assemblyName);
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
                    assembly = moduleof(e, assemblyName);
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

        public static Assembly moduleof(this f.ISysExpander e, string assemblyName)
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
                name = Path.Combine(f.fss.appdir(), name);
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

        public static void nameof(this f.ISysExpander e, object value, out string assemblyName, out string typeName, out string valueName)
        {
            assemblyName = null;
            typeName = null;
            valueName = null;
            if (value == null) return;
            var type = value.GetType();
            valueName = value.As<string>();
            nameof(e, type, out assemblyName, out typeName);
        }

        public static void nameof<T>(this f.ISysExpander e, out string assemblyName, out string typeName)
        {
            nameof(e, typeof(T), out assemblyName, out typeName);
        }

        public static void nameof(this f.ISysExpander e, Type type, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = null;
            if (type == null) return;
            if (type.ToString().ToLower().Contains("dynamicmodule")) type = type.BaseType;
            if (type == null) return;
            assemblyName = Path.GetFileNameWithoutExtension(type.Assembly.Location);
            typeName = type.ToString();
        }

        public static void nameof(this f.ISysExpander e, string metaName, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = null;
            if (string.IsNullOrWhiteSpace(metaName)) return;
            var names = metaName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            assemblyName = names.Length > 0 ? names[0] : null;
            typeName = names.Length > 1 ? names[1] : null;
        }

        public static void nameof(this f.ISysExpander e, string metaName, out string assemblyName, out string typeName, out string value)
        {
            var names = (metaName ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            assemblyName = names.Length > 1 ? names[0] : string.Empty;
            typeName = names.Length > 1 ? names[1] : metaName;
            value = names.Length > 2 ? names[2] : string.Empty;
        }

        public static string printof(this f.ISysExpander e, object value)
        {
            if (value == null) return string.Empty;
            var type = value.GetType();
            var stringValue = issimple(e, type) ? $", {value.As<string>()}" : string.Empty;
            return $"{printof(e, type)}{stringValue}";
        }

        public static string printof<T>(this f.ISysExpander e)
        {
            return printof(e, typeof(T));
        }

        public static string printof(this f.ISysExpander e, Type type)
        {
            return type != null ? $"{Path.GetFileNameWithoutExtension(type.Assembly.Location)}, {type}" : string.Empty;
        }

        public static string printof(this f.ISysExpander e, string assemblyName, string typeName)
        {
            return $"{assemblyName}, {typeName}";
        }

        public static string printof(this f.ISysExpander e, LambdaExpression memberSelector)
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

        public static T defaultof<T>(this f.ISysExpander e)
        {
            return (T)defaultof(e, typeof(T));
        }

        public static object defaultof(this f.ISysExpander e, Type type)
        {
            return
            type == typeof(Icon) ? SYS_ICON.As<Icon>() : (
            type == typeof(Image) ? SYS_IMAGE.As<Image>() : (
            type.IsValueType ? Activator.CreateInstance(type) : null));
        }

        public static Type genericof(this f.ISysExpander e, Type type)
        {
            var types = type.GetGenericArguments();
            if (types.Length > 0) return types[0];
            return type.HasElementType ? type.GetElementType() : null;
        }

        public static bool islist(this f.ISysExpander e, Type type)
        {
            if (type == null) return false;
            var genericType = genericof(e, type);
            return genericType != null && type.GetGenericTypeDefinition() == typeof(List<>) && genericType.IsClass && !typeof(IEnumerable).IsAssignableFrom(genericType);
        }

        public static bool isof(this f.ISysExpander e, Type type, Type sample)
        {
            return type == sample || type.IsSubclassOf(sample) || type.GetInterfaces().Any(x => x == sample);
        }

        public static bool issimple(this f.ISysExpander e, Type type)
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

    
    [Serializable]
    public class ReusableThread
    {
        private readonly ManualResetEvent _mre = new ManualResetEvent(false);
        private Action _working;
        private Thread _thread;

        public void Execute(Action working)
        {
            _thread = _thread ?? f.sys.threadof(Working, 0);
            _working = working;
            _mre.Set();
        }

        private void Working()
        {
            _mre.WaitOne();
            _mre.Reset();
            try { if (_working != null) _working(); }
            catch (Exception e) { f.log(e); }
            finally { t<ReusableThread>.few(this); }
        }
    }
}