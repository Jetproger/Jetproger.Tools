using System;
using System.Collections;
using System.Threading;

namespace Jetproger.Tools.Resource.Bases
{
    public static partial class Toolx
    {
        public static ICommandLineArguments Console => null;

        public static IAppConfiguration Config => null;

        public static IResourceNames Name => null;

        public static IResourceSpecialNames SpecialName => null;

        public static IResourceDescriptions Description => null;

        public static IResourcePictures Picture => null;

        public static IResourceShortcuts Shortcut => null;
    }

    public interface ICommandLineArguments { }

    public interface IAppConfiguration { }

    public interface IResourceNames { }

    public interface IResourceSpecialNames { }

    public interface IResourceDescriptions { }

    public interface IResourcePictures { }

    public interface IResourceShortcuts { }
}