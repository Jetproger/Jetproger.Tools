using System;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Convert.Commands
{
    public class TaskCommand : BaseCommand, ICommand
    {
        private CommandGroup _onExecute = new CommandGroup();
        protected virtual void BeforeExecute() { }
        protected virtual void AfterExecute() { }
        protected event Action BeforeExecuted;
        protected event Action AfterExecuted;
        public int Enabled { get; set; }
        private ICommand _command;

        public CommandGroup OnExecute
        {
            get { return _onExecute; }
            set { _onExecute = _onExecute ?? value; }
        }

        public void Execute()
        {
            try
            {
                BeforeExecute();
                if (BeforeExecuted != null) BeforeExecuted();
                if (State != ECommandState.None) return;
                State = ECommandState.Running;
                _command = new _TaskCommand(this);
                _command.Executed += OnExecuted;
                _command.History = this;
                _command.Value = Value;
                _command.Execute();
            }
            catch (Exception e)
            {
                Error = e;
            }
        }

        private void OnExecuted()
        {
            Error = _command.Error;
            Result = _command.Result;
            AfterExecute();
            if (AfterExecuted != null) AfterExecuted();
        }

        private class _TaskCommand : EmptyCommand<object, object>
        {
            private readonly TaskCommand _command;

            public _TaskCommand(TaskCommand command)
            {
                _command = command;
            }

            protected override System.Collections.Generic.IEnumerable<ICommand> Executings()
            {
                var result = Value;
                var command0 = (ICommand)null;
                for (int i = 0; i < _command.OnExecute.Count; i++)
                {
                    if (i < _command.Enabled) continue;
                    var command = _command.OnExecute[i];
                    if (command == null) continue;
                    if (command0 == null)
                    {
                        command0 = command;
                        command0.History = this;
                    }
                    command.Value = result;
                    yield return command;
                    result = command;
                }
            }
        }
    }
}