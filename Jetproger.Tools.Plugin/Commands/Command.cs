using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Plugin.Bases;
using Jetproger.Tools.Plugin.Services;
using MD = Tools.Metadata;

namespace Jetproger.Tools.Plugin.Commands
{
    public abstract class Command : TraceListener, ICommand, ICommandIsolate
    {
        private static readonly ConcurrentDictionary<Guid, CommandWorker> Workers = new ConcurrentDictionary<Guid, CommandWorker>();
        private readonly ConcurrentQueue<string> _messages;
        private readonly CommandState[] _stateHolder;
        private readonly string _assemblyName;
        private readonly string _typeName;

        protected Command(string assemblyName, string typeName)
        {
            _messages = new ConcurrentQueue<string>();
            _stateHolder = new[] { CommandState.New };
            _assemblyName = assemblyName;
            _typeName = typeName;
        }

        protected virtual bool IsEnabledIsolate => false;
        protected virtual bool IsExecuteIsolate => true;
        protected virtual bool IsEnabledRemote => false;
        protected virtual bool IsExecuteRemote => false;
        protected virtual string Host => null;
        protected virtual int Port => 0;

        public string CommandName
        {
            get;
            set;
        }

        public CommandState State
        {
            get { return Methods.Lock(_stateHolder); }
            private set { Methods.Lock(_stateHolder, value); }
        }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }

        public override void Write(object message)
        {
            WriteLine(message);
        }

        public override void WriteLine(object message)
        {
            var serverToClient = message as CommandServerToClientMessage;
            if (serverToClient != null && !string.IsNullOrWhiteSpace(serverToClient.Message)) _messages.Enqueue(serverToClient.Message);
        }

        public void Cancel()
        {
            State = CommandState.Cancelled;
        }

        public string[] GetMessages()
        {
            var counter = _messages.Count;
            var list = new List<string>();
            while (counter-- > 0)
            {
                string message;
                if (!_messages.TryDequeue(out message)) break;
                if (!string.IsNullOrWhiteSpace(message)) list.Add(message);
            }
            return list.Count > 0 ? list.ToArray() : null;
        }

        #region Unexecute

        public void Unexecute()
        {
            Base(CommandType.Unexecute);
        }

        string ICommandIsolate.Unexecute()
        {
            return UnexecuteAspect(new [] { false });
        }

        public static CommandResponse UnexecuteService(CommandRequest request)
        {
            return BaseService(request, CommandType.Unexecute);
        }

        protected virtual string UnexecuteAspect(bool[] useServiceToken)
        {
            return BaseAspect(useServiceToken, CommandType.Unexecute);
        }

        protected virtual void UnexecuteCustom()
        {
        }

        #endregion

        #region Execute

        public void Execute()
        {
            Base(CommandType.Execute);
        }

        string ICommandIsolate.Execute()
        {
            return ExecuteAspect(new [] { false });
        }

        public static CommandResponse ExecuteService(CommandRequest request)
        {
            return BaseService(request, CommandType.Execute);
        }

        protected virtual string ExecuteAspect(bool[] useServiceToken)
        {
            return BaseAspect(useServiceToken, CommandType.Execute);
        }

        protected virtual void ExecuteCustom()
        {
        }

        #endregion

        #region Enabled

        public bool Enabled()
        {
            return Base(CommandType.Enabled);
        }

        string ICommandIsolate.Enabled()
        {
            return EnabledAspect(new [] { false });
        }

        public static CommandResponse EnabledService(CommandRequest request)
        {
            return BaseService(request, CommandType.Enabled);
        }

        protected virtual string EnabledAspect(bool[] useServiceToken)
        {
            return BaseAspect(useServiceToken, CommandType.Enabled);
        }

        protected virtual bool EnabledCustom()
        {
            return true;
        }

        #endregion

        #region Base

        public static CommandResponse BaseService(CommandRequest request, CommandType commandType)
        {
            var response = new CommandResponse { SessionId = request.SessionId };
            CommandWorker worker;
            if (!Workers.TryGetValue(request.SessionId, out worker))
            {
                if (request.IsPostBack || request.IsCancel) return response;
                worker = CreateWorker(request, commandType);
            }
            while (true)
            {
                response.Source = worker.GetSource();
                response.Messages = worker.GetMessages();
                response.Error = worker.GetError();
                response.Json = worker.GetResult();
                if (request.IsCancel || worker.IsStopped() || response.IsError || response.IsJson)
                {
                    CloseWorker(request.SessionId, worker);
                    response.IsFinished = true;
                    break;
                }
                if (response.IsMessage)
                {
                    break;
                }
                Thread.Sleep(333);
            }
            return response;
        }

        private bool Base(CommandType commandType)
        {
            if (State == CommandState.Running) return false;
            State = CommandState.Running;
            try
            {
                var useService = false
                    || (commandType == CommandType.Enabled && IsEnabledRemote)
                    || (commandType == CommandType.Enabled && IsEnabledIsolate)
                    || (commandType != CommandType.Enabled && IsExecuteRemote)
                    || (commandType != CommandType.Enabled && IsExecuteIsolate);
                var useServiceToken = new[] { useService };
                var json = MethodAspect(useServiceToken, commandType);
                if (!string.IsNullOrWhiteSpace(json) && commandType != CommandType.Enabled && (useService || useService == useServiceToken[0]))
                {
                    SetPropertyValues(json.As(GetType()));
                }
                return commandType == CommandType.Enabled && json.As<bool>();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
                State = CommandState.Error;
                return false;
            }
            finally
            {
                if (State == CommandState.Running) State = CommandState.Successful;
            }
        }

