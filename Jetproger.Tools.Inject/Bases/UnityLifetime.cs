using System;

namespace Jetproger.Tools.Injection.Bases
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UnityLifetimeAttribute : Attribute
    {
        public string Lifetime
        {
            get; private set;
        }

        public UnityLifetimeAttribute(string lifetime)
        {
            Lifetime = lifetime;
        }
    }
}