using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net; 
using System.Threading; 

namespace Jetproger.Service.Bases
{
    internal class AppUpdater
    {
        private static readonly CultureInfo Culture = new CultureInfo("en-us") { NumberFormat = { NumberGroupSeparator = string.Empty, NumberDecimalSeparator = "." }, DateTimeFormat = { DateSeparator = "-", TimeSeparator = ":" } };
        private static readonly WebClient Client;
        private static Thread _thread;

        static AppUpdater()
        {
            Client = new WebClient();
            Client.Headers[HttpRequestHeader.ContentType] = "text/plain";
            Client.UploadStringCompleted += UploadStringCompleted;
        }

        private static void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var state = e.UserState as _State;
            if (state == null) return;
            state.Response = e;
            state.Synchronizer.Set();
        }

        public static void StartUpdater()
        {
            if (_thread != null) return;
            _thread = new Thread(Updating);
            _thread.Start();
        }

        private static void Updating()
        {
            for (long i = long.MinValue; i < long.MaxValue; i++)
            {
                Thread.Sleep(3600000);
                try
                {
                    Update();
                }
                catch (Exception e)
                {
                    f.log.Error(e);
                }
            }
        }

        private static void Update()
        {
            var request = new _Request { Session = Guid.NewGuid(), Command = "Jetproger.Tools.Update.Bases.UpdateFilesCommand", P0 = f.exe.name };
            var json = f.jsonof(request);
            var state = new _State();
            Client.BaseAddress = f.configof("UpdHost", "127.0.0.1:9001");
            Client.UploadStringAsync(new Uri("/jetproger/v1/cmd", UriKind.Relative), "POST", json, state);
            state.Synchronizer.WaitOne();
            state.Synchronizer.Reset();
            if (state.Response == null) return;
            if (state.Response.Error != null)
            {
                f.log.Error(state.Response.Error);
                return;
            }
            var response = f.jsonto<_Response>(state.Response.Result);
            if (response == null) return;
            var remoteFiles = f.xmlto<_Files>(response.Result);
            var updateFiles = GetUpdateFiles(remoteFiles);
            if (updateFiles.Files.Length == 0) return;
            for (int i = 0; i < updateFiles.Files.Length; i++)
            {
                var name = updateFiles.Files[i].FileName;
                while (DownloadFile(name))
                {
                }
            }
            CopyFiles(updateFiles.Files);
        }

        private static void CopyFiles(_File[] files)
        {
            try
            {
                AppMethods.Instance.StopCommands();
                AppMethods.Instance.Reset();
                var sourcePath = Path.Combine(f.exe.path, ".update");
                var targetPath = f.exe.path;
                foreach (var file in files)
                {
                    var fileName = $"{file.FileName}.dll";
                    var sourceFile = Path.Combine(sourcePath, fileName);
                    var targetFile = Path.Combine(targetPath, fileName);
                    File.Delete(targetFile);
                    File.Copy(sourceFile, targetFile, true);
                    File.Delete(sourceFile);
                }
            }
            catch (Exception e)
            {
                f.log.Error(e);
            }
            finally
            {
                AppMethods.Instance.StartCommands();
            }
        }

