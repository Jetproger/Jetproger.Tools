using System;
using System.Collections.Generic;
using System.Net;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Works;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class RemoteCommand<TOutput, TInput> : BaseCommandAsync<TOutput, TInput>
    {
        private readonly CommandRequest _request;
        private bool _isSend;

        protected RemoteCommand(string command)
        {
            GTINNlogTracer.Run();
            _request = new CommandRequest { Session = Guid.NewGuid(), Command = command, };
        }

        protected override void BeginExecute(Action<string> callback, string xml)
        {
            _isSend = false;
            _request.Document = xml;
            _request.Session = Guid.NewGuid();
            base.BeginExecute(callback, xml);
            Je.cmd.BeginExecute<ClientCommander>(this, _request);
        }

        protected override string Execute(string xml)
        {
            _request.Document = xml;
            Executing().Work()
            .Catch(x => SetError(x))
            .Start();
            return null;
        }

        private IEnumerable<IWork> Executing()
        {
            var request = CreateRequest();
            var error = (Exception)null;
            IsRunning = true;
            Result = null;
            Error = null;
            var webWork = WebWork<CommandResponse>.Op(GetEfUrlService() + "cmd")
                .Header(HttpRequestHeader.AcceptEncoding, "gzip,deflate")
                .Header(HttpRequestHeader.ContentType, "application/json")
                .Header(HttpRequestHeader.ContentLength, "0")
                .Certificate(Je.Cert.Self)
                .Content(Je.web.To(request))
                .POST()
                .Catch(x => error = x);
            yield return webWork;
            EndWork(webWork, error);
        }

        protected override void ApplyWork(object work)
        {
            var webWork = (WebWork<CommandResponse>)work;
            Result = webWork.Result.Result;
            Error = Error ?? Je.cmd.LastError(webWork.Result.Report);
            Je.cmd.Log(webWork.Result.Report);
        }

        private CommandRequest CreateRequest()
        {
            if (_isSend) return new CommandRequest { Session = _request.Session, Command = null, Document = null };
            _isSend = true;
            return new CommandRequest { Session = _request.Session, Command = _request.Command, Document = _request.Document };
        }
 
        private string GetEfUrlService()
        {
            if (string.IsNullOrWhiteSpace(Je<GTINEfHost>.As)) { throw new Exception(@"В конфиге не указан url сервиса KIZAPI (нода <GTINEfHost>)");}
            return Je.Config.EfHost;
        }
    }
}