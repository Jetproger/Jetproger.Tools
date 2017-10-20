using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using Newtonsoft.Json;

namespace Tools
{
    public static partial class Resource
    {
        private static class Methods
        {
            private const string IconDefault = @"AAABAAEAEBAAAAAAGABoAwAAFgAAACgAAAAQAAAAIAAAAAEAGAAAAAAAAAMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACyYzmARyl9RSh7RCd6RCd3QiZyQCVvPiRsPCNpOyJmOSFjNyBhNh9gNR8AAAAAAAC4ZjvYoIPYoIPQjmvHeE+4ZjueVzKGSit4QiZ3QiZ4QiZ3QiZbMh1hNh8AAAAAAAC/aj3oybjiuaTbqY7TlXTJfVW+aj2eVzKDSSp4QiZsPCOMTi17RCdjNyAAAAAAAADEcUXht6Hw29DiuaTZo4fQj2zHeE62ZTqSUS9jNyDCbD67aDwAAABmOSEAAAAAAADHeE/Mg13iuaTw29DiuaTYoIPOiWTDb0KSUS/Zo4fSknEAAADOiGNpOyIAAAAAAADLgVrz4djKf1fiuaPw2s/ht6HUl3egWTPhtqDjvKcAAADiuaPQjmtsPCMAAAAAAADOimb+/fzz4djFc0jiuaPlwK2HSyvbqY59RSh9RSjdrZTnxrXTlXRvPiQAAAAAAADRkW/+/fz+/fzw29DFc0jFc0ju1snz49rw29Dt1Mfrz8HpyrrWnH1yQCUAAAAAAADUmHj+/fz+/fz+/fz9+vj68/D47+rnxLKyYzmtYDfhtqDrz8HZo4Z3QiYAAAAAAADXn4H+/fz+/fz+/fz+/fz9+vj68/DDb0LbqY/FdEmtYDft1Mfcq5F6RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz9+vjEcUXu1srbqY+yYznw29Dfspp7RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fzw29DEcUXDb0LnxLLz49rhtqB9RSgAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fz+/fz9+vj68/D47+r16OHz49qARykAAAAAAADZo4bZo4bZo4bZo4bXn4HUmHjRkW/OimbLgVrHeE/EcUW/aj24ZjuyYzkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//wAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAAD//wAA";
            private const string ImageDefault = @"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIDSURBVDhPpZLrS5NhGMb3j4SWh0oRQVExD4gonkDpg4hGYKxG6WBogkMZKgPNCEVJFBGdGETEvgwyO9DJE5syZw3PIlPEE9pgBCLZ5XvdMB8Ew8gXbl54nuf63dd90OGSnwCahxbPRNPAPMw9Xpg6ZmF46kZZ0xSKzJPIrhpDWsVnpBhGkKx3nAX8Pv7z1zg8OoY/cITdn4fwbf/C0kYAN3Ma/w3gWfZL5kzTKBxjWyK2DftwI9tyMYCZKXbNHaD91bLYJrDXsYbrWfUKwJrPE9M2M1OcVzOOpHI7Jr376Hi9ogHqFIANO0/MmmmbmSmm9a8ze+I4MrNWAdjtoJgWcx+PSzg166yZZ8xM8XvXDix9c4jIqFYAjoriBV9AhEPv1mH/sonogha0afbZMMZz+yreTGyhpusHwtNNCsA5U1zS4BLxzJIfg299qO32Ir7UJtZfftyATqeT+8o2D8JSjQrAJblrncYL7ZJ2+bfaFnC/1S1NjL3diRat7qrO7wLRP3HjWsojBeComDEo5mNjuweFGvjWg2EBhCbpkW78htSHHwRyNdmgAFzPEee2iFkzayy2OLXzT4gr6UdUnlXrullsxxQ+kx0g8BTA3aZlButjSTyjODq/WcQcW/B/Je4OQhLvKQDnzN1mp0nnkvAhR8VuMzNrpm1mpjgkoVwB/v8DTgDQASA1MVpwzwAAAABJRU5ErkJggg==";
            private readonly static Image[] DefaultImage = { null };
            private readonly static Icon[] DefaultIcon = { null };

