using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Jetproger.Tools.Plugin.Bases;
using Jetproger.Tools.Plugin.Commands;
using Jetproger.Tools.Resource.Bases;
using Jetproger.Tools.Trace.Bases;
using TNLog = Tools.Trace;
using TF = Tools.File;
using Res = Tools.Resource;
using DTrace = System.Diagnostics.Trace;

namespace Tools
{
    public static class Program
    {
        public static string Name => $"{AppDomain.CurrentDomain.FriendlyName}, {Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture)}";

        public static bool IsHost => AppDomain.CurrentDomain.IsDefaultAppDomain();

        public static bool IsStopped { get; private set; }

        public static string Error { get; private set; }

        public static IRunTrace RunTrace(TraceListener traceListener)
        {
            return new ProgramStarter(traceListener, x =>
            {
                DTrace.Listeners.Add((TraceListener)x);
                DTrace.WriteLine(new ProgramStarterMessage("Loading resources"));
                TNLog.Run();
                AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

                var lang = ConfigurationManager.AppSettings["Culture"];
                if (string.IsNullOrWhiteSpace(lang)) lang = "en-US";
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                DTrace.WriteLine(new ProgramStarterMessage(string.Format(Toolx.Note.StartAppLang(), lang)));
                DTrace.WriteLine(Toolx.Note.StartNLogBase());
                DTrace.WriteLine(new ProgramStarterMessage(string.Format(Toolx.Note.StartTrace(), traceListener.GetType().FullName)));
            });
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            IsStopped = true;
            if (e?.ExceptionObject == null) return;
            Error = e.ExceptionObject.ToString();
            DTrace.WriteLine(Error);
        }
    }
}

namespace Jetproger.Tools.Plugin.Bases
{
    public class ProgramStarter : MarshalByRefObject, IRunTrace, IRunNLog, IParseCommandLineArguments, IInitCommandPool, IRun
    {
        private readonly List<Tuple<object, Action<object>>> _actions = new List<Tuple<object, Action<object>>>();

        public ProgramStarter(object parameter, Action<object> action)
        {
            _actions.Add(new Tuple<object, Action<object>>(parameter, action));
        }

        public IRunTrace RunTrace(TraceListener traceListener)
        {
            _actions.Add(new Tuple<object, Action<object>>(traceListener, x => {
                DTrace.WriteLine(new ProgramStarterMessage(string.Format(Toolx.Note.StartTrace(), traceListener.GetType().FullName)));
                DTrace.Listeners.Add((TraceListener)x);
            }));
            return this;
        }

        public IRunNLog RunNLog<T>() where T : TypedMessage
        {
            _actions.Add(new Tuple<object, Action<object>>(null, x => {
                DTrace.WriteLine(new ProgramStarterMessage(string.Format(Toolx.Note.StartNLogTyped(), typeof(T).FullName)));
                TNLog.Run<T>();
            }));
            return this;
        }

        public IParseCommandLineArguments OnUnhandledException(UnhandledExceptionEventHandler handler)
        {
            _actions.Add(new Tuple<object, Action<object>>(handler, x => {
                var y = (UnhandledExceptionEventHandler)x;
                AppDomain.CurrentDomain.UnhandledException -= y;
                AppDomain.CurrentDomain.UnhandledException += y;
            }));
            return this;
        }

        public IInitCommandPool ParseCommandLineArguments(string[] args)
        {
            _actions.Add(new Tuple<object, Action<object>>(args, x => {
                DTrace.WriteLine(new ProgramStarterMessage(Toolx.Note.StartReadConf()));
                Res.ReadAppConfiguration();

                DTrace.WriteLine(new ProgramStarterMessage(Toolx.Note.StartCmdArgs()));
                Res.ReadCommandLineArguments((string[])x);
            }));
            return this;
        }

        public IRun InitCommandPool(int size)
        {
            _actions.Add(new Tuple<object, Action<object>>(null, x => {
                DTrace.WriteLine(new ProgramStarterMessage(Toolx.Note.StartDI()));
                //StartDI

                DTrace.WriteLine(new ProgramStarterMessage(Toolx.Note.StartCmdPool()));
                CommandFactory.InitCommandPool(size);
            }));
            return this;
        }

        public void Run(Action work)
        {
            _actions.Add(new Tuple<object, Action<object>>(work, x => {
                DTrace.WriteLine(new ProgramStarterEndMessage());
                if (Toolx.Args.Uninstall())
                {
                    Uninstall();
                    return;
                }
                if (Toolx.Args.Install())
                {
                    Install();
                    return;
                }
                (x as Action)?.Invoke();
            }));
            Task.Factory.StartNew(() =>
            {
                foreach (Tuple<object, Action<object>> tuple in _actions)
                {
                    ExecuteAction(tuple.Item2, tuple.Item1);
                }
            });
        }

        private static void ExecuteAction(Action<object> func, object param)
        {
            try
            {
                func(param);
            }
            catch (Exception e)
            {
                DTrace.WriteLine(e);
            }
        }

        public static void Install()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { TF.AppFile() });
            }
            catch
            {
                Console.WriteLine(@"Failed to install service");
            }
        }

        public static void Uninstall()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { "/u", TF.AppFile() });
            }
            catch
            {
                Console.WriteLine(@"Failed to uninstall service");
            }
        }
    }

    public class ProgramStarterMessage : TypedMessage
    {
        public ProgramStarterMessage(string message) : base(message, null) { }
    }

    public class ProgramStarterEndMessage : ProgramStarterMessage
    {
        public ProgramStarterEndMessage(string message) : base(Toolx.Note.StartEnd()) { }
        public ProgramStarterEndMessage() : this(null) { }
    }

    public interface IRunTrace
    {
        IRunTrace RunTrace(TraceListener traceListener);
        IRunNLog RunNLog<T>() where T : TypedMessage;
    }

    public interface IRunNLog
    {
        IRunNLog RunNLog<T>() where T : TypedMessage;
        IParseCommandLineArguments OnUnhandledException(UnhandledExceptionEventHandler handler);
    }

    public interface IParseCommandLineArguments
    {
        IInitCommandPool ParseCommandLineArguments(string[] args);
    }

    public interface IInitCommandPool
    {
        IRun InitCommandPool(int size);
    }

    public interface IRun
    {
        void Run(Action work);
    }
}