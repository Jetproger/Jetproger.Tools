using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Jetproger.Tools.Convert.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Commanders
{
    public class ClientCommander : BaseCommander
    {
        private readonly ConcurrentDictionary<Guid, CommandTransaction> _commands = new ConcurrentDictionary<Guid, CommandTransaction>();
        private readonly List<Guid> _deleteds = new List<Guid>();

        public ClientCommander() : base(1000 * J_<ClientCommandPeriodSeconds>.Sz.As<int>())
        {
        }

        public static CommandResponse Run(ICommand command, CommandRequest request)
        {
            return BaseCommander<ClientCommander>.Instance.BeginExecute(command, request);
        }

        public override CommandResponse BeginExecute(ICommand command, CommandRequest request)
        {
            var transaction = new CommandTransaction(command, request);
            _commands.TryAdd(request.Session, transaction);
            return base.BeginExecute(command, request);
        }

        protected override void Action()
        {
            PrepareDeleteds();
            foreach (CommandTransaction transaction in _commands.Values)
            {
                ExecuteCommand(transaction);
            }
            RemoveDeleteds();
        }

        private void ExecuteCommand(CommandTransaction transaction)
        {
            try
            {
                if (transaction.Command.State == ECommandState.Completed)
                {
                    _deleteds.Add(transaction.Request.Session);
                    return;
                }
                if (transaction.Command.State == ECommandState.None)
                {
                    transaction.Command.Value = transaction.Request.DeserializeParameter();
                    transaction.Command.Execute();
                    return;
                }
            }
            catch (Exception e)
            {
                _deleteds.Add(transaction.Request.Session);
                Je.log.To(e);
            }
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

namespace Jetproger.Tools.Convert.AppConfig
{
    public class ClientCommandPeriodSeconds : ConfigSetting { public ClientCommandPeriodSeconds() : base("5") { } }
}