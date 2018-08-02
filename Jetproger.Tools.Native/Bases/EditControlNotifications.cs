namespace Jetproger.Tools.WinApi
{
    public static unsafe partial class Native
    {
        public static class EditControlNotifications
        {
            public const uint EN_SETFOCUS = 0x0100;
            public const uint EN_KILLFOCUS = 0x0200;
            public const uint EN_CHANGE = 0x0300;
            public const uint EN_UPDATE = 0x0400;
            public const uint EN_ERRSPACE = 0x0500;
            public const uint EN_MAXTEXT = 0x0501;
            public const uint EN_HSCROLL = 0x0601;
            public const uint EN_VSCROLL = 0x0602;
            public const uint EN_ALIGN_LTR_EC = 0x0700;
            public const uint EN_ALIGN_RTL_EC = 0x0701;
        }
    }
}