using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Jetproger.Tools.Convert.Converts
{
    public static class FileExtensions
    {
        private static readonly HashSet<char> InvalidFileNameChars = new HashSet<char> { '"', '<', '>', '|', char.MinValue, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x000E', '\x000F', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001A', '\x001B', '\x001C', '\x001D', '\x001E', '\x001F', ':', '*', '?', '\\', '/' };
        private static readonly HashSet<char> InvalidPathChars = new HashSet<char> { '"', '<', '>', '|', char.MinValue, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x000E', '\x000F', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001A', '\x001B', '\x001C', '\x001D', '\x001E', '\x001F' };

        public static List<T> Load<T>(Jc.QueryScope query)
        {
            return null;
        }

        public static void Save<T>(Jc.QueryScope query)
        {
            //Deleting
        }

        public static void Save<T>(Jc.QueryData queryData)
        {
            //Save
        }

        public static string FileOf(this Jc.IFileExpander exp, string fileName)
        {
            return Path.GetFileName(FullOf(exp, fileName));
        }

        public static string NameOf(this Jc.IFileExpander exp, string fileName)
        {
            return Path.GetFileNameWithoutExtension(FullOf(exp, fileName));
        }

        public static string ExtOf(this Jc.IFileExpander exp, string fileName)
        {
            var ext = Path.GetExtension(FullOf(exp, fileName)) ?? string.Empty;
            if (ext.StartsWith(".")) ext = ext.Substring(1);
            return ext;
        }

        public static string FolderOf(this Jc.IFileExpander exp, string fileName)
        {
            return new DirectoryInfo(PathOf(exp, fileName)).Name;
        }

        public static string PathOf(this Jc.IFileExpander exp, string fileName)
        {
            return Path.GetDirectoryName(FullOf(exp, fileName));
        }

        public static string FullOf(this Jc.IFileExpander exp, string fileName)
        {
            fileName = fileName ?? string.Empty;
            var args = fileName.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0) return Path.Combine(AppDir(exp), TmpFile(exp));
            if (args.Length == 1 && !fileName.EndsWith("\\")) return Path.Combine(AppDir(exp), IsValidFileName(exp, args[0]) ? args[0] : TmpFile(exp));
            if (fileName.EndsWith("\\"))
            {
                var newArgs = new string[args.Length + 1];
                for (var i = 0; i < args.Length; i++) newArgs[i] = args[i];
                newArgs[newArgs.Length - 1] = TmpFile(exp);
                args = newArgs;
            }
            var file = IsValidFileName(exp, args[args.Length - 1]) ? args[args.Length - 1] : TmpFile(exp);
            var root = Path.IsPathRooted(args[0]) ? args[0] : AppDir(exp);
            var full = root + "\\";
            if (fileName.StartsWith("\\\\")) full = "\\\\" + full;
            foreach (var arg in args)
            {
                if (arg == root || arg == file)
                {
                    continue;
                }
                if (!IsValidPath(exp, arg))
                {
                    full = root;
                    break;
                }
                full = Path.Combine(full, arg);
            }
            return Path.Combine(full, file);
        }

        public static string AppDir(this Jc.IFileExpander exp)
        {
            return (HttpContext.Current == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("~")) ?? string.Empty;
        }

        public static string AppFile(this Jc.IFileExpander exp)
        {
            return (HttpContext.Current == null ? Process.GetCurrentProcess().MainModule.FileName : null);
        }

        public static string TmpFile(this Jc.IFileExpander exp)
        {
            return $"{Guid.NewGuid()}.tmp";
        }

        public static bool ExistsFile(this Jc.IFileExpander exp, string fileName)
        {
            return System.IO.File.Exists(FullOf(exp, fileName));
        }

        public static bool ExistsFolder(this Jc.IFileExpander exp, string fileName)
        {
            return Directory.Exists(PathOf(exp, fileName));
        }

        public static bool IsValidFileName(this Jc.IFileExpander exp, string fileName)
        {
            return !string.IsNullOrWhiteSpace(fileName) && fileName.All(c => !InvalidFileNameChars.Contains(c));
        }

        public static bool IsValidPath(this Jc.IFileExpander exp, string path)
        {
            return !string.IsNullOrWhiteSpace(path) && path.All(c => !InvalidPathChars.Contains(c));
        }
    }
}