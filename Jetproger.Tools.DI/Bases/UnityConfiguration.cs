using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Jetproger.Tools.Plugin.DI
{
    [XmlRoot("configuration")]
    public class UnityConfiguration
    {
        public UnityConfiguration()
        {
            unity = new Unity();
            configSections = new List<Section>();
            configSections.Add(new Section { name = "unity", type = "Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Microsoft.Practices.Unity.Configuration" });
        }

        public List<Section> configSections { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/practices/2010/unity")]
        public Unity unity { get; set; }

        public string ToXml()
        {
            using (var sw = new StringWriter())
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);
                var xs = new XmlSerializer(GetType());
                xs.Serialize(sw, this, ns);
                return sw.ToString();
            }
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
            SectionExtensions.Add(new SectionExtension { type = "Microsoft.Practices.Unity.InterceptionExtension.Configuration.InterceptionConfigurationExtension, Microsoft.Practices.Unity.Interception.Configuration" });
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
        public Interception()
        {
            Policies = new List<Policy>();
        }

        [XmlElement("policy")]
        public List<Policy> Policies { get; set; }
    }

    public class Policy
    {
        public Policy()
        {
            MatchingRules = new List<MatchingRule>();
        }

        [XmlElement("matchingRule")]
        public List<MatchingRule> MatchingRules { get; set; }

        public void AddRule(Type type)
        {
            var rule = new MatchingRule { name = "type-name", type = "TypeMatchingRule" };
            rule.constructor.Params.Add(new Param { name = "typeName", value = type.FullName });
            MatchingRules.Add(rule);
        }

        public void AddRule(string methodName)
        {
            var rule = new MatchingRule { name = "method-name", type = "MemberNameMatchingRule" };
            rule.constructor.Params.Add(new Param { name = "nameToMatch", value = methodName });
            MatchingRules.Add(rule);
        }
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
            Params.Add(new Param { name = "ignoreCase", value = "false" });
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

    [AttributeUsage(AttributeTargets.Class)]
    public class MapOfDependencyInjectionAttribute : Attribute
    {
        public string MapOf { get; private set; }
        public MapOfDependencyInjectionAttribute(string mapOf) { MapOf = mapOf; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class LifetimeDependencyInjectionAttribute : Attribute
    {
        public string Lifetime { get; private set; }
        public LifetimeDependencyInjectionAttribute(string lifetime) { Lifetime = lifetime; }
    }
}