using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Jetproger.Tools.Xml.Bases
{
    public static partial class XmlExtensions
    {
        private static class Graphics
        {
            private readonly static Image[] DefaultImage = { null };

            private readonly static Icon[] DefaultIcon = { null };

            public static string AsString(Image image)
            {
                return Convert.ToBase64String(AsBytes(image));
            }

            public static string AsString(Icon icon)
            {
                return Convert.ToBase64String(AsBytes(icon));
            }

            public static byte[] AsBytes(Image image)
            {
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }

            public static byte[] AsBytes(Icon icon)
            {
                using (var ms = new MemoryStream())
                {
                    icon.Save(ms);
                    return ms.ToArray();
                }
            }

            public static Icon AsIcon(string base64)
            {
                return AsIcon(Convert.FromBase64String(base64));
            }

            public static Icon AsIcon(byte[] bytes)
            {
                try
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        return new Icon(ms);
                    }
                }
                catch
                {
                    return GetDefaultIcon();
                }
            }

            public static Image AsImage(string base64)
            {
                return AsImage(Convert.FromBase64String(base64));
            }

            public static Image AsImage(byte[] bytes)
            {
                try
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        return Image.FromStream(ms, true);
                    }
                }
                catch
                {
                    return GetDefaultImage();
                }
            }

            public static Image GetDefaultImage()
            {
                return Methods.GetOne(DefaultImage, () => AsImage(ImageDefault));
            }

            public static Icon GetDefaultIcon()
            {
                return Methods.GetOne(DefaultIcon, () => AsIcon(IconDefault));
            }
        }
    }
}