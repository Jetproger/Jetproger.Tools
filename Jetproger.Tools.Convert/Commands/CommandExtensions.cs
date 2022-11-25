using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{ 
    public static class CommandExtensions
    {

        public static CommandResponse ServerExecute(this f.ICmdExpander exp, CommandRequest request)
        {
            var response = new CommandResponse { Session = request.Session };
            try
            {
                if (string.IsNullOrWhiteSpace(request.Command)) return ServerCommander.Run(null, request);
                var command = f.sys.commandof(request.Command);
                var parameterType = f.sys.classof(request.Parameter);
                f.err.GuardTypeNotFound(parameterType == null, request.Parameter);
                return ServerCommander.Run(command, request);
            }
            catch (Exception e)
            {
                var msg = e.As<CommandMessage>();
                f.log.To(e);
                response.Result = string.Empty;
                response.Report = f.cmd.reportof(msg);
                return response;
            }
        }

        public static CommandMessage[] reportof(this f.ICmdExpander exp, params CommandMessage[] messages)
        {
            return messages;
        }
    }
}