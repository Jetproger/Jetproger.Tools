using System;
using System.Globalization;
using System.Threading;
using Jetproger.Tools.Plugin.Bases;
using Jetproger.Tools.Resource.Bases;

namespace Tools
{
    public static class Plugin
    {
        public static bool IsStopped { get; internal set; }
        public static string Error { get; internal set; }
        public static bool IsHost => AppDomain.CurrentDomain.IsDefaultAppDomain();
        public static string Name => $"{AppDomain.CurrentDomain.FriendlyName}, {Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture)}";

        public static void StartApp(StartAppConfig config)
        {
            Trace.Run();
            config = config ?? new StartAppConfig();
            Resource.ReadCommandLineArguments(config.Arguments ?? new string[0]);
            Resource.ReadAppConfiguration();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Toolx.Conf.Culture());
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            if (config.UnhandledExceptionHandler != null)
            {
                AppDomain.CurrentDomain.UnhandledException -= config.UnhandledExceptionHandler;
                AppDomain.CurrentDomain.UnhandledException += config.UnhandledExceptionHandler;
            }
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            IsStopped = true;
            if (e?.ExceptionObject == null) return;
            Error = e.ExceptionObject.ToString();
            System.Diagnostics.Trace.WriteLine(Error);
        }
    }
}