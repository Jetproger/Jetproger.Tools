using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace Jetproger.Tools.Inject.Bases
{
    [XmlRoot("configuration")]
    public class UnityConfiguration
    {
        public UnityConfiguration()
        {
            unity = new Unity();
            configSections = new List<Section>();
        }

        [XmlArrayItem("section")]
        public List<Section> configSections { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/practices/2010/unity")]
        public Unity unity { get; set; }

        public void ToXml()
        {
            var fileName = GetUnityConfigurationFileName();
            using (var sw = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);
                var xs = new XmlSerializer(GetType());
                xs.Serialize(sw, this, ns);
            }
        }

        public void OfXml()
        {
            var fileName = GetUnityConfigurationFileName();
            if (!File.Exists(fileName))
            {
                if (unity.SectionExtensions.Count == 0) unity.SectionExtensions.Add(new SectionExtension { type = "Microsoft.Practices.Unity.InterceptionExtension.Configuration.InterceptionConfigurationExtension, Microsoft.Practices.Unity.Interception.Configuration" });
                if (configSections.Count == 0) configSections.Add(new Section { name = "unity", type = "Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration" });
                SetIgnoreCase();
                return;
            }
            using (var sr = new StreamReader(fileName, Encoding.UTF8))
            {
                var unityConfiguration = (UnityConfiguration)(new XmlSerializer(GetType())).Deserialize(sr);
                configSections = unityConfiguration.configSections;
                unity = unityConfiguration.unity;
                if (unity.SectionExtensions.Count == 0) unity.SectionExtensions.Add(new SectionExtension { type = "Microsoft.Practices.Unity.InterceptionExtension.Configuration.InterceptionConfigurationExtension, Microsoft.Practices.Unity.Interception.Configuration" });
                if (configSections.Count == 0) configSections.Add(new Section { name = "unity", type = "Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration" });
                SetIgnoreCase();
            }
        }

        private void SetIgnoreCase()
        {
            foreach (var policy in unity.container.interception.Policies)
            {
                foreach (var matchingRule in policy.MatchingRules)
                {
                    if (matchingRule.constructor.Params.All(x => x.name != "ignoreCase")) matchingRule.constructor.Params.Add(new Param { name = "ignoreCase", value = "false" });
                }
            }
        }

        public static string GetUnityConfigurationFileName()
        {
            var appDir = (HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("~"));
            var fileName = Process.GetCurrentProcess().MainModule.FileName.ToLower();
            fileName = Path.GetFileNameWithoutExtension(fileName.Replace(".vshost", string.Empty));
            fileName = $"{fileName}.unity.config";
            return Path.Combine(appDir, fileName);
        }
    }

    public class Section
    {
        [XmlAttribute]
        public string name { get; set; }

        [XmlAttribute]
        public string type { get; set; }
    }

    public class Unity
    {
        public Unity()
        {
            container = new Container();
            Assemblies = new List<UnityAssembly>();
            Namespaces = new List<UnityNamespace>();
            SectionExtensions = new List<SectionExtension>();
        }

        [XmlElement("assembly")]
        public List<UnityAssembly> Assemblies { get; set; }

        [XmlElement("namespace")]
        public List<UnityNamespace> Namespaces { get; set; }

        [XmlElement("sectionExtension")]
        public List<SectionExtension> SectionExtensions { get; set; }

        public Container container { get; set; }

        public void AddAssembly(string assemblyName)
        {
            if (Assemblies.All(x => x.name != assemblyName)) Assemblies.Add(new UnityAssembly { name = assemblyName });
        }

        public void AddNamespace(string unityNamespace)
        {
            if (Namespaces.All(x => x.name != unityNamespace)) Namespaces.Add(new UnityNamespace { name = unityNamespace });
        }
    }

    public class SectionExtension
    {
        [XmlAttribute]
        public string type { get; set; }
    }

    public class UnityAssembly
    {
        [XmlAttribute]
        public string name { get; set; }
    }

    public class UnityNamespace
    {
        [XmlAttribute]
        public string name { get; set; }
    }

    public class Container
    {
        public Container()
        {
            extension = new Extension();
            interception = new Interception();
            Registers = new List<Register>();
        }

        public Extension extension { get; set; }

        public Interception interception { get; set; }

        [XmlElement("register")]
        public List<Register> Registers { get; set; }
    }

    public class Extension
    {
        public Extension()
        {
            type = "Interception";
        }

        [XmlAttribute]
        public string type { get; set; }
    }

    public class Register
    {
        public Register()
        {
            interceptor = new Interceptor();
            interceptionBehavior = new InterceptionBehavior();
        }

        [XmlAttribute]
        public string type { get; set; }

        [XmlAttribute]
        public string mapTo { get; set; }

        public UnityLifeTimeItem lifetime { get; set; }

        public Interceptor interceptor { get; set; }

        public InterceptionBehavior interceptionBehavior { get; set; }
    }

    public class UnityLifeTimeItem
    {
        [XmlAttribute]
        public string type { get; set; }
    }

    public class Interceptor
    {
        public Interceptor()
        {
            type = "VirtualMethodInterceptor";
        }

        [XmlAttribute]
        public string type { get; set; }
    }

    public class InterceptionBehavior
    {
        public InterceptionBehavior()
        {
            type = "PolicyInjectionBehavior";
        }

        [XmlAttribute]
        public string type { get; set; }
    }

    public class Interception
    {
        [XmlIgnore]
        private readonly Dictionary<string, Policy> _policies = new Dictionary<string, Policy>();

        [XmlElement("policy")]
        public Policy[] Policies
        {
            get
            {
                return _policies.Values.ToArray();
            }
            set
            {
                _policies.Clear();
                foreach (var policy in value)
                {
                    if (!_policies.ContainsKey(policy.name)) _policies.Add(policy.name, policy);
                }
            }
        }

        public void AddPolicies(Policy policy)
        {
            AddPolicies(new [] { policy });
        }

        public void AddPolicies(IEnumerable<Policy> policies)
        {
            foreach (var policy in policies)
            {
                if (!_policies.ContainsKey(policy.name))
                {
                    _policies.Add(policy.name, policy);
                }
                else
                {
                    var existsPolicy = _policies[policy.name];
                    existsPolicy.AddProperties(policy.callHandler.Properties);
                }
            }
        }
    }

    public class Policy
    {
        public Policy()
        {
        }

        public Policy(string typeName, string methodName, string callHandlerTypeName, IEnumerable<UnityPropertyItem> properties)
        {
            var key = $"{typeName.ToLower().Replace(".", "-")}-{methodName.ToLower()}-{callHandlerTypeName.ToLower()}";
            name = $"{key}-policy";
            MatchingRules = new List<MatchingRule>();

            var rule = new MatchingRule { name = $"{key}-type-name", type = "TypeMatchingRule" };
            rule.constructor.Params.Clear();
            rule.constructor.Params.Add(new Param { name = "typeName", value = typeName });
            rule.constructor.Params.Add(new Param { name = "ignoreCase", value = "false" });
            MatchingRules.Add(rule);

            rule = new MatchingRule { name = $"{key}-method-name", type = "MemberNameMatchingRule" };
            rule.constructor.Params.Clear();
            rule.constructor.Params.Add(new Param { name = "nameToMatch", value = methodName });
            rule.constructor.Params.Add(new Param { name = "ignoreCase", value = "false" });
            MatchingRules.Add(rule);

            callHandler = new CallHandler { name = $"{key}-call-handler", type = callHandlerTypeName };
            AddProperties(properties);
        }

        public void AddProperties(IEnumerable<UnityPropertyItem> properties)
        {
            foreach (UnityPropertyItem item in properties)
            {
                if (callHandler.Properties.All(x => item.name != x.name)) callHandler.Properties.Add(item);
            }
        }

        [XmlAttribute]
        public string name { get; set; }

        [XmlElement("matchingRule")]
        public List<MatchingRule> MatchingRules { get; set; }

        public CallHandler callHandler { get; set; }
    }

    public class MatchingRule
    {
        public MatchingRule()
        {
            @constructor = new Constructor();
        }

        [XmlAttribute]
        public string name { get; set; }

        [XmlAttribute]
        public string type { get; set; }

        public Constructor @constructor { get; set; }
    }

    public class Constructor
    {
        public Constructor()
        {
            Params = new List<Param>();
        }

        [XmlElement("param")]
        public List<Param> Params { get; set; }
    }

    public class Param
    {
        [XmlAttribute]
        public string name { get; set; }

        [XmlAttribute]
        public string value { get; set; }
    }

    public class CallHandler
    {
        public CallHandler()
        {
            lifetime = new UnityLifeTimeItem { type = "transient" };
            Properties = new List<UnityPropertyItem>();
        }

        [XmlAttribute]
        public string name { get; set; }

        [XmlAttribute]
        public string type { get; set; }

        public UnityLifeTimeItem lifetime { get; set; }

        [XmlElement("property")]
        public List<UnityPropertyItem> Properties { get; set; }
    }

    public class UnityPropertyItem
    {
        [XmlAttribute]
        public string name { get; set; }

        [XmlAttribute]
        public string value { get; set; }
    }
}