using System;

namespace Jetproger.Tools.Inject.Bases
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UnityMapAttribute : Attribute
    {
        public string MapOf { get; private set; }

        public UnityMapAttribute(string mapOf)
        {
            MapOf = mapOf;
        }
    }
}