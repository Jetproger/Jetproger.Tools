using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Convert.Converts
{
    public static class ConvertExtensions
    { 
        public static T As<T>(this object value, Encoding encoding = null)
        {
            if (value == null || value == DBNull.Value) return f.sys.defaultof<T>();
            var typeFr = value.GetType();
            var typeTo = typeof(T);
            if (typeFr == typeTo) return (T)value;
            if (f.sys.IsTypeOf(typeFr, typeTo)) return (T)value;
            // commands                                                                                           
            if (typeTo == typeof(CommandException) && value is CommandMessage[]) return (T)(object)CommandMessagesAsCommandException((CommandMessage[])value);
            if (typeTo == typeof(CommandException) && value is CommandMessage) return (T)(object)CommandMessageAsCommandException((CommandMessage)value);
            if (typeTo == typeof(CommandMessage) && value is Exception) return (T)(object)ExceptionAsCommandMessage((Exception)value);
            // bytes                                                                                              
            if (typeTo == typeof(byte[]) && value is MemoryStream) return (T)(object)MemoryStreamAsBytesArray((MemoryStream)value);
            if (typeTo == typeof(byte[]) && value is Image) return (T)(object)ImageAsBytesArray((Image)value);
            if (typeTo == typeof(byte[]) && value is Icon) return (T)(object)IconAsBytesArray((Icon)value);
            if (typeTo == typeof(byte[]) && value is string) return (T)(object)StringAsBytesArray((string)value, encoding);
            if (typeTo == typeof(byte[])) return (T)(object)ObjectAsBytesArray((string)value, encoding);
            if (typeTo == typeof(MemoryStream) && value is byte[]) return (T)(object)BytesArrayAsMemoryStream((byte[])value);
            if (typeTo == typeof(Image) && value is byte[]) return (T)(object)BytesArrayAsImage((byte[])value);
            if (typeTo == typeof(Icon) && value is byte[]) return (T)(object)BytesArrayAsIcon((byte[])value);
            if (typeTo == typeof(MemoryStream)) return (T)(object)ObjectAsMemoryStream(value, encoding);
            if (value is MemoryStream) return (T)MemoryStreamAsObject((MemoryStream)value, typeTo, encoding);
            if (value is byte[]) return (T)BytesArrayAsObject((byte[])value, typeTo, encoding);
            //pictures
            if (value is Icon && typeTo == typeof(Image)) return (T)(object)IconAsImage((Icon)value);
            if (value is Image && typeTo == typeof(Icon)) return (T)(object)ImageAsIcon((Image)value);
            // strings
            if (value is string) return f.str.to<T>(value.ToString());
            if (typeTo == typeof(string) && f.sys.IsTypeOf(typeFr, typeof(Exception))) return (T)(object)ExceptionAsString((Exception)value);
            if (typeTo == typeof(string)) return (T)(object)f.str.of(value);
            // bools
            if (typeTo == typeof(bool)) return (T)(object)ObjectAsBool(value);
            // dates
            if ((typeTo == typeof(DateTime))
            && (value is decimal || value is float || value is double || value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte)) return (T)(object)DateTime.FromOADate(As<double>(value));
            if ((typeTo == typeof(decimal) || typeTo == typeof(float) || typeTo == typeof(double) || typeTo == typeof(long) || typeTo == typeof(ulong) || typeTo == typeof(int) || typeTo == typeof(uint) || typeTo == typeof(short) || typeTo == typeof(ushort) || typeTo == typeof(byte) || typeTo == typeof(sbyte))
            && (value is DateTime)) return Cast<T>(((DateTime)value).ToOADate());
            // enums
            if (typeTo.IsEnum
            && (value is decimal || value is float || value is double || value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte)) return (T)(object)Cast<long>(value);
            if ((typeTo == typeof(decimal) || typeTo == typeof(float) || typeTo == typeof(double) || typeTo == typeof(long) || typeTo == typeof(ulong) || typeTo == typeof(int) || typeTo == typeof(uint) || typeTo == typeof(short) || typeTo == typeof(ushort) || typeTo == typeof(byte) || typeTo == typeof(sbyte))
            && value.GetType().IsEnum) return Cast<T>((long)value);
            // numbers
            if ((typeTo == typeof(decimal) || typeTo == typeof(float) || typeTo == typeof(double) || typeTo == typeof(long) || typeTo == typeof(ulong) || typeTo == typeof(int) || typeTo == typeof(uint) || typeTo == typeof(short) || typeTo == typeof(ushort) || typeTo == typeof(byte) || typeTo == typeof(sbyte))
            && (value is decimal || value is float || value is double || value is long || value is ulong || value is int || value is uint || value is short || value is ushort || value is byte || value is sbyte)) return Cast<T>(value);
            // try
            return (T)value;
        }

        public static object As(this object value, Type typeTo)
        {
            var nullType = Nullable.GetUnderlyingType(typeTo);
            if (nullType != null) return As(value, nullType);
            var method = t<MethodInfo>.key(typeTo, GetConvertMethod);
            return method != null ? method.Invoke(null, new[] { value }) : null;
        }

        private static bool ObjectAsBool(object value)
        {
            if (value == null || value == DBNull.Value) return false;
            if (!f.sys.IsSimple(value.GetType())) return true;
            if (value is string)
            {
                var s = value.ToString().ToLower();
                return !string.IsNullOrWhiteSpace(s) && (s == "yes" || s == "да" || s == "1" || s == "true");
            }
            var longValue = value.As<long>();
            return longValue != 0;
        }

        private static MethodInfo GetConvertMethod(Type genericType)
        {
            var method = typeof(ConvertExtensions)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(x => x.Name == "As" && x.GetParameters().Length == 1);
            return method != null ? method.MakeGenericMethod(genericType) : null;
        }

        public static T Cast<T>(object value)
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

        private static MemoryStream ObjectAsMemoryStream(object obj, Encoding encoding = null)
        {
            return BytesArrayAsMemoryStream(ObjectAsBytesArray(obj, encoding));
        }

        private static MemoryStream BytesArrayAsMemoryStream(byte[] bytes)
        {
            var ms = new MemoryStream();
            ms.Write(bytes, 0, bytes.Length);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        private static T MemoryStreamAsObject<T>(MemoryStream ms, Encoding encoding = null)
        {
            return (T)MemoryStreamAsObject(ms, typeof(T), encoding);
        }

        private static object MemoryStreamAsObject(MemoryStream ms, Type type, Encoding encoding = null)
        {
            return BytesArrayAsObject(MemoryStreamAsBytesArray(ms), type, encoding);
        }

        private static byte[] MemoryStreamAsBytesArray(MemoryStream ms)
        {
            var bytes = new byte[ms.Length];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        private static byte[] ObjectAsBytesArray(object obj, Encoding encoding = null)
        {
            if (obj == null || obj == DBNull.Value) return new byte[0];
            var bytes = obj as byte[];
            if (bytes != null) return bytes;
            var str = obj as string;
            if (str != null) return StringAsBytesArray(str, encoding);
            var doc = obj as XmlDocument;
            if (doc != null) return StringAsBytesArray(doc.InnerXml, encoding);
            return StringAsBytesArray(f.web.of(obj), encoding);
        }

        private static T BytesArrayAsObject<T>(byte[] bytes, Encoding encoding = null)
        {
            return (T)BytesArrayAsObject(bytes, typeof(T));
        }

        private static object BytesArrayAsObject(byte[] bytes, Type type, Encoding encoding = null)
        {
            if (type == typeof(byte[])) return bytes;
            encoding = encoding ?? Encoding.GetEncoding("utf-16");
            var str = encoding.GetString(bytes);
            if (type == typeof(string)) return str;
            if (type == typeof(XmlDocument)) return StringAsXmlDocument(str);
            return f.web.to(str, type);
        }

        private static XmlDocument StringAsXmlDocument(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        private static byte[] StringAsBytesArray(string str, Encoding encoding = null)
        {
            var utf16 = Encoding.GetEncoding("utf-16");
            encoding = encoding ?? utf16;
            var bytes = utf16.GetBytes(str);
            if (encoding.EncodingName != utf16.EncodingName) bytes = Encoding.Convert(utf16, encoding, bytes);
            return bytes;
        }

        private static Image IconAsImage(Icon icon)
        {
            try
            {
                return icon.ToBitmap();
            }
            catch
            {
                return f.gui.DefaultImage();
            }
        }

        private static Icon ImageAsIcon(Image image)
        {
            try
            {
                return f.gui.ImageAsIcon(image, image.Size.Width, image.Size.Height);
            }
            catch
            {
                return f.gui.DefaultIcon();
            }
        }

        private static byte[] IconAsBytesArray(Icon icon)
        {
            using (var ms = new MemoryStream())
            {
                icon.Save(ms);
                return ms.ToArray();
            }
        }

        private static byte[] ImageAsBytesArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private static Image BytesArrayAsImage(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms, true);
            }
        }

        private static Icon BytesArrayAsIcon(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return new Icon(ms);
            }
        }

        private static string ExceptionAsString(Exception e)
        {
            var we = e as WebException;
            if (we != null)
            {
                if (we.Response == null) return we.ToString();
                var responseStream = we.Response.GetResponseStream();
                if (responseStream == null) return we.ToString();
                using (var sr = new StreamReader(responseStream)) { return sr.ReadToEnd(); }
            }
            var sb = new StringBuilder();
            sb.AppendLine(e.ToString());
            while (e != null)
            {
                sb.AppendLine(e.Message);
                e = e.InnerException;
            }
            return sb.ToString();
        }

        private static CommandMessage ExceptionAsCommandMessage(Exception exception)
        {
            return new CommandMessage(exception);
        }

        private static CommandException CommandMessageAsCommandException(CommandMessage msg)
        {
            if (msg.Category == ECommandMessage.Error) return new CommandException(msg);
            foreach (CommandMessage item in (msg.Messages ?? new CommandMessage[0]))
            {
                if (item.Category == ECommandMessage.Error) return new CommandException(item); ;
            }
            return null;
        }

        private static Exception CommandMessagesAsCommandException(CommandMessage[] messages)
        {
            var msg = (messages ?? new CommandMessage[0]).FirstOrDefault(x => x.Category == ECommandMessage.Error);
            return msg != null ? new CommandException(msg) : null;
        }
    }
}