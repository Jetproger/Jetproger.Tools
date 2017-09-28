using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Tools
{
    public static partial class Metadata
    {
        private static class Convert
        {
            private const string IconDefault = @"AAABAAEAEBAAAAAAGABoAwAAFgAAACgAAAAQAAAAIAAAAAEAGAAAAAAAAAMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACyYzmARyl9RSh7RCd6RCd3QiZyQCVvPiRsPCNpOyJmOSFjNyBhNh9gNR8AAAAAAAC4ZjvYoIPYoIPQjmvHeE+4ZjueVzKGSit4QiZ3QiZ4QiZ3QiZbMh1hNh8AAAAAAAC/aj3oybjiuaTbqY7TlXTJfVW+aj2eVzKDSSp4QiZsPCOMTi17RCdjNyAAAAAAAADEcUXht6Hw29DiuaTZo4fQj2zHeE62ZTqSUS9jNyDCbD67aDwAAABmOSEAAAAAAADHeE/Mg13iuaTw29DiuaTYoIPOiWTDb0KSUS/Zo4fSknEAAADOiGNpOyIAAAAAAADLgVrz4djKf1fiuaPw2s/ht6HUl3egWTPhtqDjvKcAAADiuaPQjmtsPCMAAAAAAADOimb+/fzz4djFc0jiuaPlwK2HSyvbqY59RSh9RSjdrZTnxrXTlXRvPiQAAAAAAADRkW/+/fz+/fzw29DFc0jFc0ju1snz49rw29Dt1Mfrz8HpyrrWnH1yQCUAAAAAAADUmHj+/fz+/fz+/fz9+vj68/D47+rnxLKyYzmtYDfhtqDrz8HZo4Z3QiYAAAAAAADXn4H+/fz+/fz+/fz+/fz9+vj68/DDb0LbqY/FdEmtYDft1Mfcq5F6RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz9+vjEcUXu1srbqY+yYznw29Dfspp7RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fzw29DEcUXDb0LnxLLz49rhtqB9RSgAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fz+/fz9+vj68/D47+r16OHz49qARykAAAAAAADZo4bZo4bZo4bZo4bXn4HUmHjRkW/OimbLgVrHeE/EcUW/aj24ZjuyYzkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//wAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAAD//wAA";
            private const string ImageDefault = @"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIDSURBVDhPpZLrS5NhGMb3j4SWh0oRQVExD4gonkDpg4hGYKxG6WBogkMZKgPNCEVJFBGdGETEvgwyO9DJE5syZw3PIlPEE9pgBCLZ5XvdMB8Ew8gXbl54nuf63dd90OGSnwCahxbPRNPAPMw9Xpg6ZmF46kZZ0xSKzJPIrhpDWsVnpBhGkKx3nAX8Pv7z1zg8OoY/cITdn4fwbf/C0kYAN3Ma/w3gWfZL5kzTKBxjWyK2DftwI9tyMYCZKXbNHaD91bLYJrDXsYbrWfUKwJrPE9M2M1OcVzOOpHI7Jr376Hi9ogHqFIANO0/MmmmbmSmm9a8ze+I4MrNWAdjtoJgWcx+PSzg166yZZ8xM8XvXDix9c4jIqFYAjoriBV9AhEPv1mH/sonogha0afbZMMZz+yreTGyhpusHwtNNCsA5U1zS4BLxzJIfg299qO32Ir7UJtZfftyATqeT+8o2D8JSjQrAJblrncYL7ZJ2+bfaFnC/1S1NjL3diRat7qrO7wLRP3HjWsojBeComDEo5mNjuweFGvjWg2EBhCbpkW78htSHHwRyNdmgAFzPEee2iFkzayy2OLXzT4gr6UdUnlXrullsxxQ+kx0g8BTA3aZlButjSTyjODq/WcQcW/B/Je4OQhLvKQDnzN1mp0nnkvAhR8VuMzNrpm1mpjgkoVwB/v8DTgDQASA1MVpwzwAAAABJRU5ErkJggg==";

            private static readonly DateTime MaxDate = new DateTime(2100, 1, 1, 0, 0, 0, 0);
            private static readonly DateTime MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            private readonly static Image[] DefaultImage = { null };
            private readonly static Icon[] DefaultIcon = { null };

            private static readonly JsonSerializer JsonSerializer = new JsonSerializer
            {
                Formatting = Formatting.None, ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

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

            private static readonly Dictionary<Type, Func<object, object>> Convertors = new Dictionary<Type, Func<object, object>>
            {
                { typeof(bool), o => AsBool(o) },
                { typeof(bool?), o => AsBoolNull(o) },
                { typeof(byte), o => AsByte(o) },
                { typeof(byte?), o => AsByteNull(o) },
                { typeof(sbyte), o => AsSbyte(o) },
                { typeof(sbyte?), o => AsSbyteNull(o) },
                { typeof(byte[]), o => AsBytes(o) },
                { typeof(char), o => AsChar(o) },
                { typeof(char?), o => AsCharNull(o) },
                { typeof(char[]), o => AsChars(o) },
                { typeof(short), o => AsShort(o) },
                { typeof(short?), o => AsShortNull(o) },
                { typeof(ushort), o => AsUshort(o) },
                { typeof(ushort?), o => AsUshortNull(o) },
                { typeof(int), o => AsInt(o) },
                { typeof(int?), o => AsIntNull(o) },
                { typeof(uint), o => AsUint(o) },
                { typeof(uint?), o => AsUintNull(o) },
                { typeof(long), o => AsLong(o) },
                { typeof(long?), o => AsLongNull(o) },
                { typeof(ulong), o => AsUlong(o) },
                { typeof(ulong?), o => AsUlongNull(o) },
                { typeof(Guid), o => AsGuid(o) },
                { typeof(Guid?), o => AsGuidNull(o) },
                { typeof(float), o => AsFloat(o) },
                { typeof(float?), o => AsFloatNull(o) },
                { typeof(decimal), o => AsDecimal(o) },
                { typeof(decimal?), o => AsDecimalNull(o) },
                { typeof(double), o => AsDouble(o) },
                { typeof(double?), o => AsDoubleNull(o) },
                { typeof(DateTime), o => AsDateTime(o) },
                { typeof(DateTime?), o => AsDateTimeNull(o) },
                { typeof(string), o => AsString(o) }
            };

            private static readonly Dictionary<Type, object> Defaults = new Dictionary<Type, object>
            {
                { typeof(bool), default(bool) },
                { typeof(bool?), default(bool?) },
                { typeof(byte), default(byte) },
                { typeof(byte?), default(byte?) },
                { typeof(sbyte), default(sbyte) },
                { typeof(sbyte?), default(sbyte?) },
                { typeof(byte[]), default(byte[]) },
                { typeof(short), default(short) },
                { typeof(short?), default(short?) },
                { typeof(ushort), default(ushort) },
                { typeof(ushort?), default(ushort?) },
                { typeof(int), default(int) },
                { typeof(int?), default(int?) },
                { typeof(uint), default(uint) },
                { typeof(uint?), default(uint?) },
                { typeof(long), default(long) },
                { typeof(long?), default(long?) },
                { typeof(ulong), default(ulong) },
                { typeof(ulong?), default(ulong?) },
                { typeof(Guid), default(Guid) },
                { typeof(Guid?), default(Guid?) },
                { typeof(float), default(float) },
                { typeof(float?), default(float?) },
                { typeof(decimal), default(decimal) },
                { typeof(decimal?), default(decimal?) },
                { typeof(double), default(double) },
                { typeof(double?), default(double?) },
                { typeof(DateTime), default(DateTime) },
                { typeof(DateTime?), default(DateTime?) },
                { typeof(char), default(char) },
                { typeof(char?), default(char?) },
                { typeof(char[]), default(char[]) },
                { typeof(string), default(string) }
            };

            public static object Default(Type type)
            {
                if (!Defaults.ContainsKey(type))
                {
                    if (!type.IsGenericType && !type.IsEnum) return null;
                    if (type.IsAbstract || type.IsInterface) return null;
                    var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                    if (genericType != null && (genericType == typeof (Nullable<>) || genericType == typeof (Func<>) || genericType == typeof (Action<>))) return null;
                    return Activator.CreateInstance(type);
                }
                return Defaults[type];
            }

            public static T As<T>(object value)
            {
                var resultType = typeof (T);
                if (value == null) return (T) Default(resultType);
                var valueType = value.GetType();
                if (IsTypeOf(valueType, resultType)) return (T) value;
                if (valueType == typeof (string) && resultType == typeof (Icon)) return (T)(object)AsIcon((string)value);
                if (valueType == typeof (byte[]) && resultType == typeof (Icon)) return (T)(object)AsIcon((byte[])value);
                if (valueType == typeof (string) && resultType == typeof (Image)) return (T)(object)AsImage((string)value);
                if (valueType == typeof (byte[]) && resultType == typeof (Image)) return (T)(object)AsImage((byte[])value);
                if (Convertors.ContainsKey(resultType)) return (T) Convertors[resultType](value);
                if (resultType.IsAbstract || resultType.IsInterface) return (T) (object) null;
                if (valueType == typeof (string)) return (T) Methods.DeserializeJson((string) value, resultType);
                return (T) Methods.DeserializeJson(Methods.SerializeJson(value), resultType);
            }
            
            public static object As(object value, Type resultType)
            {
                if (Convertors.ContainsKey(resultType)) return Convertors[resultType](value);
                if (value == null) return null;
                var valueType = value.GetType();
                if (IsTypeOf(valueType, resultType)) return value;
                if (valueType == typeof (string) && resultType == typeof (Icon)) return AsIcon((string)value);
                if (valueType == typeof (byte[]) && resultType == typeof (Icon)) return AsIcon((byte[])value);
                if (valueType == typeof (string) && resultType == typeof (Image)) return AsImage((string)value);
                if (valueType == typeof (byte[]) && resultType == typeof (Image)) return AsImage((byte[])value);
                if (Convertors.ContainsKey(resultType)) return Convertors[resultType](value);
                if (resultType.IsAbstract || resultType.IsInterface) return null;
                if (valueType == typeof (string)) return Methods.DeserializeJson((string) value, resultType);
                return Methods.DeserializeJson(Methods.SerializeJson(value), resultType);
            }
            
            public static bool? AsBoolNull(object value)
            {
                return value != null ? AsBool(value) : (bool?) null;
            }
            
            public static byte? AsByteNull(object value)
            {
                return value != null ? AsByte(value) : (byte?) null;
            }
            
            public static sbyte? AsSbyteNull(object value)
            {
                return value != null ? AsSbyte(value) : (sbyte?) null;
            }
            
            public static char? AsCharNull(object value)
            {
                return value != null ? AsChar(value) : (char?) null;
            }
            
            public static short? AsShortNull(object value)
            {
                return value != null ? AsShort(value) : (short?) null;
            }
            
            public static ushort? AsUshortNull(object value)
            {
                return value != null ? AsUshort(value) : (ushort?) null;
            }
            
            public static int? AsIntNull(object value)
            {
                return value != null ? AsInt(value) : (int?) null;
            }
            
            public static uint? AsUintNull(object value)
            {
                return value != null ? AsUint(value) : (uint?) null;
            }
            
            public static long? AsLongNull(object value)
            {
                return value != null ? AsLong(value) : (long?) null;
            }
            
            public static ulong? AsUlongNull(object value)
            {
                return value != null ? AsUlong(value) : (ulong?) null;
            }
            
            public static Guid? AsGuidNull(object value)
            {
                return value != null ? AsGuid(value) : (Guid?) null;
            }
            
            public static float? AsFloatNull(object value)
            {
                return value != null ? AsFloat(value) : (float?) null;
            }
            
            public static decimal? AsDecimalNull(object value)
            {
                return value != null ? AsDecimal(value) : (decimal?) null;
            }
            
            public static double? AsDoubleNull(object value)
            {
                return value != null ? AsDouble(value) : (double?) null;
            }
            
            public static DateTime? AsDateTimeNull(object value)
            {
                return value != null ? AsDateTime(value) : (DateTime?) null;
            }
            
            public static byte AsByte(object value)
            {
                byte result;
                if (value == null || value == DBNull.Value) return default(byte);
                if (value is byte) return (byte) value;
                if (value is string) return byte.TryParse((string) value, NumberStyles.Any, Formatter, out result) ? result : default(byte);
                if (value is bool) return (bool) value ? (byte) 1 : (byte) 0;
                if (value is DateTime) return (byte) ((DateTime) value).ToOADate();
                if (value is Guid) return (byte) BitConverter.ToUInt16(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return (byte) BitConverter.ToUInt16((byte[]) value, 0);
                if (value is char[]) return byte.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(byte);
                if (value.GetType().IsClass) return (byte) value.GetHashCode();
                return Cast<byte>(value);
            }
            
            public static sbyte AsSbyte(object value)
            {
                sbyte result;
                if (value == null || value == DBNull.Value) return default(sbyte);
                if (value is sbyte) return (sbyte) value;
                if (value is string) return sbyte.TryParse((string) value, NumberStyles.Any, Formatter, out result) ? result : default(sbyte);
                if (value is bool) return (bool) value ? (sbyte) 1 : (sbyte) 0;
                if (value is DateTime) return (sbyte) ((DateTime) value).ToOADate();
                if (value is Guid) return (sbyte) BitConverter.ToInt16(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return (sbyte) BitConverter.ToInt16((byte[]) value, 0);
                if (value is char[]) return sbyte.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(sbyte);
                if (value.GetType().IsClass) return (sbyte) value.GetHashCode();
                return Cast<sbyte>(value);
            }
            
            public static char AsChar(object value)
            {
                char result;
                if (value == null || value == DBNull.Value) return default(char);
                if (value is char) return (char) value;
                if (value is string) return char.TryParse((string) value, out result) ? result : default(char);
                if (value is bool) return (bool) value ? '1' : '0';
                if (value is DateTime) return (char) ((DateTime) value).ToOADate();
                if (value is Guid) return (char) BitConverter.ToUInt16(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return (char) BitConverter.ToUInt16((byte[]) value, 0);
                if (value is char[]) return SetLen((char[]) value, 1)[0];
                if (value.GetType().IsClass) return (char) value.GetHashCode();
                return Cast<char>(value);
            }
            
            public static short AsShort(object value)
            {
                short result;
                if (value == null || value == DBNull.Value) return default(short);
                if (value is short) return (short) value;
                if (value is string) return short.TryParse((string) value, NumberStyles.Any, Formatter, out result) ? result : default(short);
                if (value is bool) return (bool) value ? (short) 1 : (short) 0;
                if (value is DateTime) return (short) ((DateTime) value).ToOADate();
                if (value is Guid) return BitConverter.ToInt16(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return BitConverter.ToInt16((byte[]) value, 0);
                if (value is char[]) return short.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(short);
                if (value.GetType().IsClass) return (short) value.GetHashCode();
                return Cast<short>(value);
            }
            
            public static ushort AsUshort(object value)
            {
                ushort result;
                if (value == null || value == DBNull.Value) return default(ushort);
                if (value is ushort) return (ushort) value;
                if (value is string) return ushort.TryParse((string) value, NumberStyles.Any, Formatter, out result) ? result : default(ushort);
                if (value is bool) return (bool) value ? (ushort) 1 : (ushort) 0;
                if (value is DateTime) return (ushort) ((DateTime) value).ToOADate();
                if (value is Guid) return BitConverter.ToUInt16(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return BitConverter.ToUInt16((byte[]) value, 0);
                if (value is char[]) return ushort.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(ushort);
                if (value.GetType().IsClass) return (ushort) value.GetHashCode();
                return Cast<ushort>(value);
            }
            
            public static int AsInt(object value)
            {
                int result;
                if (value == null || value == DBNull.Value) return default(int);
                if (value is int) return (int) value;
                if (value is string) return int.TryParse((string) value, NumberStyles.Any, Formatter, out result) ? result : default(int);
                if (value is bool) return (bool) value ? 1 : 0;
                if (value is DateTime) return (int) ((DateTime) value).ToOADate();
                if (value is Guid) return BitConverter.ToInt32(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return BitConverter.ToInt32((byte[]) value, 0);
                if (value is char[]) return int.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(int);
                if (value.GetType().IsClass) return value.GetHashCode();
                return Cast<int>(value);
            }
            
            public static uint AsUint(object value)
            {
                uint result;
                if (value == null || value == DBNull.Value) return default(uint);
                if (value is uint) return (uint) value;
                if (value is string) return uint.TryParse((string) value, NumberStyles.Any, Formatter, out result) ? result : default(uint);
                if (value is bool) return (bool) value ? (uint) 1 : 0;
                if (value is DateTime) return (uint) ((DateTime) value).ToOADate();
                if (value is Guid) return BitConverter.ToUInt32(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return BitConverter.ToUInt32((byte[]) value, 0);
                if (value is char[]) return uint.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(uint);
                if (value.GetType().IsClass) return (uint) value.GetHashCode();
                return Cast<uint>(value);
            }
            
            public static long AsLong(object value)
            {
                long result;
                if (value == null || value == DBNull.Value) return default(long);
                if (value is long) return (long) value;
                if (value is string) return long.TryParse((string) value, NumberStyles.Any, Formatter, out result) ? result : default(long);
                if (value is bool) return (bool) value ? 1 : 0;
                if (value is DateTime) return (int) ((DateTime) value).ToOADate();
                if (value is Guid) return BitConverter.ToInt64(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return BitConverter.ToInt64((byte[]) value, 0);
                if (value is char[]) return long.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(long);
                if (value.GetType().IsClass) return value.GetHashCode();
                return Cast<long>(value);
            }
            
            public static ulong AsUlong(object value)
            {
                ulong result;
                if (value == null || value == DBNull.Value) return default(ulong);
                if (value is ulong) return (ulong) value;
                if (value is string) return ulong.TryParse((string) value, NumberStyles.Any, Formatter, out result) ? result : default(ulong);
                if (value is bool) return (bool) value ? (ulong) 1 : 0;
                if (value is DateTime) return (ulong) ((DateTime) value).ToOADate();
                if (value is Guid) return BitConverter.ToUInt64(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return BitConverter.ToUInt64((byte[]) value, 0);
                if (value is char[]) return ulong.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(ulong);
                if (value.GetType().IsClass) return (ulong) value.GetHashCode();
                return Cast<ulong>(value);
            }
            
            public static float AsFloat(object value)
            {
                float result;
                if (value == null || value == DBNull.Value) return default(float);
                if (value is float) return (float) value;
                if (value is string) return float.TryParse((string) value, NumberStyles.Any, Formatter, out result) ? result : default(float);
                if (value is bool) return (bool) value ? 1 : 0;
                if (value is DateTime) return (float) ((DateTime) value).ToOADate();
                if (value is Guid) return BitConverter.ToSingle(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return BitConverter.ToSingle((byte[]) value, 0);
                if (value is char[]) return float.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(float);
                if (value.GetType().IsClass) return value.GetHashCode();
                return Cast<float>(value);
            }
            
            public static decimal AsDecimal(object value)
            {
                decimal result;
                if (value == null || value == DBNull.Value) return default(decimal);
                if (value is decimal) return (decimal) value;
                if (value is string) return decimal.TryParse((string) value, NumberStyles.Any, Formatter, out result) ? result : default(decimal);
                if (value is bool) return (bool) value ? 1 : 0;
                if (value is DateTime) return (decimal) ((DateTime) value).ToOADate();
                if (value is Guid) return (decimal) BitConverter.ToDouble(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return (decimal) BitConverter.ToDouble((byte[]) value, 0);
                if (value is char[]) return decimal.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(decimal);
                if (value.GetType().IsClass) return value.GetHashCode();
                return Cast<decimal>(value);
            }
            
            public static double AsDouble(object value)
            {
                double result;
                if (value == null || value == DBNull.Value) return default(double);
                if (value is double) return (double) value;
                if (value is string) return double.TryParse((string) value, NumberStyles.Any, Formatter, out result) ? result : default(double);
                if (value is bool) return (bool) value ? 1 : 0;
                if (value is DateTime) return ((DateTime) value).ToOADate();
                if (value is Guid) return BitConverter.ToDouble(((Guid) value).ToByteArray(), 0);
                if (value is byte[]) return BitConverter.ToDouble((byte[]) value, 0);
                if (value is char[]) return double.TryParse(value.AsString(), NumberStyles.Any, Formatter, out result) ? result : default(double);
                if (value.GetType().IsClass) return value.GetHashCode();
                return Cast<double>(value);
            }
            
            public static bool AsBool(object value)
            {
                if (value == null || value == DBNull.Value) return default(bool);
                if (value is bool) return (bool) value;
                if (value is string) return AsLong(value) != default(long);
                if (value is long) return (long) value != default(long);
                if (value is decimal) return (decimal) value != default(decimal);
                if (value is int) return (int) value != default(int);
                if (value is short) return (short) value != default(short);
                if (value is byte) return (byte) value != default(byte);
                if (value is ulong) return (ulong) value != default(ulong);
                if (value is uint) return (uint) value != default(uint);
                if (value is ushort) return (ushort) value != default(ushort);
                if (value is sbyte) return (sbyte) value != default(sbyte);
                if (value is float) return (float) value != default(float);
                if (value is double) return (double) value != default(double);
                if (value is Guid) return (Guid) value != default(Guid);
                if (value is DateTime) return (DateTime) value != default(DateTime);
                if (value is char) return (char) value != default(char);
                if (value is char[]) return ((char[]) value).Length > 0;
                if (value is byte[]) return ((byte[]) value).Length > 0;
                return default(bool);
            }
            
            public static byte[] AsBytes(object value)
            {
                if (value == null || value == DBNull.Value) return default(byte[]);
                if (value is byte[]) return (byte[]) value;
                if (value is string) return GetStringAsBytes((string) value);
                if (value is char[]) return GetStringAsBytes(value.AsString());
                if (value is Guid) return ((Guid) value).ToByteArray();
                if (value is DateTime) return BitConverter.GetBytes(((DateTime) value).ToOADate());
                if (value is bool) return BitConverter.GetBytes((bool) value);
                if (value is byte) return BitConverter.GetBytes((ushort) (byte) value);
                if (value is sbyte) return BitConverter.GetBytes((short) (sbyte) value);
                if (value is char) return BitConverter.GetBytes((char) value);
                if (value is short) return BitConverter.GetBytes((short) value);
                if (value is ushort) return BitConverter.GetBytes((ushort) value);
                if (value is int) return BitConverter.GetBytes((int) value);
                if (value is uint) return BitConverter.GetBytes((uint) value);
                if (value is long) return BitConverter.GetBytes((long) value);
                if (value is ulong) return BitConverter.GetBytes((ulong) value);
                if (value is float) return BitConverter.GetBytes((float) value);
                if (value is decimal) return BitConverter.GetBytes((double) value);
                if (value is double) return BitConverter.GetBytes((double) value);
                if (value is Icon) return AsBytes((Icon)value);
                if (value is Image) return AsBytes((Image)value);
                return default(byte[]);
            }
            
            public static DateTime AsDateTimeMin(object value)
            {
                var date = AsDateTime(value);
                return date >= MinDate && date <= MaxDate ? date : MinDate;
            }
            
            public static DateTime AsDateTimeMax(object value)
            {
                var date = AsDateTime(value);
                return date >= MinDate && date <= MaxDate ? date : MaxDate;
            }
            
            public static DateTime AsDateTime(object value)
            {
                DateTime result;
                if (value == null || value == DBNull.Value) return default(DateTime);
                if (value is DateTime) return (DateTime) value;
                if (value is string) return DateTime.TryParse((string) value, Formatter, DateTimeStyles.None, out result) ? result : default(DateTime);
                if (value is char[]) return DateTime.TryParse(value.AsString(), Formatter, DateTimeStyles.None, out result) ? result : default(DateTime);
                if (value is byte[]) return DateTime.FromOADate(BitConverter.ToDouble((byte[]) value, 0));
                if (value is Guid) return DateTime.FromOADate(BitConverter.ToDouble(((Guid) value).ToByteArray(), 0));
                if (value is bool) return DateTime.FromOADate((bool) value ? 1 : 0);
                if (value is byte) return DateTime.FromOADate((byte) value);
                if (value is sbyte) return DateTime.FromOADate((sbyte) value);
                if (value is char) return DateTime.FromOADate((char) value);
                if (value is short) return DateTime.FromOADate((short) value);
                if (value is ushort) return DateTime.FromOADate((ushort) value);
                if (value is int) return DateTime.FromOADate((int) value);
                if (value is uint) return DateTime.FromOADate((uint) value);
                if (value is long) return DateTime.FromOADate((long) value);
                if (value is ulong) return DateTime.FromOADate((ulong) value);
                if (value is float) return DateTime.FromOADate((float) value);
                if (value is decimal) return DateTime.FromOADate((double) (decimal) value);
                if (value is double) return DateTime.FromOADate((double) value);
                return default(DateTime);
            }
            
            public static Guid AsGuid(object value)
            {
                Guid result;
                if (value == null || value == DBNull.Value) return default(Guid);
                if (value is Guid) return (Guid) value;
                if (value is string) return Guid.TryParse((string) value, out result) ? result : default(Guid);
                if (value is char[]) return Guid.TryParse(value.AsString(), out result) ? result : default(Guid);
                if (value is byte[]) return new Guid(SetLen((byte[]) value, 16));
                if (value is DateTime) return new Guid(SetLen(AsBytes(((DateTime)value).ToOADate()), 16));
                if (value is bool) return new Guid(SetLen(AsBytes(value), 16));
                if (value is byte) return new Guid(SetLen(AsBytes(value), 16));
                if (value is sbyte) return new Guid(SetLen(AsBytes(value), 16));
                if (value is char) return new Guid(SetLen(AsBytes(value), 16));
                if (value is short) return new Guid(SetLen(AsBytes(value), 16));
                if (value is ushort) return new Guid(SetLen(AsBytes(value), 16));
                if (value is int) return new Guid(SetLen(AsBytes(value), 16));
                if (value is uint) return new Guid(SetLen(AsBytes(value), 16));
                if (value is long) return new Guid(SetLen(AsBytes(value), 16));
                if (value is ulong) return new Guid(SetLen(AsBytes(value), 16));
                if (value is float) return new Guid(SetLen(AsBytes(value), 16));
                if (value is decimal) return new Guid(SetLen(AsBytes(value), 16));
                if (value is double) return new Guid(SetLen(AsBytes(value), 16));
                return default(Guid);
            }
            
            public static string AsString(object value)
            {
                if (value == null || value == DBNull.Value) return default(string);
                if (value is string) return (string) value;
                if (value is bool) return (bool) value ? "1" : "0";
                if (value is long) return ((long) value).ToString("#################0", Formatter);
                if (value is decimal) return ((decimal) value).ToString("#################0.00", Formatter);
                if (value is int) return ((int) value).ToString("#################0", Formatter);
                if (value is short) return ((short) value).ToString("#################0", Formatter);
                if (value is byte) return ((byte) value).ToString("#################0", Formatter);
                if (value is ulong) return ((ulong) value).ToString("#################0", Formatter);
                if (value is uint) return ((uint) value).ToString("#################0", Formatter);
                if (value is ushort) return ((ushort) value).ToString("#################0", Formatter);
                if (value is sbyte) return ((sbyte) value).ToString("#################0", Formatter);
                if (value is float) return ((float) value).ToString("#################0.00", Formatter);
                if (value is double) return ((double) value).ToString("#################0.00", Formatter);
                if (value is Guid) return ((Guid) value).ToString();
                if (value is DateTime) return ((DateTime) value).ToString("yyyy-MM-ddTHH:mm:ss.fff", Formatter);
                if (value is char) return ((char) value).ToString(CultureInfo.InvariantCulture);
                if (value is char[]) return string.Concat((char[]) value);
                if (value is byte[]) return GetBytesAsString((byte[]) value);
                if (value is Icon) return AsString((Icon)value);
                if (value is Image) return AsString((Image)value);
                if (value is Exception) return GetExceptionAsString((Exception) value);
                return Methods.SerializeJson(value);
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
            
            public static char[] AsChars(object value)
            {
                var stringValue = value.AsString();
                return stringValue != default(string) ? stringValue.ToCharArray() : default(char[]);
            }
            
            private static T Cast<T>(object value)
            {
                try
                {
                    dynamic o = value;
                    return value != null ? (T) o : default(T);
                }
                catch
                {
                    return default(T);
                }
            }

            public static string GetBytesAsString(IEnumerable<byte> bytes)
            {
                var sb = new StringBuilder("0x");
                foreach (byte b in bytes)
                {
                    string s = System.Convert.ToString(b, 16);
                    sb.AppendFormat("{0}{1}", s.Length < 2 ? "0" : "", s);
                }
                return sb.ToString();
            }

            public static byte[] GetStringAsBytes(string s)
            {
                if (s == null) return null;
                s = s.Trim(' ', '\r', '\n', '\t');
                if (s == "" || s == "0x") return new byte[0];
                if (!s.StartsWith("0x") || s.Length % 2 != 0) return Encoding.Unicode.GetBytes(s);
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
                    return Encoding.Unicode.GetBytes(s);
                }
            }

            public static T[] SetLen<T>(T[] array, int length)
            {
                if (length < 1 || array.Length == length) return array;
                var newArray = new T[length];
                for (int i = 0; i < newArray.Length; i++) newArray[i] = i < array.Length ? array[i] : default(T);
                return newArray;
            }

            public static string AsString(Image image)
            {
                return System.Convert.ToBase64String(AsBytes(image));
            }

            public static string AsString(Icon icon)
            {
                return System.Convert.ToBase64String(AsBytes(icon));
            }

            public static byte[] AsBytes(Image image)
            {
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }

            public static byte[] AsBytes(Icon icon)
            {
                using (var ms = new MemoryStream())
                {
                    icon.Save(ms);
                    return ms.ToArray();
                }
            }

            public static Icon AsIcon(Image image)
            {
                return AsIcon(image, image.Size.Width, image.Size.Height);
            }

            public static Icon AsIcon(Image image, int width, int height)
            {
                try
                {
                    var bmp = new Bitmap(image, new Size(width, height));
                    bmp.MakeTransparent(Color.Magenta);
                    var hIcon = bmp.GetHicon();
                    return Icon.FromHandle(hIcon);
                }
                catch
                {
                    return GetDefaultIcon();
                }
            }

            public static Icon AsIcon(string base64)
            {
                return AsIcon(System.Convert.FromBase64String(base64));
            }

            public static Icon AsIcon(byte[] bytes)
            {
                try
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        return new Icon(ms);
                    }
                }
                catch
                {
                    return GetDefaultIcon();
                }
            }

            public static Image AsImage(string base64)
            {
                return AsImage(System.Convert.FromBase64String(base64));
            }

            public static Image AsImage(byte[] bytes)
            {
                try
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        return Image.FromStream(ms, true);
                    }
                }
                catch
                {
                    return GetDefaultImage();
                }
            }

            public static Image GetDefaultImage()
            {
                return GetOne(DefaultImage, () => AsImage(ImageDefault));
            }

            public static Icon GetDefaultIcon()
            {
                return GetOne(DefaultIcon, () => AsIcon(IconDefault));
            }

            public static T GetOne<T>(T[] holder, Func<T> factory) where T : class
            {
                if (holder[0] == null)
                {
                    lock (holder)
                    {
                        if (holder[0] == null) holder[0] = factory();
                    }
                }
                return holder[0];
            }
        }
    }
}