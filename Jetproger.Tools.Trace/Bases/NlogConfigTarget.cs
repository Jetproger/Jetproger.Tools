using System.Xml.Serialization;

namespace Jetproger.Tools.Trace.Bases
{
    public sealed class NlogConfigTarget
    {
        public NlogConfigTarget()
        {
            var name = NlogConfig.GetMainTraceName();
            var targetName = NlogConfig.BuildName(name, "_");
            var fileName = NlogConfig.BuildName(name, "-");
            Name = $"{targetName}_log";
            Type = "File";
            Encoding = "UTF-8";
            Layout = "${date:format=dd.MM.yyyy HH\\:mm\\:ss} ${AppUser} ${Computer} ${WinUser} [${callsite}] (${level:uppercase=true}): ${message}. ${exception:format=ToString}";
            KeepFileOpen = true;
            NetworkWrites = true;
            ConcurrentWrites = true;
            FileName = "${basedir}/log/" + fileName + ".$$.log";
            ArchiveFileName = "${basedir}/log/" + fileName + ".{##}.log";
            ArchiveEvery = "Day";
            ArchiveNumbering = "Rolling";
            MaxArchiveFiles = 33;
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("encoding")]
        public string Encoding { get; set; }

        [XmlAttribute("layout")]
        public string Layout { get; set; }

        [XmlAttribute("keepFileOpen")]
        public bool KeepFileOpen { get; set; }

        [XmlAttribute("networkWrites")]
        public bool NetworkWrites { get; set; }

        [XmlAttribute("concurrentWrites")]
        public bool ConcurrentWrites { get; set; }

        [XmlAttribute("fileName")]
        public string FileName { get; set; }

        [XmlAttribute("archiveFileName")]
        public string ArchiveFileName { get; set; }

        [XmlAttribute("archiveEvery")]
        public string ArchiveEvery { get; set; }

        [XmlAttribute("archiveNumbering")]
        public string ArchiveNumbering { get; set; }

        [XmlAttribute("maxArchiveFiles")]
        public int MaxArchiveFiles { get; set; }
    }
}