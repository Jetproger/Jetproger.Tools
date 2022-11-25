using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Converts
{
    public static class BinExtensions
    {
        public static byte[] of(this f.IBinExpander e, object value)
        {
            return t<BinaryConverter>.one().Serialize(value);
        }

        public static T to<T>(this f.IBinExpander e, byte[] bytes)
        {
            return (T)t<BinaryConverter>.one().Deserialize(bytes, typeof(T));
        }

        public static object To(this f.IBinExpander e, byte[] bytes, Type type)
        {
            return t<BinaryConverter>.one().Deserialize(bytes, type);
        }

        public static Guid hashof(this f.IBinExpander exp, object value)
        {
            return hashof(exp, of(exp, value));
        }

        public static Guid hashof(this f.IBinExpander exp, byte[] bytes)
        {   
            return new Guid(MD5.Create().ComputeHash(bytes));
        }

        public static byte[] zip(this f.IBinExpander exp, byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return new byte[0];
            using (var ms = new MemoryStream())
            {
                using (var zip = new GZipStream(ms, CompressionMode.Compress))
                {
                    zip.Write(bytes, 0, bytes.Length);
                }
                return ms.ToArray();
            }
        }

        public static byte[] unzip(this f.IBinExpander exp, byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return new byte[0];
            using (var source = new MemoryStream())
            {
                source.Write(bytes, 0, bytes.Length);
                source.Position = 0;
                using (var zip = new GZipStream(source, CompressionMode.Decompress))
                {
                    using (var target = new MemoryStream())
                    {
                        var buffer = new byte[1024];
                        while (true)
                        {
                            int length = zip.Read(buffer, 0, buffer.Length);
                            if (length == 0) break;
                            target.Write(buffer, 0, length);
                        }
                        return target.ToArray();
                    }
                }
            }
        }

        public static bool IsEqualBytes(this f.IBinExpander exp, byte[] bytes1, byte[] bytes2)
        {
            if (bytes1 == null || bytes2 == null) return false;
            if (bytes1.Length != bytes2.Length) return false;
            for (int i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] != bytes2[i]) return false;
            }
            return true;
        }
    }

    public class BinaryConverter
    {
        public virtual byte[] Serialize(object value)
        {
            if (value == null || value == DBNull.Value) return default(byte[]);
            if (value is byte[]) return (byte[])value;
            if (value is string) return System.Convert.FromBase64String((string)value);
            if (value is char[]) return System.Convert.FromBase64CharArray((char[])value, 0, ((char[])value).Length);
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
            if (value is Icon) return value.As<byte[]>();
            if (value is Image) return value.As<byte[]>();
            return SerializeObject(value);
        } 

        protected virtual byte[] SerializeObject(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public virtual object Deserialize(byte[] bytes, Type type)
        {
            if (type == null) return null;
            if (bytes == null || bytes.Length == 0) return type.IsValueType ? Activator.CreateInstance(type) : null;
            if (type == typeof(byte[])) return bytes;
            if (type == typeof(string)) return System.Convert.ToBase64String(bytes, Base64FormattingOptions.None);
            if (type == typeof(char[])) return DeserializeChars(bytes);
            if (type == typeof(Guid)) return new Guid(bytes);
            if (type == typeof(DateTime)) return DateTime.FromOADate(BitConverter.ToDouble(bytes, 0));
            if (type == typeof(bool)) return BitConverter.ToBoolean(bytes, 0);
            if (type == typeof(byte)) return (byte)BitConverter.ToUInt16(bytes, 0);
            if (type == typeof(sbyte)) return (sbyte)BitConverter.ToUInt16(bytes, 0);
            if (type == typeof(char)) return BitConverter.ToChar(bytes, 0);
            if (type == typeof(short)) return BitConverter.ToInt16(bytes, 0);
            if (type == typeof(ushort)) return BitConverter.ToUInt16(bytes, 0);
            if (type == typeof(int)) return BitConverter.ToInt32(bytes, 0);
            if (type == typeof(uint)) return BitConverter.ToUInt32(bytes, 0);
            if (type == typeof(long)) return BitConverter.ToInt64(bytes, 0);
            if (type == typeof(ulong)) return BitConverter.ToUInt64(bytes, 0);
            if (type == typeof(float)) return BitConverter.ToSingle(bytes, 0);
            if (type == typeof(decimal)) return (decimal)BitConverter.ToDouble(bytes, 0);
            if (type == typeof(double)) return BitConverter.ToDouble(bytes, 0);
            if (type == typeof(Icon)) return bytes.As<Icon>();
            if (type == typeof(Image)) return bytes.As<Image>();
            return DeserializeObject(bytes);
        }

        protected virtual char[] DeserializeChars(byte[] bytes)
        {
            var chars = new char[bytes.Length];
            System.Convert.ToBase64CharArray(bytes, 0, bytes.Length, chars, 0, Base64FormattingOptions.None);
            return chars;
        }

        protected virtual object DeserializeObject(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                var bin = new BinaryFormatter();
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return bin.Deserialize(ms);
            }
        }
    }
}