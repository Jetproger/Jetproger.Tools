using System.Collections.Generic;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Convert.Commands
{
    public class TaskCommand : WorkCommand<object, object>
    {
        public CommandGroup OnExecute { get { return _onExecute; } set {; } }
        private readonly CommandGroup _onExecute;

        public TaskCommand()
        {
            AddCommands(Executing());
            _onExecute = new CommandGroup();
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