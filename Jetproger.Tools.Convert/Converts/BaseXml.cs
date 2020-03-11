using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Jc
{
    public static class Xml<T> where T : BaseXml, new()
    {
        public static string Of(object value)
        {
            return One<T>.Ge.Of(value);
        }

        public static byte[] BinOf(object value)
        {
            return One<T>.Ge.BinOf(value);
        }

        public static TOut To<TOut>(string xml)
        {
            return One<T>.Ge.To<TOut>(xml);
        }

        public static object To(string xml, Type type)
        {
            return One<T>.Ge.To(xml, type);
        }
    }

    public class BaseXml
    {
        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        public virtual string Of(object value)
        {
            return Encoding.UTF8.GetString(BinOf(value)).Remove(0, ByteOrderMarkUtf8.Length);
        }

        public virtual byte[] BinOf(object value)
        {
            if (value == null) return null;
            using (var ms = new MemoryStream()) {
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

        public T To<T>(string xml)
        {
            return (T)To(xml, typeof(T));
        }

        public virtual object To(string xml, Type type)
        {
            using (var sr = new StringReader(xml))
            {
                return (new XmlSerializer(type)).Deserialize(sr);
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