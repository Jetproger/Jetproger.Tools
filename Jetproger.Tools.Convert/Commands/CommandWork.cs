using System;
using System.Collections.Generic; 
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{
    public static class CommandWorkExtensions
    {
        public static void BeginExecute(this ICommand command, Action<ICommand> callback)
        {
            var work = new CommandWork(ExecutingCommand(command));
            var cmd = (ICommand)work;
            cmd.EndExecute += x => callback(command);
            cmd.Execute();
        }

        public static ICommand Execute(this ICommand command)
        {
            var work = new CommandWork(ExecutingCommand(command));
            var cmd = (ICommand)work;
            var mre = new ManualResetEvent(false);
            cmd.EndExecute += x => mre.Set();
            cmd.Execute();
            mre.WaitOne();
            return command;
        }

        private static IEnumerable<ICommand> ExecutingCommand(ICommand cmd)
        {
            yield return cmd;
        }

        public static void BeginExecute(this IEnumerable<ICommand> commands, Action<CommandWork> callback)
        {
            var work = new CommandWork(commands);
            var cmd = (ICommand)work;
            cmd.EndExecute += x => callback(work);
            cmd.Execute();
        }

        public static void BeginExecute<TResult>(this IEnumerable<ICommand> commands, Action<CommandWork<TResult>> callback)
        {
            var work = new CommandWork<TResult>(commands);
            var cmd = (ICommand)work;
            cmd.EndExecute += x => callback(work);
            cmd.Execute();
        }

        public static CommandWork Execute(this IEnumerable<ICommand> commands)
        {
            var work = new CommandWork(commands);
            var cmd = (ICommand)work;
            var mre = new ManualResetEvent(false);
            cmd.EndExecute += x => mre.Set();
            cmd.Execute();
            mre.WaitOne();
            return work;
        }

        public static CommandWork<TResult> Execute<TResult>(this IEnumerable<ICommand> commands)
        {
            var work = new CommandWork<TResult>(commands);
            var cmd = (ICommand)work;
            var mre = new ManualResetEvent(false);
            cmd.EndExecute += x => mre.Set();
            cmd.Execute();
            mre.WaitOne();
            return work;
        }
    }

    public class CommandWork : CommandWork<object>
    {
        public CommandWork(IEnumerable<ICommand> commands) : base(commands) { }
    }

    public class CommandWork<TResult> : Command<TResult>
    {
        private readonly IEnumerator<ICommand> _commands;
        private ICommand _command;

        public CommandWork(IEnumerable<ICommand> commands)
        {
            _commands = commands.GetEnumerator();
        }

        protected override void Execute()
        {
            try
            {
                if (_command != null && _command.Error != null)
                {
                    Error = _command.Error;
                    EndExecuteEvent(Je.cmd.ErrorEventArgs(this, Error));
                    return;
                }
                if (!_commands.MoveNext())
                {
                    Result = _command != null ? _command.Result.As<TResult>() : default(TResult);
                    Error = _command != null ? _command.Error : null;
                    EndExecuteEvent(Je.cmd.ErrorEventArgs(this, Error));
                    return;
                }
                var command = _commands.Current;
                if (command != null)
                {
                    OnExecutedAssign(command);
                    _command = command;
                    command.Execute();
                    return;
                }
            }
            catch (Exception exception)
            {
                Error = exception;
                EndExecuteEvent(Je.cmd.ErrorEventArgs(this, exception));
                return;
            }
            Execute();
        }

        private void OnExecutedAssign(ICommand command)
        {
            command.EndExecute -= OnExecuted;
            command.EndExecute += OnExecuted;
        }

        private void OnExecuted(CommandExecuteEventArgs e)
        {
            Execute();
        }
    }
}