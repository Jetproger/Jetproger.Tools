using System.IO;
using System.Xml;
using Microsoft.Practices.Unity.Configuration;

namespace Jetproger.Tools.Injection.Bases
{
    public class InjectionSection : UnityConfigurationSection
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