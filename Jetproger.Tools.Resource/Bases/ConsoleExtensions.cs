using Res = Tools.Resource;

namespace Jetproger.Tools.Resource.Bases
{
    public static class ConsoleExtesions
    {
        public static bool Install(this ICommandLineArguments console)
        {
            var item = Res.GetCommandLineValue<string>("install");
            if (item.IsDeclared) return true;
            item = Res.GetCommandLineValue<string>("i");
            if (item.IsDeclared) return true;
            return false;
        }

        public static bool Uninstall(this ICommandLineArguments console)
        {
            var item = Res.GetCommandLineValue<string>("uninstall");
            if (item.IsDeclared) return true;
            item = Res.GetCommandLineValue<string>("u");
            if (item.IsDeclared) return true;
            return false;
        }
    }
}