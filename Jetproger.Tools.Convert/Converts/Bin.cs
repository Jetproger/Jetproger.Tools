using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace Jetproger.Tools.Convert.Converts
{
    public static class BinExtensions
    {
        public static byte[] Of(this Jc.IBinExpander exp, object value)
        {
            return Jc.Bin<Jc.BaseBin>.Of(value);
        }

        public static TOut To<TOut>(this Jc.IBinExpander exp, byte[] bytes)
        {
            return Jc.Bin<Jc.BaseBin>.To<TOut>(bytes);
        }

        public static object To(this Jc.IBinExpander exp, byte[] bytes, Type type)
        {
            return Jc.Bin<Jc.BaseBin>.To(bytes, type);
        }

        public static Guid HashOf(this Jc.IBinExpander exp, object value)
        {
            var bytes = Of(exp, value);
            return HashOf(exp, bytes);
        }

        public static Guid HashOf(this Jc.IBinExpander exp, byte[] bytes)
        {
            bytes = MD5.Create().ComputeHash(bytes);
            return new Guid(bytes);
        }

        public static byte[] Compress(this Jc.IBinExpander exp, byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return new byte[0];
            using (var ms = new MemoryStream())
            {
                using (var zip = new GZipStream(ms, CompressionMode.Compress))
                {
                    zip.Write(bytes, 0, bytes.Length);
                }
                return ms.ToArray();
            }
        }

        public static byte[] Decompress(this Jc.IBinExpander exp, byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return new byte[0];
            using (var source = new MemoryStream())
            {
                source.Write(bytes, 0, bytes.Length);
                source.Position = 0;

                using (var zip = new GZipStream(source, CompressionMode.Decompress))
                {
                    using (var target = new MemoryStream())
                    {
                        var buffer = new byte[1024];
                        while (true)
                        {
                            int length = zip.Read(buffer, 0, buffer.Length);
                            if (length == 0) break;
                            target.Write(buffer, 0, length);
                        }
                        return target.ToArray();
                    }
                }
            }
        }
    }
}