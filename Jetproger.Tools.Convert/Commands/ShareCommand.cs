using System;
using System.Collections.Generic;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commanders
{
    public class ServerCommandsJob : CommandsJob
    {
        private static readonly ServerCommandsJob[] InstanceHolder = { null };
        private ServerCommandsJob() { }

        public static ServerCommandsJob Instance
        {
            get
            {
                if (InstanceHolder[0] == null)
                {
                    lock (InstanceHolder)
                    {
                        if (InstanceHolder[0] == null)
                        {
                            InstanceHolder[0] = new ServerCommandsJob();
                            InstanceHolder[0].Execute();
                        }
                    }
                }
                return InstanceHolder[0];
            }
        }

        protected override ECommandState StateTransaction(_CommandTransaction transaction)
        {
            return transaction.Command.State != ECommandState.Completed ? transaction.Command.State : (transaction.Response == null && !transaction.IsExpired() ? ECommandState.Running : ECommandState.Completed);
        }

        public override CommandResponse Execute(ICommand command, CommandRequest request)
        {
            var response = new CommandResponse { Session = request.Session, Result = null, Report = null };
            var transaction = ExistTransaction(request.Session);
            if (transaction != null)
            {
                response.Report = transaction.Command.Trace.As<CommandReport>().As<SimpleXml>().As<string>();
                if (transaction.Command.State == ECommandState.Completed) transaction.SetResponseResults(response);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.Command)) response.Report = (new CommandNotDefinedException()).As<CommandReport>().As<SimpleXml>().As<string>();
                else StartTransaction(request.Session, new _CommandTransaction(command, request));
            }
            return response;
        }
    }

    public class ClientCommandsJob : CommandsJob
    {
        private static readonly ClientCommandsJob[] InstanceHolder = { null };
        private ClientCommandsJob() { }

        public static ClientCommandsJob Instance
        {
            get
            {
                if (InstanceHolder[0] == null)
                {
                    lock (InstanceHolder)
                    {
                        if (InstanceHolder[0] == null)
                        {
                            InstanceHolder[0] = new ClientCommandsJob();
                            InstanceHolder[0].Execute();
                        }
                    }
                }
                return InstanceHolder[0];
            }
        }
    }

    public abstract class CommandsJob : InfinityJob
    {
        protected void StartTransaction(Guid session, _CommandTransaction transaction) { _transactions.StartTransaction(session, transaction); }
        protected virtual ECommandState StateTransaction(_CommandTransaction transaction) { return transaction.Command.State; }
        protected _CommandTransaction ExistTransaction(Guid session) { return _transactions.ExistTransaction(session); }
        private readonly _CommandTransactions _transactions;

        protected CommandsJob() : base(k<JetprogerJobCommandPeriodSeconds>.key<int>() * 1000, 1000, 5000)
        {
            _transactions = new _CommandTransactions(this);
            OnExecute += () => new _FinderCommand(this);
        }

        public virtual CommandResponse Execute(ICommand command, CommandRequest request)
        {
            StartTransaction(request.Session, new _CommandTransaction(command, request));
            return new CommandResponse { Session = request.Session, Result = null, Report = null };
        }

        private class _FinderCommand : BaseCommand<object, object>, ICommand
        {
            public _FinderCommand(CommandsJob job) { _job = job; }
            private readonly CommandsJob _job;
            public void Execute()
            {
                try
                {
                    if (State != ECommandState.None) return;
                    State = ECommandState.Running;
                    Result = _job._transactions.FindTransaction() ?? (object)this;
                }
                catch (Exception e)
                {
                    Error = e;
                }
            }
        }

        #region inner types

        private class _CommandTransactions
        {
            public _CommandTransaction ExistTransaction(Guid session) { lock (_hash) { return _hash.ContainsKey(session) ? _hash[session] : null; } }
            private _CommandTransaction ExistTransaction(int index) { lock (_hash) { return index < _list.Count ? _list[index] : null; ; } }
            private int Count() { lock (_hash) { return _list.Count; } }
            private readonly Dictionary<Guid, _CommandTransaction> _hash = new Dictionary<Guid, _CommandTransaction>();
            private readonly List<_CommandTransaction> _list = new List<_CommandTransaction>();
            public _CommandTransactions(CommandsJob job) { _job = job; }
            private readonly CommandsJob _job;

            public _CommandTransaction FindTransaction()
            {
                var transaction = (_CommandTransaction)null;
                for (int i = 0; i < Count();)
                {
                    if (i >= Count()) break;
                    var currentTransaction = ExistTransaction(i);
                    if (currentTransaction == null) break;
                    var state = _job.StateTransaction(currentTransaction);
                    if (state == ECommandState.Completed)
                    {
                        CloseTransaction(currentTransaction.Request.Session, i);
                    }
                    else
                    {
                        transaction = transaction ?? (state == ECommandState.None ? currentTransaction : null);
                        i++;
                    }
                }
                if (transaction != null) ExecuteTransaction(transaction);
                return transaction;
            }

            public void StartTransaction(Guid session, _CommandTransaction transaction)
            {
                lock (_hash)
                {
                    if (_hash.ContainsKey(session)) return;
                    _hash.Add(session, transaction);
                    _list.Add(transaction);
                }
            }

            private void CloseTransaction(Guid session, int index)
            {
                lock (_hash)
                {
                    if (_hash.ContainsKey(session)) _hash.Remove(session);
                    _list.RemoveAt(index);
                }
            }

            private void ExecuteTransaction(_CommandTransaction transaction)
            {
                try
                {
                    transaction.Command.Value = transaction.Request;
                    transaction.Request.Value = null;
                    transaction.Request.P0 = null;
                    transaction.Request.P1 = null;
                    transaction.Request.P2 = null;
                    transaction.Request.P3 = null;
                    transaction.Request.P4 = null;
                    transaction.Request.P5 = null;
                    transaction.Request.P6 = null;
                    transaction.Request.P7 = null;
                    transaction.Request.P8 = null;
                    transaction.Request.P9 = null;
                    transaction.Command.Execute();
                }
                catch (Exception e)
                {
                    transaction.Command.Error = e;
                }
            }
        }

        protected class _CommandTransaction
        {
            private static readonly TimeSpan Timespan = TimeSpan.FromMilliseconds(1200000);
            private readonly IParameterizedCommand _parameterizedCommand;
            private DateTime? _timestamp;
            public CommandResponse Response;
            public CommandRequest Request;
            public ICommand Command;

            public _CommandTransaction(ICommand command, CommandRequest request)
            {
                _parameterizedCommand = (IParameterizedCommand)command;
                Request = request;
                Command = command;
            }

            public bool IsExpired()
            {
                _timestamp = _timestamp ?? (Command.State == ECommandState.Completed ? DateTime.UtcNow : (DateTime?)null);
                return _timestamp != null && (DateTime.UtcNow - _timestamp.Value) >= Timespan;
            }

            public void SetResponseResults(CommandResponse response)
            {
                try
                {
                    response.P0 = _parameterizedCommand.P0 == null || Type0 == TypeObject ? (string)null : (f.sys.issimple(Type0) ? _parameterizedCommand.P0.As<string>() : _parameterizedCommand.P0.As<SimpleXml>().As<string>());
                    response.P1 = _parameterizedCommand.P1 == null || Type1 == TypeObject ? (string)null : (f.sys.issimple(Type1) ? _parameterizedCommand.P1.As<string>() : _parameterizedCommand.P1.As<SimpleXml>().As<string>());
                    response.P2 = _parameterizedCommand.P2 == null || Type2 == TypeObject ? (string)null : (f.sys.issimple(Type2) ? _parameterizedCommand.P2.As<string>() : _parameterizedCommand.P2.As<SimpleXml>().As<string>());
                    response.P3 = _parameterizedCommand.P3 == null || Type3 == TypeObject ? (string)null : (f.sys.issimple(Type3) ? _parameterizedCommand.P3.As<string>() : _parameterizedCommand.P3.As<SimpleXml>().As<string>());
                    response.P4 = _parameterizedCommand.P4 == null || Type4 == TypeObject ? (string)null : (f.sys.issimple(Type4) ? _parameterizedCommand.P4.As<string>() : _parameterizedCommand.P4.As<SimpleXml>().As<string>());
                    response.P5 = _parameterizedCommand.P5 == null || Type5 == TypeObject ? (string)null : (f.sys.issimple(Type5) ? _parameterizedCommand.P5.As<string>() : _parameterizedCommand.P5.As<SimpleXml>().As<string>());
                    response.P6 = _parameterizedCommand.P6 == null || Type6 == TypeObject ? (string)null : (f.sys.issimple(Type6) ? _parameterizedCommand.P6.As<string>() : _parameterizedCommand.P6.As<SimpleXml>().As<string>());
                    response.P7 = _parameterizedCommand.P7 == null || Type7 == TypeObject ? (string)null : (f.sys.issimple(Type7) ? _parameterizedCommand.P7.As<string>() : _parameterizedCommand.P7.As<SimpleXml>().As<string>());
                    response.P8 = _parameterizedCommand.P8 == null || Type8 == TypeObject ? (string)null : (f.sys.issimple(Type8) ? _parameterizedCommand.P8.As<string>() : _parameterizedCommand.P8.As<SimpleXml>().As<string>());
                    response.P9 = _parameterizedCommand.P9 == null || Type9 == TypeObject ? (string)null : (f.sys.issimple(Type9) ? _parameterizedCommand.P9.As<string>() : _parameterizedCommand.P9.As<SimpleXml>().As<string>());
                    response.Result = (Command.Result == null || TypeResult == TypeObject ? string.Empty : (f.sys.issimple(TypeResult) ? Command.Result.As<string>() : Command.Result.As<SimpleXml>().As<string>())) ?? string.Empty;
                    Response = response;
                }
                catch (Exception e)
                {
                    Command.Error = e;
                }
            }
        }

        #endregion
    }

    public abstract class ShareCommand : ShareCommand<object, object, object, object, object, object, object, object, object, object, object, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<T> : ShareCommand<T, T, object, object, object, object, object, object, object, object, object, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<T, T_> : ShareCommand<T, T_, object, object, object, object, object, object, object, object, object, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<T, T_, T0> : ShareCommand<T, T_, T0, object, object, object, object, object, object, object, object, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<T, T_, T0, T1> : ShareCommand<T, T_, T0, T1, object, object, object, object, object, object, object, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<T, T_, T0, T1, T2> : ShareCommand<T, T_, T0, T1, T2, object, object, object, object, object, object, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<T, T_, T0, T1, T2, T3> : ShareCommand<T, T_, T0, T1, T2, T3, object, object, object, object, object, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<T, T_, T0, T1, T2, T3, T4> : ShareCommand<T, T_, T0, T1, T2, T3, T4, object, object, object, object, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<T, T_, T0, T1, T2, T3, T4, T5> : ShareCommand<T, T_, T0, T1, T2, T3, T4, T5, object, object, object, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<T, T_, T0, T1, T2, T3, T4, T5, T6> : ShareCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, object, object, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7> : ShareCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, object, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8> : ShareCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, object> { protected ShareCommand(string commandType, BaseCommand<CommandResponse, CommandRequest> command) : base(commandType) { } }
    public abstract class ShareCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : BaseCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, ICommand
    {
        private CommandRequest CloneRequest() { return new CommandRequest { Session = _request.Session, Command = _request.Command, Value = _request.Value, P0 = _request.P0, P1 = _request.P1, P2 = _request.P2, P3 = _request.P3, P4 = _request.P4, P5 = _request.P5, P6 = _request.P6, P7 = _request.P7, P8 = _request.P8, P9 = _request.P9 }; }
        private _ShareCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> _command;
        protected abstract ICommand GetExecuter();
        private readonly CommandRequest _request;
        protected virtual void BeforeExecute() { } 
        protected virtual void AfterExecute() { }
        protected event Action BeforeExecuted;
        protected event Action AfterExecuting;
        protected event Action AfterExecuted;
        private bool _isClientCommander;
        private bool _isSend;

        protected ShareCommand(string commandType)
        {
            _request = new CommandRequest { Session = Guid.NewGuid(), Command = commandType, };
        }

        private void SetRequestValues()
        {
            try
            {
                _request.P0 = Type0 == TypeObject ? (string)null : (f.sys.issimple(Type0) ? P0.As<string>() : P0.As<SimpleXml>().As<string>());
                _request.P1 = Type1 == TypeObject ? (string)null : (f.sys.issimple(Type1) ? P1.As<string>() : P1.As<SimpleXml>().As<string>());
                _request.P2 = Type2 == TypeObject ? (string)null : (f.sys.issimple(Type2) ? P2.As<string>() : P2.As<SimpleXml>().As<string>());
                _request.P3 = Type3 == TypeObject ? (string)null : (f.sys.issimple(Type3) ? P3.As<string>() : P3.As<SimpleXml>().As<string>());
                _request.P4 = Type4 == TypeObject ? (string)null : (f.sys.issimple(Type4) ? P4.As<string>() : P4.As<SimpleXml>().As<string>());
                _request.P5 = Type5 == TypeObject ? (string)null : (f.sys.issimple(Type5) ? P5.As<string>() : P5.As<SimpleXml>().As<string>());
                _request.P6 = Type6 == TypeObject ? (string)null : (f.sys.issimple(Type6) ? P6.As<string>() : P6.As<SimpleXml>().As<string>());
                _request.P7 = Type7 == TypeObject ? (string)null : (f.sys.issimple(Type7) ? P7.As<string>() : P7.As<SimpleXml>().As<string>());
                _request.P8 = Type8 == TypeObject ? (string)null : (f.sys.issimple(Type8) ? P8.As<string>() : P8.As<SimpleXml>().As<string>());
                _request.P9 = Type9 == TypeObject ? (string)null : (f.sys.issimple(Type9) ? P9.As<string>() : P9.As<SimpleXml>().As<string>());
                _request.Value = TypeValue == TypeObject ? (string)null : (f.sys.issimple(TypeValue) ? Value.As<string>() : Value.As<SimpleXml>().As<string>());
            }
            catch (Exception e)
            {
                Error = e;
            }
        }

        public void Execute()
        {
            try
            {
                if (ClientCommandsJob.Instance == null) return;
                BeforeExecute();
                if (BeforeExecuted != null) BeforeExecuted();
                if (State != ECommandState.None) return;
                State = ECommandState.Running;
                _command = new _ShareCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this);
                var interfaceCommand = (ICommand)_command;
                interfaceCommand.Executed += OnExecuted;
                interfaceCommand.History = this;
                _command.Value = Value;
                _command.Execute();
            }
            catch (Exception e)
            {
                Error = e;
            }
        }

        private void OnExecuted()
        {
            _isSend = _command.IsSend;
            if (_command.IsCompleted)
            {
                Error = _command.Error;
                Result = _command.Result;
                AfterExecute();
                if (AfterExecuted != null) AfterExecuted();
            }
            else
            {
                State = ECommandState.None;
                if (AfterExecuting != null) AfterExecuting();
            }
        }

        #region inner types

        private class _ShareCommand<TResult1, TValue1, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : EmptyCommand<TResult1, TValue1, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>
        {
            private readonly ShareCommand<TResult1, TValue1, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> _command; 
            public bool IsCompleted;
            public bool IsSend;

            public _ShareCommand(ShareCommand<TResult1, TValue1, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> command)
            {
                IsSend = command._isSend;
                _command = command;
            }

            protected override IEnumerable<ICommand> Executings()
            {
                var request = new CommandRequest { Session = _command._request.Session };
                if (!IsSend)
                {
                    IsSend = true;
                    if (!_command._isClientCommander) _command.SetRequestValues();
                    request = _command.CloneRequest();
                    if (!_command._isClientCommander)
                    {
                        ClientCommandsJob.Instance.Execute(_command, request);
                        _command._isClientCommander = true; 
                    }
                }
                var cmd = _command.GetExecuter();
                cmd.Value = request;
                yield return cmd;
                var response = (CommandResponse)cmd.Result;
                var exception = response != null ? response.Report.As<SimpleXml>().As<CommandReport>().As<Exception>() : new CommandNotDefinedException();
                var result = default(TResult1);
                if (CommandNotDefinedException.IsCommandNotDefined(exception))
                {
                    IsSend = false;
                    IsCompleted = false;
                }
                else if (exception != null)
                {
                    IsSend = true;
                    IsCompleted = true;
                }
                else if (response.Result != null)
                {
                    IsSend = true;
                    IsCompleted = true;
                    ((IBaseCommand)this).Value = response;
                }
                else
                {
                    IsSend = true;
                    IsCompleted = false;
                }
                Result = result;
            }
        }
        #endregion
    }
}