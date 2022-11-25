using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Factories;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Converts
{
    public class WebExpander
    {
        public int RequestTimeout { get { return f.one.Get(_requestTimeoutHolder, () => k<HttpConnectionTimeoutSeconds>.key.As<int>() * 1000); } }
        private readonly int?[] _requestTimeoutHolder = { null };

        public Encoding WebEncoding { get { return f.one.Get(_webEncodingHolder, () => Encoding.GetEncoding("utf-8")); } }
        private readonly Encoding[] _webEncodingHolder = { null };

        public string AppAddress { get { return f.one.Get(_appAddressHolder, GetAppAddress); } }
        private readonly string[] _appAddressHolder = { null };

        public string AppHost { get { return f.one.Get(_appHostHolder, GetAppHost); } }
        private readonly string[] _appHostHolder = { null };

        public string AppUrl { get { return f.one.Get(_appUrlHolder, GetAppUrl); } }
        private readonly string[] _appUrlHolder = { null };

        private string GetAppAddress()
        {
            return string.Format("{0}/jetproger/v1/cmd", AppUrl);
        }

        private string GetAppHost()
        {
            return string.Format("{0}/jetproger/v1/cmd", AppUrl);
        }

        private string GetAppUrl()
        {
            f.err.Guard(string.IsNullOrWhiteSpace(k<AppHost>.key), new AppConfigAppHostException());
            var url = k<AppHost>.key.Replace("https://", string.Empty).Replace("http://", string.Empty);
            if (string.IsNullOrWhiteSpace(k<AppCert>.key)) return string.Format("http://{0}", url);
            return f.cry.App != null ? string.Format("https://{0}", url) : string.Format("http://{0}", url);
        }
    }

    public static class WebExtensions
    {
        private static List<string> ProxyExcludesList { get { return f.one.Get(ProxyExcludesListHolder, GetProxyExcludesList); } }
        
        private static readonly List<string>[] ProxyExcludesListHolder = { null }; 
        
        private static readonly WebConverter Converter = t<WebConverter>.one();

        static WebExtensions()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            if (!TrySetSecurityProtocol()) TrySetSecurityProtocolXp();
        }

        public static string of(this WebExpander e, object value, WebConverter converter = null)
        {
            return (converter ?? Converter).Serialize(value);
        }

        public static string of<TConverter>(this WebExpander e, object value) where TConverter : WebConverter
        {
            return (f.sys.valueof<TConverter>()).Serialize(value);
        }

        public static object to(this WebExpander e, string s, Type type, WebConverter converter = null)
        {
            return (converter ?? Converter).Deserialize(s, type);
        }

        public static T to<T>(this WebExpander exp, string json, WebConverter converter = null)
        {
            return (T)(converter ?? Converter).Deserialize(json, typeof(T));
        }

        public static TResult to<TResult, TConverter>(this WebExpander exp, string json) where TConverter : WebConverter
        {
            return (TResult)(f.sys.valueof<TConverter>()).Deserialize(json, typeof(TResult));
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

        public static WebProxy GetProxy(this WebExpander exp, string url)
        {
            if (!k<ProxyUse>.key.As<bool>()) return new WebProxy();
            if (!UseProxyFor(exp, url)) return new WebProxy();
            var proxy = !string.IsNullOrWhiteSpace(k<ProxyServer>.key) ? new WebProxy(k<ProxyServer>.key, k<ProxyPort>.key.As<int>()) : WebProxy.GetDefaultProxy();
            proxy.Credentials = !string.IsNullOrWhiteSpace(k<ProxyUser>.key) ? new NetworkCredential(k<ProxyUser>.key, k<ProxyPassword>.key) : CredentialCache.DefaultCredentials;
            return proxy;
        }

        public static void TimeoutCallback(this WebExpander e, object state, bool timedOut)
        {
            if (!timedOut) return;
            var request = state as HttpWebRequest;
            if (request != null) request.Abort();
        }

        public static bool UseProxyFor(this WebExpander exp, string url)
        {
            if (!k<ProxyUse>.key.As<bool>()) return false;
            url = url.Replace("https://", string.Empty).Replace("http://", string.Empty);
            foreach (var exclude in ProxyExcludesList)
            {
                if (url.StartsWith(exclude)) return false;
            }
            return true;
        }

        private static List<string> GetProxyExcludesList()
        {
            var excludes = (k<ProxyExcludes>.key ?? string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var excludesList = new List<string>();
            foreach (var exclude in excludes)
            {
                excludesList.Add(exclude.Replace("https://", string.Empty).Replace("http://", string.Empty));
            }
            return excludesList;
        }
    }

    public class WebConverter
    { 
        private static JavaScriptSerializer Serializer { get { return t<JavaScriptSerializer>.one(CreateJsonSerializer); } } 
        
        private static JavaScriptSerializer Deserializer { get { return t<JavaScriptSerializer>.one(); } }

        public virtual string Serialize(object o)
        {
            return Serializer.Serialize(o);
        }

        public virtual object Deserialize(string json, Type type)
        {
            return Deserializer.Deserialize(json, type);
        }

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