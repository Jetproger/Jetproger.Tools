using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Datasets;

namespace Jetproger.Tools.Convert.Converts
{
    public static class ConvertExtensions
    {
        public static T As<T>(this object value)
        {
            if (value == null || value == DBNull.Value) return default(T);
            var typeFr = value.GetType();
            var typeTo = typeof(T);
            if (typeFr == typeTo) return (T)value;
            if (Je.sys.IsTypeOf(typeFr, typeTo)) return (T)value;
            if (value is string) return Je.str.To<T>((string)value);
            if (typeTo == typeof(string)) return (T)(object)Je.str.Of(value);
            if (value is Jc.DataScope) return (new DataDeserializer<T>((Jc.DataScope)value)).Item;
            if (typeTo == typeof (Jc.DataScope)) return (T)(object)(new DataSerializer(value)).DataScope;
            if (value is Icon && typeTo == typeof(Image)) return (T)(object)Je.gui.GetIconAsImage((Icon)value);
            if (value is Image && typeTo == typeof(Icon)) return (T)(object)Je.gui.GetImageAsIcon((Image)value);
            if (value is Icon && typeTo == typeof(byte[])) return (T)(object)Je.bin.Of((Icon)value);
            if (value is byte[] && typeTo == typeof(Icon)) return Je.bin.To<T>((byte[])value);
            if (value is Image && typeTo == typeof(byte[])) return (T)(object)Je.bin.Of((Image)value);
            if (value is byte[] && typeTo == typeof(Image)) return Je.bin.To<T>((byte[])value);
            if (value is byte[]) return Je.bin.To<T>((byte[])value);
            if (typeTo == typeof(byte[])) return (T)(object)Je.bin.Of(value);
            if ((typeTo == typeof(decimal) || typeTo == typeof(float) || typeTo == typeof(double) || typeTo == typeof(long) || typeTo == typeof(ulong) || typeTo == typeof(int) || typeTo == typeof(uint) || typeTo == typeof(short) || typeTo == typeof(ushort) || typeTo == typeof(byte) || typeTo == typeof(sbyte)) && (value is decimal || value is float || value is double || value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte)) return Cast<T>(value);
            return (T)value;
        }

        public static object As(this object value, Type typeTo)
        {
            var method = Je<MethodInfo>.Get(typeTo, x => GetConvertMethod((Type)x));
            return method != null ? method.Invoke(null, new[] { value }) : Je.sys.DefaultOf(typeTo);
        }

        private static MethodInfo GetConvertMethod(Type genericType)
        {
            var method = typeof(ConvertExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name == "As" && x.GetParameters().Length == 1);
            return method != null ? method.MakeGenericMethod(genericType) : null;
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
    }
}