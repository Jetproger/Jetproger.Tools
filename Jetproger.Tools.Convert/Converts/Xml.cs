using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Converts
{
    public static class XmlExtensions
    {
        private static readonly XmlConverter Converter = t<XmlConverter>.one();

        public static string of(this f.IXmlExpander e, object value, XmlConverter converter = null)
        {
            return (converter ?? Converter).SerializeToString(value);
        }

        public static TResult of<TResult>(this f.IXmlExpander e, object value, XmlConverter converter = null)
        {
            var type = typeof(TResult);
            converter = converter ?? Converter;
            if (type == typeof(string)) return (TResult)(object)converter.SerializeToString(value);
            if (type == typeof(byte[])) return (TResult)(object)converter.SerializeToBytes(value);
            throw new XmlConverterInvalidResultTypeException();
        }

        public static TResult of<TResult, TConverter>(this f.IXmlExpander e, object value) where TConverter : XmlConverter
        {
            var type = typeof(TResult);
            var converter = f.sys.valueof<TConverter>();
            if (type == typeof(string)) return (TResult)(object)converter.SerializeToString(value);
            if (type == typeof(byte[])) return (TResult)(object)converter.SerializeToBytes(value);
            throw new XmlConverterInvalidResultTypeException();
        }

        public static object to(this f.IXmlExpander e, string s, Type type, XmlConverter converter = null)
        {
            return (converter ?? Converter).Deserialize(s, type);
        }

        public static TResult to<TResult>(this f.IXmlExpander e, string s, XmlConverter converter = null)
        {
            return (TResult)(converter ?? Converter).Deserialize(s, typeof(TResult));
        }

        public static TResult to<TResult, TConverter>(this f.IXmlExpander e, string s) where TConverter : XmlConverter
        {
            return (TResult)f.sys.valueof<TConverter>().Deserialize(s, typeof(TResult));
        }

        public static object to(this f.IXmlExpander e, byte[] bytes, Type type, XmlConverter converter = null)
        {
            return (converter ?? Converter).Deserialize(bytes, type);
        }

        public static TResult to<TResult>(this f.IXmlExpander e, byte[] bytes, XmlConverter converter = null)
        {
            return (TResult)(converter ?? Converter).Deserialize(bytes, typeof(TResult));
        }

        public static TResult to<TResult, TConverter>(this f.IXmlExpander e, byte[] bytes) where TConverter : XmlConverter
        {
            return (TResult)f.sys.valueof<TConverter>().Deserialize(bytes, typeof(TResult));
        }

        public static string op(this f.IXmlExpander e, string xml, string xslt)
        {
            using (var sw = new StringWriter())
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                var xpath = new XPathDocument(new XmlNodeReader(doc));
                var transformer = new XslCompiledTransform();
                var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit };
                using (var sr = new StringReader(xslt))
                {
                    using (var reader = XmlReader.Create(sr, settings))
                    {
                        transformer.Load(reader);
                        transformer.Transform(xpath, null, sw);
                        return sw.ToString();
                    }
                }
            }
        }

        public static string ValidateXml(this f.IXmlExpander e, string xml, string xsd)
        {
            var error = new StringBuilder();
            var doc = new XmlDocument();
            var xsdReader = new StringReader(xsd);
            var schema = XmlSchema.Read(xsdReader, (x, y) => error.AppendLine(y.Message));
            doc.Schemas.Add(schema);
            try
            {
                doc.LoadXml(xml);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
            doc.Validate((x, y) => error.AppendLine(y.Message));
            return error.ToString();
        }

        public static T GetNode<T>(this f.IXmlExpander e, XmlNode root, int index)
        {
            return GetNode(e, root, index).As<T>();
        }

        public static object GetNode(this f.IXmlExpander e, XmlNode root, int index)
        {
            return root != null && index >= 0 && index < root.ChildNodes.Count ? root.ChildNodes[index].InnerText : null;
        }

        public static T GetNode<T>(this f.IXmlExpander e, XmlNode root, string name)
        {
            return GetNode(e, root, name).As<T>();
        }

        public static object GetNode(this f.IXmlExpander e, XmlNode root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return null;
            var node = root.SelectSingleNode(name);
            return node?.InnerText;
        }

        public static T GetNode<T>(this f.IXmlExpander e, XmlDocument doc, string xpath)
        {
            return GetNode(e, doc, xpath).As<T>();
        }

        public static object GetNode(this f.IXmlExpander e, XmlDocument doc, string xpath)
        {
            if (doc == null || string.IsNullOrWhiteSpace(xpath)) return null;
            var node = doc.SelectSingleNode(xpath);
            return node?.InnerText;
        }

        public static void SetNode(this f.IXmlExpander e, XmlNode root, int index, object value)
        {
            if (root == null || value == null || index < 0 && index >= root.ChildNodes.Count) return;
            var node = root.ChildNodes[index];
            if (node != null) node.InnerText = value.As<string>();
        }

        public static void SetNode(this f.IXmlExpander e, XmlNode root, string name, object value)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return;
            var node = root.SelectSingleNode(name);
            if (node != null) node.InnerText = value.As<string>();
        }

        public static void SetNode(this f.IXmlExpander e, XmlDocument doc, string xpath, object value)
        {
            var node = doc.SelectSingleNode(xpath) ?? AddNode(e, doc, xpath);
            node.InnerText = value.As<string>();
        }

        public static XmlNode AddNode(this f.IXmlExpander e)
        {
            var doc = new XmlDocument();
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "XML", null);
            doc.AppendChild(node);
            return node;
        }

        public static XmlNode AddNode(this f.IXmlExpander e, XmlDocument doc, string xpath)
        {
            string[] data = xpath.Split('/');
            var current = (XmlNode)doc.DocumentElement;
            string path = "";
            foreach (string name in data)
            {
                path = path + "/" + name;
                var node = doc.SelectSingleNode(path) ?? AddNode(e, current, name, "");
                current = node;
            }
            return current;
        }

        public static XmlNode AddNode(this f.IXmlExpander e, XmlNode root, string name)
        {
            return AddNode(e, root, name, "");
        }

        public static XmlNode AddNode(this f.IXmlExpander e, XmlNode root, string name, object value, string namespaceUri = null)
        {
            if (value == null || value == DBNull.Value) return null;
            var doc = root.OwnerDocument ?? (XmlDocument)root;
            var node = doc.CreateNode(XmlNodeType.Element, name, namespaceUri);
            node.InnerText = f.str.of(value);
            root.AppendChild(node);
            return node;
        }

        public static XmlNode AddAttr(this f.IXmlExpander e, XmlNode root, string name, object value)
        {
            if (value == null || value == DBNull.Value) return null;
            var doc = root.OwnerDocument ?? (XmlDocument)root;
            var attr = doc.CreateAttribute(name);
            attr.InnerText = f.str.of(value);
            root.Attributes?.Append(attr);
            return root;
        }

        public static T GetAttr<T>(this f.IXmlExpander e, XmlNode root, int index)
        {
            return GetAttr(e, root, index).As<T>();
        }

        public static object GetAttr(this f.IXmlExpander e, XmlNode root, int index)
        {
            return root != null && root.Attributes != null && index >= 0 && index < root.Attributes.Count ? root.Attributes[index].InnerText : null;
        }

        public static T GetAttr<T>(this f.IXmlExpander e, XmlNode root, string name)
        {
            return GetAttr(e, root, name).As<T>();
        }

        public static object GetAttr(this f.IXmlExpander e, XmlNode root, string name)
        {
            if (root == null || root.Attributes == null || string.IsNullOrWhiteSpace(name)) return null;
            var attr = root.Attributes[name];
            return attr?.InnerText;
        }

        public static void SetAttr(this f.IXmlExpander e, XmlNode root, int index, object value)
        {
            if (root == null || root.Attributes == null || index < 0 && index >= root.Attributes.Count) return;
            var attr = root.Attributes[index];
            if (attr != null) attr.InnerText = value.As<string>();
        }

        public static void SetAttr(this f.IXmlExpander e, XmlNode root, string name, object value)
        {
            if (root == null || root.Attributes == null || string.IsNullOrWhiteSpace(name)) return;
            var attr = root.Attributes[name];
            if (attr != null) attr.InnerText = value.As<string>();
        }
    }

    public class KizXmlConverter : XmlConverter
    {
        public override string SerializeToString(object value)
        {
            return Encoding.UTF8.GetString(SerializeToBytes(value));
        }

        public override byte[] SerializeToBytes(object value)
        {
            if (value == null)  return null;
            using (var ms = new MemoryStream())
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("xs", "http://www.w3.org/2001/XMLSchema");
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                var xs = new XmlSerializer(value.GetType());
                var xtw = new XmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = Formatting.Indented;
                xtw.Namespaces = true;
                var xws = new XmlWriterSettings();
                xws.NewLineHandling = NewLineHandling.Replace;
                xws.NewLineChars = "\n";
                var xw = XmlWriter.Create(xtw, xws);
                xs.Serialize(xw, value, ns);
                return ms.ToArray();
            }
        }

        public override object Deserialize(string xml, Type type)
        {
            return !string.IsNullOrWhiteSpace(xml) ? Deserialize(Encoding.UTF8.GetBytes(xml), type) : Deserialize(new byte[0], type);
        }

        public override object Deserialize(byte[] bytes, Type type)
        {
            if (bytes == null || bytes.Length == 0) return f.sys.defaultof(type);
            using (var ms = new MemoryStream(bytes))
            {
                var xs = new XmlSerializer(type);
                var xtr = new XmlTextReader(new StreamReader(ms, Encoding.UTF8));
                xtr.Namespaces = false;
                var xrs = new XmlReaderSettings();
                xrs.IgnoreComments = true;
                xrs.IgnoreProcessingInstructions = true;
                xrs.IgnoreWhitespace = true;
                var xr = XmlReader.Create(xtr, xrs);
                return xs.Deserialize(xr);
            }
        }
    }

    public class XmlConverter
    {
        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        public virtual string SerializeToString(object o)
        {
            return Encoding.UTF8.GetString(SerializeToBytes(o)).Remove(0, ByteOrderMarkUtf8.Length);
        }

        public virtual byte[] SerializeToBytes(object o)
        {
            if (o == null) return null;
            var ds = o as DataSet;
            if (ds != null) return SerializeDataSet(ds);
            var dt = o as DataTable;
            if (dt != null) return SerializeDataTable(dt);
            using (var ms = new MemoryStream())
            {
                var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var xs = new XmlSerializer(o.GetType());
                var xtw = new NoDeclarationXmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = Formatting.None;
                xtw.Namespaces = true;
                var xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                var xw = XmlWriter.Create(xtw, xws);
                xs.Serialize(xw, o, ns);
                return ms.ToArray();
            }
        }

        public virtual object Deserialize(string xml, Type type)
        {
            if (string.IsNullOrWhiteSpace(xml)) return null;
            if (type == typeof(DataSet)) return DeserializeDataSet(xml);
            if (type == typeof(DataTable)) return DeserializeDataTable(xml);
            return !string.IsNullOrWhiteSpace(xml) ? Deserialize(Encoding.UTF8.GetBytes(xml), type) : Deserialize(new byte[0], type);
        }

        public virtual object Deserialize(byte[] bytes, Type type)
        {
            if (bytes == null || bytes.Length == 0) return null;
            if (type == typeof(DataSet)) return DeserializeDataSet(bytes);
            if (type == typeof(DataTable)) return DeserializeDataTable(bytes);
            using (var ms = new MemoryStream(bytes))
            {
                var xs = new XmlSerializer(type);
                var xtr = new NoDeclarationXmlTextReader(ms, Encoding.UTF8);
                xtr.Namespaces = false;
                var xrs = new XmlReaderSettings();
                xrs.IgnoreComments = true;
                xrs.IgnoreProcessingInstructions = true;
                xrs.IgnoreWhitespace = true;
                var xr = XmlReader.Create(xtr, xrs);
                return xs.Deserialize(xr);
            }
        }

        private byte[] SerializeDataTable(DataTable dt)
        {
            if (dt == null) return null;
            var ds = new DataSet { EnforceConstraints = false };
            ds.Tables.Add(dt);
            return SerializeDataSet(ds);
        }

        private byte[] SerializeDataSet(DataSet ds)
        {
            if (ds == null) return null;
            using (var ms = new MemoryStream())
            {
                var xtw = new NoDeclarationXmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = Formatting.None;
                xtw.Namespaces = true;
                var xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                var xw = XmlWriter.Create(xtw, xws);
                ds.WriteXml(xw, XmlWriteMode.WriteSchema);
                return ms.ToArray();
            }
        }

        private DataTable DeserializeDataTable(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            var ds = DeserializeDataSet(bytes);
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        private DataSet DeserializeDataSet(byte[] bytes)
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

        private DataTable DeserializeDataTable(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return null;
            var ds = DeserializeDataSet(xml);
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        private DataSet DeserializeDataSet(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return null;
            using (var sr = new StringReader(xml))
            {
                var ds = new DataSet { EnforceConstraints = false };
                ds.ReadXml(sr);
                return ds;
            }
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
    public class XmlConverterInvalidResultTypeException : Exception
    {
        public XmlConverterInvalidResultTypeException() : base(@"The return type must be System.String or System.Byte[]") { }
    }
}