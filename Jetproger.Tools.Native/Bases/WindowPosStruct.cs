using System;
using System.Runtime.InteropServices;

namespace Tools
{
    public static unsafe partial class Native
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct WindowPosStruct
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }
	}
}