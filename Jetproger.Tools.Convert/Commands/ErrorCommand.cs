using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Convert.Commands
{
    public class ErrorCommandGroup : CommandGroup
    {
        public static ErrorCommandGroup operator -(ErrorCommandGroup group, ErrorCommand command) { return (ErrorCommandGroup)group.Remove(command); }
        
        public static ErrorCommandGroup operator +(ErrorCommandGroup group, ErrorCommand command) { return (ErrorCommandGroup)group.Add(command); }
    }

    public abstract class ErrorCommand : Command<ICommand, ICommand>
    {
    }
}