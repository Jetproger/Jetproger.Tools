using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Je
    {
        public static ISzExpander Sz => null;
        public interface ISzExpander { }
    }
}

namespace Jetproger.Tools.Convert.Converts
{
    public static class SzExtensions
    {
        private static readonly CultureInfo Culture = new CultureInfo("en-us") { NumberFormat = { NumberGroupSeparator = string.Empty, NumberDecimalSeparator = "." }, DateTimeFormat = { DateSeparator = "-", TimeSeparator = ":" } };
        private static readonly BaseBin Binary = new BaseBin();

        public static string Of(this Je.ISzExpander e, object value)
        {
            if (value == null || value == DBNull.Value) return null;
            if (value is string) return (string)value;
            if (value is bool) return (bool)value ? "1" : "0";
            if (value is byte[]) return System.Convert.ToBase64String((byte[])value, Base64FormattingOptions.None);
            if (value is char[]) return string.Concat((char[])value);
            if (value is Icon) return System.Convert.ToBase64String(Binary.Of((Icon)value));
            if (value is Image) return System.Convert.ToBase64String(Binary.Of((Image)value));
            if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fff", Culture);
            if (value is Guid || value is char || value is StringBuilder || value.GetType().IsEnum) return value.ToString();
            if (value is decimal || value is float || value is double) return ConvertExtensions.Cast<double>(value).ToString("#################0.00", Culture);
            if (value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte) return ConvertExtensions.Cast<long>(value).ToString("#################0", Culture);
            return System.Convert.ToBase64String(Binary.Of(value));
        }

        public static T To<T>(this Je.ISzExpander e, string sz)
        {
            return (T)To(e, sz, typeof(T));
        }

        public static object To(this Je.ISzExpander e, string sz, Type type)
        {
            if (type == null) return null;
            if (string.IsNullOrWhiteSpace(sz)) return type.IsValueType ? Activator.CreateInstance(type) : null;
            if (type.IsEnum) return ToEnum(sz, type);
            if (type == typeof(string)) return sz;
            if (type == typeof(Icon)) return Binary.To<Icon>(System.Convert.FromBase64String(sz));
            if (type == typeof(Image)) return Binary.To<Image>(System.Convert.FromBase64String(sz));
            if (type == typeof(byte[])) return System.Convert.FromBase64String(sz);
            if (type == typeof(char[])) return sz.ToCharArray();
            if (type == typeof(StringBuilder)) return new StringBuilder(sz);
            if (type == typeof(bool) || type == typeof(bool?)) return sz == "1" || sz == "true" || sz == "yes" || sz == "да";
            if (type == typeof(byte) || type == typeof(byte?)) return ToByte(sz);
            if (type == typeof(sbyte) || type == typeof(sbyte?)) return ToSbyte(sz);
            if (type == typeof(char) || type == typeof(char?)) return ToChar(sz);
            if (type == typeof(short) || type == typeof(short?)) return ToShort(sz);
            if (type == typeof(ushort) || type == typeof(ushort?)) return ToUshort(sz);
            if (type == typeof(int) || type == typeof(int?)) return ToInt(sz);
            if (type == typeof(uint) || type == typeof(uint?)) return ToUint(sz);
            if (type == typeof(long) || type == typeof(long?)) return ToLong(sz);
            if (type == typeof(ulong) || type == typeof(ulong?)) return ToUlong(sz);
            if (type == typeof(float) || type == typeof(float?)) return ToFloat(sz);
            if (type == typeof(decimal) || type == typeof(decimal?)) return ToDecimal(sz);
            if (type == typeof(double) || type == typeof(double?)) return ToDouble(sz);
            if (type == typeof(DateTime) || type == typeof(DateTime?)) return ToDateTime(sz);
            if (type == typeof(Guid) || type == typeof(Guid?)) return ToGuid(sz);
            return Binary.To(System.Convert.FromBase64String(sz), type);
        }

        private static byte ToByte(string sz)
        {
            byte x; return byte.TryParse(sz, NumberStyles.Any, Culture, out x) ? x : default(byte);
        }

        private static sbyte ToSbyte(string sz)
        {
            sbyte x; return sbyte.TryParse(sz, NumberStyles.Any, Culture, out x) ? x : default(sbyte);
        }

        private static char ToChar(string sz)
        {
            char x; return char.TryParse(sz, out x) ? x : default(char);
        }

        private static short ToShort(string sz)
        {
            short x; return short.TryParse(sz, NumberStyles.Any, Culture, out x) ? x : default(short);
        }

        private static ushort ToUshort(string sz)
        {
            ushort x; return ushort.TryParse(sz, NumberStyles.Any, Culture, out x) ? x : default(ushort);
        }

        private static int ToInt(string sz)
        {
            int x; return int.TryParse(sz, NumberStyles.Any, Culture, out x) ? x : default(int);
        }

        private static uint ToUint(string sz)
        {
            uint x; return uint.TryParse(sz, NumberStyles.Any, Culture, out x) ? x : default(uint);
        }

        private static long ToLong(string sz)
        {
            long x; return long.TryParse(sz, NumberStyles.Any, Culture, out x) ? x : default(long);
        }

        private static ulong ToUlong(string sz)
        {
            ulong x; return ulong.TryParse(sz, NumberStyles.Any, Culture, out x) ? x : default(ulong);
        }

        private static float ToFloat(string sz)
        {
            float x; return float.TryParse(sz, NumberStyles.Any, Culture, out x) ? x : default(float);
        }

        private static decimal ToDecimal(string sz)
        {
            decimal x; return decimal.TryParse(sz, NumberStyles.Any, Culture, out x) ? x : default(decimal);
        }

        private static double ToDouble(string sz)
        {
            double x; return double.TryParse(sz, NumberStyles.Any, Culture, out x) ? x : default(double);
        }

        private static DateTime ToDateTime(string sz)
        {
            DateTime x; return DateTime.TryParse(sz, Culture, DateTimeStyles.None, out x) ? x : default(DateTime);
        }

        private static Guid ToGuid(string sz)
        {
            Guid x; return Guid.TryParse(sz, out x) ? x : default(Guid);
        }

        private static object ToEnum(string sz, Type enumType)
        {
            try
            {
                return Enum.Parse(enumType, sz, true);
            }
            catch
            {
                return Activator.CreateInstance(enumType);
            }
        }
    }
}