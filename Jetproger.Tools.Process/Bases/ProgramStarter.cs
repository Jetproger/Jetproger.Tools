using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Inject.Bases;
using Jetproger.Tools.Process.Commands;
using Jetproger.Tools.Resource.Bases;
using Jetproger.Tools.Trace.Bases;
using TF = Tools.File;

namespace Jetproger.Tools.Process.Bases
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
                System.Diagnostics.Trace.Listeners.Add((TraceListener)x);
                Ex.Trace.Write(new ProgramStarterMessage("Loading resources"));
                AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
                var lang = ConfigurationManager.AppSettings["Culture"];
                if (string.IsNullOrWhiteSpace(lang)) lang = "en-US";
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                Ex.Trace.Write(new ProgramStarterMessage(string.Format(Ex.Rs<StartAppLangSetting>.Description, lang)));
                Ex.Trace.Write(Ex.Rs<StartNLogBaseSetting>.Description);
                Ex.Trace.Write(new ProgramStarterMessage(string.Format(Ex.Rs<StartTraceSetting>.Description, traceListener.GetType().FullName)));
            });
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            IsStopped = true;
            if (e?.ExceptionObject == null) return;
            Error = e.ExceptionObject.ToString();
            Ex.Trace.Write(Error);
        }
    }

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
                Ex.Trace.Write(new ProgramStarterMessage(string.Format(Ex.Rs<StartTraceSetting>.Description, traceListener.GetType().FullName)));
                System.Diagnostics.Trace.Listeners.Add((TraceListener)x);
            }));
            return this;
        }

        public IRunNLog RunNLog<T>() where T : Jc.Ticket
        {
            _actions.Add(new Tuple<object, Action<object>>(null, x => {
                Ex.Trace.Write(new ProgramStarterMessage(string.Format(Ex.Rs<StartNLogTypedSetting>.Description, typeof(T).FullName)));
                Ex.Trace.RegisterFileLogger<T>();
            }));
            return this;
        }

        public IParseCommandLineArguments OnUnhandledException(UnhandledExceptionEventHandler handler)
        {
            _actions.Add(new Tuple<object, Action<object>>(handler, x => {
                Ex.Trace.RegisterFileLogger();
                var y = (UnhandledExceptionEventHandler)x;
                AppDomain.CurrentDomain.UnhandledException -= y;
                AppDomain.CurrentDomain.UnhandledException += y;
            }));
            return this;
        }

        public IInitCommandPool ParseCommandLineArguments(string[] args)
        {
            _actions.Add(new Tuple<object, Action<object>>(args, x => {
                Ex.Trace.Write(new ProgramStarterMessage(Ex.Rs<StartReadConfSetting>.Description));
                //Res.ReadAppConfiguration();

                Ex.Trace.Write(new ProgramStarterMessage(Ex.Rs<StartCmdArgsSetting>.Description));
                //Res.ReadCommandLineArguments((string[])x);
            }));
            return this;
        }

        public IRun InitCommandPool(int size)
        {
            _actions.Add(new Tuple<object, Action<object>>(null, x => {
                Ex.Trace.Write(new ProgramStarterMessage(Ex.Rs<StartDISetting>.Description));
                Ex.Inject.Register();

                Ex.Trace.Write(new ProgramStarterMessage(Ex.Rs<StartCmdPoolSetting>.Description));
                CommandFactory.InitCommandPool(size);
            }));
            return this;
        }

        public void Run(Action work)
        {
            _actions.Add(new Tuple<object, Action<object>>(work, x => {
                Ex.Trace.Write(new ProgramStarterEndMessage());
                if (Ex.Ln<UninstallSetting>.IsDeclare)
                {
                    Uninstall();
                    return;
                }
                if (Ex.Ln<InstallSetting>.IsDeclare)
                {
                    Install();
                    return;
                }
                (x as Action)?.Invoke();
            }));
            Task.Factory.StartNew(() => {
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
                Ex.Trace.Write(e);
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

    public class ProgramStarterMessage : Jc.Ticket
    {
        public ProgramStarterMessage(string message)
        {
            Text = Description= message;
        }
    }

    public class ProgramStarterEndMessage : ProgramStarterMessage
    {
        public ProgramStarterEndMessage(string message) : base(Ex.Rs<MessageStartSetting>.Description) { }
        public ProgramStarterEndMessage() : this(null) { }
    }

    public interface IRunTrace
    {
        IRunTrace RunTrace(TraceListener traceListener);
        IRunNLog RunNLog<T>() where T : Jc.Ticket;
    }

    public interface IRunNLog
    {
        IRunNLog RunNLog<T>() where T : Jc.Ticket;
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