using System.Collections.Generic;
using System.Linq;

namespace Jetproger.Tools.Plugin.Commands
{
    public class CommandStrategy : System.MarshalByRefObject
    {
        public List<CommandAgent> Items { get; set; }

        public bool IsRunning
        {
            get { return AllAgents().Any(item => item.State == CommandState.Running); }
        }

        public void Start()
        {
            if (IsRunning) return;
            //Reset();
            foreach (var item in Items) item.Run();
        }

        public void Stop()
        {
            foreach (var item in AllAgents())
            {
                item.Cancel();
            }
        }

        public void Reset()
        {
            foreach (var item in AllAgents())
            {
                item.Reset();
            }
        }

        public CommandAgent GetLastAgent()
        {
            var agent = (CommandAgent)null;
            foreach (var item in AllAgents())
            {
                agent = item;
            }
            return agent;
        }

        public IEnumerable<CommandAgent> AllAgents()
        {
            return AllAgents(Items);
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