using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Converts
{
    public static class Cast
    {
        public static T As<T>(this object valueFr, params object[] argsTo) { return (T)As(valueFr, typeof(T), argsTo); }
        
        private static readonly IConverter Bin = new BinaryConverter();
        
        private static readonly IConverter Str = new StringConverter();
        
        private static readonly IConverter Sql = new SqlConverter();

        public static object As(this object valueFr, Type typeTo, params object[] argsTo)
        {
            if (valueFr == null || valueFr == DBNull.Value) return f.sys.defaultof(typeTo);
            var typeFr = valueFr.GetType();
            if (f.sys.isof(typeFr, typeTo)) return valueFr;
            var nullTypeTo = Nullable.GetUnderlyingType(typeTo);
            if (nullTypeTo != null) return As(valueFr, nullTypeTo);
            return
            TryConverter(typeFr, typeTo, valueFr, out object valueTo, argsTo) ? valueTo : (
            TryCommand(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryException(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryImage(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryIcon(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryBool(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryDate(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryEnum(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryNum(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryStream(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TrySql(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryBinary(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryString(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryType(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryArray(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : (
            TryList(typeFr, typeTo, valueFr, out valueTo, argsTo) ? valueTo : valueFr)))))))))))))));
        }

        #region bytes

        private static bool TryBinary(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (typeFr == typeof(byte[]) || typeTo == typeof(byte[]) || typeof(Stream).IsAssignableFrom(typeFr) || typeof(Stream).IsAssignableFrom(typeTo))
            {
                Bin.Is(valueFr);
                valueTo = Bin.As(typeTo, argsTo);
                return true;
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region chars

        private static bool TryString(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (typeFr == typeof(string) || typeTo == typeof(string))
            {
                Str.Is(valueFr);
                valueTo = Str.As(typeTo);
                return true;
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region types

        private static bool TryType(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (typeTo == typeof(Type))
            {
                if (valueFr is string name)
                {
                    valueTo = f.sys.classof(name);
                    return true;
                }
                valueTo = typeFr;
                return true;
            }
            if (valueFr is Type type)
            {
                valueTo = !typeTo.IsInterface && typeTo != typeof(object) ? f.sys.valueof(typeTo, argsTo) : f.sys.valueof(type, argsTo);
                return true;
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region arrays

        private static bool TryArray(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (typeFr.IsArray)
            {
                var elementType = typeFr.GetElementType();
                if (elementType != null && f.sys.isof(typeTo, elementType))
                {
                    valueTo = valueFr is IList list && list.Count > 0 ? list[0] : f.sys.defaultof(typeTo);
                    return true;
                }
            }
            if (typeTo.IsArray)
            {
                var elementType = typeTo.GetElementType();
                if (elementType != null && f.sys.isof(typeFr, elementType))
                {
                    var arr = Array.CreateInstance(elementType, 1);
                    arr.SetValue(valueFr, 0);
                    valueTo = arr;
                    return true;
                }
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region lists

        private static bool TryList(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (typeFr == typeof(List<>))
            {
                var genericTypes = typeFr.GetGenericArguments();
                var genericType = genericTypes.Length == 1 ? genericTypes[0] : null;
                if (genericType != null && f.sys.isof(typeTo, genericType))
                {
                    valueTo = valueFr is IList list && list.Count > 0 ? list[0] : f.sys.defaultof(typeTo);
                    return true;
                }
            }
            if (typeTo == typeof(List<>))
            { 
                var genericTypes = typeTo.GetGenericArguments();
                var genericType = genericTypes.Length == 1 ? genericTypes[0] : null;
                if (genericType != null && f.sys.isof(typeFr, genericType))
                {
                    var list = (IList)Activator.CreateInstance(typeTo, argsTo);
                    list.Add(valueFr);
                    valueTo = list;
                    return true;
                }
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region converters

        private static bool TryConverter(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (valueFr is IConverter arg)
            {
                valueTo = arg.As(typeTo, argsTo);
                return true;
            }
            if (f.sys.isof(typeTo, typeof(IConverter)))
            {
                valueTo = Activator.CreateInstance(typeTo, argsTo);
                ((IConverter)valueTo).Is(valueFr);
                return true;
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region sql

        private static bool TrySql(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (typeFr == typeof(DataSet) && typeTo == typeof(DataTable))
            {
                var ds = (DataSet)valueFr;
                valueTo = ds.Tables.Count > 0 ? ds.Tables[0] : null;
                return true;
            }
            if (typeFr == typeof(DataTable) && typeTo == typeof(DataSet))
            {
                var dt = (DataTable)valueFr;
                var ds = new DataSet { EnforceConstraints = false };
                ds.Tables.Add(dt);
                valueTo = ds;
                return true;
            }
            if (typeFr == typeof(DataSet) || typeFr == typeof(DataTable) || typeTo == typeof(DataSet) || typeTo == typeof(DataTable))
            {
                Sql.Is(valueFr);
                valueTo = Sql.As(typeTo);
                return true;
            }
            if (valueFr is IDataReader reader)
            {
                valueTo = (new EntityWriter(reader, typeTo)).Write();
                return true;
            } 
            if (typeof(IDataReader).IsAssignableFrom(typeTo))
            {
                valueTo = new EntityReader(valueFr);
                return true;
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region command

        private static bool TryCommand(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (valueFr is CommandResponse response)
            {
                if (typeTo == typeof(Exception))
                {
                    valueTo = null;
                    var cmdMsg = response.Report.FirstOrDefault(x => x.Category == ECommandMessage.Error.ToString());
                    if (cmdMsg != null)
                    {
                        var e = new Exception(cmdMsg.Message);
                        if (!string.IsNullOrWhiteSpace(cmdMsg.Status)) e.Data.Add("CommandMessageStatus", cmdMsg.Status);
                        valueTo = e;
                    }
                    return true;
                }
                valueTo = string.IsNullOrWhiteSpace(response.Result) ? f.sys.defaultof(typeTo) : response.Result.As<SimpleXml>().As(typeTo);
                return true;
            }
            if (f.sys.isof(typeTo, typeof(ICommand)))
            {
                if (valueFr is CommandRequest request)
                {
                    valueTo = f.sys.commandof(request.Command);
                    var property = typeTo.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public);
                    if (property != null) property.SetValue(valueTo, request.Value.As<SimpleXml>().As(property.PropertyType));
                    return true;
                }
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region exceptions

        private static bool TryException(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (valueFr is Exception arg)
            {
                if (typeTo == typeof(string))
                {
                    valueTo = (new CommandMessage(arg)).Message;
                    return true;
                }
                if (typeTo == typeof(CommandMessage))
                {
                    valueTo = (new CommandMessage(arg));
                    return true;
                }
                if (typeTo == typeof(CommandMessage[]))
                {
                    valueTo = (new [] { new CommandMessage(arg) });
                    return true;
                }
            }
            if (typeTo == typeof(Exception))
            {
                if (typeFr == typeof(string))
                {
                    valueTo = (new Exception(valueFr.ToString()));
                    return true;
                }
                if (valueFr is CommandMessage msg)
                {
                    var e = new Exception(msg.Message);
                    if (!string.IsNullOrWhiteSpace(msg.Status)) e.Data.Add("CommandMessageStatus", msg.Status);
                    valueTo = e;
                    return true;
                }
                if (valueFr is CommandMessage[] messages)
                {
                    valueTo = null;
                    var cmdMsg = messages.FirstOrDefault(x => x.Category == ECommandMessage.Error.ToString());
                    if (cmdMsg != null)
                    {
                        var e = new Exception(cmdMsg.Message);
                        if (!string.IsNullOrWhiteSpace(cmdMsg.Status)) e.Data.Add("CommandMessageStatus", cmdMsg.Status);
                        valueTo = e;
                    }
                    return true;
                }
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region streams

        private static bool TryStream(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (valueFr is Stream stream && typeTo != typeof(byte[]) && typeTo != typeof(string))
            {
                using (stream)
                {
                    var bin = new BinaryFormatter();
                    stream.Seek(0, SeekOrigin.Begin);
                    valueTo = bin.Deserialize(stream);
                    return true;
                }
            }
            if (typeof(Stream).IsAssignableFrom(typeTo) && typeFr != typeof(byte[]) && typeFr != typeof(string))
            {
                stream = (Stream)Activator.CreateInstance(typeTo, argsTo);
                var bin = new BinaryFormatter();
                bin.Serialize(stream, valueFr);
                valueTo = stream;
                return true;
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region bools

        private static bool TryBool(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (valueFr is bool arg)
            {
                if (!f.sys.issimple(valueFr.GetType()))
                {
                    valueTo = arg ? Activator.CreateInstance(typeTo, argsTo) : f.sys.defaultof(typeTo);
                    return true;
                }
                if (typeTo == typeof(string))
                {
                    valueTo = arg ? "1" : "0";
                    return true;
                }
                if (typeTo == typeof(DateTime) || typeTo == typeof(DateTime?))
                {
                    valueTo = arg ? DateTime.Now : DateTime.FromOADate(0);
                    return true;
                }
                if (typeTo == typeof(Guid) || typeTo == typeof(Guid?))
                {
                    valueTo = arg ? Guid.NewGuid() : Guid.Empty;
                    return true;
                }
                if (IsNumeric(typeTo) || typeTo.IsEnum)
                {
                    valueTo = CastNumeric(arg ? 1 : 0, typeTo);
                    return true;
                }
            }
            if (typeTo == typeof(bool) || typeTo == typeof(bool?))
            {
                if (!f.sys.issimple(valueFr.GetType()))
                {
                    valueTo = true;
                    return true;
                }
                if (typeFr == typeof(string))
                {
                    var s = valueFr.ToString().ToLower().Trim(' ', '\n', '\r', '\t');
                    valueTo = (!string.IsNullOrWhiteSpace(s) && (s == "yes" || s == "да" || s == "1" || s == "true"));
                    return true;
                }
                if (typeFr == typeof(DateTime) || typeFr == typeof(DateTime?))
                {
                    valueTo = ((DateTime)valueFr).ToOADate() != 0;
                    return true;
                }
                if (typeFr == typeof(Guid) || typeFr == typeof(Guid?))
                {
                    valueTo = (Guid)valueFr != Guid.Empty;
                    return true;
                }
                if (IsNumeric(typeFr) || typeFr.IsEnum)
                {
                    valueTo = CastNumeric<long>(valueFr) != 0;
                    return true;
                }
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region dates

        private static bool TryDate(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (valueFr is DateTime arg)
            {
                if (IsNumeric(typeTo))
                {
                    valueTo = CastNumeric(((DateTime)valueFr).ToOADate(), typeTo);
                    return true;
                }
            }
            if (typeTo == typeof(DateTime) || typeTo == typeof(DateTime?))
            {
                if (IsNumeric(typeFr))
                {
                    valueTo = DateTime.FromOADate(CastNumeric<double>(valueFr));
                    return true;
                }
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region enums

        private static bool TryEnum(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (typeFr.IsEnum)
            {
                if (typeTo == typeof(string))
                {
                    valueTo = valueFr.ToString();
                    return true;
                }
                if (IsNumeric(typeTo))
                {
                    valueTo = CastNumeric((long)valueFr, typeTo);
                    return true;
                }
            }
            if (typeTo.IsEnum)
            {
                if (typeFr == typeof(string))
                {
                    valueTo = Enum.Parse(typeTo, valueFr.ToString(), true);
                    return true;
                }
                if (IsNumeric(typeFr))
                {
                    valueTo = CastNumeric((int)valueFr, typeTo);
                    return true;
                }
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        #endregion

        #region drawing

        private static bool TryImage(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (valueFr is Image image)
            {
                if (typeTo == typeof(Icon))
                {
                    valueTo = ImgToIcn(image);
                    return true;
                }
                if (typeTo == typeof(byte[]))
                {
                    valueTo = ImgToBts(image);
                    return true;
                }
                if (typeTo == typeof(string))
                {
                    valueTo = System.Convert.ToBase64String(ImgToBts(image), Base64FormattingOptions.None);
                    return true;
                }
            }
            if (typeTo == typeof(Image))
            {
                if (valueFr is Icon icon)
                {
                    valueTo = IcnToImg(icon);
                    return true;
                }
                if (valueFr is byte[] bytes)
                {
                    valueTo = BtsToImg(bytes);
                    return true;
                }
                if (valueFr is string chars)
                {
                    valueTo = BtsToImg(System.Convert.FromBase64String(chars));
                    return true;
                }
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        private static bool TryIcon(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (valueFr is Icon icon)
            {
                if (typeTo == typeof(byte[]))
                {
                    valueTo = IcnToBts(icon);
                    return true;
                }
                if (typeTo == typeof(string))
                {
                    valueTo = System.Convert.ToBase64String(IcnToBts(icon), Base64FormattingOptions.None);
                    return true;
                }
            }
            if (typeTo == typeof(Icon))
            {
                if (valueFr is byte[] bytes)
                {
                    valueTo = BtsToIcn(bytes);
                    return true;
                }
                if (valueFr is string chars)
                {
                    valueTo = BtsToIcn(System.Convert.FromBase64String(chars));
                    return true;
                }
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        private static Icon ImgToIcn(Image img)
        {
            var bmp = new Bitmap(img, new Size(img.Size.Width, img.Size.Height));
            bmp.MakeTransparent(Color.Magenta);
            var hIcon = bmp.GetHicon();
            return Icon.FromHandle(hIcon);
        }

        private static Image IcnToImg(Icon icn)
        {
            return icn.ToBitmap();
        }

        private static byte[] IcnToBts(Icon icn)
        {
            using (var ms = new MemoryStream())
            {
                icn.Save(ms);
                return ms.ToArray();
            }
        }

        private static Icon BtsToIcn(byte[] bts)
        {
            using (var ms = new MemoryStream(bts))
            {
                return new Icon(ms);
            }
        }

        private static byte[] ImgToBts(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private static Image BtsToImg(byte[] bts)
        {
            using (var ms = new MemoryStream(bts))
            {
                return Image.FromStream(ms, true);
            }
        }

        #endregion

        #region cast numerics

        private static object CastNumeric(object valueFr, Type typeTo) { return (CastNumericMethodOf(typeTo) ?? CastNumericMethod.MakeGenericMethod(typeTo)).Invoke(null, new[] { valueFr }); }
        private static void CastNumericMethodsAddType(Type type) { CastNumericMethods.Add(type, CastNumericMethod.MakeGenericMethod(type)); }
        private static readonly Dictionary<Type, MethodInfo> CastNumericMethods = new Dictionary<Type, MethodInfo>();
        private static readonly MethodInfo[] CastNumericMethodHolder = new MethodInfo[] { null };

        private static bool TryNum(Type typeFr, Type typeTo, object valueFr, out object valueTo, params object[] argsTo)
        {
            if (IsNumeric(typeFr) && IsNumeric(typeTo))
            {
                var castNumericMethod = CastNumericMethodOf(typeTo) ?? CastNumericMethod.MakeGenericMethod(typeTo);
                valueTo = castNumericMethod.Invoke(null, new[] { valueFr });
                return true;
            }
            valueTo = f.sys.defaultof(typeTo);
            return false;
        }

        private static MethodInfo CastNumericMethodOf(Type type)
        {
            if (CastNumericMethods.Count == 0)
            {
                lock (CastNumericMethods)
                {
                    if (CastNumericMethods.Count == 0)
                    {
                        CastNumericMethodsAddType(typeof(decimal));
                        CastNumericMethodsAddType(typeof(decimal?));
                        CastNumericMethodsAddType(typeof(float));
                        CastNumericMethodsAddType(typeof(float?));
                        CastNumericMethodsAddType(typeof(double));
                        CastNumericMethodsAddType(typeof(double?));
                        CastNumericMethodsAddType(typeof(long));
                        CastNumericMethodsAddType(typeof(long?));
                        CastNumericMethodsAddType(typeof(ulong));
                        CastNumericMethodsAddType(typeof(ulong?));
                        CastNumericMethodsAddType(typeof(int));
                        CastNumericMethodsAddType(typeof(int?));
                        CastNumericMethodsAddType(typeof(uint));
                        CastNumericMethodsAddType(typeof(uint?));
                        CastNumericMethodsAddType(typeof(short));
                        CastNumericMethodsAddType(typeof(short?));
                        CastNumericMethodsAddType(typeof(ushort));
                        CastNumericMethodsAddType(typeof(ushort?));
                        CastNumericMethodsAddType(typeof(byte));
                        CastNumericMethodsAddType(typeof(byte?));
                        CastNumericMethodsAddType(typeof(sbyte));
                        CastNumericMethodsAddType(typeof(sbyte?));
                    }
                }
            }
            return CastNumericMethods.ContainsKey(type) ? CastNumericMethods[type] : null;
        }

        private static MethodInfo CastNumericMethod
        {
            get
            {
                if (CastNumericMethodHolder[0] == null)
                {
                    lock (CastNumericMethodHolder)
                    {
                        if (CastNumericMethodHolder[0] == null) CastNumericMethodHolder[0] = typeof(Cast).GetMethods(BindingFlags.Static | BindingFlags.NonPublic).FirstOrDefault(x => x.Name == "CastNumeric");
                    }
                }
                return CastNumericMethodHolder[0];
            }
        }

        private static T CastNumeric<T>(object value)
        {
            try
            {
                dynamic o = value;
                return (T)o;
            }
            catch
            {
                return default(T);
            }
        }

        private static bool IsNumeric(Type type)
        {
            return false
            || type == typeof(decimal) || type == typeof(decimal?) || type == typeof(float) || type == typeof(float?) || type == typeof(double) || type == typeof(double?)
            || type == typeof(long) || type == typeof(long?) || type == typeof(ulong) || type == typeof(ulong?)
            || type == typeof(int) || type == typeof(int?) || type == typeof(uint) || type == typeof(uint?)
            || type == typeof(short) || type == typeof(short?) || type == typeof(ushort) || type == typeof(ushort?)
            || type == typeof(byte) || type == typeof(byte?) || type == typeof(sbyte) || type == typeof(sbyte?);
        }

        private static bool IsNumeric(object value)
        {
            return false
            || value is decimal || value is float || value is double
            || value is long || value is ulong
            || value is int || value is uint
            || value is short || value is ushort
            || value is byte || value is sbyte;
        }

        #endregion
    }

    public abstract class Converter : IConverter
    {
        private static readonly CultureInfo _culture = new CultureInfo("en-us") { NumberFormat = { NumberGroupSeparator = string.Empty, NumberDecimalSeparator = "." }, DateTimeFormat = { DateSeparator = "-", TimeSeparator = ":" } };
        private string ValueAsCharsBase(object value) { return value is Converter converter ? converter.ValueAsChars(converter._value) : ValueAsChars(value); }
        private byte[] ValueAsBytesBase(object value) { return value is Converter converter ? converter.ValueAsBytes(converter._value) : ValueAsBytes(value); }
        private string Chars { get { return _chars ?? (_chars = _bytes != null ? BytesAsChars(_bytes) : ValueAsCharsBase(_value)); } }
        private byte[] Bytes { get { return _bytes ?? (_bytes = _chars != null ? CharsAsBytes(_chars) : ValueAsBytesBase(_value)); } }
        protected virtual string BytesAsChars(byte[] bytes) { return Encoder.GetString(bytes); }
        protected virtual byte[] CharsAsBytes(string chars) { return Encoder.GetBytes(chars); }
        protected abstract object CharsAsValue(string chars, Type typeTo);
        protected abstract object BytesAsValue(byte[] bytes, Type typeTo);
        protected abstract string ValueAsChars(object value);
        protected abstract byte[] ValueAsBytes(object value);
        protected virtual Encoding Encoder => Encoding.UTF8;
        protected virtual CultureInfo Culture => _culture;
        private byte[] _bytes;
        private string _chars;
        private object _value;

        void IConverter.Is(object value)
        {
            _bytes = null;
            _chars = null;
            _value = null;
            if (value is string s) _chars = s;
            if (value is StringBuilder sb) _chars = sb.ToString();
            if (value is char[] chars) _chars = new string(chars);
            if (value is IEnumerable<char> listChars) _chars = string.Concat(listChars);
            if (value is byte[] bytes) _bytes = bytes;
            if (value is Stream stream) _bytes = StreamToBytes(stream);
            if (value is IEnumerable<byte> listBytes) _bytes = listBytes.ToArray();
            if (_chars == null && _bytes == null) _value = value;
        }

        object IConverter.As(Type typeTo, params object[] argsTo)
        {
            if (_value is IConverter converter) return converter.As(typeTo, argsTo);
            if (TryString(typeTo, out object valueTo, argsTo)) return valueTo;
            if (TryStringBuilder(typeTo, out valueTo, argsTo)) return valueTo;
            if (TryCharArray(typeTo, out valueTo, argsTo)) return valueTo;
            if (TryCharList(typeTo, out valueTo, argsTo)) return valueTo;
            if (TryBytes(typeTo, out valueTo, argsTo)) return valueTo;
            if (TryStream(typeTo, out valueTo, argsTo)) return valueTo;
            if (TryByteList(typeTo, out valueTo, argsTo)) return valueTo;
            if (_chars != null) return CharsAsValueBase(_chars, typeTo, argsTo);
            if (_bytes != null) return BytesAsValueBase(_bytes, typeTo, argsTo);
            return _value.As(typeTo, argsTo);
        }

        private bool TryString(Type typeTo, out object valueTo, params object[] argsTo)
        {
            var result = typeTo == typeof(string);
            valueTo = result ? Chars : f.sys.defaultof(typeTo);
            return result;
        }

        private bool TryStringBuilder(Type typeTo, out object valueTo, params object[] argsTo)
        {
            var result = typeTo == typeof(StringBuilder);
            valueTo = result ? new StringBuilder(Chars) : f.sys.defaultof(typeTo);
            return result;
        }

        private bool TryCharArray(Type typeTo, out object valueTo, params object[] argsTo)
        {
            var result = typeTo == typeof(char[]);
            valueTo = result ? Chars.ToCharArray() : f.sys.defaultof(typeTo);
            return result;
        }

        private bool TryCharList(Type typeTo, out object valueTo, params object[] argsTo)
        {
            valueTo = f.sys.defaultof(typeTo);
            var genericTypes = typeTo.GetGenericArguments();
            var genericType = genericTypes.Length == 1 && genericTypes[0] == typeof(char) ? genericTypes[0] : null;
            if (genericType == null) return false;
            var definitionType = typeTo.GetGenericTypeDefinition();
            if (definitionType != typeof(List<>) && definitionType != typeof(IList<>)) return false;
            valueTo = Activator.CreateInstance(typeTo);
            var list = (IList)valueTo;
            foreach (char c in Chars) list.Add(c);
            return true;
        }

        private bool TryBytes(Type typeTo, out object valueTo, params object[] argsTo)
        {
            var result = typeTo == typeof(byte[]);
            valueTo = result ? Bytes : f.sys.defaultof(typeTo);
            return result;
        }

        private bool TryByteList(Type typeTo, out object valueTo, params object[] argsTo)
        {
            valueTo = f.sys.defaultof(typeTo);
            var genericTypes = typeTo.GetGenericArguments();
            var genericType = genericTypes.Length == 1 && genericTypes[0] == typeof(byte) ? genericTypes[0] : null;
            if (genericType == null) return false;
            var definitionType = typeTo.GetGenericTypeDefinition();
            if (definitionType != typeof(List<>) && definitionType != typeof(IList<>)) return false;
            valueTo = Activator.CreateInstance(typeTo);
            var list = (IList)valueTo;
            foreach (byte b in Bytes) list.Add(b);
            return true;
        }

        private bool TryStream(Type typeTo, out object valueTo, params object[] argsTo)
        {
            var result = typeof(Stream).IsAssignableFrom(typeTo);
            valueTo = result ? BytesToStream(Bytes, typeTo, argsTo) : f.sys.defaultof(typeTo);
            return result;
        }

        private object CharsAsValueBase(string chars, Type typeTo, params object[] argsTo)
        {
            var converter = Activator.CreateInstance(typeTo, argsTo) as Converter;
            if (converter == null) return CharsAsValue(chars, typeTo);
            ((IConverter)converter).Is(chars);
            return converter;
        }

        private object BytesAsValueBase(byte[] bytes, Type typeTo, params object[] argsTo)
        {
            var converter = Activator.CreateInstance(typeTo, argsTo) as Converter;
            if (converter == null) return BytesAsValue(bytes, typeTo);
            ((IConverter)converter).Is(bytes);
            return converter;
        }

        private static byte[] StreamToBytes(Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        private static object BytesToStream(byte[] bytes, Type typeTo, params object[] argsTo)
        {
            var stream = Activator.CreateInstance(typeTo, argsTo) as Stream;
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }

    public interface IConverter
    {
        void Is(object valueFr);
        object As(Type typeTo, params object[] argsTo);
    }
}