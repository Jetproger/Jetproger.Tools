using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Converts
{
    public static class StrExtensions
    { 
        public static string Script(this f.IStrExpander e, string fileName, string args)
        {
            var sb = new StringBuilder();
            var psi = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.GetEncoding(866)
            };
            psi.FileName = fileName;
            psi.Arguments = args;
            var process = Process.Start(psi);
            while (process != null && !process.StandardOutput.EndOfStream)
            {
                sb.Append(process.StandardOutput.ReadLine());
            }
            if (process != null)
            {
                process.WaitForExit();
                process.Close();
            }
            var result = sb.ToString();
            return result;
        }

        public static string Repeat(this f.IStrExpander e, string s, int count)
        {
            if (s == null) return null;
            if (s == string.Empty || count <= 0) return s;
            if (count == 1) return s;
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++) sb.Append(s);
            return sb.ToString();
        }

        public static string Reverse(this f.IStrExpander e, string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;
            var sb = new StringBuilder();
            for (int i = s.Length - 1; i >= 0; i--) sb.Append(s[i]);
            return sb.ToString();
        }

        public static string Left(this f.IStrExpander e, string s, int length)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (length < 1) return string.Empty;
            if (length > s.Length) return s;
            return s.Substring(0, length);
        }

        public static string Right(this f.IStrExpander e, string s, int length)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            if (length < 1) return string.Empty;
            if (length > s.Length) return s;
            return s.Substring(s.Length - length);
        }

        public static string Replace(this f.IStrExpander e, string s, string substringOld, string substringNew)
        {
            if (substringOld == substringNew) return s;
            if (substringOld == null) return s;
            if (s == null) return null;
            if (substringOld.Length > s.Length) return s;
            substringNew = substringNew ?? "";
            return s.Replace(substringOld, substringNew);
        }

        public static string NewLen(this f.IStrExpander exp, string s, int len)
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

    public class StringConverter : Converter
    {
        private static readonly IConverter Bin = new BinaryConverter();
        protected override object BytesAsValue(byte[] bytes, Type typeTo) { return CharsAsValue(BytesAsChars(bytes), typeTo); } 
        protected override byte[] ValueAsBytes(object value) { return CharsAsBytes(ValueAsChars(value)); } 
        public StringConverter(Encoding encoder = null) { Encoder = encoder ?? base.Encoder; }
        protected override Encoding Encoder { get; }

        protected override string ValueAsChars(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            if (value is string s) return s;
            if (value is bool b) return b ? "1" : "0"; 
            if (value is byte[] bytes) return BytesAsChars(bytes);
            if (value is char[] chars) return new string(chars);
            if (value is DateTime time) return time.ToString("yyyy-MM-ddTHH:mm:ss.fff", Culture);
            if (value is Guid || value is char || value is StringBuilder || value.GetType().IsEnum) return value.ToString();
            if (value is decimal || value is float || value is double) return value.As<double>().ToString("#################0.00", Culture);
            if (value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte) return value.As<long>().ToString("#################0", Culture);
            Bin.Is(value);
            return System.Convert.ToBase64String((byte[])Bin.As(typeof(byte[])));
        }

        protected override object CharsAsValue(string s, Type typeTo)
        {
            if (string.IsNullOrWhiteSpace(s)) return f.sys.defaultof(typeTo);
            if (typeTo.IsEnum) return ToEnum(s, typeTo);
            if (typeTo == typeof(string)) return s;
            if (typeTo == typeof(byte[])) return CharsAsBytes(s);
            if (typeTo == typeof(char[])) return s.ToCharArray();
            if (typeTo == typeof(StringBuilder)) return new StringBuilder(s);
            if (typeTo == typeof(bool) || typeTo == typeof(bool?)) return (s == "1" || s == "true" || s == "yes" || s == @"??");
            if (typeTo == typeof(byte) || typeTo == typeof(byte?)) return ToByte(s);
            if (typeTo == typeof(sbyte) || typeTo == typeof(sbyte?)) return ToSbyte(s);
            if (typeTo == typeof(char) || typeTo == typeof(char?)) return ToChar(s);
            if (typeTo == typeof(short) || typeTo == typeof(short?)) return ToShort(s);
            if (typeTo == typeof(ushort) || typeTo == typeof(ushort?)) return ToUshort(s);
            if (typeTo == typeof(int) || typeTo == typeof(int?)) return ToInt(s);
            if (typeTo == typeof(uint) || typeTo == typeof(uint?)) return ToUint(s);
            if (typeTo == typeof(long) || typeTo == typeof(long?)) return ToLong(s);
            if (typeTo == typeof(ulong) || typeTo == typeof(ulong?)) return ToUlong(s);
            if (typeTo == typeof(float) || typeTo == typeof(float?)) return ToFloat(s);
            if (typeTo == typeof(decimal) || typeTo == typeof(decimal?)) return ToDecimal(s);
            if (typeTo == typeof(double) || typeTo == typeof(double?)) return ToDouble(s);
            if (typeTo == typeof(DateTime) || typeTo == typeof(DateTime?)) return ToDateTime(s);
            if (typeTo == typeof(Guid) || typeTo == typeof(Guid?)) return ToGuid(s);
            Bin.Is(System.Convert.FromBase64String(s));
            return Bin.As(typeTo);
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

        private byte ToByte(string s) { return byte.TryParse(s, NumberStyles.Any, Culture, out var x) ? x : default(byte); }
        private sbyte ToSbyte(string s) { return sbyte.TryParse(s, NumberStyles.Any, Culture, out var x) ? x : default(sbyte); }
        private char ToChar(string s) { return char.TryParse(s, out var x) ? x : default(char); }
        private short ToShort(string s) { return short.TryParse(s, NumberStyles.Any, Culture, out var x) ? x : default(short); }
        private ushort ToUshort(string s) { return ushort.TryParse(s, NumberStyles.Any, Culture, out var x) ? x : default(ushort); }
        private int ToInt(string s) { return int.TryParse(s, NumberStyles.Any, Culture, out var x) ? x : default(int); }
        private uint ToUint(string s) { return uint.TryParse(s, NumberStyles.Any, Culture, out var x) ? x : default(uint); }
        private long ToLong(string s) { return long.TryParse(s, NumberStyles.Any, Culture, out var x) ? x : default(long); }
        private ulong ToUlong(string s) { return ulong.TryParse(s, NumberStyles.Any, Culture, out var x) ? x : default(ulong); }
        private float ToFloat(string s) { return float.TryParse(s, NumberStyles.Any, Culture, out var x) ? x : default(float); }
        private decimal ToDecimal(string s) { return decimal.TryParse(s, NumberStyles.Any, Culture, out var x) ? x : default(decimal); }
        private double ToDouble(string s) { return double.TryParse(s, NumberStyles.Any, Culture, out var x) ? x : default(double); }
        private DateTime ToDateTime(string s) { return DateTime.TryParse(s, Culture, DateTimeStyles.None, out var x) ? x : default(DateTime); }
        private Guid ToGuid(string s) { return Guid.TryParse(s, out var x) ? x : default(Guid); }
    }
}