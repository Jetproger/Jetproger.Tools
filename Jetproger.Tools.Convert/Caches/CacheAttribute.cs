namespace Jetproger.Tools.Convert.Caches
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class CacheAttribute : System.Attribute
    {
        public string Lifetime { get; private set; }

        public CacheAttribute(string lifetime) { Lifetime = lifetime; }
    }
}