using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Bases
{
    public class CryExpander
    {
        public X509Certificate2 app { get { return CryptoExtensions.AppCertificate; } }
        public X509Certificate2 ext { get { return CryptoExtensions.OutCertificate; } } 
    }

    public static class CryptoExtensions
    {
        public static X509Certificate2 AppCertificate { get { return f.one.of(AppCertificateHolder, () => FindCertificateByThumbprint(null, k<AppCert>.key())); } }
        private static readonly X509Certificate2[] AppCertificateHolder = { null };

        public static X509Certificate2 OutCertificate { get { return f.one.of(OutCertificateHolder, () => FindCertificateByThumbprint(null, k<OutCert>.key())); } }
        private static readonly X509Certificate2[] OutCertificateHolder = { null };

        public static bool ExistsKeyContainer(this CryExpander exp)
        {
            try
            {
                return FindSignerCertificate() != null;
            }
            catch
            {
                return false;
            }
        }

        private static X509Certificate2 FindSignerCertificate()
        {
            var cert = FindCertificateByThumbprint(null, k<OutCert>.key());
            if (cert == null) throw new CertificateStoreException(k<OutCert>.key(), "OuterCert");
            cert = FindSignerCertInKeyContainer(null, cert);
            if (cert == null) throw new CertificateKeyException(k<OutCert>.key(), "OuterCert");
            return cert;
        }

        private static X509Certificate2 FindSignerCertInStoreMyByThumbprint()
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            if (!k<OutCert>.has) return null;
            var configThumbprint = (k<OutCert>.key() ?? string.Empty).ToUpper();
            if (string.IsNullOrWhiteSpace(configThumbprint)) return null;
            try
            {
                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    if ((certificate.Thumbprint ?? string.Empty).ToUpper() == configThumbprint) return certificate;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                store.Close();
            }
        }

        public static X509Certificate2 FindSignerCertInKeyContainer(this CryExpander exp, X509Certificate2 certificate)
        {
            if (certificate == null) return null;
            var certificateKey = certificate.PublicKey.EncodedKeyValue.RawData;
            var containers = GetContainers();
            foreach (var container in containers)
            {
                var containerKey = GetPublicKeyOfContainer(container);
                if (f.bin.equals(containerKey, certificateKey)) return certificate;
            }
            return null;
        }

        public static X509Certificate2 FindCertificateByThumbprint(this CryExpander exp, string thumbprint)
        {
            thumbprint = (thumbprint ?? string.Empty).ToUpper();
            if (string.IsNullOrWhiteSpace(thumbprint)) return null;
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            try
            {
                var thumbprintBytes = Encoding.UTF8.GetBytes(thumbprint.ToUpper());
                var cert = FindCertificateByThumbprint(store, thumbprintBytes);
                if (cert != null) return cert;
                thumbprintBytes = RemovePreamble(thumbprintBytes);
                cert = FindCertificateByThumbprint(store, thumbprintBytes);
                if (cert != null) return cert;
                thumbprintBytes = RemovePreamble(thumbprintBytes);
                cert = FindCertificateByThumbprint(store, thumbprintBytes);
                return cert;
            }
            catch
            {
                return null;
            }
            finally
            {
                store.Close();
            }
        }

        private static X509Certificate2 FindCertificateByThumbprint(X509Store store, byte[] thumbprintBytes)
        {
            foreach (X509Certificate2 certificate in store.Certificates)
            {
                var certificateThumbprintBytes = Encoding.UTF8.GetBytes(certificate.Thumbprint ?? string.Empty);
                if (f.bin.equals(thumbprintBytes, certificateThumbprintBytes)) return certificate;
            }
            return null;
        }

        private static byte[] RemovePreamble(byte[] inBytes)
        {
            var bytes = new byte[inBytes.Length - 3];
            for (int i = 0; i < bytes.Length; i++) bytes[i] = inBytes[i + 3];
            return bytes;
        }

        #region CryptoApi

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int PostMessage(int hWnd, uint dwMsg, int wParam, int lParam);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptAcquireContext(ref IntPtr hProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool CryptGetProvParam(IntPtr hProv, uint dwParam, [In, Out] byte[] pbData, ref uint dwDataLen, uint dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool CryptGetProvParam(IntPtr hProv, uint dwParam, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pbData, ref uint dwDataLen, uint dwFlags);

        [DllImport("Crypt32.dll", EntryPoint = "CryptExportPublicKeyInfo", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CryptExportPublicKeyInfo(IntPtr hProv, uint dwKeySpec, uint dwCertEncodingType, IntPtr pInfo, ref uint pcbInfo);

        [DllImport("Advapi32.dll", EntryPoint = "CryptReleaseContext", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CryptReleaseContext(IntPtr hProv, Int32 dwFlags);

        private static readonly uint CRYPT_PROV_TYPE = k<CryptProviderType>.key<uint>();
        public const uint CRYPT_NEWKEYSET = 0x8;
        public const uint CRYPT_DELETEKEYSET = 0x10;
        public const uint CRYPT_MACHINE_KEYSET = 0x20;
        public const uint CRYPT_SILENT = 0x40;
        public const uint CRYPT_DEFAULT_CONTAINER_OPTIONAL = 0x80;
        public const uint CRYPT_VERIFYCONTEXT = 0xF0000000;
        public const uint CRYPT_FQCN = 0x10;
        public const uint CRYPT_FIRST = 0x00000001;
        public const uint PP_ENUMREADERS = 114;
        public const uint PP_ENUMCONTAINERS = 0x2;
        public const uint AT_KEYEXCHANGE = 1;
        public const uint AT_SIGNATURE = 2;
        public const uint X509_ASN_ENCODING = 0x00000001;
        public const uint X509_NDR_ENCODING = 0x00000002;
        public const uint PKCS_7_ASN_ENCODING = 0x00010000;
        public const uint PKCS_7_NDR_ENCODING = 0x00020000;
        public const int WM_KEYDOWN = 0x0100;
        public const int VK_ENTER = 0x0D;
        public const int VK_TAB = 0x09;

        [StructLayout(LayoutKind.Sequential)]
        public struct CERT_PUBLIC_KEY_INFO
        {
            public CRYPT_ALGORITHM_IDENTIFIER Algorithm;
            public CRYPT_BIT_BLOB PublicKey;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPT_ALGORITHM_IDENTIFIER
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public String pszObjId;
            public CRYPT_OBJID_BLOB Parameters;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPT_BIT_BLOB
        {
            public uint cbData;
            public IntPtr pbData;
            public uint cUnusedBits;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPT_OBJID_BLOB
        {
            public uint cbData;
            public IntPtr pbData;
        }

        private static string[] GetContainers()
        {
            var containers = new List<string>();
            uint pdwDataLen = 0;
            var hProv = IntPtr.Zero;
            var dwFlags = CRYPT_FIRST;
            StringBuilder sb = null;
            try
            {
                CryptAcquireContext(ref hProv, null, null, CRYPT_PROV_TYPE, CRYPT_VERIFYCONTEXT);
                CryptGetProvParam(hProv, PP_ENUMCONTAINERS, sb, ref pdwDataLen, dwFlags);
                var buffsize = (int)(2 * pdwDataLen);
                sb = new StringBuilder(buffsize);
                while (CryptGetProvParam(hProv, PP_ENUMCONTAINERS, sb, ref pdwDataLen, dwFlags | CRYPT_FQCN))
                {
                    dwFlags = 0; //required to continue entire enumeration
                    containers.Add(sb.ToString());
                }
            }
            finally
            {
                if (hProv != IntPtr.Zero) CryptReleaseContext(hProv, 0);
            }
            return containers.ToArray();
        }

        private static byte[] GetPublicKeyOfContainer(string container)
        {
            var info = new CERT_PUBLIC_KEY_INFO();
            var pInfo = IntPtr.Zero;
            var hProv = IntPtr.Zero;
            uint pcbInfo = 0;
            try
            {
                var i = 0;
                CryptAcquireContext(ref hProv, container, null, CRYPT_PROV_TYPE, 0);
                CryptExportPublicKeyInfo(hProv, AT_KEYEXCHANGE, X509_ASN_ENCODING | PKCS_7_ASN_ENCODING, IntPtr.Zero, ref pcbInfo);
                pInfo = Marshal.AllocHGlobal((int)pcbInfo);
                Marshal.StructureToPtr(info, pInfo, false);
                CryptExportPublicKeyInfo(hProv, AT_KEYEXCHANGE, X509_ASN_ENCODING | PKCS_7_ASN_ENCODING, pInfo, ref pcbInfo);
                info = (CERT_PUBLIC_KEY_INFO)Marshal.PtrToStructure(pInfo, typeof(CERT_PUBLIC_KEY_INFO));
                var bytes = new byte[66];
                Marshal.Copy(info.PublicKey.pbData, bytes, 0, bytes.Length);
                return bytes;
            }
            finally
            {
                if (hProv != IntPtr.Zero) CryptReleaseContext(hProv, 0);
                Marshal.FreeHGlobal(pInfo);
            }
        }

        #endregion
    }
}

namespace Jetproger.Tools.AppConfig
{
    public class CryptProviderType : ConfigSetting { public CryptProviderType() : base("75") { } } 
}