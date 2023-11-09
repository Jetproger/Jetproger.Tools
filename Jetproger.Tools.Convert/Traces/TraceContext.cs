using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using NLog;

namespace Jetproger.Tools.Convert.Traces
{
    public class TraceContext : MarshalByRefObject
    {
        private readonly Queue<_Message> _messages = new Queue<_Message>();
        private readonly Dictionary<Type, Logger>[] _loggers = { null };
        private const string DEFAULT_TRACE_NAME = "Jetproger";
        private Logger _defaultLogger;

        public void Write(Type[] types, string message, string category)
        {
            if (TraceJob.Instance == null) return;
            var loggers = Loggers;
            var logger = (Logger)null;
            lock (loggers)
            {
                foreach (var type in types)
                {
                    if (!loggers.ContainsKey(type)) continue;
                    logger = loggers[type];
                    break;
                } 
                logger = logger ?? _defaultLogger;
            }
            lock (_messages)
            {
                _messages.Enqueue(new _Message { Logger = logger, Message = message, Category = category });
            }
        }

        public void Write()
        {
            var message = (_Message)null;
            lock (_messages)
            {
                message = _messages.Count > 0 ? _messages.Dequeue() : null;
            }
            if (message != null) switch (message.Category.ToLower())
            {
                case "error": message.Logger.Error(message.Message); break;
                case "debug": message.Logger.Debug(message.Message); break;
                case "info": message.Logger.Info(message.Message); break;
                case "warn": message.Logger.Warn(message.Message); break;
                default: message.Logger.Trace(message.Message); break;
            }
        }

        private Dictionary<Type, Logger> Loggers
        {
            get
            {
                if (_loggers[0] == null)
                {
                    lock (_loggers)
                    {
                        if (_loggers[0] == null) _loggers[0] = LoadLoggers();
                    }
                }
                return _loggers[0];
            }
        }

        private Dictionary<Type, Logger> LoadLoggers()
        {
            var names = new Dictionary<Type, string>();
            var hash = new HashSet<string> { DEFAULT_TRACE_NAME };
            foreach (KeyValuePair<Type, string> pair in LoadAttributeLoggers())
            {
                if (!names.ContainsKey(pair.Key)) names.Add(pair.Key, pair.Value);
                if (!hash.Contains(pair.Value)) hash.Add(pair.Value);
            }
            var hashConfig = new HashSet<string>();
            foreach (string logger in LoadConfigLoggers())
            {
                if (!hashConfig.Contains(logger)) hashConfig.Add(logger);
            } 
            var doc = ReadConfigFile();
            var isModified = false;
            foreach (string logger in hash)
            {
                if (hashConfig.Contains(logger)) continue;
                CreateConfigLogger(doc, logger);
                isModified = true;
            }
            if (isModified) WriteConfigFile(doc);
            _defaultLogger = LogManager.GetLogger(DEFAULT_TRACE_NAME);
            var loggers = new Dictionary<Type, Logger>();
            foreach (KeyValuePair<Type, string> pair in names)
            {
                loggers.Add(pair.Key, LogManager.GetLogger(pair.Value));
            }
            return loggers;
        }

        private IEnumerable<KeyValuePair<Type, string>> LoadAttributeLoggers()
        {
            foreach (var pair in f.sys.attrall<TraceAttribute>())
            {
                yield return new KeyValuePair<Type, string>(pair.Type, !string.IsNullOrWhiteSpace(pair.Attribute.Name) ? pair.Attribute.Name : DEFAULT_TRACE_NAME);
            }
        }

        private IEnumerable<string> LoadConfigLoggers()
        {
            var doc = ReadConfigFile();
            var nlogRoot = FindNodeByName(doc, "nlog");
            if (nlogRoot == null) yield break;
            var targetsRoot = FindNodeByName(nlogRoot, "targets");
            if (targetsRoot == null) yield break;
            foreach (XmlNode node in targetsRoot.ChildNodes)
            {
                foreach (XmlAttribute attr in ((IEnumerable)node.Attributes ?? new XmlAttribute[0]))
                {
                    if (attr.Name == "name") yield return attr.Value;
                }
            }
        }

        private XmlDocument ReadConfigFile()
        { 
            var file = f.fss.pathnameextof("nlog.config");
            var doc = new XmlDocument();
            doc.Load(file);
            return doc;
        }

        private void WriteConfigFile(XmlDocument doc)
        {
            var sb = new StringBuilder();
            var el = XElement.Parse(doc.OuterXml);
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            using (var xmlWriter = XmlWriter.Create(sb, settings)) { el.Save(xmlWriter); }
            var file = f.fss.pathnameextof("nlog.config");
            File.WriteAllText(file, sb.ToString().Replace("xmlns=\"\"", string.Empty));
            LogManager.Configuration = LogManager.Configuration.Reload();
            LogManager.ReconfigExistingLoggers();
        }

        private void CreateConfigLogger(XmlDocument doc, string name)
        {
            var nlogRoot = FindNodeByName(doc, "nlog");
            if (nlogRoot == null) return;
            var targetsRoot = FindNodeByName(nlogRoot, "targets");
            if (targetsRoot == null) return;
            var targetNode = doc.CreateNode(XmlNodeType.Element, "target", string.Empty);
            AddAttribute(doc, targetNode, "name", name);
            AddAttribute(doc, targetNode, "type", "File", "http://www.w3.org/2001/XMLSchema-instance");
            AddAttribute(doc, targetNode, "layout", "${longdate} ${uppercase:${level}} ${message} ${exception:format=tostring}");
            AddAttribute(doc, targetNode, "fileName", "${basedir}/logs/" + name + "/" + name + "-${shortdate}.log");
            AddAttribute(doc, targetNode, "archiveFileName", "${basedir}/logs/" + name + "/" + name + "-{##}.log");
            AddAttribute(doc, targetNode, "archiveNumbering", "Rolling");
            AddAttribute(doc, targetNode, "maxArchiveFiles", "7");
            AddAttribute(doc, targetNode, "archiveEvery", "Day");
            AddAttribute(doc, targetNode, "deleteOldFileOnStartup", "false");
            AddAttribute(doc, targetNode, "concurrentWrites", "true");
            AddAttribute(doc, targetNode, "networkWrites", "true");
            AddAttribute(doc, targetNode, "keepFileOpen", "true");
            AddAttribute(doc, targetNode, "encoding", "UTF-8");
            ((XmlElement)targetNode).RemoveAttribute("xmlns");
            targetsRoot.AppendChild(targetNode);
            var rulesRoot = FindNodeByName(nlogRoot, "rules");
            if (rulesRoot == null) return;
            var loggerNode = doc.CreateNode(XmlNodeType.Element, "logger", string.Empty);
            AddAttribute(doc, loggerNode, "name", name + "Logger");
            AddAttribute(doc, loggerNode, "minlevel", "Trace");
            AddAttribute(doc, loggerNode, "writeTo", name);
            ((XmlElement)loggerNode).RemoveAttribute("xmlns");
            rulesRoot.AppendChild(loggerNode);
        }

        private static XmlNode FindNodeByName(XmlNode root, string nodeName)
        {
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == nodeName) return node;
            }
            return null;
        }

        private static void AddAttribute(XmlDocument doc, XmlNode node, string name, string value, string namespaceUri = null)
        {
            if (node == null || node.Attributes == null) return;
            var attr = doc.CreateAttribute(name, namespaceUri);
            attr.Value = value;
            node.Attributes.Append(attr);
        }

        private class _Message
        {
            public Logger Logger;
            public string Message;
            public string Category;
        }
    }
}