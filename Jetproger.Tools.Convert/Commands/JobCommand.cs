using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading; 
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Convert.Commands
{
    public class JobCommand : TaskCommand
    {
        protected bool IsRunningParallel { get { return Get(_isRunningParallelHolder); } set { Set(_isRunningParallelHolder, value); } }
        private readonly bool[] _isRunningParallelHolder = { false };

        protected bool IsRunningExecute { get { return Get(_isRunningExecuteHolder); } set { Set(_isRunningExecuteHolder, value); } }
        private readonly bool[] _isRunningExecuteHolder = { false };

        public CommandGroup OnParallel { get { return _onParallel; } set { } }
        private readonly CommandGroup _onParallel = new CommandGroup();

        public ErrorCommandGroup OnError { get { return _onError; } set {; } }
        private readonly ErrorCommandGroup _onError = new ErrorCommandGroup();

        private InfinityCursor _parallelInfinityCursor;
        
        private InfinityCursor _executeInfinityCursor;

        public JobCommand()
        {
            ((ICommand)this).EndExecute += OnEndExecute;
        }

        protected override void Execute()
        {
            if (IsRunningExecute)
            {
                base.Execute();
                return;
            }
            RunParallel();
            RunExecute();
        }

        #region parallel

        private void RunParallel()
        {
            if (IsRunningParallel) return;
            _parallelInfinityCursor = GetParallelInfinityCursor();
            if (_parallelInfinityCursor == null) return;
            IsRunningParallel = true;
            (new Thread(TryParallel) { IsBackground = true }).Start();
        }

        private InfinityCursor GetParallelInfinityCursor()
        {
            var counter = 0;
            var cursor = new InfinityCursor();
            for (int i = 0; i < _onParallel.Count; i++)
            {
                var command = _onParallel[i];
                if (command == null) continue;
                command.Precommand = this;
                cursor.Add(command);
                counter++;
            }
            return counter > 0 ? cursor : null;
        }

        private void TryParallel()
        {
            try
            {
                WorkParallel();
            }
            finally
            {
                IsRunningParallel = false;
            }
        }

        private void WorkParallel()
        {
            var timer = new Stopwatch();
            while (true)
            {
                if (!IsRunningParallel) break;
                timer.Start();
                var obj = _parallelInfinityCursor.Next();
                var cmd = obj as ICommand;
                if (obj == null) break;
                if (cmd != null)
                {
                    cmd.Value = Value;
                    cmd.Execute();
                }
                else
                {
                    Thread.Sleep(333);
                }
                Timing(timer);
            }
        }

        #endregion

        #region execute

        private void RunExecute()
        {
            if (IsRunningExecute) return;
            _executeInfinityCursor = GetExecuteInfinityCursor();
            if (_executeInfinityCursor == null) return;
            IsRunningExecute = true;
            TryExecute();
        }

        private InfinityCursor GetExecuteInfinityCursor()
        {
            var counter = 0;
            for (int i = 0; i < OnExecute.Count; i++)
            {
                if (OnExecute[i] != null) counter++;
            }
            if (counter == 0) return null;
            var cursor = new InfinityCursor();
            State = ECommandState.None;
            cursor.Add(this);
            return cursor;
        }

        private void TryExecute()
        {
            try
            {
                WorkExecute();
            }
            catch (Exception e)
            {
                Error = e;
            }
            finally
            {
                IsRunningExecute = false;
            }
        }

        private void WorkExecute()
        {
            var timer = new Stopwatch();
            while (true)
            {
                if (!IsRunningExecute) break;
                timer.Start();
                var obj = _executeInfinityCursor.Next();
                var cmd = obj as ICommand;
                if (obj == null) break;
                if (cmd != null)
                {
                    cmd.Value = Value;
                    cmd.Execute();
                }
                else
                {
                    Thread.Sleep(333);
                }
                Timing(timer);
            }
        }

        #endregion

        private void Timing(Stopwatch timer)
        {
            timer.Stop();
            if (timer.ElapsedMilliseconds > 9999)
            {
                timer.Reset();
                Thread.Sleep(5555);
            }
        }

        private void OnEndExecute()
        {
            if (Error != null) (new WorkCommand()).AwaitExecute(ExecutingError(this));
        }

        private IEnumerable<ICommand> ExecutingError(ICommand command)
        {
            for (int i = 0; i < OnError.Count; i++)
            {
                var handler = OnError[i] as ErrorHandlerCommand;
                if (handler == null) continue;
                handler.Value = command;
                yield return handler;
            }
        }

        #region inner types

        private class InfinityCursor
        {
            private readonly List<ICommand> _commands = new List<ICommand>();
            public void Add(ICommand command) { _commands.Add(command); }
            private int _completed = 0;
            private int _running = 0;
            private int _curcor = -1;

            public object Next()
            {
                if (_commands.Count == 0) return null;
                while (true)
                {
                    _curcor++;
                    if (_curcor >= _commands.Count)
                    {
                        var completed = _completed;
                        var running = _running;
                        _completed = 0;
                        _running = 0;
                        _curcor = 0;
                        if (running == _commands.Count) return this;
                        if (completed == _commands.Count) return null;
                    }
                    var command = _commands[_curcor];
                    var state = command.Error != null ? ECommandState.Completed : command.State;
                    if (state == ECommandState.None) return command;
                    if (state == ECommandState.Completed) _completed++;
                    if (state == ECommandState.Running) _running++;
                }
            }
        }

        #endregion
    }
}