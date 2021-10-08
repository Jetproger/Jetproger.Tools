using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Converts
{
    public static class XmlExtensions
    {
        public static T Of<T>(this XmlExpander e, object value)
        {
            return Je<XmlConverter>.One(() => Je<XmlConverter>.New()).Serialize<T>(value);
        }

        public static string Of(this XmlExpander e, object value)
        {
            return Je<XmlConverter>.One(() => Je<XmlConverter>.New()).Serialize<string>(value);
        }

        public static T To<T>(this XmlExpander e, string s)
        {
            return (T)Je<XmlConverter>.One(() => Je<XmlConverter>.New()).Deserialize(s, typeof(T)); 
        }

        public static object To(this XmlExpander e, string s, Type type)
        {  
            return Je<XmlConverter>.One(() => Je<XmlConverter>.New()).Deserialize(s, type);
        }

        public static string Op(this XmlExpander exp, XmlDocument doc, string xsltScript)
        {
            using (var sw = new StringWriter())
            {
                var xpathDoc = new XPathDocument(new XmlNodeReader(doc));
                var xslt = new XslCompiledTransform();
                var rs = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit };
                using (var sr = new StringReader(xsltScript))
                {
                    using (var reader = XmlReader.Create(sr, rs))
                    {
                        xslt.Load(reader);
                        xslt.Transform(xpathDoc, null, sw);
                        return sw.ToString();
                    }
                }
            }
        }
    }

    public class XmlExpander
    {
        public XmlAttributeExpander Attribute => Je<XmlAttributeExpander>.One(() => Je<XmlAttributeExpander>.New());
        public XmlNodeExpander Node => Je<XmlNodeExpander>.One(() => Je<XmlNodeExpander>.New());
    }

    public class XmlNodeExpander
    { 
        public XmlNode Add(XmlNode root, string name)
        {
            return Add(root, name, "");
        }

        public XmlNode Add<T>(XmlNode root, string name, T value, string namespaceUri = null)
        {
            if (value == null) return null;
            XmlDocument doc = root.OwnerDocument ?? (XmlDocument)root;
            XmlNode node = doc.CreateNode(XmlNodeType.Element, name, namespaceUri);
            node.InnerText = Je.str.Of(value);
            root.AppendChild(node);
            return node;
        }

        public T Get<T>(XmlNode root, int index)
        {
            return (root != null && index >= 0 && index < root.ChildNodes.Count ? root.ChildNodes[index].InnerText : null).As<T>();
        }

        public T Get<T>(XmlNode root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return default(T);
            XmlNode node = root.SelectSingleNode(name);
            return (node?.InnerText).As<T>();
        }

        public void Set(XmlNode root, int index, object value)
        {
            if (root == null || value == null || index < 0 && index >= root.ChildNodes.Count) return;
            XmlNode node = root.ChildNodes[index];
            if (node != null) node.InnerText = value.As<string>();
        }

        public void Set(XmlNode root, string name, object value)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return;
            XmlNode node = root.SelectSingleNode(name);
            if (node != null) node.InnerText = value.As<string>();
        }

        public void Set( XmlDocument doc, string xmlPath, string value)
        {
            XmlNode node = doc.SelectSingleNode(xmlPath) ?? Create(doc, xmlPath);
            node.InnerText = value;
        }

        public XmlNode Create()
        {
            var doc = new XmlDocument();
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "XML", null);
            doc.AppendChild(node);
            return node;
        }

        public XmlNode Create(XmlDocument doc, string xpath)
        {
            string[] data = xpath.Split('/');
            XmlNode current = doc.DocumentElement;
            string path = "";
            foreach (string name in data)
            {
                path = path + "/" + name;
                XmlNode node = doc.SelectSingleNode(path) ?? Add(current, name, "");
                current = node;
            }
            return current;
        }
    }

    public class XmlAttributeExpander
    {
        public XmlNode Add<T>(XmlNode root, string name, T value)
        {
            if (value == null) return null;
            XmlDocument doc = root.OwnerDocument ?? (XmlDocument)root;
            XmlAttribute attr = doc.CreateAttribute(name);
            attr.InnerText = Je.str.Of(value);
            root.Attributes?.Append(attr);
            return root;
        }

        public T Get<T>(XmlNode root, int index)
        {
            return (root != null && root.Attributes != null && index >= 0 && index < root.Attributes.Count ? root.Attributes[index].InnerText : null).As<T>();
        }

        public T Get<T>(XmlNode root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name) || root.Attributes == null) return default(T);
            XmlAttribute attr = root.Attributes[name];
            return (attr?.InnerText).As<T>();
        }

        public void Set(XmlNode root, int index, object value)
        {
            if (root == null || value == null || root.Attributes == null || index < 0 && index >= root.Attributes.Count) return;
            XmlAttribute attr = root.Attributes[index];
            if (attr != null) attr.InnerText = value.As<string>();
        }

        public void Set(XmlNode root, string name, object value)
        {
            if (root == null || string.IsNullOrWhiteSpace(name) || root.Attributes == null) return;
            XmlAttribute attr = root.Attributes[name];
            if (attr != null) attr.InnerText = value.As<string>();
        }
    }

    public class KizXmlConverter : XmlConverter
    {
        protected override string SerializeToString(object value)
        {
            return Encoding.UTF8.GetString(SerializeToBytes(value));
        }

        protected override byte[] SerializeToBytes(object value)
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

        protected override object DeserializeToObject(string xml, Type type)
        {
            if (string.IsNullOrWhiteSpace(xml)) return Je.sys.DefaultOf(type);
            var bytes = Encoding.UTF8.GetBytes(xml);
            if (bytes == null || bytes.Length == 0) return Je.sys.DefaultOf(type);
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

    public class XmlConverter : ISerializer, IDeserializer
    { 
        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        public T Serialize<T>(object value)
        {
            if (typeof(T) == typeof(string)) return SerializeToString(value).As<T>();
            if (typeof(T) == typeof(byte[])) return SerializeToBytes(value).As<T>();
            throw new InvalidCastException();
        }

        public string Serialize(object value)
        {
            return SerializeToString(value);
        }

        public T Deserialize<T>(string s)
        {
            return (T)DeserializeToObject(s, typeof(T));
        }

        public object Deserialize(string s, Type type)
        {  
            return DeserializeToObject(s, type);
        }

        protected virtual object DeserializeToObject(string xml, Type type)
        {
            using (var sr = new StringReader(xml))
            {
                return (new XmlSerializer(type)).Deserialize(sr);
            }
        }

        protected virtual string SerializeToString(object value)
        {
            return Encoding.UTF8.GetString(SerializeToBytes(value)).Remove(0, ByteOrderMarkUtf8.Length);
        }

        protected virtual byte[] SerializeToBytes(object value)
        {
            if (value == null) return null;
            using (var ms = new MemoryStream())
            {
                var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var xs = new XmlSerializer(value.GetType());
                var xtw = new NoDeclarationXmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = Formatting.None;
                xtw.Namespaces = false;
                var xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                var xw = XmlWriter.Create(xtw, xws);
                xs.Serialize(xw, value, ns);
                return ms.ToArray();
            }
        }

        private class NoDeclarationXmlTextWriter : XmlTextWriter
        {
            public NoDeclarationXmlTextWriter(Stream w, Encoding encoding) : base(w, encoding) { }
            public NoDeclarationXmlTextWriter(string filename, Encoding encoding) : base(filename, encoding) { }
            public NoDeclarationXmlTextWriter(TextWriter w) : base(w) { }
            public override void WriteStartDocument(bool standalone) { }
            public override void WriteStartDocument() { }
        }
    }
}