using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using Jetproger.Tools.Convert.Converts;

namespace Jc
{
    public static class Txt<T> where T : BaseTxt, new()
    {
        public static string Of(object value)
        {
            return One<T>.Ge.Of(value);
        }

        public static TOut To<TOut>(string txt)
        {
            return One<T>.Ge.To<TOut>(txt);
        }

        public static object To(string txt, Type type)
        {
            return One<T>.Ge.To(txt, type);
        }
    }

    public class BaseTxt
    {

        private static readonly CultureInfo Culture = new CultureInfo("en-us") { NumberFormat = { NumberGroupSeparator = string.Empty, NumberDecimalSeparator = "." }, DateTimeFormat = { DateSeparator = "-", TimeSeparator = ":" } };

        private static readonly BaseBin Binary = new BaseBin();

        protected virtual CultureInfo Formatter => Culture;

        protected virtual BaseBin BinConvertor => Binary;

        public virtual string Of(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            if (value is string) return (string)value;
            if (value is bool) return (bool)value ? "1" : "0";
            if (value is byte[]) return System.Convert.ToBase64String((byte[])value, Base64FormattingOptions.None);
            if (value is char[]) return string.Concat((char[])value);
            if (value is Icon) return System.Convert.ToBase64String(BinConvertor.Of((Icon)value));
            if (value is Image) return System.Convert.ToBase64String(BinConvertor.Of((Image)value));
            if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fff", Formatter);
            if (value is Guid || value is char || value is StringBuilder || value.GetType().IsEnum) return value.ToString();
            if (value is decimal || value is float || value is double) return ConvertExtensions.Cast<double>(value).ToString("#################0.00", Formatter);
            if (value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte) return ConvertExtensions.Cast<long>(value).ToString("#################0", Formatter);
            return System.Convert.ToBase64String(BinConvertor.Of(value));
        }

        public T To<T>(string txt)
        {
            return (T)To(txt, typeof(T));
        }

        public virtual object To(string txt, Type type)
        {
            if (type == null) return null;
            if (string.IsNullOrWhiteSpace(txt)) return type.IsValueType ? Activator.CreateInstance(type) : null;
            if (type.IsEnum) return ToEnum(txt, type);
            if (type == typeof(string)) return txt;
            if (type == typeof(Icon)) return BinConvertor.To<Icon>(System.Convert.FromBase64String(txt));
            if (type == typeof(Image)) return BinConvertor.To<Image>(System.Convert.FromBase64String(txt));
            if (type == typeof(byte[])) return System.Convert.FromBase64String(txt);
            if (type == typeof(char[])) return txt.ToCharArray();
            if (type == typeof(StringBuilder)) return new StringBuilder(txt);
            if (type == typeof(bool) || type == typeof(bool?)) return txt == "1" || txt == "true" || txt == "yes" || txt == "да";
            if (type == typeof(byte) || type == typeof(byte?)) return ToByte(txt);
            if (type == typeof(sbyte) || type == typeof(sbyte?)) return ToSbyte(txt);
            if (type == typeof(char) || type == typeof(char?)) return ToChar(txt);
            if (type == typeof(short) || type == typeof(short?)) return ToShort(txt);
            if (type == typeof(ushort) || type == typeof(ushort?)) return ToUshort(txt);
            if (type == typeof(int) || type == typeof(int?)) return ToInt(txt);
            if (type == typeof(uint) || type == typeof(uint?)) return ToUint(txt);
            if (type == typeof(long) || type == typeof(long?)) return ToLong(txt);
            if (type == typeof(ulong) || type == typeof(ulong?)) return ToUlong(txt);
            if (type == typeof(float) || type == typeof(float?)) return ToFloat(txt);
            if (type == typeof(decimal) || type == typeof(decimal?)) return ToDecimal(txt);
            if (type == typeof(double) || type == typeof(double?)) return ToDouble(txt);
            if (type == typeof(DateTime) || type == typeof(DateTime?)) return ToDateTime(txt);
            if (type == typeof(Guid) || type == typeof(Guid?)) return ToGuid(txt);
            return BinConvertor.To(System.Convert.FromBase64String(txt), type);
        }

        private byte ToByte(string txt)
        {
            byte x; return byte.TryParse(txt, NumberStyles.Any, Formatter, out x) ? x : default(byte);
        }

        private sbyte ToSbyte(string txt)
        {
            sbyte x; return sbyte.TryParse(txt, NumberStyles.Any, Formatter, out x) ? x : default(sbyte);
        }

        private char ToChar(string txt)
        {
            char x; return char.TryParse(txt, out x) ? x : default(char);
        }

        private short ToShort(string txt)
        {
            short x; return short.TryParse(txt, NumberStyles.Any, Formatter, out x) ? x : default(short);
        }

        private ushort ToUshort(string txt)
        {
            ushort x; return ushort.TryParse(txt, NumberStyles.Any, Formatter, out x) ? x : default(ushort);
        }

        private int ToInt(string txt)
        {
            int x; return int.TryParse(txt, NumberStyles.Any, Formatter, out x) ? x : default(int);
        }

        private uint ToUint(string txt)
        {
            uint x; return uint.TryParse(txt, NumberStyles.Any, Formatter, out x) ? x : default(uint);
        }

        private long ToLong(string txt)
        {
            long x; return long.TryParse(txt, NumberStyles.Any, Formatter, out x) ? x : default(long);
        }

        private ulong ToUlong(string txt)
        {
            ulong x; return ulong.TryParse(txt, NumberStyles.Any, Formatter, out x) ? x : default(ulong);
        }

        private float ToFloat(string txt)
        {
            float x; return float.TryParse(txt, NumberStyles.Any, Formatter, out x) ? x : default(float);
        }

        private decimal ToDecimal(string txt)
        {
            decimal x; return decimal.TryParse(txt, NumberStyles.Any, Formatter, out x) ? x : default(decimal);
        }

        private double ToDouble(string txt)
        {
            double x; return double.TryParse(txt, NumberStyles.Any, Formatter, out x) ? x : default(double);
        }

        private DateTime ToDateTime(string txt)
        {
            DateTime x; return DateTime.TryParse(txt, Formatter, DateTimeStyles.None, out x) ? x : default(DateTime);
        }

        private Guid ToGuid(string txt)
        {
            Guid x; return Guid.TryParse(txt, out x) ? x : default(Guid);
        }

        private static object ToEnum(string txt, Type enumType)
        {
            try
            {
                return Enum.Parse(enumType, txt, true);
            }
            catch
            {
                return Activator.CreateInstance(enumType);
            }
        }
    }
}