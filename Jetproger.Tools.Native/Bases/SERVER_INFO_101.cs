using System;
using System.Runtime.InteropServices;

namespace Tools
{
    public static unsafe partial class Native
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SERVER_INFO_101
        {
            public int dwPlatformID;
            public IntPtr lpszServerName;
            public int dwVersionMajor;
            public int dwVersionMinor;
            public int dwType;
            public IntPtr lpszComment;
        }
    }
}