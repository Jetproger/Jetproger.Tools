using System;
using System.Runtime.InteropServices;

namespace Jetproger.Tools.WinApi
{
    public static unsafe partial class Native
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPT_OBJID_BLOB
        {
            public uint cbData;
            public IntPtr pbData;
        }
    }
}