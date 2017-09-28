using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Jetproger.Tools.Xml.Bases
{
    public static partial class XmlExtensions
    {
        public static XmlNode AddNode(this XmlNode root, string name, bool value)
        {
            return AddNode(root, name, value.AsInt().AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, bool? value)
        {
            return value != null ? AddNode(root, name, value.AsInt().AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, byte value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, byte? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, sbyte value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, sbyte? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, byte[] value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, char value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, char? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, char[] value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, short value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, short? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, ushort value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, ushort? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, int value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, int? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, uint value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, uint? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, long value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, long? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, ulong value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, ulong? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, Guid value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, Guid? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, float value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, float? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, decimal value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, decimal? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, double value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, double? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, DateTime value)
        {
            return AddNode(root, name, value.AsString());
        }

        public static XmlNode AddNode(this XmlNode root, string name, DateTime? value)
        {
            return value != null ? AddNode(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name)
        {
            return AddAttr(root, name, "");
        }

        public static XmlNode AddAttr(this XmlNode root, string name, bool value)
        {
            return AddAttr(root, name, value.AsInt().AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, bool? value)
        {
            return value != null ? AddAttr(root, name, value.AsInt().AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, byte value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, byte? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, sbyte value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, sbyte? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, byte[] value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, char value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, char? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, char[] value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, short value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, short? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, ushort value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, ushort? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, int value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, int? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, uint value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, uint? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, long value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, long? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, ulong value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, ulong? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, Guid value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, Guid? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, float value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, float? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, decimal value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, decimal? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, double value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, double? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, DateTime value)
        {
            return AddAttr(root, name, value.AsString());
        }

        public static XmlNode AddAttr(this XmlNode root, string name, DateTime? value)
        {
            return value != null ? AddAttr(root, name, value.AsString()) : null;
        }

        public static XmlNode AddNode(this XmlNode root, string name, object value)
        {
            if (value == null) return null;
            Type type = value.GetType();
            if (type.IsBool()) return AddNode(root, name, value.AsBool());
            if (type.IsBoolNull()) return AddNode(root, name, value.AsBoolNull());
            if (type.IsByte()) return AddNode(root, name, value.AsByte());
            if (type.IsByteNull()) return AddNode(root, name, value.AsByteNull());
            if (type.IsSbyte()) return AddNode(root, name, value.AsSbyte());
            if (type.IsSbyteNull()) return AddNode(root, name, value.AsSbyteNull());
            if (type.IsBytes()) return AddNode(root, name, value.AsBytes());
            if (type.IsChar()) return AddNode(root, name, value.AsChar());
            if (type.IsCharNull()) return AddNode(root, name, value.AsCharNull());
            if (type.IsChars()) return AddNode(root, name, value.AsChars());
            if (type.IsShort()) return AddNode(root, name, value.AsShort());
            if (type.IsShortNull()) return AddNode(root, name, value.AsShortNull());
            if (type.IsUshort()) AddNode(root, name, value.AsUshort());
            if (type.IsUshortNull()) return AddNode(root, name, value.AsUshortNull());
            if (type.IsInt()) return AddNode(root, name, value.AsInt());
            if (type.IsIntNull()) return AddNode(root, name, value.AsIntNull());
            if (type.IsUint()) return AddNode(root, name, value.AsUint());
            if (type.IsUintNull()) return AddNode(root, name, value.AsUintNull());
            if (type.IsEnum()) return AddNode(root, name, value.AsInt());
            if (type.IsEnumNull()) return AddNode(root, name, value.AsIntNull());
            if (type.IsLong()) return AddNode(root, name, value.AsLong());
            if (type.IsLongNull()) return AddNode(root, name, value.AsLongNull());
            if (type.IsUlong()) return AddNode(root, name, value.AsUlong());
            if (type.IsUlongNull()) return AddNode(root, name, value.AsUlongNull());
            if (type.IsGuid()) return AddNode(root, name, value.AsGuid());
            if (type.IsGuidNull()) return AddNode(root, name, value.AsGuidNull());
            if (type.IsFloat()) return AddNode(root, name, value.AsFloat());
            if (type.IsFloatNull()) return AddNode(root, name, value.AsFloatNull());
            if (type.IsDecimal()) return AddNode(root, name, value.AsDecimal());
            if (type.IsDecimalNull()) return AddNode(root, name, value.AsDecimalNull());
            if (type.IsDouble()) return AddNode(root, name, value.AsDouble());
            if (type.IsDoubleNull()) return AddNode(root, name, value.AsDoubleNull());
            if (type.IsDateTime()) return AddNode(root, name, value.AsDateTime());
            if (type.IsDateTimeNull()) return AddNode(root, name, value.AsDateTimeNull());
            if (type.IsString()) return AddNode(root, name, value.AsString());
            return AddNode(root, name, value.ToString());
        }

        public static XmlNode AddNode(this XmlNode root, Exception exception)
        {
            if (root == null || exception == null) return root;
            root = AddNode(root, "Exception");
            AddNode(root, "Message", exception.Message);
            AddNode(root, "Info", exception.AsString());
            return root;
        }

        public static XmlNode AddNode(this XmlNode root, string name, string namespaceUri = null)
        {
            return AddNode(root, name, "", namespaceUri);
        }

        public static XmlNode AddNode(this XmlNode root, string name, string value, string namespaceUri = null)
        {
            if (value == null) return null;
            XmlDocument doc = root.OwnerDocument ?? (XmlDocument)root;
            XmlNode node = doc.CreateNode(XmlNodeType.Element, name, namespaceUri);
            node.InnerText = value;
            root.AppendChild(node);
            return node;
        }

        public static XmlNode AddAttr(this XmlNode root, string name, string value)
        {
            if (value == null) return null;
            XmlDocument doc = root.OwnerDocument ?? (XmlDocument)root;
            XmlAttribute attr = doc.CreateAttribute(name);
            attr.InnerText = value;
            if (root.Attributes != null) root.Attributes.Append(attr);
            return root;
        }

        public static void AddNode(this XmlNode root, XmlNodeList list)
        {
            XmlDocument doc = root.OwnerDocument ?? (XmlDocument)root;
            foreach (XmlNode node in list)
            {
                XmlNode newChild = doc.ImportNode(node, true);
                root.AppendChild(newChild);
            }
        }

        public static XmlNode NewXml()
        {
            return NewXml(false);
        }

        public static XmlNode NewXml(bool addDeclaration)
        {
            var doc = new XmlDocument();
            if (addDeclaration)
            {
                XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "Windows-1251", null);
                doc.AppendChild(decl);
            }
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "XML", null);
            doc.AppendChild(node);
            return node;
        }

        public static string GetValue(this XmlNode root, int index)
        {
            return root != null && index >= 0 && index < root.ChildNodes.Count ? root.ChildNodes[index].InnerText : null;
        }

        public static string GetValue(this XmlNode root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return null;
            XmlNode node = root.SelectSingleNode(name);
            return node != null ? node.InnerText : null;
        }

        public static void SetValue(this XmlNode root, int index, object value)
        {
            if (root == null || value == null || index < 0 && index >= root.ChildNodes.Count) return;
            XmlNode node = root.ChildNodes[index];
            if (node != null) node.InnerText = value.AsString();
        }

        public static void SetValue(this XmlNode root, string name, object value)
        {
            if (root == null || string.IsNullOrWhiteSpace(name)) return;
            XmlNode node = root.SelectSingleNode(name);
            if (node != null) node.InnerText = value.AsString();
        }

        public static void SetValue(this XmlDocument doc, string xmlPath, string value)
        {
            XmlNode node = doc.SelectSingleNode(xmlPath) ?? CreateXmlPath(doc, xmlPath);
            node.InnerText = value;
        }

        public static XmlNode CreateXmlPath(this XmlDocument doc, string xpath)
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

        public static string GetAttr(this XmlNode root, int index)
        {
            return root != null && root.Attributes != null && index >= 0 && index < root.Attributes.Count ? root.Attributes[index].InnerText : null;
        }

        public static string GetAttr(this XmlNode root, string name)
        {
            if (root == null || string.IsNullOrWhiteSpace(name) || root.Attributes == null) return null;
            XmlAttribute attr = root.Attributes[name];
            return attr != null ? attr.InnerText : null;
        }

        public static void SetAttr(this XmlNode root, int index, object value)
        {
            if (root == null || value == null || root.Attributes == null || index < 0 && index >= root.Attributes.Count) return;
            XmlAttribute attr = root.Attributes[index];
            if (attr != null) attr.InnerText = value.AsString();
        }

        public static void SetAttr(this XmlNode root, string name, object value)
        {
            if (root == null || string.IsNullOrWhiteSpace(name) || root.Attributes == null) return;
            XmlAttribute attr = root.Attributes[name];
            if (attr != null) attr.InnerText = value.AsString();
        }

        public static string FormatXml(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return FormatXml(doc);
        }

        public static string FormatXml(this XmlNode root)
        {
            var sw = new StringWriter();
            var xmlWriter = new XmlTextWriter(sw) { Formatting = Formatting.Indented };
            root.WriteTo(xmlWriter);
            return sw.ToString();
        }

        public static void Transform(this XmlDocument doc, string xsltFile, string outFile)
        {
            if (File.Exists(outFile))
            {
                File.Delete(outFile);
            }
            using (var sw = new StreamWriter(outFile))
            {
                var xpathDoc = new XPathDocument(new XmlNodeReader(doc));
                var xslt = new XslCompiledTransform();
                var rs = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit };
                var reader = XmlReader.Create(xsltFile, rs);
                xslt.Load(reader);
                xslt.Transform(xpathDoc, null, sw);
            }
        }
    }
}