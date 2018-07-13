using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace Jetproger.Tools.Convert.Bases
{
    public static class StringExtensions
    {
        private static readonly CultureInfo Formatter = new CultureInfo("en-us")
        {
            NumberFormat =
            {
                NumberGroupSeparator = string.Empty, NumberDecimalSeparator = "."
            },
            DateTimeFormat =
            {
                DateSeparator = "-", TimeSeparator = ":"
            }
        };

        public static string Write(this IStringExpander expander, object value)
        {
            if (value == null || value == DBNull.Value) return null;
            if (value is string) return (string)value;
            if (value is bool) return (bool)value ? "1" : "0";
            if (value is byte[]) return WriteBytes((byte[])value);
            if (value is char[]) return string.Concat((char[])value);
            if (value is Icon) return WriteIcon((Icon)value);
            if (value is Image) return WriteImage((Image)value);
            if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fff", Formatter);
            if (value is Guid || value is char || value is StringBuilder || value.GetType().IsEnum) return value.ToString();
            if (value is decimal || value is float || value is double) return ConvertExtensions.Cast<double>(value).ToString("#################0.00", Formatter);
            if (value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte) return ConvertExtensions.Cast<long>(value).ToString("#################0", Formatter);
            return Ex.Json.Write(value);
        }

        public static T Read<T>(this IStringExpander expander, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return default(T);
            var type = typeof(T);
            if (type.IsEnum) return (T)(ReadEnum(value, type));
            if (type == typeof(string)) return (T)(object)value;
            if (type == typeof(Icon)) return (T)(object)(ReadIcon(value));
            if (type == typeof(Image)) return (T)(object)(ReadImage(value));
            if (type == typeof(byte[])) return (T)(object)(ReadBytes(value));
            if (type == typeof(char[])) return (T)(object)(value.ToCharArray());
            if (type == typeof(StringBuilder)) return (T)(object)(new StringBuilder(value));
            if (type == typeof(bool) || type == typeof(bool?)) return (T)(object)(value == "1" || value == "true" || value == "yes" || value == "да");
            byte @byte;
            if (type == typeof(byte) || type == typeof(byte?)) return (T)(object)(byte.TryParse(value, NumberStyles.Any, Formatter, out @byte) ? @byte : default(byte));
            sbyte @sbyte;
            if (type == typeof(sbyte) || type == typeof(sbyte?)) return (T)(object)(sbyte.TryParse(value, NumberStyles.Any, Formatter, out @sbyte) ? @sbyte : default(sbyte));
            char @char;
            if (type == typeof(char) || type == typeof(char?)) return (T)(object)(char.TryParse(value, out @char) ? @char : default(char));
            short @short;
            if (type == typeof(short) || type == typeof(short?)) return (T)(object)(short.TryParse(value, NumberStyles.Any, Formatter, out @short) ? @short : default(short));
            ushort @ushort;
            if (type == typeof(ushort) || type == typeof(ushort?)) return (T)(object)(ushort.TryParse(value, NumberStyles.Any, Formatter, out @ushort) ? @ushort : default(ushort));
            int @int;
            if (type == typeof(int) || type == typeof(int?)) return (T)(object)(int.TryParse(value, NumberStyles.Any, Formatter, out @int) ? @int : default(int));
            uint @uint;
            if (type == typeof(uint) || type == typeof(uint?)) return (T)(object)(uint.TryParse(value, NumberStyles.Any, Formatter, out @uint) ? @uint : default(uint));
            long @long;
            if (type == typeof(long) || type == typeof(long?)) return (T)(object)(long.TryParse(value, NumberStyles.Any, Formatter, out @long) ? @long : default(long));
            ulong @ulong;
            if (type == typeof(ulong) || type == typeof(ulong?)) return (T)(object)(ulong.TryParse(value, NumberStyles.Any, Formatter, out @ulong) ? @ulong : default(ulong));
            float @float;
            if (type == typeof(float) || type == typeof(float?)) return (T)(object)(float.TryParse(value, NumberStyles.Any, Formatter, out @float) ? @float : default(float));
            decimal @decimal;
            if (type == typeof(decimal) || type == typeof(decimal?)) return (T)(object)(decimal.TryParse(value, NumberStyles.Any, Formatter, out @decimal) ? @decimal : default(decimal));
            double @double;
            if (type == typeof(double) || type == typeof(double?)) return (T)(object)(double.TryParse(value, NumberStyles.Any, Formatter, out @double) ? @double : default(double));
            DateTime @datetime;
            if (type == typeof(DateTime) || type == typeof(DateTime?)) return (T)(object)(DateTime.TryParse(value, Formatter, DateTimeStyles.None, out @datetime) ? @datetime : default(DateTime));
            Guid @guid;
            if (type == typeof(Guid) || type == typeof(Guid?)) return (T)(object)(Guid.TryParse(value, out @guid) ? @guid : default(Guid));
            return Ex.Json.Read<T>(value);
        }

        private static Image ReadImage(string base64)
        {
            return Ex.Image.Read(System.Convert.FromBase64String(base64));
        }

        private static string WriteImage(Image image)
        {
            return System.Convert.ToBase64String(Ex.Image.Write(image));
        }

        private static Icon ReadIcon(string base64)
        {
            return Ex.Image.ReadIcon(System.Convert.FromBase64String(base64));
        }

        private static string WriteIcon(Icon icon)
        {
            return System.Convert.ToBase64String(Ex.Image.Write(icon));
        }

        private static object ReadEnum(string value, Type enumType)
        {
            try
            {
                return Enum.Parse(enumType, value, true);
            }
            catch
            {
                return enumType.Default();
            }
        }

        private static string WriteBytes(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder("0x");
            foreach (byte b in bytes)
            {
                string s = System.Convert.ToString(b, 16);
                sb.AppendFormat("{0}{1}", s.Length < 2 ? "0" : "", s);
            }
            return sb.ToString();
        }

        private static byte[] ReadBytes(string s)
        {
            if (s == null) return null;
            s = s.Trim(' ', '\r', '\n', '\t');
            if (s == "" || s == "0x") return new byte[0];
            if (s.Length % 2 != 0) return null;
            if (!s.StartsWith("0x")) s = $"0x{s}";
            try
            {
                var bytes = new List<byte>();
                var max = s.Length / 2;
                for (int i = 1; i < max; i++)
                {
                    var k = 2 * i;
                    var valueString = $"{s[k]}{s[k + 1]}";
                    var valueByte = System.Convert.ToByte(valueString, 16);
                    bytes.Add(valueByte);
                }
                return bytes.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static long ReadHex(string value) { return System.Convert.ToInt64(value, 16); }
        public static string WriteHex(byte value) { return System.Convert.ToString(value, 16); }
        public static string WriteHex(sbyte value) { return System.Convert.ToString(value, 16); }
        public static string WriteHex(char value) { return System.Convert.ToString(value, 16); }
        public static string WriteHex(short value) { return System.Convert.ToString(value, 16); }
        public static string WriteHex(int value) { return System.Convert.ToString(value, 16); }
        public static string WriteHex(long value) { return System.Convert.ToString(value, 16); }

        public static long ReadOct(string value) { return System.Convert.ToInt64(value, 8); }
        public static string WriteOct(byte value) { return System.Convert.ToString(value, 8); }
        public static string WriteOct(sbyte value) { return System.Convert.ToString(value, 8); }
        public static string WriteOct(char value) { return System.Convert.ToString(value, 8); }
        public static string WriteOct(short value) { return System.Convert.ToString(value, 8); }
        public static string WriteOct(int value) { return System.Convert.ToString(value, 8); }
        public static string WriteOct(long value) { return System.Convert.ToString(value, 8); }

        public static long ReadBin(string value) { return System.Convert.ToInt64(value, 2); }
        public static string WriteBin(byte value) { return System.Convert.ToString(value, 2); }
        public static string WriteBin(sbyte value) { return System.Convert.ToString(value, 2); }
        public static string WriteBin(char value) { return System.Convert.ToString(value, 2); }
        public static string WriteBin(short value) { return System.Convert.ToString(value, 2); }
        public static string WriteBin(int value) { return System.Convert.ToString(value, 2); }
        public static string WriteBin(long value) { return System.Convert.ToString(value, 2); }
    }
}