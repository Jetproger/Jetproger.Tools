using System;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Commands
{
    public class AsyncCommand : AsyncCommand<object, object, object, object, object, object, object, object, object, object, object, object> { public AsyncCommand(EmptyCommand command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<T> : AsyncCommand<T, T, object, object, object, object, object, object, object, object, object, object> { public AsyncCommand(EmptyCommand<T> command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<T, T_> : AsyncCommand<T, T_, object, object, object, object, object, object, object, object, object, object> { public AsyncCommand(EmptyCommand<T, T_> command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<T, T_, T0> : AsyncCommand<T, T_, T0, object, object, object, object, object, object, object, object, object> { public AsyncCommand(EmptyCommand<T, T_, T0> command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<T, T_, T0, T1> : AsyncCommand<T, T_, T0, T1, object, object, object, object, object, object, object, object> { public AsyncCommand(EmptyCommand<T, T_, T0, T1> command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<T, T_, T0, T1, T2> : AsyncCommand<T, T_, T0, T1, T2, object, object, object, object, object, object, object> { public AsyncCommand(EmptyCommand<T, T_, T0, T1, T2> command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<T, T_, T0, T1, T2, T3> : AsyncCommand<T, T_, T0, T1, T2, T3, object, object, object, object, object, object> { public AsyncCommand(EmptyCommand<T, T_, T0, T1, T2, T3> command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<T, T_, T0, T1, T2, T3, T4> : AsyncCommand<T, T_, T0, T1, T2, T3, T4, object, object, object, object, object> { public AsyncCommand(EmptyCommand<T, T_, T0, T1, T2, T3, T4> command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5> : AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, object, object, object, object> { public AsyncCommand(EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5> command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6> : AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, object, object, object> { public AsyncCommand(EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6> command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7> : AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, object, object> { public AsyncCommand(EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7> command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8> : AsyncCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, object> { public AsyncCommand(EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8> command) : base(command) { } public AsyncCommand() { } }
    public class AsyncCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : DelayCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, ICommand
    {
        private readonly EmptyCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> _command;
        protected virtual TResult Executing() { return GetResult(Value); }
        protected virtual void BeforeExecute() { }
        protected virtual void AfterExecute() { }
        public AsyncCommand() { }
        private Exception _error;
        private TResult _result;

        public AsyncCommand(EmptyCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> command)
        {
            _command = command;
            ((ICommand)_command).History = this;
        }

        public void Execute()
        {
            try
            {
                BeforeExecute();
                if (State != ECommandState.None) return;
                State = ECommandState.Running;
                if (_command != null)
                {
                    ((ICommand)this).Value = _command;
                    Value = _command.Value;
                }
                var proc = _command != null ? _command.Execute : (Action)BeginExecute;
                proc.BeginInvoke(EndExecute, proc);
            }
            catch (Exception e)
            {
                Error = e;
            }
        }

        private void BeginExecute()
        {
            try
            {
                _result = Executing();
            }
            catch (Exception e)
            {
                _error = e;
            }
        }

        private void EndExecute(IAsyncResult asyncResult)
        {
            try
            {
                ((Action)asyncResult.AsyncState).EndInvoke(asyncResult);
                Delay();
            }
            catch (Exception e)
            {
                Error = e;
            }
        }

        private void Delay()
        {
            if (((IDelay)this).CancelDelay) Completing(); else SetState(ECommandState.Completed);
        }

        public override void Complete()
        {
            Completing();
            base.Complete();
        }

        private void Completing()
        {
            try
            {
                if (_command != null)
                {
                    ((ICommand)this).Value = _command;
                    Value = _command.Value;
                    Error = _command.Error;
                    Result = _command.Result;
                }
                else
                {
                    Error = _error;
                    Result = _result;
                }
                AfterExecute();
            }
            catch (Exception e)
            {
                Error = e;
            }
        }
    }
}