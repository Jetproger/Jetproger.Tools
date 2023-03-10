using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Commands
{
    public sealed class AsyncCommand<TResult, TValue> : Command<TResult, TValue>
    {
        private readonly Command<TResult, TValue> _command;
        private readonly ICommand _icommand;

        public AsyncCommand(Command<TResult, TValue> command)
        {
            if (command is AsyncCommand<TResult, TValue>) throw new ArgumentException("Argument [command] cannot be a type [AsyncCommand]", "command");
            _command = command;
            _icommand = command;
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
            f.sys.threadof(_icommand.Execute);
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
        event Action ICommand.EndExecute { add { _endExecute += value; } remove { if (_endExecute != null) _endExecute -= value; } }
        protected void Set<T>(T[] holder, T value) { lock (holder) { holder[0] = value; } }
        protected T Get<T>(T[] holder) { lock (holder) { return holder[0]; } }
        protected virtual void Execute() { Result = Value.As<TResult>(); }
        public Command(TValue value) : this() { Value = value; }
        public Command() { InterfaceCommand = this; }
        protected readonly ICommand InterfaceCommand;
        private Action _endExecute;

        ICommand ICommand.Precommand { get => Precommand; set => Precommand = value; }
        public ICommand Precommand { get => GetPrecommand(); private set => SetPrecommand(value); }
        protected virtual void SetPrecommand(ICommand precommand) { Set(_precommand, precommand); }
        protected virtual ICommand GetPrecommand() { return Get(_precommand); }
        private readonly ICommand[] _precommand = { null };

        object ICommand.Value { get => Value;  set => Value = value.As<TValue>(); }
        public TValue Value { get => GetValue(); set => SetValue(value); }
        protected virtual void SetValue(TValue value) { Set(_value, value); }
        protected virtual TValue GetValue() { return Get(_value); }
        private readonly TValue[] _value = { default(TValue) };

        object ICommand.Result => Result;
        public TResult Result { get => GetResult(); protected set => SetResult(value); }
        protected virtual TResult GetResult() { return Get(_result); }
        private readonly TResult[] _result = { default(TResult) };

        Exception ICommand.Error => Error;
        public Exception Error { get => GetError(); protected set => SetError(value); }
        protected virtual Exception GetError() { return Get(_error); }
        public virtual void ErrorExecute(Exception e) { Error = e; }
        private readonly Exception[] _error = { null };

        ECommandState ICommand.State => State;
        public ECommandState State { get => GetState(); protected set => SetState(value); }
        protected virtual ECommandState GetState() { return Get(_state); }
        private readonly ECommandState[] _state = { ECommandState.None };

        private List<CommandMessage> Messages { get { return f.one.of(_ticketsHolder, () => new List<CommandMessage>()); } }
        private void WriteLine(CommandMessage message) { lock (Messages) { Messages.Add(message); } }
        public override void WriteLine(string message) { WriteLine((object)message); }
        public override void Write(string message) { WriteLine((object)message); }
        public override void Write(object message) { WriteLine(message); }
        private readonly List<CommandMessage>[] _ticketsHolder = { null };

        public void StartExecute(TValue value) { Value = value; InterfaceCommand.Execute(); }
        public void AwaitExecute(TValue value) { Value = value; AwaitExecute(); }
        public void StartExecute() { InterfaceCommand.Execute(); }

        public T PreByCommandType<T>() where T : class, ICommand { return f.cmd.precommandof<T>(this); }
        public ICommand PreByResultType<T>() { return f.cmd.preresultof<T>(this); }
        public ICommand PreByValueType<T>() { return f.cmd.prevalueof<T>(this); }
        public ICommand PreByIndex(int i) { return f.cmd.preindexof(this, i); }
        public ICommand PreFirst() { return f.cmd.prefirstof(this); }
        public ICommand PreLast() { return f.cmd.prelastof(this); }

        public override void WriteLine(object message)
        {
            if (message == null) { }
            else
            if (message is CommandMessage commandMessage) WriteLine(commandMessage);
            else
            if (message is Exception exception) WriteLine(new CommandMessage(exception));
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
            if (error == null) return;
            WriteLine(error);
            f.log(error);
            State = ECommandState.Completed;
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
            if (_endExecute != null) _endExecute();
        }
    }

    public interface ICommand
    {
        ICommand Precommand { get; set; }
        void ErrorExecute(Exception e);
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