using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Converts
{
    public static class SqlExtensions
    {
        public static SqlDbType classof(this f.ISqlExpander exp, Type type)
        {
            if (type.IsEnum
            || type == typeof(bool)
            || type == typeof(bool?)
            || type == typeof(byte)
            || type == typeof(byte?)
            || type == typeof(sbyte)
            || type == typeof(sbyte?)
            || type == typeof(short)
            || type == typeof(short?)
            || type == typeof(ushort)
            || type == typeof(ushort?)
            || type == typeof(int)
            || type == typeof(int?)
            || type == typeof(uint)
            || type == typeof(uint?)
            || type == typeof(long)
            || type == typeof(long?)
            || type == typeof(ulong)
            || type == typeof(ulong?)) return SqlDbType.BigInt;
            if (type == typeof(decimal)
            || type == typeof(decimal?)
            || type == typeof(float)
            || type == typeof(float?)
            || type == typeof(double)
            || type == typeof(double?)) return SqlDbType.Money;
            if (type == typeof(Guid) || type == typeof(Guid?)) return SqlDbType.UniqueIdentifier;
            if (type == typeof(DateTime) || type == typeof(DateTime?)) return SqlDbType.DateTime;
            if (type == typeof(char) || type == typeof(char?)) return SqlDbType.NChar;
            if (type == typeof(byte[])) return SqlDbType.VarBinary;
            if (type == typeof(string)) return SqlDbType.NVarChar;
            return SqlDbType.NVarChar;
        }

        public static object valueof(this f.ISqlExpander exp, object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            var type = value.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(MssqlCommandParameterOutput<>))
            {
                return type.GetProperty("Value", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(value, null);
            }
            if (!f.sys.issimple(type))
            {
                return value.As<SimpleXml>().As<string>();
            }
            if (value is Guid guid)
            {
                if (guid == Guid.Empty) return DBNull.Value;
            }
            if (value is DateTime date)
            {
                if (SqlDateTime.MinValue.Value >= date || date >= SqlDateTime.MaxValue.Value) return DBNull.Value;
            }
            if (value.Equals(f.sys.defaultof(value.GetType())))
            {
                return DBNull.Value;
            }
            if (type.IsEnum || type == typeof(bool) || type == typeof(bool?) || type == typeof(byte) || type == typeof(byte?) || type == typeof(sbyte) || type == typeof(sbyte?) || type == typeof(short) || type == typeof(short?) || type == typeof(ushort) || type == typeof(ushort?) || type == typeof(int) || type == typeof(int?) || type == typeof(uint) || type == typeof(uint?) || type == typeof(long) || type == typeof(long?) || type == typeof(ulong) || type == typeof(ulong?))
            {
                return (long)value;
            }
            if (type == typeof(decimal) || type == typeof(decimal?) || type == typeof(float) || type == typeof(float?) || type == typeof(double) || type == typeof(double?))
            {
                return (decimal)value;
            }
            return value;
        }

        public static bool isout(this f.ISqlExpander exp, ref Type type)
        {
            var geType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(MssqlCommandParameterOutput<>) ? f.sys.genericof(type) : null;
            type = geType ?? type;
            return geType != null;
        }
    }

    public class SqlConverter : Converter
    {
        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        public SqlConverter(Encoding encoder = null) { Encoder = encoder ?? base.Encoder; }
        protected override Encoding Encoder { get; }

        protected override string ValueAsChars(object value)
        {
            return value != null ? Encoder.GetString(ValueAsBytes(value)).Remove(0, ByteOrderMarkUtf8.Length) : string.Empty;
        }

        protected override byte[] ValueAsBytes(object value)
        {
            if (value == null) return new byte[0];
            var ds = value is DataSet dataset ? dataset : (value is DataTable dt ? DataSetOf(dt) : DataSetOf(value));
            using (var ms = new MemoryStream())
            {
                var xtw = new NoDeclarationXmlTextWriter(ms, Encoder);
                xtw.Formatting = Formatting.None;
                xtw.Namespaces = true;
                var xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                var xw = XmlWriter.Create(xtw, xws);
                ds.WriteXml(xw, XmlWriteMode.WriteSchema);
                return ms.ToArray();
            }
        }

        protected override object CharsAsValue(string chars, Type typeTo)
        {
            if (string.IsNullOrWhiteSpace(chars)) return f.sys.defaultof(typeTo);
            var utf16 = Encoding.GetEncoding("utf-16");
            var bytes = utf16.GetBytes(chars);
            if (Encoder.EncodingName != utf16.EncodingName) bytes = Encoding.Convert(utf16, Encoder, bytes);
            return BytesAsValue(bytes, typeTo);
        }

        protected override object BytesAsValue(byte[] bytes, Type typeTo)
        {
            var ds = DataSetOf(bytes);
            if (typeTo == typeof(DataSet)) return ds;
            if (typeTo == typeof(DataTable)) return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
            return (new EntityWriter(ds.CreateDataReader(), typeTo)).Write();
        }

        private static DataSet DataSetOf(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            using (var ms = new MemoryStream(bytes))
            {
                var xtr = new NoDeclarationXmlTextReader(ms, Encoding.UTF8);
                xtr.Namespaces = false;
                var xrs = new XmlReaderSettings();
                xrs.IgnoreComments = true;
                xrs.IgnoreProcessingInstructions = true;
                xrs.IgnoreWhitespace = true;
                var xr = XmlReader.Create(xtr, xrs);
                var ds = new DataSet { EnforceConstraints = false };
                ds.ReadXml(xr, XmlReadMode.ReadSchema);
                return ds;
            }
        }

        private static DataSet DataSetOf(DataTable dt)
        {
            var ds = new DataSet { EnforceConstraints = false };
            ds.Tables.Add(dt);
            return ds;
        }

        private static DataSet DataSetOf(object value)
        {
            var ds = new DataSet();
            var reader = new EntityReader(value);
            ds.Load(reader, LoadOption.PreserveChanges, reader.GetTableNames());
            return ds;
        }

        private class NoDeclarationXmlTextWriter : XmlTextWriter
        {
            public NoDeclarationXmlTextWriter(string filename, Encoding encoding) : base(filename, encoding) { }
            public NoDeclarationXmlTextWriter(Stream w, Encoding encoding) : base(w, encoding) { }
            public NoDeclarationXmlTextWriter(TextWriter w) : base(w) { }
            public override void WriteStartDocument(bool standalone) { }
            public override void WriteStartDocument() { }
        }

        private class NoDeclarationXmlTextReader : XmlTextReader
        {
            public NoDeclarationXmlTextReader(string fileName) : base(new FileStream(fileName, FileMode.OpenOrCreate)) { }
            public NoDeclarationXmlTextReader(Stream r, Encoding encoding) : base(new StreamReader(r, encoding)) { }
            public NoDeclarationXmlTextReader(TextReader r) : base(r) { }
        }
    }
}

namespace Jetproger.Tools.AppConfig
{
    public class ConnectionString : ConfigSetting
    {
        public ConnectionString() : base("")
        {
            var connectionString = Value;
            try
            {
                var csb = new SqlConnectionStringBuilder(connectionString);
                csb.AsynchronousProcessing = true;
                csb.MultipleActiveResultSets = true;
                Value = csb.ToString();
            }
            catch
            {
                Value = connectionString;
            }
        }
    }
}