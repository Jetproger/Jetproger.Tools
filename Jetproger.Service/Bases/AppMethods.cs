using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using NLog;

namespace Jetproger.Service.Bases
{
    public class AppMethods
    {
        private static readonly AppMethods[] AppMethodsHolder = { null };
        private readonly object[] _providerHolder = { null };
        private readonly MethodInfo[] _baseHolder = { null };
        private readonly MethodInfo[] _httpHolder = { null };
        private readonly MethodInfo[] _liveHolder = { null };
        private readonly MethodInfo[] _mainHolder = { null };
        public void StartCommands() { _stop = false; }
        public void StopCommands() { _stop = true; }
        private AppDomain _domain;
        private bool _stop;
        private AppMethods() { }

        public static AppMethods Instance
        {
            get
            {
                if (AppMethodsHolder[0] == null)
                {
                    lock (AppMethodsHolder)
                    {
                        if (AppMethodsHolder[0] == null) AppMethodsHolder[0] = new AppMethods();
                    }
                }
                return AppMethodsHolder[0];
            }
        }

        public bool MainExecute(string[] args)
        {
            if (_stop) return false;
            try
            {
                return (bool)MainMethod.Invoke(Provider, new object[] { f.exe.name, args });
            }
            catch (Exception e)
            {
                ErrorProcessing(e);
                return true;
            }
        }

        public string HttpExecute(string request)
        {
            if (_stop) return null;
            try
            {
                return (string)HttpMethod.Invoke(Provider, new object[] { request });
            }
            catch (Exception e)
            {
                ErrorProcessing(e);
                return string.Empty;
            }
        }

        public void BaseExecute()
        {
            if (_stop) return;
            try
            {
                BaseMethod.Invoke(Provider, new object[] { f.exe.name });
            }
            catch (Exception e)
            {
                ErrorProcessing(e);
            }
        }

        public void Reset()
        {
            lock (_providerHolder)
            {
                try
                {
                    if (_domain != null) AppDomain.Unload(_domain);
                }
                catch
                {
                    _domain = null;
                }
                _domain = null;
                _providerHolder[0] = null;
                _baseHolder[0] = null;
                _httpHolder[0] = null;
                _liveHolder[0] = null;
                _mainHolder[0] = null;
            }
        }

        private MethodInfo BaseMethod
        {
            get
            {
                if (_baseHolder[0] == null)
                {
                    lock (_baseHolder)
                    {
                        if (_baseHolder[0] == null) _baseHolder[0] = Provider.GetType().GetMethod("BaseExecute", BindingFlags.Instance | BindingFlags.Public);
                    }
                }
                return _baseHolder[0];
            }
        }

        private MethodInfo HttpMethod
        {
            get
            {
                if (_httpHolder[0] == null)
                {
                    lock (_httpHolder)
                    {
                        if (_httpHolder[0] == null) _httpHolder[0] = Provider.GetType().GetMethod("HttpExecute", BindingFlags.Instance | BindingFlags.Public);
                    }
                }
                return _httpHolder[0];
            }
        }

        private MethodInfo LiveMethod
        {
            get
            {
                if (_liveHolder[0] == null)
                {
                    lock (_liveHolder)
                    {
                        if (_liveHolder[0] == null) _liveHolder[0] = Provider.GetType().GetMethod("Live", BindingFlags.Instance | BindingFlags.Public);
                    }
                }
                return _liveHolder[0];
            }
        }

        private MethodInfo MainMethod
        {
            get
            {
                if (_mainHolder[0] == null)
                {
                    lock (_mainHolder)
                    {
                        if (_mainHolder[0] == null) _mainHolder[0] = Provider.GetType().GetMethod("MainExecute", BindingFlags.Instance | BindingFlags.Public);
                    }
                }
                return _mainHolder[0];
            }
        }

        private object Provider
        {
            get
            {
                if (_providerHolder[0] == null)
                {
                    lock (_providerHolder)
                    {
                        if (_providerHolder[0] == null) _providerHolder[0] = CreateProvider();
                    }
                }
                return _providerHolder[0];
            }
        }

