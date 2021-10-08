using System.Collections.Generic;
using System.Xml.Serialization;

namespace Jetproger.Tools.Trace.Bases
{
    public sealed class NlogConfigTargets
    {
        public NlogConfigTargets()
        {
            Async = true;
        }

        [XmlAttribute("async")]
        public bool Async { get; set; }

        [XmlElement("target")]
        public List<NlogConfigTarget> Targets { get; set; }
    }
}