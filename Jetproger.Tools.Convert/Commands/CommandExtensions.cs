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

        public static CommandResponse ServerExecute(this Je.ICmdExpander exp, CommandRequest request)
        {
            var response = new CommandResponse { Session = request.Session };
            try
            {
                if (string.IsNullOrWhiteSpace(request.Command)) return ServerCommander.Run(null, request);
                var command = Je.cmd.CommandOf(request.Command);
                var parameterType = Je.sys.TypeOf(request.Parameter);
                Je.err.GuardTypeNotFound(parameterType == null, request.Parameter);
                return ServerCommander.Run(command, request);
            }
            catch (Exception e)
            {
                var msg = e.As<CommandMessage>();
                Je.log.To(e);
                response.Result = string.Empty;
                response.Report = Je.cmd.ReportOf(msg);
                return response;
            }
        }

        public static ICommand CommandOf(this Je.ICmdExpander exp, string commandTypeName)
        {
            var commandType = Je.sys.TypeOf(commandTypeName);
            Je.err.GuardTypeNotFound(commandType == null, commandTypeName);
            var command = Je.sys.InstanceOf(commandType) as ICommand;
            Je.err.GuardTypeNotSubtype(command == null, commandTypeName, typeof(ICommand).FullName);
            return command;
        }

        public static CommandMessage[] ReportOf(this Je.ICmdExpander exp, params CommandMessage[] messages)
        {
            return messages;
        }

        public static int IndexOf(this Je.ICmdExpander exp, IEnumerable items, object item)
        {
            var i = 0;
            foreach (object obj in items)
            {
                if (ReferenceEquals(obj, item)) return i;
                i++;
            }
            return -1;
        }
    }
}