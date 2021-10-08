using System;
using System.Diagnostics;
using System.Threading;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class BaseCommander
    {
        protected abstract void Action();

        private readonly TimeSpan _timespan;

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

        protected CommandResponse Execute(ICommand command, CommandRequest request, out ICommandAsync commandAsync)
        {
            commandAsync = command as ICommandAsync;
            if (commandAsync != null) return null;
            var error = (CommandTicket[])null;
            var result = (string)null;
            try
            {
                result = command.Execute(request.Document);
            }
            catch (Exception e)
            {
                error = Je.cmd.ReportJe.cmd.Error(e));
            }
            return new CommandResponse { Session = request.Session, Result = result ?? string.Empty, Report = error };
        }

        private void Working()
        {
            _isRunning = true;
            while (true)
            {
                if (Interlocked.Increment(ref _ticks) >= long.MaxValue)
                {
                    break;
                }
                var sw = new Stopwatch();
                sw.Start();
                try
                {
                    Action();
                }
                catch (Exception e)
                {
                    Je.log.Error(e);
                }
                finally
                {
                    sw.Stop();
                }
                var interval = sw.Elapsed;
                if (_timespan > interval.Duration()) Thread.Sleep(_timespan.Subtract(interval));
            }
            _isRunning = false;
        }
    }

    public static class BaseCommander<T> where T : BaseCommander
    {
        private static readonly T[] Holder = { null };

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