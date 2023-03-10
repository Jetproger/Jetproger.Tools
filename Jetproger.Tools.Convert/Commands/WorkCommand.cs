using System;
using System.Collections.Generic; 
using System.Threading;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{
    public class WorkCommand : WorkCommand<object, object>
    {
        public WorkCommand(IEnumerable<ICommand> commands) : base(commands) { }
        public WorkCommand() { }
    }

    public class WorkCommand<TResult> : WorkCommand<TResult, TResult>
    {
        public WorkCommand(IEnumerable<ICommand> commands) : base(commands) { }
        public WorkCommand() { }
    }

    public class WorkCommand<TResult, TValue> : Command<TResult, TValue>
    {
        protected void SetCommand(ICommand command) { _commands = (new List<ICommand> { command }).GetEnumerator(); }
        protected void SetCommand(IEnumerable<ICommand> commands) { _commands = commands.GetEnumerator(); }
        public WorkCommand(IEnumerable<ICommand> commands) { _commands = commands.GetEnumerator(); }
        private IEnumerator<ICommand> _commands;
        private ICommand _command;
        public WorkCommand() { }

        public void StartExecute(IEnumerable<ICommand> commands)
        {
            SetCommand(commands);
            InterfaceCommand.Execute();
        }

        public void AwaitExecute(IEnumerable<ICommand> commands)
        {
            SetCommand(commands);
            var mre = new ManualResetEvent(false);
            InterfaceCommand.EndExecute += () => mre.Set();
            InterfaceCommand.Execute();
            mre.WaitOne();
        }

        protected override void Execute()
        {
            _command = null;
            ExecuteCommands();
        }

        private void ExecuteCommands()
        {
            try
            {
                if (_command != null && _command.Error != null)
                {
                    Error = _command.Error;
                    return;
                }
                if (!_commands.MoveNext())
                {
                    if (_command == null)
                    {
                        Result = GetResult(Value);
                        return;
                    }
                    Error = _command.Error;
                    Result = GetResult(_command.Result);
                    return;
                }
                var command = _commands.Current;
                if (command != null)
                {
                    command.Precommand = _command ?? command.Precommand;
                    _command = command;
                    command.EndExecute -= ExecuteCommands;
                    command.EndExecute += ExecuteCommands;
                    command.Execute();
                    return;
                }
                ExecuteCommands();
            }
            catch (Exception exception)
            {
                Error = exception;
            }
        }

        private TResult GetResult(object result)
        {
            try
            {
                return result.As<TResult>();
            }
            catch
            {
                return default(TResult);
            }
        }
    }
}