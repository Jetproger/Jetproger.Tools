using System; 
using System.Collections.Generic;
using Jetproger.Tools.Convert.Bases; 
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Commanders
{ 
    public class CmdExpander
    {
        public ClientCommander ClientCommander { get { return f.one.of(ClientCommanderHolder, () => new ClientCommander()); } }
        private readonly ClientCommander[] ClientCommanderHolder = { null };
        public ServerCommander ServerCommander { get { return f.one.of(ServerCommanderHolder, () => new ServerCommander()); } }
        private readonly ServerCommander[] ServerCommanderHolder = { null };

        public CommandResponse ClientEnqueue(CommandRequest request)
        {
            return ClientCommander.Execute(request);
        }

        public CommandResponse ServerEnqueue(CommandRequest request)
        {
            return ServerCommander.Execute(request);
        }

        public AsyncCommand<TResult, TValue> asyncof<TResult, TValue>(Command<TResult, TValue> command)
        {
            return new AsyncCommand<TResult, TValue>(command);
        }

        public ICommand prelastof(ICommand command)
        {
            return command.Precommand;
        }

        public ICommand prefirstof(ICommand command)
        {
            var precommand = command.Precommand;
            while (true)
            {
                if (precommand == null) break;
                if (precommand.Precommand == null) return precommand;
                precommand = precommand.Precommand;
            }
            return null;
        }

        public ICommand preindexof(ICommand commnad, int i)
        {
            var precommand = commnad.Precommand;
            var precommands = new List<ICommand>();
            while (true)
            {
                if (precommand == null) break;
                precommands.Add(precommand);
                precommand = precommand.Precommand;
            }
            i = i >= 0 ? i : precommands.Count + i;
            return i >= 0 && i < precommands.Count ? precommands[i] : null;
        }

        public T precommandof<T>(ICommand commnad) where T : class, ICommand
        {
            var precommand = commnad.Precommand;
            while (true)
            {
                if (precommand == null) break;
                var command = precommand as T;
                if (command != null) return command;
                precommand = precommand.Precommand;
            }
            return null;
        }

        public ICommand prevalueof<T>(ICommand command)
        {
            var precommand = command.Precommand;
            while (true)
            {
                if (precommand == null) break;
                if (precommand.Value is T) return precommand;
                precommand = precommand.Precommand;
            }
            return null;
        }

        public ICommand preresultof<T>(ICommand command)
        {
            var precommand = command.Precommand;
            while (true)
            {
                if (precommand == null) break;
                if (precommand.Result is T) return precommand;
                precommand = precommand.Precommand;
            }
            return null;
        }
    }
}