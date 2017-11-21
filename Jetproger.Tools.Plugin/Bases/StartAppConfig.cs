using System;
using System.Threading;

namespace Jetproger.Tools.Plugin.Bases
{
    public class StartAppConfig
    {
        public string[] Arguments { get; set; }
        public string Culture { get; set; }
        public Action StartProcedure { get; set; }
        public ThreadExceptionEventHandler ThreadExceptionHandler { get; set; }
        public UnhandledExceptionEventHandler UnhandledExceptionHandler { get; set; }
    }
}