using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class AsyncCommand<TResult, TValue, TCommand> : Command<TResult, TValue> where TCommand : Command<TResult, TValue>
    {
        private readonly ICommand _icommand;
        private readonly TCommand _command;

        protected AsyncCommand()
        {
            _command = (TCommand)Activator.CreateInstance(typeof(TCommand), this);
            _icommand = _command;
            _icommand.EndExecute += OnEndExecute;
        }

        private void OnEndExecute()
        {
            Error = _command.Error;
            Result = _command.Result;
        }

        protected override void Execute()
        {
            _command.Value = Value;
            CommandThreads.Run(_icommand.Execute);
        }
    }

    public class Command : Command<object, object>
    {
        public Command() { }
        public Command(object value) : this() { Value = value; }
    }

    public class Command<TResult> : Command<TResult, TResult>
    {
        public Command() { }
        public Command(TResult value) : this() { Value = value; }
    }

    public class Command<TResult, TValue> : TraceListener, ICommand
    {
        public Command(TValue value) : this() { Value = value; }
        public Command() { InterfaceCommand = this; }
        protected readonly ICommand InterfaceCommand;
        event Action ICommand.EndExecute { add { _endExecute += value; } remove { if (_endExecute != null) _endExecute -= value; } }
        private Action _endExecute;

        object ICommand.Value { get { return Value; } set { Value = value.As<TValue>(); } }
        public TValue Value { get { return GetValue(); } set { SetValue(value); } }
        protected virtual void SetValue(TValue value) { Set(_value, value); }
        protected virtual TValue GetValue() { return Get(_value); }
        private readonly TValue[] _value = { default(TValue) };

        object ICommand.Result { get { return Result; } }
        public TResult Result { get { return GetResult(); } protected set { SetResult(value); } }
        protected virtual TResult GetResult() { return Get(_result); }
        private readonly TResult[] _result = { default(TResult) };

        Exception ICommand.Error { get { return Error; } }
        public Exception Error { get { return GetError(); } protected set { SetError(value); } }
        protected virtual Exception GetError() { return Get(_error); }
        private readonly Exception[] _error = { null };

        ECommandState ICommand.State { get { return State; } }
        public ECommandState State { get { return GetState(); } protected set { SetState(value); } }
        protected virtual ECommandState GetState() { return Get(_state); }
        private readonly ECommandState[] _state = { ECommandState.None };

        private List<CommandMessage> Messages { get { return f.one.Get(_ticketsHolder, () => new List<CommandMessage>()); } }
        private void WriteLine(CommandMessage ticket) { lock (Messages) { Messages.Add(ticket); } }
        public override void WriteLine(string message) { WriteLine((object)message); }
        public override void Write(string message) { WriteLine((object)message); }
        public override void Write(object message) { WriteLine(message); }
        private readonly List<CommandMessage>[] _ticketsHolder = { null };

        public void StartExecute(TValue value) { Value = value; InterfaceCommand.Execute(); }
        public void AwaitExecute(TValue value) { Value = value; AwaitExecute(); }
        public void StartExecute() { InterfaceCommand.Execute(); }

        public override void WriteLine(object message)
        {
            if (message == null) return;
            if (message is CommandMessage) WriteLine((CommandMessage)message);
            else
            if (message is Exception) WriteLine(new CommandMessage((Exception)message));
            else
            if (true) WriteLine(new CommandMessage(message.ToString()));
        }

        public void AwaitExecute()
        {
            var mre = new ManualResetEvent(false);
            InterfaceCommand.EndExecute += () => mre.Set();
            InterfaceCommand.Execute();
            mre.WaitOne();
        }

        CommandMessage[] ICommand.LogExecute()
        {
            lock (Messages)
            {
                var messages = Messages.ToArray();
                Messages.Clear();
                return messages;
            }
        }

        void ICommand.Execute()
        {
            try
            {
                if (State != ECommandState.None) return;
                State = ECommandState.Running;
                Execute();
            }
            catch (Exception e)
            {
                Error = e;
            }
        }

        protected virtual void Execute()
        {
            Result = Value.As<TResult>();
        }

        public void Reset()
        {
            Set(_error, null);
            Set(_result, default(TResult));
            Set(_state, ECommandState.None);
        }

        protected virtual void SetError(Exception error)
        {
            if (Error != null) return;
            Set(_error, error);
            if (error != null) State = ECommandState.Completed;
        }

        protected virtual void SetResult(TResult result)
        {
            if (State == ECommandState.Completed) return;
            Set(_result, result);
            State = ECommandState.Completed;
        }

        protected virtual void SetState(ECommandState state)
        {
            if (State == ECommandState.Completed) return;
            Set(_state, state);
            if (state != ECommandState.Completed) return;
            var exception = Error;
            if (exception != null)
            {
                WriteLine(exception);
                var commandException = exception as CommandException;
                if (commandException == null)
                {
                    commandException = new CommandException(exception);
                    f.log.To(commandException.Message);
                    Set(_error, commandException);
                }
            }
            if (_endExecute != null) _endExecute();
        }

        protected T Get<T>(T[] holder)
        {
            lock (holder) { return holder[0]; }
        }

        protected void Set<T>(T[] holder, T value)
        {
            lock (holder) { holder[0] = value; }
        }
    }

    public interface ICommand
    {
        CommandMessage[] LogExecute();
        event Action EndExecute;
        ECommandState State { get; }
        object Value { get; set; }
        Exception Error { get; }
        object Result { get; }
        void Execute();
    }

    public enum ECommandState
    {
        None, Running, Completed
    }
}