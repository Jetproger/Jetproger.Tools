using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ePlus.Kiz.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Commands
{
    public class ServerCommander : BaseCommander
    {
        private readonly ConcurrentDictionary<Guid, CommandTransaction> _commands = new ConcurrentDictionary<Guid, CommandTransaction>();
        private readonly List<Guid> _deleteds = new List<Guid>();
        public ServerCommander() : base(1000 * J_<ServerCommandPeriodSeconds>.Sz.As<int>()) { }

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
                var report = existsTransaction.Command.GetLog();
                var result = (string)null;
                if (existsTransaction.Command.State == ECommandState.Completed)
                {
                    Je.log.UnregisterTracer(command as TraceListener);
                    _commands.TryRemove(session, out existsTransaction);
                    result = (existsTransaction.Command.Result != null ? Je.xml.Of(existsTransaction.Command.Result) : string.Empty) ?? string.Empty;
                }
                return new CommandResponse { Session = session, Result = result, Report = report };
            }
            if (string.IsNullOrWhiteSpace(request.Command))
            {
                var session = request.Session;
                var result = string.Empty;
                var report = Je.cmd.MessagesOf(Je.err.ErrToMsg(new CommandNotDefinedException(session)));
                return new CommandResponse { Session = session, Result = result, Report = report };
            } 
            var transaction = new CommandTransaction(command, request);
            if (_commands.TryAdd(request.Session, transaction))
            {
                Je.log.RegisterTracer(command as TraceListener);
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
    public class ServerCommandPeriodSeconds : ConfigSetting { public ServerCommandPeriodSeconds() : base("5") { } }
}