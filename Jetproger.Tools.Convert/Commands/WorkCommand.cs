using System;
using System.Collections.Generic; 
using System.Threading;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{
    public static class CommandWorkExtensions
    {

        public static void ExecuteAsync(this ICommand command)
        {
            ((ICommand)(new WorkCommand(ExecutingCommand(command)))).Execute();
        }

        public static ICommand Execute(this ICommand command)
        {
            var work = (ICommand)new WorkCommand(ExecutingCommand(command));
            var mre = new ManualResetEvent(false);
            work.EndExecute += () => mre.Set();
            work.Execute();
            mre.WaitOne();
            return command;
        }

        private static IEnumerable<ICommand> ExecutingCommand(ICommand cmd)
        {
            yield return cmd;
        }

        public static void ExecuteAsync(this IEnumerable<ICommand> commands)
        {
            var work = new WorkCommand(commands);
            var cmd = (ICommand)work;
            cmd.Execute();
        }

        public static void ExecuteAsync<TResult>(this IEnumerable<ICommand> commands)
        {
            var work = new WorkCommand<TResult>(commands);
            var cmd = (ICommand)work;
            cmd.Execute();
        }

        public static WorkCommand Execute(this IEnumerable<ICommand> commands)
        {
            var work = new WorkCommand(commands);
            var cmd = (ICommand)work;
            var mre = new ManualResetEvent(false);
            cmd.EndExecute += () => mre.Set();
            cmd.Execute();
            mre.WaitOne();
            return work;
        }

        public static WorkCommand<TResult> Execute<TResult>(this IEnumerable<ICommand> commands)
        {
            var work = new WorkCommand<TResult>(commands);
            var cmd = (ICommand)work;
            var mre = new ManualResetEvent(false);
            cmd.EndExecute += () => mre.Set();
            cmd.Execute();
            mre.WaitOne();
            return work;
        }
    }

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

        protected override void Execute()
        {
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