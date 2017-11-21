using System;

namespace Jetproger.Tools.Plugin.Commands
{
    public sealed class CommandDirect : Command
    {
        protected override bool IsExecuteIsolate => false;
        private readonly Action<CommandAgent> _procedure;
        private CommandAgent _agent;

        public CommandDirect(Action<CommandAgent> procedure) : base("Jetproger.Tools.Plugin", "Jetproger.Tools.Plugin.Commands.CommandDirect")
        {
            _procedure = procedure;
        }

        public CommandDirect() : base("Jetproger.Tools.Plugin", "Jetproger.Tools.Plugin.Commands.CommandDirect")
        {
        }

        public void SetAgent(CommandAgent agent)
        {
            _agent = agent;
        }

        protected override void ExecuteCustom()
        {
            _procedure?.Invoke(_agent);
        }
    }
}