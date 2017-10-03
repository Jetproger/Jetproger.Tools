namespace Tools
{
    public static unsafe partial class Native
    {
        public static class WindowStyles
        {
            public const long WS_OVERLAPPED = 0x00000000L;
            public const long WS_POPUP = 0x80000000L;
            public const long WS_CHILD = 0x40000000L;
            public const long WS_MINIMIZE = 0x20000000L;
            public const long WS_VISIBLE = 0x10000000L;
            public const long WS_DISABLED = 0x08000000L;
            public const long WS_CLIPSIBLINGS = 0x04000000L;
            public const long WS_CLIPCHILDREN = 0x02000000L;
            public const long WS_MAXIMIZE = 0x01000000L;
            public const long WS_CAPTION = 0x00C00000L;
            public const long WS_BORDER = 0x00800000L;
            public const long WS_DLGFRAME = 0x00400000L;
            public const long WS_VSCROLL = 0x00200000L;
            public const long WS_HSCROLL = 0x00100000L;
            public const long WS_SYSMENU = 0x00080000L;
            public const long WS_THICKFRAME = 0x00040000L;
            public const long WS_GROUP = 0x00020000L;
            public const long WS_TABSTOP = 0x00010000L;
            public const long WS_MINIMIZEBOX = 0x00020000L;
            public const long WS_MAXIMIZEBOX = 0x00010000L;
            public const long WS_TILED = WS_OVERLAPPED;
            public const long WS_ICONIC = WS_MINIMIZE;
            public const long WS_SIZEBOX = WS_THICKFRAME;
            public const long WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;
            public const long WS_OVERLAPPEDWINDOW =(WS_OVERLAPPED     | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
            public const long WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU);
            public const long WS_CHILDWINDOW = (WS_CHILD);
            public const long WS_EX_DLGMODALFRAME = 0x00000001L;
            public const long WS_EX_NOPARENTNOTIFY = 0x00000004L;
            public const long WS_EX_TOPMOST = 0x00000008L;
            public const long WS_EX_ACCEPTFILES = 0x00000010L;
            public const long WS_EX_TRANSPARENT = 0x00000020L;
            public const long WS_EX_MDICHILD = 0x00000040L;
            public const long WS_EX_TOOLWINDOW = 0x00000080L;
            public const long WS_EX_WINDOWEDGE = 0x00000100L;
            public const long WS_EX_CLIENTEDGE = 0x00000200L;
            public const long WS_EX_CONTEXTHELP = 0x00000400L;
            public const long WS_EX_RIGHT = 0x00001000L;
            public const long WS_EX_LEFT = 0x00000000L;
            public const long WS_EX_RTLREADING = 0x00002000L;
            public const long WS_EX_LTRREADING = 0x00000000L;
            public const long WS_EX_LEFTSCROLLBAR = 0x00004000L;
            public const long WS_EX_RIGHTSCROLLBAR = 0x00000000L;
            public const long WS_EX_CONTROLPARENT = 0x00010000L;
            public const long WS_EX_STATICEDGE = 0x00020000L;
            public const long WS_EX_APPWINDOW = 0x00040000L;
            public const long WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
            public const long WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
            public const long WS_EX_LAYERED = 0x00080000;
            public const long WS_EX_NOINHERITLAYOUT = 0x00100000L;
            public const long WS_EX_NOREDIRECTIONBITMAP = 0x00200000L;
            public const long WS_EX_LAYOUTRTL = 0x00400000L;
            public const long WS_EX_COMPOSITED = 0x02000000L;
            public const long WS_EX_NOACTIVATE = 0x08000000L;
        }
    }
}