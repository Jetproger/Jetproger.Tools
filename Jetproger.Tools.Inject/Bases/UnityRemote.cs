namespace Jetproger.Tools.Inject.Bases
{
    public class UnityRemote : Jc.RemoteCaller
    {
        public string GetConfig()
        {
            return UnityCore.Config;
        }
    }
}