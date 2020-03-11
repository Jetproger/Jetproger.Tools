using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Converts
{
    public static class XmlExtensions
    {
        public static string Of(this Jc.IXmlExpander exp, object value)
        {
            return Jc.Xml<Jc.BaseXml>.Of(value);
        }

        public static byte[] BinOf(this Jc.IXmlExpander exp, object value)
        {
            return Jc.Xml<Jc.BaseXml>.BinOf(value);
        }

        public static TOut To<TOut>(this Jc.IXmlExpander exp, string xml)
        {
            return Jc.Xml<Jc.BaseXml>.To<TOut>(xml);
        }

        public static object To(this Jc.IXmlExpander exp, string xml, Type type)
        {
            return Jc.Xml<Jc.BaseXml>.To(xml, type);
        }

        public static XmlNode AddNode(this Jc.IXmlExpander exp, XmlNode root, string name)
        {
            return AddNode(exp, root, name, "");
        }

        public static XmlNode AddNode<T>(this Jc.IXmlExpander exp, XmlNode root, string name, T value, string namespaceUri = null)
        {
            if (value == null) return null;
            XmlDocument doc = root.OwnerDocument ?? (XmlDocument)root;
            XmlNode node = doc.CreateNode(XmlNodeType.Element, name, namespaceUri);
            node.InnerText = Je.Txt.Of(value);
            root.AppendChild(node);
            return node;
        }

        public static XmlNode AddAttr<T>(this Jc.IXmlExpander exp, XmlNode root, string name, T value)
        {
            if (value == null) return null;
            XmlDocument doc = root.OwnerDocument ?? (XmlDocument)root;
            XmlAttribute attr = doc.CreateAttribute(name);
            attr.InnerText = Je.Txt.Of(value);
            root.Attributes?.Append(attr);
            return root;
        }

        public static T GetNode<T>(this Jc.IXmlExpander exp, XmlNode root, int index)
        {
            return (root != null && index >= 0 && index < root.ChildNodes.Count ? root.ChildNodes[index].InnerText : null).As<T>();
        }

        public static T GetNode<T>(this Jc.IXmlExpander exp, XmlNode root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return default(T);
            XmlNode node = root.SelectSingleNode(name);
            return (node?.InnerText).As<T>();
        }

        public static void SetNode(this Jc.IXmlExpander exp, XmlNode root, int index, object value)
        {
            if (root == null || value == null || index < 0 && index >= root.ChildNodes.Count) return;
            XmlNode node = root.ChildNodes[index];
            if (node != null) node.InnerText = value.As<string>();
        }

        public static void SetNode(this Jc.IXmlExpander exp, XmlNode root, string name, object value)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return;
            XmlNode node = root.SelectSingleNode(name);
            if (node != null) node.InnerText = value.As<string>();
        }

        public static void SetNode(this Jc.IXmlExpander exp, XmlDocument doc, string xmlPath, string value)
        {
            XmlNode node = doc.SelectSingleNode(xmlPath) ?? Create(exp, doc, xmlPath);
            node.InnerText = value;
        }

        public static T GetAttr<T>(this Jc.IXmlExpander exp, XmlNode root, int index)
        {
            return (root != null && root.Attributes != null && index >= 0 && index < root.Attributes.Count ? root.Attributes[index].InnerText : null).As<T>();
        }

        public static T GetAttr<T>(this Jc.IXmlExpander exp, XmlNode root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name) || root.Attributes == null) return default(T);
            XmlAttribute attr = root.Attributes[name];
            return (attr?.InnerText).As<T>();
        }

        public static void SetAttr(this Jc.IXmlExpander exp, XmlNode root, int index, object value)
        {
            if (root == null || value == null || root.Attributes == null || index < 0 && index >= root.Attributes.Count) return;
            XmlAttribute attr = root.Attributes[index];
            if (attr != null) attr.InnerText = value.As<string>();
        }

        public static void SetAttr(this Jc.IXmlExpander exp, XmlNode root, string name, object value)
        {
            if (root == null || string.IsNullOrWhiteSpace(name) || root.Attributes == null) return;
            XmlAttribute attr = root.Attributes[name];
            if (attr != null) attr.InnerText = value.As<string>();
        }

        public static XmlNode Create(this Jc.IXmlExpander exp)
        {
            var doc = new XmlDocument();
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "XML", null);
            doc.AppendChild(node);
            return node;
        }

        public static XmlNode Create(this Jc.IXmlExpander exp, XmlDocument doc, string xpath)
        {
            string[] data = xpath.Split('/');
            XmlNode current = doc.DocumentElement;
            string path = "";
            foreach (string name in data)
            {
                path = path + "/" + name;
                XmlNode node = doc.SelectSingleNode(path) ?? AddNode(exp, current, name, "");
                current = node;
            }
            return current;
        }

        public static string Xslt(this Jc.IXmlExpander exp, XmlDocument doc, string xsltScript)
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