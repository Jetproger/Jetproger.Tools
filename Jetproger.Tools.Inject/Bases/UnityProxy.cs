using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Injection.Bases
{
    public class UnityProxy : OneProxy
    {
        public string GetConfig()
        {
            return UnityCore.Config;
        }
    }
}