namespace Jetproger.Tools.WinApi
{
    public static unsafe partial class Native
    {
        public static class CryptCodes
        {
            public const uint CRYPT_NEWKEYSET = 0x8;
            public const uint CRYPT_DELETEKEYSET = 0x10;
            public const uint CRYPT_MACHINE_KEYSET = 0x20;
            public const uint CRYPT_SILENT = 0x40;
            public const uint CRYPT_DEFAULT_CONTAINER_OPTIONAL = 0x80;
            public const uint CRYPT_VERIFYCONTEXT = 0xF0000000;
            public const uint CRYPT_FQCN = 0x10;
            public const uint CRYPT_FIRST = 0x00000001;
            public static readonly uint CRYPT_PROV_TYPE = 75;//йпхорн-опн
            public const uint PP_ENUMREADERS = 114;
            public const uint PP_ENUMCONTAINERS = 0x2;
            public const uint AT_KEYEXCHANGE = 1;
            public const uint AT_SIGNATURE = 2;
            public const uint X509_ASN_ENCODING = 0x00000001;
            public const uint X509_NDR_ENCODING = 0x00000002;
            public const uint PKCS_7_ASN_ENCODING = 0x00010000;
            public const uint PKCS_7_NDR_ENCODING = 0x00020000;
        }
    }
}