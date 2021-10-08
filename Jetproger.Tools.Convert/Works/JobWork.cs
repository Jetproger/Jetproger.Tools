using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Works
{
    public class JobWork : JobWork<object>
    {
        public JobWork(IEnumerable<IWork> works) : base(works) { }
    }

    public class JobWork<TResult> : Work<TResult>
    {
        private readonly IEnumerator<IWork> _works;

        private IWork _lastWork; 

        public JobWork(IEnumerable<IWork> works)
        { 
            _works = works.GetEnumerator();
        }

        protected override void OnStart()
        {
            Continue();
        }

        public override void Continue()
        {
            try
            {
                if (!_works.MoveNext())
                {
                    Result = _lastWork != null ? _lastWork.Result.As<TResult>() : default(TResult);
                    Disposed();
                    return;
                }
                var work = _works.Current;
                if (work != null)
                {
                    _lastWork = work;
                    work.Assign(this);
                    work.Start();
                    return;
                }
            }
            catch (Exception e)
            {
                ErrorAction(e);
                return;
            }
            Continue();
        }
    }

    public static class WorkExtensions
    {
        public static Work<T> Work<T>(this IEnumerable<IWork> works)
        {
            return new JobWork<T>(works);
        }

        public static Work<object> Work(this IEnumerable<IWork> works)
        {
            return new JobWork(works);
        }
    }

    public class ThreadStock
    {
        private static ThreadStock Stock { get { return Je.one.Get(StockHolder, () => new ThreadStock()); } }
        private static readonly ThreadStock[] StockHolder = { null };
        private readonly ConcurrentBag<ThreadHolder> _threads = new ConcurrentBag<ThreadHolder>();
        private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
        private readonly ManualResetEvent _mre = new ManualResetEvent(false);
        private long _ticks;

        public static void Run(Action action)
        {
            Stock.Execute(action);
        }

        private ThreadStock()
        {
            var max = Je<ThreadStockMaxThreadCount>.As.As<int>();
            for (int i = 0; i < max; i++) _threads.Add(new ThreadHolder(this));
            (new Thread(Working) { IsBackground = true }).Start();
        }

        public void ThreadAdd(ThreadHolder thread)
        {
            _threads.Add(thread);
        }

        private void Execute(Action action)
        {
            ThreadHolder thread;
            if (_threads.TryTake(out thread))
            {
                thread.Execute(action);
            }
            else
            {
                _actions.Enqueue(action);
                _mre.Set();
            }
        }

        private void Working()
        {
            while (true)
            {
                if (Interlocked.Increment(ref _ticks) >= long.MaxValue) break;
                ThreadHolder thread;
                if (_threads.TryTake(out thread))
                {
                    Action action;
                    if (_actions.TryDequeue(out action))
                    {
                        thread.Execute(action);
                        continue;
                    }
                    ThreadAdd(thread);
                }
                _mre.WaitOne();
                _mre.Reset();
            }
        }
    }

    public class ThreadHolder
    {
        private readonly ManualResetEvent _mre = new ManualResetEvent(false);
        private readonly ThreadStock _threadStock;
        private Action _action;
        private long _ticks;

        public ThreadHolder(ThreadStock threadStock)
        {
            _threadStock = threadStock;
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
                Je.log.Error(e);
            }
            finally
            {
                _threadStock.ThreadAdd(this);
            }
        }
    }
}