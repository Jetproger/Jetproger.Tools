using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Jetproger.Tools.Convert.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Commanders
{
    public class ServerCommander : BaseCommander
    {
        public ServerCommander() : base(1000 * k<ServerCommanderPeriodSeconds>.As<int>()) { }

        public override CommandResponse Execute(CommandRequest request)
        {
            var existTransaction = ExistTransaction(request.Session);
            if (existTransaction != null)
            {
                var result = (string)null;
                if (existTransaction.Command.State == ECommandState.Completed)
                {
                    CloseTransaction(request.Session);
                    result = (existTransaction.Command.Result != null ? existTransaction.Command.Result.As<SimpleXml>().As<string>() : string.Empty) ?? string.Empty;
                }
                return new CommandResponse { Session = request.Session, Result = result, Report = existTransaction.Command.LogExecute() };
            }
            if (string.IsNullOrWhiteSpace(request.Command)) return new CommandResponse { Session = request.Session, Result = string.Empty, Report = (new CommandNotDefinedException(request.Session)).As<CommandMessage[]>() };
            var transaction = new CommandTransaction(request);
            if (StartTransaction(request.Session, transaction))
            {
                base.Execute(request);
                Thread.Sleep(1111);
                return Execute(request);
            }
            return base.Execute(request);
        }

        protected override bool IsTransactionCompleted(CommandTransaction transaction)
        {
            try
            {
                if (transaction.Command.State == ECommandState.None) transaction.Command.Execute();
                if (transaction.IsExpired()) return true;
            }
            catch (Exception e)
            {
                transaction.Command.ErrorExecute(e);
                return true;
            }
            return false;
        }
    }

    public class ClientCommander : BaseCommander
    {
        public ClientCommander() : base(1000 * k<ClientCommanderPeriodSeconds>.As<int>()) { } 

        public override CommandResponse Execute(CommandRequest request)
        {
            var transaction = new CommandTransaction(request);
            StartTransaction(request.Session, transaction);
            return base.Execute(request);
        }

        protected override bool IsTransactionCompleted(CommandTransaction transaction)
        {
            try
            {
                if (transaction.Command.State == ECommandState.None) transaction.Command.Execute();
                if (transaction.Command.State == ECommandState.Completed) return true;
            }
            catch (Exception e)
            {
                transaction.Command.ErrorExecute(e);
                return true;
            }
            return false;
        }
    }

    public abstract class BaseCommander
    { 
        private readonly ConcurrentDictionary<Guid, CommandTransaction> _commands = new ConcurrentDictionary<Guid, CommandTransaction>();
        public int CommandCount => _commands.Count;
        private readonly long _periodMilliseconds;
        private Thread _thread;

        private void Working() { foreach (var deleted in (from x in _commands.Values where IsTransactionCompleted(x) select x.Request.Session)) CloseTransaction(deleted); }
        protected CommandTransaction ExistTransaction(Guid session) { return _commands.TryGetValue(session, out var transaction) ? transaction : null; }
        protected bool StartTransaction(Guid session, CommandTransaction transaction) { return _commands.TryAdd(session, transaction); }
        protected bool CloseTransaction(Guid session) { return _commands.TryRemove(session, out var transaction); }
        protected abstract bool IsTransactionCompleted(CommandTransaction transaction);
        protected BaseCommander(long periodMilliseconds) { _periodMilliseconds = periodMilliseconds; }

        public virtual CommandResponse Execute(CommandRequest request)
        {
            _thread = _thread ?? f.sys.threadof(Working, _periodMilliseconds);
            return new CommandResponse { Session = request.Session, Result = null, Report = null };
        }
    }
}

namespace Jetproger.Tools.Convert.AppConfig
{
    public class ClientCommanderPeriodSeconds : ConfigSetting { public ClientCommanderPeriodSeconds() : base("5") { } }
    public class ServerCommanderPeriodSeconds : ConfigSetting { public ServerCommanderPeriodSeconds() : base("5") { } }
}