using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Jetproger.Tools.Convert.Bases
{
    public static class ConvertExtensions
    {
        private static readonly One<Type, MethodInfo> ConvertMethods = Ex.GetOne<Type, MethodInfo>(GetConvertMethod);

        public static T As<T>(this IDataReader reader, string columnName)
        {
            return reader.GetValue(reader.GetOrdinal(columnName)).As<T>();
        }

        public static T As<T>(this IDataReader reader, int columnIndex)
        {
            return reader.GetValue(columnIndex).As<T>();
        }

        public static T As<T>(this DataSet ds, int tableIndex, int rowIndex, string columnName)
        {
            return ds != null && tableIndex >= 0 && tableIndex < ds.Tables.Count && rowIndex >= 0 && rowIndex < ds.Tables[tableIndex].Rows.Count && ds.Tables[tableIndex].Columns.Contains(columnName) ? ds.Tables[tableIndex].Rows[rowIndex][columnName].As<T>() : default(T);
        }

        public static T As<T>(this DataSet ds, int tableIndex, int rowIndex, int columnIndex)
        {
            return ds != null && tableIndex >= 0 && tableIndex < ds.Tables.Count && rowIndex >= 0 && rowIndex < ds.Tables[tableIndex].Rows.Count && columnIndex >= 0 && columnIndex < ds.Tables[tableIndex].Columns.Count ? ds.Tables[tableIndex].Rows[rowIndex][columnIndex].As<T>() : default(T);
        }

        public static T As<T>(this DataTable table, int rowIndex, string columnName)
        {
            return table != null && rowIndex >= 0 && rowIndex < table.Rows.Count && table.Columns.Contains(columnName) ? table.Rows[rowIndex][columnName].As<T>() : default(T);
        }

        public static T As<T>(this DataTable table, int rowIndex, int columnIndex)
        {
            return table != null && rowIndex >= 0 && rowIndex < table.Rows.Count && columnIndex >= 0 && columnIndex < table.Columns.Count ? table.Rows[rowIndex][columnIndex].As<T>() : default(T);
        }

        public static T As<T>(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].As<T>() : default(T);
        }

        public static T As<T>(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].As<T>() : default(T);
        }

        public static T As<T>(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].As<T>() : default(T);
        }

        public static T As<T>(this IList values, int index)
        {
            return values != null && index >= 0 && index < values.Count ? values[index].As<T>() : default(T);
        }

        public static T As<T>(this object value)
        {
            if (value == null || value == DBNull.Value) return default(T);
            var typeFr = value.GetType();
            var typeTo = typeof(T);
            if (typeFr == typeTo) return (T)value;
            if (IsTypeOf(typeFr, typeTo)) return (T)value;
            if (value is string) return Ex.String.Read<T>((string)value);
            if (typeTo == typeof(string)) return (T)(object)Ex.String.Write(value);
            if (value is Icon && typeTo == typeof(Image)) return (T)(object)Ex.Image.Read((Icon)value);
            if (value is Image && typeTo == typeof(Icon)) return (T)(object)Ex.Image.ReadIcon((Image)value);
            if (value is Icon && typeTo == typeof(byte[])) return (T)(object)Ex.Image.Write((Icon)value);
            if (value is byte[] && typeTo == typeof(Icon)) return (T)(object)Ex.Image.ReadIcon((byte[])value);
            if (value is Image && typeTo == typeof(byte[])) return (T)(object)Ex.Image.Write((Image)value);
            if (value is byte[] && typeTo == typeof(Image)) return (T)(object)Ex.Image.Read((byte[])value);
            if ((typeTo == typeof(decimal) || typeTo == typeof(float) || typeTo == typeof(double) || typeTo == typeof(long) || typeTo == typeof(ulong) || typeTo == typeof(int) || typeTo == typeof(uint) || typeTo == typeof(short) || typeTo == typeof(ushort) || typeTo == typeof(byte) || typeTo == typeof(sbyte))
            && (value is decimal || value is float || value is double || value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte)) return Cast<T>(value);
            return (T)value;
        }

        public static object As(this object value, Type typeTo)
        {
            var method = ConvertMethods[typeTo];
            return method != null ? method.Invoke(null, new[] { value }) : typeTo.Default();
        }

        public static object Default(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool IsTypeOf(this Type type, Type sample)
        {
            return type == sample || type.IsSubclassOf(sample) || type.GetInterfaces().Any(interfaceType => interfaceType == sample);
        }

        public static Type GetGenericArgumentType(this Type type)
        {
            var types = type.GetGenericArguments();
            if (types.Length > 0)
            {
                return types[0];
            }
            return type.HasElementType ? type.GetElementType() : null;
        }

        public static T Cast<T>(object value)
        {
            try
            {
                dynamic o = value;
                return value != null ? (T)o : default(T);
            }
            catch
            {
                return default(T);
            }
        }

        private static MethodInfo GetConvertMethod(Type genericType)
        {
            var method = typeof(ConvertExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name == "As" && x.GetParameters().Length == 1);
            return method != null ? method.MakeGenericMethod(genericType) : null;
        }
    }
}