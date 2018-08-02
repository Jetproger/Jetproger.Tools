using System;

namespace Jetproger.Tools.WinApi
{
    public static unsafe partial class Native
    {
        public struct TVINSERTSTRUCT
        {
            public IntPtr hParent;
            public IntPtr hInsertAfter;
            public TVITEM item;
        }
    }
}