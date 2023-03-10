using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commanders
{
    public class CommandTransaction
    {
        public CommandResponse Response { get; private set; }
        public CommandRequest Request { get; private set; } 
        public ICommand Command { get; private set; }
        private int _expirationCounter = 0;

        public CommandTransaction(CommandRequest request)
        {
            Request = request;
            Command = request.As<ICommand>();
            Response = new CommandResponse { Session = request.Session };
        }

        public bool IsExpired()
        {
            _expirationCounter = Command.State == ECommandState.Completed ? _expirationCounter + 1 : _expirationCounter; return _expirationCounter > 9;
        }
    }
}