            private static readonly JsonSerializer JsonSerializer = new JsonSerializer {
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            private static readonly HashSet<Type> SimpleTypes = new HashSet<Type> {
                typeof(bool), typeof(bool?),
                typeof(byte), typeof(byte?),
                typeof(sbyte), typeof(sbyte?),
                typeof(char), typeof(char?),
                typeof(short), typeof(short?), typeof(ushort), typeof(ushort?),
                typeof(int), typeof(int?), typeof(uint), typeof(uint?),
                typeof(IntPtr), typeof(IntPtr?), typeof(UIntPtr), typeof(UIntPtr?),
                typeof(long), typeof(long?), typeof(ulong), typeof(ulong?),
                typeof(Guid), typeof(Guid?),
                typeof(float), typeof(float?),
                typeof(decimal), typeof(decimal?),
                typeof(double), typeof(double?),
                typeof(DateTime), typeof(DateTime?),
                typeof(char[]), typeof(byte[]),
                typeof(string), typeof(string[]),
            };

            public static bool IsSimple(Type type)
            {
                return type != null && (type.IsEnum || SimpleTypes.Contains(type));
            }

            public static bool IsTypeOf(Type type, Type sample)
            {
                return type == sample || type.IsSubclassOf(sample) || type.GetInterfaces().Any(interfaceType => interfaceType == sample);
            }

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

            public static byte[] GetStringAsBytes(string s)
            {
                if (s == null) return null;
                s = s.Trim(' ', '\r', '\n', '\t');
                if (s == "" || s == "0x") return new byte[0];
                if (!s.StartsWith("0x") || s.Length % 2 != 0) return Encoding.Unicode.GetBytes(s);
                try
                {
                    var bytes = new List<byte>();
                    var max = s.Length / 2;
                    for (int i = 1; i < max; i++)
                    {
                        var k = 2 * i;
                        var valueString = $"{s[k]}{s[k + 1]}";
                        var valueByte = Convert.ToByte(valueString, 16);
                        bytes.Add(valueByte);
                    }
                    return bytes.ToArray();
                }
                catch
                {
                    return Encoding.Unicode.GetBytes(s);
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

            public static string SerializeJson(object o)
            {
                if (o == null)
                {
                    return null;
                }
                using (var sw = new UTF8StringWriter())
                {
                    if (IsSimple(o.GetType())) sw.Write(o.AsString()); else JsonSerializer.Serialize(sw, o);
                    return sw.ToString();
                }
            }

            public static object DeserializeJson(string json, Type resultType)
            {
                using (var sr = new StringReader(json))
                {
                    return JsonSerializer.Deserialize(sr, resultType);
                }
            }

            private class UTF8StringWriter : StringWriter
            {
                public override Encoding Encoding => Encoding.UTF8;
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
                if (key == null || key == DBNull.Value) return string.Empty;
                var type = key.GetType();
                if (!IsSimple(type) || type.IsArray) return key.ToString();
                return key.AsString();
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
                return GetOne(DefaultImage, () => AsImage(ImageDefault));
            }

            public static Icon GetDefaultIcon()
            {
                return GetOne(DefaultIcon, () => AsIcon(IconDefault));
            }

            public static T GetOne<T>(T[] holder, Func<T> factory) where T : class
            {
                if (holder[0] == null)
                {
                    lock (holder)
                    {
                        if (holder[0] == null) holder[0] = factory();
                    }
                }
                return holder[0];
            }

            public static T[] SetLen<T>(T[] array, int length)
            {
                if (length < 1 || array.Length == length) return array;
                var newArray = new T[length];
                for (int i = 0; i < newArray.Length; i++) newArray[i] = i < array.Length ? array[i] : default(T);
                return newArray;
            }

            public static bool TryStringAs<T>(string inValue, T defaultValue, out T value)
            {
                try
                {
                    value = inValue.As<T>();
                    return true;
                }
                catch
                {
                    value = defaultValue;
                    return false;
                }
            }
        }
    }
}