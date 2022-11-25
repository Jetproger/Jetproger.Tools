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
     
        public static string AppDir(this f.IFssExpander e)
        {
            return (HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("~"));
        }

        public static string AppFile(this f.IFssExpander e)
        {
            return (HttpContext.Current == null ? Process.GetCurrentProcess().MainModule.FileName : null);
        }

        public static string TmpFile(this f.IFssExpander e)
        {
            return $"{Guid.NewGuid()}.tmp";
        }

        public static FileWay UseFile(this f.IFssExpander e, string fileName)
        {
            return t<FileWay>.key(fileName.ToLower());
        }

        public static bool IsValidFileName(this f.IFssExpander e, string fileName)
        {
            return !string.IsNullOrWhiteSpace(fileName) && fileName.All(c => !InvalidFileNameChars.Contains(c));
        }

        public static bool IsValidPath(this f.IFssExpander e, string path)
        {
            return !string.IsNullOrWhiteSpace(path) && path.All(c => !InvalidPathChars.Contains(c));
        }

        public static bool RemoveFile(this f.IFssExpander e, string fileName)
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
                    Thread.Sleep(333);
                }
                if (counter > 10) break;
            }
            return !System.IO.File.Exists(fileName);
        }

        public static bool RemoveFolder(this f.IFssExpander e, string path)
        {
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
                    Thread.Sleep(500);
                }
                if (counter > 10) break;
            }
            return !Directory.Exists(path);
        }
    }

    [Serializable]
    public class FileWay
    {
        public readonly string PathNameExt;
        public readonly string NameExt;
        public readonly string Folder;
        public readonly string Disk;
        public readonly string Name;
        public readonly string Path;
        public readonly string Ext;

        public FileWay() { }

        public FileWay(string fileName)
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
            if (args.Length == 0) return System.IO.Path.Combine(f.fss.AppDir(), f.fss.TmpFile());
            if (args.Length == 1 && !fileName.EndsWith("\\")) return System.IO.Path.Combine(f.fss.AppDir(), f.fss.IsValidFileName(args[0]) ? args[0] : f.fss.TmpFile());
            if (fileName.EndsWith("\\"))
            {
                var newArgs = new string[args.Length + 1];
                for (var i = 0; i < args.Length; i++) newArgs[i] = args[i];
                newArgs[newArgs.Length - 1] = f.fss.TmpFile();
                args = newArgs;
            }
            var file = f.fss.IsValidFileName(args[args.Length - 1]) ? args[args.Length - 1] : f.fss.TmpFile();
            var root = System.IO.Path.IsPathRooted(args[0]) ? args[0] : f.fss.AppDir();
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

        public string FindFile(string mask)
        {
            return GetFiles(Path, null).FirstOrDefault(x => mask == f.fss.UseFile(x).NameExt);
        }

        public string FindFolder(string fileName, string mask)
        {
            return GetFolders(Path).FirstOrDefault(x => mask == f.fss.UseFile(x).Folder);
        }

        public IEnumerable<string> AllFiles()
        {
            return GetFiles(Path, null);
        }

        public IEnumerable<string> AllFiles(string mask)
        {
            return GetFiles(Path, mask);
        }

        private IEnumerable<string> GetFiles(string root, string mask)
        {
            foreach (var file in Directory.EnumerateFiles(root, mask))
            {
                yield return file;
            }
            foreach (var path in GetFolders(root))
            {
                foreach (var file in Directory.EnumerateFiles(path, mask)) yield return file;
            }
        }

        public IEnumerable<string> AllFolders()
        {
            return GetFolders(Path);
        }

        private static IEnumerable<string> GetFolders(string root)
        {
            foreach (var path in Directory.EnumerateDirectories(root))
            {
                yield return path;
                foreach (var child in GetFolders(path)) yield return child;
            }
        }

        public bool ClearFolder()
        {
            FolderFullAccess();
            foreach (var file in GetFiles(Path, null))
            {
                if (!f.fss.RemoveFile(file)) return false;
            }
            foreach (var folder in GetFolders(Path))
            {
                if (!f.fss.RemoveFolder(folder)) return false;
            }
            return true;
        }

        public bool FolderFullAccess()
        {
            try
            {
                if (!Directory.Exists(Path)) return false;
                var di = new DirectoryInfo(Path);
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
    }
}