using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class ConvertExtensions
    {
        private static class Supports
        {
            public static T[] SetLen<T>(T[] array, int length)
            {
                if (length < 1 || array.Length == length) return array;
                var newArray = new T[length];
                for (int i = 0; i < newArray.Length; i++) newArray[i] = i < array.Length ? array[i] : default(T);
                return newArray;
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

            public static T GetOne<T>(T?[] holder, Func<T> factory) where T : struct
            {
                if (holder[0] == null)
                {
                    lock (holder)
                    {
                        if (holder[0] == null) holder[0] = factory();
                    }
                }
                return holder[0].Value;
            }

            public static bool IsTypeOf(Type type, Type sample)
            {
                return type == sample || type.IsSubclassOf(sample) || type.GetInterfaces().Any(interfaceType => interfaceType == sample);
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

            public static T DeserializeJson<T>(string json)
            {
                return (T)DeserializeJson(json, typeof(T));
            }

            public static object DeserializeJson(string json, Type resultType)
            {
                if (IsSimple(resultType))
                {
                    return  json.As(resultType);
                }
                using (var sr = new StringReader(json))
                {
                    return JsonSerializer.Deserialize(sr, resultType);
                }
            }

            private class UTF8StringWriter : StringWriter
            {
                public override Encoding Encoding => Encoding.UTF8;
            }

            private static readonly HashSet<Type> _simpleTypes = new HashSet<Type> {
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
                typeof(string),
            };

            private static bool IsSimple(object value)
            {
                return value != null && IsSimple(value.GetType());
            }

            private static bool IsSimple(Type type)
            {
                return type != null && (type.IsEnum || _simpleTypes.Contains(type));
            }
        }
    }
}