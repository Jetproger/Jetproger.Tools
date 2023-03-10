using System.Collections.Generic;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Convert.Commands
{
    public class TaskCommand : WorkCommand<object, object>
    {

        private readonly CommandGroup _onExecute = new CommandGroup();

        public int Enabled { get; set; }

        public CommandGroup OnExecute
        {
            get { return _onExecute; }
            set { }
        }

        protected override void Execute()
        {
            SetCommand(Executing());
            base.Execute();
        }

        private IEnumerable<ICommand> Executing()
        {
            var result = Value;
            var command0 = (ICommand)null;
            for (int i = 0; i < OnExecute.Count; i++)
            {
                if (i < Enabled) continue;
                var command = OnExecute[i];
                if (command == null) continue;
                if (command0 == null)
                {
                    command0 = command;
                    command0.Precommand = this;
                }
                command.Value = result;
                yield return command;
                result = command.Result;
            }
        }
    }
}