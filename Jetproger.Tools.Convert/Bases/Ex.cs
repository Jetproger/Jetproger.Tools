using System;
using System.Runtime.InteropServices;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class Ex
    {
        public static ICacheExpander Cache => null;

        public static IDotnetExpander Dotnet => null;

        public static IImageExpander Image => null;

        public static IInjectionExpander Inject => null;

        public static IJsonExpander Json => null;

        public static IGuardExpander Guard => null;

        public static INativeExpander Native => null;

        public static IStringExpander String => null;

        public static ITraceExpander Trace => null;

        public static IValueExpander Value => null;

        public static IXmlExpander Xml => null;

        public static IUtilsExpander Utils => null;
    }

    public interface ICacheExpander { }

    public interface IDotnetExpander { }

    public interface IUtilsExpander { }

    public interface IGuardExpander { }

    public interface IImageExpander { }

    public interface IJsonExpander { }

    public interface INativeExpander { }

    public interface IStringExpander { }

    public interface ITraceExpander { }

    public interface IXmlExpander { }

    public interface IValueExpander { }

    public interface IInjectionExpander { }

    public static class Extensions
    {
        public static int GetLastError(this INativeExpander expander) { return GetLastError(); }

        [DllImport("kernel32.dll", EntryPoint = "GetLastError", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetLastError();
    }

    public class DefaultSettingFactory : ISettingFactory
    {
        public object CreateSetting(Type type) { return Activator.CreateInstance(type); }
    }

    public interface ISettingFactory
    {
        object CreateSetting(Type type);
    }

    public interface IEntityId
    {
        Guid GetEntityId();
    }

    public interface IParentId
    {
        Guid GetParentId();
    }

    public interface IDocumentId
    {
        Guid GetDocumentId();
    }
}