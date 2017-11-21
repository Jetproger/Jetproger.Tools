using Res = Tools.Resource;

namespace Jetproger.Tools.Resource.Bases
{
    public static class ConfigExtesions
    {
        public static string ConnectionString(this IAppConfiguration config)
        {
            return Res.GetConfigurationValue<string>("ConnectionString").Value;
        }

        public static int HostPort(this IAppConfiguration config)
        {
            return Res.GetConfigurationValue<int>("HostPort", 1234).Value;
        }

        public static string Culture(this IAppConfiguration config)
        {
            return Res.GetConfigurationValue<string>("Culture", "en-US").Value;
        }
    }
}