        private static bool DownloadFile(string name)
        {
            var path = Path.Combine(f.exe.path, ".update");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var sourceFile = Path.Combine(path, $"{name}.download");
            var targetFile = Path.Combine(path, $"{name}.dll");
            var content = GetContentLocalFile(name);
            var request = new _Request { Session = Guid.NewGuid(), Command = "Jetproger.Tools.Update.Bases.DownloadFileCommand", Value = name, P0 = f.exe.name, P1 = content.Length.ToString() };
            var json = f.jsonof(request);
            var state = new _State();
            Client.BaseAddress = f.configof("UpdHost", "127.0.0.1:9001");
            Client.UploadStringAsync(new Uri("/jetproger/v1/cmd", UriKind.Relative), "POST", json, state);
            state.Synchronizer.WaitOne();
            state.Synchronizer.Reset();
            if (state.Response == null) return false;
            if (state.Response.Error != null)
            {
                f.log.Error(state.Response.Error);
                return false;
            }
            var response = f.jsonto<_Response>(state.Response.Result);
            if (response == null) return false;
            if (string.IsNullOrWhiteSpace(response.Result))
            {
                File.Delete(targetFile);
                File.Copy(sourceFile, targetFile, true);
                return false;
            }
            var bytes = Convert.FromBase64String(response.Result);
            var bytes2 = new byte[content.Length + bytes.Length];
            Buffer.BlockCopy(content, 0, bytes2, 0, content.Length);
            Buffer.BlockCopy(bytes, 0, bytes2, content.Length, bytes.Length);
            File.WriteAllBytes(sourceFile, bytes2);
            return true;
        }

        private static byte[] GetContentLocalFile(string name)
        {
            var fileName = Path.Combine(f.exe.path, $"{name}.download");
            return File.Exists(fileName) ? File.ReadAllBytes(fileName) : new byte[0];
        }

        private static _Files GetUpdateFiles(_Files remoteFiles)
        {
            var files = new List<_File>();
            var localFiles = GetLocalFiles();
            foreach (var remoteFile in remoteFiles.Files)
            {
                var exists = false;
                var remoteKey = remoteFile.FileName.ToLower();
                foreach (var localFile in localFiles.Files)
                {
                    if (localFile.FileName.ToLower() != remoteKey) continue;
                    if (IsMore(remoteFile.Version, localFile.Version)) files.Add(remoteFile);
                    exists = true;
                    break;
                }
                if (!exists) files.Add(remoteFile);
            }
            return new _Files { Files = files.ToArray() };
        }

        private static _Files GetLocalFiles()
        {
            var files = new List<_File>();
            foreach (var fileName in Directory.EnumerateFiles(f.exe.path, "*.dll"))
            {
                var version = $"{typeof(f).Assembly.GetName().Version}";
                files.Add(new _File { FileName = Path.GetFileNameWithoutExtension(fileName), Version = version });
            }
            return new _Files { Files = files.ToArray() };
        }

        private static bool IsMore(string version1, string version2)
        {
            var a = GetVersionValues(version1);
            var b = GetVersionValues(version2);
            if (a[0] > b[0]) return true;
            if (a[1] > b[1]) return true;
            if (a[2] > b[2]) return true;
            return (a[3] > b[3]);
        }

        private static int[] GetVersionValues(string version)
        {
            var values = new int[4];
            var strings = version.Split('.');
            values[0] = strings.Length > 0 ? (int.TryParse(strings[0], NumberStyles.Any, Culture, out var x0) ? x0 : default(int)) : 0;
            values[1] = strings.Length > 1 ? (int.TryParse(strings[1], NumberStyles.Any, Culture, out var x1) ? x1 : default(int)) : 0;
            values[2] = strings.Length > 2 ? (int.TryParse(strings[2], NumberStyles.Any, Culture, out var x2) ? x2 : default(int)) : 0;
            values[3] = strings.Length > 3 ? (int.TryParse(strings[3], NumberStyles.Any, Culture, out var x3) ? x3 : default(int)) : 0;
            return values;
        }

        private class _State
        {
            public ManualResetEvent Synchronizer = new ManualResetEvent(false);
            public UploadStringCompletedEventArgs Response;
        }

        private class _Request
        {
            public Guid Session { get; set; }
            public string Command { get; set; }
            public string Value { get; set; }
            public string P0 { get; set; }
            public string P1 { get; set; }
        }

        private class _Response
        {
            public Guid Session { get; set; }
            public string Report { get; set; }
            public string Result { get; set; }
            public string P0 { get; set; }
        }

        private class _Files
        {
            public _File[] Files { get; set; }
        }

        private class _File
        {
            public string FileName { get; set; }
            public string Version { get; set; }
        }
    }
}