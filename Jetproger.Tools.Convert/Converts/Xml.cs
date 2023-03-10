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
        public static string xsltof(this f.IXmlExpander e, string xslt, string xml)
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

        public static string isxsd(this f.IXmlExpander e, string xsd, string xml)
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
            node.InnerText = value.As<string>();
            root.AppendChild(node);
            return node;
        }

        public static XmlNode AddAttr(this f.IXmlExpander e, XmlNode root, string name, object value)
        {
            if (value == null || value == DBNull.Value) return null;
            var doc = root.OwnerDocument ?? (XmlDocument)root;
            var attr = doc.CreateAttribute(name);
            attr.InnerText = value.As<string>();
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

    public class KizXml : Converter
    {
        private static readonly IConverter Sql = new SqlConverter();
        protected override object CharsAsValue(string chars, Type typeTo) { return BytesAsValue(!string.IsNullOrWhiteSpace(chars) ? Encoder.GetBytes(chars) : new byte[0], typeTo); }
        protected override string ValueAsChars(object value) { return Encoder.GetString(ValueAsBytes(value)); }
        
        public KizXml(Encoding encoder = null) { Encoder = encoder ?? base.Encoder; }
        protected override Encoding Encoder { get; }

        protected override byte[] ValueAsBytes(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is DataSet || value is DataTable)
            {
                Sql.Is(value);
                return (byte[])Sql.As(typeof(byte[]));
            }
            if (value is XmlDocument doc)
            {
                return Encoder.GetBytes(doc.InnerXml);
            }
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

        protected override object BytesAsValue(byte[] bytes, Type typeTo)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return f.sys.defaultof(typeTo);
            }
            if (typeTo == typeof(DataSet) || typeTo == typeof(DataTable))
            {
                Sql.Is(bytes);
                return Sql.As(typeTo);
            }
            if (typeTo == typeof(XmlDocument))
            {
                var doc = new XmlDocument();
                doc.LoadXml(Encoder.GetString(bytes));
                return doc;
            }
            using (var ms = new MemoryStream(bytes))
            {
                var xs = new XmlSerializer(typeTo);
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

    public class SimpleXml : Converter
    {
        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        private static readonly IConverter Sql = new SqlConverter();
        protected override object CharsAsValue(string chars, Type typeTo) { return BytesAsValue(!string.IsNullOrWhiteSpace(chars) ? Encoder.GetBytes(chars) : new byte[0], typeTo); }
        protected override string ValueAsChars(object value) { return Encoder.GetString(ValueAsBytes(value)).Remove(0, ByteOrderMarkUtf8.Length); }
        
        public SimpleXml(Encoding encoder = null) { Encoder = encoder ?? base.Encoder; }
        protected override Encoding Encoder { get; }

        protected override byte[] ValueAsBytes(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is DataSet || value is DataTable)
            {
                Sql.Is(value);
                return (byte[])Sql.As(typeof(byte[]));
            }
            if (value is XmlDocument doc)
            {
                return Encoder.GetBytes(doc.InnerXml);
            }
            using (var ms = new MemoryStream())
            {
                var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var xs = new XmlSerializer(value.GetType());
                var xtw = new NoDeclarationXmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = Formatting.None;
                xtw.Namespaces = true;
                var xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                var xw = XmlWriter.Create(xtw, xws);
                xs.Serialize(xw, value, ns);
                return ms.ToArray();
            }
        }

        protected override object BytesAsValue(byte[] bytes, Type typeTo)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return f.sys.defaultof(typeTo);
            }
            if (typeTo == typeof(DataSet) || typeTo == typeof(DataTable))
            {
                Sql.Is(bytes);
                return Sql.As(typeTo);
            }
            if (typeTo == typeof(XmlDocument))
            {
                var doc = new XmlDocument();
                doc.LoadXml(Encoder.GetString(bytes));
                return doc;
            }
            using (var ms = new MemoryStream(bytes))
            {
                var xs = new XmlSerializer(typeTo);
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