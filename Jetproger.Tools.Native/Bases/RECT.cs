using System.Runtime.InteropServices;

namespace Jetproger.Tools.WinApi
{
    public static unsafe partial class Native
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct RECT
        {
            [FieldOffset(0)] public int left;
            [FieldOffset(4)] public int top;
            [FieldOffset(8)] public int right;
            [FieldOffset(12)] public int bottom;
        }
    }
}