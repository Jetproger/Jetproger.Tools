using System;
using System.Net;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class RemoteCommand : RemoteCommand<object, object, object, object, object, object, object, object, object, object, object, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<T> : RemoteCommand<T, T, object, object, object, object, object, object, object, object, object, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<T, T_> : RemoteCommand<T, T_, object, object, object, object, object, object, object, object, object, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<T, T_, T0> : RemoteCommand<T, T_, T0, object, object, object, object, object, object, object, object, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<T, T_, T0, T1> : RemoteCommand<T, T_, T0, T1, object, object, object, object, object, object, object, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<T, T_, T0, T1, T2> : RemoteCommand<T, T_, T0, T1, T2, object, object, object, object, object, object, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<T, T_, T0, T1, T2, T3> : RemoteCommand<T, T_, T0, T1, T2, T3, object, object, object, object, object, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<T, T_, T0, T1, T2, T3, T4> : RemoteCommand<T, T_, T0, T1, T2, T3, T4, object, object, object, object, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<T, T_, T0, T1, T2, T3, T4, T5> : RemoteCommand<T, T_, T0, T1, T2, T3, T4, T5, object, object, object, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<T, T_, T0, T1, T2, T3, T4, T5, T6> : RemoteCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, object, object, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7> : RemoteCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, object, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8> : RemoteCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, object> { protected RemoteCommand(string commandType, string url = null) : base(commandType, url) { } }
    public abstract class RemoteCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : ShareCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        protected RemoteCommand(string commandType, string url = null) : base(commandType) { _url = url; }
        protected override ICommand GetExecuter() { return new _RemoteWebCommand(_url); }
        private static readonly WebClient Client;
        private readonly string _url;

        static RemoteCommand()
        {
            Client = new WebClient();
            Client.Headers[HttpRequestHeader.ContentType] = "text/plain";
            Client.UploadStringCompleted += WebClientUploadStringCompleted;
        }

        private class _RemoteWebCommand : BaseCommand<CommandResponse, CommandRequest>, ICommand
        {
            public _RemoteWebCommand(string url) { _url = url; }
            private readonly string _url;

            public void Execute()
            {
                try
                {
                    if (State != ECommandState.None) return;
                    State = ECommandState.Running;
                    var json = Value.As<NewtonsoftJson>().As<string>();
                    Client.BaseAddress = _url ?? Jetproger.Tools.Convert.Bases.f.web.appurl;
                    Client.UploadStringAsync(new Uri("/jetproger/v1/cmd", UriKind.Relative), "POST", json, this);
                }
                catch (Exception e)
                {
                    Error = e;
                }
            }
        }

        private static void WebClientUploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var cmd = e.UserState as IBaseCommand;
            if (cmd == null)
            {
                return;
            }
            if (e.Error != null)
            {
                cmd.Error = e.Error;
                return;
            }
            try
            {
                var json = (e.Result ?? string.Empty).Trim('\0');
                var result = json.As<NewtonsoftJson>().As<CommandResponse>();
                cmd.Result = result;
            }
            catch (Exception exception)
            {
                cmd.Error = exception;
            }
        }
    }
}