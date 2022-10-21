using System;
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Commanders
{
    public abstract class BaseCommander
    { 
        private readonly TimeSpan _timespan; 
        protected abstract void Action(); 
        private bool _isRunning; 
        private long _ticks;

        protected BaseCommander(long periodMilliseconds)
        {
            _timespan = TimeSpan.FromMilliseconds(periodMilliseconds);
        }

        public virtual CommandResponse BeginExecute(ICommand command, CommandRequest request)
        {
            if (!_isRunning) (new Thread(Working) { IsBackground = true }).Start();
            return new CommandResponse { Session = request.Session, Result = null, Report = null };
        }

        private void Working()
        {
            _isRunning = true;
            while (true)
            {
                if (Interlocked.Increment(ref _ticks) >= long.MaxValue) break;
                try
                {
                    Action();
                }
                catch (Exception e)
                {
                    Je.log.To(e);
                }
                finally
                {
                    Thread.Sleep(_timespan);
                }
            }
            _isRunning = false;
        }
    }

    public static class BaseCommander<T> where T : BaseCommander
    {
        private static readonly T[] Holder = { null };

        //static BaseCommander() { GTINNlogTracer.Run(); }

        public static T Instance
        {
            get
            {
                if (Holder[0] == null)
                {
                    lock (Holder)
                    {
                        if (Holder[0] == null) Holder[0] = Activator.CreateInstance<T>();
                    }
                }
                return Holder[0];
            }
        }
    }
}