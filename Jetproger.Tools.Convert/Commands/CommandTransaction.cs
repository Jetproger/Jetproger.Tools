namespace Jetproger.Tools.Convert.Commands
{
    public class CommandTransaction
    {
        public CommandResponse Response { get; private set; } 

        public CommandRequest Request { get; private set; } 

        public ICommand Command { get; private set; }

        public CommandTransaction(ICommand command, CommandRequest request)
        {
            Response = new CommandResponse { Session = request.Session };
            Request = request;
            Command = command;
        }

        private int _expirationCounter = 0;
        public bool IsExpiration()
        {
            if (Command.State == ECommandState.Completed) _expirationCounter++;
            return _expirationCounter > 9;
        }
    }
}