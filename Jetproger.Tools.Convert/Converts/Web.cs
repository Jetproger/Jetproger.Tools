using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Factories;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Converts
{
    public class WebExpander
    {
        public int RequestTimeout { get { return f.one.of(_requestTimeoutHolder, () => k<HttpConnectionTimeoutSeconds>.As<int>() * 1000); } }
        private readonly int?[] _requestTimeoutHolder = { null };

        public Encoding WebEncoding { get { return f.one.of(_webEncodingHolder, () => Encoding.GetEncoding("utf-8")); } }
        private readonly Encoding[] _webEncodingHolder = { null };

        public string AppAddress => f.one.of(_appAddressHolder, GetAppAddress);
        private readonly string[] _appAddressHolder = { null };

        public string AppHost => f.one.of(_appHostHolder, GetAppHost);
        private readonly string[] _appHostHolder = { null };

        public string AppUrl => f.one.of(_appUrlHolder, GetAppUrl);
        private readonly string[] _appUrlHolder = { null };

        private static string[] ProxyExcludes => f.one.of(ProxyExcludesHolder, GetProxyExcludes);
        private static readonly string[][] ProxyExcludesHolder = { null };

        static WebExpander()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            if (!TrySetSecurityProtocol()) TrySetSecurityProtocolXp();
        }

        private static bool TrySetSecurityProtocol()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TrySetSecurityProtocolXp()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)768;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut && state is HttpWebRequest request) request.Abort();
        }

        public WebProxy ProxyOf(string url)
        {
            if (!k<ProxyUse>.As<bool>() || !ProxyFor(url)) return new WebProxy();
            var proxy = !string.IsNullOrWhiteSpace(k<ProxyServer>.As<string>()) ? new WebProxy(k<ProxyServer>.As<string>(), k<ProxyPort>.As<int>()) : WebProxy.GetDefaultProxy();
            proxy.Credentials = !string.IsNullOrWhiteSpace(k<ProxyUser>.As<string>()) ? new NetworkCredential(k<ProxyUser>.As<string>(), k<ProxyPassword>.As<string>()) : CredentialCache.DefaultCredentials;
            return proxy;
        }

        public bool ProxyFor(string url)
        {
            if (!k<ProxyUse>.As<bool>()) return false;
            url = UrlWithoutProtocol(url);
            return ProxyExcludes.All(x => !url.StartsWith(x));
        }

        private string GetAppAddress()
        {
            return $"{AppUrl}/jetproger/v1/cmd";
        }

        private string GetAppHost()
        {
            return $"{AppUrl}/jetproger/v1/";
        }

        private string GetAppUrl()
        {
            f.err<AppConfigAppHostException>(string.IsNullOrWhiteSpace(k<AppHost>.As<string>()));
            var url = UrlWithoutProtocol(k<AppHost>.As<string>());
            if (string.IsNullOrWhiteSpace(k<AppCert>.As<string>())) return $"http://{url}";
            return f.cry.App != null ? $"https://{url}" : $"http://{url}";
        }

        private static string[] GetProxyExcludes()
        {
            var excludes = (k<ProxyExcludes>.As<string>() ?? string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < excludes.Length; i++)
            {
                excludes[i] = excludes[i].Replace("https://", string.Empty).Replace("http://", string.Empty);
            }
            return excludes;
        }

        private static string UrlWithoutProtocol(string url)
        {
            return (url ?? string.Empty).Replace("https://", string.Empty).Replace("http://", string.Empty);
        }
    }

    public class NewtonsoftJson : Converter
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        protected override byte[] ValueAsBytes(object value) { return Encoder.GetBytes(ValueAsChars(value)); }
        protected override string ValueAsChars(object value) { return JsonConvert.SerializeObject(value, Formatting.None, Settings); }
        protected override object BytesAsValue(byte[] bytes, Type typeTo) { return CharsAsValue(Encoder.GetString(bytes), typeTo); }
        protected override object CharsAsValue(string chars, Type typeTo) { return JsonConvert.DeserializeObject(chars, typeTo); }
        public NewtonsoftJson(Encoding encoder = null) { Encoder = encoder ?? base.Encoder; }
        protected override Encoding Encoder { get; }
    }

    public class SimpleJson : Converter
    {
        private static JavaScriptSerializer JsonSerializer { get { return t<JavaScriptSerializer>.one(CreateJsonSerializer); } } 
        private static JavaScriptSerializer JsonDeserializer { get { return t<JavaScriptSerializer>.one(); } }
        
        protected override byte[] ValueAsBytes(object value) { return Encoder.GetBytes(ValueAsChars(value)); }
        protected override string ValueAsChars(object value) { return JsonSerializer.Serialize(value); }
        
        protected override object BytesAsValue(byte[] bytes, Type typeTo) { return CharsAsValue(Encoder.GetString(bytes), typeTo); }
        protected override object CharsAsValue(string chars, Type typeTo) { return JsonDeserializer.Deserialize(chars, typeTo); }

        public SimpleJson(Encoding encoder = null) { Encoder = encoder ?? base.Encoder; }
        protected override Encoding Encoder { get; }

        private static JavaScriptSerializer CreateJsonSerializer()
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new NullPropertiesConverter() });
            return serializer;
        }

        private class NullPropertiesConverter : JavaScriptConverter
        {
            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                throw new NotImplementedException();
            }

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

            public override IEnumerable<Type> SupportedTypes
            {
                get { return GetType().Assembly.GetTypes(); }
            }
        }
    }
}
namespace Jetproger.Tools.AppConfig
{
    public class HttpConnectionTimeoutSeconds : ConfigSetting { public HttpConnectionTimeoutSeconds() : base("300") { } }
    public class AppHost : ConfigSetting { public AppHost() : base("127.0.0.1") { } }
    public class AppPort : ConfigSetting { public AppPort() : base("9000") { } }
    public class AppCert : ConfigSetting { public AppCert() : base("") { } }
    public class OutCert : ConfigSetting { public OutCert() : base("") { } }
    public class AppHostNameComparisonMode : ConfigSetting { public AppHostNameComparisonMode() : base("0") { } }
    public class ProxyUse : ConfigSetting { public ProxyUse() : base("0") { } }
    public class ProxyServer : ConfigSetting { public ProxyServer() : base("proxy-ob.protek") { } }
    public class ProxyPort : ConfigSetting { public ProxyPort() : base("3128") { } }
    public class ProxyUser : ConfigSetting { public ProxyUser() : base("user") { } }
    public class ProxyPassword : ConfigSetting { public ProxyPassword() : base("password") { } }
    public class ProxyExcludes : ConfigSetting { public ProxyExcludes() : base("localhost") { } }
}