        private void SetPropertyValues(object source)
        {
            foreach (var p in MD.GetSelfProperties(GetType()))
            {
                p.SetValue(this, p.GetValue(source, null), null);
            }
        }

        private string BaseAspect(bool[] useServiceToken, CommandType commandType)
        {
            var useService = useServiceToken[0];
            useServiceToken[0] = !useServiceToken[0];
            if (!useService)
            {
                try
                {
                    var json = MethodCustom(commandType);
                    State = CommandState.Successful;
                    return json;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(e);
                    State = CommandState.Error;
                    return null;
                }
            }
            var inRequest = new CommandRequest
            {
                SessionId = Guid.NewGuid(),
                TypeName = _typeName,
                AssemblyName = _assemblyName,
                Json = this.AsString(),
                Login = null,
                Password = null,
                IsRemote = (commandType == CommandType.Enabled && IsEnabledRemote) || IsExecuteRemote,
                IsIsolate = (commandType == CommandType.Enabled && IsEnabledIsolate) || IsExecuteIsolate,
            };
            var clientService = inRequest.IsRemote ? Methods.CreateServiceClientRemote(Host, Port) : Methods.CreateServiceClientLocal();
            var request = inRequest;
            while (true)
            {
                var asyncResult = MethodBegin(clientService, request, commandType);
                var response =  MethodEnd(clientService, asyncResult, commandType);
                if (response.IsFinished)
                {
                    State = response.IsError ? CommandState.Error : CommandState.Successful;
                    System.Diagnostics.Trace.WriteLine(response);
                    return response.Json;
                }
                if (State == CommandState.Cancelled)
                {
                    var cancelRequest = new CommandRequest
                    {
                        SessionId = inRequest.SessionId,
                        IsIsolate = inRequest.IsIsolate,
                        IsRemote = inRequest.IsRemote,
                        IsPostBack = true,
                        IsCancel = true
                    };
                    asyncResult = MethodBegin(clientService, cancelRequest, commandType);
                    response = MethodEnd(clientService, asyncResult, commandType);
                    System.Diagnostics.Trace.WriteLine(response);
                    return null;
                }
                System.Diagnostics.Trace.WriteLine(response);
                var postbackRequest = new CommandRequest
                {
                    SessionId = inRequest.SessionId,
                    IsIsolate = inRequest.IsIsolate,
                    IsRemote = inRequest.IsRemote,
                    IsPostBack = true,
                    IsCancel = false
                };
                request = postbackRequest;
            }
        }

        private string MethodCustom(CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.Unexecute: UnexecuteCustom(); return this.AsString();
                case CommandType.Enabled: return EnabledCustom() ? "1" : "0";
                default: ExecuteCustom(); return this.AsString();
            }
        }

        private string MethodAspect(bool[] useServiceToken, CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.Unexecute: return UnexecuteAspect(useServiceToken);
                case CommandType.Enabled: return EnabledAspect(useServiceToken);
                default: return ExecuteAspect(useServiceToken);
            }
        }

        private static IAsyncResult MethodBegin(ICommandService clientService, CommandRequest request, CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.Unexecute: return clientService.BeginInvokeUnexecute(request, null, clientService);
                case CommandType.Enabled: return clientService.BeginInvokeEnabled(request, null, clientService);
                default: return clientService.BeginInvokeExecute(request, null, clientService);
            }
        }

        private static CommandResponse MethodEnd(ICommandService clientService, IAsyncResult asyncResult, CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.Unexecute: return clientService.EndInvokeUnexecute(asyncResult);
                case CommandType.Enabled: return clientService.EndInvokeEnabled(asyncResult);
                default: return clientService.EndInvokeExecute(asyncResult);
            }
        }

        private static CommandWorker CreateWorker(CommandRequest request, CommandType commandType)
        {
            return Workers.GetOrAdd(request.SessionId, x =>
            {
                var worker = request.IsIsolate ? CommandFactory.GetWorker() : new CommandWorker();
                var workerBegin = WorkerBegin(worker, commandType);
                workerBegin.BeginInvoke(request, WorkerEnd, workerBegin);
                return worker;
            });
        }

        private static void CloseWorker(Guid sessionId, CommandWorker worker)
        {
            try
            {
                var domain = worker.GetDomain();
                Workers.TryRemove(sessionId, out worker);
                if (AppDomain.CurrentDomain != domain) AppDomain.Unload(domain);
                Methods.GarbageCollect();
            }
            catch
            {
                Methods.GarbageCollect();
            }
        }

        private static Action<CommandRequest> WorkerBegin(CommandWorker worker, CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.Unexecute: return worker.Unexecute;
                case CommandType.Enabled: return worker.Enabled;
                default: return worker.Execute;
            }
        }

        private static void WorkerEnd(IAsyncResult result)
        {
            try
            {
                ((Action<string[]>)result.AsyncState).EndInvoke(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
            }
        }

        #endregion
    }

    public interface ICommandIsolate
    {
        string Unexecute();
        string Execute();
        string Enabled();
    }

    public interface ICommand
    {
        void Unexecute();
        void Execute();
        bool Enabled();
    }
}