using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Injection.Bases
{
    public class UnityRemote : Jc.RemoteCaller
    {
        public string GetConfig()
        {
            return UnityCore.Config;
        }
    }
}