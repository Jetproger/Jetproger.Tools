using System;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace Jetproger.Tools.Xml.Bases
{
    public static partial class XmlExtensions
    {
        private static object Default(this Type type)
        {
            if (!_defaults.ContainsKey(type))
            {
                if (!type.IsGenericType && !type.IsEnum) return null;
                if (type.IsAbstract || type.IsInterface) return null;
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && (genericType == typeof(Nullable<>) || genericType == typeof(Func<>) || genericType == typeof(Action<>))) return null;
                return Activator.CreateInstance(type);
            }
            return _defaults[type];
        }

        private static T As<T>(this object value)
        {
            var resultType = typeof(T);
            if (value == null) return (T)resultType.Default();
            var valueType = value.GetType();
            if (Supports.IsTypeOf(valueType, resultType)) return (T)value;
            if (valueType == typeof(string) && resultType == typeof(Icon)) return (T)(object)Graphics.AsIcon((string)value);
            if (valueType == typeof(byte[]) && resultType == typeof(Icon)) return (T)(object)Graphics.AsIcon((byte[])value);
            if (valueType == typeof(string) && resultType == typeof(Image)) return (T)(object)Graphics.AsImage((string)value);
            if (valueType == typeof(byte[]) && resultType == typeof(Image)) return (T)(object)Graphics.AsImage((byte[])value);
            if (_convertors.ContainsKey(resultType)) return (T)_convertors[resultType](value);
            if (resultType.IsAbstract || resultType.IsInterface) return (T)(object)null;
            if (valueType == typeof(string)) return (T)Supports.DeserializeJson((string)value, resultType);
            return (T)Supports.DeserializeJson(Supports.SerializeJson(value), resultType);
        }

        private static object As(this object value, Type resultType)
        {
            if (_convertors.ContainsKey(resultType)) return _convertors[resultType](value);
            if (value == null) return null;
            var valueType = value.GetType();
            if (Supports.IsTypeOf(valueType, resultType)) return value;
            if (valueType == typeof(string) && resultType == typeof(Icon)) return Graphics.AsIcon((string)value);
            if (valueType == typeof(byte[]) && resultType == typeof(Icon)) return Graphics.AsIcon((byte[])value);
            if (valueType == typeof(string) && resultType == typeof(Image)) return Graphics.AsImage((string)value);
            if (valueType == typeof(byte[]) && resultType == typeof(Image)) return Graphics.AsImage((byte[])value);
            if (_convertors.ContainsKey(resultType)) return _convertors[resultType](value);
            if (resultType.IsAbstract || resultType.IsInterface) return null;
            if (valueType == typeof(string)) return Supports.DeserializeJson((string)value, resultType);
            return Supports.DeserializeJson(Supports.SerializeJson(value), resultType);
        }

        private static bool? AsBoolNull(this object value)
        {
            return value != null ? AsBool(value) : (bool?)null;
        }

        private static byte? AsByteNull(this object value)
        {
            return value != null ? AsByte(value) : (byte?)null;
        }

        private static sbyte? AsSbyteNull(this object value)
        {
            return value != null ? AsSbyte(value) : (sbyte?)null;
        }

        private static char? AsCharNull(this object value)
        {
            return value != null ? AsChar(value) : (char?)null;
        }

        private static short? AsShortNull(this object value)
        {
            return value != null ? AsShort(value) : (short?)null;
        }

        private static ushort? AsUshortNull(this object value)
        {
            return value != null ? AsUshort(value) : (ushort?)null;
        }

        private static int? AsIntNull(this object value)
        {
            return value != null ? AsInt(value) : (int?)null;
        }

        private static uint? AsUintNull(this object value)
        {
            return value != null ? AsUint(value) : (uint?)null;
        }

        private static long? AsLongNull(this object value)
        {
            return value != null ? AsLong(value) : (long?)null;
        }

        private static ulong? AsUlongNull(this object value)
        {
            return value != null ? AsUlong(value) : (ulong?)null;
        }

        private static Guid? AsGuidNull(this object value)
        {
            return value != null ? AsGuid(value) : (Guid?)null;
        }

        private static float? AsFloatNull(this object value)
        {
            return value != null ? AsFloat(value) : (float?)null;
        }

        private static decimal? AsDecimalNull(this object value)
        {
            return value != null ? AsDecimal(value) : (decimal?)null;
        }

        private static double? AsDoubleNull(this object value)
        {
            return value != null ? AsDouble(value) : (double?)null;
        }

        private static DateTime? AsDateTimeNull(this object value)
        {
            return value != null ? AsDateTime(value) : (DateTime?)null;
        }

        private static byte AsByte(this object value)
        {
            byte result;
            if (value == null || value == DBNull.Value) return default(byte);
            if (value is byte) return (byte)value;
            if (value is string) return byte.TryParse((string)value, NumberStyles.Any, Formatter, out result) ? result : default(byte);
            if (value is bool) return (bool)value ? (byte)1 : (byte)0;
            if (value is DateTime) return (byte)((DateTime)value).ToOADate();
            if (value is Guid) return (byte)BitConverter.ToUInt16(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return (byte)BitConverter.ToUInt16((byte[])value, 0);
            if (value is char[]) return byte.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(byte);
            if (value.GetType().IsClass) return (byte)value.GetHashCode();
            return Cast<byte>(value);
        }

        private static sbyte AsSbyte(this object value)
        {
            sbyte result;
            if (value == null || value == DBNull.Value) return default(sbyte);
            if (value is sbyte) return (sbyte)value;
            if (value is string) return sbyte.TryParse((string)value, NumberStyles.Any, Formatter, out result) ? result : default(sbyte);
            if (value is bool) return (bool)value ? (sbyte)1 : (sbyte)0;
            if (value is DateTime) return (sbyte)((DateTime)value).ToOADate();
            if (value is Guid) return (sbyte)BitConverter.ToInt16(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return (sbyte)BitConverter.ToInt16((byte[])value, 0);
            if (value is char[]) return sbyte.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(sbyte);
            if (value.GetType().IsClass) return (sbyte)value.GetHashCode();
            return Cast<sbyte>(value);
        }

        private static char AsChar(this object value)
        {
            char result;
            if (value == null || value == DBNull.Value) return default(char);
            if (value is char) return (char)value;
            if (value is string) return char.TryParse((string)value, out result) ? result : default(char);
            if (value is bool) return (bool)value ? '1' : '0';
            if (value is DateTime) return (char)((DateTime)value).ToOADate();
            if (value is Guid) return (char)BitConverter.ToUInt16(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return (char)BitConverter.ToUInt16((byte[])value, 0);
            if (value is char[]) return Supports.SetLen((char[])value, 1)[0];
            if (value.GetType().IsClass) return (char)value.GetHashCode();
            return Cast<char>(value);
        }

        private static short AsShort(this object value)
        {
            short result;
            if (value == null || value == DBNull.Value) return default(short);
            if (value is short) return (short)value;
            if (value is string) return short.TryParse((string)value, NumberStyles.Any, Formatter, out result) ? result : default(short);
            if (value is bool) return (bool)value ? (short)1 : (short)0;
            if (value is DateTime) return (short)((DateTime)value).ToOADate();
            if (value is Guid) return BitConverter.ToInt16(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return BitConverter.ToInt16((byte[])value, 0);
            if (value is char[]) return short.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(short);
            if (value.GetType().IsClass) return (short)value.GetHashCode();
            return Cast<short>(value);
        }

        private static ushort AsUshort(this object value)
        {
            ushort result;
            if (value == null || value == DBNull.Value) return default(ushort);
            if (value is ushort) return (ushort)value;
            if (value is string) return ushort.TryParse((string)value, NumberStyles.Any, Formatter, out result) ? result : default(ushort);
            if (value is bool) return (bool)value ? (ushort)1 : (ushort)0;
            if (value is DateTime) return (ushort)((DateTime)value).ToOADate();
            if (value is Guid) return BitConverter.ToUInt16(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return BitConverter.ToUInt16((byte[])value, 0);
            if (value is char[]) return ushort.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(ushort);
            if (value.GetType().IsClass) return (ushort)value.GetHashCode();
            return Cast<ushort>(value);
        }

        private static int AsInt(this object value)
        {
            int result;
            if (value == null || value == DBNull.Value) return default(int);
            if (value is int) return (int)value;
            if (value is string) return int.TryParse((string)value, NumberStyles.Any, Formatter, out result) ? result : default(int);
            if (value is bool) return (bool)value ? 1 : 0;
            if (value is DateTime) return (int)((DateTime)value).ToOADate();
            if (value is Guid) return BitConverter.ToInt32(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return BitConverter.ToInt32((byte[])value, 0);
            if (value is char[]) return int.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(int);
            if (value.GetType().IsClass) return value.GetHashCode();
            return Cast<int>(value);
        }

        private static uint AsUint(this object value)
        {
            uint result;
            if (value == null || value == DBNull.Value) return default(uint);
            if (value is uint) return (uint)value;
            if (value is string) return uint.TryParse((string)value, NumberStyles.Any, Formatter, out result) ? result : default(uint);
            if (value is bool) return (bool)value ? (uint)1 : 0;
            if (value is DateTime) return (uint)((DateTime)value).ToOADate();
            if (value is Guid) return BitConverter.ToUInt32(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return BitConverter.ToUInt32((byte[])value, 0);
            if (value is char[]) return uint.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(uint);
            if (value.GetType().IsClass) return (uint)value.GetHashCode();
            return Cast<uint>(value);
        }

        private static long AsLong(this object value)
        {
            long result;
            if (value == null || value == DBNull.Value) return default(long);
            if (value is long) return (long)value;
            if (value is string) return long.TryParse((string)value, NumberStyles.Any, Formatter, out result) ? result : default(long);
            if (value is bool) return (bool)value ? 1 : 0;
            if (value is DateTime) return (int)((DateTime)value).ToOADate();
            if (value is Guid) return BitConverter.ToInt64(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return BitConverter.ToInt64((byte[])value, 0);
            if (value is char[]) return long.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(long);
            if (value.GetType().IsClass) return value.GetHashCode();
            return Cast<long>(value);
        }

        private static ulong AsUlong(this object value)
        {
            ulong result;
            if (value == null || value == DBNull.Value) return default(ulong);
            if (value is ulong) return (ulong)value;
            if (value is string) return ulong.TryParse((string)value, NumberStyles.Any, Formatter, out result) ? result : default(ulong);
            if (value is bool) return (bool)value ? (ulong)1 : 0;
            if (value is DateTime) return (ulong)((DateTime)value).ToOADate();
            if (value is Guid) return BitConverter.ToUInt64(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return BitConverter.ToUInt64((byte[])value, 0);
            if (value is char[]) return ulong.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(ulong);
            if (value.GetType().IsClass) return (ulong)value.GetHashCode();
            return Cast<ulong>(value);
        }

        private static float AsFloat(this object value)
        {
            float result;
            if (value == null || value == DBNull.Value) return default(float);
            if (value is float) return (float)value;
            if (value is string) return float.TryParse((string)value, NumberStyles.Any, Formatter, out result) ? result : default(float);
            if (value is bool) return (bool)value ? 1 : 0;
            if (value is DateTime) return (float)((DateTime)value).ToOADate();
            if (value is Guid) return BitConverter.ToSingle(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return BitConverter.ToSingle((byte[])value, 0);
            if (value is char[]) return float.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(float);
            if (value.GetType().IsClass) return value.GetHashCode();
            return Cast<float>(value);
        }

        private static decimal AsDecimal(this object value)
        {
            decimal result;
            if (value == null || value == DBNull.Value) return default(decimal);
            if (value is decimal) return (decimal)value;
            if (value is string) return decimal.TryParse((string)value, NumberStyles.Any, Formatter, out result) ? result : default(decimal);
            if (value is bool) return (bool)value ? 1 : 0;
            if (value is DateTime) return (decimal)((DateTime)value).ToOADate();
            if (value is Guid) return (decimal)BitConverter.ToDouble(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return (decimal)BitConverter.ToDouble((byte[])value, 0);
            if (value is char[]) return decimal.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(decimal);
            if (value.GetType().IsClass) return value.GetHashCode();
            return Cast<decimal>(value);
        }

        private static double AsDouble(this object value)
        {
            double result;
            if (value == null || value == DBNull.Value) return default(double);
            if (value is double) return (double)value;
            if (value is string) return double.TryParse((string)value, NumberStyles.Any, Formatter, out result) ? result : default(double);
            if (value is bool) return (bool)value ? 1 : 0;
            if (value is DateTime) return ((DateTime)value).ToOADate();
            if (value is Guid) return BitConverter.ToDouble(((Guid)value).ToByteArray(), 0);
            if (value is byte[]) return BitConverter.ToDouble((byte[])value, 0);
            if (value is char[]) return double.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(double);
            if (value.GetType().IsClass) return value.GetHashCode();
            return Cast<double>(value);
        }

        private static bool AsBool(this object value)
        {
            if (value == null || value == DBNull.Value) return default(bool);
            if (value is bool) return (bool)value;
            if (value is string) return value.AsLong() != default(long);
            if (value is long) return (long)value != default(long);
            if (value is decimal) return (decimal)value != default(decimal);
            if (value is int) return (int)value != default(int);
            if (value is short) return (short)value != default(short);
            if (value is byte) return (byte)value != default(byte);
            if (value is ulong) return (ulong)value != default(ulong);
            if (value is uint) return (uint)value != default(uint);
            if (value is ushort) return (ushort)value != default(ushort);
            if (value is sbyte) return (sbyte)value != default(sbyte);
            if (value is float) return (float)value != default(float);
            if (value is double) return (double)value != default(double);
            if (value is Guid) return (Guid)value != default(Guid);
            if (value is DateTime) return (DateTime)value != default(DateTime);
            if (value is char) return (char)value != default(char);
            if (value is char[]) return ((char[])value).Length > 0;
            if (value is byte[]) return ((byte[])value).Length > 0;
            return default(bool);
        }

        private static byte[] AsBytes(this object value)
        {
            if (value == null || value == DBNull.Value) return default(byte[]);
            if (value is byte[]) return (byte[])value;
            if (value is string) return Supports.GetStringAsBytes((string)value);
            if (value is char[]) return Supports.GetStringAsBytes(value.AsString());
            if (value is Guid) return ((Guid)value).ToByteArray();
            if (value is DateTime) return BitConverter.GetBytes(((DateTime)value).ToOADate());
            if (value is bool) return BitConverter.GetBytes((bool)value);
            if (value is byte) return BitConverter.GetBytes((ushort)(byte)value);
            if (value is sbyte) return BitConverter.GetBytes((short)(sbyte)value);
            if (value is char) return BitConverter.GetBytes((char)value);
            if (value is short) return BitConverter.GetBytes((short)value);
            if (value is ushort) return BitConverter.GetBytes((ushort)value);
            if (value is int) return BitConverter.GetBytes((int)value);
            if (value is uint) return BitConverter.GetBytes((uint)value);
            if (value is long) return BitConverter.GetBytes((long)value);
            if (value is ulong) return BitConverter.GetBytes((ulong)value);
            if (value is float) return BitConverter.GetBytes((float)value);
            if (value is decimal) return BitConverter.GetBytes((double)value);
            if (value is double) return BitConverter.GetBytes((double)value);
            if (value is Icon) return Graphics.AsBytes((Icon)value);
            if (value is Image) return Graphics.AsBytes((Image)value);
            return default(byte[]);
        }

        private static DateTime AsDateTimeMin(this object value)
        {
            var date = AsDateTime(value);
            return date >= MinDate && date <= MaxDate ? date : MinDate;
        }

        private static DateTime AsDateTimeMax(this object value)
        {
            var date = AsDateTime(value);
            return date >= MinDate && date <= MaxDate ? date : MaxDate;
        }

        private static DateTime AsDateTime(this object value)
        {
            DateTime result;
            if (value == null || value == DBNull.Value) return default(DateTime);
            if (value is DateTime) return (DateTime)value;
            if (value is string) return DateTime.TryParse((string)value, Formatter, DateTimeStyles.None, out result) ? result : default(DateTime);
            if (value is char[]) return DateTime.TryParse(value.AsString(), Formatter, DateTimeStyles.None, out result) ? result : default(DateTime);
            if (value is byte[]) return DateTime.FromOADate(BitConverter.ToDouble((byte[])value, 0));
            if (value is Guid) return DateTime.FromOADate(BitConverter.ToDouble(((Guid)value).ToByteArray(), 0));
            if (value is bool) return DateTime.FromOADate((bool)value ? 1 : 0);
            if (value is byte) return DateTime.FromOADate((byte)value);
            if (value is sbyte) return DateTime.FromOADate((sbyte)value);
            if (value is char) return DateTime.FromOADate((char)value);
            if (value is short) return DateTime.FromOADate((short)value);
            if (value is ushort) return DateTime.FromOADate((ushort)value);
            if (value is int) return DateTime.FromOADate((int)value);
            if (value is uint) return DateTime.FromOADate((uint)value);
            if (value is long) return DateTime.FromOADate((long)value);
            if (value is ulong) return DateTime.FromOADate((ulong)value);
            if (value is float) return DateTime.FromOADate((float)value);
            if (value is decimal) return DateTime.FromOADate((double)(decimal)value);
            if (value is double) return DateTime.FromOADate((double)value);
            return default(DateTime);
        }

        private static Guid AsGuid(this object value)
        {
            Guid result;
            if (value == null || value == DBNull.Value) return default(Guid);
            if (value is Guid) return (Guid)value;
            if (value is string) return Guid.TryParse((string)value, out result) ? result : default(Guid);
            if (value is char[]) return Guid.TryParse(value.AsString(), out result) ? result : default(Guid);
            if (value is byte[]) return new Guid(Supports.SetLen((byte[])value, 16));
            if (value is DateTime) return new Guid(Supports.SetLen(((DateTime)value).ToOADate().AsBytes(), 16));
            if (value is bool) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is byte) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is sbyte) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is char) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is short) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is ushort) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is int) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is uint) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is long) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is ulong) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is float) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is decimal) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            if (value is double) return new Guid(Supports.SetLen(value.AsBytes(), 16));
            return default(Guid);
        }

        private static string AsString(this object value)
        {
            if (value == null || value == DBNull.Value) return default(string);
            if (value is string) return (string)value;
            if (value is bool) return (bool)value ? "1" : "0";
            if (value is long) return ((long)value).ToString("#################0", Formatter);
            if (value is decimal) return ((decimal)value).ToString("#################0.00", Formatter);
            if (value is int) return ((int)value).ToString("#################0", Formatter);
            if (value is short) return ((short)value).ToString("#################0", Formatter);
            if (value is byte) return ((byte)value).ToString("#################0", Formatter);
            if (value is ulong) return ((ulong)value).ToString("#################0", Formatter);
            if (value is uint) return ((uint)value).ToString("#################0", Formatter);
            if (value is ushort) return ((ushort)value).ToString("#################0", Formatter);
            if (value is sbyte) return ((sbyte)value).ToString("#################0", Formatter);
            if (value is float) return ((float)value).ToString("#################0.00", Formatter);
            if (value is double) return ((double)value).ToString("#################0.00", Formatter);
            if (value is Guid) return ((Guid)value).ToString();
            if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fff", Formatter);
            if (value is char) return ((char)value).ToString(CultureInfo.InvariantCulture);
            if (value is char[]) return string.Concat((char[])value);
            if (value is byte[]) return Supports.GetBytesAsString((byte[])value);
            if (value is Icon) return Graphics.AsString((Icon)value);
            if (value is Image) return Graphics.AsString((Image)value);
            if (value is Exception) return GetExceptionAsString((Exception)value);
            return Supports.SerializeJson(value);
        }

        private static string GetExceptionAsString(Exception e)
        {
            var sb = new StringBuilder();
            sb.AppendLine(e.ToString());
            while (e != null)
            {
                sb.AppendLine(e.Message);
                e = e.InnerException;
            }
            return sb.ToString();
        }

        private static char[] AsChars(this object value)
        {
            var stringValue = value.AsString();
            return stringValue != default(string) ? stringValue.ToCharArray() : default(char[]);
        }

        private static string AsSql(this object value)
        {
            if (value == null || value == DBNull.Value) return "NULL";
            var stringValue = value.AsString();
            if (value is string || value is DateTime || value is char || value is char[]) stringValue = $"'{stringValue.Replace("'", "''").Replace("%", "%%")}'";
            else
            if (value is decimal || value is float || value is double) stringValue = stringValue.Replace(",", ".");
            return stringValue;
        }

        private static T Cast<T>(this object value)
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