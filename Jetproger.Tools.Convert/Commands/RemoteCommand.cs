using System;
using System.Collections.Generic;
using System.Net;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class RemoteCommand<TResult, TValue> : WorkCommand<TResult, TValue>
    {
        private readonly CommandRequest _request;
        private bool _isCompleted;
        private bool _isSend;

        protected RemoteCommand(string command, string parameter, string result)
        {
            _request = new CommandRequest { Session = Guid.NewGuid(), Command = command, Parameter = parameter, Result = result };
        }

        protected override void SetResult(TResult result)
        {
            if (_isCompleted) base.SetResult(result);
        }

        protected override void Execute()
        {
            var request = new CommandRequest(_request.Session);
            if (!_isSend)
            {
                _isSend = true;
                _request.Document = Je.xml.Of(Value);
                request = new CommandRequest(_request);
                ClientCommander.Run(this, request);
            }
            var webCmd = new AppRemoteCommand(request);
            webCmd.AddHeader(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            webCmd.AddHeader(HttpRequestHeader.ContentType, "application/json");
            webCmd.AddHeader(HttpRequestHeader.ContentLength, "0");
            webCmd.Certificate = Je.cry.App;
            AddCommands(Executing(webCmd));
            base.Execute();
        }

        private IEnumerable<ICommand> Executing(AppRemoteCommand webCmd)
        {
            yield return webCmd;
            var response = webCmd.Result;
            var exception = (new CommandMessage(response.Report)).As<CommandException>();
            if (exception != null)
            {
                _isSend = false;
                _isCompleted = true;
                throw exception;
            }
            var cmdResult = new Command { Value = default(TResult) };
            if (response.Result != null)
            {
                _isSend = false;
                _isCompleted = true;
                cmdResult.Value = string.IsNullOrWhiteSpace(response.Result) ? cmdResult.Value : Je.xml.To<TResult>(response.Result);
            }
            else
            {
                _isSend = true;
                _isCompleted = false;
                State = ECommandState.None;
            }
            yield return cmdResult;
        }
    }

    public class AppRemoteCommand : PostWebCommand<CommandResponse, CommandRequest>
    {
        public AppRemoteCommand(CommandRequest content) : base(Je.web.AppHost, content)
        {

        }
    }
}