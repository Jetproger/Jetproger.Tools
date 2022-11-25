using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;
using Jetproger.Tools.Process.Commands;
using Jetproger.Tools.Process.Services;
using Newtonsoft.Json;

namespace Jetproger.Tools.Process.Bases
{
    internal static class Methods
    {
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer { Formatting = Formatting.None, ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.Objects };
        private static readonly CultureInfo FormatProvider = new CultureInfo("en-us") { NumberFormat = { NumberGroupSeparator = string.Empty, NumberDecimalSeparator = "." }, DateTimeFormat = { DateSeparator = "-", TimeSeparator = ":" } };

        private static Dictionary<string, string> ConfigKeys => f.one.Get(ConfigKeysHolder, GetConfigurationKeys);
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

        public static T ReturnInvalidResult<T>(Exception e)
        {
            if (e is CommunicationObjectAbortedException) return default(T);
            if (e is CommunicationException) return default(T);
            System.Diagnostics.Trace.WriteLine(e.As<string>());
            return f.sys.IsTypeOf(typeof(T), typeof(Exception)) ? (T)(object)e : default(T);
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
    }
}