using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ePlus.Kiz.AppConfig;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Convert.Commands
{
    public class ClientCommander : BaseCommander
    {
        private readonly ConcurrentDictionary<Guid, CommandTransaction> _commands = new ConcurrentDictionary<Guid, CommandTransaction>();
        private readonly List<Guid> _deleteds = new List<Guid>();

        public ClientCommander() : base(1000 * Je<GTINClientCommandPeriodSeconds>.As.As<int>()) { }

        public override CommandResponse BeginExecute(ICommand command, CommandRequest request)
        {
            ICommandAsync commandAsync;
            var response = Execute(command, request, out commandAsync);
            if (response != null) return response;
            var transaction = new CommandTransaction(commandAsync, request);
            _commands.TryAdd(request.Session, transaction);
            return base.BeginExecute(command, request);
        }

        protected override void Action()
        {
            _deleteds.Clear();
            foreach (CommandTransaction transaction in _commands.Values) ExecuteCommand(transaction);
            RemoveDeleteds();
        }

        private void ExecuteCommand(CommandTransaction transaction)
        {
            try
            {
                if (transaction.Command.Result != null)
                {
                    _deleteds.Add(transaction.Request.Session);
                    transaction.Command.EndExecute();
                    return;
                }
                if (!transaction.Command.IsRunning)
                {
                    transaction.Command.Execute(transaction.Request.Document);
                    return;
                }
            }
            catch (Exception e)
            {
                _deleteds.Add(transaction.Request.Session);
                Je.cmd.Log(Je.cmd.Error(e));
            }
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
    public class GTINClientCommandPeriodSeconds : ConfigSetting<int> { public GTINClientCommandPeriodSeconds() : base(5) { } }
}