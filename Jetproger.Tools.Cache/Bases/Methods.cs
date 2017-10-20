﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;

namespace Tools
{
    public static partial class Cache
    {
        private static class Methods
        {
            public static readonly CultureInfo Formatter = new CultureInfo("en-us")
            {
                NumberFormat =
                {
                    NumberGroupSeparator = string.Empty,
                    NumberDecimalSeparator = "."
                },
                DateTimeFormat =
                {
                    DateSeparator = "-",
                    TimeSeparator = ":"
                }
            };

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

            public static string GetBytesAsString(IEnumerable<byte> bytes)
            {
                var sb = new StringBuilder("0x");
                foreach (byte b in bytes)
                {
                    string s = Convert.ToString(b, 16);
                    sb.AppendFormat("{0}{1}", s.Length < 2 ? "0" : "", s);
                }
                return sb.ToString();
            }

            public static string GetExceptionAsString(Exception e)
            {
                var sb = new StringBuilder();
                sb.AppendLine(e.ToString());
                while (e != null)
                {
                    sb.AppendLine(e.Message);
                    e = e.InnerException;
                }
                return sb.ToString();
            }

            public static string[] GetStringKeys(object[] keys)
            {
                if (keys == null) return new string[0];
                var stringKeys = new string[keys.Length];
                var i = 0;
                foreach (var key in keys)
                {
                    stringKeys[i++] = GetStringKey(key);
                }
                return stringKeys;
            }

            private static string GetStringKey(object key)
            {
                return key != null && key != DBNull.Value ? key.AsString() : string.Empty;
            }
        }

        private static string AsString(this object value)
        {
            if (value == null || value == DBNull.Value) return default(string);
            if (value is string) return (string)value;
            if (value is bool) return (bool)value ? "1" : "0";
            if (value is long) return ((long)value).ToString("#################0", Methods.Formatter);
            if (value is decimal) return ((decimal)value).ToString("#################0.00", Methods.Formatter);
            if (value is int) return ((int)value).ToString("#################0", Methods.Formatter);
            if (value is short) return ((short)value).ToString("#################0", Methods.Formatter);
            if (value is byte) return ((byte)value).ToString("#################0", Methods.Formatter);
            if (value is ulong) return ((ulong)value).ToString("#################0", Methods.Formatter);
            if (value is uint) return ((uint)value).ToString("#################0", Methods.Formatter);
            if (value is ushort) return ((ushort)value).ToString("#################0", Methods.Formatter);
            if (value is sbyte) return ((sbyte)value).ToString("#################0", Methods.Formatter);
            if (value is float) return ((float)value).ToString("#################0.00", Methods.Formatter);
            if (value is double) return ((double)value).ToString("#################0.00", Methods.Formatter);
            if (value is Guid) return ((Guid)value).ToString();
            if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fff", Methods.Formatter);
            if (value is char) return ((char)value).ToString(CultureInfo.InvariantCulture);
            if (value is char[]) return string.Concat((char[])value);
            if (value is byte[]) return Methods.GetBytesAsString((byte[])value);
            if (value is Icon) return Methods.AsString((Icon)value);
            if (value is Image) return Methods.AsString((Image)value);
            if (value is Exception) return Methods.GetExceptionAsString((Exception)value);
            return value.ToString();
        }
    }
}