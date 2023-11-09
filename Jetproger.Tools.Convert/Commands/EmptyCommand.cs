using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Convert.Commands
{
    public class EmptyCommand : EmptyCommand<object, object, object, object, object, object, object, object, object, object, object> { }
    public class EmptyCommand<T> : EmptyCommand<T, T, object, object, object, object, object, object, object, object, object> { }
    public class EmptyCommand<T, T_> : EmptyCommand<T, T_, object, object, object, object, object, object, object, object, object, object> { }
    public class EmptyCommand<T, T_, T0> : EmptyCommand<T, T_, T0, object, object, object, object, object, object, object, object, object> { }
    public class EmptyCommand<T, T_, T0, T1> : EmptyCommand<T, T_, T0, T1, object, object, object, object, object, object, object, object> { }
    public class EmptyCommand<T, T_, T0, T1, T2> : EmptyCommand<T, T_, T0, T1, T2, object, object, object, object, object, object, object> { }
    public class EmptyCommand<T, T_, T0, T1, T2, T3> : EmptyCommand<T, T_, T0, T1, T2, T3, object, object, object, object, object, object> { }
    public class EmptyCommand<T, T_, T0, T1, T2, T3, T4> : EmptyCommand<T, T_, T0, T1, T2, T3, T4, object, object, object, object, object> { }
    public class EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5> : EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, object, object, object, object> { }
    public class EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6> : EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, object, object, object> { }
    public class EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7> : EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, object, object> { }
    public class EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8> : EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, object> { }
    public class EmptyCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : BaseCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, ICommand
    {
        public EmptyCommand(IEnumerable<ICommand> commands) : this() { _externalCommands = commands; }
        public EmptyCommand() { }
        protected virtual T Executing() { return GetResult(Value); }
        protected virtual IEnumerable<ICommand> Executings() { yield break; }
        private readonly IEnumerable<ICommand> _externalCommands;
        private IEnumerator<ICommand> _commands;
        private ICommand _currentCommand;
        private ICommand _wrapperCommand;

        public void Execute()
        {
            try
            {
                if (State != ECommandState.None) return;
                State = ECommandState.Running;
                _wrapperCommand = new WrapperCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this);
                _commands = Commands.GetEnumerator();
                _currentCommand = null;
                Executes();
            }
            catch (Exception e)
            {
                Error = e;
            }
        }

        private void Executes()
        {
            try
            {
                if (_currentCommand != null && _currentCommand.Error != null)
                {
                    Error = _currentCommand.Error;
                    return;
                }
                if (!_commands.MoveNext())
                {
                    _currentCommand = _currentCommand ?? _wrapperCommand;
                    Error = _currentCommand.Error;
                    Result = GetResult(_currentCommand.Result);
                    return;
                }
                var command = _commands.Current;
                if (command != null)
                {
                    command.History = _currentCommand ?? command.History;
                    _currentCommand = command;
                    command.Executed -= Executes;
                    command.Executed += Executes;
                    command.Execute();
                    return;
                }
                Executes();
            }
            catch (Exception exception)
            {
                Error = exception;
            }
        }

        private IEnumerable<ICommand> Commands
        {
            get
            {
                yield return _wrapperCommand;
                if (_externalCommands != null) foreach (var command in _externalCommands) yield return command;
                else foreach (var command in Executings()) yield return command;
            }
        }

        private class WrapperCommand<W, W_, W0, W1, W2, W3, W4, W5, W6, W7, W8, W9> : BaseCommand<W, W_, W0, W1, W2, W3, W4, W5, W6, W7, W8, W9>, ICommand
        {
            private readonly EmptyCommand<W, W_, W0, W1, W2, W3, W4, W5, W6, W7, W8, W9> _command;

            public WrapperCommand(EmptyCommand<W, W_, W0, W1, W2, W3, W4, W5, W6, W7, W8, W9> command)
            {
                ((IBaseCommand)this).History = command;
                _command = command;
            }

            public void Execute()
            {
                try
                {
                    if (State != ECommandState.None) return;
                    State = ECommandState.Running;
                    Result = _command.Executing();
                }
                catch (Exception e)
                {
                    Error = e;
                }
            }
        }
    }
    
    public interface ICommand : IBaseCommand
    {
        void Execute();
    }
}