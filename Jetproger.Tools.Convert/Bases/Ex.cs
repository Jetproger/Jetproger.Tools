using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Ex
    {
        public static ICacheExpander Cache => null;

        public static IDotnetExpander Dotnet => null;

        public static IImageExpander Image => null;

        public static IJsonExpander Json => null;

        public static IErrorExpander Error => null;

        public static INativeExpander Native => null;

        public static IStringExpander String => null;

        public static ITraceExpander Trace => null;

        public static IValueExpander Value => null;

        public static IXmlExpander Xml => null;

        public static IUtilsExpander Utils => null;

        public static class Res
        {
            public static ICommandLineArgumentExpander Args => null;

            public static IAppConfigurationExpander Conf => null;

            public static IResourceNameExpander Name => null;

            public static IResourceSpecialNameExpander Spec => null;

            public static IResourceDescriptionExpander Note => null;

            public static IResourcePictureExpander Icon => null;

            public static IResourceShortcutExpander Keys => null;
        }
    }

    public interface ICacheExpander { }

    public interface IDotnetExpander { }

    public interface IUtilsExpander { }

    public interface IErrorExpander { }

    public interface IImageExpander { }

    public interface IJsonExpander { }

    public interface INativeExpander { }

    public interface IStringExpander { }

    public interface ITraceExpander { }

    public interface IXmlExpander { }

    public interface IValueExpander { }

    public interface ICommandLineArgumentExpander { }

    public interface IAppConfigurationExpander { }

    public interface IResourceNameExpander { }

    public interface IResourceSpecialNameExpander { }

    public interface IResourceDescriptionExpander { }

    public interface IResourcePictureExpander { }

    public interface IResourceShortcutExpander { }

    public static class Extensions
    {
        public static void Write(this ITraceExpander expander, object message)
        {
            Trace.WriteLine(message);
        }

        public static int GetLastError(this INativeExpander expander)
        {
            return GetLastError();
        }

        [DllImport("kernel32.dll", EntryPoint = "GetLastError", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetLastError();
    }
}