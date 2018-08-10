using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Jetproger.Tools.Convert.Bases
{
    public class Starter : TraceListener, IRegisterLogger, ICustomizeUnhandledExceptionMode, ICustomizeUnhandledExceptionEventHandler, ICustomizeThreadExceptionEventHandler, IRegisterInject, ICreateCommandPool, ICustomizeCompatibleTextRenderingDefault, ICustomizeVisualStyles, IStarter
    {
        private readonly List<object> _messages;
        private readonly int _listenerIndex;
        private readonly List<Task> _tasks;
        private bool _isRegistrating;

        public Starter()
        {
            _isRegistrating = true;
            _tasks = new List<Task>();
            _messages = new List<object>();
            _listenerIndex = Trace.Listeners.Add(this);
            var proc = new Action(Scheduling);
            proc.BeginInvoke(EndScheduling, proc);
        }

        public virtual void RegisterSettings(string[] args)
        {
        }

        IRegisterLogger IRegisterLogger.RegisterLoggerContinue<T>()
        {
            AddAction(RegisterLoggerContinue<T>);
            return this;
        }

        protected virtual void RegisterLoggerContinue<T>() where T : ExTicket
        {
        }

        IRegisterLogger IRegisterLogger.RegisterLoggerContinue(TraceListener listener)
        {
            AddAction(() => RegisterLoggerContinue(listener));
            return this;
        }

        protected virtual void RegisterLoggerContinue(TraceListener listener)
        {
            Trace.Listeners.Add(listener);
        }

        ICustomizeUnhandledExceptionMode IRegisterLogger.RegisterLogger()
        {
            AddAction(() => {
                RegisterLogger();
                Trace.Listeners.RemoveAt(_listenerIndex);
                foreach (var message in _messages) Trace.WriteLine(message);
            });
            return this;
        }

        protected virtual void RegisterLogger()
        {
        }

        ICustomizeUnhandledExceptionEventHandler ICustomizeUnhandledExceptionMode.CustomizeUnhandledExceptionMode(int mode)
        {
            AddAction(() => CustomizeUnhandledExceptionMode(mode));
            return this;
        }

        protected virtual void CustomizeUnhandledExceptionMode(int mode)
        {
        }

        ICustomizeThreadExceptionEventHandler ICustomizeUnhandledExceptionEventHandler.CustomizeUnhandledExceptionEventHandler(UnhandledExceptionEventHandler handler)
        {
            AddAction(() => CustomizeUnhandledExceptionEventHandler(handler));
            return this;
        }

        protected virtual void CustomizeUnhandledExceptionEventHandler(UnhandledExceptionEventHandler handler)
        {
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            if (handler == null) return;
            AppDomain.CurrentDomain.UnhandledException -= handler;
            AppDomain.CurrentDomain.UnhandledException += handler;
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e?.ExceptionObject != null) Trace.WriteLine(new ExTicket { IsException = true, Text = "UnhandledException", Description = e.ExceptionObject.ToString() });
        }

        IRegisterInject ICustomizeThreadExceptionEventHandler.CustomizeThreadExceptionEventHandler(ThreadExceptionEventHandler handler)
        {
            AddAction(() => CustomizeThreadExceptionEventHandler(handler));
            return this;
        }

        protected virtual void CustomizeThreadExceptionEventHandler(ThreadExceptionEventHandler handler)
        {
        }

        ICreateCommandPool IRegisterInject.RegisterInject()
        {
            AddAction(RegisterInject);
            return this;
        }

        protected virtual void RegisterInject()
        {
        }

        ICustomizeCompatibleTextRenderingDefault ICreateCommandPool.CreateCommandPool()
        {
            AddAction(CreateCommandPool);
            return this;
        }

        protected virtual void CreateCommandPool()
        {
        }

        ICustomizeVisualStyles ICustomizeCompatibleTextRenderingDefault.CustomizeCompatibleTextRenderingDefault(bool defaultValue)
        {
            AddAction(() => CustomizeCompatibleTextRenderingDefault(defaultValue));
            return this;
        }

        protected virtual void CustomizeCompatibleTextRenderingDefault(bool defaultValue)
        {
        }

        IStarter ICustomizeVisualStyles.CustomizeVisualStyles(bool enable)
        {
            AddAction(() => CustomizeVisualStyles(enable));
            return this;
        }

        protected virtual void CustomizeVisualStyles(bool enable)
        {
        }

        public void Start(Action action)
        {
            _isRegistrating = false;
            action();
        }

        public override void Write(string message)
        {
            _messages.Add(message);
        }

        public override void WriteLine(string message)
        {
            _messages.Add(message);
        }

        public override void Write(object message)
        {
            _messages.Add(message);
        }

        public override void WriteLine(object message)
        {
            _messages.Add(message);
        }

        private void Scheduling()
        {
            while (true)
            {
                lock (_tasks)
                {
                    if (_tasks.Count > 0)
                    {
                        if (IsEndTask(_tasks[0]))
                        {
                            _tasks.RemoveAt(0);
                            if (_tasks.Count > 0) _tasks[0].Start();
                        }
                    }
                    else
                    {
                        if (!_isRegistrating)
                        {
                            break;
                        }
                    }
                }
                Thread.Sleep(333);
            }
        }

        private void EndScheduling(IAsyncResult asyncResult)
        {
            ((Action)asyncResult.AsyncState).EndInvoke(asyncResult);
        }

        private void AddAction(Action action)
        {
            AddTask(new Task(action));
        }

        private bool IsEndTask(Task task)
        {
            return task.IsCompleted || task.IsCanceled || task.IsFaulted;
        }

        private void AddTask(Task task)
        {
            lock (_tasks)
            {
                _tasks.Add(task);
                if (_tasks.Count == 1) _tasks[0].Start();
            }
        }
    }

    public static class AppExtensions
    {
        private static readonly Type StarterType = typeof(Starter);

        public static IRegisterLogger RegisterSettings(string[] args)
        {
            var starter = GetStarter(Assembly.GetExecutingAssembly());
            starter.RegisterSettings(args);
            return starter;
        }

        private static Starter GetStarter(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(StarterType)) return (Starter)Activator.CreateInstance(type);
            }
            return new Starter();
        }
    }

    public interface IRegisterLogger
    {
        IRegisterLogger RegisterLoggerContinue<T>() where T : ExTicket;
        IRegisterLogger RegisterLoggerContinue(TraceListener listener);
        ICustomizeUnhandledExceptionMode RegisterLogger();
    }

    public interface ICustomizeUnhandledExceptionMode
    {
        ICustomizeUnhandledExceptionEventHandler CustomizeUnhandledExceptionMode(int mode);
    }

    public interface ICustomizeUnhandledExceptionEventHandler
    {
        ICustomizeThreadExceptionEventHandler CustomizeUnhandledExceptionEventHandler(UnhandledExceptionEventHandler handler);
    }

    public interface ICustomizeThreadExceptionEventHandler
    {
        IRegisterInject CustomizeThreadExceptionEventHandler(ThreadExceptionEventHandler handler);
    }

    public interface IRegisterInject
    {
        ICreateCommandPool RegisterInject();
    }

    public interface ICreateCommandPool
    {
        ICustomizeCompatibleTextRenderingDefault CreateCommandPool();
    }

    public interface ICustomizeCompatibleTextRenderingDefault
    {
        ICustomizeVisualStyles CustomizeCompatibleTextRenderingDefault(bool defaultValue);
    }

    public interface ICustomizeVisualStyles
    {
        IStarter CustomizeVisualStyles(bool enable);
    }

    public interface IStarter
    {
        void Start(Action action);
    }
}