using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Je
    {
        public static class str_<TConverter> where TConverter : StringConverter
        {
            public static string Of(object value)
            {
                return Je<TConverter>.One(() => Je<TConverter>.New()).Serialize(value);
            }

            public static T To<T>(string s)
            {
                return (T)Je<TConverter>.One(() => Je<TConverter>.New()).Deserialize(s, typeof(T));
            }

            public static object To(string s, Type type)
            {
                return Je<TConverter>.One(() => Je<TConverter>.New()).Deserialize(s, type);
            }
        }
    }
}

namespace Jetproger.Tools.Convert.Converts
{
    public static class StrExtensions
    {                                                                             
        public static string Of(this Je.IStrExpander e, object value)
        {
            return Je<StringConverter>.One(() => Je<StringConverter>.New()).Serialize(value); 
        }

        public static T To<T>(this Je.IStrExpander e, string s)
        {
            return (T)Je<StringConverter>.One(() => Je<StringConverter>.New()).Deserialize(s, typeof(T));
        }

        public static object To(this Je.IStrExpander e, string s, Type type)
        {
            return Je<StringConverter>.One(() => Je<StringConverter>.New()).Deserialize(s, type);
        }

        public static string Repeat(this Je.IStrExpander e, string s, int count)
        {
            if (s == null) return null;
            if (s == string.Empty || count <= 0) return s;
            if (count == 1) return s;
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++) sb.Append(s);
            return sb.ToString();
        }

        public static string Reverse(this Je.IStrExpander e, string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;
            var sb = new StringBuilder();
            for (int i = s.Length - 1; i >= 0; i--) sb.Append(s[i]);
            return sb.ToString();
        }

