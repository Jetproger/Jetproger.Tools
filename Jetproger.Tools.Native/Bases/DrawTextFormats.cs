namespace Tools
{
    public static unsafe partial class Native
    {
        public static class DrawTextFormats
        {
            public const uint DT_TOP = 0x00000000;
            public const uint DT_LEFT = 0x00000000;
            public const uint DT_CENTER = 0x00000001;
            public const uint DT_RIGHT = 0x00000002;
            public const uint DT_VCENTER = 0x00000004;
            public const uint DT_BOTTOM = 0x00000008;
            public const uint DT_WORDBREAK = 0x00000010;
            public const uint DT_SINGLELINE = 0x00000020;
            public const uint DT_EXPANDTABS = 0x00000040;
            public const uint DT_TABSTOP = 0x00000080;
            public const uint DT_NOCLIP = 0x00000100;
            public const uint DT_EXTERNALLEADING = 0x00000200;
            public const uint DT_CALCRECT = 0x00000400;
            public const uint DT_NOPREFIX = 0x00000800;
            public const uint DT_INTERNAL = 0x00001000;
            public const uint DT_EDITCONTROL = 0x00002000;
            public const uint DT_PATH_ELLIPSIS = 0x00004000;
            public const uint DT_END_ELLIPSIS = 0x00008000;
            public const uint DT_MODIFYSTRING = 0x00010000;
            public const uint DT_RTLREADING = 0x00020000;
            public const uint DT_WORD_ELLIPSIS = 0x00040000;
            public const uint DT_NOFULLWIDTHCHARBREAK = 0x00080000;
            public const uint DT_HIDEPREFIX = 0x00100000;
            public const uint DT_PREFIXONLY = 0x00200000;
        }
    }
}