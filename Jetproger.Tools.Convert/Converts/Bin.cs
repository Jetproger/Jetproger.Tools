using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Converts
{
    public static class BinExtensions
    {
        public static byte[] Of(this Je.IBinExpander e, object value)
        {
            return Je<BinaryConverter>.Onu().Serialize(value);
        }

        public static T To<T>(this Je.IBinExpander e, byte[] bytes)
        {
            return (T)Je<BinaryConverter>.Onu().Deserialize(bytes, typeof(T));
        }

        public static object To(this Je.IBinExpander e, byte[] bytes, Type type)
        {
            return Je<BinaryConverter>.Onu().Deserialize(bytes, type);
        }

        public static Guid HashOf(this Je.IBinExpander exp, object value)
        {
            return HashOf(exp, Of(exp, value));
        }

        public static Guid HashOf(this Je.IBinExpander exp, byte[] bytes)
        {   
            return new Guid(MD5.Create().ComputeHash(bytes));
        }

        public static byte[] Zip(this Je.IBinExpander exp, byte[] bytes)
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

        public static byte[] Unzip(this Je.IBinExpander exp, byte[] bytes)
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

        public static MemoryStream ObjToMem(this Je.IBinExpander exp, object obj, Encoding encoding = null)
        {
            return BinToMem(exp, ObjToBin(exp, obj, encoding));
        }

        public static MemoryStream BinToMem(this Je.IBinExpander exp, byte[] bytes)
        {
            var ms = new MemoryStream();
            ms.Write(bytes, 0, bytes.Length);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public static T MemToObj<T>(this Je.IBinExpander exp, MemoryStream ms, Encoding encoding = null)
        {
            return (T)MemToObj(exp, ms, typeof(T), encoding);
        }

        public static object MemToObj(this Je.IBinExpander exp, MemoryStream ms, Type type, Encoding encoding = null)
        {
            return BinToObj(exp, MemToBin(exp, ms), type, encoding);
        }

        public static byte[] MemToBin(this Je.IBinExpander exp, MemoryStream ms)
        {
            var bytes = new byte[ms.Length];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        public static byte[] ObjToBin(this Je.IBinExpander exp, object obj, Encoding encoding = null)
        {
            if (obj == null || obj == DBNull.Value) return new byte[0];
            var bytes = obj as byte[];
            if (bytes != null) return bytes;
            var str = obj as string;
            if (str != null) return Je.str.StrToBin(str, encoding);
            var doc = obj as XmlDocument;
            if (doc != null) return Je.str.StrToBin(doc.InnerXml, encoding);
            return Je.str.StrToBin(Je.web.Of(obj), encoding);
        }

        public static T BinToObj<T>(this Je.IBinExpander exp, byte[] bytes, Encoding encoding = null)
        {
            return (T)BinToObj(exp, bytes, typeof(T));
        }

        public static object BinToObj(this Je.IBinExpander exp, byte[] bytes, Type type, Encoding encoding = null)
        {
            if (type == typeof(byte[])) return bytes;
            encoding = encoding ?? Encoding.GetEncoding("utf-16");
            var str = encoding.GetString(bytes);
            if (type == typeof(string)) return str;
            if (type == typeof(XmlDocument)) return Je.str.StrToXml(str);
            return Je.web.To(str, type);
        }

        public static bool IsEqualBytes(this Je.IBinExpander exp, byte[] bytes1, byte[] bytes2)
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
            if (value is Icon) return SerializeIcon((Icon)value);
            if (value is Image) return SerializeImage((Image)value);
            return SerializeObject(value);
        }

        protected virtual byte[] SerializeIcon(Icon icon)
        {
            using (var ms = new MemoryStream())
            {
                icon.Save(ms);
                return ms.ToArray();
            }
        }

        protected virtual byte[] SerializeImage(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
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
            if (type == typeof(Icon)) return DeserializeIcon(bytes);
            if (type == typeof(Image)) return DeserializeImage(bytes);
            return DeserializeObject(bytes);
        }

        protected virtual char[] DeserializeChars(byte[] bytes)
        {
            var chars = new char[bytes.Length];
            System.Convert.ToBase64CharArray(bytes, 0, bytes.Length, chars, 0, Base64FormattingOptions.None);
            return chars;
        }

        protected virtual Image DeserializeImage(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms, true);
            }
        }

        protected virtual Icon DeserializeIcon(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return new Icon(ms);
            }
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