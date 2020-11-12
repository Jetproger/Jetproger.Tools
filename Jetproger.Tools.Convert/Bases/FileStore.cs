using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Web;

namespace Jetproger.Tools.Convert.Bases
{
    public class FileStore
    {
        public readonly string FileName;
        public readonly string FileExt;
        public readonly string Folder;
        public readonly string File;
        public readonly string Path;
        public readonly string Ext;

        public FileStore(string fileName)
        {
            FileName = ResolveFileName(fileName);
            FileExt = System.IO.Path.GetFileName(FileName);
            File = System.IO.Path.GetFileNameWithoutExtension(FileName);

            Folder = new DirectoryInfo(FileName).Name;
            Path = System.IO.Path.GetDirectoryName(FileName);

            var ext = System.IO.Path.GetExtension(FileName) ?? string.Empty;
            if (ext.StartsWith(".")) ext = ext.Substring(1);
            Ext = ext;
        }

        private string ResolveFileName(string fileName)
        {
            fileName = fileName ?? string.Empty;
            var args = fileName.Split(new[] { System.IO.Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0) return System.IO.Path.Combine(AppDir(), TempFile());
            if (args.Length == 1 && !fileName.EndsWith("\\")) return System.IO.Path.Combine(AppDir(), IsValidFileName(args[0]) ? args[0] : TempFile());
            if (fileName.EndsWith("\\"))
            {
                var newArgs = new string[args.Length + 1];
                for (var i = 0; i < args.Length; i++) newArgs[i] = args[i];
                newArgs[newArgs.Length - 1] = TempFile();
                args = newArgs;
            }
            var file = IsValidFileName(args[args.Length - 1]) ? args[args.Length - 1] : TempFile();
            var root = System.IO.Path.IsPathRooted(args[0]) ? args[0] : AppDir();
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
                full = System.IO.Path.Combine(full, arg);
            }
            return System.IO.Path.Combine(full, file);
        }

        public bool ExistsFile()
        {
            return System.IO.File.Exists(FileName);
        }

        public bool ExistsFolder()
        {
            return Directory.Exists(Path);
        }

        public void CreateFolder()
        {
            if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
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

        public bool ClearFolder()
        {
            foreach (var file in GetFiles(Path, null))
            {
                if (!(new FileMaster(file)).RemoveFile()) return false;
            }
            foreach (var folder in GetFolders(Path))
            {
                if (!(new FileMaster(folder)).RemoveFolder()) return false;
            }
            return true;
        }

        public bool RemoveFolder()
        {
            if (!Directory.Exists(Path)) return true;
            int counter = 0;
            while (Directory.Exists(Path))
            {
                try
                {
                    FolderFullAccess();
                    Directory.Delete(Path);
                }
                catch
                {
                    counter++;
                    Thread.Sleep(500);
                }
                if (counter > 10) break;
            }
            return !Directory.Exists(Path);
        }

        public bool RemoveFile()
        {
            if (!System.IO.File.Exists(FileName)) return true;
            int counter = 0;
            while (System.IO.File.Exists(FileName))
            {
                try
                {
                    System.IO.File.SetAttributes(FileName, FileAttributes.Normal);
                    System.IO.File.Delete(FileName);
                }
                catch
                {
                    counter++;
                    Thread.Sleep(500);
                }
                if (counter > 10) break;
            }
            return !System.IO.File.Exists(FileName);
        }

        public string FindFile(string mask)
        {
            return GetFiles(Path, null).FirstOrDefault(x => mask == (new FileMaster(x)).FileExt);
        }

        public string FindFolder(string fileName, string mask)
        {
            return GetFolders(Path).FirstOrDefault(x => mask == (new FileMaster(x)).Folder);
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
            foreach (var file in Directory.GetFiles(root, mask))
            {
                yield return file;
            }
            foreach (var path in GetFolders(root))
            {
                foreach (var file in Directory.GetFiles(path, mask)) yield return file;
            }
        }

        public IEnumerable<string> AllFolders()
        {
            return GetFolders(Path);
        }

        private static IEnumerable<string> GetFolders(string root)
        {
            foreach (var path in Directory.GetDirectories(root))
            {
                yield return path;
                foreach (var child in GetFolders(path)) yield return child;
            }
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

        public static bool IsValidFileName(string fileName)
        {
            return !string.IsNullOrWhiteSpace(fileName) && fileName.All(c => !InvalidFileNameChars.Contains(c));
        }

        public static bool IsValidPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && path.All(c => !InvalidPathChars.Contains(c));
        }

        private static readonly HashSet<char> InvalidFileNameChars = new HashSet<char> { '"', '<', '>', '|', char.MinValue, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x000E', '\x000F', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001A', '\x001B', '\x001C', '\x001D', '\x001E', '\x001F', ':', '*', '?', '\\', '/' };

        private static readonly HashSet<char> InvalidPathChars = new HashSet<char> { '"', '<', '>', '|', char.MinValue, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x000E', '\x000F', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001A', '\x001B', '\x001C', '\x001D', '\x001E', '\x001F' };
    }
}