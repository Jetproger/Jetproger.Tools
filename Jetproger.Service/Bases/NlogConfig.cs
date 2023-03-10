using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace Jetproger.Tools.Trace.Bases
{
    [XmlRoot("nlog", Namespace = "http://www.nlog-project.org/schemas/NLog.xsd")]
    public sealed class NlogConfig
    {
        private static readonly string[] MainTraceNameHolder = { null };
        private bool _canRegisterRule = true;

        public NlogConfig()
        {
            Extensions = new List<NlogConfigExtension>();
        }

        [XmlAttribute("throwExceptions")]
        public bool ThrowExceptions { get; set; }

        [XmlAttribute("autoReload")]
        public bool AutoReload { get; set; }

        [XmlArray("extensions")]
        [XmlArrayItem("add")]
        public List<NlogConfigExtension> Extensions { get; set; }

        [XmlElement("targets")]
        public NlogConfigTargets Targets { get; set; }

        [XmlArray("rules")]
        [XmlArrayItem("logger")]
        public List<NlogConfigRule> Rules { get; set; }

        public bool IsSaved()
        {
            return !_canRegisterRule;
        }

        public void ToXml()
        {
            if (!ExistsExtension("bin/Jetproger.Tools.Trace.dll")) Extensions.Add(new NlogConfigExtension { AssemblyFile = "bin/Jetproger.Tools.Trace.dll" });
            if (!ExistsExtension("Jetproger.Tools.Trace.dll")) Extensions.Add(new NlogConfigExtension { AssemblyFile = "Jetproger.Tools.Trace.dll" });
            var appDir = (HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("~"));
            var fileName = Path.Combine(appDir, "nlog.config");
            using (var sw = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                var xs = new XmlSerializer(GetType());
                xs.Serialize(sw, this, ns);
            }
            _canRegisterRule = false;
        }

        public void OfXml()
        {
            var appDir = (HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("~"));
            var fileName = Path.Combine(appDir, "nlog.config");
            if (!File.Exists(fileName))
            {
                Targets = new NlogConfigTargets { Targets = new List<NlogConfigTarget>(new[] { new NlogConfigTarget() }) };
                Rules = new List<NlogConfigRule>(new [] { new NlogConfigRule() });
                ThrowExceptions = false;
                AutoReload = true;
                return;
            }
            using (var sr = new StreamReader(fileName, Encoding.UTF8))
            {
                var ns = new XmlSerializerNamespaces();
                var config = (NlogConfig)(new XmlSerializer(GetType())).Deserialize(sr);
                ThrowExceptions = config.ThrowExceptions;
                AutoReload = config.AutoReload;
                Extensions = config.Extensions;
                Targets = config.Targets;
                Rules = config.Rules;
            }
        }

        public bool RegisterRule(string ruleName)
        {
            if (!_canRegisterRule) return false;
            ruleName = ParseName(ruleName);
            if (ExistsRule(ruleName)) return true;
            var targetName = BuildName(ruleName, "_");
            var fileName = BuildName(ruleName, "-");
            Rules.Add(new NlogConfigRule
            {
                Name = ruleName,
                MinLevel = "Trace",
                WriteTo = $"{targetName}_log"
            });
            Targets.Targets.Add(new NlogConfigTarget
            {
                Name = $"{targetName}_log",
                Type = "File",
                Encoding = "UTF-8",
                Layout = "${date:format=dd.MM.yyyy HH\\:mm\\:ss} ${AppUser} ${Computer} ${WinUser} [${callsite}] (${level:uppercase=true}): ${message}. ${exception:format=ToString}",
                KeepFileOpen = true,
                NetworkWrites = true,
                ConcurrentWrites = true,
                FileName = "${basedir}/log/" + fileName + ".$$.log",
                ArchiveFileName = "${basedir}/log/" + fileName + ".{##}.log",
                ArchiveEvery = "Day",
                ArchiveNumbering = "Rolling",
                MaxArchiveFiles = 33
            });
            return true;
        }

        public bool ExistsRule(string ruleName)
        {
            return Rules != null && Rules.Any(rule => rule.Name == ruleName);
        }

        public bool ExistsExtension(string extensionName)
        {
            return Extensions != null && Extensions.Any(extension => extension.AssemblyFile == extensionName);
        }

        public static string GetMainTraceName()
        {
            if (MainTraceNameHolder[0] == null)
            {
                lock (MainTraceNameHolder)
                {
                    if (MainTraceNameHolder[0] == null)
                    {
                        var fileName = Process.GetCurrentProcess().MainModule.FileName;
                        fileName = Path.GetFileNameWithoutExtension(fileName.Replace(".vshost", string.Empty));
                        var names = fileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        MainTraceNameHolder[0] = names[names.Length - 1];
                    }
                }
            }
            return MainTraceNameHolder[0];
        }

        public static string ParseName(string fullName)
        {
            return fullName.Replace("ExTicket", string.Empty).Replace("Ticket", string.Empty);
        }

        public static string BuildName(string name, string separator)
        {
            var sb = new StringBuilder();
            foreach (char c in (name ?? string.Empty))
            {
                if (char.IsLower(c))
                {
                    sb.Append(c);
                    continue;
                }
                if (sb.Length > 0) sb.Append(separator);
                sb.Append(c.ToString().ToLower());
            }
            return sb.ToString();
        }
    }
}