using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Factories;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Converts
{
    public class WebExpander
    {
        public int RequestTimeout { get { return Je.one.Get(RequestTimeoutHolder, () => J_<HttpConnectionTimeoutSeconds>.Sz.As<int>() * 1000); } }
        private readonly int?[] RequestTimeoutHolder = { null };

        public Encoding WebEncoding { get { return Je.one.Get(WebEncodingHolder, () => Encoding.GetEncoding("utf-8")); } }
        private readonly Encoding[] WebEncodingHolder = { null };

        public string AppAddress { get { return Je.one.Get(AppAddressHolder, GetAppAddress); } }
        private readonly string[] AppAddressHolder = { null };

        public string AppHost { get { return Je.one.Get(AppHostHolder, GetAppHost); } }
        private readonly string[] AppHostHolder = { null };

        public string AppUrl { get { return Je.one.Get(AppUrlHolder, GetAppUrl); } }
        private readonly string[] AppUrlHolder = { null };

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
            Je.err.Guard(string.IsNullOrWhiteSpace(J_<AppHost>.Sz), new AppConfigAppHostException());
            var url = J_<AppHost>.Sz.Replace("https://", string.Empty).Replace("http://", string.Empty);
            if (string.IsNullOrWhiteSpace(J_<AppCert>.Sz)) return string.Format("http://{0}", url);
            return Je.cry.App != null ? string.Format("https://{0}", url) : string.Format("http://{0}", url);
        }
    }

    public static class WebExtensions
    {
        private static List<string> ProxyExcludesList { get { return Je.one.Get(ProxyExcludesListHolder, GetProxyExcludesList); } }
        private static readonly List<string>[] ProxyExcludesListHolder = { null }; 
        private static readonly WebConverter Converter = Je<WebConverter>.Onu();

        static WebExtensions()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            if (!TrySetSecurityProtocol()) TrySetSecurityProtocolXp();
        }

        public static string Of(this WebExpander e, object value, WebConverter converter = null)
        {
            return (converter ?? Converter).Serialize(value);
        }

        public static string Of<TConverter>(this WebExpander e, object value) where TConverter : WebConverter
        {
            return (Je.sys.InstanceOf<TConverter>()).Serialize(value);
        }

        public static object To(this WebExpander e, string s, Type type, WebConverter converter = null)
        {
            return (converter ?? Converter).Deserialize(s, type);
        }

        public static T To<T>(this WebExpander exp, string json, WebConverter converter = null)
        {
            return (T)(converter ?? Converter).Deserialize(json, typeof(T));
        }

        public static TResult To<TResult, TConverter>(this WebExpander exp, string json) where TConverter : WebConverter
        {
            return (TResult)(Je.sys.InstanceOf<TConverter>()).Deserialize(json, typeof(TResult));
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
            if (!J_<ProxyUse>.Sz.As<bool>()) return new WebProxy();
            if (!UseProxyFor(exp, url)) return new WebProxy();
            var proxy = !string.IsNullOrWhiteSpace(J_<ProxyServer>.Sz) ? new WebProxy(J_<ProxyServer>.Sz, J_<ProxyPort>.Sz.As<int>()) : WebProxy.GetDefaultProxy();
            proxy.Credentials = !string.IsNullOrWhiteSpace(J_<ProxyUser>.Sz) ? new NetworkCredential(J_<ProxyUser>.Sz, J_<ProxyPassword>.Sz) : CredentialCache.DefaultCredentials;
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
            if (!J_<ProxyUse>.Sz.As<bool>()) return false;
            url = url.Replace("https://", string.Empty).Replace("http://", string.Empty);
            foreach (var exclude in ProxyExcludesList)
            {
                if (url.StartsWith(exclude)) return false;
            }
            return true;
        }

        private static List<string> GetProxyExcludesList()
        {
            var excludes = (J_<ProxyExcludes>.Sz ?? string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
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
        private static JavaScriptSerializer Serializer { get { return Je<JavaScriptSerializer>.One(CreateJsonSerializer); } } 
        private static JavaScriptSerializer Deserializer { get { return Je<JavaScriptSerializer>.Onu(); } }

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