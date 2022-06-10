using System;
using System.Collections.Generic;
using System.Net;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class RemoteCommand<TResult, TValue> : Command<TResult, TValue>
    { 
        private readonly CommandRequest _request;

        private bool _isSend;

        protected RemoteCommand(string command, string parameter, string result)
        {
            _request = new CommandRequest { Session = Guid.NewGuid(), Command = command, Parameter = parameter, Result = result };
        }

        protected override void Execute()
        {  
            Executing().BeginExecute(x => Je.log.To(x.Error));
        }

        private IEnumerable<ICommand> Executing()
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
            yield return webCmd;
            var response = webCmd.Result;
            Error = Je.err.MsgToErr(response.Report);
            var e = Je.cmd.ErrorEventArgs(this, Error, webCmd.Error);
            if (e.IsSuccess)
            {
                State = response.Result != null ? ECommandState.Completed : ECommandState.None;
                if (State == ECommandState.Completed) Result = Je.xml.To<TResult>(response.Result);
            }
            else
            {
                State = ECommandState.Completed; 
            }
            if (State == ECommandState.Completed)
            {
                _isSend = false;
                EndExecuteEvent(e);
            }
        }
    }

    public class AppRemoteCommand : PostWebCommand<CommandResponse, CommandRequest>
    {
        public AppRemoteCommand(CommandRequest content) : base(Je.web.AppHost, content) { }
    }
}