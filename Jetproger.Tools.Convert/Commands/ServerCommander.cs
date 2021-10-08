using System;
using System.Collections.Concurrent;
using System.Threading;
using Jetproger.AppConfig;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Commands
{
    public class ServerCommander : BaseCommander
    {
        private readonly ConcurrentDictionary<Guid, _CommandTransaction> _commands = new ConcurrentDictionary<Guid, _CommandTransaction>();
        public ServerCommander() : base(1000 * Je<ServerCommandPeriodSeconds>.As.As<int>()) { }

        public override CommandResponse BeginExecute(ICommand command, CommandRequest request)
        {
            _CommandTransaction existsTransaction;
            if (_commands.TryGetValue(request.Session, out existsTransaction))
            {
                var session = request.Session;
                var result = existsTransaction.Transaction.Command.Result;
                var report = existsTransaction.Transaction.Command.ReadReport();
                if (result != null) _commands.TryRemove(session, out existsTransaction);
                return new CommandResponse { Session = session, Result = result, Report = report };
            }
            if (string.IsNullOrWhiteSpace(request.Command))
            {
                var session = request.Session;
                var result = string.Empty;
                var report = Je.cmd.Report(Je.cmd.Error(string.Format("Сессия [{0}]: не определена команда для выполнения(CommandRequest.Command)", session)));
                return new CommandResponse { Session = session, Result = result, Report = report };
            }
            ICommandAsync commandAsync;
            var response = Execute(command, request, out commandAsync);
            if (response != null) return response;
            var transaction = new _CommandTransaction(commandAsync, request);
            if (_commands.TryAdd(request.Session, transaction))
            {
                base.BeginExecute(command, request);
                Thread.Sleep(1111);
                return BeginExecute(command, request);
            }
            return base.BeginExecute(command, request);
        }

        protected override void Action()
        {
            foreach (_CommandTransaction transaction in _commands.Values)
            {
                if (transaction.IsStart) continue;
                transaction.IsStart = true;
                transaction.Transaction.Command.BeginExecute(null, transaction.Transaction.Request.Document);
            }
        }

        private class _CommandTransaction
        {
            public readonly CommandTransaction Transaction;
            public bool IsStart;
            public _CommandTransaction(ICommandAsync command, CommandRequest request)
            {
                Transaction = new CommandTransaction(command, request);
                IsStart = false;
            }
        }
    }
}
namespace Jetproger.AppConfig
{
    public class ServerCommandPeriodSeconds : ConfigSetting<int> { public ServerCommandPeriodSeconds() : base(5) { } }
}