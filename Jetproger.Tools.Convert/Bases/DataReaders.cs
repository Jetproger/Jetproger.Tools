using System;
using System.Data;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class ConvertExtensions
    {
        public static bool AsBool(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsBool(), default(bool));
        }

        public static bool AsBool(this IDataReader reader, string columnName, bool defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsBool(), defaultValue);
        }

        public static bool? AsBoolNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsBoolNull(), default(bool?));
        }

        public static bool? AsBoolNull(this IDataReader reader, string columnName, bool? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsBoolNull(), defaultValue);
        }

        public static char AsChar(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsChar(), default(char));
        }

        public static char AsChar(this IDataReader reader, string columnName, char defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsChar(), defaultValue);
        }

        public static char? AsCharNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsCharNull(), default(char?));
        }

        public static char? AsCharNull(this IDataReader reader, string columnName, char? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsCharNull(), defaultValue);
        }

        public static byte AsByte(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsByte(), default(byte));
        }

        public static byte AsByte(this IDataReader reader, string columnName, byte defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsByte(), defaultValue);
        }

        public static byte? AsByteNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsByteNull(), default(byte?));
        }

        public static byte? AsByteNull(this IDataReader reader, string columnName, byte? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsByteNull(), defaultValue);
        }

        public static sbyte AsSbyte(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsSbyte(), default(sbyte));
        }

        public static sbyte AsSbyte(this IDataReader reader, string columnName, sbyte defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsSbyte(), defaultValue);
        }

        public static sbyte? AsSbyteNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsSbyteNull(), default(sbyte?));
        }

        public static sbyte? AsSbyteNull(this IDataReader reader, string columnName, sbyte? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsSbyteNull(), defaultValue);
        }

        public static short AsShort(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsShort(), default(short));
        }

        public static short AsShort(this IDataReader reader, string columnName, short defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsShort(), defaultValue);
        }

        public static short? AsShortNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsShortNull(), default(short?));
        }

        public static short? AsShortNull(this IDataReader reader, string columnName, short? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsShortNull(), defaultValue);
        }

        public static ushort AsUshort(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUshort(), default(ushort));
        }

        public static ushort AsUshort(this IDataReader reader, string columnName, ushort defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUshort(), defaultValue);
        }

        public static ushort? AsUshortNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUshortNull(), default(ushort?));
        }

        public static ushort? AsUshortNull(this IDataReader reader, string columnName, ushort? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUshortNull(), defaultValue);
        }

        public static int AsInt(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsInt(), default(int));
        }

        public static int AsInt(this IDataReader reader, string columnName, int defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsInt(), defaultValue);
        }

        public static int? AsIntNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsIntNull(), default(int?));
        }

        public static int? AsIntNull(this IDataReader reader, string columnName, int? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsIntNull(), defaultValue);
        }

        public static uint AsUint(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUshort(), default(uint));
        }

        public static uint AsUint(this IDataReader reader, string columnName, uint defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUshort(), defaultValue);
        }

        public static uint? AsUintNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUshortNull(), default(uint?));
        }

        public static uint? AsUintNull(this IDataReader reader, string columnName, uint? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUshortNull(), defaultValue);
        }

        public static T AsEnum<T>(this IDataReader reader, string columnName)
        {
            try
            {
                dynamic value = reader.GetValue(reader.GetOrdinal(columnName)).AsInt();
                return (T)value;
            }
            catch
            {
                return default(T);
            }
        }

        public static T AsEnum<T>(this IDataReader reader, string columnName, T defaultValue)
        {
            try
            {
                dynamic value = reader.GetValue(reader.GetOrdinal(columnName)).AsInt();
                return (T)value;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T? AsEnumNull<T>(this IDataReader reader, string columnName) where T : struct
        {
            try
            {
                dynamic value = reader.GetValue(reader.GetOrdinal(columnName)).AsInt();
                return (T?)value;
            }
            catch
            {
                return default(T?);
            }
        }

        public static T? AsEnumNull<T>(this IDataReader reader, string columnName, T? defaultValue) where T : struct
        {
            try
            {
                dynamic value = reader.GetValue(reader.GetOrdinal(columnName)).AsInt();
                return (T?)value;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long AsLong(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsLong(), default(long));
        }

        public static long AsLong(this IDataReader reader, string columnName, long defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsLong(), defaultValue);
        }

        public static long? AsLongNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsLongNull(), default(long?));
        }

        public static long? AsLongNull(this IDataReader reader, string columnName, long? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsLongNull(), defaultValue);
        }

        public static ulong AsUlong(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUlong(), default(ulong));
        }

        public static ulong AsUlong(this IDataReader reader, string columnName, ulong defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUlong(), defaultValue);
        }

        public static ulong? AsUlongNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUlongNull(), default(ulong?));
        }

        public static ulong? AsUlongNull(this IDataReader reader, string columnName, ulong? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsUlongNull(), defaultValue);
        }

        public static Guid AsGuid(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsGuid(), default(Guid));
        }

        public static Guid AsGuid(this IDataReader reader, string columnName, Guid defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsGuid(), defaultValue);
        }

        public static Guid? AsGuidNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsGuidNull(), default(Guid?));
        }

        public static Guid? AsGuidNull(this IDataReader reader, string columnName, Guid? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsGuidNull(), defaultValue);
        }

        public static float AsFloat(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsFloat(), default(float));
        }

        public static float AsFloat(this IDataReader reader, string columnName, float defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsFloat(), defaultValue);
        }

        public static float? AsFloatNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsFloatNull(), default(float?));
        }

        public static float? AsFloatNull(this IDataReader reader, string columnName, float? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsFloatNull(), defaultValue);
        }

        public static double AsDouble(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDouble(), default(double));
        }

        public static double AsDouble(this IDataReader reader, string columnName, double defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDouble(), defaultValue);
        }

        public static double? AsDoubleNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDoubleNull(), default(double?));
        }

        public static double? AsDoubleNull(this IDataReader reader, string columnName, double? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDoubleNull(), defaultValue);
        }

        public static decimal AsDecimal(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDecimal(), default(decimal));
        }

        public static decimal AsDecimal(this IDataReader reader, string columnName, decimal defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDecimal(), defaultValue);
        }

        public static decimal? AsDecimalNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDecimalNull(), default(decimal?));
        }

        public static decimal? AsDecimalNull(this IDataReader reader, string columnName, decimal? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDecimalNull(), defaultValue);
        }

        public static DateTime AsDateTime(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDateTime(), default(DateTime));
        }

        public static DateTime AsDateTime(this IDataReader reader, string columnName, DateTime defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDateTime(), defaultValue);
        }

        public static DateTime? AsDateTimeNull(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDateTimeNull(), default(DateTime?));
        }

        public static DateTime? AsDateTimeNull(this IDataReader reader, string columnName, DateTime? defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsDateTimeNull(), defaultValue);
        }

        public static char[] AsChars(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsChars(), default(char[]));
        }

        public static char[] AsChars(this IDataReader reader, string columnName, char[] defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsChars(), defaultValue);
        }

        public static byte[] AsBytes(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsBytes(), default(byte[]));
        }

        public static byte[] AsBytes(this IDataReader reader, string columnName, byte[] defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsBytes(), defaultValue);
        }

        public static string AsString(this IDataReader reader, string columnName)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsString(), default(string));
        }

        public static string AsString(this IDataReader reader, string columnName, string defaultValue)
        {
            return Try(() => reader.GetValue(reader.GetOrdinal(columnName)).AsString(), defaultValue);
        }

        public static bool AsBool(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsBool() , default(bool));
        }

        public static bool AsBool(this IDataReader reader, int columnIndex, bool defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsBool(), defaultValue);
        }

        public static bool? AsBoolNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsBoolNull(), default(bool?));
        }

        public static bool? AsBoolNull(this IDataReader reader, int columnIndex, bool? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsBoolNull(), defaultValue);
        }

        public static char AsChar(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsChar(), default(char));
        }

        public static char AsChar(this IDataReader reader, int columnIndex, char defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsChar(), defaultValue);
        }

        public static char? AsCharNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsCharNull(), default(char?));
        }

        public static char? AsCharNull(this IDataReader reader, int columnIndex, char? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsCharNull(), defaultValue);
        }

        public static byte AsByte(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsByte(), default(byte));
        }

        public static byte AsByte(this IDataReader reader, int columnIndex, byte defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsByte(), defaultValue);
        }

        public static byte? AsByteNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsByteNull(), default(byte?));
        }

        public static byte? AsByteNull(this IDataReader reader, int columnIndex, byte? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsByteNull(), defaultValue);
        }

        public static sbyte AsSbyte(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsSbyte(), default(sbyte));
        }

        public static sbyte AsSbyte(this IDataReader reader, int columnIndex, sbyte defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsSbyte(), defaultValue);
        }

        public static sbyte? AsSbyteNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsSbyteNull(), default(sbyte?));
        }

        public static sbyte? AsSbyteNull(this IDataReader reader, int columnIndex, sbyte? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsSbyteNull(), defaultValue);
        }

        public static short AsShort(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsShort(), default(short));
        }

        public static short AsShort(this IDataReader reader, int columnIndex, short defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsShort(), defaultValue);
        }

        public static short? AsShortNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsShortNull(), default(short?));
        }

        public static short? AsShortNull(this IDataReader reader, int columnIndex, short? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsShortNull(), defaultValue);
        }

        public static ushort AsUshort(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsUshort(), default(ushort));
        }

        public static ushort AsUshort(this IDataReader reader, int columnIndex, ushort defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsUshort(), defaultValue);
        }

        public static ushort? AsUshortNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsUshortNull(), default(ushort?));
        }

        public static ushort? AsUshortNull(this IDataReader reader, int columnIndex, ushort? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsUshortNull(), defaultValue);
        }

        public static int AsInt(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsInt(), default(int));
        }

        public static int AsInt(this IDataReader reader, int columnIndex, int defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsInt(), defaultValue);
        }

        public static int? AsIntNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsIntNull(), default(int?));
        }

        public static int? AsIntNull(this IDataReader reader, int columnIndex, int? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsIntNull(), defaultValue);
        }

        public static uint AsUint(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsUshort(), default(uint));
        }

        public static uint AsUint(this IDataReader reader, int columnIndex, uint defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsUshort(), defaultValue);
        }

        public static uint? AsUintNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsUshortNull(), default(uint?));
        }

        public static uint? AsUintNull(this IDataReader reader, int columnIndex, uint? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsUshortNull(), defaultValue);
        }

        public static T AsEnum<T>(this IDataReader reader, int columnIndex)
        {
            try
            {
                dynamic value = reader.GetValue(columnIndex).AsInt();
                return (T)value;
            }
            catch
            {
                return default(T);
            }
        }

        public static T AsEnum<T>(this IDataReader reader, int columnIndex, T defaultValue)
        {
            try
            {
                dynamic value = reader.GetValue(columnIndex).AsInt();
                return (T)value;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T? AsEnumNull<T>(this IDataReader reader, int columnIndex) where T : struct
        {
            try
            {
                dynamic value = reader.GetValue(columnIndex).AsInt();
                return (T?)value;
            }
            catch
            {
                return default(T?);
            }
        }

        public static T? AsEnumNull<T>(this IDataReader reader, int columnIndex, T? defaultValue) where T : struct
        {
            try
            {
                dynamic value = reader.GetValue(columnIndex).AsInt();
                return (T?)value;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long AsLong(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsLong(), default(long));
        }

        public static long AsLong(this IDataReader reader, int columnIndex, long defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsLong(), defaultValue);
        }

        public static long? AsLongNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsLongNull(), default(long?));
        }

        public static long? AsLongNull(this IDataReader reader, int columnIndex, long? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsLongNull(), defaultValue);
        }

        public static ulong AsUlong(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsUlong(), default(ulong));
        }

        public static ulong AsUlong(this IDataReader reader, int columnIndex, ulong defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsUlong(), defaultValue);
        }

        public static ulong? AsUlongNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsUlongNull(), default(ulong?));
        }

        public static ulong? AsUlongNull(this IDataReader reader, int columnIndex, ulong? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsUlongNull(), defaultValue);
        }

        public static Guid AsGuid(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsGuid(), default(Guid));
        }

        public static Guid AsGuid(this IDataReader reader, int columnIndex, Guid defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsGuid(), defaultValue);
        }

        public static Guid? AsGuidNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsGuidNull(), default(Guid?));
        }

        public static Guid? AsGuidNull(this IDataReader reader, int columnIndex, Guid? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsGuidNull(), defaultValue);
        }

        public static float AsFloat(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsFloat(), default(float));
        }

        public static float AsFloat(this IDataReader reader, int columnIndex, float defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsFloat(), defaultValue);
        }

        public static float? AsFloatNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsFloatNull(), default(float?));
        }

        public static float? AsFloatNull(this IDataReader reader, int columnIndex, float? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsFloatNull(), defaultValue);
        }

        public static double AsDouble(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsDouble(), default(double));
        }

        public static double AsDouble(this IDataReader reader, int columnIndex, double defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsDouble(), defaultValue);
        }

        public static double? AsDoubleNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsDoubleNull(), default(double?));
        }

        public static double? AsDoubleNull(this IDataReader reader, int columnIndex, double? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsDoubleNull(), defaultValue);
        }

        public static decimal AsDecimal(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsDecimal(), default(decimal));
        }

        public static decimal AsDecimal(this IDataReader reader, int columnIndex, decimal defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsDecimal(), defaultValue);
        }

        public static decimal? AsDecimalNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsDecimalNull(), default(decimal?));
        }

        public static decimal? AsDecimalNull(this IDataReader reader, int columnIndex, decimal? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsDecimalNull(), defaultValue);
        }

        public static DateTime AsDateTime(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsDateTime(), default(DateTime));
        }

        public static DateTime AsDateTime(this IDataReader reader, int columnIndex, DateTime defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsDateTime(), defaultValue);
        }

        public static DateTime? AsDateTimeNull(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsDateTimeNull(), default(DateTime?));
        }

        public static DateTime? AsDateTimeNull(this IDataReader reader, int columnIndex, DateTime? defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsDateTimeNull(), defaultValue);
        }

        public static char[] AsChars(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsChars(), default(char[]));
        }

        public static char[] AsChars(this IDataReader reader, int columnIndex, char[] defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsChars(), defaultValue);
        }

        public static byte[] AsBytes(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsBytes(), default(byte[]));
        }

        public static byte[] AsBytes(this IDataReader reader, int columnIndex, byte[] defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsBytes(), defaultValue);
        }

        public static string AsString(this IDataReader reader, int columnIndex)
        {
            return Try(() => reader.GetValue(columnIndex).AsString(), default(string));
        }

        public static string AsString(this IDataReader reader, int columnIndex, string defaultValue)
        {
            return Try(() => reader.GetValue(columnIndex).AsString(), defaultValue);
        }

        private static T Try<T>(Func<T> func, T defaultValue)
        {
            try
            {
                return func();
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}