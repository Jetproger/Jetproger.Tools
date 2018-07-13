using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Jetproger.Tools.Convert.Bases
{
    public static class XmlExtensions
    {
        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        public static string Write(this IXmlExpander expander, object o)
        {
            return Encoding.UTF8.GetString(WriteBytes(expander, o)).Remove(0, ByteOrderMarkUtf8.Length);
        }

        public static byte[] WriteBytes(this IXmlExpander expander, object o)
        {
            if (o == null) return null;
            using (var ms = new MemoryStream())
            {
                var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var xs = new XmlSerializer(o.GetType());
                var xtw = new NoDeclarationXmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = Formatting.None;
                xtw.Namespaces = false;
                var xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                var xw = XmlWriter.Create(xtw, xws);
                xs.Serialize(xw, o, ns);
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

        public static string WriteEx(this IXmlExpander expander, object o)
        {
            return Encoding.UTF8.GetString(WriteBytesEx(expander, o));
        }

        public static byte[] WriteBytesEx(this IXmlExpander expander, object o)
        {
            if (o == null)
            {
                return null;
            }
            using (var ms = new MemoryStream())
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("xs", "http://www.w3.org/2001/XMLSchema");
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                var xs = new XmlSerializer(o.GetType());
                var xtw = new XmlTextWriter(ms, Encoding.UTF8);
                xtw.Formatting = Formatting.Indented;
                xtw.Namespaces = true;
                var xws = new XmlWriterSettings();
                xws.NewLineHandling = NewLineHandling.Replace;
                xws.NewLineChars = "\n";
                var xw = XmlWriter.Create(xtw, xws);
                xs.Serialize(xw, o, ns);
                return ms.ToArray();
            }
        }

        public static T Read<T>(this IXmlExpander expander, string xml)
        {
            using (var sr = new StringReader(xml))
            {
                return (T)(new XmlSerializer(typeof(T))).Deserialize(sr);
            }
        }

        public static object Read(this IXmlExpander expander, string xml, Type type)
        {
            using (var sr = new StringReader(xml))
            {
                return (new XmlSerializer(type)).Deserialize(sr);
            }
        }

        public static XmlNode AddNode<T>(this XmlNode root, string name, T value, string namespaceUri = null)
        {
            if (value == null) return null;
            XmlDocument doc = root.OwnerDocument ?? (XmlDocument)root;
            XmlNode node = doc.CreateNode(XmlNodeType.Element, name, namespaceUri);
            node.InnerText = Ex.String.Write(value);
            root.AppendChild(node);
            return node;
        }

        public static XmlNode AddAttribute<T>(this XmlNode root, string name, T value)
        {
            if (value == null) return null;
            XmlDocument doc = root.OwnerDocument ?? (XmlDocument)root;
            XmlAttribute attr = doc.CreateAttribute(name);
            attr.InnerText = Ex.String.Write(value);
            root.Attributes?.Append(attr);
            return root;
        }

        public static T ReadNode<T>(this XmlNode root, int index)
        {
            return (root != null && index >= 0 && index < root.ChildNodes.Count ? root.ChildNodes[index].InnerText : null).As<T>();
        }

        public static T ReadNode<T>(this XmlNode root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return default(T);
            XmlNode node = root.SelectSingleNode(name);
            return (node?.InnerText).As<T>();
        }

        public static void WriteNode(this XmlNode root, int index, object value)
        {
            if (root == null || value == null || index < 0 && index >= root.ChildNodes.Count) return;
            XmlNode node = root.ChildNodes[index];
            if (node != null) node.InnerText = value.As<string>();
        }

        public static void WriteNode(this XmlNode root, string name, object value)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return;
            XmlNode node = root.SelectSingleNode(name);
            if (node != null) node.InnerText = value.As<string>();
        }

        public static void WriteNode(this XmlDocument doc, string xmlPath, string value)
        {
            XmlNode node = doc.SelectSingleNode(xmlPath) ?? doc.Create(xmlPath);
            node.InnerText = value;
        }

        public static T ReadAttribute<T>(this XmlNode root, int index)
        {
            return (root != null && root.Attributes != null && index >= 0 && index < root.Attributes.Count ? root.Attributes[index].InnerText : null).As<T>();
        }

        public static T ReadAttribute<T>(this XmlNode root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name) || root.Attributes == null) return default(T);
            XmlAttribute attr = root.Attributes[name];
            return (attr?.InnerText).As<T>();
        }

        public static void WriteAttribute(this XmlNode root, int index, object value)
        {
            if (root == null || value == null || root.Attributes == null || index < 0 && index >= root.Attributes.Count) return;
            XmlAttribute attr = root.Attributes[index];
            if (attr != null) attr.InnerText = value.As<string>();
        }

        public static void WriteAttribute(this XmlNode root, string name, object value)
        {
            if (root == null || string.IsNullOrWhiteSpace(name) || root.Attributes == null) return;
            XmlAttribute attr = root.Attributes[name];
            if (attr != null) attr.InnerText = value.As<string>();
        }

        public static XmlNode Create(this IXmlExpander expander)
        {
            var doc = new XmlDocument();
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "XML", null);
            doc.AppendChild(node);
            return node;
        }

        public static XmlNode Create(this XmlDocument doc, string xpath)
        {
            string[] data = xpath.Split('/');
            XmlNode current = doc.DocumentElement;
            string path = "";
            foreach (string name in data)
            {
                path = path + "/" + name;
                XmlNode node = doc.SelectSingleNode(path) ?? AddNode(current, name, "");
                current = node;
            }
            return current;
        }

        public static XmlNode AddNode(this XmlNode root, string name)
        {
            return AddNode(root, name, "");
        }

        public static string Xslt(this XmlDocument doc, string xsltScript)
        {
            using (var sw = new StringWriter())
            {
                var xpathDoc = new XPathDocument(new XmlNodeReader(doc));
                var xslt = new XslCompiledTransform();
                var rs = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit };
                using (var sr = new StringReader(xsltScript)){
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
}