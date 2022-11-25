using System.Collections.Generic;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Convert.Commands
{
    public class TaskCommand : WorkCommand<object, object>
    {
        public CommandGroup OnExecute { get { return _onExecute; } set {; } }
        private readonly CommandGroup _onExecute = new CommandGroup();

        protected override void Execute()
        {
            AddCommands(Executing());
            base.Execute();
        }

        private IEnumerable<ICommand> Executing()
        {
            var result = Value;
            for (int i = 0; i < OnExecute.Count; i++)
            {
                var command = OnExecute[i];
                if (command == null) continue;
                command.Value = result;
                yield return command;
                result = command.Result;
            }
        }
    }
}