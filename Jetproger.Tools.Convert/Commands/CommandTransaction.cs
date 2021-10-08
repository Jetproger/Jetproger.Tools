namespace Jetproger.Tools.Convert.Commands
{
    public class CommandTransaction
    {
        public CommandResponse Response { get; private set; }
        public CommandRequest Request { get; private set; }
        public ICommandAsync Command { get; private set; }
        public CommandTransaction(ICommandAsync command, CommandRequest request)
        {
            Response = new CommandResponse { Session = request.Session };
            Request = request;
            Command = command;
        }
    }
}