        public static string Left(this Je.IStrExpander e, string s, int length)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (length < 1) return string.Empty;
            if (length > s.Length) return s;
            return s.Substring(0, length);
        }

        public static string Right(this Je.IStrExpander e, string s, int length)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (length < 1) return string.Empty;
            if (length > s.Length) return s;
            return s.Substring(s.Length - length);
        }

        public static string Replace(this Je.IStrExpander e, string s, string substringOld, string substringNew)
        {
            if (substringOld == substringNew) return s;
            if (substringOld == null) return s;
            if (s == null) return null;
            if (substringOld.Length > s.Length) return s;
            substringNew = substringNew ?? "";
            return s.Replace(substringOld, substringNew);
        }

        public static string NewLength(this Je.IStrExpander e, string s, int len)
        {
            s = s ?? string.Empty;
            var oldLen = s.Length;
            var newLen = len > 0 ? len : oldLen;
            if (newLen == oldLen) return s;
            var sb = new StringBuilder();
            for (int i = 0; i < newLen; i++) sb.Append(i < s.Length ? s[i] : ' ');
            return sb.ToString();
        }
    }

    public class StringConverter
    {
        private CultureInfo Culture => _culture ?? (_culture = GetFormatter());
        private readonly BinConverter _bin = new BinConverter();
        public static readonly Type StringType = typeof(string);
        private CultureInfo _culture;

        public virtual string Serialize(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            if (value is string) return (string)value;
            if (value is bool) return (bool)value ? "1" : "0";
            if (value is byte[]) return System.Convert.ToBase64String((byte[])value, Base64FormattingOptions.None);
            if (value is char[]) return string.Concat((char[])value);
            if (value is Icon) return System.Convert.ToBase64String(_bin.Of((Icon)value));
            if (value is Image) return System.Convert.ToBase64String(_bin.Of((Image)value));
            if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fff", Culture);
            if (value is Guid || value is char || value is StringBuilder || value.GetType().IsEnum) return value.ToString();
            if (value is decimal || value is float || value is double) return ConvertExtensions.Cast<double>(value).ToString("#################0.00", Culture);
            if (value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte) return ConvertExtensions.Cast<long>(value).ToString("#################0", Culture);
            return Je.sys.MakeTypeName(value);
        }

        public virtual object Deserialize(string s, Type type)
        {
            if (type == null) return null;
            if (string.IsNullOrWhiteSpace(s)) return type.IsValueType ? Activator.CreateInstance(type) : null;
            if (type.IsEnum) return ToEnum(s, type);
            if (type == typeof(string)) return s;
            if (type == typeof(Icon)) return _bin.To<Icon>(System.Convert.FromBase64String(s));
            if (type == typeof(Image)) return _bin.To<Image>(System.Convert.FromBase64String(s));
            if (type == typeof(byte[])) return System.Convert.FromBase64String(s);
            if (type == typeof(char[])) return s.ToCharArray();
            if (type == typeof(StringBuilder)) return new StringBuilder(s);
            if (type == typeof(bool) || type == typeof(bool?)) return s == "1" || s == "true" || s == "yes" || s == "да";
            if (type == typeof(byte) || type == typeof(byte?)) return ToByte(s);
            if (type == typeof(sbyte) || type == typeof(sbyte?)) return ToSbyte(s);
            if (type == typeof(char) || type == typeof(char?)) return ToChar(s);
            if (type == typeof(short) || type == typeof(short?)) return ToShort(s);
            if (type == typeof(ushort) || type == typeof(ushort?)) return ToUshort(s);
            if (type == typeof(int) || type == typeof(int?)) return ToInt(s);
            if (type == typeof(uint) || type == typeof(uint?)) return ToUint(s);
            if (type == typeof(long) || type == typeof(long?)) return ToLong(s);
            if (type == typeof(ulong) || type == typeof(ulong?)) return ToUlong(s);
            if (type == typeof(float) || type == typeof(float?)) return ToFloat(s);
            if (type == typeof(decimal) || type == typeof(decimal?)) return ToDecimal(s);
            if (type == typeof(double) || type == typeof(double?)) return ToDouble(s);
            if (type == typeof(DateTime) || type == typeof(DateTime?)) return ToDateTime(s);
            if (type == typeof(Guid) || type == typeof(Guid?)) return ToGuid(s);
            return Je.sys.CreateInstance(s);
        }

        protected virtual CultureInfo GetFormatter()
        {
            return new CultureInfo("en-us")
            {
                NumberFormat = { NumberGroupSeparator = string.Empty, NumberDecimalSeparator = "." },
                DateTimeFormat = { DateSeparator = "-", TimeSeparator = ":" }
            };
        }

        private byte ToByte(string s)
        {
            byte x; return byte.TryParse(s, NumberStyles.Any, Culture, out x) ? x : default(byte);
        }

        private sbyte ToSbyte(string s)
        {
            sbyte x; return sbyte.TryParse(s, NumberStyles.Any, Culture, out x) ? x : default(sbyte);
        }

        private char ToChar(string s)
        {
            char x; return char.TryParse(s, out x) ? x : default(char);
        }

        private short ToShort(string s)
        {
            short x; return short.TryParse(s, NumberStyles.Any, Culture, out x) ? x : default(short);
        }

        private ushort ToUshort(string s)
        {
            ushort x; return ushort.TryParse(s, NumberStyles.Any, Culture, out x) ? x : default(ushort);
        }

        private int ToInt(string s)
        {
            int x; return int.TryParse(s, NumberStyles.Any, Culture, out x) ? x : default(int);
        }

        private uint ToUint(string s)
        {
            uint x; return uint.TryParse(s, NumberStyles.Any, Culture, out x) ? x : default(uint);
        }

        private long ToLong(string s)
        {
            long x; return long.TryParse(s, NumberStyles.Any, Culture, out x) ? x : default(long);
        }

        private ulong ToUlong(string s)
        {
            ulong x; return ulong.TryParse(s, NumberStyles.Any, Culture, out x) ? x : default(ulong);
        }

        private float ToFloat(string s)
        {
            float x; return float.TryParse(s, NumberStyles.Any, Culture, out x) ? x : default(float);
        }

        private decimal ToDecimal(string s)
        {
            decimal x; return decimal.TryParse(s, NumberStyles.Any, Culture, out x) ? x : default(decimal);
        }

        private double ToDouble(string s)
        {
            double x; return double.TryParse(s, NumberStyles.Any, Culture, out x) ? x : default(double);
        }

        private DateTime ToDateTime(string s)
        {
            DateTime x; return DateTime.TryParse(s, Culture, DateTimeStyles.None, out x) ? x : default(DateTime);
        }

        private Guid ToGuid(string s)
        {
            Guid x; return Guid.TryParse(s, out x) ? x : default(Guid);
        }

        private object ToEnum(string s, Type enumType)
        {
            try
            {
                return Enum.Parse(enumType, s, true);
            }
            catch
            {
                return Activator.CreateInstance(enumType);
            }
        }
    }
}