using System.IO;
using System.Xml;
using Microsoft.Practices.Unity.Configuration;

namespace Jetproger.Tools.Plugin.DI
{
    public class InmemoryUnityConfigurationSection : UnityConfigurationSection
    {
        public void DeserializeSection(string xml)
        {
            using (var sr = new StringReader(xml))
            {
                using (var reader = XmlReader.Create(sr))
                {
                    base.DeserializeSection(reader);
                }
            }
        }
    }
}