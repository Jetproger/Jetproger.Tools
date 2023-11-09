using System;
using System.Collections.Generic;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Commanders
{
    public class ErrorCommandGroup : CommandGroup
    {
        public static ErrorCommandGroup operator -(ErrorCommandGroup group, ErrorHandlerCommand command) { return (ErrorCommandGroup)group.Remove(command); }
        public static ErrorCommandGroup operator +(ErrorCommandGroup group, ErrorHandlerCommand command) { return (ErrorCommandGroup)group.Add(command); }
    }

    public abstract class ErrorHandlerCommand : EmptyCommand<ICommand, ICommand>
    {
    }

    public class CommandGroup
    {
        public static CommandGroup operator +(CommandGroup group, Func<ICommand> constructor) { return group.Add(constructor); }
        public static CommandGroup operator -(CommandGroup group, ICommand command) { return group.Remove(command); }
        public static CommandGroup operator +(CommandGroup group, ICommand command) { return group.Add(command); }

        private readonly List<Func<ICommand>> _constructors = new List<Func<ICommand>>();
        private readonly List<ICommand> _commands = new List<ICommand>();

        public ICommand this[int index] { get { return _commands[index]; } }
        public int Count { get { return _commands.Count; } }

        protected CommandGroup Add(Func<ICommand> constructor)
        {
            if (constructor != null)
            {
                _commands.Add(constructor());
                _constructors.Add(constructor);
            }
            else
            {
                _commands.Add(null);
                _constructors.Add(null);
            }
            return this;
        }

        protected CommandGroup Add(ICommand command)
        {
            Remove(command);
            _commands.Add(command);
            var constructor = (Func<ICommand>)null;
            if (command != null) constructor = () => (ICommand)Activator.CreateInstance(command.GetType());
            _constructors.Add(constructor);
            return this;
        }

        protected CommandGroup Remove(ICommand command)
        {
            var i = f.sys.indexof(_commands, command);
            if (i > -1)
            {
                _commands.RemoveAt(i);
                _constructors.RemoveAt(i);
            }
            return this;
        }

        public void Renew()
        {
            _commands.Clear();
            foreach (Func<ICommand> constructor in _constructors)
            {
                _commands.Add(constructor != null ? constructor() : null);
            }
        }
    }
}