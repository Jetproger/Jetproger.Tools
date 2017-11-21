using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace Tools
{
    public static partial class File
    {
        public static void ToJson(object o, string fileName)
        {
            if (o == null) return;
            using (var sw = new StreamWriter(FileNameAsPathFileExt(fileName), false, Encoding.UTF8))
            {
                if (o.GetType().IsSimple()) sw.Write(o.AsString()); else JsonSerializer.Serialize(sw, o);
            }
        }

        public static void ToXml(object o, string fileName)
        {
            if (o == null) return;
            using (var sw = new StreamWriter(FileNameAsPathFileExt(fileName), false, Encoding.UTF8))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);
                var xs = new XmlSerializer(o.GetType());
                xs.Serialize(sw, o, ns);
            }
        }

        public static T OfJson<T>(string fileName)
        {
            return (T)OfJson(fileName, typeof(T));
        }

        public static object OfJson(string fileName, Type type)
        {
            var fullName = FileNameAsPathFileExt(fileName);
            if (!System.IO.File.Exists(fullName))
            {
                return Default(type);
            }
            using (var sr = new StreamReader(fullName, Encoding.UTF8))
            {
                return type.IsSimple() ? sr.ReadToEnd().As(type) : JsonSerializer.Deserialize(sr, type);
            }
        }

        public static T OfXml<T>(string fileName)
        {
            return (T)OfXml(fileName, typeof(T));
        }

        public static object OfXml(string fileName, Type type)
        {
            var fullName = FileNameAsPathFileExt(fileName);
            if (!System.IO.File.Exists(fullName))
            {
                return Default(type);
            }
            using (var sr = new StreamReader(fullName, Encoding.UTF8))
            {
                return (new XmlSerializer(type)).Deserialize(sr);
            }
        }

        public static string FileNameAsFileExt(string fileName)
        {
            return Path.GetFileName(FileNameAsPathFileExt(fileName));
        }

        public static string FileNameAsFile(string fileName)
        {
            return Path.GetFileNameWithoutExtension(FileNameAsPathFileExt(fileName));
        }

        public static string FileNameAsExt(string fileName)
        {
            var ext = Path.GetExtension(FileNameAsPathFileExt(fileName)) ?? string.Empty;
            if (ext.StartsWith(".")) ext = ext.Substring(1);
            return ext;
        }

        public static string FileNameAsFolder(string fileName)
        {
            return new DirectoryInfo(FileNameAsPath(fileName)).Name;
        }

        public static string FileNameAsPath(string fileName)
        {
            return Path.GetDirectoryName(FileNameAsPathFileExt(fileName));
        }

        public static string FileNameAsPathFileExt(string fileName)
        {
            fileName = fileName ?? string.Empty;
            var args = fileName.Split(new [] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0) return Path.Combine(AppDir(), TempFile());
            if (args.Length == 1 && !fileName.EndsWith("\\")) return Path.Combine(AppDir(), IsValidFileName(args[0]) ? args[0] : TempFile());
            if (fileName.EndsWith("\\"))
            {
                var newArgs = new string[args.Length + 1];
                for (var i = 0; i < args.Length; i++) newArgs[i] = args[i];
                newArgs[newArgs.Length - 1] = TempFile();
                args = newArgs;
            }
            var file = IsValidFileName(args[args.Length - 1]) ? args[args.Length - 1] : TempFile();
            var root = Path.IsPathRooted(args[0]) ? args[0] : AppDir();
            var full = root + "\\";
            if (fileName.StartsWith("\\\\")) full = "\\\\" + full;
            foreach (var arg in args)
            {
                if (arg == root || arg == file)
                {
                    continue;
                }
                if (!IsValidPath(arg))
                {
                    full = root;
                    break;
                }
                full = Path.Combine(full, arg);
            }
            return Path.Combine(full, file);
        }

        public static string AppDir()
        {
            return (HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("~"));
        }

        public static string AppFile()
        {
            return (HttpContext.Current == null ? Process.GetCurrentProcess().MainModule.FileName : null);
        }

        public static string TempFile()
        {
            return $"{Guid.NewGuid()}.tmp";
        }

        public static bool ExistsFile(string fileName)
        {
            return System.IO.File.Exists(FileNameAsPathFileExt(fileName));
        }

        public static Task<bool> RemoveFile(string fileName)
        {
            return Task<bool>.Factory.StartNew(() => TryRemoveFile(FileNameAsPathFileExt(fileName)));
        }

        public static bool ExistsFolder(string fileName)
        {
            return Directory.Exists(FileNameAsPath(fileName));
        }

        public static void CreateFolder(string fileName)
        {
            var path = FileNameAsPath(fileName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public static Task<bool> RemoveFolder(string path)
        {
            return Task<bool>.Factory.StartNew(() =>
            {
                path = FileNameAsPath(path);
                TryClearFolder(path);
                return TryRemoveFolder(path);
            });
        }

        public static Task<bool> ClearFolder(string path)
        {
            return Task<bool>.Factory.StartNew(() => TryClearFolder(FileNameAsPath(path)));
        }

        private static bool TryClearFolder(string path)
        {
            foreach (var file in TryGetFiles(path, null))
            {
                if (!TryRemoveFile(file)) return false;
            }
            foreach (var child in TryGetFolders(path))
            {
                if (!TryRemoveFolder(child)) return false;
            }
            return true;
        }

        private static bool TryRemoveFolder(string dir)
        {
            if (!System.IO.File.Exists(dir)) return true;
            int counter = 0;
            while (Directory.Exists(dir))
            {
                try
                {
                    FolderFullAccess(dir);
                    Directory.Delete(dir);
                }
                catch
                {
                    counter++;
                    Thread.Sleep(500);
                }
                if (counter > 10) break;
            }
            return !Directory.Exists(dir);
        }

        private static bool TryRemoveFile(string fileName)
        {
            if (!System.IO.File.Exists(fileName)) return true;
            int counter = 0;
            while (System.IO.File.Exists(fileName))
            {
                try
                {
                    System.IO.File.SetAttributes(fileName, FileAttributes.Normal);
                    System.IO.File.Delete(fileName);
                }
                catch
                {
                    counter++;
                    Thread.Sleep(500);
                }
                if (counter > 10) break;
            }
            return !System.IO.File.Exists(fileName);
        }

        public static string FindFile(string fileName, string mask)
        {
            return TryGetFiles(FileNameAsPath(fileName), null).FirstOrDefault(file => mask == FileNameAsFileExt(file));
        }

        public static string FindFolder(string fileName, string mask)
        {
            return TryGetFolders(FileNameAsPath(fileName)).FirstOrDefault(folder => mask == FileNameAsFolder(folder));
        }

        public static IEnumerable<string> AllFiles(string fileName)
        {
            return TryGetFiles(FileNameAsPath(fileName), null);
        }

        public static IEnumerable<string> AllFiles(string fileName, string mask)
        {
            return TryGetFiles(FileNameAsPath(fileName), mask);
        }

        private static IEnumerable<string> TryGetFiles(string root, string mask)
        {
            foreach (var file in Directory.GetFiles(root, mask))
            {
                yield return file;
            }
            foreach (var path in TryGetFolders(root))
            {
                foreach (var file in Directory.GetFiles(path, mask)) yield return file;
            }
        }

        public static IEnumerable<string> AllFolders(string fileName)
        {
            return TryGetFolders(FileNameAsPath(fileName));
        }

        private static IEnumerable<string> TryGetFolders(string root)
        {
            foreach (var path in Directory.GetDirectories(root))
            {
                yield return path;
                foreach (var child in TryGetFolders(path)) yield return child;
            }
        }

        public static bool FolderFullAccess(string fileName)
        {
            try
            {
                var path = FileNameAsPath(fileName);
                if (!Directory.Exists(path)) return false;
                var di = new DirectoryInfo(path);
                var ds = di.GetAccessControl();
                ds.AddAccessRule(new FileSystemAccessRule($@"{Environment.UserDomainName}\{Environment.UserName}", FileSystemRights.FullControl, AccessControlType.Allow));
                di.SetAccessControl(ds);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region Validate

        public static bool IsValidFileName(string fileName)
        {
            return !string.IsNullOrWhiteSpace(fileName) && fileName.All(c => !InvalidFileNameChars.Contains(c));
        }

        public static bool IsValidPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && path.All(c => !InvalidPathChars.Contains(c));
        }

        private static readonly HashSet<char> InvalidFileNameChars = new HashSet<char> {
            '"',
            '<',
            '>',
            '|',
            char.MinValue,
            '\x0001',
            '\x0002',
            '\x0003',
            '\x0004',
            '\x0005',
            '\x0006',
            '\a',
            '\b',
            '\t',
            '\n',
            '\v',
            '\f',
            '\r',
            '\x000E',
            '\x000F',
            '\x0010',
            '\x0011',
            '\x0012',
            '\x0013',
            '\x0014',
            '\x0015',
            '\x0016',
            '\x0017',
            '\x0018',
            '\x0019',
            '\x001A',
            '\x001B',
            '\x001C',
            '\x001D',
            '\x001E',
            '\x001F',
            ':',
            '*',
            '?',
            '\\',
            '/'
        };
        private static readonly HashSet<char> InvalidPathChars = new HashSet<char> {
            '"',
            '<',
            '>',
            '|',
            char.MinValue,
            '\x0001',
            '\x0002',
            '\x0003',
            '\x0004',
            '\x0005',
            '\x0006',
            '\a',
            '\b',
            '\t',
            '\n',
            '\v',
            '\f',
            '\r',
            '\x000E',
            '\x000F',
            '\x0010',
            '\x0011',
            '\x0012',
            '\x0013',
            '\x0014',
            '\x0015',
            '\x0016',
            '\x0017',
            '\x0018',
            '\x0019',
            '\x001A',
            '\x001B',
            '\x001C',
            '\x001D',
            '\x001E',
            '\x001F'
        };

        #endregion
    }
}