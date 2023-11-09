using System;
using System.Collections.Generic;
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Commands
{
    public class JobCommand : TaskCommand
    {
        private ErrorCommandGroup _onError;
        private CommandGroup _onParallel;
        private bool _isInitialize;

        public JobCommand()
        {
            ((ICommand)this).Executed += OnEndExecute;
            _onError = new ErrorCommandGroup();
            _onParallel = new CommandGroup();
            BeforeExecuted += OnBeforeExecuted;
        }

        public ErrorCommandGroup OnError
        {
            get { return _onError; }
            set { _onError = _onError ?? value; }
        }

        public CommandGroup OnParallel
        {
            get { return _onParallel; }
            set { _onParallel = _onParallel ?? value; }
        }

        private void OnBeforeExecuted()
        {
            if (_isInitialize) return;
            _isInitialize = true;
            State = _processor != null ? State : ECommandState.None;
            AddCommands(GetCommands());
        }

        private ICommand[] GetCommands()
        {
            var list = new List<ICommand>();
            for (var i = 0; i < OnExecute.Count; i++)
            {
                if (OnExecute[i] == null) continue;
                list.Add(this);
                break;
            }
            for (var i = 0; i < OnParallel.Count; i++)
            {
                var command = OnParallel[i];
                if (command == null) continue;
                command.History = this;
                list.Add(command);
            }
            return list.ToArray();
        }

        private void OnEndExecute()
        {
            if (Error == null) return;
            for (int i = 0; i < OnError.Count; i++)
            {
                var handler = OnError[i] as ErrorHandlerCommand;
                if (handler == null) continue;
                handler.Value = this;
                f.cmd.wait(handler);
            }
        }

        #region processor

        private static _Processor _processor;

        private static void AddCommands(IEnumerable<ICommand> commands)
        {
            var needStart = _processor == null;
            _processor = _processor ?? new _Processor();
            _processor.Add(commands);
            if (f.sys.iscmd())
            {
                _processor.Processing();
            }
            else
            {
                if (needStart) (new Thread(_processor.Processing)).Start();
            }
        }

        private class _Processor
        {
            private readonly System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();
            public void Add(IEnumerable<ICommand> commands) { _commands.AddRange(commands); }
            private readonly List<ICommand> _commands = new List<ICommand>();

            public void Processing()
            {
                for (int i = 0, c = 0, r = 0; i > int.MinValue; i = i < int.MaxValue ? i + 1 : 0)
                {
                    if (_commands.Count == 0)
                    {
                        Thread.Sleep(100);
                        if (f.sys.iscmd()) return; else continue;
                    }
                    _stopwatch.Stop();
                    if (_stopwatch.ElapsedMilliseconds > 500)
                    {
                        _stopwatch.Restart();
                        Thread.Sleep(100);
                    }
                    _stopwatch.Start();
                    if (c >= _commands.Count)
                    {
                        if (f.sys.iscmd()) return;
                        if (_commands.Count == r) Thread.Sleep(100);
                        c = 0;
                        r = 0;
                    }
                    var command = _commands[c];
                    if (command.State == ECommandState.Completed)
                    {
                        _commands.RemoveAt(c);
                    }
                    else
                    {
                        c = c + 1;
                        r = command.State == ECommandState.Running ? r + 1 : r;
                        if (command.State == ECommandState.None) try { command.Execute(); } catch (Exception e) { command.Error = e; }
                    }
                }
            }
        }

        #endregion
    }

    public class InfinityJob : JobCommand
    {
        public InfinityJob(long periodMilliseconds, long minMilliseconds, long maxMilliseconds) { _watcher = CommandWatcher.CreateCommandWatcherMilliseconds(periodMilliseconds, minMilliseconds, maxMilliseconds); }
        protected override ECommandState GetState() { return _watcher.Expired() ? base.GetState() : ECommandState.Running; }
        private readonly CommandWatcher _watcher;

        protected override void SetState(ECommandState state)
        {
            if (state == ECommandState.Completed)
            {
                if (Error != null || Result == null)
                {
                    _watcher.Start();
                }
                state = ECommandState.None;
                Reset();
                try
                {
                    OnExecute.Renew();
                }
                catch (Exception e)
                {
                    f.log(e);
                }
            }
            base.SetState(state);
        }
    }

    public class InterruptJob : JobCommand
    {
        protected override void SetState(ECommandState state)
        {
            if (state == ECommandState.Completed)
            {
                if (Error != null || Result == null)
                {
                    base.SetState(state);
                    return;
                }
                state = ECommandState.None;
                Reset();
                try
                {
                    OnExecute.Renew();
                }
                catch (Exception e)
                {
                    f.log(e);
                }
            }
            base.SetState(state);
        }
    }
}

namespace Jetproger.Tools.AppConfig
{
    public class JetprogerJobCommandPeriodSeconds : ConfigSetting { public JetprogerJobCommandPeriodSeconds() : base("1") { } }
}