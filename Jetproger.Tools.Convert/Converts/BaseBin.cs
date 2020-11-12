using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Jetproger.Tools.Convert.Converts
{
    public static class Bin<T> where T : BaseBin, new()
    {
        public static byte[] Of(object value)
        {
            return One<T>.Ge.Of(value);
        }

        public static TOut To<TOut>(byte[] bytes)
        {
            return One<T>.Ge.To<TOut>(bytes);
        }

        public static object To(byte[] bytes, Type type)
        {
            return One<T>.Ge.To(bytes, type);
        }
    }

    public class BaseBin
    {
        public virtual byte[] Of(object value)
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
            if (value is Icon) return OfIcon((Icon)value);
            if (value is Image) return OfImage((Image)value);
            return OfObject(value);
        }

        private static byte[] OfIcon(Icon icon)
        {
            using (var ms = new MemoryStream())
            {
                icon.Save(ms);
                return ms.ToArray();
            }
        }

        private static byte[] OfImage(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private static byte[] OfObject(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public T To<T>(byte[] bytes)
        {
            return (T)To(bytes, typeof(T));
        }

        public virtual object To(byte[] bytes, Type type)
        {
            if (type == null) return null;
            if (bytes == null || bytes.Length == 0) return type.IsValueType ? Activator.CreateInstance(type) : null;
            if (type == typeof(byte[])) return bytes;
            if (type == typeof(string)) return System.Convert.ToBase64String(bytes, Base64FormattingOptions.None);
            if (type == typeof(char[])) return ToChars(bytes);
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
            if (type == typeof(Icon)) return ToIcon(bytes);
            if (type == typeof(Image)) return ToImage(bytes);
            return ToObject(bytes);
        }

        private static char[] ToChars(byte[] bytes)
        {
            var chars = new char[bytes.Length];
            System.Convert.ToBase64CharArray(bytes, 0, bytes.Length, chars, 0, Base64FormattingOptions.None);
            return chars;
        }

        private static Image ToImage(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms, true);
            }
        }

        private static Icon ToIcon(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return new Icon(ms);
            }
        }

        private static object ToObject(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                var obj = bf.Deserialize(ms);
                return obj;
            }
        }
    }
}