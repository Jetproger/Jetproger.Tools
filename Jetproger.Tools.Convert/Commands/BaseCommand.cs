using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Commands
{
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
        public Command()
        {
            InterfaceCommand = this;
            InterfaceCommand.EndExecute += OnExecuted;
        }

        event CommandExecuteEventHandler ICommand.EndExecute { add { _endExecute += value; } remove { if (_endExecute != null) _endExecute -= value; } }
        private CommandExecuteEventHandler _endExecute;
        public Command(TValue value) : this() { Value = value; }
        protected readonly ICommand InterfaceCommand;

        object ICommand.Value { get { return Value; } set { Value = value.As<TValue>(); } }
        public TValue Value { get { return GetValue(); } set { SetValue(value); } }
        protected virtual void SetValue(TValue value) { Set(_value, value); }
        protected virtual TValue GetValue() { return Get(_value); }
        private readonly TValue[] _value = { default(TValue) };

        object ICommand.Result { get { return Result; } }
        public TResult Result { get { return GetResult(); } protected set { SetResult(value); } }
        protected virtual void SetResult(TResult result) { Set(_result, result); }
        protected virtual TResult GetResult() { return Get(_result); }
        private readonly TResult[] _result = { default(TResult) };

        Exception ICommand.Error { get { return Error; } }
        public Exception Error { get { return GetError(); } protected set { SetError(value); } }
        protected virtual void SetError(Exception error) { Set(_error, error); }
        protected virtual Exception GetError() { return Get(_error); }
        private readonly Exception[] _error = { null };

        ECommandState ICommand.State { get { return State; } }
        public ECommandState State { get { return GetState(); } protected set { SetState(value); } }
        protected virtual void SetState(ECommandState state) { Set(_state, state); }
        protected virtual ECommandState GetState() { return Get(_state); }
        private readonly ECommandState[] _state = { ECommandState.None };

        private List<CommandMessage> Messages { get { return Je.one.Get(_messagesHolder, () => new List<CommandMessage>()); } }
        private void WriteLine(CommandMessage message) { lock (Messages) { Messages.Add(message); } }
        public override void WriteLine(string message) { WriteLine((object)message); }
        public override void Write(string message) { WriteLine((object)message); }
        public override void Write(object message) { WriteLine(message); }
        private readonly List<CommandMessage>[] _messagesHolder = { null };

        CommandMessage[] ICommand.GetLog()
        {
            lock (Messages)
            {
                var tickets = Messages.ToArray();
                Messages.Clear();
                return tickets;
            }
        }

        public virtual void Execute(TValue value)
        {
            Value = value;
            InterfaceCommand.Execute();
        }

        void ICommand.Execute()
        {
            try
            {
                State = ECommandState.Running;
                Execute();
            }
            catch (Exception e)
            {
                Finalize(e);
            }
        }

        protected virtual void Execute()
        {
            Result = Value.As<TResult>();
            EndExecuteEvent(Je.cmd.EmptyEventArgs(this));
        }

        protected void EndExecuteEvent(CommandExecuteEventArgs e)
        {
            if (_endExecute != null) _endExecute(e);
        }

        private void OnExecuted(CommandExecuteEventArgs e)
        {
            State = ECommandState.Completed;
            e.IsSuccess = false;
            if (Error != null) return;
            e.IsSuccess = true;
            if (e.Exceptions == null || e.Exceptions.Length == 0) return;
            foreach (Exception exception in e.Exceptions)
            {
                if (IsFinalizeWithError(exception)) e.IsSuccess = false;
            }
        }

        public override void WriteLine(object message)
        {
            if (message == null)
            {
                return;
            } 
            var msg = message as CommandMessage;
            if (msg != null)
            {
                WriteLine(msg);
                return;
            }
            var e = message as Exception;
            if (e != null)
            {
                WriteLine(Je.err.ErrToMsg(e));
                return;
            } 
            var s = message.ToString();
            if (!string.IsNullOrWhiteSpace(s))
            {
                WriteLine(Je.cmd.TraceMsg(s));
                return;
            }
        }

        protected void Finalize(Exception e)
        {
            var exceptions = new List<Exception> { e };
            try
            {
                IsFinalizeWithError(e);
            }
            catch (Exception exception)
            {
                exceptions.Add(exception);
            }
            finally
            {
                EndExecuteEvent(Je.cmd.ErrorEventArgs(this, exceptions));
            }
        }

        protected bool IsFinalizeWithError(Exception e)
        {
            State = ECommandState.Completed;
            if (e == null) return false;
            Error = e;
            var msg = Je.err.ErrToMsg(e);
            if (msg == null) return false;
            WriteLine(msg);
            return true;
        }

        protected bool IsFinalizeWithError(string s)
        {
            State = ECommandState.Completed;
            if (string.IsNullOrWhiteSpace(s)) return false;
            Error = new Exception(s);
            var msg = Je.cmd.ErrorMsg(s);
            if (msg == null) return false;
            WriteLine(msg);
            return true;
        }

        protected bool IsFinalizeWithMessage(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            var msg = Je.cmd.TraceMsg(s);
            if (msg == null) return false;
            WriteLine(msg);
            return true;
        }

        private static T Get<T>(T[] holder)
        {
            lock (holder) { return holder[0]; }
        }

        private static void Set<T>(T[] holder, T value)
        {
            lock (holder) { holder[0] = value; }
        }
    }

    public interface ICommand
    {
        event CommandExecuteEventHandler EndExecute;
        CommandMessage[] GetLog();
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

    public delegate void CommandExecuteEventHandler(CommandExecuteEventArgs e);

    public class CommandExecuteEventArgs : EventArgs
    {
        public bool IsSuccess { get; set; }
        public ICommand Command { get; set; }
        public Exception[] Exceptions { get; set; }
    }
}