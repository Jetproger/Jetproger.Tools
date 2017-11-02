using System.Xml.Serialization;

namespace Jetproger.Tools.Trace.Bases
{
    public sealed class NlogConfigRule
    {
        public NlogConfigRule()
        {
            var name = NlogConfig.GetMainTraceName();
            Name = name;
            MinLevel = "Trace";
            var targetName = NlogConfig.FormatName(Name, "_");
            WriteTo = $"{targetName}_log";
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("minlevel")]
        public string MinLevel { get; set; }

        [XmlAttribute("writeTo")]
        public string WriteTo { get; set; }
    }
}