using System;
using System.Web.Http;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Commands
{
    public class CommandController : ApiController
    {
        [HttpPost]
        public CommandResponse ExecuteCommand(CommandRequest request)
        {
            var response = new CommandResponse { Session = request.Session };
            try
            {
                if (string.IsNullOrWhiteSpace(request.Command)) return ServerCommander.Run(null, request);
                var commandType = Je.sys.TypeOf(request.Command);
                Je.err.GuardTypeNotFound(commandType == null, request.Command);
                var parameterType = Je.sys.TypeOf(request.Parameter);
                Je.err.GuardTypeNotFound(parameterType == null, request.Parameter);
                var command = Activator.CreateInstance(commandType) as ICommand;
                Je.err.GuardTypeNotSubtype(command == null, request.Command, typeof(ICommand).FullName);
                return ServerCommander.Run(command, request);
            }
            catch (Exception e)
            {
                var msg = Je.err.ErrToMsg(e);
                Je.log.To(e);
                response.Result = string.Empty;
                response.Report = Je.cmd.MessagesOf(msg);
                return response;
            }
        }
    }
}