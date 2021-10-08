using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace Jetproger.Tools.File.Jsons
{
    public class JsonManager
    {
        private static readonly ConcurrentDictionary<string, JsonFile>[] JsonFilesHolder;

        static JsonManager()
        {
            JsonFilesHolder = new ConcurrentDictionary<string, JsonFile>[] { null };
            Write();
        }

        public static JsonFile GetFile(Type itemType)
        {
            var id = itemType.AssemblyQualifiedName ?? string.Empty;
            var jsonFile = JsonFiles.GetOrAdd(id, x => new JsonFile(itemType));
            return jsonFile;
        }

        private static ConcurrentDictionary<string, JsonFile> JsonFiles
        {
            get
            {
                if (JsonFilesHolder[0] == null)
                {
                    lock (JsonFilesHolder)
                    {
                        if (JsonFilesHolder[0] == null)
                        {
                            JsonFilesHolder[0] = new ConcurrentDictionary<string, JsonFile>();
                        }
                    }
                }
                return JsonFilesHolder[0];
            }
        }

        private static void Write()
        {
            try
            {
                var proc = new Action(BeginWrite);
                proc.BeginInvoke(EndWrite, proc);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        private static void EndWrite(IAsyncResult asyncResult)
        {
            try
            {
                ((Action)asyncResult.AsyncState).EndInvoke(asyncResult);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        private static void BeginWrite()
        {
            while (true)
            {
                try
                {
                    if (JsonFiles.Count == int.MaxValue)
                    {
                        break;
                    }
                    foreach (var jsonFile in JsonFiles.Values)
                    {
                        if (jsonFile.IsModified)
                        {
                            lock (jsonFile)
                            {
                                if (jsonFile.IsModified)
                                {
                                    jsonFile.Write();
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
                Thread.Sleep(3333);
            }
        }
    }
}