using System.Xml.Serialization;

namespace Jetproger.Tools.Trace.Bases
{
    public sealed class NlogConfigExtension
    {
        [XmlAttribute("assemblyFile")]
        public string AssemblyFile { get; set; }
    }
}