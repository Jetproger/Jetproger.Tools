using System;
using System.Collections;
using System.Collections.Generic;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class ConvertExtensions
    {
        public static bool AsBool(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsBool() : default(bool);
        }

        public static bool AsBool(this IDictionary<string, object> values, string key, bool defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsBool() : defaultValue;
        }

        public static bool? AsBoolNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsBoolNull() : default(bool?);
        }

        public static bool? AsBoolNull(this IDictionary<string, object> values, string key, bool? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsBoolNull() : defaultValue;
        }

        public static char AsChar(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsChar() : default(char);
        }

        public static char AsChar(this IDictionary<string, object> values, string key, char defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsChar() : defaultValue;
        }

        public static char? AsCharNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsCharNull() : default(char?);
        }

        public static char? AsCharNull(this IDictionary<string, object> values, string key, char? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsCharNull() : defaultValue;
        }

        public static byte AsByte(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsByte() : default(byte);
        }

        public static byte AsByte(this IDictionary<string, object> values, string key, byte defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsByte() : defaultValue;
        }

        public static byte? AsByteNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsByteNull() : default(byte?);
        }

        public static byte? AsByteNull(this IDictionary<string, object> values, string key, byte? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsByteNull() : defaultValue;
        }

        public static sbyte AsSbyte(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsSbyte() : default(sbyte);
        }

        public static sbyte AsSbyte(this IDictionary<string, object> values, string key, sbyte defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsSbyte() : defaultValue;
        }

        public static sbyte? AsSbyteNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsSbyteNull() : default(sbyte?);
        }

        public static sbyte? AsSbyteNull(this IDictionary<string, object> values, string key, sbyte? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsSbyteNull() : defaultValue;
        }

        public static short AsShort(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsShort() : default(short);
        }

        public static short AsShort(this IDictionary<string, object> values, string key, short defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsShort() : defaultValue;
        }

        public static short? AsShortNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsShortNull() : default(short?);
        }

        public static short? AsShortNull(this IDictionary<string, object> values, string key, short? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsShortNull() : defaultValue;
        }

        public static ushort AsUshort(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUshort() : default(ushort);
        }

        public static ushort AsUshort(this IDictionary<string, object> values, string key, ushort defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUshort() : defaultValue;
        }

        public static ushort? AsUshortNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUshortNull() : default(ushort?);
        }

        public static ushort? AsUshortNull(this IDictionary<string, object> values, string key, ushort? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUshortNull() : defaultValue;
        }

        public static int AsInt(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsInt() : default(int);
        }

        public static int AsInt(this IDictionary<string, object> values, string key, int defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsInt() : defaultValue;
        }

        public static int? AsIntNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsIntNull() : default(int?);
        }

        public static int? AsIntNull(this IDictionary<string, object> values, string key, int? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsIntNull() : defaultValue;
        }

        public static uint AsUint(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUshort() : default(uint);
        }

        public static uint AsUint(this IDictionary<string, object> values, string key, uint defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUshort() : defaultValue;
        }

        public static uint? AsUintNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUshortNull() : default(uint?);
        }

        public static uint? AsUintNull(this IDictionary<string, object> values, string key, uint? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUshortNull() : defaultValue;
        }

        public static T AsEnum<T>(this IDictionary<string, object> values, string key)
        {
            if (values == null || !values.ContainsKey(key)) return default(T);
            dynamic value = values[key].AsInt();
            return (T)value;
        }

        public static T AsEnum<T>(this IDictionary<string, object> values, string key, T defaultValue)
        {
            if (values == null || !values.ContainsKey(key)) return defaultValue;
            dynamic value = values[key].AsInt();
            return (T)value;
        }

        public static T? AsEnumNull<T>(this IDictionary<string, object> values, string key) where T : struct
        {
            if (values == null || !values.ContainsKey(key)) return default(T?);
            dynamic value = values[key].AsInt();
            return (T?)value;
        }

        public static T? AsEnumNull<T>(this IDictionary<string, object> values, string key, T? defaultValue) where T : struct
        {
            if (values == null || !values.ContainsKey(key)) return defaultValue;
            dynamic value = values[key].AsInt();
            return (T?)value;
        }

        public static long AsLong(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsLong() : default(long);
        }

        public static long AsLong(this IDictionary<string, object> values, string key, long defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsLong() : defaultValue;
        }

        public static long? AsLongNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsLongNull() : default(long?);
        }

        public static long? AsLongNull(this IDictionary<string, object> values, string key, long? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsLongNull() : defaultValue;
        }

        public static ulong AsUlong(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUlong() : default(ulong);
        }

        public static ulong AsUlong(this IDictionary<string, object> values, string key, ulong defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUlong() : defaultValue;
        }

        public static ulong? AsUlongNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUlongNull() : default(ulong?);
        }

        public static ulong? AsUlongNull(this IDictionary<string, object> values, string key, ulong? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsUlongNull() : defaultValue;
        }

        public static Guid AsGuid(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsGuid() : default(Guid);
        }

        public static Guid AsGuid(this IDictionary<string, object> values, string key, Guid defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsGuid() : defaultValue;
        }

        public static Guid? AsGuidNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsGuidNull() : default(Guid?);
        }

        public static Guid? AsGuidNull(this IDictionary<string, object> values, string key, Guid? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsGuidNull() : defaultValue;
        }

        public static float AsFloat(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsFloat() : default(float);
        }

        public static float AsFloat(this IDictionary<string, object> values, string key, float defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsFloat() : defaultValue;
        }

        public static float? AsFloatNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsFloatNull() : default(float?);
        }

        public static float? AsFloatNull(this IDictionary<string, object> values, string key, float? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsFloatNull() : defaultValue;
        }

        public static double AsDouble(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDouble() : default(double);
        }

        public static double AsDouble(this IDictionary<string, object> values, string key, double defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDouble() : defaultValue;
        }

        public static double? AsDoubleNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDoubleNull() : default(double?);
        }

        public static double? AsDoubleNull(this IDictionary<string, object> values, string key, double? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDoubleNull() : defaultValue;
        }

        public static decimal AsDecimal(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDecimal() : default(decimal);
        }

        public static decimal AsDecimal(this IDictionary<string, object> values, string key, decimal defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDecimal() : defaultValue;
        }

        public static decimal? AsDecimalNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDecimalNull() : default(decimal?);
        }

        public static decimal? AsDecimalNull(this IDictionary<string, object> values, string key, decimal? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDecimalNull() : defaultValue;
        }

        public static DateTime AsDateTime(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDateTime() : default(DateTime);
        }

        public static DateTime AsDateTime(this IDictionary<string, object> values, string key, DateTime defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDateTime() : defaultValue;
        }

        public static DateTime? AsDateTimeNull(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDateTimeNull() : default(DateTime?);
        }

        public static DateTime? AsDateTimeNull(this IDictionary<string, object> values, string key, DateTime? defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsDateTimeNull() : defaultValue;
        }

        public static char[] AsChars(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsChars() : default(char[]);
        }

        public static char[] AsChars(this IDictionary<string, object> values, string key, char[] defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsChars() : defaultValue;
        }

        public static byte[] AsBytes(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsBytes() : default(byte[]);
        }

        public static byte[] AsBytes(this IDictionary<string, object> values, string key, byte[] defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsBytes() : defaultValue;
        }

        public static string AsString(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsString() : default(string);
        }

        public static string AsString(this IDictionary<string, object> values, string key, string defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key].AsString() : defaultValue;
        }

        public static IList AsList(this IDictionary<string, object> values, string key)
        {
            return values != null && values.ContainsKey(key) ? values[key] as IList : default(IList);
        }

        public static IList AsList(this IDictionary<string, object> values, string key, IList defaultValue)
        {
            return values != null && values.ContainsKey(key) ? values[key] as IList : defaultValue;
        }

        public static T As<T>(this IDictionary<string, object> values, string key) where T : class
        {
            return values != null && values.ContainsKey(key) ? values[key] as T : default(T);
        }

        public static T As<T>(this IDictionary<string, object> values, string key, T defaultValue) where T : class
        {
            return values != null && values.ContainsKey(key) ? values[key] as T : defaultValue;
        }
    }
}