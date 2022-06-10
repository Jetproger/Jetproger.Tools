using System;
using System.Collections.Concurrent;
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Commands
{
    public class CommandThreads
    {
        private static CommandThreads Threads { get { return Je.one.Get(ThreadsHolder, () => new CommandThreads()); } }
        private static readonly CommandThreads[] ThreadsHolder = { null };
        private readonly ConcurrentBag<CommandThread> _threads = new ConcurrentBag<CommandThread>();

        public static void Run(Action action)
        {
            Threads.Execute(action);
        }

        public static void Put(CommandThread thread)
        {
            Threads._threads.Add(thread);
        }

        private CommandThreads()
        {
            _threads.Add(new CommandThread());
            _threads.Add(new CommandThread());
            _threads.Add(new CommandThread());
            _threads.Add(new CommandThread());
        }

        private void Execute(Action action)
        {
            var counter = 0;
            while (true)
            {
                if (counter++ > 3) break;
                var thread = GetThread();
                if (thread != null)
                {
                    thread.Execute(action);
                    return;
                }
                Thread.Sleep(999);
            }
           (new CommandThread()).Execute(action);
        }

        private CommandThread GetThread()
        {
            CommandThread thread; return _threads.TryTake(out thread) ? thread : null;
        }
    }

    public class CommandThread
    {
        private readonly ManualResetEvent _mre = new ManualResetEvent(false);
        private Action _action;
        private long _ticks;

        public CommandThread()
        { 
            (new Thread(Working) { IsBackground = true }).Start();
        }

        public void Execute(Action action)
        {
            _action = action;
            _mre.Set();
        }

        private void Working()
        {
            while (true)
            {
                if (Interlocked.Increment(ref _ticks) >= long.MaxValue) break;
                _mre.WaitOne();
                _mre.Reset();
                InvokeAction();
            }
        }

        private void InvokeAction()
        {
            try
            {
                if (_action != null) _action();
            }
            catch (Exception e)
            {
                Je.log.To(e);
            }
            finally
            {
                CommandThreads.Put(this);
            }
        }
    }
}