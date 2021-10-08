using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Jetproger.Tools.Process.Bases;
using Jetproger.Tools.Process.Commands;

namespace Jetproger.Tools.Process.Services
{
    public class SchedulerService : StarterService
    {
        private readonly Tuple<Command, CommandStrategy>[] _strategies;
        private readonly bool[] _isRunningHolder = { false };

        public SchedulerService() : this(null)
        {
        }

        public SchedulerService(CommandStrategy strategy)
        {
            ServiceName = Methods.ConfigAsString("ServiceName", "Jetproger.Tools.Service");
            //var strategy = CoreMethods.Async
            //    .Prepare<TreeLoadCallerLocal>()
            //    .Set(x => x.Code, treeCode)
            //    .Result<ITreeLoadCommand>()
            //    .Tree
            //    .AsCommandStrategy();
            _strategies = strategy.Items.Select(x => new Tuple<Command, CommandStrategy>(x.Command, new CommandStrategy { Items = new List<CommandAgent> { x } })).ToArray();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (_strategies.Length == 0) return;
                if (IsRunning) return;
                IsRunning = true;
                var proc = new Action(BeginStart);
                proc.BeginInvoke(EndStart, proc);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
                IsRunning = false;
            }
        }

        protected override void OnStop()
        {
            try
            {
                lock (_strategies)
                {
                    foreach (var tuple in _strategies)
                    {
                        var strategy = tuple.Item2;
                        strategy.Stop();
                    }
                }
            }
            finally
            {
                IsRunning = false;
            }
        }

        private void BeginStart()
        {
            while (true)
            {
                if (!IsRunning)
                {
                    return;
                }
                lock (_strategies)
                {
                    foreach (var tuple in _strategies)
                    {
                        var command = tuple.Item1;
                        var strategy = tuple.Item2;
                        if (command.Enabled()) strategy.Start();
                    }
                }
                Thread.Sleep(999);
            }
        }

        private void EndStart(IAsyncResult asyncResult)
        {
            try
            {
                ((Action)asyncResult.AsyncState).EndInvoke(asyncResult);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
            }
            finally
            {
                IsRunning = false;
            }
        }

        public bool IsRunning
        {
            get
            {
                lock (_isRunningHolder)
                {
                    return _isRunningHolder[0];
                }
            }
            set
            {
                lock (_isRunningHolder)
                {
                    _isRunningHolder[0] = value;
                }
            }
        }
    }
}