using System.Collections.Generic;
using System.Xml.Serialization;

namespace Jetproger.Tools.Convert.Settings
{
    [XmlRoot("access")]
    public class AccessConfig
    {
        [XmlArrayItem("add", IsNullable = false)]
        public List<ConfigEntry> Entries { get; set; }
        public AccessConfig() { Entries = new List<ConfigEntry>(); }
    }

    [XmlRoot("cache")]
    public class CacheConfig
    {
        [XmlArrayItem("add", IsNullable = false)]
        public List<ConfigEntry> Entries { get; set; }
        public CacheConfig() { Entries = new List<ConfigEntry>(); }
    }

    public class ConfigEntry
    {
        [XmlAttribute("key")]
        public string Type { get; set; }
        
        [XmlAttribute("value")]
        public string Value { get; set; }
    } 
}