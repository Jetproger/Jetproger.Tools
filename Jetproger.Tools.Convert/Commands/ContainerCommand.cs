using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class ContainerCommand<TResult, TValue> : WorkCommand<TResult, TValue>
    {
        private readonly CommandRequest _request;
        private string _containerId;
        private bool _isCompleted;
        private bool _isSend;

        protected ContainerCommand(string commandType)
        {
            _request = new CommandRequest { Session = Guid.NewGuid(), Command = commandType };
        }

        protected override void SetResult(TResult result)
        {
            if (_isCompleted) base.SetResult(result);
        }

        protected override void Execute()
        {
            var request = new CommandRequest { Session = _request.Session }; ;
            if (!_isSend)
            {
                _isSend = true;
                _request.Value = Value.As<SimpleXml>().As<string>();
                request = new CommandRequest { Session = _request.Session, Command = _request.Command, Value = _request.Value };
                _containerId = SafeCommandController.GetContainerId(request);
                f.cmd.ClientEnqueue(request);
            }
            var cmd = f.cmd.asyncof(new _ContainerCommand { ContainerId = _containerId, Request = request });
            SetCommand(Executing(cmd));
            base.Execute();
        }

        private IEnumerable<ICommand> Executing(Command<CommandResponse, CommandRequest> cmd)
        {
            yield return cmd;
            var response = cmd.Result;
            var exception = response.As<Exception>();
            if (exception != null)
            {
                _isSend = false;
                _isCompleted = true;
                throw exception;
            }
            var cmdResult = new Command
            {
                Value = response.As<TResult>()
            };
            if (response.Result != null)
            {
                _isSend = false;
                _isCompleted = true;
            }
            else
            {
                _isSend = !((response.Report ?? new CommandMessage[0]).Any(x => x.Category == ECommandMessage.Warn.ToString() && x.Message == typeof(ContainerNotFoundException).Name));
                _isCompleted = false;
                State = ECommandState.None;
            }
            yield return cmdResult;
        }

        private class _ContainerCommand : Command<CommandResponse, CommandRequest>
        {
            public CommandRequest Request { get; set; }
            public string ContainerId { get; set; }
            protected override void Execute() { Result = SafeCommandController.Execute(ContainerId, Request); }
        }
    }
}