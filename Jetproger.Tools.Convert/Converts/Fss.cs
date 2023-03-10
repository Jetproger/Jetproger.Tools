using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Web;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Converts
{ 
    public static class FssExtensions
    {
        private static readonly HashSet<char> InvalidFileNameChars = new HashSet<char> { '"', '<', '>', '|', char.MinValue, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x000E', '\x000F', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001A', '\x001B', '\x001C', '\x001D', '\x001E', '\x001F', ':', '*', '?', '\\', '/' };
        private static readonly HashSet<char> InvalidPathChars = new HashSet<char> { '"', '<', '>', '|', char.MinValue, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x000E', '\x000F', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001A', '\x001B', '\x001C', '\x001D', '\x001E', '\x001F' };
        private static readonly Dictionary<string, _File> Files = new Dictionary<string, _File>();

        public static string appdir(this f.IFssExpander e) { return (HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("~")); }
        public static bool IsValidFileName(this f.IFssExpander e, string file) { return !string.IsNullOrWhiteSpace(file) && file.All(c => !InvalidFileNameChars.Contains(c)); }
        public static bool IsValidPath(this f.IFssExpander e, string path) { return !string.IsNullOrWhiteSpace(path) && path.All(c => !InvalidPathChars.Contains(c)); }
        public static string appfile(this f.IFssExpander e) { return (HttpContext.Current == null ? Process.GetCurrentProcess().MainModule.FileName : null); }
        public static string tmpfile(this f.IFssExpander e) { return string.Format("{0}.tmp", Guid.NewGuid()); }
        public static string pathnameextof(this f.IFssExpander e, string file) { return _F(file).PathNameExt; }
        public static string nameextof(this f.IFssExpander e, string file) { return _F(file).NameExt; }
        public static string folderof(this f.IFssExpander e, string file) { return _F(file).Folder; }
        public static string diskof(this f.IFssExpander e, string file) { return _F(file).Disk; }
        public static string nameof(this f.IFssExpander e, string file) { return _F(file).Name; }
        public static string pathof(this f.IFssExpander e, string file) { return _F(file).Path; }
        public static string extensionof(this f.IFssExpander e, string file) { return _F(file).Ext; }

        private static _File _F(string file)
        {
            file = (file ?? string.Empty).ToLower().Trim(' ', '\t', '\r', '\n');
            if (!Files.ContainsKey(file))
            {
                lock (Files)
                {
                    if (!Files.ContainsKey(file)) Files.Add(file, new _File(file));
                }
            }
            return Files[file];
        }

        public static string FindFile(this f.IFssExpander e, string dir, string nameExt)
        {
            return f.fss.FindFiles(dir).FirstOrDefault(x => nameExt == _F(x).NameExt);
        }

        public static IEnumerable<string> FindFiles(this f.IFssExpander e, string dir, string mask = null)
        {
            dir = f.fss.pathof(dir);
            mask = mask ?? string.Empty;
            foreach (var file in Directory.EnumerateFiles(dir, mask))
            {
                yield return file;
            }
            foreach (var path in f.fss.AllFolders(dir))
            {
                foreach (var file in Directory.EnumerateFiles(path, mask)) yield return file;
            }
        }

        public static IEnumerable<string> AllFolders(this f.IFssExpander e, string dir)
        {
            foreach (var path in Directory.EnumerateDirectories(dir))
            {
                yield return path;
                foreach (var innerPath in f.fss.AllFolders(path)) yield return innerPath;
            }
        }

        public static bool ClearFolder(this f.IFssExpander e, string path)
        {
            path = f.fss.pathof(path);
            f.fss.FolderFullAccess(path);
            foreach (var file in f.fss.FindFiles(path))
            {
                if (!f.fss.RemoveFile(file)) return false;
            }
            foreach (var folder in f.fss.AllFolders(path))
            {
                if (!f.fss.RemoveFolder(folder)) return false;
            }
            return true;
        }

        public static bool RemoveFile(this f.IFssExpander e, string fileName)
        {
            fileName = f.fss.pathnameextof(fileName);
            if (!File.Exists(fileName)) return true;
            int counter = 0;
            while (File.Exists(fileName))
            {
                try
                {
                    File.SetAttributes(fileName, FileAttributes.Normal);
                    File.Delete(fileName);
                }
                catch
                {
                    counter++;
                    Thread.Sleep(333);
                }
                if (counter > 10) break;
            }
            return !File.Exists(fileName);
        }

        public static bool RemoveFolder(this f.IFssExpander e, string path)
        {
            path = f.fss.pathof(path);
            if (!Directory.Exists(path)) return true;
            int counter = 0;
            while (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path);
                }
                catch
                {
                    counter++;
                    Thread.Sleep(333);
                }
                if (counter > 10) break;
            }
            return !Directory.Exists(path);
        }

        public static bool FolderFullAccess(this f.IFssExpander e, string path)
        {
            try
            {
                path = f.fss.pathof(path);
                if (!Directory.Exists(path)) return false;
                var di = new DirectoryInfo(path);
                var ds = di.GetAccessControl();
                ds.AddAccessRule(new FileSystemAccessRule(string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName), FileSystemRights.FullControl, AccessControlType.Allow));
                di.SetAccessControl(ds);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private class _File
        {
            public readonly string PathNameExt;
            public readonly string NameExt;
            public readonly string Folder;
            public readonly string Disk;
            public readonly string Name;
            public readonly string Path;
            public readonly string Ext;

            public _File(string fileName)
            {
                PathNameExt = ResolveFileName(fileName) ?? string.Empty;
                NameExt = System.IO.Path.GetFileName(PathNameExt);
                Name = System.IO.Path.GetFileNameWithoutExtension(PathNameExt);
                Folder = new DirectoryInfo(PathNameExt).Name;
                Path = System.IO.Path.GetDirectoryName(PathNameExt);
                var ext = System.IO.Path.GetExtension(PathNameExt) ?? string.Empty;
                if (ext.StartsWith(".")) ext = ext.Substring(1);
                Ext = ext;
                Disk = GetDisk(PathNameExt);
            }

            private string GetDisk(string pathNameExt)
            {
                var disk = System.IO.Path.GetPathRoot(pathNameExt);
                if (string.IsNullOrWhiteSpace(disk)) return disk;
                if (!disk.StartsWith(@"\\")) return disk;
                while (true)
                {
                    var i = disk.LastIndexOf(@"\");
                    if (i <= 1) break;
                    disk = disk.Substring(0, i);
                }
                return !disk.EndsWith(@"\") ? disk + @"\" : disk;
            }

            private string ResolveFileName(string fileName)
            {
                fileName = fileName ?? string.Empty;
                var args = fileName.Split(new[] { System.IO.Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                if (args.Length == 0) return System.IO.Path.Combine(f.fss.appdir(), f.fss.tmpfile());
                if (args.Length == 1 && !fileName.EndsWith("\\")) return System.IO.Path.Combine(f.fss.appdir(), f.fss.IsValidFileName(args[0]) ? args[0] : f.fss.tmpfile());
                if (fileName.EndsWith("\\"))
                {
                    var newArgs = new string[args.Length + 1];
                    for (var i = 0; i < args.Length; i++) newArgs[i] = args[i];
                    newArgs[newArgs.Length - 1] = f.fss.tmpfile();
                    args = newArgs;
                }
                var file = f.fss.IsValidFileName(args[args.Length - 1]) ? args[args.Length - 1] : f.fss.tmpfile();
                var root = System.IO.Path.IsPathRooted(args[0]) ? args[0] : f.fss.appdir();
                var full = root + "\\";
                if (fileName.StartsWith("\\\\")) full = "\\\\" + full;
                foreach (var arg in args)
                {
                    if (arg == root || arg == file)
                    {
                        continue;
                    }
                    if (!f.fss.IsValidPath(arg))
                    {
                        full = root;
                        break;
                    }
                    full = System.IO.Path.Combine(full, arg);
                }
                return System.IO.Path.Combine(full, file);
            }
        }
    }
}