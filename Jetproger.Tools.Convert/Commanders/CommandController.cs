using System; 
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commanders
{
    public class DomainController : MarshalByRefObject
    {
        private readonly CommandController _commandController = new CommandController();
        public void SetOwner(AppDomain owner) { f.owner(owner); }
        public string GetState() { return _state; }
        private static string _state;

        public DomainController()
        {
            AppDomain.CurrentDomain.UnhandledException -= DomainUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += DomainUnhandledException;
        }

        private static void DomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _state = _state ?? e.ExceptionObject?.ToString();
        }

        public CommandResponse Execute(CommandRequest request)
        {
            return _commandController.Execute(request);
        }
    }

    public class CommandController
    {
        public CommandResponse Execute(CommandRequest request)
        {
            var response = new CommandResponse { Session = request.Session };
            try
            {
                if (string.IsNullOrWhiteSpace(request.Command)) return ServerCommandsJob.Instance.Execute(null, request);
                var commandType = f.sys.classof(request.Command);
                if (commandType == null) throw new TypeNotFoundException(request.Command);
                var command = Activator.CreateInstance(commandType) as ICommand;
                if (command == null) throw new TypeNotSubtypeException(request.Command, typeof(ICommand).FullName);
                return ServerCommandsJob.Instance.Execute(command, request);
            }
            catch (Exception e)
            {
                f.log(e);
                response.Result = string.Empty;
                response.Report = e.As<CommandReport>().As<SimpleXml>().As<string>();
                return response;
            }
        }
    }
}