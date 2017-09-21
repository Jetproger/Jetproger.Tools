using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Jetproger.Tools.Convert.Bases
{
    public static partial class ConvertExtensions
    {
        private static class Graphics
        {
            private readonly static Image[] DefaultImage = { null };

            private readonly static Icon[] DefaultIcon = { null };

            public static string AsString(Image image)
            {
                return System.Convert.ToBase64String(AsBytes(image));
            }

            public static string AsString(Icon icon)
            {
                return System.Convert.ToBase64String(AsBytes(icon));
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

            public static Icon AsIcon(Image image)
            {
                return AsIcon(image, image.Size.Width, image.Size.Height);
            }

            public static Icon AsIcon(Image image, int width, int height)
            {
                try
                {
                    var bmp = new Bitmap(image, new Size(width, height));
                    bmp.MakeTransparent(Color.Magenta);
                    var hIcon = bmp.GetHicon();
                    return Icon.FromHandle(hIcon);
                }
                catch
                {
                    return GetDefaultIcon();
                }
            }

            public static Icon AsIcon(string base64)
            {
                return AsIcon(System.Convert.FromBase64String(base64));
            }

            public static Icon AsIcon(byte[] bytes)
            {
                using (var ms = new MemoryStream(bytes))
                {
                    return new Icon(ms);
                }
            }

            public static Image AsImage(string base64)
            {
                return AsImage(System.Convert.FromBase64String(base64));
            }

            public static Image AsImage(byte[] bytes)
            {
                using (var ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms, true);
                }
            }

            public static Image GetDefaultImage()
            {
                return Supports.GetOne(DefaultImage, () => AsImage(ImageDefault));
            }

            public static Icon GetDefaultIcon()
            {
                return Supports.GetOne(DefaultIcon, () => AsIcon(IconDefault));
            }
        }
    }
}