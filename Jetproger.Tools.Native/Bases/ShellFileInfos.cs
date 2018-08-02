namespace Jetproger.Tools.WinApi
{
    public static unsafe partial class Native
    {
        public static class ShellFileInfos
        {
            public const int SHGFI_ICON = 0x000000100;
            public const int SHGFI_DISPLAYNAME = 0x000000200;
            public const int SHGFI_TYPENAME = 0x000000400;
            public const int SHGFI_ATTRIBUTES = 0x000000800;
            public const int SHGFI_ICONLOCATION = 0x000001000;
            public const int SHGFI_EXETYPE = 0x000002000;
            public const int SHGFI_SYSICONINDEX = 0x000004000;
            public const int SHGFI_LINKOVERLAY = 0x000008000;
            public const int SHGFI_SELECTED = 0x000010000;
            public const int SHGFI_ATTR_SPECIFIED  = 0x000020000;
            public const int SHGFI_LARGEICON = 0x000000000;
            public const int SHGFI_SMALLICON = 0x000000001;
            public const int SHGFI_OPENICON = 0x000000002;
            public const int SHGFI_SHELLICONSIZE = 0x000000004;
            public const int SHGFI_PIDL = 0x000000008;
            public const int SHGFI_USEFILEATTRIBUTES = 0x000000010;
            public const int SHGFI_ADDOVERLAYS = 0x000000020;
            public const int SHGFI_OVERLAYINDEX = 0x000000040;
        }
    }
}