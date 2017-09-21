using System;
using System.Data;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class ConvertExtensions
    {
        public static bool AsBool(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsBool() : default(bool);
        }

        public static bool AsBool(this DataRow row, string columnName, bool defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsBool() : defaultValue;
        }

        public static bool? AsBoolNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsBoolNull() : default(bool?);
        }

        public static bool? AsBoolNull(this DataRow row, string columnName, bool? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsBoolNull() : defaultValue;
        }

        public static char AsChar(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsChar() : default(char);
        }

        public static char AsChar(this DataRow row, string columnName, char defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsChar() : defaultValue;
        }

        public static char? AsCharNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsCharNull() : default(char?);
        }

        public static char? AsCharNull(this DataRow row, string columnName, char? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsCharNull() : defaultValue;
        }

        public static byte AsByte(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsByte() : default(byte);
        }

        public static byte AsByte(this DataRow row, string columnName, byte defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsByte() : defaultValue;
        }

        public static byte? AsByteNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsByteNull() : default(byte?);
        }

        public static byte? AsByteNull(this DataRow row, string columnName, byte? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsByteNull() : defaultValue;
        }

        public static sbyte AsSbyte(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsSbyte() : default(sbyte);
        }

        public static sbyte AsSbyte(this DataRow row, string columnName, sbyte defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsSbyte() : defaultValue;
        }

        public static sbyte? AsSbyteNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsSbyteNull() : default(sbyte?);
        }

        public static sbyte? AsSbyteNull(this DataRow row, string columnName, sbyte? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsSbyteNull() : defaultValue;
        }

        public static short AsShort(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsShort() : default(short);
        }

        public static short AsShort(this DataRow row, string columnName, short defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsShort() : defaultValue;
        }

        public static short? AsShortNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsShortNull() : default(short?);
        }

        public static short? AsShortNull(this DataRow row, string columnName, short? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsShortNull() : defaultValue;
        }

        public static ushort AsUshort(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUshort() : default(ushort);
        }

        public static ushort AsUshort(this DataRow row, string columnName, ushort defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUshort() : defaultValue;
        }

        public static ushort? AsUshortNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUshortNull() : default(ushort?);
        }

        public static ushort? AsUshortNull(this DataRow row, string columnName, ushort? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUshortNull() : defaultValue;
        }

        public static int AsInt(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsInt() : default(int);
        }

        public static int AsInt(this DataRow row, string columnName, int defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsInt() : defaultValue;
        }

        public static int? AsIntNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsIntNull() : default(int?);
        }

        public static int? AsIntNull(this DataRow row, string columnName, int? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsIntNull() : defaultValue;
        }

        public static uint AsUint(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUshort() : default(uint);
        }

        public static uint AsUint(this DataRow row, string columnName, uint defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUshort() : defaultValue;
        }

        public static uint? AsUintNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUshortNull() : default(uint?);
        }

        public static uint? AsUintNull(this DataRow row, string columnName, uint? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUshortNull() : defaultValue;
        }

        public static T AsEnum<T>(this DataRow row, string columnName)
        {
            if (row == null || !row.Table.Columns.Contains(columnName)) return default(T);
            dynamic value = row[columnName].AsInt();
            return (T)value;
        }

        public static T AsEnum<T>(this DataRow row, string columnName, T defaultValue)
        {
            if (row == null || !row.Table.Columns.Contains(columnName)) return defaultValue;
            dynamic value = row[columnName].AsInt();
            return (T)value;
        }

        public static T? AsEnumNull<T>(this DataRow row, string columnName) where T : struct
        {
            if (row == null || !row.Table.Columns.Contains(columnName)) return default(T?);
            dynamic value = row[columnName].AsInt();
            return (T?)value;
        }

        public static T? AsEnumNull<T>(this DataRow row, string columnName, T? defaultValue) where T : struct
        {
            if (row == null || !row.Table.Columns.Contains(columnName)) return defaultValue;
            dynamic value = row[columnName].AsInt();
            return (T?)value;
        }

        public static long AsLong(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsLong() : default(long);
        }

        public static long AsLong(this DataRow row, string columnName, long defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsLong() : defaultValue;
        }

        public static long? AsLongNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsLongNull() : default(long?);
        }

        public static long? AsLongNull(this DataRow row, string columnName, long? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsLongNull() : defaultValue;
        }

        public static ulong AsUlong(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUlong() : default(ulong);
        }

        public static ulong AsUlong(this DataRow row, string columnName, ulong defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUlong() : defaultValue;
        }

        public static ulong? AsUlongNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUlongNull() : default(ulong?);
        }

        public static ulong? AsUlongNull(this DataRow row, string columnName, ulong? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsUlongNull() : defaultValue;
        }

        public static Guid AsGuid(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsGuid() : default(Guid);
        }

        public static Guid AsGuid(this DataRow row, string columnName, Guid defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsGuid() : defaultValue;
        }

        public static Guid? AsGuidNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsGuidNull() : default(Guid?);
        }

        public static Guid? AsGuidNull(this DataRow row, string columnName, Guid? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsGuidNull() : defaultValue;
        }

        public static float AsFloat(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsFloat() : default(float);
        }

        public static float AsFloat(this DataRow row, string columnName, float defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsFloat() : defaultValue;
        }

        public static float? AsFloatNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsFloatNull() : default(float?);
        }

        public static float? AsFloatNull(this DataRow row, string columnName, float? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsFloatNull() : defaultValue;
        }

        public static double AsDouble(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDouble() : default(double);
        }

        public static double AsDouble(this DataRow row, string columnName, double defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDouble() : defaultValue;
        }

        public static double? AsDoubleNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDoubleNull() : default(double?);
        }

        public static double? AsDoubleNull(this DataRow row, string columnName, double? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDoubleNull() : defaultValue;
        }

        public static decimal AsDecimal(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDecimal() : default(decimal);
        }

        public static decimal AsDecimal(this DataRow row, string columnName, decimal defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDecimal() : defaultValue;
        }

        public static decimal? AsDecimalNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDecimalNull() : default(decimal?);
        }

        public static decimal? AsDecimalNull(this DataRow row, string columnName, decimal? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDecimalNull() : defaultValue;
        }

        public static DateTime AsDateTime(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDateTime() : default(DateTime);
        }

        public static DateTime AsDateTime(this DataRow row, string columnName, DateTime defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDateTime() : defaultValue;
        }

        public static DateTime? AsDateTimeNull(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDateTimeNull() : default(DateTime?);
        }

        public static DateTime? AsDateTimeNull(this DataRow row, string columnName, DateTime? defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsDateTimeNull() : defaultValue;
        }

        public static char[] AsChars(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsChars() : default(char[]);
        }

        public static char[] AsChars(this DataRow row, string columnName, char[] defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsChars() : defaultValue;
        }

        public static byte[] AsBytes(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsBytes() : default(byte[]);
        }

        public static byte[] AsBytes(this DataRow row, string columnName, byte[] defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsBytes() : defaultValue;
        }

        public static string AsString(this DataRow row, string columnName)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsString() : default(string);
        }

        public static string AsString(this DataRow row, string columnName, string defaultValue)
        {
            return row != null && row.Table.Columns.Contains(columnName) ? row[columnName].AsString() : defaultValue;
        }

        public static bool AsBool(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsBool() : default(bool);
        }

        public static bool AsBool(this DataRow row, int columnIndex, bool defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsBool() : defaultValue;
        }

        public static bool? AsBoolNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsBoolNull() : default(bool?);
        }

        public static bool? AsBoolNull(this DataRow row, int columnIndex, bool? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsBoolNull() : defaultValue;
        }

        public static char AsChar(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsChar() : default(char);
        }

        public static char AsChar(this DataRow row, int columnIndex, char defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsChar() : defaultValue;
        }

        public static char? AsCharNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsCharNull() : default(char?);
        }

        public static char? AsCharNull(this DataRow row, int columnIndex, char? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsCharNull() : defaultValue;
        }

        public static byte AsByte(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsByte() : default(byte);
        }

        public static byte AsByte(this DataRow row, int columnIndex, byte defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsByte() : defaultValue;
        }

        public static byte? AsByteNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsByteNull() : default(byte?);
        }

        public static byte? AsByteNull(this DataRow row, int columnIndex, byte? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsByteNull() : defaultValue;
        }

        public static sbyte AsSbyte(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsSbyte() : default(sbyte);
        }

        public static sbyte AsSbyte(this DataRow row, int columnIndex, sbyte defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsSbyte() : defaultValue;
        }

        public static sbyte? AsSbyteNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsSbyteNull() : default(sbyte?);
        }

        public static sbyte? AsSbyteNull(this DataRow row, int columnIndex, sbyte? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsSbyteNull() : defaultValue;
        }

        public static short AsShort(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsShort() : default(short);
        }

        public static short AsShort(this DataRow row, int columnIndex, short defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsShort() : defaultValue;
        }

        public static short? AsShortNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsShortNull() : default(short?);
        }

        public static short? AsShortNull(this DataRow row, int columnIndex, short? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsShortNull() : defaultValue;
        }

        public static ushort AsUshort(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUshort() : default(ushort);
        }

        public static ushort AsUshort(this DataRow row, int columnIndex, ushort defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUshort() : defaultValue;
        }

        public static ushort? AsUshortNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUshortNull() : default(ushort?);
        }

        public static ushort? AsUshortNull(this DataRow row, int columnIndex, ushort? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUshortNull() : defaultValue;
        }

        public static int AsInt(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsInt() : default(int);
        }

        public static int AsInt(this DataRow row, int columnIndex, int defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsInt() : defaultValue;
        }

        public static int? AsIntNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsIntNull() : default(int?);
        }

        public static int? AsIntNull(this DataRow row, int columnIndex, int? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsIntNull() : defaultValue;
        }

        public static uint AsUint(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUshort() : default(uint);
        }

        public static uint AsUint(this DataRow row, int columnIndex, uint defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUshort() : defaultValue;
        }

        public static uint? AsUintNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUshortNull() : default(uint?);
        }

        public static uint? AsUintNull(this DataRow row, int columnIndex, uint? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUshortNull() : defaultValue;
        }

        public static T AsEnum<T>(this DataRow row, int columnIndex)
        {
            if (row == null || columnIndex < 0 || columnIndex >= row.Table.Columns.Count) return default(T);
            dynamic value = row[columnIndex].AsInt();
            return (T)value;
        }

        public static T AsEnum<T>(this DataRow row, int columnIndex, T defaultValue)
        {
            if (row == null || columnIndex < 0 || columnIndex >= row.Table.Columns.Count) return defaultValue;
            dynamic value = row[columnIndex].AsInt();
            return (T)value;
        }

        public static T? AsEnumNull<T>(this DataRow row, int columnIndex) where T : struct
        {
            if (row == null || columnIndex < 0 || columnIndex >= row.Table.Columns.Count) return default(T?);
            dynamic value = row[columnIndex].AsInt();
            return (T?)value;
        }

        public static T? AsEnumNull<T>(this DataRow row, int columnIndex, T? defaultValue) where T : struct
        {
            if (row == null || columnIndex < 0 || columnIndex >= row.Table.Columns.Count) return defaultValue;
            dynamic value = row[columnIndex].AsInt();
            return (T?)value;
        }

        public static long AsLong(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsLong() : default(long);
        }

        public static long AsLong(this DataRow row, int columnIndex, long defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsLong() : defaultValue;
        }

        public static long? AsLongNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsLongNull() : default(long?);
        }

        public static long? AsLongNull(this DataRow row, int columnIndex, long? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsLongNull() : defaultValue;
        }

        public static ulong AsUlong(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUlong() : default(ulong);
        }

        public static ulong AsUlong(this DataRow row, int columnIndex, ulong defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUlong() : defaultValue;
        }

        public static ulong? AsUlongNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUlongNull() : default(ulong?);
        }

        public static ulong? AsUlongNull(this DataRow row, int columnIndex, ulong? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsUlongNull() : defaultValue;
        }

        public static Guid AsGuid(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsGuid() : default(Guid);
        }

        public static Guid AsGuid(this DataRow row, int columnIndex, Guid defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsGuid() : defaultValue;
        }

        public static Guid? AsGuidNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsGuidNull() : default(Guid?);
        }

        public static Guid? AsGuidNull(this DataRow row, int columnIndex, Guid? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsGuidNull() : defaultValue;
        }

        public static float AsFloat(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsFloat() : default(float);
        }

        public static float AsFloat(this DataRow row, int columnIndex, float defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsFloat() : defaultValue;
        }

        public static float? AsFloatNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsFloatNull() : default(float?);
        }

        public static float? AsFloatNull(this DataRow row, int columnIndex, float? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsFloatNull() : defaultValue;
        }

        public static double AsDouble(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDouble() : default(double);
        }

        public static double AsDouble(this DataRow row, int columnIndex, double defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDouble() : defaultValue;
        }

        public static double? AsDoubleNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDoubleNull() : default(double?);
        }

        public static double? AsDoubleNull(this DataRow row, int columnIndex, double? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDoubleNull() : defaultValue;
        }

        public static decimal AsDecimal(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDecimal() : default(decimal);
        }

        public static decimal AsDecimal(this DataRow row, int columnIndex, decimal defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDecimal() : defaultValue;
        }

        public static decimal? AsDecimalNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDecimalNull() : default(decimal?);
        }

        public static decimal? AsDecimalNull(this DataRow row, int columnIndex, decimal? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDecimalNull() : defaultValue;
        }

        public static DateTime AsDateTime(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDateTime() : default(DateTime);
        }

        public static DateTime AsDateTime(this DataRow row, int columnIndex, DateTime defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDateTime() : defaultValue;
        }

        public static DateTime? AsDateTimeNull(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDateTimeNull() : default(DateTime?);
        }

        public static DateTime? AsDateTimeNull(this DataRow row, int columnIndex, DateTime? defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsDateTimeNull() : defaultValue;
        }

        public static char[] AsChars(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsChars() : default(char[]);
        }

        public static char[] AsChars(this DataRow row, int columnIndex, char[] defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsChars() : defaultValue;
        }

        public static byte[] AsBytes(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsBytes() : default(byte[]);
        }

        public static byte[] AsBytes(this DataRow row, int columnIndex, byte[] defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsBytes() : defaultValue;
        }

        public static string AsString(this DataRow row, int columnIndex)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsString() : default(string);
        }

        public static string AsString(this DataRow row, int columnIndex, string defaultValue)
        {
            return row != null && columnIndex >= 0 && columnIndex < row.Table.Columns.Count ? row[columnIndex].AsString() : defaultValue;
        }
    }
}