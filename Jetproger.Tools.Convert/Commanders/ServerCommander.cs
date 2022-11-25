using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using ePlus.Kiz.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Commanders
{
    public class ServerCommander : BaseCommander
    {
        private readonly ConcurrentDictionary<Guid, CommandTransaction> _commands = new ConcurrentDictionary<Guid, CommandTransaction>();
        private readonly List<Guid> _deleteds = new List<Guid>();

        public ServerCommander() : base(1000 * k<ServerCommandPeriodSeconds>.key.As<int>())
        {
        }

        public static CommandResponse Run(ICommand command, CommandRequest request)
        {
            return BaseCommander<ServerCommander>.Instance.BeginExecute(command, request);
        }

        public override CommandResponse BeginExecute(ICommand command, CommandRequest request)
        {
            CommandTransaction existsTransaction;
            if (_commands.TryGetValue(request.Session, out existsTransaction))
            {
                var session = request.Session;
                var report = existsTransaction.Command.LogExecute();
                var result = (string)null;
                if (existsTransaction.Command.State == ECommandState.Completed)
                {
                    _commands.TryRemove(session, out existsTransaction);
                    result = (existsTransaction.Command.Result != null ? f.xml.of(existsTransaction.Command.Result) : string.Empty) ?? string.Empty;
                }
                return new CommandResponse { Session = session, Result = result, Report = report };
            }
            if (string.IsNullOrWhiteSpace(request.Command))
            {
                var session = request.Session;
                var result = string.Empty;
                var report = (new CommandMessage(new CommandNotDefinedException(session))).Messages;
                return new CommandResponse { Session = session, Result = result, Report = report };
            }
            var transaction = new CommandTransaction(command, request);
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
            PrepareDeleteds();
            foreach (CommandTransaction transaction in _commands.Values)
            {
                if (transaction.Command.State == ECommandState.None)
                {
                    transaction.Command.Value = transaction.Request.DeserializeParameter();
                    transaction.Command.Execute();
                }
                if (transaction.IsExpiration())
                {
                    _deleteds.Add(transaction.Request.Session);
                }
            }
            RemoveDeleteds();
        }

        private void PrepareDeleteds()
        {
            _deleteds.Clear();
        }

        private void RemoveDeleteds()
        {
            foreach (Guid deleted in _deleteds)
            {
                CommandTransaction transaction;
                _commands.TryRemove(deleted, out transaction);
            }
        }
    }
}
namespace ePlus.Kiz.AppConfig
{
    public class ServerCommandPeriodSeconds : ConfigSetting {public ServerCommandPeriodSeconds() : base("5") { } }
}