        private void ErrorProcessing(Exception e)
        {
            f.log.Error(e);
            if (!Live()) Reset();
        }

        private bool Live()
        {
            try
            {
                return (bool)LiveMethod.Invoke(Provider, new object[0]);
            }
            catch
            {
                return false;
            }
        }

        private object CreateProvider()
        {
            var setup = new AppDomainSetup
            {
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                LoaderOptimization = LoaderOptimization.SingleDomain,
                ShadowCopyFiles = "true",
            };
            var name = $"APPDOMAIN{Guid.NewGuid().ToString().Replace("-", "")}".ToUpper();
            _domain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setup);
            return _domain.CreateInstanceFromAndUnwrap(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "Jetproger.Tools.Convert.dll"), "Jetproger.Tools.Convert.Commanders.CommandProvider");
        }
    }

    public static class f
    {
        private static readonly JavaScriptSerializer JsonDeserializer;
        private static readonly JavaScriptSerializer JsonSerializer;
        private static readonly Dictionary<string, string> Config;
        private static readonly string ByteOrderMarkUtf8;
        private static readonly Logger Log;
        public static Logger log => Log;
        private static readonly _Exe Exe;
        public static _Exe exe => Exe;
        private static string[] _args;

        static f()
        {
            ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            JsonDeserializer = new JavaScriptSerializer();
            JsonSerializer = new JavaScriptSerializer();
            JsonSerializer.RegisterConverters(new JavaScriptConverter[] { new _NullPropertiesConverter() });
            Exe = new _Exe(Assembly.GetExecutingAssembly().Location);
            Log = LogManager.GetLogger("Jetproger");
            Config = GetConfig();
        }

        public static void keysof(string[] args)
        {
            _args = args.Select(x => x.ToLower()).ToArray();
        }

        public static bool iskey(params string[] keys)
        {
            foreach (var key in keys)
            {
                var lowerKey = key.ToLower();
                foreach (var arg in _args)
                {
                    if (lowerKey == arg) return true;
                }
            }
            return false;
        }

        public static bool isconfig(string key)
        {
            return Config.ContainsKey(key.ToLower());
        }

        public static string configof(string key, string defaultValue = null)
        {
            try
            {
                key = key.ToLower();
                return Config.ContainsKey(key) ? Config[key] : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static object defof(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static T jsonto<T>(string json)
        {
            try
            {
                return (T)jsonto(json, typeof(T));
            }
            catch
            {
                return (T)defof(typeof(T));
            }
        }

        public static object jsonto(string json, Type typeTo)
        {
            return JsonDeserializer.Deserialize(json, typeTo);
        }

        public static string jsonof(object value)
        {
            return JsonSerializer.Serialize(value);
        }

        public static T xmlto<T>(string xml)
        {
            try
            {
                return (T)xmlto(xml, typeof(T));
            }
            catch
            {
                return (T)defof(typeof(T));
            }
        }

        public static object xmlto(string xml, Type typeTo)
        {
            return DeserializeXml(!string.IsNullOrWhiteSpace(xml) ? Encoding.UTF8.GetBytes(xml) : new byte[0], typeTo);
        }

        public static string xmlof(object value)
        {
            return Encoding.UTF8.GetString(SerializeXml(value)).Remove(0, ByteOrderMarkUtf8.Length);
        }

        private static Dictionary<string, string> GetConfig()
        {
            var dict = new Dictionary<string, string>();
            foreach (string s in ConfigurationManager.AppSettings.AllKeys)
            {
                var key = s.ToLower();
                if (!dict.ContainsKey(key)) dict.Add(key, ConfigurationManager.AppSettings[s]);
            }
            return dict;
        }

        private static byte[] SerializeXml(object value)
        {
            if (value == null) return null;
            if (value is XmlDocument doc) return Encoding.UTF8.GetBytes(doc.InnerXml);
            using (var ms = new MemoryStream())
            {
                var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var xs = new XmlSerializer(value.GetType());
                using (var xtw = new NoDeclarationXmlTextWriter(ms, Encoding.UTF8))
                {
                    xtw.Formatting = Formatting.None;
                    xtw.Namespaces = true;
                    var xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;
                    using (var xw = XmlWriter.Create(xtw, xws))
                    {
                        xs.Serialize(xw, value, ns);
                        return ms.ToArray();
                    }
                }
            }
        }

        private static object DeserializeXml(byte[] bytes, Type typeTo)
        {
            if (bytes == null || bytes.Length == 0) return defof(typeTo);
            if (typeTo == typeof(XmlDocument))
            {
                var doc = new XmlDocument();
                doc.LoadXml(Encoding.UTF8.GetString(bytes));
                return doc;
            }
            using (var ms = new MemoryStream(bytes))
            {
                var xs = new XmlSerializer(typeTo);
                using (var xtr = new NoDeclarationXmlTextReader(ms, Encoding.UTF8))
                {
                    xtr.Namespaces = false;
                    var xrs = new XmlReaderSettings();
                    xrs.IgnoreComments = true;
                    xrs.IgnoreProcessingInstructions = true;
                    xrs.IgnoreWhitespace = true;
                    using (var xr = XmlReader.Create(xtr, xrs))
                    {
                        return xs.Deserialize(xr);
                    }
                }
            }
        }

        private class _NullPropertiesConverter : JavaScriptConverter
        {
            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) { throw new NotImplementedException(); }
            public override IEnumerable<Type> SupportedTypes { get { return GetType().Assembly.GetTypes(); } }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                var jsonExample = new Dictionary<string, object>();
                foreach (var p in obj.GetType().GetProperties())
                {
                    bool ignoreProp = p.IsDefined(typeof(ScriptIgnoreAttribute), true);
                    var value = p.GetValue(obj, BindingFlags.Public, null, null, null);
                    if (value != null && !ignoreProp) jsonExample.Add(p.Name, value);
                }
                return jsonExample;
            }
        }

        private class NoDeclarationXmlTextWriter : XmlTextWriter
        {
            public NoDeclarationXmlTextWriter(string filename, Encoding encoding) : base(filename, encoding) { }
            public NoDeclarationXmlTextWriter(Stream w, Encoding encoding) : base(w, encoding) { }
            public NoDeclarationXmlTextWriter(TextWriter w) : base(w) { } 
            public override void WriteStartDocument(bool standalone) { }
            public override void WriteStartDocument() { }
        }

        private class NoDeclarationXmlTextReader : XmlTextReader
        {
            public NoDeclarationXmlTextReader(string fileName) : base(new FileStream(fileName, FileMode.OpenOrCreate)) { }
            public NoDeclarationXmlTextReader(Stream r, Encoding encoding) : base(new StreamReader(r, encoding)) { }
            public NoDeclarationXmlTextReader(TextReader r) : base(r) { }
        }

        public class _Exe
        {
            public readonly string pathnameext;
            public readonly string nameext;
            public readonly string folder;
            public readonly string disk;
            public readonly string name;
            public readonly string path;
            public readonly string ext;

            public _Exe(string fileName)
            {
                pathnameext = fileName;
                nameext = System.IO.Path.GetFileName(pathnameext);
                name = System.IO.Path.GetFileNameWithoutExtension(pathnameext);
                folder = new DirectoryInfo(pathnameext).Name;
                path = System.IO.Path.GetDirectoryName(pathnameext);
                var extension = System.IO.Path.GetExtension(pathnameext) ?? string.Empty;
                if (extension.StartsWith(".")) extension = extension.Substring(1);
                ext = extension;
                disk = GetDisk(pathnameext);
            }

            private string GetDisk(string pathNameExt)
            {
                var d = System.IO.Path.GetPathRoot(pathNameExt);
                if (string.IsNullOrWhiteSpace(disk)) return d;
                if (!d.StartsWith(@"\\")) return d;
                while (true)
                {
                    var i = d.LastIndexOf(@"\");
                    if (i <= 1) break;
                    d = d.Substring(0, i);
                }
                return !d.EndsWith(@"\") ? d + @"\" : d;
            }
        }
    }
}