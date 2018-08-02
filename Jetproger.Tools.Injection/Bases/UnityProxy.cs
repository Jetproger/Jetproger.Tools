using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Injection.Bases
{
    public class UnityProxy : OneProxy
    {
        public string GetUnityConfigXml()
        {
            return UnityXml.Xml;
        }
    }
}