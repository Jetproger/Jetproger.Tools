using System.Collections.Generic;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Commanders
{
    public class CommandGroup
    {
        public static CommandGroup operator -(CommandGroup group, ICommand command) { return group.Remove(command); }
        public static CommandGroup operator +(CommandGroup group, ICommand command) { return group.Add(command); }
		public ICommand this[int index] { get { return _commands[index]; } }
		private readonly List<ICommand> _commands = new List<ICommand>();
        
		public int Count { get { return _commands.Count; } }

        protected CommandGroup Add(ICommand command)
        {
            Remove(command);
            _commands.Add(command);
            return this;
        }

        protected CommandGroup Remove(ICommand command)
        {
            var i = Je.cmd.IndexOf(_commands, command);
            if (i > -1) _commands.RemoveAt(i);
            return this;
        }
    }
}