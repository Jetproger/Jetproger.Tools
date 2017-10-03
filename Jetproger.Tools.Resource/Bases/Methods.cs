using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Jetproger.Tools.Resource.Bases
{
    public static partial class Toolx
    {
        private static class Methods
        {
            public static readonly JsonSerializer JsonSerializer = new JsonSerializer {
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            public static readonly CultureInfo Formatter = new CultureInfo("en-us") {
                NumberFormat =
                {
                    NumberGroupSeparator = string.Empty,
                    NumberDecimalSeparator = "."
                },
                DateTimeFormat =
                {
                    DateSeparator = "-",
                    TimeSeparator = ":"
                }
            };

            private static readonly HashSet<Type> SimpleTypes = new HashSet<Type> {
                typeof(bool), typeof(bool?),
                typeof(byte), typeof(byte?),
                typeof(sbyte), typeof(sbyte?),
                typeof(char), typeof(char?),
                typeof(short), typeof(short?), typeof(ushort), typeof(ushort?),
                typeof(int), typeof(int?), typeof(uint), typeof(uint?),
                typeof(IntPtr), typeof(IntPtr?), typeof(UIntPtr), typeof(UIntPtr?),
                typeof(long), typeof(long?), typeof(ulong), typeof(ulong?),
                typeof(Guid), typeof(Guid?),
                typeof(float), typeof(float?),
                typeof(decimal), typeof(decimal?),
                typeof(double), typeof(double?),
                typeof(DateTime), typeof(DateTime?),
                typeof(char[]), typeof(byte[]),
                typeof(string), typeof(string[]),
                typeof(Type), typeof(Type[]),
                typeof(Assembly), typeof(Assembly[]),
                typeof(FieldInfo), typeof(FieldInfo[]),
                typeof(MethodInfo), typeof(MethodInfo[]),
                typeof(PropertyInfo), typeof(PropertyInfo[]),
            };

            public static bool IsSimple(Type type)
            {
                return type != null && (type.IsEnum || SimpleTypes.Contains(type));
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

            public static string GetExceptionAsString(Exception e)
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

            public static string SerializeJson(object o)
            {
                if (o == null)
                {
                    return null;
                }
                using (var sw = new UTF8StringWriter())
                {
                    if (IsSimple(o.GetType())) sw.Write(o.AsString()); else JsonSerializer.Serialize(sw, o);
                    return sw.ToString();
                }
            }

            public static object DeserializeJson(string json, Type resultType)
            {
                using (var sr = new StringReader(json))
                {
                    return JsonSerializer.Deserialize(sr, resultType);
                }
            }

            private class UTF8StringWriter : StringWriter
            {
                public override Encoding Encoding => Encoding.UTF8;
            }

            public static string[] GetStringKeys(object[] keys)
            {
                if (keys == null) return new string[0];
                var stringKeys = new string[keys.Length];
                var i = 0;
                foreach (var key in keys)
                {
                    stringKeys[i++] = GetStringKey(key);
                }
                return stringKeys;
            }

            private static string GetStringKey(object key)
            {
                if (key == null || key == DBNull.Value) return string.Empty;
                var type = key.GetType();
                if (!IsSimple(type) || type.IsArray) return key.ToString();
                return key.AsString();
            }
        }

        private static string AsString(this object value)
        {
            if (value == null || value == DBNull.Value) return default(string);
            if (value is string) return (string)value;
            if (value is bool) return (bool)value ? "1" : "0";
            if (value is long) return ((long)value).ToString("#################0", Methods.Formatter);
            if (value is decimal) return ((decimal)value).ToString("#################0.00", Methods.Formatter);
            if (value is int) return ((int)value).ToString("#################0", Methods.Formatter);
            if (value is short) return ((short)value).ToString("#################0", Methods.Formatter);
            if (value is byte) return ((byte)value).ToString("#################0", Methods.Formatter);
            if (value is ulong) return ((ulong)value).ToString("#################0", Methods.Formatter);
            if (value is uint) return ((uint)value).ToString("#################0", Methods.Formatter);
            if (value is ushort) return ((ushort)value).ToString("#################0", Methods.Formatter);
            if (value is sbyte) return ((sbyte)value).ToString("#################0", Methods.Formatter);
            if (value is float) return ((float)value).ToString("#################0.00", Methods.Formatter);
            if (value is double) return ((double)value).ToString("#################0.00", Methods.Formatter);
            if (value is Guid) return ((Guid)value).ToString();
            if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fff", Methods.Formatter);
            if (value is char) return ((char)value).ToString(CultureInfo.InvariantCulture);
            if (value is char[]) return string.Concat((char[])value);
            if (value is byte[]) return Methods.GetBytesAsString((byte[])value);
            if (value is Icon) return Methods.AsString((Icon)value);
            if (value is MediaTypeNames.Image) return Methods.AsString((Image)value);
            if (value is Exception) return Methods.GetExceptionAsString((Exception)value);
            return Methods.SerializeJson(value);
        }
    }
}