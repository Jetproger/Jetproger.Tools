using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Plugin.Commands;
using Jetproger.Tools.Plugin.Services;
using Newtonsoft.Json;
using Tools;

namespace Jetproger.Tools.Plugin.Bases
{
    internal static class Methods
    {
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer { Formatting = Formatting.None, ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.Objects };
        private static readonly CultureInfo FormatProvider = new CultureInfo("en-us") { NumberFormat = { NumberGroupSeparator = string.Empty, NumberDecimalSeparator = "." }, DateTimeFormat = { DateSeparator = "-", TimeSeparator = ":" } };

        private static Dictionary<string, string> ConfigKeys => GetOne(ConfigKeysHolder, GetConfigurationKeys);
        private static readonly Dictionary<string, string>[] ConfigKeysHolder = { null };

        public static string ConfigAsString(string key, string defaultValue)
        {
            try
            {
                return (ConfigKeys.ContainsKey(key) ? ConfigKeys[key] : defaultValue) ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int ConfigAsInt(string key, int defaultValue)
        {
            try
            {
                if (ConfigKeys.ContainsKey(key)) return defaultValue;
                var stringValue = ConfigKeys[key];
                int intValue;
                return int.TryParse(stringValue, NumberStyles.Any, FormatProvider, out intValue) ? intValue : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private static Dictionary<string, string> GetConfigurationKeys()
        {
            var keys = new Dictionary<string, string>();
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (!keys.ContainsKey(key)) keys.Add(key, ConfigurationManager.AppSettings[key]);
            }
            return keys;
        }

        public static T Lock<T>(T[] holder)
        {
            lock (holder)
            {
                return holder[0];
            }
        }

        public static void Lock<T>(T[] holder, T value)
        {
            lock (holder)
            {
                holder[0] = value;
            }
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

        public static T GetOne<T>(T?[] holder, Func<T> factory) where T : struct
        {
            if (holder[0] == null)
            {
                lock (holder)
                {
                    if (holder[0] == null) holder[0] = factory();
                }
            }
            return holder[0].Value;
        }

        public static T ReturnInvalidResult<T>(Exception e)
        {
            if (e is CommunicationObjectAbortedException) return default(T);
            if (e is CommunicationException) return default(T);
            System.Diagnostics.Trace.WriteLine(e.AsString());
            return (typeof(T)).IsTypeOf(typeof(Exception)) ? (T)(object)e : default(T);
        }

        public static void GarbageCollect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect(2);
            Thread.Sleep(111);
        }

        public static ICommandService CreateServiceClientLocal()
        {
            return new CommandService();
        }

        public static ICommandService CreateServiceClientRemote()
        {
            var builder = new ServerBuilder();
            var factory = new ChannelFactory<ICommandService>(builder.GetClientBinding(), builder.GetUrl());
            builder.ConfigureChannelFactory(factory);
            return factory.CreateChannel();
        }

        public static ICommandService CreateServiceClientRemote(string host, int port)
        {
            var builder = new ServerBuilder(host, port);
            var factory = new ChannelFactory<ICommandService>(builder.GetClientBinding(), builder.GetUrl());
            builder.ConfigureChannelFactory(factory);
            return factory.CreateChannel();
        }

        public static void Start<T>() where T : StarterService, new()
        {
            Start(new T());
        }

        public static void Start(StarterService service)
        {
            if (Environment.UserInteractive)
            {
                Console.CancelKeyPress += (o, e) => service.Stop();
                service.Start();
                Console.WriteLine(@"Running service, press a key to stop");
                Console.ReadKey();
                service.Stop();
                Console.WriteLine(@"Service stopped. Goodbye.");
                return;
            }
            ServiceBase.Run(new ServiceBase[] { service });
        }

        public static string SerializeJson(object o)
        {
            if (o == null)
            {
                return null;
            }
            using (var sw = new UTF8StringWriter())
            {
                if (o.GetType().IsSimple()) sw.Write(o.AsString()); else JsonSerializer.Serialize(sw, o);
                return sw.ToString();
            }
        }

        public static T DeserializeJson<T>(string json)
        {
            return (T)DeserializeJson(json, typeof(T));
        }

        public static object DeserializeJson(string json, Type resultType)
        {
            if (resultType.IsSimple())
            {
                return json.As(resultType);
            }
            using (var sr = new StringReader(json))
            {
                return JsonSerializer.Deserialize(sr, resultType);
            }
        }

        private class UTF8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}