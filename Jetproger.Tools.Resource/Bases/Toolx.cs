namespace Jetproger.Tools.Resource.Bases
{
    public static class Toolx
    {
        public static ICommandLineArguments Args => null;

        public static IAppConfiguration Conf => null;

        public static IResourceNames Name => null;

        public static IResourceSpecialNames Spec => null;

        public static IResourceDescriptions Note => null;

        public static IResourcePictures Icon => null;

        public static IResourceShortcuts Keys => null;
    }

    public interface ICommandLineArguments { }

    public interface IAppConfiguration { }

    public interface IResourceNames { }

    public interface IResourceSpecialNames { }

    public interface IResourceDescriptions { }

    public interface IResourcePictures { }

    public interface IResourceShortcuts { }
}