using System;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commands;

namespace Jetproger.Tools.Convert.Traces
{ 
    public class TraceJob : InfinityJob
    {
        private static readonly TraceContext Context = t<TraceContext>.one();
        private static readonly TraceJob[] InstanceHolder = { null };

        private TraceJob() : base(k<JetprogerJobCommandPeriodSeconds>.key<int>() * 1000, 1000, 5000)
        {
            OnExecute += new _TraceCommand();
        } 

        public static TraceJob Instance
        {
            get
            {
                if (InstanceHolder[0] == null)
                {
                    lock (InstanceHolder)
                    {
                        if (InstanceHolder[0] == null)
                        {
                            InstanceHolder[0] = new TraceJob();
                            InstanceHolder[0].Execute();
                        }
                    }
                }
                return InstanceHolder[0];
            }
        }

        private class _TraceCommand : BaseCommand, ICommand
        {
            public void Execute()
            {
                try
                {
                    if (State != ECommandState.None) return;
                    State = ECommandState.Running;
                    Context.Write();
                    Result = this;
                }
                catch (Exception e)
                {
                    Error = e;
                }
            }
        }
    }
}