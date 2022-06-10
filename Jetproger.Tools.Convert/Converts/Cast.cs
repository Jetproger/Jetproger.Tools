using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Jetproger.Tools.Convert.Bases;

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
            if (value is string) return Je.str.To<T>(value.ToString());
            if (typeTo == typeof(string)) return (T)(object)Je.str.Of(value);
            if (typeTo == typeof(bool)) return (T)(object)AsBool(value);
            if ((typeTo == typeof(DateTime))
            && (value is decimal || value is float || value is double || value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte)) return (T)(object)DateTime.FromOADate(As<double>(value));
            if ((typeTo == typeof(decimal) || typeTo == typeof(float) || typeTo == typeof(double) || typeTo == typeof(long) || typeTo == typeof(ulong) || typeTo == typeof(int) || typeTo == typeof(uint) || typeTo == typeof(short) || typeTo == typeof(ushort) || typeTo == typeof(byte) || typeTo == typeof(sbyte))
            && (value is DateTime)) return Cast<T>(((DateTime)value).ToOADate());
            if (value is Icon && typeTo == typeof(Image)) return (T)(object)Je.gui.IcnToImg((Icon)value);
            if (value is Image && typeTo == typeof(Icon)) return (T)(object)Je.gui.ImgToIcn((Image)value);
            if (value is Icon && typeTo == typeof(byte[])) return (T)(object)Je.bin.Of((Icon)value);
            if (value is byte[] && typeTo == typeof(Icon)) return Je.bin.To<T>((byte[])value);
            if (value is Image && typeTo == typeof(byte[])) return (T)(object)Je.bin.Of((Image)value);
            if (value is byte[] && typeTo == typeof(Image)) return Je.bin.To<T>((byte[])value);
            if (value is byte[]) return Je.bin.To<T>((byte[])value);
            if ((typeTo.IsEnum
            || typeTo == typeof(decimal) || typeTo == typeof(float) || typeTo == typeof(double) || typeTo == typeof(long) || typeTo == typeof(ulong) || typeTo == typeof(int) || typeTo == typeof(uint) || typeTo == typeof(short) || typeTo == typeof(ushort) || typeTo == typeof(byte) || typeTo == typeof(sbyte))
            && (value is decimal || value is float || value is double || value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte)) return Cast<T>(value);
            return (T)value;
        }

        public static object As(this object value, Type typeTo)
        {
            var nullType = Nullable.GetUnderlyingType(typeTo);
            if (nullType != null) return As(value, nullType);
            var method = Je<MethodInfo>.Key(typeTo, GetConvertMethod);
            return method != null ? method.Invoke(null, new[] { value }) : null;
        }

        private static bool AsBool(object value)
        {
            if (value == null || value == DBNull.Value) return false;
            if (!Je.sys.IsSimple(value.GetType())) return true;
            if (value is string)
            {
                var s = value.ToString().ToLower();
                return !string.IsNullOrWhiteSpace(s) && (s == "yes" || s == "да" || s == "1" || s == "true");
            }
            var longValue = value.As<long>();
            return longValue != 0;
        }

        private static MethodInfo GetConvertMethod(Type genericType)
        {
            var method = typeof(ConvertExtensions)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(x => x.Name == "As" && x.GetParameters().Length == 1);
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