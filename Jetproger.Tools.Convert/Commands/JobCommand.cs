using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Commands
{
    public class JobCommand : TaskCommand
    {
        protected bool IsRunningBranches { get { return Get(_isRunningBranchesHolder); } set { Set(_isRunningBranchesHolder, value); } }
        private readonly bool[] _isRunningBranchesHolder = { false };

        protected bool IsRunningTrunk { get { return Get(_isRunningTrunkHolder); } set { Set(_isRunningTrunkHolder, value); } }
        private readonly bool[] _isRunningTrunkHolder = { false };

        public ICommand[] Commands { get { return _commands.ToArray(); } set { SetCommands(value); } }
        private readonly List<ICommand> _commands = new List<ICommand>();

        public ErrorCommandGroup OnError { get { return _onError; } set {; } }
        private readonly ErrorCommandGroup _onError;

        private InfinityCursor _branchesInfinityCursor;
        private InfinityCursor _trunkInfinityCursor;
        private TaskCommand _trunkCommand;
        public int Level { get { return GetLevel(); } }
        public JobCommand Parent { get; private set; }
        public int Enabled { get; set; }

        public JobCommand()
        {
            _onError = new ErrorCommandGroup();
            ((ICommand)this).EndExecute += OnEndExecute;
        }

        protected override void Execute()
        {
            _branchesInfinityCursor = GetBranchesInfinityCursor();
            ExecuteBranches();
            _trunkInfinityCursor = GetTrunkInfinityCursor();
            ExecuteTrunk();
        }

        private void ExecuteBranches()
        {
            if (_branchesInfinityCursor == null) return;
            if (IsRunningBranches) return;
            IsRunningBranches = true;
            (new Thread(ExecuteBranchesWork) { IsBackground = true }).Start();
        }

        private void ExecuteBranchesWork()
        {
            var timer = new Stopwatch();
            while (true)
            {
                timer.Start();
                var obj = _branchesInfinityCursor.Next();
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
                timer.Stop();
                if (timer.ElapsedMilliseconds > 9999)
                {
                    timer.Reset();
                    Thread.Sleep(5555);
                }
            }
            IsRunningBranches = false;
        }

        private void ExecuteTrunk()
        {
            if (_trunkInfinityCursor == null) return;
            if (IsRunningTrunk) return;
            IsRunningTrunk = true;
            var timer = new Stopwatch();
            while (true)
            {
                if (!IsRunningTrunk) break;
                timer.Start();
                var obj = _trunkInfinityCursor.Next();
                var cmd = obj as ICommand;
                if (obj == null)
                {
                    Result = Value;
                    break;
                }
                if (cmd != null)
                {
                    cmd.Value = Value;
                    cmd.Execute();
                }
                else
                {
                    Thread.Sleep(333);
                }
                timer.Stop();
                if (timer.ElapsedMilliseconds > 9999)
                {
                    timer.Reset();
                    Thread.Sleep(5555);
                }
            }
            IsRunningTrunk = false;
        }

        private InfinityCursor GetTrunkInfinityCursor()
        {
            _trunkCommand = new TaskCommand { Value = Value };
            ((ICommand)_trunkCommand).EndExecute += OnEndExecuteCurrentTrunkCommand;
            if (Level >= Enabled)
            {
                for (int i = 0; i < OnExecute.Count; i++)
                {
                    var command = OnExecute[i];
                    if (command != null) _trunkCommand.OnExecute += command;
                }
            }
            var command0 = _commands.Count > 0 ? _commands[0] : null;
            if (command0 != null)
            {
                var job = command0 as JobCommand;
                if (job != null) job.Enabled = Enabled;
                _trunkCommand.OnExecute += command0;
            }
            if (_trunkCommand.OnExecute.Count == 0) return null;
            var cursor = new InfinityCursor();
            cursor.Add(_trunkCommand);
            return cursor;
        }

        private void OnEndExecuteCurrentTrunkCommand()
        {
            Error = _trunkCommand.Error;
            Value = _trunkCommand.Result;
            IsRunningTrunk = Error == null;
        }

        private InfinityCursor GetBranchesInfinityCursor()
        {
            var counter = 0;
            var cursor = new InfinityCursor();
            for (int i = 1; i < _commands.Count; i++)
            {
                var command = _commands[i];
                if (command == null) continue;
                cursor.Add(command);
                counter++;
            }
            return counter > 0 ? cursor : null;
        }

        private void OnEndExecute()
        {
            if (Error != null) ExecutingError(this).Execute();
        }

        private IEnumerable<ICommand> ExecutingError(ICommand command)
        {
            for (int i = 0; i < OnError.Count; i++)
            {
                var handler = OnError[i] as ErrorCommand;
                if (handler == null) continue;
                handler.Value = command;
                yield return handler;
            }
        }

        private int GetLevel()
        {
            var i = 0;
            var parent = this;
            while (true)
            {
                if (parent.Parent == null) return i;
                parent = parent.Parent;
                i++;
            }
        }

        protected void AddCommand(ICommand command)
        {
            var job = command as JobCommand;
            if (job != null)
            {
                if (job.Parent != null) RemoveCommand(job.Parent, command);
                job.Parent = this;
            }
            _commands.Add(command);
        }

        private void RemoveCommand(JobCommand job, ICommand command)
        {
            var i = Je.cmd.IndexOf(job.Commands, command);
            if (i > -1) job._commands.RemoveAt(i);
        }

        private void SetCommands(ICommand[] commands)
        {
            _commands.Clear();
            _commands.AddRange(commands);
        }
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
                        if (_completed == _commands.Count) return null;
                        if (_running == _commands.Count) return this;
                        _completed = 0;
                        _running = 0;
                        _curcor = 0;
                    }
                    var command = _commands[_curcor];
                    if (command.State == ECommandState.None) return command;
                    if (command.State == ECommandState.Completed) _completed++;
                    if (command.State == ECommandState.Running) _running++;
                }
            }
        }
    }
}