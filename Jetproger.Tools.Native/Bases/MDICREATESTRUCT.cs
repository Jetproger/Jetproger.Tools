using System;
using System.Runtime.InteropServices;

namespace Tools
{
    public static unsafe partial class Native
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct MDICREATESTRUCT
        {
            public char* szClass;
            public char* szTitle;
            public IntPtr hOwner;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint style;
            public int lParam;
        }
    }
}