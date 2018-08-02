using System.Runtime.InteropServices;

namespace Jetproger.Tools.WinApi
{
    public static unsafe partial class Native
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct CERT_PUBLIC_KEY_INFO
        {
            public CRYPT_ALGORITHM_IDENTIFIER Algorithm;
            public CRYPT_BIT_BLOB PublicKey;
        }
    }
}