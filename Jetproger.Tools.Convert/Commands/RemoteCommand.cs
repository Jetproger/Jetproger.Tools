using System;
using System.Collections.Generic;
using System.Net;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{ 
    public abstract class RemoteCommand2<TResult, TValue> : WorkCommand<TResult, TValue>
    {
        private readonly CommandRequest _request;
        private bool _isCompleted;
        private bool _isSend;

        protected RemoteCommand2(string command)
        {
            _request = new CommandRequest { Session = Guid.NewGuid(), Command = command };
        }

        protected override void SetResult(TResult result)
        {
            if (_isCompleted) base.SetResult(result);
        }

        protected override void Execute()
        {
            var request = new CommandRequest { Session = _request.Session };
            if (!_isSend)
            {
                _isSend = true;
                _request.Value = Value.As<SimpleXml>().As<string>();
                request = new CommandRequest { Session = _request.Session, Command = _request.Command, Value = _request.Value };
                f.cmd.ClientEnqueue(request);
            }
            SetCommand(Executing(new _AppRemoteCommand(request)));
            base.Execute();
        }

        private IEnumerable<ICommand> Executing(_AppRemoteCommand webCmd)
        {
            yield return webCmd;
            var response = webCmd.Result;
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
                _isSend = true;
                _isCompleted = false;
                State = ECommandState.None;
            }
            yield return cmdResult;
        }

        private class _AppRemoteCommand : PostWebCommand<CommandResponse, CommandRequest>
        {
            public _AppRemoteCommand(CommandRequest content) : base(f.web.AppHost, content)
            {
                AddHeader(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                AddHeader(HttpRequestHeader.ContentType, "application/json");
                AddHeader(HttpRequestHeader.ContentLength, "0");
                Certificate = f.cry.App;
            }
        }
    }
}