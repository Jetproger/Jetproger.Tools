using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Jetproger.Tools.Plugin.DI
{
    public static class UnityConfigXml
    {
        private static readonly string[] XmlHolder = { null };

        public static string Xml
        {
            get
            {
                if (XmlHolder[0] == null)
                {
                    lock (XmlHolder)
                    {
                        if (XmlHolder[0] == null) XmlHolder[0] = ReadXmlFromFile();
                    }
                }
                return XmlHolder[0];
            }
        }

        private static string ReadXmlFromFile()
        {
            var fileName = Process.GetCurrentProcess().MainModule.FileName;
            fileName = Path.GetFileNameWithoutExtension(fileName.Replace(".vshost", string.Empty));
            var names = fileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            fileName = $"unity-{names[names.Length - 1]}.config";
            var s = System.IO.File.ReadAllText(fileName);
            var doc = new XmlDocument();
            doc.LoadXml(s);
            var root = doc.GetElementsByTagName("unity")[0];
            return root.OuterXml;
        }
    }
}