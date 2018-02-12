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
    }
}