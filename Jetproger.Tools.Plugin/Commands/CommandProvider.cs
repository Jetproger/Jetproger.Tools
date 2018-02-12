using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.Practices.Unity.Utility;
using TDI = Tools.DI;

namespace Jetproger.Tools.Plugin.Commands
{
    public class CommandProvider<T> : CommandProvider
    {
        public CommandProvider(CommandAgent agent) : base(agent)
        {
        }

        public CommandProvider<T> Set<TSource>(Expression<Func<T, object>> target, Expression<Func<TSource, object>> source)
        {
            Agent.Map(target, source);
            return this;
        }

        public CommandProvider<T> Set(Expression<Func<T, object>> target, object value)
        {
            Agent.Map(target, value);
            return this;
        }

        public CommandProvider<T> Set(Expression<Func<T, object>> target)
        {
            Agent.Map(target);
            return this;
        }

        public override CommandProvider Parent()
        {
            Agent?.Parent?.Run();
            return Agent?.Parent != null ? new CommandProvider(Agent.Parent) : this;
        }

        public override CommandProvider<T1> Prepare<T1>()
        {
            Agent?.Parent?.Run();
            return base.Prepare<T1>();
        }

        public override CommandProvider<T1> Prepare<T1>(T1 command)
        {
            Agent?.Parent?.Run();
            return base.Prepare(command);
        }

        public CommandProvider<T> Operation()
        {
            Agent?.Parent?.Run();
            return this;
        }

        public override CommandProvider Operation(Action<CommandAgent> procedure)
        {
            Agent?.Parent?.Run();
            return base.Operation(procedure);
        }

        public override CommandProvider Operation(Command command)
        {
            Agent?.Parent?.Run();
            return base.Operation(command);
        }

        //public override CommandProvider Operation(Task task)
        //{
        //    Agent?.Parent?.Run();
        //    return base.Operation(task);
        //}
        //
        //public override CommandProvider Operation(Tree tree)
        //{
        //    Agent?.Parent?.Run();
        //    return base.Operation(tree);
        //}
        //
        //public override CommandProvider Operation(TreeItem treeItem)
        //{
        //    Agent?.Parent?.Run();
        //    return base.Operation(treeItem);
        //}
        //
        //public override CommandProvider Operation(IList<TreeItem> treeItems)
        //{
        //    Agent?.Parent?.Run();
        //    return base.Operation(treeItems);
        //}

        public override CommandProvider Operation(CommandStrategy strategy)
        {
            Agent?.Parent?.Run();
            return base.Operation(strategy);
        }

        public override CommandProvider Operation(CommandAgent agent)
        {
            Agent?.Parent?.Run();
            return base.Operation(agent);
        }

        public override CommandProvider Operation(IList<CommandAgent> agents)
        {
            Agent?.Parent?.Run();
            return base.Operation(agents);
        }

        public override T1 Result<T1>()
        {
            Agent?.Parent?.Run();
            return base.Result<T1>();
        }
    }

    public class CommandProvider : MarshalByRefObject
    {
        protected readonly CommandAgent Agent;

        public CommandProvider(CommandAgent agent)
        {
            Guard.ArgumentNotNull(agent, "agent");
            Agent = agent;
        }

        public virtual CommandProvider Parent()
        {
            Agent?.Run();
            return Agent?.Parent != null ? new CommandProvider(Agent.Parent) : this;
        }

        public virtual CommandProvider<T> Prepare<T>() where T : Command
        {
            var command = TDI.Resolve<T>();
            var agent = new CommandAgent(command);
            agent.Parent = Agent;
            Agent?.Items.Add(agent);
            return new CommandProvider<T>(agent);
        }

        public virtual CommandProvider<T> Prepare<T>(T command) where T : Command
        {
            Guard.ArgumentNotNull(command, "command");
            var agent = new CommandAgent(command);
            agent.Parent = Agent;
            Agent?.Items.Add(agent);
            return new CommandProvider<T>(agent);
        }

        public virtual CommandProvider Operation(Action<CommandAgent> procedure)
        {
            Agent?.Run();
            if (procedure == null) return this;
            var command = new CommandDirect(procedure);
            var agent = new CommandAgent(command);
            command.SetAgent(agent);
            agent.Parent = Agent;
            agent.Run();
            Agent?.Items.Add(agent);
            return new CommandProvider(agent);
        }

        public virtual CommandProvider Operation(Command command)
        {
            Agent?.Run();
            Guard.ArgumentNotNull(command, "command");
            var agent = new CommandAgent(command);
            agent.Parent = Agent;
            Agent?.Items.Add(agent);
            return new CommandProvider(agent);
        }

        //public virtual CommandProvider Operation(Task task)
        //{
        //    Agent?.Run();
        //    if (task == null) return this;
        //    var agent = task.AsCommandAgent();
        //    agent.Parent = Agent;
        //    agent.Run();
        //    Agent?.Items.Add(agent);
        //    return new CommandProvider(agent);
        //}
        //
        //public virtual CommandProvider Operation(Tree tree)
        //{
        //    return tree?.Items != null && tree.Items.Count > 0 ? Operation(tree.AsCommandAgents()) : this;
        //}
        //
        //public virtual CommandProvider Operation(TreeItem treeItem)
        //{
        //    return treeItem != null ? Operation((new[] {treeItem}).AsCommandAgents()) : this;
        //}
        //
        //public virtual CommandProvider Operation(IList<TreeItem> treeItems)
        //{
        //    return treeItems != null && treeItems.Count > 0 ? Operation(treeItems.AsCommandAgents()) : this;
        //}

        public virtual CommandProvider Operation(CommandStrategy strategy)
        {
            return strategy?.Items != null && strategy.Items.Count > 0 ? Operation(strategy.Items) : this;
        }

        public virtual CommandProvider Operation(CommandAgent agent)
        {
            return agent != null ? Operation(new[] {agent}) : this;
        }

        public virtual CommandProvider Operation(IList<CommandAgent> agents)
        {
            Agent?.Run();
            if (agents == null || agents.Count == 0)
            {
                return this;
            }
            var commandAgent = (CommandAgent)null;
            foreach (var agent in AllAgents(agents))
            {
                commandAgent = agent;
                commandAgent.Run();
            }
            if (Agent != null)
            {
                foreach (var agent in agents)
                {
                    agent.Parent = Agent;
                    Agent.Items.Add(agent);
                }
            }
            return new CommandProvider(commandAgent);
        }

        public virtual T Result<T>()
        {
            Agent.Run();
            var agent = Agent;
            while (true)
            {
                if (agent.Parent == null) break;
                agent = agent.Parent;
            }
            while (true)
            {
                if (AllAgents(agent.Items).All(x => x.State != CommandState.New && x.State != CommandState.Running)) break;
                Thread.Sleep(333);
            }
            return (T)(object)Agent.Command;
        }

        private IEnumerable<CommandAgent> AllAgents(IEnumerable<CommandAgent> agents)
        {
            foreach (var agent in (agents ?? new CommandAgent[0]))
            {
                yield return agent;
                foreach (var item in AllAgents(agent.Items))
                {
                    yield return item;
                }
            }
        }
    }
}