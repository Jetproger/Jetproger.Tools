using System; 
using System.Collections.Generic;
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Commands
{
    public class CommandJob : ICommand
    {
        event CommandExecuteEventHandler ICommand.EndExecute { add { Component.EndExecute += value; } remove { Component.EndExecute -= value; } }
        protected bool IsRunning { get { return Je.one.Get(_isRunningHolder); } set { Je.one.Set(_isRunningHolder, value); } }
        public ECommandState State { get { return Component.State; } protected set { Component.State = value; } }
        public Exception Error { get { return Component.Error; } protected set { Component.Error = value; } }
        public object Result { get { return Component.Result; } protected set { Component.Result = value; } }
        public object Value { get { return Component.Value; } set { Component.Value = value; } }
        CommandMessage[] ICommand.GetLog() { return Component.GetLog(); }
        public ErrorCommandGroup OnError { get { return Component.OnError; } set { } }
        public CommandGroup OnExecute { get { return Component.OnExecute; } set { } }
        protected readonly CommandComponent Component = new CommandComponent();
        private readonly bool?[] _isRunningHolder = { false };
        protected virtual bool IsAbort() { return true; }
        public ICommand Command { get; set; }
        public int StartLevel { get; set; }
        protected int CurrentLevel;

        void ICommand.Execute()
        {
            if (IsRunning) return;
            IsRunning = true;
            try
            {
                while (true)
                {
                    State = ECommandState.Running;
                    Executing().BeginExecute(x =>
                    {
                        Error = x.Error;
                        Result = x.Result;
                        if (Error != null) ExecutingError(this).Execute();
                        Component.EndExecuteEvent(Je.cmd.ErrorEventArgs(this, Error));
                    });
                    Waiting();
                    if (IsAbort()) break;
                }
            }
            finally
            {
                IsRunning = false;
            }
        }

        private IEnumerable<ICommand> Executing()
        {
            var value = Value;
            var command = CurrentLevel >= StartLevel ? Command : null;
            if (command != null)
            {
                TrySetValue(command, value);
                yield return command;
                value = command.Result;
            }
            var nextCommand = OnExecute.Count > 0 ? OnExecute[0] : null;
            Execute(value, nextCommand);
            if (nextCommand != null)
            {
                TrySetJobLevel(nextCommand);
                TrySetValue(nextCommand, value);
                yield return nextCommand;
            }
        }

        private void Execute(object value, ICommand nextCommand)
        {
            for (int i = 0; i < Component.OnExecute.Count; i++)
            {
                var command = Component.OnExecute[i];
                if (command == null || ReferenceEquals(command, nextCommand)) continue;
                TrySetJobLevel(command);
                TrySetValue(command, value);
                CommandThreads.Run(() => command.Execute());
            }
        }

        private void TrySetJobLevel(ICommand command)
        {
            var job = command as CommandJob;
            if (job == null) return;
            job.CurrentLevel = CurrentLevel + 1;
            job.StartLevel = StartLevel;
        }

        private void TrySetValue(ICommand command, object value)
        {
            if (command == null) return;
            command.Value = value;
            command.EndExecute -= EndExecuteError;
            command.EndExecute += EndExecuteError;
        }

        public void ErrorProcessing(ICommand command)
        {
            if (command.Error != null) ExecutingError(command).Execute();
        }

        private void EndExecuteError(CommandExecuteEventArgs e)
        {
            if (e.Command.Error != null) ExecutingError(e.Command).Execute();
        }

        private IEnumerable<ICommand> ExecutingError(ICommand command)
        {
            for (int i = 0; i < Component.OnError.Count; i++)
            {
                var handler = Component.OnError[i] as ErrorHandler;
                if (handler == null) continue;
                handler.Value = command;
                yield return handler;
            }
        }

        private void Waiting()
        {
            while (CurrentState() != ECommandState.Completed) Thread.Sleep(111);
        }

        private ECommandState CurrentState()
        {
            if (Command != null && Command.State != ECommandState.Completed) return State;
            for (int i = 0; i < Component.OnExecute.Count; i++)
            {
                var command = Component.OnExecute[i];
                if (command == null) continue;
                if (command.State != ECommandState.Completed) return State;
            }
            State = ECommandState.Completed;
            return State;
        }
    }

    public class CommandTask : ICommand
    {
        event CommandExecuteEventHandler ICommand.EndExecute { add { Component.EndExecute += value; } remove { Component.EndExecute -= value; } }
        public ECommandState State { get { return Component.State; } protected set { Component.State = value; } }
        public Exception Error { get { return Component.Error; } protected set { Component.Error = value; } }
        public object Result { get { return Component.Result; } protected set { Component.Result = value; } }
        public object Value { get { return Component.Value; } set { Component.Value = value; } }
        CommandMessage[] ICommand.GetLog() { return Component.GetLog(); }
        public CommandGroup OnExecute { get { return Component.OnExecute; } set { } }
        protected readonly CommandComponent Component = new CommandComponent();

        void ICommand.Execute()
        {
            State = ECommandState.Running;
            Result = Value;
            Executing().BeginExecute(x =>
            {
                Error = x.Error;
                Result = x.Result;
                Component.EndExecuteEvent(Je.cmd.ErrorEventArgs(this, Error));
            });
        }

        private IEnumerable<ICommand> Executing()
        { 
            for (int i = 0; i < Component.OnExecute.Count; i++)
            { 
                var command = Component.OnExecute[i];
                if (command == null) continue;
                command.Value = Result;
                yield return command;
                Error = command.Error;
                if (Error != null) break;
                Result = command.Result;
            }
        }
    }

    public sealed class CommandComponent : ICommand
    {
        public event CommandExecuteEventHandler EndExecute { add { _icommand.EndExecute += value; } remove { _icommand.EndExecute -= value; } }
        public ECommandState State { get { return _command.State; } set { _command._SetState(value); } }
        public Exception Error { get { return _command.Error; } set { _command._SetError(value); } }
        public object Result { get { return _command.Result; } set { _command._SetResult(value); } }
        public void EndExecuteEvent(CommandExecuteEventArgs e) { _command._EndExecuteEvent(e); }
        public object Value { get { return _command.Value; } set { _command.Value = value; } }
        public CommandMessage[] GetLog() { return _icommand.GetLog(); }
        public ErrorCommandGroup OnError { get { return _onError; } set { } }
        public CommandGroup OnExecute { get { return _onExecute; } set { } }
        private readonly ErrorCommandGroup _onError;
        private readonly CommandGroup _onExecute;
        private readonly ICommand _icommand;
        private readonly _Command _command;
        void ICommand.Execute() { }

        public CommandComponent()
        {
            _onError = new ErrorCommandGroup();
            _onExecute = new CommandGroup();
            _command = new _Command();
            _icommand = _command;
        }

        private class _Command : Command
        {
            public void _EndExecuteEvent(CommandExecuteEventArgs e) { EndExecuteEvent(e); }
            public void _SetState(ECommandState state) { SetState(state); }
            public void _SetResult(object result) { SetResult(result); }
            public void _SetError(Exception error) { SetError(error); }
        }
    }

    public class ErrorCommandGroup : CommandGroup
    { 
        public static ErrorCommandGroup operator +(ErrorCommandGroup group, ErrorHandler command)
        {
            group.Add(command);
            return group;
        } 

        public static ErrorCommandGroup operator -(ErrorCommandGroup group, ErrorHandler command)
        {
            group.Remove(command);
            return group;
        }
    }


    public abstract class ErrorHandler : Command<ICommand, ICommand> { }

    public class CommandGroup
    { 
        public ICommand this[int index] { get { return _commands[index]; } }
        private readonly List<ICommand> _commands = new List<ICommand>();
        public int Count { get { return _commands.Count; } }

        public static CommandGroup operator +(CommandGroup group, ICommand command)
        {
            group.Add(command);
            return group;
        }

        public static CommandGroup operator -(CommandGroup group, ICommand command)
        {
            group.Remove(command);
            return group;
        }

        protected void Add(ICommand command)
        {
            Remove(command);
            _commands.Add(command);
        }

        protected void Remove(ICommand command)
        {
            var i = IndexOf(command);
            if (i > -1) _commands.RemoveAt(i);
        }

        protected int IndexOf(ICommand command)
        {
            for (int i = 0; i < _commands.Count; i++)
            {
                if (ReferenceEquals(_commands[i], command)) return i;
            }
            return -1;
        }
    }
}