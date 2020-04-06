using System;

namespace Jetproger.Tools.Process.Commands
{
    public sealed class CommandDirect : Command
    {
        protected override bool IsExecuteIsolate => false;
        private readonly Action<CommandAgent> _procedure;
        private CommandAgent _agent;

        public CommandDirect(Action<CommandAgent> procedure) : base("Jetproger.Tools.Process", "Jetproger.Tools.Process.Commands.CommandDirect")
        {
            _procedure = procedure;
        }

        public CommandDirect() : base("Jetproger.Tools.Process", "Jetproger.Tools.Process.Commands.CommandDirect")
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