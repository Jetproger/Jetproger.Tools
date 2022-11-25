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
        protected void AddCommands(IEnumerable<ICommand> commands) { _commands = commands.GetEnumerator(); }
        public WorkCommand(IEnumerable<ICommand> commands) { _commands = commands.GetEnumerator(); }
        private IEnumerator<ICommand> _commands;
        private ICommand _command;
        public WorkCommand() { }

        public void StartExecute(IEnumerable<ICommand> commands)
        {
            _commands = commands.GetEnumerator();
            InterfaceCommand.Execute();
        }

        public void AwaitExecute(IEnumerable<ICommand> commands)
        {
            _commands = commands.GetEnumerator();
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
                    if (_command != null)
                    {
                        Error = _command.Error;
                        Result = GetResult(_command.Result);
                    }
                    else
                    {
                        Result = GetResult(Value);
                    }
                    return;
                }
                var command = _commands.Current;
                if (command != null)
                {
                    _command = command;
                    command.EndExecute -= ExecuteCommands;
                    command.EndExecute += ExecuteCommands;
                    command.Execute();
                    return;
                }
            }
            catch (Exception exception)
            {
                Error = exception;
                return;
            }
            ExecuteCommands();
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